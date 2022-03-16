using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
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

            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<NAVFornecedoresViewModel> AllFornecedores = DBNAV2017Fornecedores.GetFornecedores(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<VisitasEstados> AllEstados = DBVisitasEstados.GetAll();
            List<ConfigUtilizadores> AllUsers = DBUserConfigurations.GetAll();
            List<NAVEmployeeViewModel> AllEmployees = DBNAV2009Employees.GetAll("", _config.NAV2009DatabaseName, _config.NAV2009CompanyName);

            result.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.CodCliente)) x.ClienteTexto = AllClients.FirstOrDefault(y => y.No_ == x.CodCliente).Name; else x.ClienteTexto = "";
                if (!string.IsNullOrEmpty(x.CodFornecedor)) x.FornecedorTexto = AllFornecedores.FirstOrDefault(y => y.No == x.CodFornecedor).Name; else x.FornecedorTexto = "";
                if (x.InicioDataHora.HasValue) x.InicioDataTexto = x.InicioDataHora.Value.ToString("yyyy-MM-dd"); else x.InicioDataTexto = "";
                if (x.InicioDataHora.HasValue) x.InicioHoraTexto = x.InicioDataHora.Value.ToString("HH:mm"); else x.InicioHoraTexto = "";
                if (x.FimDataHora.HasValue) x.FimDataTexto = x.FimDataHora.Value.ToString("yyyy-MM-dd"); else x.FimDataTexto = "";
                if (x.FimDataHora.HasValue) x.FimHoraTexto = x.FimDataHora.Value.ToString("HH:mm"); else x.FimHoraTexto = "";
                if (x.CodEstado.HasValue) x.EstadoTexto = AllEstados.FirstOrDefault(y => y.CodEstado == x.CodEstado).Estado; else x.EstadoTexto = "";
                if (!string.IsNullOrEmpty(x.IniciativaCriador)) x.IniciativaCriadorTexto = AllUsers.FirstOrDefault(y => y.IdUtilizador == x.IniciativaCriador).Nome; else x.IniciativaCriadorTexto = "";
                if (!string.IsNullOrEmpty(x.IniciativaResponsavel)) x.IniciativaResponsavelTexto = AllEmployees.FirstOrDefault(y => y.No == x.IniciativaResponsavel).Name; else x.IniciativaResponsavelTexto = "";
            });

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
                if (dp["clienteTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["clienteTexto"]["label"].ToString()); Col = Col + 1; }
                if (dp["codFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["codFornecedor"]["label"].ToString()); Col = Col + 1; }
                if (dp["fornecedorTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["fornecedorTexto"]["label"].ToString()); Col = Col + 1; }
                if (dp["entidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["entidade"]["label"].ToString()); Col = Col + 1; }
                if (dp["codRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["codRegiao"]["label"].ToString()); Col = Col + 1; }
                if (dp["codArea"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["codArea"]["label"].ToString()); Col = Col + 1; }
                if (dp["codCresp"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["codCresp"]["label"].ToString()); Col = Col + 1; }
                if (dp["inicioDataTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["inicioDataTexto"]["label"].ToString()); Col = Col + 1; }
                if (dp["inicioHoraTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["inicioHoraTexto"]["label"].ToString()); Col = Col + 1; }
                if (dp["fimDataTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["fimDataTexto"]["label"].ToString()); Col = Col + 1; }
                if (dp["fimHoraTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["fimHoraTexto"]["label"].ToString()); Col = Col + 1; }
                if (dp["estadoTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["estadoTexto"]["label"].ToString()); Col = Col + 1; }
                if (dp["iniciativaCriadorTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["iniciativaCriadorTexto"]["label"].ToString()); Col = Col + 1; }
                if (dp["iniciativaResponsavelTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(dp["iniciativaResponsavelTexto"]["label"].ToString()); Col = Col + 1; }
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
                        if (dp["clienteTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClienteTexto); Col = Col + 1; }
                        if (dp["codFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodFornecedor); Col = Col + 1; }
                        if (dp["fornecedorTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FornecedorTexto); Col = Col + 1; }
                        if (dp["entidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Entidade); Col = Col + 1; }
                        if (dp["codRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodRegiao); Col = Col + 1; }
                        if (dp["codArea"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodArea); Col = Col + 1; }
                        if (dp["codCresp"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodCresp); Col = Col + 1; }
                        if (dp["inicioDataTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.InicioDataTexto); Col = Col + 1; }
                        if (dp["inicioHoraTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.InicioHoraTexto); Col = Col + 1; }
                        if (dp["fimDataTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FimDataTexto); Col = Col + 1; }
                        if (dp["fimHoraTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FimHoraTexto); Col = Col + 1; }
                        if (dp["estadoTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.EstadoTexto); Col = Col + 1; }
                        if (dp["iniciativaCriadorTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.IniciativaCriadorTexto); Col = Col + 1; }
                        if (dp["iniciativaResponsavelTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.IniciativaResponsavelTexto); Col = Col + 1; }
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
            {
                result = DBVisitas.ParseToViewModel(DBVisitas.GetByVisita(visita.CodVisita));

                List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                List<NAVFornecedoresViewModel> AllFornecedores = DBNAV2017Fornecedores.GetFornecedores(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                List<VisitasEstados> AllEstados = DBVisitasEstados.GetAll();
                List<ConfigUtilizadores> AllUsers = DBUserConfigurations.GetAll();
                List<NAVEmployeeViewModel> AllEmployees = DBNAV2009Employees.GetAll("", _config.NAV2009DatabaseName, _config.NAV2009CompanyName);

                if (!string.IsNullOrEmpty(result.CodCliente)) result.ClienteTexto = AllClients.FirstOrDefault(y => y.No_ == result.CodCliente).Name; else result.ClienteTexto = "";
                if (!string.IsNullOrEmpty(result.CodFornecedor)) result.FornecedorTexto = AllFornecedores.FirstOrDefault(y => y.No == result.CodFornecedor).Name; else result.FornecedorTexto = "";
                if (result.CodEstado.HasValue) result.EstadoTexto = AllEstados.FirstOrDefault(y => y.CodEstado == result.CodEstado).Estado; else result.EstadoTexto = "";
                if (!string.IsNullOrEmpty(result.IniciativaCriador)) result.IniciativaCriadorTexto = AllUsers.FirstOrDefault(y => y.IdUtilizador == result.IniciativaCriador).Nome; else result.IniciativaCriadorTexto = "";
                if (!string.IsNullOrEmpty(result.IniciativaResponsavel)) result.IniciativaResponsavelTexto = AllEmployees.FirstOrDefault(y => y.No == result.IniciativaResponsavel).Name; else result.IniciativaResponsavelTexto = "";
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
































    }
}