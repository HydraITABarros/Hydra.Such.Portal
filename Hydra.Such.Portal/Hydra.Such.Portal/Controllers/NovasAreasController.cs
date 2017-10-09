using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Such.Portal.Controllers
{
    public class NovasAreasController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Projetos()
        {
            return View();
        }

        public ActionResult TabelasAuxiliares()
        {
            return View();
        }
    }
}