using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data.Logic.Project;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.NAV;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data;
using static Hydra.Such.Data.Enumerations;
using System.Net;
using Hydra.Such.Data.ViewModel.Clients;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly GeneralConfigurations _generalConfig;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ClientesController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            _generalConfig = appSettingsGeneral.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        #region Clientes
        public IActionResult Index()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Clientes); //4, 47);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesCliente(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Clientes); //4, 47);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.No = id ?? "";
                ViewBag.reportServerURL = _config.ReportServerURL;
                ViewBag.userLogin = User.Identity.Name.ToString();
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        #region List
        [HttpPost]
        public JsonResult Get([FromBody] JObject requestParams)
        {
            //int AreaId = int.Parse(requestParams["areaid"].ToString());
            JToken customerNoValue;
            string customerNo = string.Empty;
            if (requestParams != null)
            {
                if (requestParams.TryGetValue("customerNo", out customerNoValue))
                    customerNo = (string)customerNoValue;
            }
            List<ClientDetailsViewModel> result = new List<ClientDetailsViewModel>();
            result = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, customerNo).Select(c => new ClientDetailsViewModel()
            {
                No = c.No_,
                Name = c.Name,
                Address = c.Address,
                Post_Code = c.PostCode,
                City = c.City,
                Regiao_Cliente = c.Country_RegionCode

            }).ToList();
            /*
            //Apply User Dimensions Validations
            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.ResponsabilityCenterCode));
            */
            return Json(result);
        }

        #endregion

        #region Details

        [HttpPost]
        public JsonResult GetDetails([FromBody] ClientDetailsViewModel data)
        {
            if (data != null && data.No != null)
            {
                var getClientTask = WSCustomerService.GetByNoAsync(data.No, _configws);
                try
                {
                    getClientTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro a obter o cliente no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                ClientDetailsViewModel client = getClientTask.Result;
                if (client != null)
                {
                    return Json(client);
                }

            }
            return Json(false);
        }


        //eReason = 1 -> Sucess
        //eReason = 2 -> Error creating Project on Databse 
        //eReason = 3 -> Error creating Project on NAV 
        //eReason = 4 -> Unknow Error 
        //eReason = 5 -> Error getting Numeration 

        [HttpPost]
        public JsonResult Create([FromBody] ClientDetailsViewModel data)
        {
            
            if (data != null)
            {
                var createClientTask = WSCustomerService.CreateAsync(data, _configws);
                try
                {
                    createClientTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao criar o cliente no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                var result = createClientTask.Result;
                if (result == null)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao criar o cliente no NAV.";
                    return Json(data);
                }

                data.eReasonCode = 1;

                var client = WSCustomerService.MapCustomerNAVToCustomerModel(result.WSCustomer);
                if (client != null)
                {
                    client.eReasonCode = 1;
                    return Json(client);
                }

            }
            return Json(data);
        }
        
        [HttpPost]
        public JsonResult Update([FromBody] ClientDetailsViewModel data)
        {
            if (data != null)
            {
                var updateClientTask = WSCustomerService.UpdateAsync(data, _configws);
                try
                {
                    updateClientTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao atualizar o cliente no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                var result = updateClientTask.Result;
                if (result == null)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao atualizar o cliente no NAV.";
                    return Json(data);
                }

                var client = WSCustomerService.MapCustomerNAVToCustomerModel(result.WSCustomer);
                if (client != null)
                {
                    client.eReasonCode = 1;
                    return Json(client);
                }

            }
            return Json(false);
        }


        [HttpPost]
        public JsonResult Delete([FromBody] ClientDetailsViewModel data)
        {
            if (data != null && data.No != null)
            {
                var deleteClientTask = WSCustomerService.DeleteAsync(data.No, _configws);
                try
                {
                    deleteClientTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro a apagar o cliente no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                var result = deleteClientTask.Result;
                if (result != null)
                {
                    return Json(new ErrorHandler()
                    {
                        eReasonCode = 0,
                        eMessage = "Projeto removido com sucesso."
                    });
                }

            }
            return Json(false);
        }
        #endregion

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_Clientes([FromBody] List<ClientDetailsViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + ".xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Clientes");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["no"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº");
                    Col = Col + 1;
                }
                if (dp["name"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome");
                    Col = Col + 1;
                }
                if (dp["address"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Morada");
                    Col = Col + 1;
                }
                if (dp["post_Code"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Postal");
                    Col = Col + 1;
                }
                if (dp["city"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cidade");
                    Col = Col + 1;
                }
                if (dp["regiao_Cliente"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Região");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ClientDetailsViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["no"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.No);
                            Col = Col + 1;
                        }
                        if (dp["name"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Name);
                            Col = Col + 1;
                        }
                        if (dp["address"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Address);
                            Col = Col + 1;
                        }
                        if (dp["post_Code"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Post_Code);
                            Col = Col + 1;
                        }
                        if (dp["city"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.City);
                            Col = Col + 1;
                        }
                        if (dp["regiao_Cliente"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Regiao_Cliente);
                            Col = Col + 1;
                        }
                        count++;
                    }
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
        public IActionResult ExportToExcelDownload_Clientes(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Clientes.xlsx");
        }

    }
}