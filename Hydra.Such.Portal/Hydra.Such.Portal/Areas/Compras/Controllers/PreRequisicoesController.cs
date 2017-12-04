using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Such.Portal.Areas.Compras.Controllers
{
    public class PreRequisicoesController : Controller
    {
        [Area("Compras")]
        public IActionResult Index()
        {
            return View();
        }
    }
}