using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.ViewModel.Compras;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.NAV;
using Hydra.Such.Portal.Configurations;
using Microsoft.Extensions.Options;
using Hydra.Such.Portal.Services;
using Hydra.Such.Portal.Extensions;
using Hydra.Such.Data.Logic.Project;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Data;
using Hydra.Such.Portal.Controllers;
using Newtonsoft.Json.Linq;

namespace Hydra.Such.Portal.Controllers
{
    public class GestaoRequisicoesController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public GestaoRequisicoesController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        
        public IActionResult Index()
        {
            UserAccessesViewModel userPermissions =
                DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.Requisições);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        
        public IActionResult Detalhes()
        {
            UserAccessesViewModel userPermissions =
                DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.Requisições);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        
        public IActionResult RequisicoesAprovadas()
        {
            UserAccessesViewModel userPermissions =
                DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.Requisições);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        
        public IActionResult DetalhesReqAprovada(string id)
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.Requisições);

            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                ViewBag.RequisitionId = id;
                ViewBag.ApprovedRequisitionEnumValue = (int)RequisitionStates.Approved;
                ViewBag.RequisitionStatesEnumString = EnumHelper.GetItemsAsDictionary(typeof(RequisitionStates));
                ViewBag.ReportServerURL = _config.ReportServerURL;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        
        public IActionResult LinhasRequisicao(string id)
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.Requisições);


            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                ViewBag.RequisitionId = id;
                ViewBag.ValidatedRequisitionEnumValue = (int)RequisitionStates.Validated;
                ViewBag.RequisitionStatesEnumString = EnumHelper.GetItemsAsDictionary(typeof(RequisitionStates));
                ViewBag.ReportServerURL = _config.ReportServerURL;

                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        public IActionResult Arquivadas()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Engenharia, Enumerations.Features.HistóricoRequisições);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Area = 4;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult HistóricoCabeçalhoRequisicao(string id)
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.HistóricoRequisições);


            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                ViewBag.RequisitionId = id;
                ViewBag.ArchivedRequisitionEnumValue = (int)RequisitionStates.Archived;
                ViewBag.RequisitionStatesEnumString = EnumHelper.GetItemsAsDictionary(typeof(RequisitionStates));
                ViewBag.ReportServerURL = _config.ReportServerURL;

                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        [HttpPost]
        public JsonResult GridRequestLineValues(/*[FromBody] string id*/)
        {
            //List<RequisitionLineViewModel> result = DBRequestLine.GetAllByRequisiçãos(id).ParseToViewModel();
            List<RequisitionLineViewModel> result = DBRequestLine.GetAll().ParseToViewModel();
            //Apply User Dimensions Validations
            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CenterResponsibilityCode));
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateAndDeleteRequestLine([FromBody] List<RequisitionLineViewModel> data)
        {
            if (data != null)
            {
                List<LinhasRequisição> previousList;
                // Get All
                previousList = DBRequestLine.GetAll();
                foreach (LinhasRequisição line in previousList)
                {
                    if (!data.Any(x => x.LineNo == line.NºLinha))
                    {
                        DBRequestLine.Delete(line);
                    }
                }
                //Update or Create
                try
                {
                    data.ForEach(x =>
                    {
                        List<LinhasRequisição> dpObject = DBRequestLine.GetByLineNo((int)x.LineNo);

                        if (dpObject.Count > 0)
                        {
                            LinhasRequisição newdp = dpObject.FirstOrDefault();
                            newdp.NºRequisição = x.RequestNo;
                            newdp.NºLinha = (int)x.LineNo;
                            newdp.Tipo = 2;
                            newdp.Código = x.Code;
                            newdp.Descrição = x.Description;
                            newdp.CódigoUnidadeMedida = x.UnitMeasureCode;
                            newdp.CódigoLocalização = x.LocalCode;
                            newdp.MercadoLocal =x.LocalMarket != null ? x.LocalMarket : false;
                            newdp.QuantidadeARequerer = x.QuantityToRequire;
                            newdp.QuantidadeRequerida = x.QuantityRequired;
                            newdp.QuantidadeADisponibilizar = x.QuantityToProvide;
                            newdp.QuantidadeDisponibilizada = x.QuantityAvailable;
                            newdp.QuantidadeAReceber = x.QuantityReceivable;
                            newdp.QuantidadeRecebida = x.QuantityReceived;
                            newdp.QuantidadePendente = x.QuantityPending;
                            newdp.CustoUnitário = x.UnitCost;
                            newdp.DataReceçãoEsperada = string.IsNullOrEmpty(x.ExpectedReceivingDate) ? (DateTime?)null : DateTime.Parse(x.ExpectedReceivingDate);
                            newdp.NºProjeto = x.ProjectNo;
                            newdp.CódigoRegião = x.RegionCode;
                            newdp.CódigoÁreaFuncional = x.FunctionalAreaCode;
                            newdp.CódigoCentroResponsabilidade = x.CenterResponsibilityCode;
                            newdp.NºFuncionário = x.FunctionalNo;
                            newdp.Viatura = x.Vehicle;
                            newdp.DataHoraCriação = x.CreateDateTime;
                            newdp.UtilizadorCriação = x.CreateUser;
                            newdp.DataHoraModificação = x.UpdateDateTime;
                            newdp.UtilizadorModificação = x.UpdateUser;
                            newdp.QtdPorUnidadeDeMedida = x.QtyByUnitOfMeasure;
                            newdp.PreçoUnitárioVenda = x.UnitCostsould;
                            newdp.ValorOrçamento = x.BudgetValue;
                            newdp.NºLinhaOrdemManutenção = x.MaintenanceOrderLineNo;
                            newdp.CriarConsultaMercado = x.CreateMarketSearch != null ? x.CreateMarketSearch : false;
                            newdp.EnviarPréCompra = x.SubmitPrePurchase != null ? x.SubmitPrePurchase : false;
                            newdp.EnviadoPréCompra = x.SendPrePurchase != null ? x.SendPrePurchase : false;
                            newdp.DataMercadoLocal = string.IsNullOrEmpty(x.LocalMarketDate) ? (DateTime?)null : DateTime.Parse(x.LocalMarketDate);
                            newdp.UserMercadoLocal = x.LocalMarketUser;
                            newdp.EnviadoParaCompras = x.SendForPurchase != null ? x.SendForPurchase : false;
                            newdp.DataEnvioParaCompras = string.IsNullOrEmpty(x.SendForPurchaseDate) ? (DateTime?)null : DateTime.Parse(x.SendForPurchaseDate);
                            newdp.ValidadoCompras = x.PurchaseValidated != null ? x.PurchaseValidated : false;
                            newdp.RecusadoCompras = x.PurchaseRefused != null ? x.PurchaseRefused : false;
                            newdp.MotivoRecusaMercLocal = x.ReasonToRejectionLocalMarket;
                            newdp.DataRecusaMercLocal = string.IsNullOrEmpty(x.RejectionLocalMarketDate) ? (DateTime?)null : DateTime.Parse(x.RejectionLocalMarketDate);
                            newdp.IdCompra = x.PurchaseId;
                            newdp.NºFornecedor = x.SupplierNo;
                            newdp.NºEncomendaAberto = x.OpenOrderNo;
                            newdp.NºLinhaEncomendaAberto = x.OpenOrderLineNo;
                            newdp.NºDeConsultaMercadoCriada = x.QueryCreatedMarketNo;
                            newdp.NºEncomendaCriada = x.CreatedOrderNo;
                            newdp.CódigoProdutoFornecedor = x.SupplierProductCode;
                            newdp.UnidadeProdutivaNutrição = x.UnitNutritionProduction;
                            newdp.RegiãoMercadoLocal = x.MarketLocalRegion;
                            newdp.NºCliente = x.CustomerNo;
                            newdp.Aprovadores = x.Approvers;
                            newdp.Faturável = x.Billable != null ? x.Billable : false;
                            DBRequestLine.Update(newdp);
                            
                        }
                        else
                        {
                            LinhasRequisição newdp = new LinhasRequisição()
                            {
                                NºRequisição = x.RequestNo,
                                NºLinha = (int)x.LineNo,
                                Tipo = 2,
                                Código = x.Code,
                                Descrição = x.Description,
                                CódigoUnidadeMedida = x.UnitMeasureCode,
                                CódigoLocalização = x.LocalCode,
                                MercadoLocal = x.LocalMarket != null ? x.LocalMarket : false,
                                QuantidadeARequerer = x.QuantityToRequire,
                                QuantidadeRequerida = x.QuantityRequired,
                                QuantidadeADisponibilizar = x.QuantityToProvide,
                                QuantidadeDisponibilizada = x.QuantityAvailable,
                                QuantidadeAReceber = x.QuantityReceivable,
                                QuantidadeRecebida = x.QuantityReceived,
                                QuantidadePendente = x.QuantityPending,
                                CustoUnitário = x.UnitCost,
                                DataReceçãoEsperada = string.IsNullOrEmpty(x.ExpectedReceivingDate) ? (DateTime?)null : DateTime.Parse(x.ExpectedReceivingDate),
                                Faturável = x.Billable != null ? x.Billable : false,
                                NºProjeto = x.ProjectNo,
                                CódigoRegião = x.RegionCode,
                                CódigoÁreaFuncional = x.FunctionalAreaCode,
                                CódigoCentroResponsabilidade = x.CenterResponsibilityCode,
                                NºFuncionário = x.FunctionalNo,
                                Viatura = x.Vehicle,
                                DataHoraCriação = x.CreateDateTime,
                                UtilizadorCriação = x.CreateUser,
                                DataHoraModificação = x.UpdateDateTime,
                                UtilizadorModificação = x.UpdateUser,
                                QtdPorUnidadeDeMedida = x.QtyByUnitOfMeasure,
                                PreçoUnitárioVenda = x.UnitCostsould,
                                ValorOrçamento = x.BudgetValue,
                                NºLinhaOrdemManutenção = x.MaintenanceOrderLineNo,
                                CriarConsultaMercado = x.CreateMarketSearch != null ? x.CreateMarketSearch : false,
                                EnviarPréCompra = x.SubmitPrePurchase != null ? x.SubmitPrePurchase : false,
                                EnviadoPréCompra = x.SendPrePurchase != null ? x.SendPrePurchase : false,
                                DataMercadoLocal = string.IsNullOrEmpty(x.LocalMarketDate) ? (DateTime?)null : DateTime.Parse(x.LocalMarketDate),
                                UserMercadoLocal = x.LocalMarketUser,
                                EnviadoParaCompras = x.SendForPurchase != null ? x.SendForPurchase : false,
                                DataEnvioParaCompras = string.IsNullOrEmpty(x.SendForPurchaseDate) ? (DateTime?)null : DateTime.Parse(x.SendForPurchaseDate),
                                ValidadoCompras = x.PurchaseValidated != null ? x.PurchaseValidated : false,
                                RecusadoCompras = x.PurchaseRefused != null ? x.PurchaseRefused : false,
                                MotivoRecusaMercLocal = x.ReasonToRejectionLocalMarket,
                                DataRecusaMercLocal = string.IsNullOrEmpty(x.RejectionLocalMarketDate) ? (DateTime?)null : DateTime.Parse(x.RejectionLocalMarketDate),
                                IdCompra = x.PurchaseId,
                                NºFornecedor = x.SupplierNo,
                                NºEncomendaAberto = x.OpenOrderNo,
                                NºLinhaEncomendaAberto = x.OpenOrderLineNo,
                                NºDeConsultaMercadoCriada = x.QueryCreatedMarketNo,
                                NºEncomendaCriada = x.CreatedOrderNo,
                                CódigoProdutoFornecedor = x.SupplierProductCode,
                                UnidadeProdutivaNutrição = x.UnitNutritionProduction,
                                RegiãoMercadoLocal = x.MarketLocalRegion,
                                NºCliente = x.CustomerNo,
                                Aprovadores = x.Approvers,
                            };
                            newdp.UtilizadorCriação = User.Identity.Name;

                            newdp.DataHoraCriação = DateTime.Now;
                            DBRequestLine.Create(newdp);
                        }
                    });
                }
                catch (Exception e)
                {
                    return Json(null);
                }
            }

            return Json(data);
        }

        [HttpPost]
        
        public JsonResult CreateRequisition([FromBody] RequisitionViewModel item)
        {
            if (item != null)
            {
                //Get Numeration
                bool autoGenId = false;
                Configuração conf = DBConfigurations.GetById(1);
                int entityNumerationConfId = conf.NumeraçãoRequisições.Value;

                if (item.RequisitionNo == "" || item.RequisitionNo == null)
                {
                    autoGenId = true;
                    item.RequisitionNo = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, autoGenId);
                }
                if (item.RequisitionNo != null)
                {
                    item.CreateUser = User.Identity.Name;
                    item.State = RequisitionStates.Validated;
                    var createdItem = DBRequest.Create(item.ParseToDB());
                    if (createdItem != null)
                    {
                        item = createdItem.ParseToViewModel();
                        if (autoGenId)
                        {
                            ConfiguraçãoNumerações configNum = DBNumerationConfigurations.GetById(entityNumerationConfId);
                            configNum.ÚltimoNºUsado = item.RequisitionNo;
                            configNum.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(configNum);
                        }
                        item.eReasonCode = 1;
                        item.eMessage = "Registo criado com sucesso.";
                    }
                    else
                    {
                        item = new RequisitionViewModel();
                        item.eReasonCode = 2;
                        item.eMessage = "Ocorreu um erro ao criar o registo.";
                    }
                }
                else
                {
                    item.eReasonCode = 5;
                    item.eMessage = "A numeração configurada não é compativel com a inserida.";
                }
            }
            else
            {
                item = new RequisitionViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: o modelo de requisição não pode ser nulo.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult UpdateRequisition([FromBody] RequisitionViewModel item)
        {
            if (item != null)
            {
                item.CreateUser = User.Identity.Name;
                var updatedItem = DBRequest.Update(item.ParseToDB());
                if (updatedItem != null)
                {
                    item = updatedItem.ParseToViewModel();
                    item.eReasonCode = 1;
                    item.eMessage = "Registo atualizado com sucesso.";
                }
                else
                {
                    item = new RequisitionViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao atualizar o registo.";
                }
            }
            else
            {
                item = new RequisitionViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: a requisição não pode ser nula.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult DeleteRequisition([FromBody] RequisitionViewModel item)
        {
            if (item != null)
            {
                if (DBRequest.Delete(item.ParseToDB()))
                {
                    item.eReasonCode = 1;
                    item.eMessage = "Registo eliminado com sucesso.";
                }
                else
                {
                    item = new RequisitionViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao eliminar o registo.";
                }
            }
            else
            {
                item = new RequisitionViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: a requisição não pode ser nula.";
            }
            return Json(item);
        }

        public JsonResult GetPlaces([FromBody] int placeId)
        {
            PlacesViewModel PlacesData = DBPlaces.ParseToViewModel(DBPlaces.GetById(placeId));
            return Json(PlacesData);
        }

        [HttpPost]
        
        public JsonResult ValidateNumeration([FromBody] RequisitionViewModel item)
        {
            //Get Project Numeration
            Configuração conf = DBConfigurations.GetById(1);
            if (conf != null)
            {
                int numConfigId = conf.NumeraçãoRequisições.Value;

                ConfiguraçãoNumerações numConf = DBNumerationConfigurations.GetById(numConfigId);

                //Validate if id is valid
                if (!(item.RequisitionNo == "" || item.RequisitionNo == null) && !numConf.Manual.Value)
                {
                    return Json("A numeração configurada para as requisições não permite inserção manual.");
                }
                else if (item.RequisitionNo == "" && !numConf.Automático.Value)
                {
                    return Json("É obrigatório inserir o Nº Requisição.");
                }
            }
            else
            {
                return Json("Não foi possivel obter as configurações base de numeração.");
            }
            return Json("");
        }

        [HttpPost]
        
        public JsonResult GetApprovedRequisitions()
        {
            List<RequisitionViewModel> result = DBRequest.GetByState(RequisitionStates.Approved).ParseToViewModel();

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CenterResponsibilityCode));
            return Json(result);
        }
        [HttpPost]
        
        public JsonResult GetValidatedRequisitions()
        {
            List<RequisitionStates> states = new List<RequisitionStates>()
            {
                RequisitionStates.Validated,
                RequisitionStates.Available,
                RequisitionStates.Received,
                RequisitionStates.Treated,
            };
            List<RequisitionViewModel> result = DBRequest.GetByState(states).ParseToViewModel();

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CenterResponsibilityCode));
            return Json(result);
        }
        [HttpPost]
        
        public JsonResult GetAllRequisitionshistoric()
        {
            List<RequisitionViewModel> result = DBRequest.GetByState(RequisitionStates.Archived).ParseToViewModel();

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CenterResponsibilityCode));

            return Json(result);
        }
        [HttpPost]
        
        public JsonResult GetRequisition([FromBody] Newtonsoft.Json.Linq.JObject requestParams)
        {
            string requisitionId = string.Empty;
            int status = -1;

            if (requestParams != null)
            {
                requisitionId = requestParams["requisitionId"].ToString();
                status = int.Parse(requestParams["status"].ToString());
            }

            bool statusIsValid = Configurations.EnumHelper.ValidateRange(typeof(RequisitionStates), status);

            RequisitionViewModel item;
            if (!string.IsNullOrEmpty(requisitionId) && requisitionId != "0" && statusIsValid)
            {
                item = DBRequest.GetById(requisitionId).ParseToViewModel();
            }
            else
                item = new RequisitionViewModel();

            return Json(item);
        }

        [HttpPost]
        
        public JsonResult CreateRequisitionLine([FromBody] RequisitionLineViewModel item)
        {
            if (item != null)
            {
                item.CreateUser = User.Identity.Name;
                var createdItem = DBRequestLine.Create(item.ParseToDB());
                if (createdItem != null)
                {
                    item = createdItem.ParseToViewModel();
                    item.eReasonCode = 1;
                    item.eMessage = "Registo criado com sucesso.";
                }
                else
                {
                    item = new RequisitionLineViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao criar o registo.";
                }
            }
            else
            {
                item = new RequisitionLineViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult UpdateRequisitionLines([FromBody] RequisitionViewModel item)
        {
            try
            {
                if (item != null && item.Lines != null)
                {
                    if (DBRequestLine.Update(item.Lines.ParseToDB()))
                    {
                        item.Lines.ForEach(x => x.Selected = false);
                        item.eReasonCode = 1;
                        item.eMessage = "Linhas atualizadas com sucesso.";
                        return Json(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //item.eReasonCode = 2;
                //item.eMessage = "Ocorreu um erro ao atualizar as linhas.";
            }
            item.eReasonCode = 2;
            item.eMessage = "Ocorreu um erro ao atualizar as linhas.";
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult DeleteRequisitionLine([FromBody] RequisitionLineViewModel item)
        {
            if (item != null)
            {
                if (DBRequestLine.Delete(item.ParseToDB()))
                {
                    item.eReasonCode = 1;
                    item.eMessage = "Registo eliminado com sucesso.";
                }
                else
                {
                    item = new RequisitionLineViewModel();
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao eliminar o registo.";
                }
            }
            else
            {
                item = new RequisitionLineViewModel();
                item.eReasonCode = 2;
                item.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(item);
        }


        [HttpPost]
        
        public JsonResult RegistByType([FromBody] RequisitionViewModel item, string registType)
        {
            if (item != null)
            {
                item.eReasonCode = 1;
                string quantityInvalid = "";
                string prodNotStockkeepUnit = "";
                string prodQuantityOverStock = "";
                string ReqLineNotCreateDP = "";
                int ReqLinesToHistCount = 0;
                switch (registType)
                {
                    case "Disponibilizar":
                        if (item.State == RequisitionStates.Validated)
                        {
                            List<RequisitionLineViewModel> getrlines = DBRequestLine.GetAllByRequisiçãos(item.RequisitionNo).ParseToViewModel();
                            List<NAVStockKeepingUnitViewModel> StockkeepingUnit = new List<NAVStockKeepingUnitViewModel>();
                            foreach (RequisitionLineViewModel rlines in getrlines)
                            {
                                StockkeepingUnit = DBNAV2017StockKeepingUnit.GetByProductsNo(_config.NAVDatabaseName, _config.NAVCompanyName, rlines.Code).ToList();
                                if (StockkeepingUnit.Count == 0)
                                {
                                    if (prodNotStockkeepUnit == "")
                                    {
                                        prodNotStockkeepUnit = rlines.Description;
                                    }
                                    else
                                    {
                                        prodNotStockkeepUnit = prodNotStockkeepUnit + " , " + rlines.Description;
                                    }
                                }
                                else
                                {
                                    foreach (NAVStockKeepingUnitViewModel VAR in StockkeepingUnit)
                                    {
                                        if (VAR.SafetyStockQuantity < rlines.QuantityToProvide)
                                        {
                                            if (prodQuantityOverStock == "")
                                            {
                                                prodQuantityOverStock = rlines.Description;
                                            }
                                            else
                                            {
                                                prodQuantityOverStock =
                                                    prodQuantityOverStock + " , " + rlines.Description;
                                            }
                                        }
                                        else
                                        {
                                            rlines.QuantityAvailable = rlines.QuantityAvailable + rlines.QuantityToProvide;
                                            rlines.QuantityReceivable = rlines.QuantityToProvide;
                                            rlines.UpdateUser = User.Identity.Name;
                                            rlines.UpdateDateTime = DateTime.Now;
                                            RequisitionLineViewModel rlinesValidation = DBRequestLine.Update(rlines.ParseToDB()).ParseToViewModel();
                                            if (rlinesValidation == null)
                                            {
                                                item.eReasonCode = 5;
                                                item.eMessage = "Ocorreu um erro ao alterar as linhas de requisição";
                                            }
                                            else
                                            {
                                                item.State = RequisitionStates.Available;
                                                item.UpdateUser = User.Identity.Name;
                                                item.UpdateDate = DateTime.Now;
                                                RequisitionViewModel rValidation = DBRequest.Update(item.ParseToDB()).ParseToViewModel();
                                                if (rValidation == null)
                                                {
                                                    item.eReasonCode = 9;
                                                    item.eMessage = "Ocorreu um erro ao alterar a requisição";
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            if (prodNotStockkeepUnit != "" && prodQuantityOverStock != "")
                            {
                                item.eReasonCode = 6;
                                item.eMessage = " Os seguintes produtos não  existem nas unidades de armazenamento do NAV: " + prodNotStockkeepUnit +
                                ". Os seguintes têm quantidades a disponibilizar superiores ao stock: " + prodQuantityOverStock + ".";
                            }
                            else if (prodNotStockkeepUnit != "" && prodQuantityOverStock == "")
                            {
                                item.eReasonCode = 7;
                                item.eMessage = " Os seguintes produtos não existem nas unidades de armazenamento do NAV: " + prodNotStockkeepUnit + ".";
                            }
                            else if (prodNotStockkeepUnit == "" && prodQuantityOverStock != "")
                            {
                                item.eReasonCode = 8;
                                item.eMessage = " Os seguintes têm quantidades a disponibilizar superiores ao stock: " + prodQuantityOverStock + ".";
                            }
                        }
                        else
                        {
                            item.eReasonCode = 3;
                            item.eMessage = "Esta requisição não está validada.";
                        }
                        if (item.eReasonCode == 1)
                        {
                            item.eMessage = "A Requisição está disponivel";
                        }
                        break;
                    case "Receber":
                        if (item.State == RequisitionStates.Validated)
                        {
                            List<RequisitionLineViewModel> getrlines = DBRequestLine.GetAllByRequisiçãos(item.RequisitionNo).ParseToViewModel();
                            List<NAVStockKeepingUnitViewModel> StockkeepingUnit = new List<NAVStockKeepingUnitViewModel>();
                            foreach (RequisitionLineViewModel rlines in getrlines)
                            {
                                if (rlines.QuantityReceivable > 0)
                                {
                                    StockkeepingUnit = DBNAV2017StockKeepingUnit.GetByProductsNo(_config.NAVDatabaseName, _config.NAVCompanyName, rlines.Code).ToList();
                                    if (StockkeepingUnit.Count == 0)
                                    {
                                        if (prodNotStockkeepUnit == "")
                                        {
                                            prodNotStockkeepUnit = rlines.Description;
                                        }
                                        else
                                        {
                                            prodNotStockkeepUnit = prodNotStockkeepUnit + " , " + rlines.Description;
                                        }
                                    }
                                    else
                                    {
                                        foreach (NAVStockKeepingUnitViewModel VAR in StockkeepingUnit)
                                        {
                                            if (VAR.SafetyStockQuantity < rlines.QuantityReceivable)
                                            {
                                                if (prodQuantityOverStock == "")
                                                {
                                                    prodQuantityOverStock = rlines.Description;
                                                }
                                                else
                                                {
                                                    prodQuantityOverStock =
                                                        prodQuantityOverStock + " , " + rlines.Description;
                                                }
                                            }
                                            else
                                            {
                                                rlines.QuantityReceived = rlines.QuantityReceived + rlines.QuantityReceivable;
                                                rlines.QuantityPending = rlines.QuantityReceivable;
                                                rlines.UpdateUser = User.Identity.Name;
                                                rlines.UpdateDateTime = DateTime.Now;
                                                RequisitionLineViewModel rlinesValidation = DBRequestLine.Update(rlines.ParseToDB()).ParseToViewModel();
                                                if (rlinesValidation == null)
                                                {
                                                    item.eReasonCode = 5;
                                                    item.eMessage = "Ocorreu um erro ao alterar as linhas de requisição";
                                                }
                                                else
                                                {
                                                    item.State = RequisitionStates.Received;
                                                    item.UpdateUser = User.Identity.Name;
                                                    item.UpdateDate = DateTime.Now;
                                                    RequisitionViewModel rValidation = DBRequest.Update(item.ParseToDB()).ParseToViewModel();
                                                    if (rValidation == null)
                                                    {
                                                        item.eReasonCode = 9;
                                                        item.eMessage = "Ocorreu um erro ao alterar a requisição";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (quantityInvalid == "")
                                    {
                                        quantityInvalid = rlines.Description;
                                    }
                                    else
                                    {
                                        quantityInvalid = quantityInvalid + " , " + rlines.Description;
                                    }
                                }
                            }
                            if (quantityInvalid != "")
                            {
                                item.eReasonCode = 12;
                                item.eMessage = "Os produtos" + quantityInvalid +
                                                "têm a quantidade a Receber a 0";
                            }
                            else
                            {
                                if (prodNotStockkeepUnit != "")
                                {
                                    item.eReasonCode = 7;
                                    item.eMessage = " O produtos " + prodNotStockkeepUnit +
                                                    " não existem nas unidades de armazenamento do NAV";
                                }
                                else
                                {
                                    foreach (RequisitionLineViewModel RLItem in getrlines)
                                    {
                                        DiárioDeProjeto newRL = new DiárioDeProjeto();
                                        DiárioDeProjeto newdp = new DiárioDeProjeto()
                                        {
                                            NºProjeto = item.ProjectNo,
                                            Data = DateTime.Now,
                                            Tipo = RLItem.Type,
                                            Código = RLItem.Code,
                                            Descrição = RLItem.Description,
                                            Quantidade = RLItem.QuantityReceivable,
                                            CódUnidadeMedida = RLItem.UnitMeasureCode,
                                            CódLocalização = item.LocalCode,
                                            CódigoRegião = item.RegionCode,
                                            CódigoÁreaFuncional = item.FunctionalAreaCode,
                                            CódigoCentroResponsabilidade = item.CenterResponsibilityCode,
                                            Utilizador =User.Identity.Name,
                                            CustoUnitário = RLItem.UnitCost,
                                            PreçoUnitário = RLItem.UnitCostsould,
                                            Faturável = RLItem.Billable,
                                            NºRequisição = item.RequisitionNo,
                                            NºLinhaRequisição = RLItem.LineNo,
                                            AcertoDePreços = false,
                                            FaturaçãoAutorizada = false,
                                            NºFuncionário = item.EmployeeNo,
                                            Registado =false,
                                            Faturada = false,
                                        };
                                        newdp.DataHoraCriação = DateTime.Now;
                                        newdp.UtilizadorCriação = User.Identity.Name;
                                        newRL = DBProjectDiary.Create(newdp);
                                        if (newRL == null)
                                        {
                                            if (ReqLineNotCreateDP == "")
                                            {
                                                ReqLineNotCreateDP = item.RequisitionNo;
                                            }
                                            else
                                            {
                                                ReqLineNotCreateDP = ReqLineNotCreateDP + " , " + item.RequisitionNo;
                                            }
                                        }
                                        else
                                        {
                                            if ((RLItem.QuantityRequired - RLItem.QuantityReceived) == 0 )
                                            {
                                                ReqLinesToHistCount++;
                                            }
                                        }
                                    }
                                    if (ReqLineNotCreateDP != "")
                                    {
                                        item.eReasonCode = 15;
                                        item.eMessage =
                                            "As linhas " + ReqLineNotCreateDP +
                                            " não passaram para os diarios de projeto";
                                    }
                                    else
                                    {
                                        if (ReqLinesToHistCount == getrlines.Count)
                                        {
                                            item.State = RequisitionStates.Archived;
                                            item.UpdateUser = User.Identity.Name;
                                            item.UpdateDate = DateTime.Now;
                                            RequisitionViewModel reqtoArchived = DBRequest.Update(item.ParseToDB()).ParseToViewModel();
                                            if (reqtoArchived == null)
                                            {
                                                item.eReasonCode = 14;
                                                item.eMessage = "Ocorreu um erro ao mandar para o histórico";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            item.eReasonCode = 11;
                            item.eMessage = "A requisição não está disponível.";
                        }
                        if (item.eReasonCode == 1)
                        {
                            item.eMessage = "A Requisição foi recebida.";
                        }
                        break;
                    case "Anular Aprovacao":
                        if (item.State == RequisitionStates.Approved)
                        {
                            item.State = RequisitionStates.Pending;
                            item.ResponsibleApproval = "";
                            item.ApprovalDate = null;
                            item.UpdateUser = User.Identity.Name;
                            item.UpdateDate = DateTime.Now;
                            RequisitionViewModel reqPend = DBRequest.Update(item.ParseToDB()).ParseToViewModel();
                            if (reqPend != null)
                            {
                                List<RequisitionLineViewModel> getrlines = DBRequestLine.GetAllByRequisiçãos(item.RequisitionNo).ParseToViewModel();
                                foreach (RequisitionLineViewModel rlines in getrlines)
                                {
                                    rlines.QuantityRequired = null;
                                    rlines.UpdateUser = User.Identity.Name;
                                    rlines.UpdateDateTime = DateTime.Now;
                                    RequisitionLineViewModel rlinesValidation = DBRequestLine.Update(rlines.ParseToDB()).ParseToViewModel();
                                    if (rlinesValidation == null)
                                    {
                                        item.eReasonCode = 5;
                                        item.eMessage = "Ocorreu um erro ao alterar as linhas de requisição";
                                    }
                                }
                            }
                            else
                            {
                                item.eReasonCode = 4;
                                item.eMessage = "Ocorreu um erro ao anular a aprovação da requisição";
                            }
                        }
                        else
                        {
                            item.eReasonCode = 2;
                            item.eMessage = "A requisição não está aprovada";
                        }
                        if (item.eReasonCode == 1)
                        {
                            item.eMessage = "A Aprovação foi anulada";
                        }
                        break;
                    case "Anular Validacao":
                        if (item.State == RequisitionStates.Validated)
                        {
                            item.State = RequisitionStates.Approved;
                            item.ResponsibleValidation = "";
                            item.ValidationDate = null;
                            item.UpdateUser = User.Identity.Name;
                            item.UpdateDate = DateTime.Now;
                            RequisitionViewModel reqAprov = DBRequest.Update(item.ParseToDB()).ParseToViewModel();
                            if (reqAprov != null)
                            {
                                List<RequisitionLineViewModel> getrlines = DBRequestLine.GetAllByRequisiçãos(item.RequisitionNo).ParseToViewModel();
                                foreach (RequisitionLineViewModel rlines in getrlines)
                                {
                                    rlines.QuantityAvailable = null;
                                    rlines.UpdateUser = User.Identity.Name;
                                    rlines.UpdateDateTime = DateTime.Now;
                                    RequisitionLineViewModel rlinesValidation = DBRequestLine.Update(rlines.ParseToDB()).ParseToViewModel();
                                    if (rlinesValidation == null)
                                    {
                                        item.eReasonCode = 5;
                                        item.eMessage = "Ocorreu um erro ao alterar as linhas de requisição";
                                    }
                                }
                            }
                            else
                            {
                                item.eReasonCode = 4;
                                item.eMessage = "Ocorreu um erro ao anular a aprovação da requisição";
                            }
                        }
                        else
                        {
                            item.eReasonCode = 3;
                            item.eMessage = "Esta requisição não está validada";
                        }
                        if (item.eReasonCode == 1)
                        {
                            item.eMessage = "A Validação foi anulada";
                        }
                        break;
                    case "Fechar Requisicao":
                        item.State = RequisitionStates.Archived;
                        item.UpdateUser = User.Identity.Name;
                        item.UpdateDate = DateTime.Now;
                        RequisitionViewModel reqArchived = DBRequest.Update(item.ParseToDB()).ParseToViewModel();
                        if (reqArchived == null)
                        {
                            item.eReasonCode = 14;
                        item.eMessage = "Ocorreu Um erro ao fechar";
                        }
                        if (item.eReasonCode == 1)
                        {
                            item.eMessage = "Requisição foi fechada";
                        }
                        break;
                    default:
                        item.eReasonCode = 10;
                        item.eMessage = "Ocorreu um erro: existe algum problema com esta requisição";
                        break;
                }
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult ValidateLocalMarketForRequisition([FromBody] RequisitionViewModel item)
        {
            if (item != null)
            {
                try
                {
                    RequisitionService serv = new RequisitionService(_configws, HttpContext.User.Identity.Name);
                    item = serv.ValidateLocalMarketFor(item);
                }
                catch (Exception ex)
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao criar encomenda de compra (" + ex.Message + ")";
                }
            }
            else
            {
                item = new RequisitionViewModel()
                {
                    eReasonCode = 3,
                    eMessage = "Não é possivel validar o mercado local. A requisição não pode ser nula."
                };
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult ValidateRequisition([FromBody] RequisitionViewModel item)
        {
            if (item != null)
            {
                try
                {
                    RequisitionService serv = new RequisitionService(_configws, HttpContext.User.Identity.Name);
                    item = serv.ValidateRequisition(item);
                }
                catch (Exception ex)
                {
                    item.eReasonCode = 3;
                    item.eMessage = "Ocorreu um erro ao validar a requisição (" + ex.Message + ")";
                }
            }
            else
            {
                item = new RequisitionViewModel()
                {
                    eReasonCode = 3,
                    eMessage = "Não é possivel validar. A requisição não pode ser nula."
                };
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult CreateMarketConsult([FromBody] RequisitionViewModel item)
        {
            if (item != null)
            {
                try
                {
                    RequisitionService serv = new RequisitionService(_configws, HttpContext.User.Identity.Name);
                    serv.CreateMarketConsultFor(item);
                }
                catch (NotImplementedException ex)
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Funcionalidade não implementada";
                }
            }
            else
            {
                item = new RequisitionViewModel()
                {
                    eReasonCode = 3,
                    eMessage = "Não é possivel criar consulta de mercado. A requisição não pode ser nula."
                };
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult CreatePurchaseOrder([FromBody] RequisitionViewModel item)
        {
            if (item != null)
            {
                try
                {
                    RequisitionService serv = new RequisitionService(_configws, HttpContext.User.Identity.Name);
                    item = serv.CreatePurchaseOrderFor(item);
                }
                catch (Exception ex)
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao criar encomenda de compra (" + ex.Message + ")";
                }
            }
            else
            {
                item = new RequisitionViewModel()
                {
                    eReasonCode = 3,
                    eMessage = "Não é possivel criar encomenda de compra. A requisição não pode ser nula."
                };
            }
            return Json(item);
        }

        //
        //public IActionResult CreateTransferShipment(string id)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(id))
        //        {
        //            RequisitionService serv = new RequisitionService(_configws, HttpContext.User.Identity.Name);
        //            GenericResult result = serv.CreateTransferShipmentFor(id);
        //            if (result.CompletedSuccessfully)
        //            {
        //                byte[] fileContents = Convert.FromBase64String(result.ResultValue);
        //                Request.HttpContext.Response.Headers.Add("content-disposition", "filename=GuiaTransporte.pdf");
        //                return File(fileContents, "application/pdf");
        //            }
        //        }
        //    }
        //    catch { }
        //    return RedirectToAction("InternalServerError", "Error", new { area = "" });
        //}
        
        
        public JsonResult CreateTransferShipment([FromBody] Newtonsoft.Json.Linq.JObject requestParams)
        {
            string requisitionId = string.Empty;

            if (requestParams != null)
            {
                requisitionId = requestParams["requisitionId"].ToString();
            }
            /*
             * Test
             
            var createTransferShipResult = new FileActionResult()
            {
                eReasonCode = 1,
                Base64FileContent = "JVBERi0xLjMKMSAwIG9iago8PCAvVHlwZSAvQ2F0YWxvZwovT3V0bGluZXMgMiAwIFIKL1BhZ2VzIDMgMCBSID4+CmVuZG9iagoyIDAgb2JqCjw8IC9UeXBlIC9PdXRsaW5lcyAvQ291bnQgMCA+PgplbmRvYmoKMyAwIG9iago8PCAvVHlwZSAvUGFnZXMKL0tpZHMgWzYgMCBSCl0KL0NvdW50IDEKL1Jlc291cmNlcyA8PAovUHJvY1NldCA0IDAgUgovRm9udCA8PCAKL0YxIDggMCBSCj4+Cj4+Ci9NZWRpYUJveCBbMC4wMDAgMC4wMDAgNjEyLjAwMCA3OTIuMDAwXQogPj4KZW5kb2JqCjQgMCBvYmoKWy9QREYgL1RleHQgXQplbmRvYmoKNSAwIG9iago8PAovQ3JlYXRvciAoRE9NUERGKQovQ3JlYXRpb25EYXRlIChEOjIwMTUwNzIwMTMzMzIzKzAyJzAwJykKL01vZERhdGUgKEQ6MjAxNTA3MjAxMzMzMjMrMDInMDAnKQo+PgplbmRvYmoKNiAwIG9iago8PCAvVHlwZSAvUGFnZQovUGFyZW50IDMgMCBSCi9Db250ZW50cyA3IDAgUgo+PgplbmRvYmoKNyAwIG9iago8PCAvRmlsdGVyIC9GbGF0ZURlY29kZQovTGVuZ3RoIDY2ID4+CnN0cmVhbQp4nOMy0DMwMFBAJovSuZxCFIxN9AwMzRTMDS31DCxNFUJSFPTdDBWMgKIKIWkKCtEaIanFJZqxCiFeCq4hAO4PD0MKZW5kc3RyZWFtCmVuZG9iago4IDAgb2JqCjw8IC9UeXBlIC9Gb250Ci9TdWJ0eXBlIC9UeXBlMQovTmFtZSAvRjEKL0Jhc2VGb250IC9UaW1lcy1Cb2xkCi9FbmNvZGluZyAvV2luQW5zaUVuY29kaW5nCj4+CmVuZG9iagp4cmVmCjAgOQowMDAwMDAwMDAwIDY1NTM1IGYgCjAwMDAwMDAwMDggMDAwMDAgbiAKMDAwMDAwMDA3MyAwMDAwMCBuIAowMDAwMDAwMTE5IDAwMDAwIG4gCjAwMDAwMDAyNzMgMDAwMDAgbiAKMDAwMDAwMDMwMiAwMDAwMCBuIAowMDAwMDAwNDE2IDAwMDAwIG4gCjAwMDAwMDA0NzkgMDAwMDAgbiAKMDAwMDAwMDYxNiAwMDAwMCBuIAp0cmFpbGVyCjw8Ci9TaXplIDkKL1Jvb3QgMSAwIFIKL0luZm8gNSAwIFIKPj4Kc3RhcnR4cmVmCjcyNQolJUVPRgo=",
            };
            */
            var createTransferShipResult = new FileActionResult()
            {
                eReasonCode = 2,
                eMessage = "Ocorreu um erro ao criar a guia de transporte."
            };

            try
            {
                if (!string.IsNullOrEmpty(requisitionId))
                {
                    RequisitionService serv = new RequisitionService(_configws, HttpContext.User.Identity.Name);
                    GenericResult result = serv.CreateTransferShipmentFor(requisitionId);
                    if (result.CompletedSuccessfully)
                    {
                        createTransferShipResult.Base64FileContent = result.ResultValue;
                        createTransferShipResult.eReasonCode = 1;
                    }
                    else
                    {
                        createTransferShipResult.eMessages.Add(new TraceInformation(TraceType.Error, result.ErrorMessage));
                    }
                }
            }
            catch { }

            return Json(createTransferShipResult);
        }

        [HttpPost]
        
        public JsonResult SendPrePurchase([FromBody] RequisitionViewModel item)
        {
            if (item != null)
            {
                try
                {
                    RequisitionService serv = new RequisitionService(_configws, HttpContext.User.Identity.Name);
                    item = serv.SendPrePurchaseFor(item);
                }
                catch (Exception ex)
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao enviar pré-encomenda (" + ex.Message + ")";
                }
            }
            else
            {
                item = new RequisitionViewModel()
                {
                    eReasonCode = 2,
                    eMessage = "Não é possivel enviar a pré-compra. A requisição não pode ser nula."
                };
            }
            return Json(item);
        }

        [HttpPost]
        
        public JsonResult SubmitForApproval([FromBody] Newtonsoft.Json.Linq.JObject requestParams)
        {
            string requisitionId = string.Empty;

            if (requestParams != null)
            {
                requisitionId = requestParams["requisitionId"].ToString();
            }
            
            ErrorHandler result = new ErrorHandler();
            if (!string.IsNullOrEmpty(requisitionId))
            {
                Requisição reqDB = DBRequest.GetById(requisitionId);
                var requisition = reqDB.ParseToViewModel();
                
                if (requisition != null)
                {
                    var totalValue = requisition.GetTotalValue();
                    result = ApprovalMovementsManager.StartApprovalMovement(1, requisition.FunctionalAreaCode, requisition.CenterResponsibilityCode, requisition.RegionCode, totalValue, requisitionId, User.Identity.Name);
                }
            }
            else
            {
                result = new ErrorHandler()
                {
                    eReasonCode = 2,
                    eMessage = "O código da requisição é nulo ou vazio.",
                };
            }
            return Json(result);
        }


        #region Pontos de Situação
        
        public IActionResult PontosSituacao()
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.Requisições);

            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;

                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        
        public IActionResult PontoSituacaoRequisicao([FromQuery] string reqId,[FromQuery] string lineId)
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.Requisições);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                ViewBag.RequisitionNo = reqId;
                ViewBag.AutoOpenDialogOnLineNo = lineId;

                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        [HttpPost]
        
        public JsonResult GetStatesOfPlay(string id)
        {
            List<StateOfPlayViewModel> items;
            if (string.IsNullOrEmpty(id))
            {
                items = DBStateOfPlay.GetAll()
                    .ParseToViewModel()
                    .OrderBy(x => x.Read)
                    .ToList();
            }
            else
            { 
                items = DBStateOfPlay.GetForRequisition(id)
                    .ParseToViewModel()
                    .OrderBy(x => x.Read)
                    .ToList();
            }
            return Json(items);
        }

        [HttpPost]
        
        public JsonResult ConfirmStateOfPlayReading([FromBody] StateOfPlayViewModel item)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 2;
            result.eMessage = "Ocorreu um erro ao confirmar a leitura.";

            if (item != null)
            {
                item.Read = true;
                var updatedItem = DBStateOfPlay.Update(item.ParseToDB());
                if (updatedItem != null)
                {
                    item = updatedItem.ParseToViewModel();
                    result.eReasonCode = 1;
                    result.eMessage = "Confirmação de leitura efetuada com sucesso.";
                }
            }

            return Json(result);
        }

        [HttpPost]
        
        public JsonResult AddStateOfPlay([FromBody] StateOfPlayViewModel item)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 2;
            result.eMessage = "Ocorreu um erro ao adicionar o ponto de situação.";

            if (item != null)
            {
                item.QuestionDate = DateTime.Now;
                item.QuestionedBy = User.Identity.Name;
                item.Read = false;
                var createdItem = DBStateOfPlay.Create(item.ParseToDB());
                if (createdItem != null)
                {
                    item = createdItem.ParseToViewModel();
                    result.eReasonCode = 1;
                    result.eMessage = "Ponto de situação criado com sucesso.";
                }
            }
            item.eReasonCode = result.eReasonCode;
            item.eMessage = result.eMessage;

            return Json(item);
        }

        [HttpPost]
        
        public JsonResult SendResponse([FromBody] StateOfPlayViewModel item)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 2;
            result.eMessage = "Ocorreu um erro ao adicionar o ponto de situação.";

            if (item != null)
            {
                item.AnswerDate = DateTime.Now;
                item.AnsweredBy = User.Identity.Name;
                item.Answer = item.Answer;

                var createdItem = DBStateOfPlay.Update(item.ParseToDB());
                if (createdItem != null)
                {
                    item = createdItem.ParseToViewModel();
                    
                    SMTPEmailSender mailSender = new SMTPEmailSender();
                    string htmlTemplateMessage = "Caro utilizador,<br /><br />Foram adicionados os comentários ao seu pedido de ponto de situação da requisição {0} ({1}):<br />\"<i>{2}</i>\"<h3>Comentários</h3><p>{3}</p>";
                    string emailBody = string.Format(htmlTemplateMessage, item.RequisitionNo, item.QuestionDate.ToShortDateString(), item.Question, item.Answer);

                    var sendMailResult = mailSender.SendMailAsync(User.Identity.Name, item.QuestionedBy, "Resposta a pedido de ponto de situação", emailBody);
                    if (sendMailResult.IsCompletedSuccessfully)
                    {
                        result.eReasonCode = 1;
                        result.eMessage = "Resposta enviada com sucesso.";
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não foi possível enviar email ao utilizador que criou o pedido (" + item.QuestionedBy + ")";
                    }
                }
            }
            item.eReasonCode = result.eReasonCode;
            item.eMessage = result.eMessage;

            return Json(item);
        }
        #endregion

        
        public IActionResult HistoricoRequisicoes()
        {
            UserAccessesViewModel userPermissions =
                DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.HistóricoRequisições);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        public JsonResult GetHistoryReq()
        {
            List<Requisição> requisition = null;
            List<RequisitionStates> states = new List<RequisitionStates>()
            {
                RequisitionStates.Archived,
            };
            requisition = DBRequest.GetReqByUserAreaStatus(User.Identity.Name, 0, states);

            List<RequisitionViewModel> result = new List<RequisitionViewModel>();

            requisition.ForEach(x => result.Add(x.ParseToViewModel()));

            return Json(result);
        }

        public JsonResult GetHistoryReqLines([FromBody] JObject requestParams)
        {
            string ReqNo = requestParams["ReqNo"].ToString();

            List<LinhasRequisição> RequisitionLines = null;
            RequisitionLines = DBRequestLine.GetAllByRequisiçãos(ReqNo);

            List<RequisitionLineViewModel> result = new List<RequisitionLineViewModel>();

            RequisitionLines.ForEach(x => result.Add(DBRequestLine.ParseToViewModel(x)));
            return Json(result);

        }

        public JsonResult GetStateDescription([FromBody] int id)
        {
            string stateDescription = EnumHelper.GetDescriptionFor(typeof(RequisitionStates), id);
            return Json(stateDescription);
        }
    }
}