using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Nutrition;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Such.Portal.Areas.Nutricao.Controllers
{
    public class RequisicoesSimplificadasController : Controller
    {
        [Area("Nutricao")]
        public IActionResult Index()
        {
            return View();
        }

        [Area("Nutricao")]
        [HttpPost]
        public JsonResult GetSimplifiedRequisitions()
        {
            List<SimplifiedRequisitionViewModel> result = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.GetByCreateResponsible(User.Identity.Name));
            return Json(result);
        }

        [Area("Nutricao")]
        public IActionResult Detalhes(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 40);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.RequestNo = id;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [Area("Nutricao")]
        [HttpPost]
        public JsonResult GetSimplifiedRequisitionData([FromBody] SimplifiedRequisitionViewModel item)
        {
            SimplifiedRequisitionViewModel result = new SimplifiedRequisitionViewModel();
            if (item != null && !string.IsNullOrEmpty(item.RequisitionNo))
                result = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.GetById(item.RequisitionNo));
            return Json(result);
        }

        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido
        // 102 - 
        [Area("Nutricao")]
        [HttpPost]
        public JsonResult CreateSimplifiedRequisition([FromBody] SimplifiedRequisitionViewModel item)
        {
            SimplifiedRequisitionViewModel result = new SimplifiedRequisitionViewModel();
            if (item != null)
            {
                result.CreateUser = User.Identity.Name;
                result = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.Create(DBSimplifiedRequisitions.ParseToDatabase(item)));

                if (result != null)
                {
                    result.eReasonCode = 100;
                    result.eMessage = "Requisição Simplificada criada com sucesso.";
                }
                else
                {
                    result.eReasonCode = 101;
                    result.eMessage = "Ocorreu um erro ao criar a Requisição Simplificada.";
                }
            }
            return Json(result);
        }

        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido
        [Area("Nutricao")]
        [HttpPost]
        public JsonResult UpdateSimplifiedRequisition([FromBody] SimplifiedRequisitionViewModel item)
        {
            SimplifiedRequisitionViewModel result = new SimplifiedRequisitionViewModel();

            try
            {
                if (item != null)
                {
                    RequisiçõesSimplificadas CLocation = DBSimplifiedRequisitions.GetById(item.RequisitionNo);
                    CLocation.Estado = item.Status;
                    CLocation.DataHoraRequisição = item.RequisitionDate != "" && item.RequisitionDate != null ? DateTime.Parse(item.RequisitionDate) : (DateTime?)null;
                    CLocation.DataRegisto = item.RegistrationDate != "" && item.RegistrationDate != null ? DateTime.Parse(item.RegistrationDate) : (DateTime?)null;
                    CLocation.CódLocalização = item.LocationCode;
                    CLocation.CódigoRegião = item.RegionCode;
                    CLocation.CódigoÁreaFuncional = item.FunctionalAreaCode;
                    CLocation.CódigoCentroResponsabilidade = item.ResponsabilityCenterCode;
                    CLocation.NºProjeto = item.ProjectNo;
                    CLocation.DataHoraAprovação = item.ApprovalDate != "" && item.ApprovalDate != null ? DateTime.Parse(item.ApprovalDate) : (DateTime?)null;
                    CLocation.DataHoraEnvio = item.ShipDate != "" && item.ShipDate != null ? DateTime.Parse(item.ShipDate) : (DateTime?)null;
                    CLocation.DataHoraDisponibilização = item.AvailabilityDate != "" && item.AvailabilityDate != null ? DateTime.Parse(item.AvailabilityDate) : (DateTime?)null;
                    CLocation.ResponsávelCriação = item.CreateResponsible;
                    CLocation.ResponsávelAprovação = item.ApprovalResponsible;
                    CLocation.ResponsávelEnvio = item.ShipResponsible;
                    CLocation.ResponsávelReceção = item.ReceiptResponsible;
                    CLocation.Imprimir = item.Print;
                    CLocation.Anexo = item.Atach;
                    CLocation.NºFuncionário = item.EmployeeNo;
                    CLocation.Urgente = item.Urgent;
                    CLocation.NºUnidadeProdutiva = item.ProductivityNo;
                    CLocation.Observações = item.Observations;
                    CLocation.Terminada = item.Finished;
                    CLocation.ResponsávelVisar = item.AimResponsible;
                    CLocation.DataHoraVisar = item.AimDate != "" && item.AimDate != null ? DateTime.Parse(item.AimDate) : (DateTime?)null;
                    CLocation.Autorizada = item.Authorized;
                    CLocation.ResponsávelAutorização = item.AuthorizedResponsible;
                    CLocation.DataHoraAutorização = item.AuthorizedDate != "" && item.AuthorizedDate != null ? DateTime.Parse(item.AuthorizedDate) : (DateTime?)null;
                    CLocation.Visadores = item.Visor;
                    CLocation.DataReceçãoLinhas = item.ReceiptLinesDate;
                    CLocation.RequisiçãoNutrição = item.NutritionRequisition;
                    CLocation.DataReceçãoEsperada = item.ReceiptPreviewDate != "" && item.ReceiptPreviewDate != null ? DateTime.Parse(item.ReceiptPreviewDate) : (DateTime?)null;
                    CLocation.RequisiçãoModelo = item.ModelRequisition;
                    CLocation.DataHoraModificação = DateTime.Now;
                    CLocation.UtilizadorModificação = User.Identity.Name;

                    if (CLocation.DataHoraRequisição != null)
                    {
                        CLocation.DataHoraRequisição = CLocation.DataHoraRequisição.Value.Date;
                        CLocation.DataHoraRequisição = CLocation.DataHoraRequisição.Value.Add(TimeSpan.Parse(item.RequisitionTime));
                    }

                    if (CLocation.DataHoraAprovação != null)
                    {
                        CLocation.DataHoraAprovação = CLocation.DataHoraAprovação.Value.Date;
                        CLocation.DataHoraAprovação = CLocation.DataHoraAprovação.Value.Add(TimeSpan.Parse(item.ApprovalTime));
                    }

                    if (CLocation.DataHoraEnvio != null)
                    {
                        CLocation.DataHoraEnvio = CLocation.DataHoraEnvio.Value.Date;
                        CLocation.DataHoraEnvio = CLocation.DataHoraEnvio.Value.Add(TimeSpan.Parse(item.ShipTime));
                    }

                    if (CLocation.DataHoraDisponibilização != null)
                    {
                        CLocation.DataHoraDisponibilização = CLocation.DataHoraDisponibilização.Value.Date;
                        CLocation.DataHoraDisponibilização = CLocation.DataHoraDisponibilização.Value.Add(TimeSpan.Parse(item.AvailabilityTime));
                    }

                    if (CLocation.DataHoraVisar != null)
                    {
                        CLocation.DataHoraVisar = CLocation.DataHoraVisar.Value.Date;
                        CLocation.DataHoraVisar = CLocation.DataHoraVisar.Value.Add(TimeSpan.Parse(item.AimTime));
                    }

                    if (CLocation.DataHoraAutorização != null)
                    {
                        CLocation.DataHoraAutorização = CLocation.DataHoraAutorização.Value.Date;
                        CLocation.DataHoraAutorização = CLocation.DataHoraAutorização.Value.Add(TimeSpan.Parse(item.AuthorizedTime));
                    }


                    result = DBSimplifiedRequisitions.ParseToViewModel(DBSimplifiedRequisitions.Update(CLocation));

                    if (result != null)
                    {
                        result.eReasonCode = 100;
                        result.eMessage = "Requisição Simplificada atualizada com sucesso.";
                    }
                    else
                    {
                        result.eReasonCode = 101;
                        result.eMessage = "Ocorreu um erro ao atualizar a Requisição Simplificada.";
                    }
                }
            }
            catch (Exception)
            {
                result.eReasonCode = 101;
                result.eMessage = "Ocorreu um erro ao atualizar a Requisição Simplificada.";
            }
            
            return Json(result);
        }

        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido
        [Area("Nutricao")]
        [HttpPost]
        public JsonResult DeleteSimplifiedRequisition([FromBody] SimplifiedRequisitionViewModel item)
        {
            SimplifiedRequisitionViewModel result = new SimplifiedRequisitionViewModel();
            if (item != null)
            {
                RequisiçõesSimplificadas CLocation = DBSimplifiedRequisitions.GetById(item.RequisitionNo);

                if (DBSimplifiedRequisitions.Delete(CLocation))
                {
                    result.eReasonCode = 100;
                    result.eMessage = "Requisição Simplificada removida com sucesso.";
                }
                else
                {
                    result.eReasonCode = 101;
                    result.eMessage = "Ocorreu um erro ao remover a Requisição Simplificada.";
                }
            }
            return Json(result);
        }

    }
}