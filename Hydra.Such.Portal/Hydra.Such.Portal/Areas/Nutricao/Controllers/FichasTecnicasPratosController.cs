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
    public class FichasTecnicasPratosController : Controller
    {
        [Area("Nutricao")]
        public IActionResult Detalhes()
        {

            UserAccessesViewModel userPermissions =  DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 40);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetAllRecTecPlatesBYPlate([FromBody]string plateNo)
        {
            List<RecordTechnicalOfPlatesModelView> result = DBRecordTechnicalOfPlates.GetByPlateNo(plateNo).ParseToViewModel();
            return Json(result);
        }
        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetAllRecTecPlates()
        {
            List<RecordTechnicalOfPlatesModelView> result = DBRecordTechnicalOfPlates.GetAll().ParseToViewModel();
            return Json(result);
        }
        [HttpPost]
        [Area("Nutricao")]
        public JsonResult GetAllProceduresConfection([FromBody]string plateNo)
        {
            List<ProceduresConfectionViewModel> result = ProceduresConfection.GetAllbyPlateNo(plateNo).ParseToViewModel();
            return Json(result);
        }
        [Area("Nutricao")]
        public IActionResult FichaTecnica(string id)
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 40);
            if (userPermissions != null && userPermissions.Read.Value)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.RecTecPlatesId = id;
                }
                else
                {
                    ViewBag.RecTecPlatesId = "";
                }

                ViewBag.UPermissions = userPermissions;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        [Area("Nutricao")]
        public JsonResult CreateRecordTechnicalOfPlates([FromBody] RecordTechnicalOfPlatesModelView data)
        {
            if (data != null)
            {
                Configuração Configs = DBConfigurations.GetById(1);
                int NumerationConfigurationId = 0;
                NumerationConfigurationId = Configs.NumeraçãoFichasTécnicasDePratos.Value;
                data.PlateNo = DBNumerationConfigurations.GetNextNumeration(NumerationConfigurationId,
                    (data.PlateNo == "" || data.PlateNo == null));
                data.CreateUser = User.Identity.Name;
                var createdItem = DBRecordTechnicalOfPlates.Create(data.ParseToDB());
                if (createdItem != null)
                {
                    data = createdItem.ParseToViewModel();
                    data.eReasonCode = 1;
                    data.eMessage = "Registo criado com sucesso.";
                }
                else
                {
                    data = new RecordTechnicalOfPlatesModelView();
                    data.eReasonCode = 2;
                    data.eMessage = "Ocorreu um erro ao criar o registo.";
                }
            }
            else
            {
                data = new RecordTechnicalOfPlatesModelView();
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro: a linha não pode ser nula.";
            }
            return Json(data);
        }
    }
}