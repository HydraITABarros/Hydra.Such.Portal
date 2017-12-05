using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.NAV;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Requisition;


namespace Hydra.Such.Portal.Areas.Requisicoes.Controllers
{
    public class GestaoRequisicoesController : Controller
    {
        [Area("Requisicoes")]
        public IActionResult Index()
        {
            return View();
        }

        [Area("Requisicoes")]
        public IActionResult Detalhes()
        {
            return View();
        }
       
        [HttpPost]
        [Area("Requisicoes")]
        public JsonResult GetGridValues()
        {
            List<RequisitionViewModel> result = DBRequest.GetAll().Select(x => new RequisitionViewModel()
            {
                RequisitionNo = x.NºRequisição,
                Area = x.Área,
                State = x.Estado,
                ProjectNo = x.NºProjeto,
                RegionCode = x.CódigoRegião,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                CenterResponsibilityCode = x.CódigoCentroResponsabilidade,
                LocalCode = x.CódigoLocalização,
                EmployeeNo = x.NºFuncionário,
                Vehicle = x.Viatura,
                ReceivedDate = !x.DataReceção.HasValue ? "" : x.DataReceção.Value.ToString("yyyy-MM-dd"),
                Urgent = x.Urgente,
                Sample = x.Amostra,
                Attachment = x.Anexo,
                Immobilized = x.Imobilizado,
                BuyCash = x.CompraADinheiro,
                LocalCollectionCode = x.CódigoLocalRecolha,
                LocalDeliveryCode = x.CódigoLocalEntrega,
                Comments = x.Observações,
                RequestModel = x.ModeloDeRequisição,
                CreateUser = x.UtilizadorCriação,
                CreateDate = x.DataHoraCriação,
                UpdateUser = x.UtilizadorModificação,
                UpdateDate = x.DataHoraModificação,
            }).ToList();
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

        [Area("Requisicoes")]
        public IActionResult LinhasRequisicao()
        {
            return View();
        }

        [HttpPost]
        [Area("Requisicoes")]
        public JsonResult GridRequestLineValues()
        {
            List<RequisitionLineViewModel> result = DBRequestLine.GetAll().Select(x => new RequisitionLineViewModel()
            {
                  RequestNo = x.NºRequisição,
                  LineNo = x.NºLinha,
                  Type = x.Tipo,
                  Code = x.Código,
                  Description = x.Descrição,
                  UnitMeasureCode =x.CódigoUnidadeMedida,
                  LocalCode = x.CódigoLocalização,
                  localMarket = x.MercadoLocal,
                  QuantityToRequire = x.QuantidadeARequerer,
                  QuantityRequired = x.QuantidadeRequerida,
                  QuantityToProvide = x.QuantidadeADisponibilizar,
                  QuantityAvailable = x.QuantidadeDisponibilizada,
                  QuantityReceivable = x.QuantidadeAReceber,
                  QuantityReceived = x.QuantidadeRecebida,
                  QuantityPending = x.QuantidadePendente,
                  UnitCost = x.CustoUnitário,
                  ExpectedReceivingDate = !x.DataReceçãoEsperada.HasValue ? "" : x.DataReceçãoEsperada.Value.ToString("yyyy-MM-dd"),
                  Billable = x.Faturável,
                  ProjectNo = x.NºProjeto,
                  RegionCode = x.CódigoRegião,
                  FunctionalAreaCode = x.CódigoÁreaFuncional,
                  CenterResponsibilityCode = x.CódigoCentroResponsabilidade,
                  FunctionalNo = x.NºFuncionário,
                  Vehicle = x.Viatura,
                  CreateDateTime = x.DataHoraCriação,
                  CreateUser = x.UtilizadorCriação,
                  UpdateDateTime = x.DataHoraModificação,
                  UpdateUser = x.UtilizadorModificação
            }).ToList();
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
    }
}