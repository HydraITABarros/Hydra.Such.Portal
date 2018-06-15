using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Portal.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Hydra.Such.Data;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class MovimentoProdutosController : Controller
    {
        private readonly NAVConfigurations _config;
        public MovimentoProdutosController(IOptions<NAVConfigurations> appSettings)
        {
            _config = appSettings.Value;
        }

        public IActionResult Index(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.DiárioProjeto);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public bool SetSessionMovimentoProduto([FromBody] StockkeepingUnitViewModel data)
        {
            if (data.ProductNo != null)
            {
                HttpContext.Session.SetString("productNo", data.ProductNo);
                HttpContext.Session.SetString("codLocation", data.Code);
                return true;
            }
            return false;
        }
    }
}