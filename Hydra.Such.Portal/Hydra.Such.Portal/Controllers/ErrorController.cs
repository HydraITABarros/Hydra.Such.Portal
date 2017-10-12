using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        public IActionResult InternalServerError()
        {
            return View();
        }

        public IActionResult PageNotFound()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}