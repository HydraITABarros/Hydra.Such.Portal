using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Compras;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Portal.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public BillingReceptionModel PostDocument(BillingReceptionModel item, string postedByUserName, NAVConfigurations _config, NAVWSConfigurations _configws)
        {
            if (item != null)
            {
                if (ValidateForPosting(item, _config))
                {
                    Task<WsPrePurchaseDocs.Create_Result> createPurchHeaderTask = NAVPurchaseHeaderService.CreateAsync(item, _configws);
                    createPurchHeaderTask.Wait();
                    if (createPurchHeaderTask.IsCompletedSuccessfully)
                    {
                        string typeDescription = Data.EnumHelper.GetDescriptionFor(item.TipoDocumento.GetType(), (int)item.TipoDocumento);

                        //createPurchHeaderTask.Result.WSPrePurchaseDocs
                        RececaoFaturacaoWorkflow rfws = new RececaoFaturacaoWorkflow();
                        rfws.IdRecFaturacao = item.Id;
                        rfws.Descricao = "Contabilização da " + typeDescription;
                        rfws.Estado = (int)BillingReceptionStates.Contabilizado;
                        rfws.Data = DateTime.Now;
                        rfws.Utilizador = postedByUserName;
                        rfws.CriadoPor = postedByUserName;

                        repo.Create(rfws);

                        item.DocumentoCriadoEm = DateTime.Now;
                        item.DocumentoCriadoPor = postedByUserName;
                        repo.Update(item);

                        try
                        {
                            repo.SaveChanges();

                            item.eReasonCode = 1;
                            item.eMessage = "Documento criado com sucesso.";
                        }
                        catch
                        {
                            //TODO: Rever comportamento no caso de erro
                            item.eReasonCode = 2;
                            item.eMessage = "Fatura rececionada mas não foi possivel atualizar os dados da receção.";
                        }
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

        private bool ValidateForPosting(BillingReceptionModel item, NAVConfigurations _config)
        {
            bool isValid = true;
            if (item.Estado != BillingReceptionStates.Rececao || item.Estado != BillingReceptionStates.Pendente)
            {
                string stateDescription = Data.EnumHelper.GetDescriptionFor(typeof(BillingReceptionStates), (int)item.Estado);
                item.eMessages.Add(new TraceInformation(TraceType.Error, "Este documento já se encontra no estado: " + stateDescription));
                isValid = false;
            }
            if (string.IsNullOrEmpty(item.CodFornecedor))
            {
                item.eMessages.Add(new TraceInformation(TraceType.Error, "O Fornecedor tem que estar preenchido."));
                isValid = false;
            }
            if (string.IsNullOrEmpty(item.NumDocFornecedor))
            {
                item.eMessages.Add(new TraceInformation(TraceType.Error, "O Nº Documento do Fornecedor tem que estar preenchido."));
                isValid = false;
            }
            else
            {
                var purchItemInfo = DBNAV2017Purchases.GetByExternalDocNo(_config.NAVDatabaseName, _config.NAVCompanyName, (NAVBaseDocumentTypes)item.TipoDocumento, item.NumDocFornecedor);
                if (purchItemInfo != null)
                {
                    string typeDescription = Data.EnumHelper.GetDescriptionFor(typeof(BillingReceptionStates), (int)item.Estado);
                    item.eMessages.Add(new TraceInformation(TraceType.Error, "Já foi criada " + typeDescription + " para este RF."));
                    isValid = false;
                }
            }
            if (!item.Valor.HasValue)
            {
                item.eMessages.Add(new TraceInformation(TraceType.Error, "O valor tem que estar preenchido."));
                isValid = false;
            }
            if (string.IsNullOrEmpty(item.CodRegiao))
            {
                item.eMessages.Add(new TraceInformation(TraceType.Error, "Tem que selecionar a Região."));
                isValid = false;
            }
            if (!string.IsNullOrEmpty(item.NumEncomenda))
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
                var purchOrderInfo = DBNAV2017Purchases.GetOrderById(_config.NAVDatabaseName, _config.NAVCompanyName, item.NumEncomendaManual);
                if (purchOrderInfo.No != item.NumEncomendaManual)
                {
                    item.eMessages.Add(new TraceInformation(TraceType.Error, "A encomenda (Núm. Encomenda Manual) " + item.NumEncomendaManual + " não existe ou já está registada."));
                    isValid = false;
                }
            }

            return isValid;
        }

        #region Problems

        public List<RecFacturasProblemas> GetProblem()
        {
            return repo.GetQuestionsProblem();
        }
        public List<RecFacturasProblemas> GetReason()
        {
            return repo.GetQuestionsReason();
        }
        public List<RecFaturacaoConfigDestinatarios> GetAreas()
        {
            return repo.GetAreas();
        }

        #endregion
    }
}
