using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
            return View();
        }

        public IActionResult DetalhesProjeto(string id)
        {
            ViewBag.ProjectNo = id == null ? "" : id;
            return View();
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

        public IActionResult TabelasAuxiliares()
        {
            return View();
        }
    }
}