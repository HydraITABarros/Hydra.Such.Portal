using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Compras;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Portal.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Services
{
    public class BillingReceptionService
    {
        private BillingReceptionRepo repo = new BillingReceptionRepo();

        public BillingReceptionService()
        {
            repo = new BillingReceptionRepo();
        }

        public BillingReceptionModel Create(BillingReceptionModel item)
        {
            item.DataCriacao = DateTime.Now;
            item.AreaPendente = (int)BillingReceptionAreas.Contabilidade;
            item.Estado = BillingReceptionStates.Rececao;
            item.DataCriacao = DateTime.Now;
            item.DataUltimaInteracao = DateTime.Now.ToString();
            item = repo.Create(item);

            RececaoFaturacaoWorkflow wfItem = new RececaoFaturacaoWorkflow();
            wfItem.IdRecFaturacao = item.Id;
            wfItem.AreaWorkflow = Data.EnumHelper.GetDescriptionFor(typeof(BillingReceptionAreas), (int)BillingReceptionAreas.Contabilidade);
            wfItem.Descricao = "Entrada fatura em receção";
            wfItem.CriadoPor = item.CriadoPor;
            wfItem.Data = DateTime.Now;
            wfItem.DataCriacao = DateTime.Now;
            wfItem.Estado = (int)BillingReceptionStates.Rececao;//TODO: Identificar estados possivels “Receção/Conferência”
            wfItem.Utilizador = item.CriadoPor;
            repo.Create(wfItem);

            try
            {
                repo.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }
            return item;
        }
         

        public string CreateNumeration(BillingReceptionModel item)
        {

            return "";
        }



        public BillingReceptionModel Update(BillingReceptionModel item)
        {
            item.DataModificacao = DateTime.Now;
            item.DataUltimaInteracao = DateTime.Now.ToString();

            item = repo.Update(item);
            try
            {
                repo.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }
            return item;
        }

        public List<BillingReceptionModel> GetAllForUser(string userName)
        {
            var billingReceptions = repo.GetAll();

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(userName);
            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                billingReceptions.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CodRegiao));
            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                billingReceptions.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CodAreaFuncional));
            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                billingReceptions.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CodCentroResponsabilidade));

            return billingReceptions;
        }

        public BillingReceptionModel GetById(string id)
        {

            //if (billingReception != null)
            //{
            //    List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);

            //    if (!userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == billingReception.CodRegiao))
            //        return Json(null);
            //    if (!userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == billingReception.CodAreaFuncional))
            //        return Json(null);
            //    if (!userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == billingReception.CodCentroResponsabilidade))
            //        return Json(null);
            //}

            return repo.GetById(id);
        }

        public bool Delete(BillingReceptionModel item)
        {
            repo.Delete(item);
            try
            {
                repo.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public BillingReceptionModel CreateWorkFlowSend(BillingReceptionModel item, BillingRecWorkflowModel wfItemLast, string postedByUserName)
        {
            //Update Header
            item.Estado = BillingReceptionStates.Pendente;
            item.DataUltimaInteracao = DateTime.Now.ToString();
            
            //item.AreaPendente;
            //item.AreaPendenteDescricao;
            item.Destinatario = wfItemLast.Utilizador;
            // descrição problea
            item.DataModificacao = DateTime.Now;
            //area ultima interação
            //user ultima interação

            item = repo.Update(item);
            

            RececaoFaturacaoWorkflow wfItem = new RececaoFaturacaoWorkflow();
            wfItem.IdRecFaturacao = item.Id;
            wfItem.Estado = (int)BillingReceptionStates.Pendente;//TODO: Identificar estados possivels “Receção/Conferência”        
            wfItem.AreaWorkflow = wfItemLast.AreaWorkflow;
            wfItem.ModificadoPor = item.ModificadoPor;
            wfItem.Data = item.DataCriacao;
            wfItem.DataCriacao = DateTime.Now;
            wfItem.EnderecoEnvio = postedByUserName;
            wfItem.EnderecoFornecedor = wfItemLast.EnderecoFornecedor;
            wfItem.Utilizador = wfItemLast.Utilizador;
            wfItem.CodTipoProblema = wfItemLast.CodTipoProblema;
            wfItem.CodProblema = wfItemLast.CodProblema;
            wfItem.Descricao = wfItemLast.Descricao;
            wfItem.Comentario = wfItemLast.Comentario;

            repo.Create(wfItem);

            try
            {
                repo.SaveChanges();
                
            }
            catch (Exception ex)
            {
                return null;
            }
            RecFacturasProblemas question = null;
           if ( wfItem.CodProblema== "RF1P")
                question = GetQuestionID(wfItem.CodTipoProblema, "RF1P");
           else
                question = GetQuestionID(wfItem.CodTipoProblema, "RF4P");
            if (question != null)
            {
                //Rever o Envio de Areas
                if (question.EnvioAreas != "1" && wfItem.EnderecoFornecedor != "")
                {

                    SendEmailBillingReception Email = new SendEmailBillingReception
                    {
                        Subject = "eSUCH - Recepção da Factura :" + item.Id,
                        From = "plataforma@such.pt"
                    };

                    Email.To.Add("rui.santos99@hotmail.com");

                    Email.Body = MakeEmailBodyContent("Solicita-se a validação da fatura enviada em anexo:", item.Id, item.Destinatario, item.DataUltimaInteracao, item.Valor.ToString(), postedByUserName);

                    Email.IsBodyHtml = true;

                    Email.SendEmail();
                    try
                    {
                        item.eReasonCode = 1;
                        item.eMessage = "Resposta enviada com sucesso.";
                    }
                    catch
                    {
                        item.eReasonCode = 2;
                        item.eMessage = "Não foi possível enviar email ao utilizador que criou o pedido (" + item.Id + ")";
                    }
                   
                }
            }
            return item;
        }

        public BillingReceptionModel PostDocument(BillingReceptionModel item, string postedByUserName, NAVConfigurations _config, NAVWSConfigurations _configws)
        {
            if (item != null)
            {
                if (ValidateForPosting(ref item, _config))
                {
                    item.DocumentoCriadoEm = DateTime.Now;
                    item.DocumentoCriadoPor = postedByUserName;
                    Task<WsPrePurchaseDocs.Create_Result> createPurchHeaderTask = NAVPurchaseHeaderService.CreateAsync(item, _configws);
                    createPurchHeaderTask.Wait();
                    if (createPurchHeaderTask.IsCompletedSuccessfully)
                    {
                        item.Id = item.Id.Remove(0, 2);                     
                        repo.Update(item);

                        try
                        {
                            repo.SaveChanges();

                            item.eReasonCode = 1;
                            item.eMessage = "Fatura criada com sucesso.";
                        }
                        catch
                        {
                            //TODO: Rever comportamento no caso de erro
                            item.eReasonCode = 2;
                            item.eMessage = "Fatura rececionada mas não foi possivel atualizar os dados da receção.";
                        }
                    }
                    else
                    {
                        item.Id = item.Id.Remove(0, 2);
                       
                    }
                }
            }
            else
            {                
                item.eReasonCode = 2;
                item.eMessage = "O registo não pode ser nulo";
            }
            return item;
        }

        private bool ValidateForPosting(ref BillingReceptionModel item, NAVConfigurations _config)
        {
            bool isValid = true;
            if(Convert.ToDateTime(item.DataDocFornecedor)>item.DataModificacao)
            {
                item.eMessages.Add(new TraceInformation(TraceType.Error, "A data do documento: " + item.DataDocFornecedor + " é maior que a data do registo: " + item.DataModificacao));
                isValid = false;
            }
            else if (!string.IsNullOrEmpty(item.NumEncomenda))
            {
                var purchOrderInfo = DBNAV2017Purchases.GetOrderById(_config.NAVDatabaseName, _config.NAVCompanyName, item.NumEncomenda);
                if (purchOrderInfo.No != item.NumEncomenda)
                {
                    item.eMessages.Add(new TraceInformation(TraceType.Error, "A encomenda " + item.NumEncomenda + " não existe ou já está registada."));
                    isValid = false;
                }
            }
            else 
            {
                if (!string.IsNullOrEmpty(item.NumEncomendaManual))
                {
                    var purchOrderInfo = DBNAV2017Purchases.GetOrderById(_config.NAVDatabaseName, _config.NAVCompanyName, item.NumEncomendaManual);
                    if (purchOrderInfo.No != item.NumEncomendaManual)
                    {
                        item.eMessages.Add(new TraceInformation(TraceType.Error, "A encomenda (Núm. Encomenda Manual) " + item.NumEncomendaManual + " não existe ou já está registada."));
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        #region Problems

        public List<RecFacturasProblemas> GetProblem()
        {
            return repo.GetQuestionsProblem();
        }
        public RecFacturasProblemas GetQuestionID(string id,string Cod)
        {
            if (id != null && id != "")
            {
                RecFacturasProblemas problem = repo.GetQuestionsID(id, Cod).LastOrDefault();
                return problem;
            }
            else
                return null;
        }
       
        public List<RecFacturasProblemas> GetReason()
        {
            return repo.GetQuestionsReason();
        }
        public List<RecFaturacaoConfigDestinatarios> GetAreas()
        {
            return repo.GetAreas();
        }
        public static string MakeEmailBodyContent(string Into, string IfFactura, string Fornecedor, string Data, string Valor, string Utilizador)
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
                                                Into +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                         "<tr>" +
                                            "<td>" +
                                               "NºFactura...: " + IfFactura +
                                            "</td>" +
                                        "</tr>" +
                                         "<tr>" +
                                            "<td>" +
                                                "Fornecedor..: " + Fornecedor +
                                            "</td>" +
                                        "</tr>" +
                                         "<tr>" +
                                            "<td>" +
                                                "Data..........: " + Data +
                                            "</td>" +
                                        "</tr>" +
                                         "<tr>" +
                                            "<td>" +
                                                "Valor.........: " + Valor +
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
                                                "<i>SUCH -" + Utilizador + "</i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";

            return Body;
        }
        #endregion
    }
}
