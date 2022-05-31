using Hydra.Such.Data;
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

            List<VisitasViewModel> result = DBVisitas.ParseListToViewModel(DBVisitas.GetAllToList()).OrderByDescending(x => x.DataHoraCriacao).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetListAllAtivas([FromBody] JObject requestParams)
        {
            Boolean ativas = Boolean.Parse(requestParams["ativas"].ToString());

            List<VisitasViewModel> result = DBVisitas.ParseListToViewModel(DBVisitas.GetAllAtivas(ativas)).OrderByDescending(x => x.DataHoraCriacao).ToList();

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
            {
                Visitas tst = DBVisitas.GetByVisita(visita.CodVisita);
                result = DBVisitas.ParseToViewModel(tst);


                //result = DBVisitas.ParseToViewModel(DBVisitas.GetByVisita(visita.CodVisita));
            }
            else
            {
                result.IniciativaCriador = User.Identity.Name;
                result.IniciativaCriadorNome = DBUserConfigurations.GetById(User.Identity.Name).Nome;
                result.CodEstado = 1;
                result.NomeEstado = DBVisitasEstados.GetByEstado(1).Estado;
            }

            result.UserLogin = User.Identity.Name;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetVisitaTarefas([FromBody] VisitasViewModel visita)
        {
            List<VisitasTarefasViewModel> result = new List<VisitasTarefasViewModel>();
            if (visita != null && !string.IsNullOrEmpty(visita.CodVisita))
            {
                result = DBVisitasTarefas.ParseListToViewModel(DBVisitasTarefas.GetByVisita(visita.CodVisita)).OrderBy(x => x.Ordem).ToList();
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetVisitaContratos([FromBody] VisitasViewModel visita)
        {
            List<VisitasContratosViewModel> result = new List<VisitasContratosViewModel>();
            if (visita != null && !string.IsNullOrEmpty(visita.CodVisita))
            {
                result = DBVisitasContratos.ParseListToViewModel(DBVisitasContratos.GetByVisita(visita.CodVisita)).OrderBy(x => x.NoContrato).ToList();
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetVisitaTarefasOpcoes()
        {
            List<VisitasTarefasTarefasViewModel> result = new List<VisitasTarefasTarefasViewModel>();
            result = DBVisitasTarefasTarefas.ParseListToViewModel(DBVisitasTarefasTarefas.GetAll()).OrderBy(x => x.CodTarefa).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateVisita([FromBody] VisitasViewModel visita)
        {
            try
            {
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
                    visita.UtilizadorCriacao = User.Identity.Name;
                    visita.UtilizadorCriacaoNome = !string.IsNullOrEmpty(visita.UtilizadorCriacao) ? DBUserConfigurations.GetById(visita.UtilizadorCriacao).Nome : "";

                    Visitas visitaDB = DBVisitas.ParseToDB(visita);

                    if (visitaDB != null && DBVisitas.Create(visitaDB) != null)
                    {
                        visita.eReasonCode = 1;
                        visita.eMessage = "Visita criada com sucesso.";
                        return Json(visita);
                    }
                }

                visita.eReasonCode = 99;
                visita.eMessage = "Ocorreu um erro ao criar a Visita.";
                return Json(visita);
            }
            catch (Exception ex)
            {
                visita.eReasonCode = 99;
                visita.eMessage = "Ocorreu um erro ao criar a Visita.";
                return Json(visita);
            }
        }

        [HttpPost]
        public JsonResult UpdateVisita([FromBody] VisitasViewModel visita)
        {
            try
            {
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
                    visita.UtilizadorModificacao = User.Identity.Name;
                    visita.UtilizadorModificacaoNome = !string.IsNullOrEmpty(visita.UtilizadorModificacao) ? DBUserConfigurations.GetById(visita.UtilizadorModificacao).Nome : "";

                    Visitas visitaDB = DBVisitas.ParseToDB(visita);

                    if (visitaDB != null && DBVisitas.Update(visitaDB) != null)
                    {
                        visita.eReasonCode = 1;
                        visita.eMessage = "Visita guardada com sucesso.";
                        return Json(visita);
                    }
                }

                visita.eReasonCode = 99;
                visita.eMessage = "Ocorreu um erro ao guardar a Visita.";
                return Json(visita);
            }
            catch (Exception ex)
            {
                visita.eReasonCode = 99;
                visita.eMessage = "Ocorreu um erro ao guardar a Visita.";
                return Json(visita);
            }
        }

        [HttpPost]
        public JsonResult DeleteVisita([FromBody] VisitasViewModel visita)
        {
            try
            {
                if (visita != null && !string.IsNullOrEmpty(visita.CodVisita))
                {
                    Visitas visitaDB = DBVisitas.GetByVisita(visita.CodVisita);
                    if (visitaDB != null && !string.IsNullOrEmpty(visitaDB.IniciativaCriador) && visitaDB.IniciativaCriador == User.Identity.Name)
                    {
                        List<VisitasTarefas> tarefasDB = DBVisitasTarefas.GetByVisita(visita.CodVisita);

                        if (tarefasDB != null && tarefasDB.Count > 0)
                        {
                            tarefasDB.ForEach(tarefa =>
                            {
                                DBVisitasTarefas.Delete(tarefa);
                            });
                        }

                        List<VisitasContratos> contratosDB = DBVisitasContratos.GetByVisita(visita.CodVisita);

                        if (contratosDB != null && contratosDB.Count > 0)
                        {
                            contratosDB.ForEach(contrato =>
                            {
                                DBVisitasContratos.Delete(contrato);
                            });
                        }

                        if (DBVisitas.Delete(visitaDB) == true)
                        {
                            visita.eReasonCode = 1;
                            visita.eMessage = "Visita eliminada com sucesso.";
                            return Json(visita);
                        }
                    }
                }

                visita.eReasonCode = 99;
                visita.eMessage = "Ocorreu um erro ao eliminar a Visita.";
                return Json(visita);
            }
            catch (Exception ex)
            {
                visita.eReasonCode = 99;
                visita.eMessage = "Ocorreu um erro ao eliminar a Visita.";
                return Json(visita);
            }
        }

        [HttpPost]
        public JsonResult DuplicarVisita([FromBody] VisitasViewModel visitaOriginal)
        {
            try
            {
                if (visitaOriginal != null)
                {
                    Visitas VisitaDuplicada = DBVisitas.ParseToDB(visitaOriginal);

                    bool autoGenId = true;
                    Configuração conf = DBConfigurations.GetById(1);
                    int entityNumerationConfId = conf.NumeracaoVisitas.Value;

                    VisitaDuplicada.CodVisita = DBNumerationConfigurations.GetNextNumeration(entityNumerationConfId, autoGenId, false);
                    VisitaDuplicada.NomeCliente = !string.IsNullOrEmpty(VisitaDuplicada.CodCliente) ? DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, VisitaDuplicada.CodCliente).Name : "";
                    VisitaDuplicada.NomeFornecedor = !string.IsNullOrEmpty(VisitaDuplicada.CodFornecedor) ? DBNAV2017Fornecedores.GetFornecedorById(_config.NAVDatabaseName, _config.NAVCompanyName, VisitaDuplicada.CodFornecedor).Name : "";
                    VisitaDuplicada.NomeRegiao = !string.IsNullOrEmpty(VisitaDuplicada.CodRegiao) ? DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, Dimensions.Region, "", VisitaDuplicada.CodRegiao).FirstOrDefault().Name : "";
                    VisitaDuplicada.NomeArea = !string.IsNullOrEmpty(VisitaDuplicada.CodArea) ? DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, Dimensions.FunctionalArea, "", VisitaDuplicada.CodArea).FirstOrDefault().Name : "";
                    VisitaDuplicada.NomeCresp = !string.IsNullOrEmpty(VisitaDuplicada.CodCresp) ? DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, Dimensions.ResponsabilityCenter, "", VisitaDuplicada.CodCresp).FirstOrDefault().Name : "";
                    VisitaDuplicada.CodEstado = 1;
                    VisitaDuplicada.NomeEstado = VisitaDuplicada.CodEstado.HasValue ? DBVisitasEstados.GetByEstado((int)VisitaDuplicada.CodEstado).Estado : "";
                    VisitaDuplicada.IniciativaCriador = User.Identity.Name;
                    VisitaDuplicada.IniciativaCriadorNome = !string.IsNullOrEmpty(VisitaDuplicada.IniciativaCriador) ? DBUserConfigurations.GetById(VisitaDuplicada.IniciativaCriador).Nome : "";
                    VisitaDuplicada.IniciativaResponsavelNome = !string.IsNullOrEmpty(VisitaDuplicada.IniciativaResponsavel) ? DBNAV2009Employees.GetAll(VisitaDuplicada.IniciativaResponsavel, _config.NAV2009DatabaseName, _config.NAV2009CompanyName).FirstOrDefault().Name : "";
                    VisitaDuplicada.UtilizadorCriacao = User.Identity.Name;
                    VisitaDuplicada.UtilizadorCriacaoNome = !string.IsNullOrEmpty(VisitaDuplicada.UtilizadorCriacao) ? DBUserConfigurations.GetById(VisitaDuplicada.UtilizadorCriacao).Nome : "";
                    VisitaDuplicada.DataHoraCriacao = DateTime.Now;
                    VisitaDuplicada.UtilizadorModificacao = null;
                    VisitaDuplicada.UtilizadorModificacaoNome = null;
                    VisitaDuplicada.DataHoraModificacao = null;

                    if (VisitaDuplicada != null && DBVisitas.Create(VisitaDuplicada) != null)
                    {
                        List<VisitasTarefas> AllTarefasOriginais = DBVisitasTarefas.GetByVisita(visitaOriginal.CodVisita);

                        if (AllTarefasOriginais != null && AllTarefasOriginais.Count > 0)
                        {
                            AllTarefasOriginais.ForEach(tarefaOriginal =>
                            {
                                VisitasTarefas tarefaDuplicada = tarefaOriginal;

                                tarefaDuplicada.CodVisita = VisitaDuplicada.CodVisita;
                                tarefaDuplicada.UtilizadorCriacao = User.Identity.Name;
                                tarefaDuplicada.DataHoraCriacao = DateTime.Now;
                                tarefaDuplicada.UtilizadorModificacao = null;
                                tarefaDuplicada.DataHoraModificacao = null;

                                DBVisitasTarefas.Create(tarefaDuplicada);
                            });
                        }

                        List<VisitasContratos> AllContratosOriginais = DBVisitasContratos.GetByVisita(visitaOriginal.CodVisita);

                        if (AllContratosOriginais != null && AllContratosOriginais.Count > 0)
                        {
                            AllContratosOriginais.ForEach(contratoOriginal =>
                            {
                                VisitasContratos contratoDuplicado = contratoOriginal;

                                contratoDuplicado.CodVisita = VisitaDuplicada.CodVisita;
                                contratoDuplicado.UtilizadorCriacao = User.Identity.Name;
                                contratoDuplicado.DataHoraCriacao = DateTime.Now;
                                contratoDuplicado.UtilizadorModificacao = null;
                                contratoDuplicado.DataHoraModificacao = null;

                                DBVisitasContratos.Create(contratoDuplicado);
                            });
                        }

                        visitaOriginal.eReasonCode = 1;
                        visitaOriginal.eMessage = "Visita Duplicada com sucesso com o código Nº " + VisitaDuplicada.CodVisita;
                        return Json(visitaOriginal);
                    }
                }

                visitaOriginal.eReasonCode = 99;
                visitaOriginal.eMessage = "Ocorreu um erro ao Duplicar a Visita.";
                return Json(visitaOriginal);
            }
            catch (Exception ex)
            {
                visitaOriginal.eReasonCode = 99;
                visitaOriginal.eMessage = "Ocorreu um erro ao Duplicar a Visita.";
                return Json(visitaOriginal);
            }
        }

        [HttpPost]
        public JsonResult CreateTarefa([FromBody] VisitasTarefasViewModel tarefa)
        {
            try
            {
                if (tarefa != null && !string.IsNullOrEmpty(tarefa.CodVisita) && tarefa.Ordem.HasValue)
                {
                    if (!string.IsNullOrEmpty(tarefa.DataTexto)) tarefa.Data = Convert.ToDateTime(tarefa.DataTexto);
                    if (!string.IsNullOrEmpty(tarefa.DuracaoTexto)) tarefa.Duracao = TimeSpan.Parse(tarefa.DuracaoTexto); else tarefa.Duracao = TimeSpan.MinValue;
                    if (tarefa.CodTarefa.HasValue == true) tarefa.Tarefa = "";
                    if (!string.IsNullOrEmpty(tarefa.Tarefa)) tarefa.CodTarefa = null;
                    tarefa.UtilizadorCriacao = User.Identity.Name;

                    VisitasTarefas tarefaDB = DBVisitasTarefas.ParseToDB(tarefa);

                    if (tarefaDB != null && DBVisitasTarefas.Create(tarefaDB) != null)
                    {
                        List<VisitasTarefas> AllTarefas = DBVisitasTarefas.GetByVisita(tarefa.CodVisita);
                        TimeSpan totalTarefas = AllTarefas.Aggregate(TimeSpan.Zero, (sumSoFar, nextMyObject) => sumSoFar + (TimeSpan)nextMyObject.Duracao);

                        Visitas Visita = DBVisitas.GetByVisita(tarefa.CodVisita);
                        Visita.TarefasTempoTotal = totalTarefas;
                        Visita.UtilizadorModificacao = User.Identity.Name;
                        DBVisitas.Update(Visita);

                        tarefa.eReasonCode = 1;
                        tarefa.eMessage = "Tarefa criada com sucesso.";
                        return Json(tarefa);
                    }
                }

                tarefa.eReasonCode = 99;
                tarefa.eMessage = "Ocorreu um erro ao criar a Tarefa.";
                return Json(tarefa);
            }
            catch (Exception ex)
            {
                tarefa.eReasonCode = 99;
                tarefa.eMessage = "Ocorreu um erro ao criar a Tarefa.";
                return Json(tarefa);
            }
        }

        [HttpPost]
        public JsonResult UpdateTarefa([FromBody] VisitasTarefasViewModel tarefa)
        {
            try
            {
                if (tarefa != null && !string.IsNullOrEmpty(tarefa.CodVisita) && tarefa.Ordem.HasValue)
                {
                    if (!string.IsNullOrEmpty(tarefa.DataTexto)) tarefa.Data = Convert.ToDateTime(tarefa.DataTexto);
                    if (!string.IsNullOrEmpty(tarefa.DuracaoTexto)) tarefa.Duracao = TimeSpan.Parse(tarefa.DuracaoTexto);
                    tarefa.UtilizadorModificacao = User.Identity.Name;

                    VisitasTarefas tarefaDB = DBVisitasTarefas.ParseToDB(tarefa);

                    if (tarefaDB != null && DBVisitasTarefas.Update(tarefaDB) != null)
                    {
                        tarefa.eReasonCode = 1;
                        tarefa.eMessage = "Tarefa guardada com sucesso.";
                        return Json(tarefa);
                    }
                }

                tarefa.eReasonCode = 99;
                tarefa.eMessage = "Ocorreu um erro ao guardar a Tarefa.";
                return Json(tarefa);
            }
            catch (Exception ex)
            {
                tarefa.eReasonCode = 99;
                tarefa.eMessage = "Ocorreu um erro ao guardar a Tarefa.";
                return Json(tarefa);
            }
        }

        [HttpPost]
        public JsonResult DeleteTarefa([FromBody] VisitasTarefasViewModel tarefa)
        {
            try
            {
                if (tarefa != null && !string.IsNullOrEmpty(tarefa.CodVisita) && tarefa.Ordem.HasValue)
                {
                    VisitasTarefas tarefaDB = DBVisitasTarefas.GetByID(tarefa.CodVisita, (int)tarefa.Ordem);

                    if (tarefaDB != null && DBVisitasTarefas.Delete(tarefaDB) == true)
                    {
                        tarefa.eReasonCode = 1;
                        tarefa.eMessage = "Tarefa eliminada com sucesso.";
                        return Json(tarefa);
                    }
                }

                tarefa.eReasonCode = 99;
                tarefa.eMessage = "Ocorreu um erro ao eliminar a Tarefa.";
                return Json(tarefa);
            }
            catch (Exception ex)
            {
                tarefa.eReasonCode = 99;
                tarefa.eMessage = "Ocorreu um erro ao eliminar a Tarefa.";
                return Json(tarefa);
            }
        }

        [HttpPost]
        public JsonResult CreateContrato([FromBody] VisitasContratosViewModel contrato)
        {
            try
            {
                if (contrato != null && !string.IsNullOrEmpty(contrato.NoContrato))
                {
                    contrato.UtilizadorCriacao = User.Identity.Name;
                    contrato.DataHoraCriacao = DateTime.Now;

                    if (DBVisitasContratos.Create(DBVisitasContratos.ParseToDB(contrato)) != null)
                    {
                        contrato.eReasonCode = 1;
                        contrato.eMessage = "Contrato criado com sucesso.";
                        return Json(contrato);
                    }
                }

                contrato.eReasonCode = 99;
                contrato.eMessage = "Ocorreu um erro ao criar o Contrato.";
                return Json(contrato);
            }
            catch (Exception ex)
            {
                contrato.eReasonCode = 99;
                contrato.eMessage = "Ocorreu um erro ao criar o Contrato.";
                return Json(contrato);
            }
        }

        [HttpPost]
        public JsonResult DeleteContrato([FromBody] VisitasContratosViewModel contrato)
        {
            try
            {
                if (contrato != null && !string.IsNullOrEmpty(contrato.NoContrato))
                {
                    VisitasContratos contratoDB = DBVisitasContratos.GetByID(contrato.CodVisita, contrato.NoContrato);

                    if (contratoDB != null && DBVisitasContratos.Delete(contratoDB) == true)
                    {
                        contrato.eReasonCode = 1;
                        contrato.eMessage = "Contrato eliminado com sucesso.";
                        return Json(contrato);
                    }
                }

                contrato.eReasonCode = 99;
                contrato.eMessage = "Ocorreu um erro ao eliminar o Contrato.";
                return Json(contrato);
            }
            catch (Exception ex)
            {
                contrato.eReasonCode = 99;
                contrato.eMessage = "Ocorreu um erro ao eliminar o Contrato.";
                return Json(contrato);
            }
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
        [Route("Visitas/FileUpload")]
        [Route("Visitas/FileUpload/{id}")]
        public JsonResult FileUpload(string id)
        {
            try
            {
                var files = Request.Form.Files;
                string full_filename;
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
                            full_filename = id + "_" + filename;
                            var path = Path.Combine(_generalConfig.FileUploadFolder + "Visitas\\", full_filename);

                            using (FileStream dd = new FileStream(path, FileMode.CreateNew))
                            {
                                file.CopyTo(dd);
                                dd.Dispose();

                                Anexos newfile = new Anexos();
                                newfile.NºOrigem = id;
                                newfile.UrlAnexo = full_filename;
                                newfile.TipoOrigem = TipoOrigemAnexos.Visitas;
                                newfile.DataHoraCriação = DateTime.Now;
                                newfile.UtilizadorCriação = User.Identity.Name;

                                DBAttachments.Create(newfile);
                                if (newfile.NºLinha == 0)
                                {
                                    System.IO.File.Delete(path);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(true);
        }

        [HttpPost]
        public JsonResult LoadAttachments([FromBody] JObject requestParams)
        {
            string id = requestParams["id"].ToString();
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Visitas);

            List<Anexos> list = DBAttachments.GetById(id);

            if (UPerm != null && UPerm.Update == false)
            {
                list.RemoveAll(x => x.Visivel != true);
            }

            List<AttachmentsViewModel> attach = new List<AttachmentsViewModel>();
            list.ForEach(x => attach.Add(DBAttachments.ParseToViewModel(x)));
            return Json(attach);
        }

        [HttpGet]
        [Route("Visitas/DownloadFile")]
        [Route("Visitas/DownloadFile/{id}")]
        public FileStreamResult DownloadFile(string id)
        {
            return new FileStreamResult(new FileStream(_generalConfig.FileUploadFolder + "Visitas\\" + id, FileMode.Open), "application/xlsx");
        }

        [HttpPost]
        public JsonResult DeleteAttachments([FromBody] AttachmentsViewModel requestParams)
        {
            try
            {
                System.IO.File.Delete(_generalConfig.FileUploadFolder + "Visitas\\" + requestParams.Url);
                DBAttachments.Delete(DBAttachments.ParseToDB(requestParams));
                requestParams.eReasonCode = 1;

            }
            catch (Exception ex)
            {
                requestParams.eReasonCode = 2;
                return Json(requestParams);
            }
            return Json(requestParams);
        }




























    }
}