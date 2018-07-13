using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic.Telemoveis;
using Microsoft.AspNetCore.Mvc;
using static Hydra.Such.Data.Enumerations;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data.ViewModel.Telemoveis;

namespace Hydra.Such.Portal.Controllers
{
    public class TelemoveisEquipamentosController : Controller
    {
        public IActionResult TelemoveisEquipamentos()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Telemoveis);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult Detalhe(int tipo, string imei)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Telemoveis);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.tipo = tipo;
                ViewBag.imei = imei;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetAllEquipamentos()
        {
            List<TelemoveisEquipamentos> result = DBTelemoveis.GetAllTelemoveisEquipamentosToList();
            List<TelemoveisEquipamentosView> list = new List<TelemoveisEquipamentosView>();

            foreach (TelemoveisEquipamentos tel in result)
            {
                list.Add(DBTelemoveis.CastTelemoveisEquipamentosToView(tel));
            }

            //return Json(result);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetEquipamentosPorTipo([FromBody] JObject requestParams)
        {
            int tipo = int.Parse(requestParams["tipo"].ToString());

            List<TelemoveisEquipamentos> result = DBTelemoveis.GetAllTelemoveisEquipamentosTypeToList(tipo);

            return Json(result);
        }
    }
}