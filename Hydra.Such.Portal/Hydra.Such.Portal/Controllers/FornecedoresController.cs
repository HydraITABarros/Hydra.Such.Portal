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

        public IActionResult FornecedorQuestionarioDetalhes(string id)
        {
            string Codigo = "QUEST_" + id;
            int Versao = 0;

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FornecedoresAvaliacao);
            List<QuestionarioFornecedorGestaoAmbiental> AllQuestionarios = DBQuestionarioFornecedorGestaoAmbiental.GetAllByFornecedor(id);

            if (AllQuestionarios != null && AllQuestionarios.Count > 0)
            {
                Versao = AllQuestionarios.OrderBy(x => x.Versao).LastOrDefault().Versao;
            }

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Codigo = Codigo ?? "";
                ViewBag.Versao = Versao.ToString();
                ViewBag.ID_Fornecedor = id ?? "";
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
                    ConfigUtilizadores UserEmail = DBUserConfigurations.GetById(User.Identity.Name);

                    if (Parametro != null && !string.IsNullOrEmpty(Parametro.Valor))
                    {
                        SendEmailApprovals Email = new SendEmailApprovals();

                        var path_tmp = Path.Combine(_generalConfig.FileUploadFolder + "Fornecedores\\tmp\\", data.NomeAnexo);
                        string FileName_Final = data.NomeAnexo.Replace("FORNECEDOR", vendor.No);
                        var path_final = Path.Combine(_generalConfig.FileUploadFolder + "Fornecedores\\", FileName_Final);

                        FileStream file_tmp = new FileStream(path_tmp, FileMode.Open);
                        FileStream file_final = new FileStream(path_final, FileMode.CreateNew);

                        file_tmp.CopyTo(file_final);

                        file_tmp.Dispose();
                        file_final.Dispose();

                        System.IO.File.Delete(path_tmp);

                        Anexos newfile = new Anexos();
                        newfile.NºOrigem = vendor.No;
                        newfile.UrlAnexo = FileName_Final;
                        newfile.TipoOrigem = TipoOrigemAnexos.Fornecedores;
                        newfile.DataHoraCriação = DateTime.Now;
                        newfile.UtilizadorCriação = User.Identity.Name;

                        Email.DisplayName = "e-SUCH - Fornecedor";
                        Email.From = "esuch@such.pt";
                        Email.To.Add(Parametro.Valor);
                        Email.BCC.Add("MMarcelo@such.pt");
                        //Email.BCC.Add("ARomao@such.pt");
                        Email.Subject = "e-SUCH - Novo Fornecedor";
                        Email.Body = MakeEmailBodyContent("Criado o Fornecedor:  " + vendor.No + " - " + vendor.Name, UserEmail.Nome);
                        Email.Anexo = path_final;
                        Email.IsBodyHtml = true;

                        Email.SendEmail_Simple();
                    }

                    return Json(vendor);
                }

            }
            return Json(data);
        }

        public static string MakeEmailBodyContent(string BodyText, string BodyAssinatura)
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
                                                BodyAssinatura +
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

        [HttpPost]
        public JsonResult VerificarData([FromBody] FornecedorDetailsViewModel data)
        {
            data.eReasonCode = 0;
            data.eMessage = "";

            if (data != null)
            {
                if (string.IsNullOrEmpty(data.Name))
                {
                    data.eReasonCode = 2;
                    data.eMessage = "O campo Nome é de preenchimento obrigatório.";
                    return Json(data);
                }

                if (string.IsNullOrEmpty(data.Address))
                {
                    data.eReasonCode = 3;
                    data.eMessage = "O campo Endereço é de preenchimento obrigatório.";
                    return Json(data);
                }

                if (string.IsNullOrEmpty(data.PostCode))
                {
                    data.eReasonCode = 4;
                    data.eMessage = "O campo Código Postal é de preenchimento obrigatório.";
                    return Json(data);
                }

                if (string.IsNullOrEmpty(data.City))
                {
                    data.eReasonCode = 5;
                    data.eMessage = "O campo Cidade é de preenchimento obrigatório.";
                    return Json(data);
                }

                if (string.IsNullOrEmpty(data.Country))
                {
                    data.eReasonCode = 6;
                    data.eMessage = "O campo Código Pais/Região é de preenchimento obrigatório.";
                    return Json(data);
                }

                if (string.IsNullOrEmpty(data.VATRegistrationNo))
                {
                    data.eReasonCode = 7;
                    data.eMessage = "O campo Nº Contribuinte é de preenchimento obrigatório.";
                    return Json(data);
                }

                if (string.IsNullOrEmpty(data.PaymentTermsCode))
                {
                    data.eReasonCode = 8;
                    data.eMessage = "O campo Termos de Pagamento é de preenchimento obrigatório.";
                    return Json(data);
                }

                if (string.IsNullOrEmpty(data.PaymentMethodCode))
                {
                    data.eReasonCode = 9;
                    data.eMessage = "O campo Forma Pagamento é de preenchimento obrigatório.";
                    return Json(data);
                }

                string No = data.No;
                string VAT = data.VATRegistrationNo;
                string Country = data.Country;

                List<NAVFornecedoresViewModel> AllFornecedor = DBNAV2017Fornecedores.GetFornecedores(_config.NAVDatabaseName, _config.NAVCompanyName, string.Empty);
                List<NAVFornecedoresViewModel> FornecedorWithVAT = AllFornecedor.Where(x => x.VATRegistrationNo == VAT && x.No != No).ToList();

                if (FornecedorWithVAT != null && FornecedorWithVAT.Count > 0)
                {
                    data.eReasonCode = 10;
                    data.eMessage = "Já existe pelo menos um fonecedor com o nome " + FornecedorWithVAT.FirstOrDefault().Name + " com o mesmo Nº de Contribuinte.";
                    return Json(data);
                }
                else
                {
                    if (Country == "PT")
                    {
                        VAT = VAT.Replace(" ", "");
                        if (VAT.Length == 9)
                        {
                            int n;
                            bool isNumeric = int.TryParse(VAT, out n);

                            if (isNumeric == false)
                            {
                                data.eReasonCode = 13;
                                data.eMessage = "Para Portugal o Nº de contribuinte só pode ter 9 carateres numéricos.";
                                return Json(data);
                            }
                            else
                            {
                                data.eReasonCode = 0;
                                data.eMessage = "";
                                return Json(data);
                            }
                        }
                        else
                        {
                            data.eReasonCode = 11;
                            data.eMessage = "Para Portugal o Nº de contribuinte só pode ter 9 carateres numéricos.";
                            return Json(data);
                        }
                    }
                    else
                    {
                        if (VAT.Length > 0)
                        {
                            data.eReasonCode = 0;
                            data.eMessage = "";
                            return Json(data);
                        }
                        else
                        {
                            data.eReasonCode = 12;
                            data.eMessage = "É obrigatório preencher o Nº de contribuinte.";
                            return Json(data);
                        }
                    }
                }
            }
            else
            {
                data.eReasonCode = 13;
                data.eMessage = "Ocorreu um erro.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult VerificarVAT([FromBody] FornecedorDetailsViewModel data)
        {
            data.eReasonCode = 0;
            data.eMessage = "";

            if (data != null && !string.IsNullOrEmpty(data.No) && !string.IsNullOrEmpty(data.VATRegistrationNo) && !string.IsNullOrEmpty(data.Country))
            {
                string No = data.No;
                string VAT = data.VATRegistrationNo;
                string Country = data.Country;

                List<NAVFornecedoresViewModel> AllFornecedor = DBNAV2017Fornecedores.GetFornecedores(_config.NAVDatabaseName, _config.NAVCompanyName, string.Empty);
                List<NAVFornecedoresViewModel> FornecedorWithVAT = AllFornecedor.Where(x => x.VATRegistrationNo == VAT && x.No != No).ToList();

                if (FornecedorWithVAT != null && FornecedorWithVAT.Count > 0)
                {
                    data.eReasonCode = 1;
                    data.eMessage = "Já existe pelo menos um fonecedor com o nome " + FornecedorWithVAT.FirstOrDefault().Name + " com o mesmo Nº de Contribuinte.";
                    return Json(data);
                }
                else
                {
                    if (Country == "PT")
                    {
                        VAT = VAT.Replace(" ", "");
                        if (VAT.Length == 9)
                        {
                            int n;
                            bool isNumeric = int.TryParse(VAT, out n);

                            if (isNumeric == false)
                            {
                                data.eReasonCode = 13;
                                data.eMessage = "Para Portugal o Nº de contribuinte só pode ter 9 carateres numéricos.";
                                return Json(data);
                            }
                            else
                            {
                                data.eReasonCode = 0;
                                data.eMessage = "";
                                return Json(data);
                            }
                        }
                        else
                        {
                            data.eReasonCode = 2;
                            data.eMessage = "Para Portugal o Nº de contribuinte só pode ter 9 carateres numéricos.";
                            return Json(data);
                        }
                    }
                    else
                    {
                        if (VAT.Length > 0)
                        {
                            data.eReasonCode = 0;
                            data.eMessage = "";
                            return Json(data);
                        }
                        else
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "É obrigatório preencher o Nº de contribuinte.";
                            return Json(data);
                        }
                    }
                }
            }
            data.eReasonCode = 4;
            data.eMessage = "É obrigatório ter preenchido os campos Nº Fornecedor, Código/País Região e Nº de contribuinte.";
            return Json(data);
        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            name = System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
            name = name.Replace("+", "_");

            return name;
        }

        [HttpPost]
        [Route("Fornecedores/FileUpload")]
        public JsonResult FileUpload()
        {
            string full_filename = string.Empty;
            try
            {
                var files = Request.Form.Files;
                foreach (var file in files)
                {
                    try
                    {
                        string extension = Path.GetExtension(file.FileName);
                        if (extension.ToLower() == ".msg" ||
                            extension.ToLower() == ".txt" || extension.ToLower() == ".text" ||
                            extension.ToLower() == ".pdf" ||
                            extension.ToLower() == ".xls" || extension.ToLower() == ".xlsx" ||
                            extension.ToLower() == ".doc" || extension.ToLower() == ".docx" || extension.ToLower() == ".dotx" ||
                            extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".pjpeg" || extension.ToLower() == ".jfif" || extension.ToLower() == ".pjp" ||
                            extension.ToLower() == ".png" || extension.ToLower() == ".gif")
                        {
                            string filename = Path.GetFileName(file.FileName);

                            filename = MakeValidFileName(filename);
                            full_filename = "FORNECEDOR_" + User.Identity.Name + "_" + filename;
                            var path = Path.Combine(_generalConfig.FileUploadFolder + "Fornecedores\\tmp\\", full_filename);

                            using (FileStream dd = new FileStream(path, FileMode.CreateNew))
                            {
                                file.CopyTo(dd);
                                dd.Dispose();

                                //Anexos newfile = new Anexos();
                                //newfile.NºOrigem = id;
                                //newfile.UrlAnexo = full_filename;
                                //newfile.TipoOrigem = TipoOrigemAnexos.PreRequisicao;
                                //newfile.DataHoraCriação = DateTime.Now;
                                //newfile.UtilizadorCriação = User.Identity.Name;

                                //DBAttachments.Create(newfile);
                                //if (newfile.NºLinha == 0)
                                //{
                                //    System.IO.File.Delete(path);
                                //}
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(null);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(null);
            }
            return Json(full_filename);
        }

        [HttpPost]
        [Route("Fornecedores/UploadQuestionarioAnexo")]
        [Route("Fornecedores/UploadQuestionarioAnexo/{codigo}/{versao}/{idfornecedor}")]
        public JsonResult UploadQuestionarioAnexo(string codigo, string versao, string idfornecedor)
        {
            string full_filename = string.Empty;
            try
            {
                var files = Request.Form.Files;
                foreach (var file in files)
                {
                    try
                    {
                        string extension = Path.GetExtension(file.FileName);
                        if (extension.ToLower() == ".msg" ||
                            extension.ToLower() == ".txt" || extension.ToLower() == ".text" ||
                            extension.ToLower() == ".pdf" ||
                            extension.ToLower() == ".xls" || extension.ToLower() == ".xlsx" ||
                            extension.ToLower() == ".doc" || extension.ToLower() == ".docx" || extension.ToLower() == ".dotx" ||
                            extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".pjpeg" || extension.ToLower() == ".jfif" || extension.ToLower() == ".pjp" ||
                            extension.ToLower() == ".png" || extension.ToLower() == ".gif")
                        {
                            string filename = Path.GetFileName(file.FileName);
                            filename = MakeValidFileName(filename);
                            full_filename = codigo + "_" + versao + "_" + filename;
                            var path = Path.Combine(_generalConfig.FileUploadFolder + "Fornecedores\\Questionarios\\tmp", full_filename);

                            using (FileStream dd = new FileStream(path, FileMode.CreateNew))
                            {
                                file.CopyTo(dd);
                                dd.Dispose();
                            }

                            QuestionarioFornecedorGestaoAmbientalAnexos Anexo = new QuestionarioFornecedorGestaoAmbientalAnexos
                            {
                                Codigo = codigo,
                                Versao = int.Parse(versao),
                                ID_Fornecedor = idfornecedor,
                                URL_Anexo = filename,
                                Visivel = false,
                                Utilizador_Criacao = User.Identity.Name
                            };
                            DBQuestionarioFornecedorGestaoAmbientalAnexos.Create(Anexo);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(null);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(null);
            }
            return Json(full_filename);
        }

        [HttpGet]
        [Route("Fornecedores/DownloadQuestionarioAnexo")]
        [Route("Fornecedores/DownloadQuestionarioAnexo/{codigo}/{versao}/{filename}")]
        public FileStreamResult DownloadQuestionarioAnexo(string codigo, string versao, string filename)
        {
            try
            {
                if (!string.IsNullOrEmpty(codigo) && !string.IsNullOrEmpty(versao) && !string.IsNullOrEmpty(filename))
                {
                    string FullFileName = codigo + "_" + versao + "_" + filename;

                    string sFileName = Path.Combine(_generalConfig.FileUploadFolder + "Fornecedores\\Questionarios", FullFileName);

                    FileStream file = new FileStream(sFileName, FileMode.Open);
                    string mimeType = GetMimeType(file.Name);
                    return new FileStreamResult(file, mimeType);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();

            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);

            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        [HttpPost]
        public JsonResult GetQuestionarioDetails([FromBody] QuestionarioFornecedorGestaoAmbientalViewModel data)
        {
            if (data != null)
            {
                data.Versao = int.Parse(data.Versao_Texto);
                if (!string.IsNullOrEmpty(data.Codigo) && data.Versao > 0)
                {
                    data = DBQuestionarioFornecedorGestaoAmbiental.GetByCodigoAndVersao(data.Codigo, data.Versao).ParseToViewModel();
                    data.Anexo = DBQuestionarioFornecedorGestaoAmbientalAnexos.GetByCodigoAndVersao(data.Codigo, data.Versao).ParseToViewModel();
                }
                else
                {
                    NAVFornecedoresViewModel Fornecedor = DBNAV2017Fornecedores.GetFornecedores(_config.NAVDatabaseName, _config.NAVCompanyName, data.ID_Fornecedor).FirstOrDefault();

                    if (Fornecedor != null)
                    {
                        data.Fornecedor = Fornecedor.Name;
                    }
                }

                return Json(data);
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult GetQuestionarioHistorico([FromBody] QuestionarioFornecedorGestaoAmbientalViewModel data)
        {
            if (data != null)
            {
                List<QuestionarioFornecedorGestaoAmbiental> Historico = new List<QuestionarioFornecedorGestaoAmbiental>();
                List<QuestionarioFornecedorGestaoAmbientalViewModel> HistoricoVM = new List<QuestionarioFornecedorGestaoAmbientalViewModel>();
                if (!string.IsNullOrEmpty(data.ID_Fornecedor))
                {
                    Historico = DBQuestionarioFornecedorGestaoAmbiental.GetAllByFornecedor(data.ID_Fornecedor);
                    if (Historico != null && Historico.Count > 0)
                    {
                        if (Historico != null && Historico.Count > 0)
                        {
                            HistoricoVM = Historico.ParseToViewModel().OrderByDescending(x => x.Versao).ToList();
                            HistoricoVM.RemoveAt(0);
                        }
                    }
                }

                return Json(HistoricoVM);
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult VerificarNovoQuestionario([FromBody] QuestionarioFornecedorGestaoAmbientalViewModel data)
        {
            if (data != null)
            {
                data.Nova_Versao = false;

                if (data.Versao == 0)
                {
                    data.Nova_Versao = true;
                }
                else
                {
                    QuestionarioFornecedorGestaoAmbientalViewModel Original = DBQuestionarioFornecedorGestaoAmbiental.GetByCodigoAndVersao(data.Codigo, data.Versao).ParseToViewModel();

                    if (data.ID_Fornecedor != Original.ID_Fornecedor) data.Nova_Versao = true;
                    if (data.Fornecedor != Original.Fornecedor) data.Nova_Versao = true;
                    if (data.Actividade != Original.Actividade) data.Nova_Versao = true;
                    if (data.Responsavel != Original.Responsavel) data.Nova_Versao = true;
                    if (data.Funcao != Original.Funcao) data.Nova_Versao = true;
                    if (data.Telefone != Original.Telefone) data.Nova_Versao = true;
                    if (data.Procedimento != Original.Procedimento) data.Nova_Versao = true;
                    if (data.Email != Original.Email) data.Nova_Versao = true;
                    if (data.Resposta_11_Sim != Original.Resposta_11_Sim) data.Nova_Versao = true;
                    if (data.Resposta_11_Nao != Original.Resposta_11_Nao) data.Nova_Versao = true;
                    if (data.Resposta_11_NA != Original.Resposta_11_NA) data.Nova_Versao = true;
                    if (data.Resposta_11_Texto != Original.Resposta_11_Texto) data.Nova_Versao = true;
                    if (data.Resposta_12_Texto != Original.Resposta_12_Texto) data.Nova_Versao = true;
                    if (data.Resposta_13_Texto != Original.Resposta_13_Texto) data.Nova_Versao = true;
                    if (data.Resposta_21_Sim != Original.Resposta_21_Sim) data.Nova_Versao = true;
                    if (data.Resposta_21_Nao != Original.Resposta_21_Nao) data.Nova_Versao = true;
                    if (data.Resposta_21_NA != Original.Resposta_21_NA) data.Nova_Versao = true;
                    if (data.Resposta_22_Sim != Original.Resposta_22_Sim) data.Nova_Versao = true;
                    if (data.Resposta_22_Nao != Original.Resposta_22_Nao) data.Nova_Versao = true;
                    if (data.Resposta_22_NA != Original.Resposta_22_NA) data.Nova_Versao = true;
                    if (data.Resposta_23_Texto != Original.Resposta_23_Texto) data.Nova_Versao = true;
                    if (data.Resposta_2_Texto != Original.Resposta_2_Texto) data.Nova_Versao = true;
                    if (data.Resposta_31_Sim != Original.Resposta_31_Sim) data.Nova_Versao = true;
                    if (data.Resposta_31_Nao != Original.Resposta_31_Nao) data.Nova_Versao = true;
                    if (data.Resposta_31_NA != Original.Resposta_31_NA) data.Nova_Versao = true;
                    if (data.Resposta_32_NA != Original.Resposta_32_NA) data.Nova_Versao = true;
                    if (data.Resposta_32_Texto != Original.Resposta_32_Texto) data.Nova_Versao = true;
                    if (data.Resposta_3_Texto != Original.Resposta_3_Texto) data.Nova_Versao = true;
                    if (data.Resposta_41_Sim != Original.Resposta_41_Sim) data.Nova_Versao = true;
                    if (data.Resposta_41_Nao != Original.Resposta_41_Nao) data.Nova_Versao = true;
                    if (data.Resposta_41_NA != Original.Resposta_41_NA) data.Nova_Versao = true;
                    if (data.Resposta_42_Sim != Original.Resposta_42_Sim) data.Nova_Versao = true;
                    if (data.Resposta_42_Nao != Original.Resposta_42_Nao) data.Nova_Versao = true;
                    if (data.Resposta_42_NA != Original.Resposta_42_NA) data.Nova_Versao = true;
                    if (data.Resposta_43_Texto != Original.Resposta_43_Texto) data.Nova_Versao = true;
                    if (data.Resposta_4_Texto != Original.Resposta_4_Texto) data.Nova_Versao = true;
                    if (data.Resposta_51_Sim != Original.Resposta_51_Sim) data.Nova_Versao = true;
                    if (data.Resposta_51_Nao != Original.Resposta_51_Nao) data.Nova_Versao = true;
                    if (data.Resposta_51_NA != Original.Resposta_51_NA) data.Nova_Versao = true;
                    if (data.Resposta_52_Sim != Original.Resposta_52_Sim) data.Nova_Versao = true;
                    if (data.Resposta_52_Nao != Original.Resposta_52_Nao) data.Nova_Versao = true;
                    if (data.Resposta_52_NA != Original.Resposta_52_NA) data.Nova_Versao = true;
                    if (data.Resposta_5_Texto != Original.Resposta_5_Texto) data.Nova_Versao = true;
                    if (data.Resposta_61_Sim != Original.Resposta_61_Sim) data.Nova_Versao = true;
                    if (data.Resposta_61_Nao != Original.Resposta_61_Nao) data.Nova_Versao = true;
                    if (data.Resposta_61_NA != Original.Resposta_61_NA) data.Nova_Versao = true;
                    if (data.Resposta_62_Sim != Original.Resposta_62_Sim) data.Nova_Versao = true;
                    if (data.Resposta_62_Nao != Original.Resposta_62_Nao) data.Nova_Versao = true;
                    if (data.Resposta_62_NA != Original.Resposta_62_NA) data.Nova_Versao = true;
                    if (data.Resposta_63_Sim != Original.Resposta_63_Sim) data.Nova_Versao = true;
                    if (data.Resposta_63_Nao != Original.Resposta_63_Nao) data.Nova_Versao = true;
                    if (data.Resposta_63_NA != Original.Resposta_63_NA) data.Nova_Versao = true;
                    if (data.Resposta_64_Sim != Original.Resposta_64_Sim) data.Nova_Versao = true;
                    if (data.Resposta_64_Nao != Original.Resposta_64_Nao) data.Nova_Versao = true;
                    if (data.Resposta_64_NA != Original.Resposta_64_NA) data.Nova_Versao = true;
                    if (data.Resposta_65_Sim != Original.Resposta_65_Sim) data.Nova_Versao = true;
                    if (data.Resposta_65_Nao != Original.Resposta_65_Nao) data.Nova_Versao = true;
                    if (data.Resposta_65_NA != Original.Resposta_65_NA) data.Nova_Versao = true;
                    if (data.Resposta_6_Texto != Original.Resposta_6_Texto) data.Nova_Versao = true;
                    if (data.Resposta_71_Sim != Original.Resposta_71_Sim) data.Nova_Versao = true;
                    if (data.Resposta_71_Nao != Original.Resposta_71_Nao) data.Nova_Versao = true;
                    if (data.Resposta_71_NA != Original.Resposta_71_NA) data.Nova_Versao = true;
                    if (data.Resposta_72_Sim != Original.Resposta_72_Sim) data.Nova_Versao = true;
                    if (data.Resposta_72_Nao != Original.Resposta_72_Nao) data.Nova_Versao = true;
                    if (data.Resposta_72_NA != Original.Resposta_72_NA) data.Nova_Versao = true;
                    if (data.Resposta_7_Texto != Original.Resposta_7_Texto) data.Nova_Versao = true;
                    if (data.Resposta_81_Sim != Original.Resposta_81_Sim) data.Nova_Versao = true;
                    if (data.Resposta_81_Nao != Original.Resposta_81_Nao) data.Nova_Versao = true;
                    if (data.Resposta_81_NA != Original.Resposta_81_NA) data.Nova_Versao = true;
                    if (data.Resposta_82_Sim != Original.Resposta_82_Sim) data.Nova_Versao = true;
                    if (data.Resposta_82_Nao != Original.Resposta_82_Nao) data.Nova_Versao = true;
                    if (data.Resposta_82_NA != Original.Resposta_82_NA) data.Nova_Versao = true;
                    if (data.Resposta_83_Sim != Original.Resposta_83_Sim) data.Nova_Versao = true;
                    if (data.Resposta_83_Nao != Original.Resposta_83_Nao) data.Nova_Versao = true;
                    if (data.Resposta_83_NA != Original.Resposta_83_NA) data.Nova_Versao = true;
                    if (data.Resposta_8_Texto != Original.Resposta_8_Texto) data.Nova_Versao = true;
                    if (data.Resposta_91_Sim != Original.Resposta_91_Sim) data.Nova_Versao = true;
                    if (data.Resposta_91_Nao != Original.Resposta_91_Nao) data.Nova_Versao = true;
                    if (data.Resposta_91_NA != Original.Resposta_91_NA) data.Nova_Versao = true;
                    if (data.Resposta_9_Texto != Original.Resposta_9_Texto) data.Nova_Versao = true;
                    if (data.Resposta_101_Sim != Original.Resposta_101_Sim) data.Nova_Versao = true;
                    if (data.Resposta_101_Nao != Original.Resposta_101_Nao) data.Nova_Versao = true;
                    if (data.Resposta_101_NA != Original.Resposta_101_NA) data.Nova_Versao = true;
                    if (data.Resposta_101_Texto != Original.Resposta_101_Texto) data.Nova_Versao = true;
                    if (data.Resposta_102_Sim != Original.Resposta_102_Sim) data.Nova_Versao = true;
                    if (data.Resposta_102_Nao != Original.Resposta_102_Nao) data.Nova_Versao = true;
                    if (data.Resposta_102_NA != Original.Resposta_102_NA) data.Nova_Versao = true;
                    if (data.Resposta_102_Texto != Original.Resposta_102_Texto) data.Nova_Versao = true;
                    if (data.Resposta_103_Texto != Original.Resposta_103_Texto) data.Nova_Versao = true;
                    if (data.Final_Responsavel != Original.Final_Responsavel) data.Nova_Versao = true;
                    if (data.Final_Data_Texto != Original.Final_Data_Texto) data.Nova_Versao = true;
                }

                return Json(data);
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult CreateQuestionario([FromBody] QuestionarioFornecedorGestaoAmbientalViewModel data)
        {
            if (data != null)
            {
                data.Versao = data.Versao + 1;
                data.Utilizador_Criacao = User.Identity.Name;
                data.Utilizador_Modificacao = null;
                data.DataHora_Modificacao = null;
                if (DBQuestionarioFornecedorGestaoAmbiental.Create(data.ParseToDB()) != null)
                {
                    QuestionarioFornecedorGestaoAmbientalAnexos Anexo = DBQuestionarioFornecedorGestaoAmbientalAnexos.GetByCodigoAndVersao(data.Codigo, data.Versao);

                    var sourceFile = _generalConfig.FileUploadFolder + "Fornecedores\\" + "Questionarios\\" + "tmp\\" + Anexo.Codigo + "_" + Anexo.Versao + "_" + Anexo.URL_Anexo;
                    var destFile = _generalConfig.FileUploadFolder + "Fornecedores\\" + "Questionarios\\" + Anexo.Codigo + "_" + Anexo.Versao + "_" + Anexo.URL_Anexo;
                    System.IO.File.Move(sourceFile, destFile);

                    Anexo.Visivel = true;
                    DBQuestionarioFornecedorGestaoAmbientalAnexos.Update(Anexo);

                    data.eReasonCode = 1;
                    data.eMessage = "Avaliação Desempenho Ambiental criada com sucesso.";

                    return Json(data);
                }
            }
            data.eReasonCode = 2;
            data.eMessage = "Não foi possivel criar uma nova Avaliação Desempenho Ambiental.";

            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateQuestionario([FromBody] QuestionarioFornecedorGestaoAmbientalViewModel data)
        {
            if (data != null)
            {
                data.Utilizador_Modificacao = User.Identity.Name;
                if (DBQuestionarioFornecedorGestaoAmbiental.Update(data.ParseToDB()) != null)
                {
                    data.eReasonCode = 1;
                    data.eMessage = "Avaliação Desempenho Ambiental atualizada com sucesso.";

                    return Json(data);
                }
            }
            data.eReasonCode = 2;
            data.eMessage = "Não foi possivel atualizar a Avaliação Desempenho Ambiental.";

            return Json(data);
        }









    }
}