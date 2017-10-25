using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return View();
        }
    }
}