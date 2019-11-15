using Hydra.Such.Data;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Approvals;
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
                     Address_2 = c.Address2,
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

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Fornecedores\\" + "tmp\\";
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
            sFileName = _generalConfig.FileUploadFolder + "Fornecedores\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Fornecedores.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [HttpPost]
        public JsonResult GetDetails([FromBody] FornecedorDetailsViewModel data)
        {
            if (data != null && data.No != null)
            {
                var getVendorTask = WSVendorService.GetByNoAsync(data.No, _configws);
                try
                {
                    getVendorTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro a obter o fornecedor no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                FornecedorDetailsViewModel vendor = getVendorTask.Result;
                if (vendor != null)
                {
                    return Json(vendor);
                }

            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult Create([FromBody] FornecedorDetailsViewModel data)
        {
            if (data != null)
            {
                if (string.IsNullOrEmpty(data.No)) data.No = "";
                if (string.IsNullOrEmpty(data.Name)) data.Name = "";
                if (string.IsNullOrEmpty(data.PostCode)) data.PostCode = "";
                if (string.IsNullOrEmpty(data.City)) data.City = "";
                if (string.IsNullOrEmpty(data.Country)) data.Country = "";
                if (string.IsNullOrEmpty(data.Phone)) data.Phone = "";
                if (string.IsNullOrEmpty(data.Email)) data.Email = "";
                if (string.IsNullOrEmpty(data.Fax)) data.Fax = "";
                if (string.IsNullOrEmpty(data.HomePage)) data.HomePage = "";
                if (string.IsNullOrEmpty(data.VATRegistrationNo)) data.VATRegistrationNo = "";
                if (string.IsNullOrEmpty(data.PaymentTermsCode)) data.PaymentTermsCode = "";
                if (string.IsNullOrEmpty(data.PaymentMethodCode)) data.PaymentMethodCode = "";
                if (string.IsNullOrEmpty(data.NoClienteAssociado)) data.NoClienteAssociado = "";
                if (data.Blocked == null) data.Blocked = 0;
                if (string.IsNullOrEmpty(data.Address)) data.Address = "";
                if (string.IsNullOrEmpty(data.Address_2)) data.Address_2 = "";
                if (string.IsNullOrEmpty(data.Distrito)) data.Distrito = "";
                if (data.Criticidade == null) data.Criticidade = 0;
                if (string.IsNullOrEmpty(data.Observacoes)) data.Observacoes = "";

                data.Utilizador_Alteracao_eSUCH = User.Identity.Name;
                var createVendorTask = WSVendorService.CreateAsync(data, _configws);
                try
                {
                    createVendorTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao criar o fornecedor no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                var result = createVendorTask.Result;
                if (result == null)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao criar o fornecedor no NAV.";
                    return Json(data);
                }

                data.eReasonCode = 1;

                var vendor = WSVendorService.MapVendorNAVToVendorModel(result.WSVendor);
                if (vendor != null)
                {
                    //SUCESSO
                    vendor.eReasonCode = 1;

                    //Envio de email
                    ConfiguracaoParametros Parametro = DBConfiguracaoParametros.GetByParametro("AddFornecedorEmail");
                    if (Parametro != null && !string.IsNullOrEmpty(Parametro.Valor))
                    {
                        SendEmailApprovals Email = new SendEmailApprovals();

                        Email.DisplayName = "e-SUCH - Fornecedor";
                        Email.From = User.Identity.Name;
                        Email.To.Add(Parametro.Valor);
                        Email.Subject = "e-SUCH - Foi criado um novo Fornecedor";
                        Email.Body = MakeEmailBodyContent("Foi criado um novo Fornecedor " + vendor.Name + " como o código Nº " + vendor.No + " no e-SUCH.");
                        Email.IsBodyHtml = true;

                        Email.SendEmail_Simple();
                    }

                    return Json(vendor);
                }

            }
            return Json(data);
        }

        public static string MakeEmailBodyContent(string BodyText)
        {
            string Body = @"<html>" +
                                "<head>" +
                                    "<style>" +
                                        "table{border:0;} " +
                                        "td{width:600px; vertical-align: top;}" +
                                    "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<table>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Caro (a)," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                BodyText +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Com os melhores cumprimentos," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<i>SUCH - Serviço de Utilização Comum dos Hospitais</i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";

            return Body;
        }

        [HttpPost]
        public JsonResult Update([FromBody] FornecedorDetailsViewModel data)
        {
            if (data != null)
            {
                data.Utilizador_Alteracao_eSUCH = User.Identity.Name;
                var updateVendorTask = WSVendorService.UpdateAsync(data, _configws);
                try
                {
                    updateVendorTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao atualizar o fornecedor no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                var result = updateVendorTask.Result;
                if (result == null)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao atualizar o fornecedor no NAV.";
                    return Json(data);
                }

                var vendor = WSVendorService.MapVendorNAVToVendorModel(result.WSVendor);
                if (vendor != null)
                {
                    vendor.eReasonCode = 1;
                    return Json(vendor);
                }

            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult Delete([FromBody] FornecedorDetailsViewModel data)
        {
            if (data != null && data.No != null)
            {
                data.Utilizador_Alteracao_eSUCH = User.Identity.Name;
                var deleteVendorTask = WSVendorService.DeleteAsync(data.No, _configws);
                try
                {
                    deleteVendorTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro a apagar o fornecedor no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                var result = deleteVendorTask.Result;
                if (result != null)
                {
                    return Json(new ErrorHandler()
                    {
                        eReasonCode = 0,
                        eMessage = "Fornecedor removido com sucesso."
                    });
                }

            }
            return Json(false);
        }
















    }
}