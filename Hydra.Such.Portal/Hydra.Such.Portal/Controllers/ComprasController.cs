using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


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

        public IActionResult TabelasAuxiliares()
        {
            return View();
        }
    }
}