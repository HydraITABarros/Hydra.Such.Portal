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
    public class TelemoveisCartoesController : Controller
    {
        public IActionResult TelemoveisCartoes()
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

        public IActionResult DetalheTelemoveisCartoes([FromQuery] string numCartao)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Telemoveis);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.numCartao = numCartao;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetAllCartoes()
        {
            List<TelemoveisCartoes> result = DBTelemoveis.GetAllTelemoveisCartoesToList();
            List<TelemoveisCartoesView> list = new List<TelemoveisCartoesView>();

            foreach (TelemoveisCartoes tel in result)
            {
                list.Add(DBTelemoveis.CastTelemoveisCartoesToView(tel));
            }

            return Json(list);
        }


        [HttpPost]
        public JsonResult GetCartoesDetails([FromBody] TelemoveisCartoesView data)
        {
            try
            {
                if (data != null)
                {
                    TelemoveisCartoes telemoveisCartoes = DBTelemoveis.GetTelemoveisCartoes(data.NumCartao);

                    if (telemoveisCartoes != null)
                    {
                        TelemoveisCartoesView telemoveisCartoesView = DBTelemoveis.CastTelemoveisCartoesToView(telemoveisCartoes);

                        return Json(telemoveisCartoes);
                    }

                    return Json(new TelemoveisEquipamentosView());
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return Json(false);
        }







    }
}