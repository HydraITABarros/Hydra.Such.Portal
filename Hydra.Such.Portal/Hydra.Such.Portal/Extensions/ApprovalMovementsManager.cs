using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Approvals;
using Hydra.Such.Data.ViewModel.Approvals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Such.Portal.Extensions
{
    public class ApprovalMovementsManager
    {
        public static bool StartApprovalMovement(int type, int area, decimal value, string number, string requestUser)
        {
            try
            {
                //Get Compatible ApprovalConfigurations
                List<ConfiguraçãoAprovações> ApprovalConfigurations = DBApprovalConfigurations.GetByTypeAreaValue(type, area, value);
                int lowLevel = ApprovalConfigurations.Where(x => x.NívelAprovação.HasValue).OrderBy(x => x.NívelAprovação.Value).Select(x => x.NívelAprovação.Value).FirstOrDefault();
                ApprovalConfigurations.RemoveAll(x => x.NívelAprovação != lowLevel);

                if (ApprovalConfigurations != null && ApprovalConfigurations.Count > 0)
                {
                    //Create ApprovalMovement
                    ApprovalMovementsViewModel ApprovalMovement = new ApprovalMovementsViewModel()
                    {
                        Type = type,
                        Area = area,
                        Number = number,
                        RequestUser = requestUser,
                        Value = value,
                        DateTimeCreate = DateTime.Now,
                        UserCreate = requestUser,
                        Status = 1,
                        Level = lowLevel
                    };

                    ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.Create(DBApprovalMovements.ParseToDatabase(ApprovalMovement)));

                    //Create User ApprovalMovements
                    List<string> UsersToNotify = new List<string>();
                    ApprovalConfigurations.ForEach(x =>
                    {
                        if (x.UtilizadorAprovação != "" && x.UtilizadorAprovação != null)
                        {
                            DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = x.UtilizadorAprovação });
                            UsersToNotify.Add(x.UtilizadorAprovação);
                        }
                        else if (x.GrupoAprovação.HasValue)
                        {
                            List<string> GUsers = DBApprovalUserGroup.GetAllFromGroup(x.GrupoAprovação.Value);

                            GUsers.ForEach(y =>
                            {
                                DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = y });
                                UsersToNotify.Add(y);
                            });
                        }
                    });

                    UsersToNotify = UsersToNotify.Distinct().ToList();
                    //Notify Users
                    UsersToNotify.ForEach(e =>
                    {
                        EmailsAprovações EmailApproval = new EmailsAprovações()
                        {
                            NºMovimento = ApprovalMovement.MovementNo,
                            EmailDestinatário = e,
                            NomeDestinatário = e,
                            Assunto = "Plataforma Such - Aprovação Pendente",
                            DataHoraEmail = DateTime.Now,
                            TextoEmail = "Existe uma nova tarefa pendente da sua aprovação na Plataforma!",
                            Enviado = false
                        };


                        SendEmailApprovals Email = new SendEmailApprovals
                        {
                            Subject = "Plataforma Such - Aprovação Pendente",
                            From = "plataforma@such.pt"
                        };

                        Email.To.Add("abarros@hydra.pt");

                        Email.Body = MakeEmailBodyContent("Existe uma nova tarefa pendente da sua aprovação na Plataforma!");

                        Email.IsBodyHtml = true;
                        Email.EmailApproval = EmailApproval;

                        Email.SendEmail();
                    });
                }
                else
                {
                    //?????????????????????????????????????
                    //Não existem configurações compativeis
                    //?????????????????????????????????????
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool ApproveMovement(int movementNo, string ApproveUser)
        {
            try
            {
                //Update Old Movement
                ApprovalMovementsViewModel ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetById(movementNo));
                ApprovalMovement.Status = 2;
                ApprovalMovement.DateTimeApprove = DateTime.Now;
                ApprovalMovement.DateTimeUpdate = DateTime.Now;
                ApprovalMovement.UserUpdate = ApproveUser;
                ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.Update(DBApprovalMovements.ParseToDatabase(ApprovalMovement)));

                //Delete All User Approval Movements
                DBUserApprovalMovements.DeleteFromMovementExcept(ApprovalMovement.MovementNo, ApproveUser);

                //Get Next Level Configuration
                List<ConfiguraçãoAprovações> ApprovalConfigurations = DBApprovalConfigurations.GetByTypeAreaValue(ApprovalMovement.Type.Value, ApprovalMovement.Area.Value, ApprovalMovement.Value.Value);
                ApprovalConfigurations.RemoveAll(x => !x.NívelAprovação.HasValue || x.NívelAprovação <= ApprovalMovement.Level);

                if (ApprovalConfigurations.Count > 0)
                {
                    int lowLevel = ApprovalConfigurations.Where(x => x.NívelAprovação.HasValue).OrderBy(x => x.NívelAprovação.Value).Select(x => x.NívelAprovação.Value).FirstOrDefault();
                    ApprovalConfigurations.RemoveAll(x => x.NívelAprovação != lowLevel);

                    //Create New Approval Movement
                    ApprovalMovement.Level = lowLevel;
                    ApprovalMovement.DateTimeUpdate = null;
                    ApprovalMovement.UserUpdate = null;
                    ApprovalMovement.DateTimeCreate = DateTime.Now;
                    ApprovalMovement.UserCreate = ApproveUser;
                    ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.Create(DBApprovalMovements.ParseToDatabase(ApprovalMovement)));

                    //Create User ApprovalMovements
                    List<string> UsersToNotify = new List<string>();
                    ApprovalConfigurations.ForEach(x =>
                    {
                        if (x.UtilizadorAprovação != "" && x.UtilizadorAprovação != null)
                        {
                            DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = x.UtilizadorAprovação });
                            UsersToNotify.Add(x.UtilizadorAprovação);
                        }
                        else if (x.GrupoAprovação.HasValue)
                        {
                            List<string> GUsers = DBApprovalUserGroup.GetAllFromGroup(x.GrupoAprovação.Value);

                            GUsers.ForEach(y =>
                            {
                                DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = y });
                                UsersToNotify.Add(y);
                            });
                        }
                    });

                    //Notify Users
                    UsersToNotify = UsersToNotify.Distinct().ToList();
                    UsersToNotify.ForEach(e =>
                    {
                        EmailsAprovações EmailApproval = new EmailsAprovações()
                        {
                            NºMovimento = ApprovalMovement.MovementNo,
                            EmailDestinatário = e,
                            NomeDestinatário = e,
                            Assunto = "Plataforma Such - Aprovação Pendente",
                            DataHoraEmail = DateTime.Now,
                            TextoEmail = "Existe uma nova tarefa pendente da sua aprovação na Plataforma!",
                            Enviado = false
                        };


                        SendEmailApprovals Email = new SendEmailApprovals
                        {
                            Subject = "Plataforma Such - Aprovação Pendente",
                            From = "plataforma@such.pt"
                        };

                        Email.To.Add(e);

                        Email.Body = MakeEmailBodyContent("Existe uma nova tarefa pendente da sua aprovação na Plataforma!");

                        Email.IsBodyHtml = true;
                        Email.EmailApproval = EmailApproval;

                        Email.SendEmail();
                    });
                }
                else
                {
                    EmailsAprovações EmailApproval = new EmailsAprovações()
                    {
                        NºMovimento = ApprovalMovement.MovementNo,
                        EmailDestinatário = ApprovalMovement.RequestUser,
                        NomeDestinatário = ApprovalMovement.RequestUser,
                        Assunto = "Plataforma Such - Tarefa aprovada",
                        DataHoraEmail = DateTime.Now,
                        TextoEmail = "A sua tarefa com o Nº "+ ApprovalMovement .Number+ " foi aprovada com sucesso!",
                        Enviado = false
                    };


                    SendEmailApprovals Email = new SendEmailApprovals
                    {
                        Subject = "Plataforma Such - Tarefa aprovada",
                        From = "plataforma@such.pt"
                    };

                    Email.To.Add(ApprovalMovement.RequestUser);

                    Email.Body = MakeEmailBodyContent("A sua tarefa com o Nº " + ApprovalMovement.Number + " foi aprovada com sucesso!");

                    Email.IsBodyHtml = true;
                    Email.EmailApproval = EmailApproval;

                    Email.SendEmail();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool RejectMovement(int movementNo, string rejectUser, string rejectReason)
        {
            try
            {
                //Update Old Movement
                ApprovalMovementsViewModel ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetById(movementNo));
                ApprovalMovement.Status = 3;
                ApprovalMovement.DateTimeApprove = DateTime.Now;
                ApprovalMovement.DateTimeUpdate = DateTime.Now;
                ApprovalMovement.UserUpdate = rejectUser;
                ApprovalMovement.ReproveReason = rejectReason;
                ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.Update(DBApprovalMovements.ParseToDatabase(ApprovalMovement)));

                //Delete All User Approval Movements
                DBUserApprovalMovements.DeleteFromMovementExcept(ApprovalMovement.MovementNo, rejectUser);


                EmailsAprovações EmailApproval = new EmailsAprovações()
                {
                    NºMovimento = ApprovalMovement.MovementNo,
                    EmailDestinatário = ApprovalMovement.RequestUser,
                    NomeDestinatário = ApprovalMovement.RequestUser,
                    Assunto = "Plataforma Such - Tarefa rejeitada",
                    DataHoraEmail = DateTime.Now,
                    TextoEmail = "A sua tarefa com o Nº " + ApprovalMovement.Number + " foi rejeitada pelo seguinte motivo \""+ ApprovalMovement.ReproveReason + "\"!",
                    Enviado = false
                };


                SendEmailApprovals Email = new SendEmailApprovals
                {
                    Subject = "Plataforma Such - Tarefa rejeitada",
                    From = "plataforma@such.pt"
                };

                Email.To.Add(ApprovalMovement.RequestUser);

                Email.Body = MakeEmailBodyContent("A sua tarefa com o Nº " + ApprovalMovement.Number + " foi rejeitada pelo seguinte motivo \"" + ApprovalMovement.ReproveReason + "\"!");

                Email.IsBodyHtml = true;
                Email.EmailApproval = EmailApproval;

                Email.SendEmail();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string MakeEmailBodyContent(string BodyText)
        {
            string Body = @"<html>" +
                                "<head>" +
                                    "<style>" +
                                        "table{border:0;} " +
                                        "td{width:600px; vertical-align: top;}" +
                                    "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<table>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Caro (a)," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                BodyText +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Com os melhores cumprimentos," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<i>SUCH - Serviço de Utilização Comum dos Hospitais</i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";

            return Body;
        }

    }
}
