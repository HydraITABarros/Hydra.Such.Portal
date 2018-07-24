using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic.Encomendas;
using Microsoft.AspNetCore.Mvc;
using static Hydra.Such.Data.Enumerations;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data.ViewModel.Encomendas;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Hydra.Such.Portal.Controllers
{
    public class LinhasPreEncomendaController : Controller
    {
        // GET: LinhasPreEncomenda
        public IActionResult LinhasPreEncomenda()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.PréEncomendas);

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
        public JsonResult GetAllLinhas()
        {
            List<LinhasPreEncomenda> result = DBEncomendas.GetAllLinhasPreEncomendaToList();
            List<LinhasPreEncomendaView> list = new List<LinhasPreEncomendaView>();

            foreach (LinhasPreEncomenda lin in result)
            {
                list.Add(DBEncomendas.CastLinhasPreEncomendaToView(lin));
            }

            return Json(list);
        }

    }
}