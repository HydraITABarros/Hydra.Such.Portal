using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class FaturacaoController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public FaturacaoController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        public IActionResult RececaoFaturas()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ReceçãoFaturação);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UserPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesRecFatura(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ReceçãoFaturação);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Id = id;
                ViewBag.UserPermissions = UPerm;
                ViewBag.BillingReceptionStates = EnumHelper.GetItemsAsDictionary(typeof(BillingReceptionStates));
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetBillingReceptions()
        {
            var billingReceptions = DBBillingReception.GetAll();

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                billingReceptions.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CodRegiao));
            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                billingReceptions.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CodAreaFuncional));
            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                billingReceptions.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CodCentroResponsabilidade));

            return Json(billingReceptions);
        }

        [HttpGet]
        public JsonResult GetBillingReception(string id)
        {
            var billingReception = DBBillingReception.GetById(id);

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
            return Json(billingReception);
        }

        [HttpPost]
        public JsonResult CreateBillingReception([FromBody] BillingReceptionModel item)
        {
            BillingReceptionModel createdItem = null;
            if (item != null)
            {
                item.CriadoPor = User.Identity.Name;
                createdItem = DBBillingReception.Create(item);
                createdItem.eReasonCode = 1;
                createdItem.eMessage = "Registo criado com sucesso";
            }
            else
            {
                createdItem = new BillingReceptionModel
                {
                    eReasonCode = 2,
                    eMessage = "O registo não pode ser nulo",
                };
            }
            return Json(createdItem);
        }

        [HttpPost]
        public JsonResult UpdateBillingReception([FromBody] BillingReceptionModel item)
        {
            BillingReceptionModel updatedItem = null;
            //if (item != null)
            if (updatedItem != null)
            {
            item.ModificadoPor = User.Identity.Name;
            updatedItem = DBBillingReception.Update(item);
            updatedItem.eReasonCode = 1;
            updatedItem.eMessage = "Registo atualizado com sucesso";
            }
            else
            {
                updatedItem = new BillingReceptionModel
                {
                    eReasonCode = 2,
                    eMessage = "O registo não pode ser nulo",
                };
            }
            return Json(updatedItem);
        }

        [HttpPost]
        public JsonResult PostDocument([FromBody] BillingReceptionModel item)
        {
            if (item != null)
            {
                if (ValidateForPosting(item))
                {
                    Task<WsPrePurchaseDocs.Create_Result> createPurchHeaderTask = NAVPurchaseHeaderService.CreateAsync(item, _configws);
                    createPurchHeaderTask.Wait();
                    if (createPurchHeaderTask.IsCompletedSuccessfully)
                    {
                        string typeDescription = EnumHelper.GetDescriptionFor(item.TipoDocumento.GetType(), (int)item.TipoDocumento);

                        //createPurchHeaderTask.Result.WSPrePurchaseDocs
                        RececaoFaturacaoWorkflow rfws = new RececaoFaturacaoWorkflow();
                        rfws.IdRecFaturacao = item.Id;
                        rfws.Descricao = "Contabilização da " + typeDescription;
                        rfws.Estado = (int)BillingReceptionStates.Contabilizado;
                        rfws.Data = DateTime.Now;
                        rfws.Utilizador = User.Identity.Name;
                        rfws.CriadoPor = User.Identity.Name;

                        var createdItem = DBBillingReceptionWf.Create(rfws);
                        if (createdItem != null)
                        {
                            item.eReasonCode = 1;
                            item.eMessage = "Documento criado com sucesso.";
                        }
                    }
                }
            }
            else
            {
                item.eReasonCode = 2;
                item.eMessage = "O registo não pode ser nulo";
            }
            return Json(item);
        }

        private bool ValidateForPosting(BillingReceptionModel item)
        {
            bool isValid = true;
            if (item.Estado != BillingReceptionStates.Rececao || item.Estado != BillingReceptionStates.Pendente)
            {
                string stateDescription = EnumHelper.GetDescriptionFor(typeof(BillingReceptionStates), (int)item.Estado);
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
                var purchItemInfo = DBNAV2017Purchases.GetByExternalDocNo(_config.NAVDatabaseName, _config.NAVCompanyName, (PurchaseDocumentTypes)item.TipoDocumento, item.NumDocFornecedor);
                if (purchItemInfo != null)
                {
                    string typeDescription = EnumHelper.GetDescriptionFor(typeof(BillingReceptionStates), (int)item.Estado);
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

        [HttpPost]
        public JsonResult DocumentIsDigitized([FromBody] BillingReceptionModel item)
        {
            //if (item != null)
            //{
                
            //}
            //else
            //{
            //    item.eReasonCode = 2;
            //    item.eMessage = "O registo não pode ser nulo";
            //}
            return Json(false);
        }
    }
}