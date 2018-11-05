using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.GuiaTransporte;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Request;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
 using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class GuiasTransporteNavController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;
        public GuiasTransporteNavController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        [HttpGet]
        // GET: GuiaTransporteNav
        public ActionResult Index()
        {
            //TO DO: define permission to access GuiasTransporte
            return View();
        }

        // GET: GuiaTransporteNav/Details/5
        [HttpGet]
        public ActionResult GuiaTransporteDetails(string id, bool isHistoric = false)
        {
            ViewBag.No = id ?? "";
            ViewBag.reportServerURL = _config.ReportServerURL;

            if (isHistoric == true)
            {
                ViewBag.Historic = "(Histórico) ";
                ViewBag.ifHistoric = true;
            }
            else
            {
                ViewBag.Historic = "";
                ViewBag.ifHistoric = false;
            }

            return View();
        }

        [HttpPost]
        public JsonResult GetListGuiasTransporteNav([FromBody] JObject requestParams)
        {
            bool historic = requestParams["Historic"] == null ? false : bool.Parse(requestParams["Historic"].ToString());
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            List<GuiaTransporteNavViewModel> result = DBNAV2017GuiasTransporte.GetListByDim(_config.NAVDatabaseName, _config.NAVCompanyName, userDimensions, historic);
            return Json(result);
        }

    }
}