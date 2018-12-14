using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.ComprasML;
using Hydra.Such.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Hydra.Such.Data.ViewModel.Encomendas;
using Newtonsoft.Json;
using Hydra.Such.Data.Logic.Encomendas;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class EncomendasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EncomendasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Encomendas);

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

        [HttpGet]
        public IActionResult DetalhesEncomenda(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Encomendas);

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
        public JsonResult GetList([FromBody] JObject requestParams)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Encomendas);

            if (UPerm != null && UPerm.Read.Value)
            {
                List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                var result = DBNAV2017Encomendas.ListByDimListAndNoFilter(_config.NAVDatabaseName, _config.NAVCompanyName, userDimensions, "C%");
                return Json(result);
            }
            else
            {
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult GetDetails([FromBody] EncomendasViewModel encomenda)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Encomendas);

            if (UPerm != null && UPerm.Read.Value)
            {
                List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                var details = DBNAV2017Encomendas.GetDetailsByNo(_config.NAVDatabaseName, _config.NAVCompanyName, encomenda.No, "C%");
                var lines = DBNAV2017Encomendas.ListLinesByNo(_config.NAVDatabaseName, _config.NAVCompanyName, encomenda.No, "C%");
                return Json(new {
                    details,
                    lines
                });
            }
            else
            {
                return Json(null);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ExportToExcel([FromBody] List<EncomendasViewModel> Lista)
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
                ISheet excelSheet = workbook.CreateSheet("Encomendas");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["no"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº"); Col = Col + 1; }
                if (dp["payToVendorNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Fornecedor"); Col = Col + 1; }
                if (dp["payToName"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nome Fornecedor"); Col = Col + 1; }
                if (dp["yourReference"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Sua Referência"); Col = Col + 1; }
                if (dp["orderDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data da Encomenda"); Col = Col + 1; }
                if (dp["noConsulta"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Consulta Mercado"); Col = Col + 1; }
                if (dp["total"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Total"); Col = Col + 1; }
                if (dp["expectedReceiptDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Recepção Esperada"); Col = Col + 1; }
                if (dp["requisitionNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Requisição"); Col = Col + 1; }
                if (dp["regionId"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Região"); Col = Col + 1; }
                if (dp["functionalAreaId"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Área Funcional"); Col = Col + 1; }
                if (dp["respCenterId"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Centro de Responsabilidade"); Col = Col + 1; }
                if (dp["hasAnAdvance"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Adiantamento"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;

                    foreach (EncomendasViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["no"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(!string.IsNullOrEmpty(item.No) ? item.No : ""); Col = Col + 1; }
                        if (dp["payToVendorNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(!string.IsNullOrEmpty(item.PayToVendorNo) ? item.PayToVendorNo : ""); Col = Col + 1; }
                        if (dp["payToName"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(!string.IsNullOrEmpty(item.PayToName) ? item.PayToName : ""); Col = Col + 1; }
                        if (dp["yourReference"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(!string.IsNullOrEmpty(item.YourReference) ? item.YourReference : ""); Col = Col + 1; }
                        if (dp["orderDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.OrderDate); Col = Col + 1; }
                        if (dp["noConsulta"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(!string.IsNullOrEmpty(item.NoConsulta) ? item.NoConsulta : ""); Col = Col + 1; }
                        if (dp["total"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Total.ToString()); Col = Col + 1; }
                        if (dp["expectedReceiptDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ExpectedReceiptDate.ToString()); Col = Col + 1; }
                        if (dp["requisitionNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RequisitionNo); Col = Col + 1; }
                        if (dp["regionId"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RegionId); Col = Col + 1; }
                        if (dp["functionalAreaId"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FunctionalAreaId); Col = Col + 1; }
                        if (dp["respCenterId"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RespCenterId); Col = Col + 1; }
                        if (dp["hasAnAdvance"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.HasAnAdvance); Col = Col + 1; }

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

        public IActionResult ExportToExcelDownload_Encomendas(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Gestão Requisições.xlsx");
        }

        #region PedidoPagamento

        [HttpGet]
        public IActionResult PedidoPagamento_List()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PedidosPagamento);

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

        [HttpGet]
        public IActionResult PedidoPagamento_Details(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PedidosPagamento);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.No = id ?? "";
                ViewBag.UPermissions = UPerm;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetPedidoPagamento(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PedidosPagamento);

            if (UPerm != null && UPerm.Read.Value)
            {
                if (id.StartsWith("C"))
                {
                    var details = DBNAV2017Encomendas.GetDetailsByNo(_config.NAVDatabaseName, _config.NAVCompanyName, id, "C%");
                    var lines = DBNAV2017Encomendas.ListLinesByNo(_config.NAVDatabaseName, _config.NAVCompanyName, id, "C%");
                    var vendor = DBNAV2017VendorBankAccount.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName, details.PayToVendorNo);
                    var pedidos = DBPedidoPagamento.GetAllPedidosPagamentoByEncomenda(id);

                    PedidosPagamentoViewModel Pedido = new PedidosPagamentoViewModel();

                    Pedido.Data = DateTime.Now;
                    Pedido.Tipo = 1; //"Transferência Bancária"
                    Pedido.Estado = 1; //"Inicial"
                    Pedido.Aprovado = false;
                    Pedido.ValorEncomenda = lines.Sum(x => x.AmountIncludingVAT);
                    Pedido.NoEncomenda = id;
                    Pedido.CodigoFornecedor = details.PayToVendorNo;
                    Pedido.Fornecedor = details.PayToName;
                    Pedido.NIB = vendor.NIB;
                    Pedido.IBAN = vendor.IBAN;
                    Pedido.DataPedido = DateTime.Now;
                    Pedido.UserPedido = User.Identity.Name;
                    Pedido.BloqueadoFaltaPagamento = false;
                    Pedido.Arquivado = false;
                    Pedido.Resolvido = false;
                    Pedido.Prioritario = false;

                    Pedido.ValorJaPedido = pedidos.Sum(x => x.Valor);
                    Pedido.DataText = DateTime.Now.ToString("yyyy-MM-dd");
                    Pedido.DataPedidoText = DateTime.Now.ToString("yyyy-MM-dd");

                    return Json(Pedido);
                }
                else
                {
                    int idPedido = Convert.ToInt32(id);
                    PedidosPagamentoViewModel Pedido = DBPedidoPagamento.ParseToViewModel(DBPedidoPagamento.GetIDPedidosPagamento(idPedido));

                    var pedidos = DBPedidoPagamento.GetAllPedidosPagamentoByEncomenda(id);
                    Pedido.ValorJaPedido = pedidos.Sum(x => x.Valor);

                    return Json(Pedido);
                }
            }
            else
            {
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult GetListPedidosPagamento()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PedidosPagamento);

            if (UPerm != null && UPerm.Read.Value)
            {
                List<PedidosPagamentoViewModel> result = DBPedidoPagamento.ParseToViewModel(DBPedidoPagamento.GetAllPedidosPagamento());

                if (result != null)
                {
                    result.ForEach(PEDIDO =>
                    {
                        PEDIDO.TipoText = PEDIDO.Tipo != null ? EnumerablesFixed.TipoPedidoPagamento.Where(y => y.Id == PEDIDO.Tipo).FirstOrDefault().Value : "";
                        PEDIDO.EstadoText = PEDIDO.Estado != null ? EnumerablesFixed.EstadoPedidoPagamento.Where(y => y.Id == PEDIDO.Estado).FirstOrDefault().Value : "";
                    });
                }
                return Json(result);
            }
            else
            {
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult CriarPedidoPagamento([FromBody] PedidosPagamentoViewModel data)
        {
            try
            {
                if (data != null)
                {
                    data.Data = DateTime.Now;
                    data.DataPedido = DateTime.Now;
                    data.UserPedido = User.Identity.Name;
                    if (data.Prioritario == true)
                        data.DataPrioridade = DateTime.Now;
                    data.UtilizadorCriacao = User.Identity.Name;
                    data.DataCriacao = DateTime.Now;

                    if (DBPedidoPagamento.Create(DBPedidoPagamento.ParseToDB(data)) != null)
                    {
                        data.eReasonCode = 1;
                        data.eMessage = "Foi criado com sucesso o Pedido de Pagemento.";
                    }
                    else
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "Ocorreu um erro na criação do Pedido de Pagemento.";
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 99;
                data.eMessage = "Ocorreu um erro ao criar o Pedido de Pagemento.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult EnviarAprovacaoPedidoPagamento([FromBody] PedidosPagamentoViewModel data)
        {
            try
            {
                if (data != null)
                {
                    if (data.Estado != 1)
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "O Pedido de Pagamento tem que estar no estado no Inicial.";
                        return Json(data);
                    }
                    if (string.IsNullOrEmpty(data.NIB))
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "O campo NIB é de preenchimento obrigatório.";
                        return Json(data);
                    }
                    if (string.IsNullOrEmpty(data.IBAN))
                    {
                        data.eReasonCode = 4;
                        data.eMessage = "O campo IBAN é de preenchimento obrigatório.";
                        return Json(data);
                    }
                    if (data.Valor <= 0)
                    {
                        data.eReasonCode = 5;
                        data.eMessage = "O valor do campo Valor do Pedido c/ IVA tem que ser superior a zero.";
                        return Json(data);
                    }

                    ConfigUtilizadores Utilizador = DBUserConfigurations.GetById(User.Identity.Name);
                    string Aprovador1 = Utilizador.AprovadorPedidoPag1;
                    string Aprovador2 = Utilizador.AprovadorPedidoPag2;

                    if (string.IsNullOrEmpty(Aprovador1) && string.IsNullOrEmpty(Aprovador2))
                    {
                        data.eReasonCode = 6;
                        data.eMessage = "Não está definido nenhum Aprovador para o seu Pedido de Pagamento.";
                        return Json(data);
                    }

                    data.Aprovadores = "-" + Aprovador1 + "-" + Aprovador1 + "-";
                    data.DataEnvioAprovacao = DateTime.Now;
                    data.Estado = 2; //"Em Aprovação"
                    data.UtilizadorModificacao = User.Identity.Name;
                    data.DataModificacao = DateTime.Now;

                    if (DBPedidoPagamento.Update(DBPedidoPagamento.ParseToDB(data)) == null)
                    {
                        data.eReasonCode = 7;
                        data.eMessage = "Ocorreu um erro ao actualizar o Pedido de Pagamento.";
                        return Json(data);
                    }

                    //FALTA O ENVIO DE EMAIL

                    data.eReasonCode = 1;
                    data.eMessage = "O Pedido de Pagamento foi Enviado para Aprovação com sucesso.";
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 99;
                data.eMessage = "Ocorreu um erro.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult AprovarPedidoPagamento([FromBody] PedidosPagamentoViewModel data)
        {
            try
            {
                if (data != null)
                {
                    if (!data.Aprovadores.ToLower().Contains(User.Identity.Name.ToLower()))
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "Não pode aprovar este Pedido de Pagamento pois não é um aprovador.";
                        return Json(data);
                    }
                    if (data.Estado != 2)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "O Pedido de Pagamento tem que estar no estado Em Aprovação.";
                        return Json(data);
                    }

                    data.UserAprovacao = User.Identity.Name;
                    data.DataAprovacao = DateTime.Now;
                    data.Estado = 3; //"Aprovado"
                    data.Aprovado = true;
                    data.UtilizadorModificacao = User.Identity.Name;
                    data.DataModificacao = DateTime.Now;

                    if (DBPedidoPagamento.Update(DBPedidoPagamento.ParseToDB(data)) == null)
                    {
                        data.eReasonCode = 7;
                        data.eMessage = "Ocorreu um erro ao actualizar o Pedido de Pagamento.";
                        return Json(data);
                    }

                    data.eReasonCode = 1;
                    data.eMessage = "O Pedido de Pagamento foi Aprovado com sucesso.";
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 99;
                data.eMessage = "Ocorreu um erro.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult ValidarPedidoPagamento([FromBody] PedidosPagamentoViewModel data)
        {
            try
            {
                if (data != null)
                {
                    ConfigUtilizadores Utilizador = DBUserConfigurations.GetById(User.Identity.Name);
                    bool ValidarPedido = Utilizador.ValidarPedidoPagamento.HasValue ? (bool)Utilizador.ValidarPedidoPagamento : false;

                    if (ValidarPedido == false)
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "Não têm permissões para Validar o Pedido de Pagamento.";
                        return Json(data);
                    }
                    if (data.Estado != 3)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "O Pedido de Pagamento tem que estar no estado Aprovado.";
                        return Json(data);
                    }

                    data.UserValidacao = User.Identity.Name;
                    data.DataValidacao = DateTime.Now;
                    data.Estado = 4; //"Validado"
                    data.UtilizadorModificacao = User.Identity.Name;
                    data.DataModificacao = DateTime.Now;

                    if (DBPedidoPagamento.Update(DBPedidoPagamento.ParseToDB(data)) == null)
                    {
                        data.eReasonCode = 7;
                        data.eMessage = "Ocorreu um erro ao actualizar o Pedido de Pagamento.";
                        return Json(data);
                    }

                    data.eReasonCode = 1;
                    data.eMessage = "O Pedido de Pagamento foi Validado com sucesso.";
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 99;
                data.eMessage = "Ocorreu um erro.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult AnularPedidoPagamento([FromBody] PedidosPagamentoViewModel data)
        {
            try
            {
                if (data != null)
                {
                    ConfigUtilizadores Utilizador = DBUserConfigurations.GetById(User.Identity.Name);
                    bool AnularPedido = Utilizador.AnulacaoPedidoPagamento.HasValue ? (bool)Utilizador.AnulacaoPedidoPagamento : false;

                    if (data.Estado != 2 && data.Estado != 3 && data.Estado != 4)
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "O Pedido de Pagamento só pode ser Anulado se estiver nos seguintes estados: Em Aprovação, Aprovado ou Validado.";
                        return Json(data);
                    }

                    if (AnularPedido == true)
                    {
                        data.Estado = 5; //"Anulado"
                        data.Arquivado = true;
                        data.UserArquivo = User.Identity.Name;
                        data.DataArquivo = DateTime.Now;
                        data.UtilizadorModificacao = User.Identity.Name;
                        data.DataModificacao = DateTime.Now;

                        if (DBPedidoPagamento.Update(DBPedidoPagamento.ParseToDB(data)) == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro ao actualizar o Pedido de Pagamento.";
                            return Json(data);
                        }

                        data.eReasonCode = 1;
                        data.eMessage = "O Pedido de Pagamento foi Anulado com sucesso.";
                        return Json(data);
                    }

                    if (data.Estado == 2) //"Em Aprovação"
                    {
                        if (data.UserPedido.ToLower() != User.Identity.Name.ToLower())
                        {
                            data.eReasonCode = 4;
                            data.eMessage = "Não pode Anular este Pedido de Pagamento.";
                            return Json(data);
                        }

                        data.Estado = 5; //"Anulado"
                        data.Arquivado = true;
                        data.UserArquivo = User.Identity.Name;
                        data.DataArquivo = DateTime.Now;
                        data.UtilizadorModificacao = User.Identity.Name;
                        data.DataModificacao = DateTime.Now;

                        if (DBPedidoPagamento.Update(DBPedidoPagamento.ParseToDB(data)) == null)
                        {
                            data.eReasonCode = 5;
                            data.eMessage = "Ocorreu um erro ao actualizar o Pedido de Pagamento.";
                            return Json(data);
                        }

                        data.eReasonCode = 1;
                        data.eMessage = "O Pedido de Pagamento foi Anulado com sucesso.";
                        return Json(data);
                    }

                    if (data.Estado == 3) //"Aprovado"
                    {
                        if (data.UserAprovacao.ToLower() == User.Identity.Name.ToLower() || AnularPedido == true)
                        {
                            data.Estado = 5; //"Anulado"
                            data.Arquivado = true;
                            data.UserArquivo = User.Identity.Name;
                            data.DataArquivo = DateTime.Now;
                            data.UtilizadorModificacao = User.Identity.Name;
                            data.DataModificacao = DateTime.Now;

                            if (DBPedidoPagamento.Update(DBPedidoPagamento.ParseToDB(data)) == null)
                            {
                                data.eReasonCode = 6;
                                data.eMessage = "Ocorreu um erro ao actualizar o Pedido de Pagamento.";
                                return Json(data);
                            }

                            data.eReasonCode = 1;
                            data.eMessage = "O Pedido de Pagamento foi Anulado com sucesso.";
                            return Json(data);
                        }
                        else
                        {
                            data.eReasonCode = 7;
                            data.eMessage = "Não pode Anular este Pedido de Pagamento.";
                            return Json(data);

                        }
                    }

                    if (data.Estado == 4) //"Validado"
                    {
                        if (data.UserValidacao.ToLower() != User.Identity.Name.ToLower())
                        {
                            data.eReasonCode = 8;
                            data.eMessage = "Não pode Anular este Pedido de Pagamento.";
                            return Json(data);
                        }

                        data.Estado = 5; //"Anulado"
                        data.Arquivado = true;
                        data.UserArquivo = User.Identity.Name;
                        data.DataArquivo = DateTime.Now;
                        data.UtilizadorModificacao = User.Identity.Name;
                        data.DataModificacao = DateTime.Now;

                        if (DBPedidoPagamento.Update(DBPedidoPagamento.ParseToDB(data)) == null)
                        {
                            data.eReasonCode = 5;
                            data.eMessage = "Ocorreu um erro ao actualizar o Pedido de Pagamento.";
                            return Json(data);
                        }

                        data.eReasonCode = 1;
                        data.eMessage = "O Pedido de Pagamento foi Anulado com sucesso.";
                        return Json(data);
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 99;
                data.eMessage = "Ocorreu um erro.";
            }
            return Json(data);
        }

        #endregion

    }
}