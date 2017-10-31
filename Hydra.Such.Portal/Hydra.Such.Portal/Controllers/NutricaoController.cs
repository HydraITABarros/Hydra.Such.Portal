using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class NutricaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Projetos
        public IActionResult Projetos()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 1);
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

        public IActionResult DetalhesProjeto(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 1);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id == null ? "" : id;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        public IActionResult Contratos()
        {
            return View();
        }

        public IActionResult Requisicoes()
        {
            return View();
        }

        public IActionResult FichasTecnicasPratos()
        {
            return View();
        }

        public IActionResult Administracao()
        {
            return View();
        }
    }
}