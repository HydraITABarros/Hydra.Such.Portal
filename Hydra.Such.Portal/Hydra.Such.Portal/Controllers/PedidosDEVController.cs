using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Database;
using Hydra.Such.Data;

namespace Hydra.Such.Portal.Controllers
{
    public class PedidosDEVController : Controller
    {
        public IActionResult PedidosDEV_List()
        {
            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminPedidosDEV);

            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !userPerm.Create.Value;
                ViewBag.UpdatePermissions = !userPerm.Update.Value;
                ViewBag.DeletePermissions = !userPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult PedidosDEV(string id)
        {
            ViewBag.NoPedidoDEV = id;

            UserAccessesViewModel userPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AdminPedidosDEV);
            if (userPerm != null && userPerm.Read.Value)
            {
                ViewBag.CreatePermissions = !userPerm.Create.Value;
                ViewBag.UpdatePermissions = !userPerm.Update.Value;
                ViewBag.DeletePermissions = !userPerm.Delete.Value;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetListPedidosDEV()
        {
            List<PedidosDEVViewModel> result = DBPedidosDEV.GetAll().ParseToViewModel();

            return Json(result);
        }

        [HttpPost]
        public JsonResult DeletePedidosDEV([FromBody] PedidosDEVViewModel data)
        {
            int result = 0;
            try
            {
                if (DBPedidosDEV.Delete(data.ID) == true)
                    result = 1;
            }
            catch
            {
                result = 99;
            }
            return Json(result);
        }




    }
}
