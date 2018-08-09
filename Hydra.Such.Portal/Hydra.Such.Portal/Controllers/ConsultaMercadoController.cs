using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.PedidoCotacao;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.PedidoCotacao;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Controllers
{
    public class ConsultaMercadoController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ConsultaMercadoController(IHostingEnvironment _hostingEnvironment)
        {
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult ConsultaMercado()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.PedidoCotacao);

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

        [HttpPost]
        public JsonResult GetAllConsultaMercado()
        {
            List<ConsultaMercado> result = DBConsultaMercado.GetAllConsultaMercadoToList();
            List<ConsultaMercadoView> list = new List<ConsultaMercadoView>();

            foreach (ConsultaMercado cm in result)
            {
                list.Add(DBConsultaMercado.CastConsultaMercadoToView(cm));
            }

            //return Json(result);
            return Json(list);
        }

    }
}