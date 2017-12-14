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

namespace Hydra.Such.Portal.Areas.Compras.Controllers
{
    public class GestaoRequisicoesController : Controller
    {
        [Area("Compras")]
        public IActionResult Index()
        {
            return View();
        }
        [Area("Compras")]
        public IActionResult Detalhes()
        {
            return View();
        }

        [Area("Compras")]
        public IActionResult RequisicoesAprovadas()
        {
            return View();
        }

        [Area("Compras")]
        public IActionResult DetalhesReqAprovada(string id)
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 10, 4);

            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                ViewBag.RequisitionId = id;
                
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        
        [Area("Compras")]
        public IActionResult LinhasRequisicao()
        {
            return View();
        }

        [HttpPost]
        [Area("Compras")]
        public JsonResult GridRequestLineValues()
        {
            List<RequisitionLineViewModel> result = DBRequestLine.GetAll().ParseToViewModel();
            //Apply User Dimensions Validations
            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (CUserDimensions.Where(y => y.Dimensão == 1).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 1 && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (CUserDimensions.Where(y => y.Dimensão == 2).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 2 && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (CUserDimensions.Where(y => y.Dimensão == 3).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 3 && y.ValorDimensão == x.CenterResponsibilityCode));
            return Json(result);
        }

        [HttpPost]
        [Area("Compras")]
        public JsonResult CreateAndDeleteRequestLine([FromBody] List<RequisitionLineViewModel> dp)
        {
            List<LinhasRequisição> previousList;
            // Get All
            previousList = DBRequestLine.GetAll();
            foreach (LinhasRequisição line in previousList)
            {
                if (!dp.Any(x => x.LineNo == line.NºLinha))
                {
                    DBRequestLine.Delete(line);
                }
            }
            //Update or Create
            try
            {
                dp.ForEach(x =>
                {
                    List<LinhasRequisição> dpObject = DBRequestLine.GetByLineNo(x.LineNo);

                    if (dpObject.Count > 0)
                    {
                        LinhasRequisição newdp = dpObject.FirstOrDefault();
                          newdp.NºRequisição= x.RequestNo;
                          newdp.NºLinha = x.LineNo;
                          newdp.Tipo = 2;
                          newdp.Código= x.Code;
                          newdp.Descrição = x.Description;
                          newdp.CódigoUnidadeMedida= x.UnitMeasureCode;
                          newdp.CódigoLocalização= x.LocalCode;
                          newdp.MercadoLocal= x.LocalMarket;
                          newdp.QuantidadeARequerer= x.QuantityToRequire;
                          newdp.QuantidadeRequerida= x.QuantityRequired;
                          newdp.QuantidadeADisponibilizar= x.QuantityToProvide;
                          newdp.QuantidadeDisponibilizada= x.QuantityAvailable;
                          newdp.QuantidadeAReceber= x.QuantityReceivable;
                          newdp.QuantidadeRecebida= x.QuantityReceived;
                          newdp.QuantidadePendente= x.QuantityPending;
                          newdp.CustoUnitário= x.UnitCost;
                          newdp.DataReceçãoEsperada= string.IsNullOrEmpty(x.ExpectedReceivingDate) ? (DateTime?)null : DateTime.Parse(x.ExpectedReceivingDate) ;
                          newdp.Faturável = x.Billable;
                          newdp.NºProjeto= x.ProjectNo;
                          newdp.CódigoRegião= x.RegionCode;
                          newdp.CódigoÁreaFuncional= x.FunctionalAreaCode;
                          newdp.CódigoCentroResponsabilidade= x.CenterResponsibilityCode;
                          newdp.NºFuncionário  = x.FunctionalNo;
                          newdp.Viatura  = x.Vehicle;
                          newdp.DataHoraCriação  = x.CreateDateTime;
                          newdp.UtilizadorCriação = x.CreateUser;
                          newdp.DataHoraModificação  = x.UpdateDateTime;
                          newdp.UtilizadorModificação  = x.UpdateUser;
                          newdp.QtdPorUnidadeDeMedida  = x.QtyByUnitOfMeasure;
                          newdp.PreçoUnitárioVenda  = x.UnitCostsould;
                          newdp.ValorOrçamento  = x.BudgetValue;
                          newdp.NºLinhaOrdemManutenção  = x.MaintenanceOrderLineNo;
                          newdp.CriarConsultaMercado  = x.CreateMarketSearch;
                          newdp.EnviarPréCompra  = x.SubmitPrePurchase;
                          newdp.EnviadoPréCompra = x.SendPrePurchase;
                          newdp.DataMercadoLocal = string.IsNullOrEmpty(x.LocalMarketDate) ? (DateTime?)null : DateTime.Parse(x.LocalMarketDate) ;
                          newdp.UserMercadoLocal  = x.LocalMarketUser;
                          newdp.EnviadoParaCompras  = x.SendForPurchase;
                          newdp.DataEnvioParaCompras  = string.IsNullOrEmpty(x.SendForPurchaseDate) ? (DateTime?)null : DateTime.Parse(x.SendForPurchaseDate) ;
                          newdp.ValidadoCompras  = x.PurchaseValidated;
                          newdp.RecusadoCompras = x.PurchaseRefused;
                          newdp.MotivoRecusaMercLocal = x.ReasonToRejectionLocalMarket;
                          newdp.DataRecusaMercLocal  = string.IsNullOrEmpty(x.RejectionLocalMarketDate) ? (DateTime?)null : DateTime.Parse(x.RejectionLocalMarketDate) ;
                          newdp.IdCompra = x.PurchaseId;
                          newdp.NºFornecedor = x.SupplierNo;
                          newdp.NºEncomendaAberto  = x.OpenOrderNo;
                          newdp.NºLinhaEncomendaAberto  = x.OpenOrderLineNo;
                          newdp.NºDeConsultaMercadoCriada = x.QueryCreatedMarketNo;
                          newdp.NºEncomendaCriada = x.CreatedOrderNo;
                          newdp.CódigoProdutoFornecedor  = x.SupplierProductCode;
                          newdp.UnidadeProdutivaNutrição  = x.UnitNutritionProduction;
                          newdp.RegiãoMercadoLocal  = x.MarketLocalRegion;
                          newdp.NºCliente  = x.CustomerNo;
                          newdp.Aprovadores  = x.Approvers;
                        DBRequestLine.Update(newdp);
                    }
                    else
                    {
                        LinhasRequisição newdp = new LinhasRequisição()
                        {
                            NºRequisição = x.RequestNo,
                            NºLinha = x.LineNo,
                            Tipo = 2,
                            Código = x.Code,
                            Descrição = x.Description,
                            CódigoUnidadeMedida = x.UnitMeasureCode,
                            CódigoLocalização = x.LocalCode,
                            MercadoLocal = x.LocalMarket,
                            QuantidadeARequerer = x.QuantityToRequire,
                            QuantidadeRequerida = x.QuantityRequired,
                            QuantidadeADisponibilizar = x.QuantityToProvide,
                            QuantidadeDisponibilizada = x.QuantityAvailable,
                            QuantidadeAReceber = x.QuantityReceivable,
                            QuantidadeRecebida = x.QuantityReceived,
                            QuantidadePendente = x.QuantityPending,
                            CustoUnitário = x.UnitCost,
                            DataReceçãoEsperada = string.IsNullOrEmpty(x.ExpectedReceivingDate) ? (DateTime?)null : DateTime.Parse(x.ExpectedReceivingDate),
                            Faturável = x.Billable,
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
                            CriarConsultaMercado = x.CreateMarketSearch,
                            EnviarPréCompra = x.SubmitPrePurchase,
                            EnviadoPréCompra = x.SendPrePurchase,
                            DataMercadoLocal = string.IsNullOrEmpty(x.LocalMarketDate) ? (DateTime?)null : DateTime.Parse(x.LocalMarketDate),
                            UserMercadoLocal = x.LocalMarketUser,
                            EnviadoParaCompras = x.SendForPurchase,
                            DataEnvioParaCompras = string.IsNullOrEmpty(x.SendForPurchaseDate) ? (DateTime?)null : DateTime.Parse(x.SendForPurchaseDate) ,
                            ValidadoCompras = x.PurchaseValidated,
                            RecusadoCompras = x.PurchaseRefused,
                            MotivoRecusaMercLocal = x.ReasonToRejectionLocalMarket,
                            DataRecusaMercLocal = string.IsNullOrEmpty(x.RejectionLocalMarketDate) ? (DateTime?)null : DateTime.Parse(x.RejectionLocalMarketDate) ,
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
            return Json(dp);
        }

        [HttpPost]
        [Area("Compras")]
        public JsonResult GetApprovedRequisitions()
        {
            List<RequisitionViewModel> result = DBRequest.GetByState(3).ParseToViewModel();
                
            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(y => y.Dimensão == 1).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == 1 && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (userDimensions.Where(y => y.Dimensão == 2).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == 2 && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (userDimensions.Where(y => y.Dimensão == 3).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == 3 && y.ValorDimensão == x.CenterResponsibilityCode));
            return Json(result);
        }
        
        [HttpPost]
        [Area("Compras")]
        public JsonResult GetRequisition([FromBody] Newtonsoft.Json.Linq.JObject requestParams)
        {
            string requisitionId = string.Empty;
            int status = -1;

            if (requestParams != null)
            {
                requisitionId = requestParams["requisitionId"].ToString();
                status = int.Parse(requestParams["status"].ToString());
            }
            
            RequisitionViewModel item;
            if (!string.IsNullOrEmpty(requisitionId) && requisitionId != "0" && status > -1)
            {
                item = DBRequest.GetById(requisitionId).ParseToViewModel();

                //Ensure correct status
                if(item == null || item.State != status)
                    item = new RequisitionViewModel();
            }
            else
                item = new RequisitionViewModel();

            return Json(item);
        }

        [HttpPost]
        [Area("Compras")]
        public JsonResult ValidateLocalMarketForRequisition([FromBody] RequisitionViewModel item)
        {
            item.eMessage = "Funcionalidade não implementada";
            item.eReasonCode = 0;

            return Json(item);
        }

        [HttpPost]
        [Area("Compras")]
        public JsonResult ValidateRequisition([FromBody] RequisitionViewModel item)
        {
            item.eMessage = "Funcionalidade não implementada";
            item.eReasonCode = 0;

            return Json(item);
        }
    }
}