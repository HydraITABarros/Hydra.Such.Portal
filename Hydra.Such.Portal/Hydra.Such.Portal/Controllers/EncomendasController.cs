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
using Hydra.Such.Data.Logic.Approvals;
using System.Data.SqlClient;

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
            try
            {
                UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Encomendas);

                int year = 0;
                int.TryParse(requestParams.GetValue("year")?.ToString(), out year);
                int month = 0;
                int.TryParse(requestParams.GetValue("month")?.ToString(), out month);
                string fornecedor = (string)requestParams.GetValue("fornecedor");
                string requisitionNo = (string)requestParams.GetValue("requisitionNo");
                string from = "";
                string to = "";
                if (year != 0)
                {
                    from = new DateTime((int)year, (month == 0 ? 1 : month), 1).ToString("yyyy-MM-dd");
                    int days = DateTime.DaysInMonth((int)year, (month == 0 ? 12 : month));
                    to = new DateTime((int)year, (month == 0 ? 12 : month), days).ToString("yyyy-MM-dd");
                }

                if (UPerm != null && UPerm.Read.Value)
                {
                    List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                    List<EncomendasViewModel> result = DBNAV2017Encomendas.ListByDimListAndNoFilter(_config.NAVDatabaseName, _config.NAVCompanyName, userDimensions, "C%", from, to, fornecedor, requisitionNo);
                    return Json(result);
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception ex)
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
                return Json(new
                {
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
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_ListaEncomendas([FromBody] List<EncomendasViewModel> Lista)
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
                if (dp["vlrRececionadoComIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Vlr rececionado com IVA"); Col = Col + 1; }
                if (dp["vlrRececionadoSemIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Vlr rececionado sem IVA"); Col = Col + 1; }
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
                        if (dp["vlrRececionadoComIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.VlrRececionadoComIVA.ToString()); Col = Col + 1; }
                        if (dp["vlrRececionadoSemIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.VlrRececionadoSemIVA.ToString()); Col = Col + 1; }
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
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Encomendas.xlsx");
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
                ViewBag.reportServerURL = _config.ReportServerURL;

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
                    var pedidos = DBPedidoPagamento.GetAllPedidosPagamentoByEncomenda(id).Where(x => x.UserArquivo.ToLower() != "esuch@such.pt".ToLower());

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
                    Pedido.EditarPrioritario = false;

                    Pedido.ValorJaPedido = pedidos.Where(y => y.Estado != 5).Sum(x => x.Valor);
                    Pedido.DataText = DateTime.Now.ToString("yyyy-MM-dd");
                    Pedido.DataPedidoText = DateTime.Now.ToString("yyyy-MM-dd");

                    return Json(Pedido);
                }
                else
                {
                    int idPedido = Convert.ToInt32(id);
                    PedidosPagamentoViewModel Pedido = DBPedidoPagamento.ParseToViewModel(DBPedidoPagamento.GetIDPedidosPagamento(idPedido));

                    var pedidos = DBPedidoPagamento.GetAllPedidosPagamentoByEncomenda(Pedido.NoEncomenda);
                    Pedido.ValorJaPedido = pedidos.Where(y => y.Estado != 5).Sum(x => x.Valor);

                    Pedido.EditarPrioritario = false;
                    if (!string.IsNullOrEmpty(Pedido.Aprovadores) && Pedido.Aprovadores.ToLower().Contains(User.Identity.Name.ToLower()))
                    {
                        Pedido.EditarPrioritario = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Pedido.UserAprovacao) && Pedido.UserAprovacao == User.Identity.Name.ToLower())
                        {
                            Pedido.EditarPrioritario = true;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(Pedido.UserValidacao) && Pedido.UserValidacao.ToLower() == User.Identity.Name.ToLower())
                            {
                                Pedido.EditarPrioritario = true;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Pedido.UserFinanceiros) && Pedido.UserFinanceiros.ToLower() == User.Identity.Name.ToLower())
                                {
                                    Pedido.EditarPrioritario = true;
                                }
                                else
                                {
                                    MovimentosDeAprovação MDA = DBApprovalMovements.GetAll().Where(x => x.Número == Pedido.NoPedido.ToString() && x.Estado == 1).LastOrDefault();
                                    if (MDA != null)
                                    {
                                        UtilizadoresMovimentosDeAprovação UMDA = DBUserApprovalMovements.GetById(MDA.NºMovimento, User.Identity.Name);
                                        if (UMDA != null)
                                            Pedido.EditarPrioritario = true;
                                    }
                                }
                            }
                        }
                    }
                    return Json(Pedido);
                }
            }
            else
            {
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult GetListPedidosPagamento(int estado)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PedidosPagamento);
            List<PedidosPagamentoViewModel> result = new List<PedidosPagamentoViewModel>();

            if (UPerm != null && UPerm.Read.Value)
            {
                if (estado == 99)
                    result = DBPedidoPagamento.ParseToViewModel(DBPedidoPagamento.GetAllPedidosPagamento());
                else
                {
                    if (estado == 7)
                        result = DBPedidoPagamento.ParseToViewModel(DBPedidoPagamento.GetAllPedidosPagamentoByArquivo(true));
                    else
                        result = DBPedidoPagamento.ParseToViewModel(DBPedidoPagamento.GetAllPedidosPagamentoByEstado(estado));
                }

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

                    NAVSupplierViewModels Fornecedor = DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, data.CodigoFornecedor).FirstOrDefault();
                    if (Fornecedor.Blocked == 1)
                    {
                        data.eReasonCode = 4;
                        data.eMessage = "Não pode criar o Pedido de Pagamento, pois o fornecedor está bloqueado.";
                    }
                    else
                    {
                        List<NAV2017VendorLedgerEntryViewModel> AllMOV = DBNAV2017VendorLedgerEntry.GetMovFornecedores(_config.NAVDatabaseName, _config.NAVCompanyName);
                        AllMOV = AllMOV.Where(x => x.VendorNo == data.Fornecedor && x.DocumentType == 1 && x.Open == 1 && x.PostingDate < DateTime.Now.AddDays(-30)).ToList();

                        if (AllMOV != null && AllMOV.Count() > 0)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Não pode criar o Pedido de Pagamento, pois já existe um Pedido de Pagamento para este Fornecedor no estado Disponível há mais de 30 dias.";
                        }
                        else
                        {
                            if (DBPedidoPagamento.Create(DBPedidoPagamento.ParseToDB(data)) != null)
                            {
                                data = DBPedidoPagamento.ParseToViewModel(DBPedidoPagamento.GetAllPedidosPagamento().Where(x => x.UserPedido.ToLower() == User.Identity.Name.ToLower()).LastOrDefault());
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
                    //if (string.IsNullOrEmpty(data.NIB))
                    //{
                    //    data.eReasonCode = 3;
                    //    data.eMessage = "O campo NIB é de preenchimento obrigatório.";
                    //    return Json(data);
                    //}
                    //if (string.IsNullOrEmpty(data.IBAN))
                    //{
                    //    data.eReasonCode = 4;
                    //    data.eMessage = "O campo IBAN é de preenchimento obrigatório.";
                    //    return Json(data);
                    //}
                    if (data.Valor <= 0)
                    {
                        data.eReasonCode = 5;
                        data.eMessage = "O valor do campo Valor do Pedido c/ IVA tem que ser superior a zero.";
                        return Json(data);
                    }

                    ConfigUtilizadores Utilizador = DBUserConfigurations.GetById(User.Identity.Name);
                    string Aprovador1 = !string.IsNullOrEmpty(Utilizador.AprovadorPedidoPag1) ? Utilizador.AprovadorPedidoPag1 : "";
                    string Aprovador2 = !string.IsNullOrEmpty(Utilizador.AprovadorPedidoPag2) ? Utilizador.AprovadorPedidoPag2 : "";

                    if (string.IsNullOrEmpty(Aprovador1) && string.IsNullOrEmpty(Aprovador2))
                    {
                        data.eReasonCode = 6;
                        data.eMessage = "Não está definido nenhum Aprovador para o seu Pedido de Pagamento.";
                        return Json(data);
                    }

                    data.Aprovadores = "-" + Aprovador1 + "-" + Aprovador2 + "-";
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

                    //ENVIO DE EMAIL
                    List<string> UsersToNotify = new List<string>();
                    if (!string.IsNullOrEmpty(Aprovador1))
                        UsersToNotify.Add(Aprovador1);
                    if (!string.IsNullOrEmpty(Aprovador2))
                        UsersToNotify.Add(Aprovador2);

                    if (UsersToNotify.Count() > 0)
                    {
                        UsersToNotify = UsersToNotify.Distinct().ToList();
                        UsersToNotify.ForEach(e =>
                        {
                            SendEmailApprovals Email = new SendEmailApprovals();

                            Email.Subject = "eSUCH - Aprovação Pendente - Movimento de Pagamento Nº " + data.NoPedido.ToString();
                            Email.From = User.Identity.Name;
                            Email.To.Add(e);
                            Email.Body = MakeEmailBodyContent("Existe uma nova tarefa pendente da sua aprovação no eSUCH!");
                            Email.IsBodyHtml = true;

                            Email.SendEmail_Simple();
                        });
                    }

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

                    if (data.MotivoAnulacao.Length > 100)
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "O motivo de rejeição não pode ter mais de 100 carateres.";
                        return Json(data);
                    }

                    if (AnularPedido == true)
                    {
                        data.Estado = 5; //"Anulado"
                        data.Arquivado = true;
                        data.UserArquivo = User.Identity.Name;
                        data.DataArquivo = DateTime.Now;
                        data.MotivoAnulacao = data.MotivoAnulacao;
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
                            data.eMessage = "Não pode Anular este Pedido de Pagamento, pois não é o utilizador do Pedido de Pagamento.";
                            return Json(data);
                        }

                        data.Estado = 5; //"Anulado"
                        data.Arquivado = true;
                        data.UserArquivo = User.Identity.Name;
                        data.DataArquivo = DateTime.Now;
                        data.MotivoAnulacao = data.MotivoAnulacao;
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
                            data.MotivoAnulacao = data.MotivoAnulacao;
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
                            data.eMessage = "Não pode Anular este Pedido de Pagamento, pois não é o utilizador da Aprovação do Pedido de Pagamento.";
                            return Json(data);

                        }
                    }

                    if (data.Estado == 4) //"Validado"
                    {
                        if (data.UserValidacao.ToLower() != User.Identity.Name.ToLower())
                        {
                            data.eReasonCode = 8;
                            data.eMessage = "Não pode Anular este Pedido de Pagamento, pois não é o utilizador da Validação do Pedido de Pagamento.";
                            return Json(data);
                        }

                        data.Estado = 5; //"Anulado"
                        data.Arquivado = true;
                        data.UserArquivo = User.Identity.Name;
                        data.DataArquivo = DateTime.Now;
                        data.MotivoAnulacao = data.MotivoAnulacao;
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

        [HttpPost]
        public JsonResult UpdatePedidoPagamento([FromBody] PedidosPagamentoViewModel data)
        {
            try
            {
                if (data != null)
                {
                    PedidosPagamento Pedido = DBPedidoPagamento.GetIDPedidosPagamento(data.NoPedido);

                    if (Pedido != null)
                    {
                        Pedido.RegiaoMercadoLocal = data.RegiaoMercadoLocal;
                        Pedido.Descricao = data.Descricao;
                        Pedido.Prioritario = data.Prioritario;
                        Pedido.UtilizadorModificacao = User.Identity.Name;
                        Pedido.DataModificacao = DateTime.Now;

                        if (DBPedidoPagamento.Update(Pedido) != null)
                        {
                            data.eReasonCode = 1;
                            data.eMessage = "Pedido de Pagamento atualizado com sucesso.";
                        }
                        else
                        {
                            data.eReasonCode = 2;
                            data.eMessage = "Ocorreu um erro ao atualizar o Pedido de Pagamento.";
                        }
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

        [HttpPost]
        public JsonResult ObterNIB([FromBody] PedidosPagamentoViewModel data)
        {
            try
            {
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data.CodigoFornecedor))
                    {
                        var vendor = DBNAV2017VendorBankAccount.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName, data.CodigoFornecedor);

                        if (vendor != null)
                        {
                            data.NIB = vendor.NIB;
                            data.IBAN = vendor.IBAN;

                            if (data.NoPedido > 0)
                            {
                                if (DBPedidoPagamento.Update(DBPedidoPagamento.ParseToDB(data)) != null)
                                {
                                    data.eReasonCode = 1;
                                    data.eMessage = "O NIB/IBAN foi obtido com sucesso.";
                                }
                                else
                                {
                                    data.eReasonCode = 2;
                                    data.eMessage = "Ocorreu um erro ao atualizar o Pedido de Pagamento.";
                                }
                            }
                        }
                        else
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Não foi possível encontrar o fornecedor para obter NIB/IBAN.";
                        }
                    }
                    else
                    {
                        data.eReasonCode = 4;
                        data.eMessage = "Para obter NIB/IBAN é necessário definir qual o Fornecedor no Pedido de Pagemento.";
                    }
                }
                else
                {
                    data.eReasonCode = 4;
                    data.eMessage = "Ocorreu um erro nos dados.";
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 99;
                data.eMessage = "Ocorreu um erro.";
            }
            return Json(data);
        }

        public JsonResult CriarTransferenciaPedidoPagamento([FromBody] List<PedidosPagamentoViewModel> data)
        {
            string execDetails = string.Empty;
            string errorMessage = string.Empty;
            bool hasErrors = false;
            int Number_SEPA = 99;
            ErrorHandler result = new ErrorHandler();

            if (data != null && data.Count() > 0)
            {
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@DBName", _config.NAVDatabaseName),
                        new SqlParameter("@CompanyName", _config.NAVCompanyName),
                    };
                    Number_SEPA = ctx.execStoredProcedurePedidoPagamento("exec NAV2017VerificarPedPagSEPA @DBName, @CompanyName", parameters);
                }

                if (Number_SEPA == 0)
                {
                    List<PedidosPagamentoViewModel> GroupFornecedor = data.GroupBy(x =>
                    x.CodigoFornecedor,
                    x => x,
                    (key, items) => new PedidosPagamentoViewModel
                    {
                        CodigoFornecedor = key,
                        NoPedido = data.Where(f => f.CodigoFornecedor == key).FirstOrDefault().NoPedido,
                        Fornecedor = data.Where(f => f.CodigoFornecedor == key).FirstOrDefault().Fornecedor,
                        Valor = data.Where(f => f.CodigoFornecedor == key).Sum(y => y.Valor),

                    }).ToList();

                    if (GroupFornecedor != null && GroupFornecedor.Count() > 0)
                    {
                        int lineNo = 10000;
                        try
                        {
                            GroupFornecedor.ForEach(pedido =>
                            {
                                WSPaymentJournalNAV.WSPaymentJournal PaymentJournal = new WSPaymentJournalNAV.WSPaymentJournal()
                                {
                                    Journal_Template_Name = "PAGAMENTOS",
                                    Journal_Batch_Name = "SEPA-ADIAN",
                                    Line_No = lineNo,
                                    Account_Type = WSPaymentJournalNAV.Account_Type.Vendor,
                                    Account_No = pedido.CodigoFornecedor,
                                    Description = pedido.Fornecedor,
                                    Posting_Date = DateTime.Now,
                                    Document_Type = WSPaymentJournalNAV.Document_Type.Payment,
                                    Amount = (decimal)pedido.Valor
                                };

                                Task<WSPaymentJournalNAV.Create_Result> createPaymentJournal = WSPaymentJournal.CreatePaymentJournalNAV(PaymentJournal, _configws);
                                createPaymentJournal.Wait();

                                if (createPaymentJournal.IsCompletedSuccessfully && createPaymentJournal.Result.WSPaymentJournal == null)
                                {
                                    result.eReasonCode = 1;
                                    result.eMessage = " Transferência criada com sucesso.";
                                }
                                else
                                {
                                    result.eReasonCode = 2;
                                    result.eMessage = " Erro ao criar a Transferência.";
                                }

                                lineNo = lineNo + 10000;
                            });
                        }
                        catch (Exception ex)
                        {
                            if (!hasErrors)
                                hasErrors = true;

                            result.eReasonCode = 2;
                            result.eMessage = " Erro ao criar a Transferência:" + ex.Message;
                            return Json(result);
                        }
                    }
                }
                else
                {
                    result.eReasonCode = 3;
                    result.eMessage = " Não é possivel Criar Transferência pois existem registos no Diário de Pagamento, secão SEPA-ADIAN.";
                }
            }

            return Json(result);
        }

        public JsonResult PassarLiquidadosPedidoPagamento([FromBody] List<PedidosPagamentoViewModel> LIQUIDADOS)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (LIQUIDADOS != null)
                {
                    ConfigUtilizadores Utilizador = DBUserConfigurations.GetById(User.Identity.Name);
                    bool AnularPedido = Utilizador.AnulacaoPedidoPagamento.HasValue ? (bool)Utilizador.AnulacaoPedidoPagamento : false;
                    bool ValidarPedido = Utilizador.ValidarPedidoPagamento.HasValue ? (bool)Utilizador.ValidarPedidoPagamento : false;

                    LIQUIDADOS.ForEach(liquidado =>
                    {
                        if (liquidado.UserPedido.ToLower() == User.Identity.Name.ToLower() || AnularPedido || ValidarPedido)
                        {
                            liquidado.Estado = 6; //Liquidado
                            liquidado.UserLiquidado = User.Identity.Name;
                            liquidado.DataLiquidado = DateTime.Now;
                            liquidado.UtilizadorModificacao = User.Identity.Name;
                            liquidado.DataModificacao = DateTime.Now;

                            if (DBPedidoPagamento.Update(DBPedidoPagamento.ParseToDB(liquidado)) == null)
                            {
                                result.eMessages.Add(new TraceInformation(TraceType.Error, "Erro ao Liquidar o Pedido de Pagamento Nº " + liquidado.NoPedido));
                            }
                            else
                            {
                                result.eReasonCode = 1;
                                result.eMessage = "O(s) Pedido(s) de Pagamento foram Liquidado(s) com sucesso.";
                            }
                        }
                        else
                        {
                            result.eMessages.Add(new TraceInformation(TraceType.Error, "Não tem permissões para Liquidar o Pedido de Pagamento Nº " + liquidado.NoPedido));
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                result.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro."));
            }
            return Json(result);
        }

        public JsonResult ArquivarPedidoPagamento([FromBody] List<PedidosPagamentoViewModel> PEDIDOS)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (PEDIDOS != null)
                {
                    ConfigUtilizadores Utilizador = DBUserConfigurations.GetById(User.Identity.Name);
                    bool AnularPedido = Utilizador.AnulacaoPedidoPagamento.HasValue ? (bool)Utilizador.AnulacaoPedidoPagamento : false;
                    bool ValidarPedido = Utilizador.ValidarPedidoPagamento.HasValue ? (bool)Utilizador.ValidarPedidoPagamento : false;

                    PEDIDOS.ForEach(pedido =>
                    {
                        if (pedido.UserPedido.ToLower() == User.Identity.Name.ToLower() || AnularPedido || ValidarPedido)
                        {
                            pedido.Estado = 7; //"Arquivado"
                            pedido.Arquivado = true;
                            pedido.UserArquivo = User.Identity.Name;
                            pedido.DataArquivo = DateTime.Now;
                            pedido.UtilizadorModificacao = User.Identity.Name;
                            pedido.DataModificacao = DateTime.Now;

                            if (DBPedidoPagamento.Update(DBPedidoPagamento.ParseToDB(pedido)) == null)
                            {
                                result.eMessages.Add(new TraceInformation(TraceType.Error, "Erro ao Arquivar o Pedido de Pagamento Nº " + pedido.NoPedido));
                            }
                            else
                            {
                                result.eReasonCode = 1;
                                result.eMessage = "O(s) Pedido(s) de Pagamento Arquivado(s) com sucesso.";
                            }
                        }
                        else
                        {
                            result.eMessages.Add(new TraceInformation(TraceType.Error, "Não tem permissões para Arquivar o Pedido de Pagamento Nº " + pedido.NoPedido));
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                result.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro."));
            }
            return Json(result);
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
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_PedidosPagamento([FromBody] List<PedidosPagamentoViewModel> Lista)
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
                ISheet excelSheet = workbook.CreateSheet("Pedidos de Pagamento");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                var columns = dp.AsJEnumerable().ToList();
                for (int i = 0; i < columns.Count; i++)
                {
                    var column = columns[i];
                    var isHidden = true;
                    var label = "";
                    try
                    {
                        isHidden = (bool)column.First()["hidden"];
                        label = (string)column.First()["label"];
                    }
                    catch { }

                    if (!isHidden)
                    {
                        row.CreateCell(i).SetCellValue(label);
                    }
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (PedidosPagamentoViewModel item in Lista)
                    {
                        row = excelSheet.CreateRow(count);
                        for (int i = 0; i < columns.Count; i++)
                        {
                            var column = columns[i];
                            var isHidden = true;
                            try { isHidden = (bool)column.First()["hidden"]; } catch { }

                            if (!isHidden)
                            {
                                var columnPath = column.Path.ToString();
                                columnPath = columnPath.First().ToString().ToUpper() + String.Join("", columnPath.Skip(1));
                                object value = null;
                                try { value = item.GetType().GetProperty(columnPath).GetValue(item, null); } catch { }
                                if (value == null) try { value = item.GetType().GetProperty(columnPath.ToUpper()).GetValue(item, null); } catch { }

                                if ((new[] { "Valor", "ValorEncomenda", "ValorJaPedido" }).Contains(columnPath))
                                {
                                    row.CreateCell(i).SetCellValue((double)(value != null ? (decimal)value : 0));
                                }
                                else
                                {
                                    row.CreateCell(i).SetCellValue(value?.ToString());
                                }
                            }
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

        public IActionResult ExportToExcelDownload_PedidosPagamento(string FileName)
        {
            FileName = @"/Upload/temp/" + FileName;
            return File(FileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Pedidos de Pagamento.xlsx");
        }

        #endregion

    }
}