using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.PedidoCotacao;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.PedidoCotacao;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Controllers
{
    public class ConsultaMercadoController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ConsultaMercadoController(IHostingEnvironment _hostingEnvironment)
        {
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult ConsultaMercado()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.PedidoCotacao);

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

        [HttpPost]
        public JsonResult GetAllConsultaMercado()
        {
            List<ConsultaMercado> result = DBConsultaMercado.GetAllConsultaMercadoToList();
            List<ConsultaMercadoView> list = new List<ConsultaMercadoView>();

            foreach (ConsultaMercado cm in result)
            {
                list.Add(DBConsultaMercado.CastConsultaMercadoToView(cm));
            }

            //return Json(result);
            return Json(list);
        }


        public IActionResult DetalheConsultaMercado(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.PedidoCotacao);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.No = id == null ? "" : id;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }


        [HttpPost]
        public JsonResult GetDetalheConsultaMercado([FromBody] ConsultaMercadoView data)
        {
            if (data != null)
            {
                ConsultaMercado consultaMercado = DBConsultaMercado.GetDetalheConsultaMercado(data.NumConsultaMercado);

                if (consultaMercado != null)
                {
                    ConsultaMercadoView result = DBConsultaMercado.CastConsultaMercadoToView(consultaMercado);

                    return Json(result);
                }

                return Json(new ConsultaMercadoView());
            }
            return Json(false);
        }


        [HttpPost]
        public JsonResult CreateConsultaMercado()
        {
            ConsultaMercado consultaMercado = DBConsultaMercado.Create(User.Identity.Name);
            
            if (consultaMercado == null || consultaMercado.NumConsultaMercado == "")
                return Json("");

            return Json(consultaMercado.NumConsultaMercado);

        }


        [HttpPost]
        public JsonResult UpdateConsultaMercado([FromBody] ConsultaMercadoView data)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (data != null)
                {
                    ConsultaMercado consultaMercado = DBConsultaMercado.Update(data);
                    if (consultaMercado == null)
                    {
                        result.eReasonCode = 1;
                        result.eMessage = "Ocorreu um erro";
                        return Json(result);
                    }

                    result.eReasonCode = 0;
                    result.eMessage = "Sucesso";
                    return Json(result);
                }
                else
                {
                    result.eReasonCode = -1;
                    result.eMessage = "Sem dados";
                    return Json(result);
                }
            }
            catch (Exception e)
            {
                result.eReasonCode = 1;
                result.eMessage = "Ocorreu um erro";
                return Json(result);
            }
        }

        [HttpPost]
        public JsonResult DeleteConsultaMercado([FromBody] ConsultaMercadoView data)
        {
            ErrorHandler result = new ErrorHandler();
            if (data != null)
            {
                if (DBConsultaMercado.Delete(data.NumConsultaMercado))
                {
                    result = new ErrorHandler()
                    {
                        eReasonCode = 0,
                        eMessage = "Consulta ao Mercado removida com sucesso"
                    };
                }
                else
                {
                    result = new ErrorHandler()
                    {
                        eReasonCode = 4,
                        eMessage = "Não foi possível remover a Consulta ao Mercado"
                    };
                }
                return Json(result);
            }
            else
            {
                result = new ErrorHandler()
                {
                    eReasonCode = -1,
                    eMessage = "Sem dados"
                };
                
                return Json(result);
            }
        }

        #region Linhas Consulta Mercado

        [HttpPost]
        public JsonResult CreateLinhaConsultaMercado([FromBody] LinhasConsultaMercadoView data)
        {
            bool result = false;
            try
            {
                LinhasConsultaMercado linhaConsultaMercado = new LinhasConsultaMercado();
                linhaConsultaMercado.CodActividade = data.CodActividade;
                linhaConsultaMercado.CodAreaFuncional = data.CodAreaFuncional;
                linhaConsultaMercado.CodCentroResponsabilidade = data.CodCentroResponsabilidade;
                linhaConsultaMercado.CodLocalizacao = data.CodLocalizacao;
                linhaConsultaMercado.CodProduto = data.CodProduto;
                linhaConsultaMercado.CodRegiao = data.CodRegiao;
                linhaConsultaMercado.CodUnidadeMedida = data.CodUnidadeMedida;
                linhaConsultaMercado.CriadoEm = DateTime.Now;
                linhaConsultaMercado.CriadoPor = User.Identity.Name;
                linhaConsultaMercado.CustoTotalObjectivo = data.CustoTotalObjectivo;
                linhaConsultaMercado.CustoTotalPrevisto = data.CustoTotalPrevisto;
                linhaConsultaMercado.CustoUnitarioObjectivo = data.CustoUnitarioObjectivo;
                linhaConsultaMercado.CustoUnitarioPrevisto = data.CustoUnitarioPrevisto;
                linhaConsultaMercado.DataEntregaPrevista = data.DataEntregaPrevista_Show != string.Empty ? DateTime.Parse(data.DataEntregaPrevista_Show) : (DateTime?)null;
                linhaConsultaMercado.Descricao = data.Descricao;
                linhaConsultaMercado.LinhaRequisicao = data.LinhaRequisicao;
                linhaConsultaMercado.ModificadoEm = data.ModificadoEm;
                linhaConsultaMercado.ModificadoPor = data.ModificadoPor;
                linhaConsultaMercado.NumConsultaMercado = data.NumConsultaMercado;
                //linhaConsultaMercado.NumLinha = data.NumLinha;
                linhaConsultaMercado.NumProjecto = data.NumProjecto;
                linhaConsultaMercado.NumRequisicao = data.NumRequisicao;
                linhaConsultaMercado.Quantidade = data.Quantidade;

                var dbCreateResult = DBConsultaMercado.Create(linhaConsultaMercado);

                if (dbCreateResult != null)
                    result = true;
                else
                    result = false;
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);

        }


        [HttpPost]
        public JsonResult DeleteLinhaConsultaMercado([FromBody] LinhasConsultaMercado data)
        {
            bool result = false;
            try
            {
                if (DBConsultaMercado.Delete(data) != null)
                    result = true;
                else
                    result = false;
                
            }
            catch (Exception ex)
            {
                //log
            }
            return Json(result);
        }

        #endregion

        #region EXCEL

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_ConsultaMercado([FromBody] List<ConsultaMercadoView> Lista)
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
                ISheet excelSheet = workbook.CreateSheet("Pedidos de Cotação");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["numConsultaMercado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Consulta Mercado"); Col = Col + 1; }
                if (dp["codProjecto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Projecto"); Col = Col + 1; }
                if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição"); Col = Col + 1; }
                if (dp["codRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Região"); Col = Col + 1; }
                if (dp["codAreaFuncional"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Área Funcional"); Col = Col + 1; }
                if (dp["codCentroResponsabilidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade"); Col = Col + 1; }
                if (dp["codActividade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Actividade"); Col = Col + 1; }
                if (dp["dataPedidoCotacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Pedido Cotação"); Col = Col + 1; }
                if (dp["fornecedorSelecionado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Fornecedor Selecionado"); Col = Col + 1; }
                if (dp["numDocumentoCompra"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Doc. Compra"); Col = Col + 1; }
                if (dp["codLocalizacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Localização"); Col = Col + 1; }
                if (dp["filtroActividade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Filtro Actividade"); Col = Col + 1; }
                if (dp["valorPedidoCotacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Valor Pedido Cotação"); Col = Col + 1; }
                if (dp["destino"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Destino"); Col = Col + 1; }
                if (dp["estado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Estado"); Col = Col + 1; }
                if (dp["utilizadorRequisicao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador Requisição"); Col = Col + 1; }
                if (dp["dataLimite"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Limite"); Col = Col + 1; }
                if (dp["especificacaoTecnica"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Especificação Técnica"); Col = Col + 1; }
                if (dp["fase"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Fase"); Col = Col + 1; }
                if (dp["modalidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Modalidade"); Col = Col + 1; }
                if (dp["pedidoCotacaoCriadoEm"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Pedido Cotação Criado Em"); Col = Col + 1; }
                if (dp["pedidoCotacaoCriadoPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Pedido Cotação Criado Por"); Col = Col + 1; }
                if (dp["consultaEm"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Consulta Em"); Col = Col + 1; }
                if (dp["consultaPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Consulta Por"); Col = Col + 1; }
                if (dp["negociacaoContratacaoEm"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Negociação Contratação Em"); Col = Col + 1; }
                if (dp["negociacaoContratacaoPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Negociação Contratação Por"); Col = Col + 1; }
                if (dp["adjudicacaoEm"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Adjudicação Em"); Col = Col + 1; }
                if (dp["adjudicacaoPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Adjudicação Por"); Col = Col + 1; }
                if (dp["numRequisicao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Requisição"); Col = Col + 1; }
                if (dp["pedidoCotacaoOrigem"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Pedido Cotação Origem"); Col = Col + 1; }
                if (dp["valorAdjudicado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Valor Adjudicado"); Col = Col + 1; }
                if (dp["codFormaPagamento"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cod. Forma Pagamento"); Col = Col + 1; }
                if (dp["seleccaoEfectuada"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Selecção Efectuada"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ConsultaMercadoView item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["numConsultaMercado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NumConsultaMercado); Col = Col + 1; }
                        if (dp["codProjecto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodProjecto); Col = Col + 1; }
                        if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Descricao); Col = Col + 1; }
                        if (dp["codRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodRegiao); Col = Col + 1; }
                        if (dp["codAreaFuncional"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodAreaFuncional); Col = Col + 1; }
                        if (dp["codCentroResponsabilidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodCentroResponsabilidade); Col = Col + 1; }
                        if (dp["codActividade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodActividade); Col = Col + 1; }
                        if (dp["dataPedidoCotacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataPedidoCotacao.ToString()); Col = Col + 1; }
                        if (dp["fornecedorSelecionado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FornecedorSelecionado); Col = Col + 1; }
                        if (dp["numDocumentoCompra"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NumDocumentoCompra); Col = Col + 1; }
                        if (dp["codLocalizacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodLocalizacao); Col = Col + 1; }
                        if (dp["filtroActividade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FiltroActividade); Col = Col + 1; }
                        if (dp["valorPedidoCotacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ValorPedidoCotacao.ToString()); Col = Col + 1; }
                        if (dp["destino"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Destino_Show.ToString()); Col = Col + 1; }
                        if (dp["estado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Estado_Show); Col = Col + 1; }
                        if (dp["utilizadorRequisicao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UtilizadorRequisicao); Col = Col + 1; }
                        if (dp["dataLimite"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataLimite.ToString()); Col = Col + 1; }
                        if (dp["especificacaoTecnica"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.EspecificacaoTecnica.ToString()); Col = Col + 1; }
                        if (dp["fase"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Fase_Show); Col = Col + 1; }
                        if (dp["modalidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Modalidade_Show); Col = Col + 1; }
                        if (dp["pedidoCotacaoCriadoEm"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PedidoCotacaoCriadoEm.ToString()); Col = Col + 1; }
                        if (dp["pedidoCotacaoCriadoPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PedidoCotacaoCriadoPor); Col = Col + 1; }
                        if (dp["consultaEm"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ConsultaEm.ToString()); Col = Col + 1; }
                        if (dp["consultaPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ConsultaPor); Col = Col + 1; }
                        if (dp["negociacaoContratacaoEm"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NegociacaoContratacaoEm.ToString()); Col = Col + 1; }
                        if (dp["negociacaoContratacaoPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NegociacaoContratacaoPor); Col = Col + 1; }
                        if (dp["adjudicacaoEm"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.AdjudicacaoEm.ToString()); Col = Col + 1; }
                        if (dp["adjudicacaoPor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.AdjudicacaoPor); Col = Col + 1; }
                        if (dp["numRequisicao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NumRequisicao); Col = Col + 1; }
                        if (dp["pedidoCotacaoOrigem"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PedidoCotacaoOrigem); Col = Col + 1; }
                        if (dp["valorAdjudicado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ValorAdjudicado.ToString()); Col = Col + 1; }
                        if (dp["codFormaPagamento"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodFormaPagamento); Col = Col + 1; }
                        if (dp["seleccaoEfectuada"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.SeleccaoEfectuada.ToString()); Col = Col + 1; }
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
        public IActionResult ExportToExcelDownload_ConsultaMercado(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Pedidos de Cotação.xlsx");
        }

        #endregion

    }
}