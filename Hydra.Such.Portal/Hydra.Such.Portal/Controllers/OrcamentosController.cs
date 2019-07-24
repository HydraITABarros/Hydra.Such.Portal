using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Such.Portal.Controllers
{
    public class OrcamentosController : Controller
    {
        public IActionResult Orcamentos_List()
        {
            return View();
        }

        public IActionResult Orcamentos_Details()
        {
            return View();
        }
    }
}