using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
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