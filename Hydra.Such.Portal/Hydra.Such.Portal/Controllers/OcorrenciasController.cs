using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.ComprasML;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.ViewModel.Encomendas;
using Hydra.Such.Portal.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class OcorrenciasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GeneralConfigurations _generalConfig;

        public OcorrenciasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment, IOptions<GeneralConfigurations> appSettingsGeneral)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
            _generalConfig = appSettingsGeneral.Value;
        }

        public IActionResult OcorrenciasList()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Ocorrencias);
            ConfigUtilizadores user = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.CriarMedidasCorretivas = false;
                if (user != null && user.CriarMedidasCorretivas == true)
                    ViewBag.CriarMedidasCorretivas = true;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult OcorrenciasDetails(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Ocorrencias);
            ConfigUtilizadores user = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.Ocorrencia = id == null ? "" : id;
                ViewBag.CriarMedidasCorretivas = false;
                if (!string.IsNullOrEmpty(id) && user != null && user.CriarMedidasCorretivas == true)
                    ViewBag.CriarMedidasCorretivas = true;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetList([FromBody] JObject requestParams)
        {
            Boolean EstadoAtivas = Boolean.Parse(requestParams["ativas"].ToString());

            List<OcorrenciasViewModel> result = DBOcorrencias.ParseListToViewModel(DBOcorrencias.GetAllByState(EstadoAtivas));
            List<ConfiguracaoTabelas> Motivos = DBConfiguracaoTabelas.GetAllByTabela("OCORRENCIAS_MOTIVO");

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CodCentroResponsabilidade));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CodAreaFuncional));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CodCentroResponsabilidade));

            result.ForEach(x =>
            {
                if (x.CodEstado != null) x.NomeEstado = EnumerablesFixed.OcorrenciasEstado.Where(y => y.Id == x.CodEstado).FirstOrDefault().Value;
                if (x.GrauGravidade != null) x.GrauGravidadeTexto = EnumerablesFixed.OcorrenciasGrauGravidade.Where(y => y.Id == x.GrauGravidade).FirstOrDefault().Value;
                if (x.CodMotivo != null) x.CodMotivoTexto = Motivos.Where(y => y.ID == x.CodMotivo).FirstOrDefault().Descricao;
            });

            return Json(result);
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Ocorrencias([FromBody] List<OcorrenciasViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Ocorrencias\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Viaturas");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["codOcorrencia"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Ocorrência"); Col = Col + 1; }
                if (dp["nomeEstado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Estado"); Col = Col + 1; }
                if (dp["codFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Fornecedor"); Col = Col + 1; }
                if (dp["nomeFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Fornecedor"); Col = Col + 1; }
                if (dp["codEncomenda"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Encomenda"); Col = Col + 1; }
                if (dp["codProcedimento"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Procedimento"); Col = Col + 1; }
                if (dp["codRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Região"); Col = Col + 1; }
                if (dp["codAreaFuncional"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Área Funcional"); Col = Col + 1; }
                if (dp["codCentroResponsabilidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade"); Col = Col + 1; }
                if (dp["dataOcorrenciaTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Ocorrência"); Col = Col + 1; }
                if (dp["localEntrega"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Local Entrega"); Col = Col + 1; }
                if (dp["noDocExterno"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("No. Documento Externo"); Col = Col + 1; }
                if (dp["codArtigo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Artigo"); Col = Col + 1; }
                if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição"); Col = Col + 1; }
                if (dp["unidMedida"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Unidade Medida"); Col = Col + 1; }
                if (dp["quantidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Quantidade"); Col = Col + 1; }
                if (dp["codMotivoTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Motivo"); Col = Col + 1; }
                if (dp["motivoDescricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Motivo Descrição"); Col = Col + 1; }
                if (dp["grauGravidadeTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Grau Gravidade"); Col = Col + 1; }
                if (dp["observacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Observação"); Col = Col + 1; }
                if (dp["dataEnvioFornecedorTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Envio Fornecedor"); Col = Col + 1; }
                if (dp["dataReforcoTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Reforço"); Col = Col + 1; }
                if (dp["dataRespostaFornecedorTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Resposta Fornecedor"); Col = Col + 1; }
                if (dp["medidaCorretiva"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Medida Corretiva"); Col = Col + 1; }
                if (dp["utilizadorMedidaCorretiva"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Medida Corretiva Utilizador Criador"); Col = Col + 1; }
                if (dp["dataMedidaCorretivaTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Medida Corretiva Data Criação"); Col = Col + 1; }
                if (dp["utilizadorCriacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador Criador"); Col = Col + 1; }
                if (dp["dataCriacaoTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Criação"); Col = Col + 1; }
                if (dp["utilizadorModificacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador Modificação"); Col = Col + 1; }
                if (dp["dataModificacaoTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Modificação"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (OcorrenciasViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["codOcorrencia"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodOcorrencia); Col = Col + 1; }
                        if (dp["nomeEstado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NomeEstado); Col = Col + 1; }
                        if (dp["codFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodFornecedor); Col = Col + 1; }
                        if (dp["nomeFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NomeFornecedor); Col = Col + 1; }
                        if (dp["codEncomenda"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodEncomenda); Col = Col + 1; }
                        if (dp["codProcedimento"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodProcedimento); Col = Col + 1; }
                        if (dp["codRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodRegiao); Col = Col + 1; }
                        if (dp["codAreaFuncional"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodAreaFuncional); Col = Col + 1; }
                        if (dp["codCentroResponsabilidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodCentroResponsabilidade); Col = Col + 1; }
                        if (dp["dataOcorrenciaTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataOcorrenciaTexto); Col = Col + 1; }
                        if (dp["localEntrega"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.LocalEntrega); Col = Col + 1; }
                        if (dp["noDocExterno"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoDocExterno); Col = Col + 1; }
                        if (dp["codArtigo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodArtigo); Col = Col + 1; }
                        if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Descricao); Col = Col + 1; }
                        if (dp["unidMedida"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UnidMedida); Col = Col + 1; }
                        if (dp["quantidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Quantidade.ToString()); Col = Col + 1; }
                        if (dp["codMotivoTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodMotivoTexto); Col = Col + 1; }
                        if (dp["motivoDescricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.MotivoDescricao); Col = Col + 1; }
                        if (dp["grauGravidadeTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.GrauGravidadeTexto); Col = Col + 1; }
                        if (dp["observacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Observacao); Col = Col + 1; }
                        if (dp["dataEnvioFornecedorTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataEnvioFornecedorTexto); Col = Col + 1; }
                        if (dp["dataReforcoTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataReforcoTexto); Col = Col + 1; }
                        if (dp["dataRespostaFornecedorTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataRespostaFornecedorTexto); Col = Col + 1; }
                        if (dp["medidaCorretiva"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.MedidaCorretiva); Col = Col + 1; }
                        if (dp["utilizadorMedidaCorretiva"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UtilizadorMedidaCorretiva); Col = Col + 1; }
                        if (dp["dataMedidaCorretivaTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataMedidaCorretivaTexto); Col = Col + 1; }
                        if (dp["utilizadorCriacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UtilizadorCriacao); Col = Col + 1; }
                        if (dp["dataCriacaoTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataCriacaoTexto); Col = Col + 1; }
                        if (dp["utilizadorModificacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UtilizadorModificacao); Col = Col + 1; }
                        if (dp["dataModificacaoTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataModificacaoTexto); Col = Col + 1; }

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
        public IActionResult ExportToExcelDownload_Ocorrencias(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Ocorrencias\\" + "tmp\\" + sFileName;
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [HttpPost]
        public JsonResult CreateMedidaCorretiva([FromBody] JObject requestParams)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 99;
            result.eMessage = "Ocorreu um erro.";

            string codOcorrencia = requestParams["codOcorrencia"].ToString();
            string medidaCorretiva = requestParams["medidaCorretiva"].ToString();

            if (!string.IsNullOrEmpty(codOcorrencia))
            {
                if (!string.IsNullOrEmpty(medidaCorretiva))
                {
                    Ocorrencias Ocorrencia = DBOcorrencias.GetByID(codOcorrencia);

                    if (Ocorrencia != null)
                    {
                        Ocorrencia.CodEstado = 2; //Arquivado
                        Ocorrencia.MedidaCorretiva = medidaCorretiva;
                        Ocorrencia.DataMedidaCorretiva = DateTime.Now;
                        Ocorrencia.UtilizadorMedidaCorretiva = User.Identity.Name;

                        if (DBOcorrencias.Update(Ocorrencia) != null)
                        {
                            result.eReasonCode = 1;
                            result.eMessage = "A Medida Corretiva foi criada com sucesso.";
                        }
                        else
                        {
                            result.eReasonCode = 99;
                            result.eMessage = "Ocorreu um erro ao atualizar a Ocorrência.";
                        }
                    }
                    else
                    {
                        result.eReasonCode = 99;
                        result.eMessage = "Não foi possivel obter a Ocorrência.";
                    }
                }
                else
                {
                    result.eReasonCode = 99;
                    result.eMessage = "A Medida Corretiva é de preenchimento Obrigatório.";
                }
            }
            else
            {
                result.eReasonCode = 99;
                result.eMessage = "Não foi possivel obter o código da Ocorrência.";
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetOcorrenciaDetails([FromBody] OcorrenciasViewModel data)
        {
            OcorrenciasViewModel Ocorrencia = new OcorrenciasViewModel();
            if (data != null && !string.IsNullOrEmpty(data.CodOcorrencia))
            {
                Ocorrencia = DBOcorrencias.ParseToViewModel(DBOcorrencias.GetByID(data.CodOcorrencia));

                if (Ocorrencia.CodEstado != null) Ocorrencia.NomeEstado = EnumerablesFixed.OcorrenciasEstado.Where(y => y.Id == Ocorrencia.CodEstado).FirstOrDefault().Value;
                if (Ocorrencia.GrauGravidade != null) Ocorrencia.GrauGravidadeTexto = EnumerablesFixed.OcorrenciasGrauGravidade.Where(y => y.Id == Ocorrencia.GrauGravidade).FirstOrDefault().Value;

            }
            else
            {
                Ocorrencia.CodEstado = 1;
                Ocorrencia.NomeEstado = EnumerablesFixed.OcorrenciasEstado.Where(y => y.Id == Ocorrencia.CodEstado).FirstOrDefault().Value;
                Ocorrencia.UtilizadorCriacao = User.Identity.Name;
                Ocorrencia.DataCriacao = DateTime.Now;
                Ocorrencia.DataCriacaoTexto = Ocorrencia.DataCriacao.Value.ToString("yyyy-MM-dd");
            }
            return Json(Ocorrencia);
        }

        [HttpPost]
        public JsonResult CreateOcorrencia([FromBody] OcorrenciasViewModel data)
        {
            if (data != null && string.IsNullOrEmpty(data.CodOcorrencia))
            {
                int MaxID = DBOcorrencias.GetAll().Max(x => x.Ind) + 1;
                string CodOcorrencia = DateTime.Now.Year + "-" + MaxID.ToString().PadLeft(6, '0');

                data.CodOcorrencia = CodOcorrencia;
                data.UtilizadorCriacao = User.Identity.Name;
                data.DataCriacao = DateTime.Now;
                data.DataCriacaoTexto = data.DataCriacao.Value.ToString("yyyy-MM-dd");

                if (DBOcorrencias.Create(DBOcorrencias.ParseToDB(data)) != null)
                {
                    data.eReasonCode = 1;
                    data.eMessage = "Ocorrência Nº " + CodOcorrencia + " criada com sucesso.";
                }
                else
                {
                    data.eReasonCode = 2;
                    data.eMessage = "Ocorreu um erro ao criar a Ocorrência.";
                }
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateOcorrencia([FromBody] OcorrenciasViewModel data)
        {
            if (data != null && !string.IsNullOrEmpty(data.CodOcorrencia))
            {
                data.UtilizadorModificacao = User.Identity.Name;
                data.DataModificacao = DateTime.Now;
                data.DataModificacaoTexto = data.DataModificacao.Value.ToString("yyyy-MM-dd");

                if (DBOcorrencias.Update(DBOcorrencias.ParseToDB(data)) != null)
                {
                    data.eReasonCode = 1;
                    data.eMessage = "Ocorrência atualizada com sucesso.";
                }
                else
                {
                    data.eReasonCode = 2;
                    data.eMessage = "Ocorreu um erro ao atualizar.";
                }
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetEncomendaDetails([FromBody] OcorrenciasViewModel data)
        {
            if (data != null)
            {
                data.CodFornecedor = "";
                data.NomeFornecedor = "";
                data.CodRegiao = "";
                data.CodAreaFuncional = "";
                data.CodCentroResponsabilidade = "";
                //data.DataOcorrencia = null;
                //data.DataOcorrenciaTexto = "";
                data.LocalEntrega = "";
                data.NoDocExterno = "";
                data.CodArtigo = "";
                data.Descricao = "";
                data.UnidMedida = "";
                data.Quantidade = null;

                if (!string.IsNullOrEmpty(data.CodEncomenda))
                {
                    EncomendasViewModel Encomenda = new EncomendasViewModel();
                    Encomenda = DBNAV2017Encomendas.GetDetailsByNo(_config.NAVDatabaseName, _config.NAVCompanyName, data.CodEncomenda, "");
                    if (Encomenda == null)
                        Encomenda = DBNAV2017Encomendas.GetDetailsByNo(_config.NAVDatabaseName, _config.NAVCompanyName, data.CodEncomenda, "", 1);

                    if (Encomenda != null)
                    {
                        data.CodFornecedor = Encomenda.PayToVendorNo;
                        data.NomeFornecedor = Encomenda.PayToName;
                        data.CodRegiao = Encomenda.RegionId;
                        data.CodAreaFuncional = Encomenda.FunctionalAreaId;
                        data.CodCentroResponsabilidade = Encomenda.RespCenterId;
                        //data.DataOcorrencia = Encomenda.OrderDate;
                        //data.DataOcorrenciaTexto = data.DataOcorrencia.Value.ToString("yyyy-MM-dd");
                        data.LocalEntrega = Encomenda.ShipToName;
                        data.NoDocExterno = Encomenda.VendorShipmentNo;
                    }
                }
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetProcedimentoDetails([FromBody] OcorrenciasViewModel data)
        {
            if (data != null)
            {
                data.CodFornecedor = "";
                data.NomeFornecedor = "";
                data.CodRegiao = "";
                data.CodAreaFuncional = "";
                data.CodCentroResponsabilidade = "";
                //data.DataOcorrencia = null;
                //data.DataOcorrenciaTexto = "";
                data.LocalEntrega = "";
                data.NoDocExterno = "";
                data.CodArtigo = "";
                data.Descricao = "";
                data.UnidMedida = "";
                data.Quantidade = null;

                if (!string.IsNullOrEmpty(data.CodEncomenda))
                {
                    EncomendasViewModel Encomenda = DBNAV2017Encomendas.GetDetailsByNo(_config.NAVDatabaseName, _config.NAVCompanyName, data.CodEncomenda, "");

                    if (Encomenda != null)
                    {
                        data.CodFornecedor = Encomenda.PayToVendorNo;
                        data.NomeFornecedor = Encomenda.PayToName;
                        data.CodRegiao = Encomenda.RegionId;
                        data.CodAreaFuncional = Encomenda.FunctionalAreaId;
                        data.CodCentroResponsabilidade = Encomenda.RespCenterId;
                        data.DataOcorrencia = Encomenda.OrderDate;
                        data.DataOcorrenciaTexto = data.DataOcorrencia.Value.ToString("yyyy-MM-dd");
                        data.LocalEntrega = Encomenda.ShipToName;
                        data.NoDocExterno = Encomenda.VendorShipmentNo;
                    }
                }
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetEncomendaLineDetails([FromBody] OcorrenciasViewModel data)
        {
            if (data != null)
            {
                data.Descricao = "";
                data.UnidMedida = "";
                data.Quantidade = null;
                List<EncomendasLinhasViewModel> AllLinhas = new List<EncomendasLinhasViewModel>();
                EncomendasLinhasViewModel Linha = new EncomendasLinhasViewModel();

                if (!string.IsNullOrEmpty(data.CodEncomenda))
                {
                    AllLinhas = DBNAV2017Encomendas.ListLinesByNo(_config.NAVDatabaseName, _config.NAVCompanyName, data.CodEncomenda, "", 0);

                    if (AllLinhas == null || AllLinhas.Count == 0)
                    {
                        AllLinhas = DBNAV2017Encomendas.ListLinesByNo(_config.NAVDatabaseName, _config.NAVCompanyName, data.CodEncomenda, "", 1);
                    }

                    if (AllLinhas != null && AllLinhas.Count > 0)
                    {
                        Linha = AllLinhas.FirstOrDefault(x => x.No == data.CodArtigo);

                        if (Linha != null)
                        {
                            data.Descricao = Linha.Description;
                            data.UnidMedida = Linha.UnitOfMeasure;
                            data.Quantidade = Linha.Quantity;
                        }
                    }
                }
            }

            return Json(data);
        }


    }
}