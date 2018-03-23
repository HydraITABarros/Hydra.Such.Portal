using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Approvals;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Approvals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Such.Portal.Extensions
{
    public class ApprovalMovementsManager
    {
        //100 - Fluxo Iniciado com suceso
        //101 - Não existem configurações de numerações compativeis
        //102 - Erro desconhecido
        public static ErrorHandler StartApprovalMovement(int type, int area, string functionalArea, string responsabilityCenter, string region, decimal value, string number, string requestUser)
        {
            try
            {
                ErrorHandler result = new ErrorHandler() {
                    eReasonCode = 100,
                    eMessage = "Fluxo Iniciado com sucesso"
                };

                //Get Compatible ApprovalConfigurations
                List<ConfiguraçãoAprovações> ApprovalConfigurations = DBApprovalConfigurations.GetByTypeAreaValueDateAndDimensions(type, area,functionalArea,responsabilityCenter,region, value, DateTime.Now);
                int lowLevel = ApprovalConfigurations.Where(x => x.NívelAprovação.HasValue).OrderBy(x => x.NívelAprovação.Value).Select(x => x.NívelAprovação.Value).FirstOrDefault();
                ApprovalConfigurations.RemoveAll(x => x.NívelAprovação != lowLevel);

                if (ApprovalConfigurations != null && ApprovalConfigurations.Count > 0)
                {
                    //Create ApprovalMovement
                    ApprovalMovementsViewModel ApprovalMovement = new ApprovalMovementsViewModel()
                    {
                        Type = type,
                        Area = area,
                        ResponsabilityCenter = responsabilityCenter,
                        FunctionalArea = functionalArea,
                        Region = region,
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

                        Email.To.Add(e);

                        Email.Body = MakeEmailBodyContent("Existe uma nova tarefa pendente da sua aprovação na Plataforma!");

                        Email.IsBodyHtml = true;
                        Email.EmailApproval = EmailApproval;

                        Email.SendEmail();
                    });
                }
                else
                {
                    result.eReasonCode = 101;
                    result.eMessage = "Não existem configurações de numerações compativeis.";
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorHandler()
                {
                    eReasonCode = 102,
                    eMessage = "Ocorreu um erro desconhecido."
                };
            }
        }

        // 100 - Tarefa aprovada com sucesso
        // 101 - Erro desconhecido
        // 103 - Aprovada pelo ultimo aprovador
        public static ErrorHandler ApproveMovement(int movementNo, string ApproveUser)
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
                List<ConfiguraçãoAprovações> ApprovalConfigurations = DBApprovalConfigurations.GetByTypeAreaValueDateAndDimensions(ApprovalMovement.Type.Value, ApprovalMovement.Area.Value, ApprovalMovement.FunctionalArea, ApprovalMovement.ResponsabilityCenter, ApprovalMovement.Region, ApprovalMovement.Value.Value, DateTime.Now);
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
                    ApprovalMovement.Status = 1;
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
                    return new ErrorHandler()
                    {
                        eReasonCode = 100,
                        eMessage = "A tarefa foi aprovada com sucesso."
                    };
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

                    return new ErrorHandler()
                    {
                        eReasonCode = 103,
                        eMessage = "A tarefa foi aprovada pelo ultimo nivel."
                    };
                }
                
            }
            catch (Exception ex)
            {
                return new ErrorHandler()
                {
                    eReasonCode = 101,
                    eMessage = "Ocorreu um erro desconhecido ao aprovar a tarefa."
                };
            }
        }


        //100 - Tarefa rejeitada com sucesso
        //101 - Erro desconhecido
        public static ErrorHandler RejectMovement(int movementNo, string rejectUser, string rejectReason)
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
                return new ErrorHandler() {
                    eReasonCode = 100,
                    eMessage = "Tarefa rejeitada com sucesso."
                };
            }
            catch (Exception ex)
            {
                return new ErrorHandler()
                {
                    eReasonCode = 101,
                    eMessage = "Ocorreu um erro desconhecido."
                };
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
