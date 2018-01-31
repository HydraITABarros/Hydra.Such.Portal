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

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ComprasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RececaoMercadorias()
        {
            return View();
        }

        public IActionResult RececaoFaturas()
        {
            return View();
        }

        public IActionResult Administracao()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 10, 18);
            if (UPerm != null && UPerm.Read.Value)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult GestaoRequisicoes_Index(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 6);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Id = id ?? "";
                ViewBag.UPermissions = UPerm;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult ProcedimentosCCP_Index(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 3, 6);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.id = id ?? "";
                ViewBag.UPermissions = UPerm;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #region Pré-Requisições

        public IActionResult PreRequisicoesLista()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 10, 3);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Area = 10;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult PreRequisicoesDetalhes(string PreRequesitionNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 10, 3);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Area = 10;
                ViewBag.PreRequesitionNo = User.Identity.Name;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        #region Pending Requesitions
        public IActionResult RequisicoesPendentes()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 10, 0);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Area = 10;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        #endregion
    }
}