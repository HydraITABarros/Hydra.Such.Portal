﻿using System;
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
using Hydra.Such.Data.ViewModel.Compras;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class GuiasTransporteNavController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GeneralConfigurations _generalConfig;

        public GuiasTransporteNavController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
            _generalConfig = appSettingsGeneral.Value;
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
                ViewBag.filtroData = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
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
            string filtroData = requestParams["filtroData"] == null ? "2017-01-01" : (string)requestParams["filtroData"];
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            List<GuiaTransporteNavViewModel> result = DBNAV2017GuiasTransporte.GetListByDim(
                _config.NAVDatabaseName, 
                _config.NAVCompanyName, 
                userDimensions, 
                historic, 
                DateTime.Parse(filtroData));

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
        public JsonResult GetListGuiasTransporteNavToCopyFrom([FromBody] JObject reqParam)
        {
            DateTime filtroData;

            try
            {
                filtroData = reqParam["filtroData"] == null ? DateTime.Parse("2017-01-01") : DateTime.Parse((string)reqParam["filtroData"]);
            }
            catch (Exception ex)
            {

                filtroData = DateTime.Parse("2017-01-01");
            }


            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            List<GuiaTransporteNavViewModel> result = DBNAV2017GuiasTransporte.GetListByDim(
                _config.NAVDatabaseName, 
                _config.NAVCompanyName, 
                userDimensions, 
                true,
               filtroData);

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
                result.RegionCode = Project.RegionCode ?? "";
                result.FunctionalAreaCode = Project.AreaCode ?? "";
                result.ResponsabilityCenterCode = Project.CenterResponsibilityCode ?? "";
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
        public JsonResult GetRequisicoes([FromBody] JObject reqParam)
        {
            DateTime filtroData = DateTime.Parse("2017-01-01");
            try
            {
                filtroData = reqParam["filtroData"] == null ? DateTime.Parse("2017-01-01") : DateTime.Parse((string)reqParam["filtroData"]);
            }
            catch (Exception ex)
            {

                filtroData = DateTime.Parse("2017-01-01");
            }
            

            List<RequisitionStates> states = new List<RequisitionStates>()
            {
              RequisitionStates.Approved,
              RequisitionStates.Archived,
              RequisitionStates.Available,
              RequisitionStates.Consulta,
              RequisitionStates.Encomenda,
              RequisitionStates.Pending,
              RequisitionStates.Received,
              RequisitionStates.Treated,
              RequisitionStates.Validated
            };

            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);

            List<RequisitionViewModel> requisitions = DBRequest.GetByState(
                                                    states, 
                                                    userDimensions, 
                                                    _config.NAVDatabaseName, 
                                                    _config.NAVCompanyName)
                                                .Where(r => r.DataHoraCriação >= filtroData)
                                                .ToList()
                                                .ParseToViewModel();

            return Json(requisitions);
        }

        [HttpPost]
        public JsonResult CopyRequsitionLines([FromBody] JObject requestParams)
        {
            string noGuia = requestParams["noGuia"] == null ? "" : (string)requestParams["noGuia"];
            string requisitionNo = requestParams["requisitionNo"] == null ? "" : (string)requestParams["requisitionNo"];
            int lastLineNo = requestParams["lastLineNo"] == null ? 0 : (int)requestParams["lastLineNo"];

            if (noGuia == "" || requisitionNo == "")
                return Json(false);

            bool result = DBNAV2017GuiasTransporte.CreateLinhasFromRequisitionNo(noGuia, requisitionNo, lastLineNo);

            return Json(result);
        }

        [HttpPost]
        public JsonResult CopyLinhasGuiaTransporte([FromBody] JObject requestParams)
        {
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);

            string noGuia = requestParams["noGuia"] == null ? "" : (string)requestParams["noGuia"];
            string noGuiaToCopyFrom = requestParams["noGuiaToCopyFrom"] == null ? "" : (string)requestParams["noGuiaToCopyFrom"];
            

            if (noGuia == "" || noGuiaToCopyFrom == "")
                return Json(null);


            try
            {
                var copyLinesTask = WSGuiasTransporteNAV.CopyLinesAsync(noGuiaToCopyFrom, noGuia, _configws);
                var result = copyLinesTask.Result;

                if (result == null)
                    return Json(null);

                bool isInt = Int32.TryParse(result.return_value, out int num);

                if (isInt)
                {
                    return Json(num);
                }
                else
                {
                    return Json(null);
                }
   
            }
            catch (Exception e)
            {

                return Json(null);
            }
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

            List<GuiaTransporteShipToAddress> customerAddresses = DBNAV2017GuiasTransporte.GetShipToAddresses(_config.NAVDatabaseName, _config.NAVCompanyName, custId, shipCode);
            return Json(customerAddresses);
        }

        public JsonResult GetShipItems([FromBody] JObject requestParams)
        {
            if (requestParams == null)
                return Json(null);

            int itemType = (requestParams["itemType"] == null || string.Compare((string)requestParams["itemType"], "") == 0) ? -1 : (int)requestParams["itemType"];
            string itemCode = requestParams["itemCode"] == null ? "-1" : (string)requestParams["itemCode"];

            if (itemType == -1)
                return Json(null);

            List<ShipmentLineItem> items = DBNAV2017GuiasTransporte.GetShipmentItems(_config.NAVDatabaseName, _config.NAVCompanyName, itemType, itemCode);
            return Json(items);
        }

        [HttpPost]
        public JsonResult GetPostCodes()
        {
            List<NAVPostCode> postCodes = DBNAV2017GuiasTransporte.GetNAVPostCodes(_config.NAVDatabaseName, _config.NAVCompanyName);
            return Json(postCodes);
        }

        #region CRUD
        [HttpPost]
        public JsonResult CreateGuiaTransporte()
        {
            try
            {
                var createGuiaTask = WSGuiasTransporteNAV.CreateAsync(User.Identity.Name, _configws);
                createGuiaTask.Wait();

                var result = createGuiaTask.Result;

                if(result.return_value == "0" || createGuiaTask.Result == null)
                {
                    return Json(null);
                }

                return Json(result.return_value);
            }
            catch (Exception ex)
            {

                return Json(null);
            }
            
        }

        [HttpPost]
        public JsonResult UpdateGuia([FromBody] GuiaTransporteNavViewModel data)
        {
            
            //if (data.NifCliente != "" && (data.VATRegistrationNo == null || data.VATRegistrationNo == ""))
            //    data.VATRegistrationNo = data.NifCliente;

            data.CastDateTimeStringPropertiesToDateTime();
                                          
            bool result = DBNAV2017GuiasTransporte.UpdateGuiaTransporte(data);
            return Json(result);
        }


        [HttpPost]
        public JsonResult Register([FromBody] JObject param)
        {
            if (param["noGuia"] == null)
                return Json(null);

            string noGuia = (string)param["noGuia"];

            if (noGuia == "")
                return Json(null);

            try
            {
                var registerGuiaTask = WSGuiasTransporteNAV.RegisterAsync(noGuia, _configws);
                registerGuiaTask.Wait();

                var result = registerGuiaTask.Result;

                if (result == null)
                    return Json(null);


                bool isInt = Int32.TryParse(result.return_value, out int num);
                if (isInt)
                {
                    return Json(num);
                }

                if (result.return_value == "0")
                    return Json(null);

                return Json(result.return_value);
            }
            catch (Exception ex)
            {

                return Json(null);
            }           
        }

        [HttpPost]
        public JsonResult DeleteGuia([FromBody] JObject param)
        {
            if (param["noGuia"] == null)
                return Json(null);

            string noGuia = (string)param["noGuia"];

            if (noGuia == "")
                return Json(null);

            bool result = DBNAV2017GuiasTransporte.DeleteGuiaTransporte(_config.NAVDatabaseName, _config.NAVCompanyName, noGuia);

            return Json(result);
        }

        [HttpPost]
        public JsonResult ForceDocRegistry([FromBody] JObject param)
        {
            if (param["noGuia"] == null)
                return Json(null);

            string noGuia = (string)param["noGuia"];

            if (noGuia == "")
                return Json(null);


            try
            {
                var forceRegisterGuiaTask = WSGuiasTransporteNAV.ForceDocumentRegistry(noGuia, _configws);
                forceRegisterGuiaTask.Wait();

                var result = forceRegisterGuiaTask.Result;

                if (result == null)
                    return Json(null);

                bool isInt = Int32.TryParse(result.return_value, out int num);
                if (isInt)
                {
                    return Json(num);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

                return Json(null);
            }


        }
        [HttpPost]
        public JsonResult CreateShipmentLine([FromBody] JObject data)
        {
            if (data == null)
                return Json(false);

            try
            {
                LinhaGuiaTransporteNavViewModel linhaGT = new LinhaGuiaTransporteNavViewModel()
                {
                    NoGuiaTransporte = (string)data["noGuiaTransporte"],
                    NoLinha = (int)data["noLinha"],
                    Tipo = (int)data["tipo"],
                    No = (string)data["no"],
                    Descricao = (string)data["descricao"],
                    CodUnidadeMedida = (string)data["codUnidadeMedida"],
                    Quantidade = (decimal)data["quantidade"],
                    QuantidadeEnviar = (decimal)data["quantidadeEnviar"],
                    NoProjecto = data["noProjecto"] == null ? "" : (string)data["noProjecto"],
                    UnitCost = (data["unitCost"] == null || string.Compare((string)data["unitCost"], "") == 0) ? 0 : (decimal)data["unitCost"],
                    UnitPrice = (data["unitPrice"] == null || string.Compare((string)data["unitPrice"], "") == 0) ? 0 : (decimal)data["unitPrice"],
                    ShortcutDimension1Code = data["shortcutDimension1code"] == null ? "" : (string)data["shortcutDimension1code"],
                    ShortcutDimension2Code = data["shortcutDimension2Code"] == null ? "" : (string)data["shortcutDimension2Code"],
                    NoCliente = data["noCliente"] == null ? "" : (string)data["noCliente"],
                    DataGuia = (data["dataGuia"] == null || string.Compare((string)data["dataGuia"], "") == 0) ? DateTime.Parse("1900-01-01") : (DateTime)data["dataGuia"],
                    DataEntrega = (data["dataEntrega"] == null || string.Compare((string)data["dataEntrega"], "") == 0) ? DateTime.Parse("1900-01-01") : (DateTime)data["dataEntrega"],
                    TipoTerceiro = data["tipoTerceiro"] == null ? 0 : (int)data["tipoTerceiro"]
                };

                linhaGT.TotalCost = linhaGT.UnitCost * linhaGT.Quantidade;
                linhaGT.TotalPrice = linhaGT.UnitPrice * linhaGT.Quantidade;

                bool result = DBNAV2017GuiasTransporte.CreateLinhasGuiaTransporte(linhaGT);

                return Json(result);
            }
            catch (Exception ex)
            {

                return Json(false);
            }
            

        }

        [HttpPost]
        public JsonResult UpdateShipmentLine([FromBody] LinhaGuiaTransporteNavViewModel line)
        {
            if(line.NoGuiaTransporte == "" || line.NoGuiaTransporte == null)
            {
                return Json(false);
            }

            if(line.NoLinha == 0)
            {
                return Json(false);
            }

            bool result = DBNAV2017GuiasTransporte.UpdateLinhaGuiaTransporte(line);

            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteShipmentLine([FromBody] JObject line)
        {
            if (line == null)
                return Json(false);

            string noGuia = (line["noGuia"] == null || string.Compare((string)line["noGuia"], "") == 0) ? "" : (string)line["noGuia"];
            // When noLinha = 0 we are deleting all document lines
            int noLinha = (line["noLinha"] == null || string.Compare((string)line["noLinha"], "") == 0) ? 0 : (int)line["noLinha"];

            if (noGuia == "")
                return Json(false);

            bool result = DBNAV2017GuiasTransporte.DeleteLinhaGuiaTransporte(_config.NAVDatabaseName, _config.NAVCompanyName, noGuia, noLinha);
            return Json(result);
        }
        #endregion

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_GuiasList([FromBody] List<GuiaTransporteNavViewModel> Lista)
        {
            string sWebRootFolder = _generalConfig.FileUploadFolder + "GuiasTransporte\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Guias Transporte Nav");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Nº");
                row.CreateCell(1).SetCellValue("Nº Guia Original");
                row.CreateCell(2).SetCellValue("Data Guia");
                row.CreateCell(3).SetCellValue("Nº Projecto");
                row.CreateCell(4).SetCellValue("Utilizador");
                row.CreateCell(5).SetCellValue("Nº Cliente");
                row.CreateCell(6).SetCellValue("Nome Cliente");
                row.CreateCell(7).SetCellValue("Nº Requisição");
                row.CreateCell(8).SetCellValue("Região");
                row.CreateCell(9).SetCellValue("Área Funcional");
                row.CreateCell(10).SetCellValue("Email Criação");

                int count = 1;
                foreach (GuiaTransporteNavViewModel item in Lista)
                {
                    row = excelSheet.CreateRow(count);

                    row.CreateCell(0).SetCellValue(item.NoGuiaTransporte);
                    row.CreateCell(1).SetCellValue(item.NoGuiaOriginalInterface);
                    row.CreateCell(2).SetCellValue(item.DataGuia);
                    row.CreateCell(3).SetCellValue(item.NoProjecto);
                    row.CreateCell(4).SetCellValue(item.Utilizador);
                    row.CreateCell(5).SetCellValue(item.NoCliente);
                    row.CreateCell(6).SetCellValue(item.NomeCliente);
                    row.CreateCell(7).SetCellValue(item.NoRequisicao);
                    row.CreateCell(8).SetCellValue(item.GlobalDimension1Code);
                    row.CreateCell(9).SetCellValue(item.GlobalDimension2Code);
                    row.CreateCell(10).SetCellValue(item.UserEmail);

                    count++;
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_GuiasList(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "GuiasTransporte\\" + "tmp\\" + sFileName;
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

    }
}