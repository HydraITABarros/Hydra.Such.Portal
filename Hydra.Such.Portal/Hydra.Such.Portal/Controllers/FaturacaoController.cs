using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class FaturacaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RececaoFaturas()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Nutrição, Enumerations.Features.ReceçãoFaturação);
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
    }
}