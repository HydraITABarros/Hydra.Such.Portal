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
using Hydra.Such.Data;
using Hydra.Such.Data.Extensions;
using Hydra.Such.Data.ViewModel.Projects;

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
           
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ImpressaoGuiaTransporteNAV);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ifHistoric = false;
                ViewBag.Historic = " ";
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }

        }

        // GET: GuiaTransporteNav/Details/5
        [HttpGet]
        public ActionResult GuiaTransporteDetails(string id, bool isHistoric = false)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ImpressaoGuiaTransporteNAV);


            if (UPerm!=null && UPerm.Read.Value)
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
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            
        }

        [HttpPost]
        public JsonResult GetListGuiasTransporteNav([FromBody] JObject requestParams)
        {
            bool historic = requestParams["Historic"] == null ? false : bool.Parse(requestParams["Historic"].ToString());
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            List<GuiaTransporteNavViewModel> result = DBNAV2017GuiasTransporte.GetListByDim(_config.NAVDatabaseName, _config.NAVCompanyName, userDimensions, historic);

            if (historic)
            {
                ViewBag.Historic = "(Histórico) ";
                ViewBag.ifHistoric = true;
            }
            else
            {
                ViewBag.Historic = " ";
                ViewBag.ifHistoric = false;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetDetailsGuia([FromBody] JObject requestParams)
        {
            string noGuia = requestParams["No"] == null ? "" : requestParams["No"].ToString();
            bool historic = requestParams["Historic"] == null ? false : bool.Parse(requestParams["Historic"].ToString());
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            if (noGuia != null)
            {
                GuiaTransporteNavViewModel guia = DBNAV2017GuiasTransporte.GetDetailsByNo(_config.NAVDatabaseName, _config.NAVCompanyName, userDimensions, noGuia, historic);

                if(guia == null)
                    return Json(null);

                guia.CastDateTimePropertiesToString();

                return Json(guia);
            }
            else
            {
                return Json(null); ;
            }
        }

        [HttpPost]
        public JsonResult GetThirdPartiesList([FromBody] JObject requestParams)
        {
            if (requestParams == null)
                return Json(null);

            int type = (requestParams["type"] == null || string.Compare((string)requestParams["type"], "") == 0) ? -1 : (int)requestParams["type"];
            if(type == -1)
            {
                return Json(null);
            }

            List<ThirdPartyViewModel> result = DBNAV2017GuiasTransporte.GetThirdParties(_config.NAVDatabaseName, _config.NAVCompanyName, type);

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetThirdPartyDetails([FromBody] JObject requestParams)
        {
            if (requestParams == null)
                return Json(null);

            int type = (requestParams["type"] == null || string.Compare((string)requestParams["type"], "") == 0) ? -1 : (int)requestParams["type"];
            string entityId = requestParams["entityId"] == null ? "" : (string)requestParams["entityId"];

            if (type == -1)
                return Json(null);

            if (entityId == "")
                return Json(null);

            ThirdPartyViewModel result = DBNAV2017GuiasTransporte.GetThirdPartyDetails(_config.NAVDatabaseName, _config.NAVCompanyName, type, entityId);

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectDim([FromBody] string ProjectNo)
        {

            ProjectListItemViewModel result = new ProjectListItemViewModel();

            List<NAVProjectsViewModel> navList = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, ProjectNo).ToList();
            NAVProjectsViewModel Project = navList.Where(x => x.No == ProjectNo).FirstOrDefault();

            if (Project != null)
            {
                result.ProjectNo = ProjectNo;
                result.RegionCode = Project.RegionCode != null ? Project.RegionCode : "";
                result.FunctionalAreaCode = Project.AreaCode != null ? Project.AreaCode : "";
                result.ResponsabilityCenterCode = Project.CenterResponsibilityCode != null ? Project.CenterResponsibilityCode : "";
            }
            else
            {
                result.RegionCode = "";
                result.FunctionalAreaCode = "";
                result.ResponsabilityCenterCode = "";
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetSourceCodes()
        {
            return Json(DBNAV2017GuiasTransporte.GetShipmentSourceCodes(_config.NAVDatabaseName, _config.NAVCompanyName));
        }

        [HttpPost]
        public JsonResult GetShipToAddr([FromBody] JObject requestParams)
        {
            if (requestParams == null)
                return Json(null);

            string custId = requestParams["customerId"] == null ? "" : (string)requestParams["customerId"];
            string shipCode = requestParams["shipToCode"] == null ? "-1" : (string)requestParams["shipToCode"];

            if (shipCode == null)
                shipCode = "-1";

            if (custId == "")
                return Json(null);

            // zpgm.11012019 TO DO: Get the ship-to addresses in the back-end and front-end
            List<GuiaTransporteShipToAddress> customerAddresses = DBNAV2017GuiasTransporte.GetShipToAddresses(_config.NAVDatabaseName, _config.NAVCompanyName, custId, shipCode);
            return Json(customerAddresses);
        }

        #region CRUD
        [HttpPost]
        public JsonResult CreateGuiaTransporte()
        {
            // call webservice and return the id of the the new object 
            //Task<WSSuchNav2017.WSNovaGuiaTransporte_Result> newGuia = WSSuchNav2017.WSNovaGuiaTransporte_Result();
            return Json(null);
        }

        [HttpPost]
        public JsonResult UpdateGuia([FromBody] GuiaTransporteNavViewModel data)
        {
            data.CastDateTimeStringPropertiesToDateTime();
            bool result = DBNAV2017GuiasTransporte.UpdateGuiaTransporte(data);
            return Json(result);
        }
        #endregion


    }
}