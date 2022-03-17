using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.Logic.VisitasDB;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.VisitasVM;
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
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class VisitasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GeneralConfigurations _generalConfig;

        public VisitasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment, IOptions<GeneralConfigurations> appSettingsGeneral)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
            _generalConfig = appSettingsGeneral.Value;
        }

        public IActionResult Visitas_List()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Visitas);

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

        public IActionResult Visitas_Details(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Visitas);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.VisitaNo = string.IsNullOrEmpty(id) ? "" : id;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetList()
        {

            List<VisitasViewModel> result = DBVisitas.ParseListToViewModel(DBVisitas.GetAllToList());

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CodRegiao));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CodArea));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CodCresp));

            return Json(result);
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_VisitasList([FromBody] List<VisitasViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Visitas\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Visitas");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["codVisita"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["codVisita"]["label"].ToString()); Col = Col + 1; }
                if (dp["objetivo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["objetivo"]["label"].ToString()); Col = Col + 1; }
                if (dp["local"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["local"]["label"].ToString()); Col = Col + 1; }
                if (dp["codCliente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["codCliente"]["label"].ToString()); Col = Col + 1; }
                if (dp["nomeCliente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["nomeCliente"]["label"].ToString()); Col = Col + 1; }
                if (dp["codFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["codFornecedor"]["label"].ToString()); Col = Col + 1; }
                if (dp["nomeFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["nomeFornecedor"]["label"].ToString()); Col = Col + 1; }
                if (dp["entidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["entidade"]["label"].ToString()); Col = Col + 1; }
                if (dp["codRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["codRegiao"]["label"].ToString()); Col = Col + 1; }
                if (dp["nomeRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["nomeRegiao"]["label"].ToString()); Col = Col + 1; }
                if (dp["codArea"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["codArea"]["label"].ToString()); Col = Col + 1; }
                if (dp["nomeArea"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["nomeArea"]["label"].ToString()); Col = Col + 1; }
                if (dp["codCresp"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["codCresp"]["label"].ToString()); Col = Col + 1; }
                if (dp["nomeCresp"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["nomeCresp"]["label"].ToString()); Col = Col + 1; }
                if (dp["inicioDataTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["inicioDataTexto"]["label"].ToString()); Col = Col + 1; }
                if (dp["inicioHoraTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["inicioHoraTexto"]["label"].ToString()); Col = Col + 1; }
                if (dp["fimDataTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["fimDataTexto"]["label"].ToString()); Col = Col + 1; }
                if (dp["fimHoraTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["fimHoraTexto"]["label"].ToString()); Col = Col + 1; }
                if (dp["nomeEstado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["nomeEstado"]["label"].ToString()); Col = Col + 1; }
                if (dp["iniciativaCriadorNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["iniciativaCriadorNome"]["label"].ToString()); Col = Col + 1; }
                if (dp["iniciativaResponsavelNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["iniciativaResponsavelNome"]["label"].ToString()); Col = Col + 1; }
                if (dp["iniciativaIntervinientes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["iniciativaIntervinientes"]["label"].ToString()); Col = Col + 1; }
                if (dp["rececaoCriador"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["rececaoCriador"]["label"].ToString()); Col = Col + 1; }
                if (dp["rececaoResponsavel"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["rececaoResponsavel"]["label"].ToString()); Col = Col + 1; }
                if (dp["rececaoIntervinientes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["rececaoIntervinientes"]["label"].ToString()); Col = Col + 1; }
                if (dp["relatorioSimplificado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["relatorioSimplificado"]["label"].ToString()); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (VisitasViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["codVisita"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodVisita); Col = Col + 1; }
                        if (dp["objetivo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Objetivo); Col = Col + 1; }
                        if (dp["local"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Local); Col = Col + 1; }
                        if (dp["codCliente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodCliente); Col = Col + 1; }
                        if (dp["nomeCliente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NomeCliente); Col = Col + 1; }
                        if (dp["codFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodFornecedor); Col = Col + 1; }
                        if (dp["nomeFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NomeFornecedor); Col = Col + 1; }
                        if (dp["entidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Entidade); Col = Col + 1; }
                        if (dp["codRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodRegiao); Col = Col + 1; }
                        if (dp["nomeRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NomeRegiao); Col = Col + 1; }
                        if (dp["codArea"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodArea); Col = Col + 1; }
                        if (dp["nomeArea"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NomeArea); Col = Col + 1; }
                        if (dp["codCresp"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodCresp); Col = Col + 1; }
                        if (dp["nomeCresp"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NomeCresp); Col = Col + 1; }
                        if (dp["inicioDataTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.InicioDataTexto); Col = Col + 1; }
                        if (dp["inicioHoraTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.InicioHoraTexto); Col = Col + 1; }
                        if (dp["fimDataTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FimDataTexto); Col = Col + 1; }
                        if (dp["fimHoraTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FimHoraTexto); Col = Col + 1; }
                        if (dp["nomeEstado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NomeEstado); Col = Col + 1; }
                        if (dp["iniciativaCriadorNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.IniciativaCriadorNome); Col = Col + 1; }
                        if (dp["iniciativaResponsavelNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.IniciativaResponsavelNome); Col = Col + 1; }
                        if (dp["iniciativaIntervinientes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.IniciativaIntervinientes); Col = Col + 1; }
                        if (dp["rececaoCriador"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RececaoCriador); Col = Col + 1; }
                        if (dp["rececaoResponsavel"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RececaoResponsavel); Col = Col + 1; }
                        if (dp["rececaoIntervinientes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RececaoIntervinientes); Col = Col + 1; }
                        if (dp["relatorioSimplificado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RelatorioSimplificado); Col = Col + 1; }

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
        public IActionResult ExportToExcelDownload_VisitasList(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Visitas\\" + "tmp\\" + sFileName;
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [HttpPost]
        public JsonResult GetVisitaDetails([FromBody] VisitasViewModel visita)
        {
            VisitasViewModel result = new VisitasViewModel();
            if (visita != null && !string.IsNullOrEmpty(visita.CodVisita))
                result = DBVisitas.ParseToViewModel(DBVisitas.GetByVisita(visita.CodVisita));
            else
            {
                result.IniciativaCriador = User.Identity.Name;
                result.IniciativaCriadorNome = DBUserConfigurations.GetById(User.Identity.Name).Nome;
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetVisitaTarefas([FromBody] VisitasViewModel visita)
        {
            List<VisitasTarefasViewModel> result = new List<VisitasTarefasViewModel>();
            if (visita != null && !string.IsNullOrEmpty(visita.CodVisita))
            {
                result = DBVisitasTarefas.ParseListToViewModel(DBVisitasTarefas.GetByVisita(visita.CodVisita));
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateVisita([FromBody] VisitasViewModel visita)
        {
            try
            {
                Visitas visitaDB = new Visitas();
                if (visita != null)
                {
                    bool autoGenId = true;
                    Configuração conf = DBConfigurations.GetById(1);
                    int entityNumerationConfId = conf.NumeracaoVisitas.Value;

                    visita.CodVisita = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, autoGenId, false);
                    visita.NomeCliente = !string.IsNullOrEmpty(visita.CodCliente) ? DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, visita.CodCliente).Name : "";
                    visita.NomeFornecedor = !string.IsNullOrEmpty(visita.CodFornecedor) ? DBNAV2017Fornecedores.GetFornecedorById(_config.NAVDatabaseName, _config.NAVCompanyName, visita.CodFornecedor).Name : "";
                    visita.NomeRegiao = !string.IsNullOrEmpty(visita.CodRegiao) ? DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, Dimensions.Region, "", visita.CodRegiao).FirstOrDefault().Name : "";
                    visita.NomeArea = !string.IsNullOrEmpty(visita.CodArea) ? DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, Dimensions.FunctionalArea, "", visita.CodArea).FirstOrDefault().Name : "";
                    visita.NomeCresp = !string.IsNullOrEmpty(visita.CodCresp) ? DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, Dimensions.ResponsabilityCenter, "", visita.CodCresp).FirstOrDefault().Name : "";
                    visita.NomeEstado = visita.CodEstado.HasValue ? DBVisitasEstados.GetByEstado((int)visita.CodEstado).Estado : "";
                    visita.IniciativaCriadorNome = !string.IsNullOrEmpty(visita.IniciativaCriador) ? DBUserConfigurations.GetById(visita.IniciativaCriador).Nome : "";
                    visita.IniciativaResponsavelNome = !string.IsNullOrEmpty(visita.IniciativaResponsavel) ? DBNAV2009Employees.GetAll(visita.IniciativaResponsavel, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault().Name : "";

                    visitaDB = DBVisitas.ParseToDB(visita);

                    if (DBVisitas.Create(visitaDB) != null)
                    {
                        visita.eReasonCode = 1;
                        visita.eMessage = "Visita criada com sucesso.";
                        return Json(visita);
                    }
                }

                visita.eReasonCode = 99;
                visita.eMessage = "Ocorreu um erro ao criar a Visita.";
                return Json(null);
            }
            catch (Exception ex)
            {
                visita.eReasonCode = 99;
                visita.eMessage = "Ocorreu um erro ao criar a Visita.";
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult UpdateVisita([FromBody] VisitasViewModel visita)
        {
            try
            {
                Visitas visitaDB = new Visitas();
                if (visita != null && !string.IsNullOrEmpty(visita.CodVisita))
                {
                    visita.NomeCliente = !string.IsNullOrEmpty(visita.CodCliente) ? DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, visita.CodCliente).Name : "";
                    visita.NomeFornecedor = !string.IsNullOrEmpty(visita.CodFornecedor) ? DBNAV2017Fornecedores.GetFornecedorById(_config.NAVDatabaseName, _config.NAVCompanyName, visita.CodFornecedor).Name : "";
                    visita.NomeRegiao = !string.IsNullOrEmpty(visita.CodRegiao) ? DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, Dimensions.Region, "", visita.CodRegiao).FirstOrDefault().Name : "";
                    visita.NomeArea = !string.IsNullOrEmpty(visita.CodArea) ? DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, Dimensions.FunctionalArea, "", visita.CodArea).FirstOrDefault().Name : "";
                    visita.NomeCresp = !string.IsNullOrEmpty(visita.CodCresp) ? DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, Dimensions.ResponsabilityCenter, "", visita.CodCresp).FirstOrDefault().Name : "";
                    visita.NomeEstado = visita.CodEstado.HasValue ? DBVisitasEstados.GetByEstado((int)visita.CodEstado).Estado : "";
                    visita.IniciativaCriadorNome = !string.IsNullOrEmpty(visita.IniciativaCriador) ? DBUserConfigurations.GetById(visita.IniciativaCriador).Nome : "";
                    visita.IniciativaResponsavelNome = !string.IsNullOrEmpty(visita.IniciativaResponsavel) ? DBNAV2009Employees.GetAll(visita.IniciativaResponsavel, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault().Name : "";
                    visitaDB = DBVisitas.ParseToDB(visita);

                    if (DBVisitas.Update(visitaDB) != null)
                    {
                        visita.eReasonCode = 1;
                        visita.eMessage = "Visita guardada com sucesso.";
                        return Json(visita);
                    }
                }

                visita.eReasonCode = 99;
                visita.eMessage = "Ocorreu um erro ao guardar a Visita.";
                return Json(null);
            }
            catch (Exception ex)
            {
                visita.eReasonCode = 99;
                visita.eMessage = "Ocorreu um erro ao guardar a Visita.";
                return Json(null);
            }
        }































    }
}