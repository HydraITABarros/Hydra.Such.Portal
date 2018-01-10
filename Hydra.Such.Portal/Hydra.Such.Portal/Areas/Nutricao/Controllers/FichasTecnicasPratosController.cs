using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public JsonResult GetAllRecTecPlates([FromBody]string plateNo)
        {
            List<RecordTechnicalOfPlatesModelView> result = DBRecordTechnicalOfPlates.GetByPlateNo(plateNo).ParseToViewModel();
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
      
    }
}