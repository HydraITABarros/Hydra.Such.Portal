using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;

namespace Hydra.Such.Portal.Controllers {

    [Authorize]
    public class InternacionalizacoesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Projetos
        public IActionResult Projetos()
        {
            return View();
        }

        public IActionResult DetalhesProjeto(string id)
        {
            ViewBag.ProjectNo = id == null ? "" : id;
            return View();
        }
        #endregion

        #region Contratos
        public IActionResult Contratos(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.ParseToViewModel(DBUserAccesses.GetByUserId(User.Identity.Name).Where(x => x.Área == 9 && x.Funcionalidade == 2).FirstOrDefault());

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesContrato(string id, string version)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.ParseToViewModel(DBUserAccesses.GetByUserId(User.Identity.Name).Where(x => x.Área == 9 && x.Funcionalidade == 2).FirstOrDefault());
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.UPermissions = DBUserAccesses.ParseToViewModel(DBUserAccesses.GetByUserId(User.Identity.Name).Where(x => x.Área == 9 && x.Funcionalidade == 2).FirstOrDefault());
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion


        public IActionResult Administracao()
        {
            return View();
        }
    }
}