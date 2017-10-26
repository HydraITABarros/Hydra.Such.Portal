using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.Logic;
using Microsoft.AspNetCore.Authorization;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ApoioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Viaturas()
        {
            return View();
        }

        public IActionResult ImportacaoCustos()
        {
            return View();
        }

        public IActionResult Administracao()
        {
            return View();
        }
    }
}