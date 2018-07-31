using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Approvals;
using Hydra.Such.Data.Logic.FolhaDeHora;
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
        public static ErrorHandler StartApprovalMovement(int type, string functionalArea, string responsabilityCenter, string region, decimal value, string number, string requestUser, string reason)
        {
            try
            {
                ErrorHandler result = new ErrorHandler()
                {
                    eReasonCode = 100,
                    eMessage = "Fluxo Iniciado com sucesso",
                    eMessages = new List<TraceInformation>()
                };

                //Get Compatible ApprovalConfigurations
                List<ConfiguraçãoAprovações> ApprovalConfigurations = DBApprovalConfigurations.GetByTypeAreaValueDateAndDimensions(type, functionalArea, responsabilityCenter, region, value, DateTime.Now);

                int lowLevel = ApprovalConfigurations.Where(x => x.NívelAprovação.HasValue).OrderBy(x => x.NívelAprovação.Value).Select(x => x.NívelAprovação.Value).FirstOrDefault();
                ApprovalConfigurations.RemoveAll(x => x.NívelAprovação != lowLevel);

                if (ApprovalConfigurations != null && ApprovalConfigurations.Count > 0)
                {
                    //Create ApprovalMovement
                    ApprovalMovementsViewModel ApprovalMovement = new ApprovalMovementsViewModel()
                    {
                        Type = type,
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

                    result.eMessages.Add(new TraceInformation(TraceType.Error, ApprovalMovement.MovementNo.ToString()));

                    //Create User ApprovalMovements
                    List<ConfigUtilizadores> UsersToNotify = new List<ConfigUtilizadores>();
                    //ApprovalConfigurations.ForEach(x =>
                    //{
                    var approvalConfiguration = ApprovalConfigurations[0];
                    if (approvalConfiguration.UtilizadorAprovação != "" && approvalConfiguration.UtilizadorAprovação != null)
                    {
                        DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = approvalConfiguration.UtilizadorAprovação });
                        ConfigUtilizadores users = DBUserConfigurations.GetById(approvalConfiguration.UtilizadorAprovação);
                        UsersToNotify.Add(users);
                    }
                    else if (approvalConfiguration.GrupoAprovação.HasValue)
                    {
                        List<string> GUsers = DBApprovalUserGroup.GetAllFromGroup(approvalConfiguration.GrupoAprovação.Value);

                        GUsers.ForEach(y =>
                        {
                            DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = y });
                            ConfigUtilizadores users = DBUserConfigurations.GetById(y);
                            UsersToNotify.Add(users);
                        });
                    }
                    //});


                    string itemToApproveInfo = string.Empty;
                    if (type == 1 && !string.IsNullOrEmpty(number))
                        itemToApproveInfo += " - Requisição " + number;

                    UsersToNotify = UsersToNotify.Distinct().ToList();
                    //Notify Users
                    UsersToNotify.ForEach(e =>
                    {
                        if(e.RfmailEnvio != "" && e.RfmailEnvio != null)
                        {
                            EmailsAprovações EmailApproval = new EmailsAprovações();

                            EmailApproval.NºMovimento = ApprovalMovement.MovementNo;
                            EmailApproval.EmailDestinatário = e.RfmailEnvio;
                            EmailApproval.NomeDestinatário = e.Nome;
                            EmailApproval.Assunto = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Aprovação Pendente" : "eSUCH - Aprovação Pendente" + itemToApproveInfo;
                            EmailApproval.DataHoraEmail = DateTime.Now;
                            if(reason != "" && reason != null)
                            {
                                EmailApproval.TextoEmail = "Uma requisição aprovada (" + number + ") foi movida novamente para aprovação." + "<br />" + "<b>Motivo:</b> " + reason;
                            }
                            else
                            {
                                EmailApproval.TextoEmail = "Existe uma nova tarefa pendente da sua aprovação no eSUCH!";

                            }

                            EmailApproval.Enviado = false;


                            SendEmailApprovals Email = new SendEmailApprovals
                            {
                                Subject = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Aprovação Pendente" : "eSUCH - Aprovação Pendente" + itemToApproveInfo,
                                From = "plataforma@such.pt"
                            };

                            Email.To.Add(e.RfmailEnvio);

                            Email.Body = MakeEmailBodyContent(EmailApproval.TextoEmail);

                            Email.IsBodyHtml = true;
                            Email.EmailApproval = EmailApproval;

                            Email.SendEmail();
                        }
                        else
                        {
                            result.eReasonCode = 100;
                            result.eMessage = "Porém um ou mais aprovadores não possuem e-mail de envio.";
                        }
                    });
                }
                else
                {
                    result.eReasonCode = 101;
                    result.eMessage = "Não existe configuração de aprovação para as dimensões indicadas. Solicite esta configuração submeta para aprovação nas suas requisições pendentes.";
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
                List<ConfiguraçãoAprovações> ApprovalConfigurations = DBApprovalConfigurations.GetByTypeAreaValueDateAndDimensions(ApprovalMovement.Type.Value, ApprovalMovement.FunctionalArea, ApprovalMovement.ResponsabilityCenter, ApprovalMovement.Region, ApprovalMovement.Value.Value, DateTime.Now);
                ApprovalConfigurations.RemoveAll(x => !x.NívelAprovação.HasValue || x.NívelAprovação <= ApprovalMovement.Level);

                string itemToApproveInfo = string.Empty;
                if (ApprovalMovement != null)
                {
                    if (ApprovalMovement.Type.Value == 1 && !string.IsNullOrEmpty(ApprovalMovement.Number))
                        itemToApproveInfo += " - Requisição " + ApprovalMovement.Number;
                }
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
                    List<ConfigUtilizadores> UsersToNotify = new List<ConfigUtilizadores>();
                    //ApprovalConfigurations.ForEach(x =>
                    //{
                    var approvalConfiguration = ApprovalConfigurations[0];
                    if (approvalConfiguration.UtilizadorAprovação != "" && approvalConfiguration.UtilizadorAprovação != null)
                    {
                        DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = approvalConfiguration.UtilizadorAprovação });
                        ConfigUtilizadores users = DBUserConfigurations.GetById(approvalConfiguration.UtilizadorAprovação);
                        UsersToNotify.Add(users);
                    }
                    else if (approvalConfiguration.GrupoAprovação.HasValue)
                    {
                        List<string> GUsers = DBApprovalUserGroup.GetAllFromGroup(approvalConfiguration.GrupoAprovação.Value);

                        GUsers.ForEach(y =>
                        {
                            DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = y });
                            ConfigUtilizadores users = DBUserConfigurations.GetById(y);
                            UsersToNotify.Add(users);
                        });
                    }
                    //});

                    //Notify Users
                    UsersToNotify = UsersToNotify.Distinct().ToList();
                    UsersToNotify.ForEach(e =>
                    {
                        EmailsAprovações EmailApproval = new EmailsAprovações()
                        {
                            NºMovimento = ApprovalMovement.MovementNo,
                            EmailDestinatário = e.RfmailEnvio,
                            NomeDestinatário = e.Nome,
                            Assunto = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Aprovação Pendente" : "eSUCH - Aprovação Pendente" + itemToApproveInfo,
                            DataHoraEmail = DateTime.Now,
                            TextoEmail = "Existe uma nova tarefa pendente da sua aprovação no eSUCH!",
                            Enviado = false
                        };


                        SendEmailApprovals Email = new SendEmailApprovals
                        {
                            Subject = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Aprovação Pendente" : "eSUCH - Aprovação Pendente" + itemToApproveInfo,
                            From = "plataforma@such.pt"
                        };

                        Email.To.Add(e.RfmailEnvio);

                        Email.Body = MakeEmailBodyContent("Existe uma nova tarefa pendente da sua aprovação no eSUCH!");

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
                        Assunto = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Tarefa aprovada" : "eSUCH - Tarefa aprovada" + itemToApproveInfo,
                        DataHoraEmail = DateTime.Now,
                        TextoEmail = "A sua tarefa com o Nº " + ApprovalMovement.Number + " foi aprovada com sucesso!",
                        Enviado = false
                    };


                    SendEmailApprovals Email = new SendEmailApprovals
                    {
                        Subject = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Tarefa aprovada" : "eSUCH - Tarefa aprovada" + itemToApproveInfo,
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

        //100 - Fluxo Iniciado com suceso
        //101 - Não existem configurações de numerações compativeis
        //102 - Erro desconhecido
        public static ErrorHandler StartApprovalMovement_FH(int type, string functionalArea, string responsabilityCenter, string region, decimal value, string number, string requestUser)
        {
            try
            {
                ErrorHandler result = new ErrorHandler()
                {
                    eReasonCode = 100,
                    eMessage = "Fluxo Iniciado com sucesso",
                    eMessages = new List<TraceInformation>()
                };

                //Get Compatible ApprovalConfigurations
                List<ConfiguraçãoAprovações> ApprovalConfigurations = DBApprovalConfigurations.GetByTypeAreaValueDateAndDimensions(type, functionalArea, responsabilityCenter, region, value, DateTime.Now);

                int lowLevel = ApprovalConfigurations.Where(x => x.NívelAprovação.HasValue).OrderBy(x => x.NívelAprovação.Value).Select(x => x.NívelAprovação.Value).FirstOrDefault();
                ApprovalConfigurations.RemoveAll(x => x.NívelAprovação != lowLevel);

                if (ApprovalConfigurations != null && ApprovalConfigurations.Count > 0)
                {
                    //Create ApprovalMovement
                    ApprovalMovementsViewModel ApprovalMovement = new ApprovalMovementsViewModel()
                    {
                        Type = type,
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

                    result.eMessages.Add(new TraceInformation(TraceType.Error, ApprovalMovement.MovementNo.ToString()));

                    //Create User ApprovalMovements
                    List<ConfigUtilizadores> UsersToNotify = new List<ConfigUtilizadores>();
                    //ApprovalConfigurations.ForEach(x =>
                    //{
                    var approvalConfiguration = ApprovalConfigurations[0];
                    if (approvalConfiguration.UtilizadorAprovação != "" && approvalConfiguration.UtilizadorAprovação != null)
                    {
                        DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = approvalConfiguration.UtilizadorAprovação });
                        ConfigUtilizadores users = DBUserConfigurations.GetById(approvalConfiguration.UtilizadorAprovação);
                        UsersToNotify.Add(users);
                    }
                    else if (approvalConfiguration.GrupoAprovação.HasValue)
                    {
                        List<string> GUsers = DBApprovalUserGroup.GetAllFromGroup(approvalConfiguration.GrupoAprovação.Value);

                        GUsers.ForEach(y =>
                        {
                            DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = y });
                            ConfigUtilizadores users = DBUserConfigurations.GetById(y);
                            UsersToNotify.Add(users);
                        });
                    }
                    //});


                    string itemToApproveInfo = string.Empty;
                    if (type == 3 && !string.IsNullOrEmpty(number))
                        itemToApproveInfo += " - Folha de Horas " + number;

                    UsersToNotify = UsersToNotify.Distinct().ToList();
                    //Notify Users
                    UsersToNotify.ForEach(e =>
                    {
                        EmailsAprovações EmailApproval = new EmailsAprovações()
                        {
                            NºMovimento = ApprovalMovement.MovementNo,
                            EmailDestinatário = e.RfmailEnvio,
                            NomeDestinatário = e.Nome,
                            Assunto = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Aprovação Pendente" : "eSUCH - Aprovação Pendente" + itemToApproveInfo,
                            DataHoraEmail = DateTime.Now,
                            TextoEmail = "Existe uma nova tarefa pendente da sua aprovação no eSUCH!",
                            Enviado = false
                        };


                        SendEmailApprovals Email = new SendEmailApprovals
                        {
                            Subject = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Aprovação Pendente" : "eSUCH - Aprovação Pendente" + itemToApproveInfo,
                            From = "plataforma@such.pt"
                        };

                        Email.To.Add(e.RfmailEnvio);

                        Email.Body = MakeEmailBodyContent("Existe uma nova tarefa pendente da sua aprovação no eSUCH!");

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

        public static ErrorHandler ApproveMovement_FH(int movementNo, string ApproveUser)
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

                FolhasDeHoras FolhaHoras = DBFolhasDeHoras.GetById(ApprovalMovement.Number);
                int NoAjudasCusto = DBLinhasFolhaHoras.GetAll().Where(x => x.NoFolhaHoras.ToLower() == FolhaHoras.NºFolhaDeHoras.ToLower() && x.TipoCusto == 2).Count();
                int Estado = 0;
                int nivel = 99;
                bool IntegradoEmRh = false;
                bool IntegradoEmRhkm = false;

                if (FolhaHoras.TipoDeslocação != 2 && NoAjudasCusto == 0)
                {
                    Estado = 2; // 2 = Registado FINAL
                    nivel = 99;
                }
                else
                {
                    Estado = 1; //VALIDADO
                    if ((FolhaHoras.IntegradoEmRh == false || FolhaHoras.IntegradoEmRh == null) && FolhaHoras.Estado == Estado)
                    {
                        IntegradoEmRh = true; //IntegracaoAjuda
                        ApprovalMovement.Value = FolhaHoras.CustoTotalHoras;
                        nivel = 2;
                    }
                    else
                    {
                        if ((FolhaHoras.IntegradoEmRhkm == false || FolhaHoras.IntegradoEmRhkm == null) && FolhaHoras.Estado == Estado && FolhaHoras.TipoDeslocação == 2)
                        {
                            IntegradoEmRhkm = true; //IntegracaoKMS
                            ApprovalMovement.Value = FolhaHoras.CustoTotalKm;
                            nivel = 3;
                        }
                        else
                        {
                            Estado = 2; // 2 = Registado FINAL
                            nivel = 99;
                        }
                    }
                }

                if (Estado == 1)
                {
                    if (IntegradoEmRh)
                    {
                        //Get Next Level Configuration
                        List<ConfiguraçãoAprovações> ApprovalConfigurations = DBApprovalConfigurations.GetByTypeAreaValueDateAndDimensionsAndNivel(ApprovalMovement.Type.Value, ApprovalMovement.FunctionalArea, ApprovalMovement.ResponsabilityCenter, ApprovalMovement.Region, ApprovalMovement.Value.Value, DateTime.Now, nivel);
                        ApprovalConfigurations.RemoveAll(x => !x.NívelAprovação.HasValue || x.NívelAprovação <= ApprovalMovement.Level);

                        string itemToApproveInfo = string.Empty;
                        if (ApprovalMovement != null)
                        {
                            if (ApprovalMovement.Type.Value == 3 && !string.IsNullOrEmpty(ApprovalMovement.Number))
                                itemToApproveInfo += " - Folha de Horas " + ApprovalMovement.Number;
                        }
                        if (ApprovalConfigurations.Count > 0 && Estado != 2)
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
                            //ApprovalConfigurations.ForEach(x =>
                            //{
                            var approvalConfiguration = ApprovalConfigurations[0];
                            if (approvalConfiguration.UtilizadorAprovação != "" && approvalConfiguration.UtilizadorAprovação != null)
                            {
                                DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = approvalConfiguration.UtilizadorAprovação });
                                UsersToNotify.Add(approvalConfiguration.UtilizadorAprovação);
                            }
                            else if (approvalConfiguration.GrupoAprovação.HasValue)
                            {
                                List<string> GUsers = DBApprovalUserGroup.GetAllFromGroup(approvalConfiguration.GrupoAprovação.Value);

                                GUsers.ForEach(y =>
                                {
                                    DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = y });
                                    UsersToNotify.Add(y);
                                });
                            }
                            //});

                            //Notify Users
                            UsersToNotify = UsersToNotify.Distinct().ToList();
                            UsersToNotify.ForEach(e =>
                            {
                                EmailsAprovações EmailApproval = new EmailsAprovações()
                                {
                                    NºMovimento = ApprovalMovement.MovementNo,
                                    EmailDestinatário = e,
                                    NomeDestinatário = e,
                                    Assunto = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Aprovação Pendente" : "eSUCH - Aprovação Pendente" + itemToApproveInfo,
                                    DataHoraEmail = DateTime.Now,
                                    TextoEmail = "Existe uma nova tarefa pendente da sua aprovação no eSUCH!",
                                    Enviado = false
                                };


                                SendEmailApprovals Email = new SendEmailApprovals
                                {
                                    Subject = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Aprovação Pendente" : "eSUCH - Aprovação Pendente" + itemToApproveInfo,
                                    From = "plataforma@such.pt"
                                };

                                Email.To.Add(e);

                                Email.Body = MakeEmailBodyContent("Existe uma nova tarefa pendente da sua aprovação no eSUCH!");

                                Email.IsBodyHtml = true;
                                Email.EmailApproval = EmailApproval;

                                Email.SendEmail();

                                EmailApproval.Enviado = true;
                                EmailApproval.ObservaçõesEnvio = "Mensagem enviada com Sucesso";
                                DBApprovalEmails.Create(EmailApproval);
                            });
                        }
                        else
                        {
                            EmailsAprovações EmailApproval = new EmailsAprovações()
                            {
                                NºMovimento = ApprovalMovement.MovementNo,
                                EmailDestinatário = ApprovalMovement.RequestUser,
                                NomeDestinatário = ApprovalMovement.RequestUser,
                                Assunto = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Tarefa aprovada" : "eSUCH - Tarefa aprovada" + itemToApproveInfo,
                                DataHoraEmail = DateTime.Now,
                                TextoEmail = "A sua tarefa com o Nº " + ApprovalMovement.Number + " foi aprovada com sucesso!",
                                Enviado = false
                            };

                            SendEmailApprovals Email = new SendEmailApprovals
                            {
                                Subject = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Tarefa aprovada" : "eSUCH - Tarefa aprovada" + itemToApproveInfo,
                                From = "plataforma@such.pt"
                            };

                            Email.To.Add(ApprovalMovement.RequestUser);

                            Email.Body = MakeEmailBodyContent("A sua tarefa com o Nº " + ApprovalMovement.Number + " foi aprovada com sucesso!");

                            Email.IsBodyHtml = true;
                            Email.EmailApproval = EmailApproval;

                            Email.SendEmail();

                            EmailApproval.Enviado = true;
                            EmailApproval.ObservaçõesEnvio = "Mensagem enviada com Sucesso";
                            DBApprovalEmails.Create(EmailApproval);

                            return new ErrorHandler()
                            {
                                eReasonCode = 353,
                                eMessage = "A tarefa foi aprovada pelo ultimo nivel."
                            };
                        }
                    }

                    if (IntegradoEmRhkm)
                    {
                        //Get Next Level Configuration
                        List<ConfiguraçãoAprovações> ApprovalConfigurations = DBApprovalConfigurations.GetByTypeAreaValueDateAndDimensionsAndNivel(ApprovalMovement.Type.Value, ApprovalMovement.FunctionalArea, ApprovalMovement.ResponsabilityCenter, ApprovalMovement.Region, ApprovalMovement.Value.Value, DateTime.Now, nivel);
                        ApprovalConfigurations.RemoveAll(x => !x.NívelAprovação.HasValue || x.NívelAprovação <= ApprovalMovement.Level);

                        string itemToApproveInfo = string.Empty;
                        if (ApprovalMovement != null)
                        {
                            if (ApprovalMovement.Type.Value == 3 && !string.IsNullOrEmpty(ApprovalMovement.Number))
                                itemToApproveInfo += " - Folha de Horas " + ApprovalMovement.Number;
                        }
                        if (ApprovalConfigurations.Count > 0 && Estado != 2)
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
                            //ApprovalConfigurations.ForEach(x =>
                            //{
                            var approvalConfiguration = ApprovalConfigurations[0];
                            if (approvalConfiguration.UtilizadorAprovação != "" && approvalConfiguration.UtilizadorAprovação != null)
                            {
                                DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = approvalConfiguration.UtilizadorAprovação });
                                UsersToNotify.Add(approvalConfiguration.UtilizadorAprovação);
                            }
                            else if (approvalConfiguration.GrupoAprovação.HasValue)
                            {
                                List<string> GUsers = DBApprovalUserGroup.GetAllFromGroup(approvalConfiguration.GrupoAprovação.Value);

                                GUsers.ForEach(y =>
                                {
                                    DBUserApprovalMovements.Create(new UtilizadoresMovimentosDeAprovação() { NºMovimento = ApprovalMovement.MovementNo, Utilizador = y });
                                    UsersToNotify.Add(y);
                                });
                            }
                            //});

                            //Notify Users
                            UsersToNotify = UsersToNotify.Distinct().ToList();
                            UsersToNotify.ForEach(e =>
                            {
                                EmailsAprovações EmailApproval = new EmailsAprovações()
                                {
                                    NºMovimento = ApprovalMovement.MovementNo,
                                    EmailDestinatário = e,
                                    NomeDestinatário = e,
                                    Assunto = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Aprovação Pendente" : "eSUCH - Aprovação Pendente" + itemToApproveInfo,
                                    DataHoraEmail = DateTime.Now,
                                    TextoEmail = "Existe uma nova tarefa pendente da sua aprovação no eSUCH!",
                                    Enviado = false
                                };


                                SendEmailApprovals Email = new SendEmailApprovals
                                {
                                    Subject = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Aprovação Pendente" : "eSUCH - Aprovação Pendente" + itemToApproveInfo,
                                    From = "plataforma@such.pt"
                                };

                                Email.To.Add(e);

                                Email.Body = MakeEmailBodyContent("Existe uma nova tarefa pendente da sua aprovação no eSUCH!");

                                Email.IsBodyHtml = true;
                                Email.EmailApproval = EmailApproval;

                                Email.SendEmail();

                                EmailApproval.Enviado = true;
                                EmailApproval.ObservaçõesEnvio = "Mensagem enviada com Sucesso";
                                DBApprovalEmails.Create(EmailApproval);
                            });
                        }
                        else
                        {
                            EmailsAprovações EmailApproval = new EmailsAprovações()
                            {
                                NºMovimento = ApprovalMovement.MovementNo,
                                EmailDestinatário = ApprovalMovement.RequestUser,
                                NomeDestinatário = ApprovalMovement.RequestUser,
                                Assunto = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Tarefa aprovada" : "eSUCH - Tarefa aprovada" + itemToApproveInfo,
                                DataHoraEmail = DateTime.Now,
                                TextoEmail = "A sua tarefa com o Nº " + ApprovalMovement.Number + " foi aprovada com sucesso!",
                                Enviado = false
                            };

                            SendEmailApprovals Email = new SendEmailApprovals
                            {
                                Subject = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Tarefa aprovada" : "eSUCH - Tarefa aprovada" + itemToApproveInfo,
                                From = "plataforma@such.pt"
                            };

                            Email.To.Add(ApprovalMovement.RequestUser);

                            Email.Body = MakeEmailBodyContent("A sua tarefa com o Nº " + ApprovalMovement.Number + " foi aprovada com sucesso!");

                            Email.IsBodyHtml = true;
                            Email.EmailApproval = EmailApproval;

                            Email.SendEmail();

                            EmailApproval.Enviado = true;
                            EmailApproval.ObservaçõesEnvio = "Mensagem enviada com Sucesso";
                            DBApprovalEmails.Create(EmailApproval);

                            return new ErrorHandler()
                            {
                                eReasonCode = 353,
                                eMessage = "A tarefa foi aprovada pelo ultimo nivel."
                            };
                        }
                    }
                }

                return new ErrorHandler()
                {
                    eReasonCode = 350,
                    eMessage = "A tarefa foi aprovada com sucesso."
                };
            }
            catch (Exception ex)
            {
                return new ErrorHandler()
                {
                    eReasonCode = 351,
                    eMessage = "Ocorreu um erro desconhecido ao aprovar a tarefa."
                };
            }
        }

        //100 - Tarefa rejeitada com sucesso
        //101 - Erro desconhecido
        public static ErrorHandler RejectMovement_FH(int movementNo, string rejectUser, string rejectReason)
        {
            try
            {
                //Update Old Movement
                ApprovalMovementsViewModel ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.GetById(movementNo));
                FolhasDeHoras FH = DBFolhasDeHoras.GetById(ApprovalMovement.Number);

                ApprovalMovement.Status = 3;
                ApprovalMovement.DateTimeApprove = DateTime.Now;
                ApprovalMovement.DateTimeUpdate = DateTime.Now;
                ApprovalMovement.UserUpdate = rejectUser;
                ApprovalMovement.ReproveReason = rejectReason;
                ApprovalMovement = DBApprovalMovements.ParseToViewModel(DBApprovalMovements.Update(DBApprovalMovements.ParseToDatabase(ApprovalMovement)));

                //Delete All User Approval Movements
                DBUserApprovalMovements.DeleteFromMovementExcept(ApprovalMovement.MovementNo, rejectUser);

                string itemToApproveInfo = string.Empty;
                if (ApprovalMovement.Type.Value == 3 && !string.IsNullOrEmpty(ApprovalMovement.Number))
                    itemToApproveInfo += " - Folha de Horas " + ApprovalMovement.Number;

                EmailsAprovações EmailApproval = new EmailsAprovações()
                {
                    NºMovimento = ApprovalMovement.MovementNo,
                    EmailDestinatário = FH.CriadoPor, // ApprovalMovement.RequestUser,
                    NomeDestinatário = FH.CriadoPor, // ApprovalMovement.RequestUser,
                    Assunto = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Tarefa rejeitada" : "eSUCH - Tarefa rejeitada" + itemToApproveInfo,
                    DataHoraEmail = DateTime.Now,
                    TextoEmail = "A sua tarefa com o Nº " + ApprovalMovement.Number + " foi rejeitada pelo seguinte motivo \"" + ApprovalMovement.ReproveReason + "\"!",
                    Enviado = false
                };


                SendEmailApprovals Email = new SendEmailApprovals
                {
                    Subject = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Tarefa rejeitada" : "eSUCH - Tarefa rejeitada" + itemToApproveInfo,
                    From = "plataforma@such.pt"
                };

                Email.To.Add(FH.CriadoPor); // ApprovalMovement.RequestUser);

                Email.Body = MakeEmailBodyContent("A sua tarefa com o Nº " + ApprovalMovement.Number + " foi rejeitada pelo seguinte motivo \"" + ApprovalMovement.ReproveReason + "\"!");

                Email.IsBodyHtml = true;
                Email.EmailApproval = EmailApproval;

                Email.SendEmail();
                return new ErrorHandler()
                {
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

                string itemToApproveInfo = string.Empty;
                if (ApprovalMovement.Type.Value == 1 && !string.IsNullOrEmpty(ApprovalMovement.Number))
                    itemToApproveInfo += " - Requisição " + ApprovalMovement.Number;

                EmailsAprovações EmailApproval = new EmailsAprovações()
                {
                    NºMovimento = ApprovalMovement.MovementNo,
                    EmailDestinatário = ApprovalMovement.RequestUser,
                    NomeDestinatário = ApprovalMovement.RequestUser,
                    Assunto = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Tarefa rejeitada" : "eSUCH - Tarefa rejeitada" + itemToApproveInfo,
                    DataHoraEmail = DateTime.Now,
                    TextoEmail = "A sua tarefa com o Nº " + ApprovalMovement.Number + " foi rejeitada pelo seguinte motivo \"" + ApprovalMovement.ReproveReason + "\"!",
                    Enviado = false
                };


                SendEmailApprovals Email = new SendEmailApprovals
                {
                    Subject = string.IsNullOrEmpty(itemToApproveInfo) ? "eSUCH - Tarefa rejeitada" : "eSUCH - Tarefa rejeitada" + itemToApproveInfo,
                    From = "plataforma@such.pt"
                };

                Email.To.Add(ApprovalMovement.RequestUser);

                Email.Body = MakeEmailBodyContent("A sua tarefa com o Nº " + ApprovalMovement.Number + " foi rejeitada pelo seguinte motivo \"" + ApprovalMovement.ReproveReason + "\"!");

                Email.IsBodyHtml = true;
                Email.EmailApproval = EmailApproval;

                Email.SendEmail();
                return new ErrorHandler()
                {
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
