using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class JuridicoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessosDisciplinares()
        {
            return View();
        }

        public IActionResult ProcessosInquerito()
        {
            return View();
        }


        public IActionResult Administracao()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 9, 18);
            if (UPerm != null && UPerm.Read.Value)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
    }
}