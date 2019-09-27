using Hydra.Such.Data;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Fornecedores;
using Hydra.Such.Portal.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class FornecedoresController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly GeneralConfigurations _generalConfig;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FornecedoresController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            _generalConfig = appSettingsGeneral.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Index()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Fornecedores);
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

        public IActionResult DetalhesFornecedor(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Fornecedores);
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

        [HttpPost]
        public JsonResult GetLisFornecedores([FromBody] JObject requestParams)
        {
            string fornecedorNo = string.Empty;
            if (requestParams != null)
            {
                if (requestParams.TryGetValue("fornecedorNo", out JToken fornecedorNoValue))
                    fornecedorNo = (string)fornecedorNoValue;
            }

            List<FornecedorDetailsViewModel> result = new List<FornecedorDetailsViewModel>();
            result = DBNAV2017Fornecedores.GetFornecedores(_config.NAVDatabaseName, _config.NAVCompanyName, fornecedorNo).Select(c =>
                 new FornecedorDetailsViewModel()
                 {
                     No = c.No,
                     Name = c.Name,
                     FullAddress = c.FullAddress,
                     PostCode = c.PostCode,
                     City = c.City,
                     Country = c.Country,
                     Phone = c.Phone,
                     Email = c.Email,
                     Fax = c.Fax,
                     HomePage = c.HomePage,
                     VATRegistrationNo = c.VATRegistrationNo,
                     PaymentTermsCode = c.PaymentTermsCode,
                     PaymentMethodCode = c.PaymentMethodCode,
                     NoClienteAssociado = c.NoClienteAssociado,
                     Blocked = c.Blocked,
                     BlockedText = c.BlockedText,
                     Address = c.Address,
                     Address2 = c.Address2,
                     Distrito = c.Distrito,
                     Criticidade = c.Criticidade,
                     CriticidadeText = c.CriticidadeText,
                     Observacoes = c.Observacoes

                 }
            ).ToList();
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

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Fornecedores([FromBody] List<FornecedorDetailsViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
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
                ISheet excelSheet = workbook.CreateSheet("Fornecedores");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["no"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº"); Col = Col + 1; }
                if (dp["name"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nome"); Col = Col + 1; }
                if (dp["fullAddress"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Endereço"); Col = Col + 1; }
                if (dp["postCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Postal"); Col = Col + 1; }
                if (dp["city"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cidade"); Col = Col + 1; }
                if (dp["country"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Pais/Região"); Col = Col + 1; }
                if (dp["phone"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Telefone"); Col = Col + 1; }
                if (dp["email"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Email"); Col = Col + 1; }
                if (dp["fax"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Fax"); Col = Col + 1; }
                if (dp["homePage"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Home Page"); Col = Col + 1; }
                if (dp["vatRegistrationNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Contribuinte"); Col = Col + 1; }
                if (dp["paymentTermsCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Termos de Pagamento"); Col = Col + 1; }
                if (dp["paymentMethodCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Forma Pagamento"); Col = Col + 1; }
                if (dp["noClienteAssociado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cliente Associado"); Col = Col + 1; }
                if (dp["blockedText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Bloqueado"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (FornecedorDetailsViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["no"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.No); Col = Col + 1; }
                        if (dp["name"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Name); Col = Col + 1; }
                        if (dp["fullAddress"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FullAddress); Col = Col + 1; }
                        if (dp["postCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PostCode); Col = Col + 1; }
                        if (dp["city"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.City); Col = Col + 1; }
                        if (dp["country"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Country); Col = Col + 1; }
                        if (dp["phone"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Phone); Col = Col + 1; }
                        if (dp["email"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Email); Col = Col + 1; }
                        if (dp["fax"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Fax); Col = Col + 1; }
                        if (dp["homePage"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.HomePage); Col = Col + 1; }
                        if (dp["vatRegistrationNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.VATRegistrationNo); Col = Col + 1; }
                        if (dp["paymentTermsCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PaymentTermsCode); Col = Col + 1; }
                        if (dp["paymentMethodCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PaymentMethodCode); Col = Col + 1; }
                        if (dp["noClienteAssociado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoClienteAssociado); Col = Col + 1; }
                        if (dp["blockedText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.BlockedText); Col = Col + 1; }

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
        public IActionResult ExportToExcelDownload_Fornecedores(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Fornecedores.xlsx");
        }

        [HttpPost]
        public JsonResult GetDetails([FromBody] FornecedorDetailsViewModel data)
        {
            if (data != null && data.No != null)
            {
                FornecedorDetailsViewModel result = new FornecedorDetailsViewModel();
                result = DBNAV2017Fornecedores.GetFornecedores(_config.NAVDatabaseName, _config.NAVCompanyName, data.No).Select(c =>
                     new FornecedorDetailsViewModel()
                     {
                         No = c.No,
                         Name = c.Name,
                         FullAddress = c.FullAddress,
                         PostCode = c.PostCode,
                         City = c.City,
                         Country = c.Country,
                         Phone = c.Phone,
                         Email = c.Email,
                         Fax = c.Fax,
                         HomePage = c.HomePage,
                         VATRegistrationNo = c.VATRegistrationNo,
                         PaymentTermsCode = c.PaymentTermsCode,
                         PaymentMethodCode = c.PaymentMethodCode,
                         NoClienteAssociado = c.NoClienteAssociado,
                         Blocked = c.Blocked,
                         BlockedText = c.BlockedText,
                         Address = c.Address,
                         Address2 = c.Address2,
                         Distrito = c.Distrito,
                         Criticidade = c.Criticidade,
                         CriticidadeText = c.CriticidadeText,
                         Observacoes = c.Observacoes

                     }
                ).FirstOrDefault();

                return Json(result);
            }
            return Json(false);
        }



















    }
}