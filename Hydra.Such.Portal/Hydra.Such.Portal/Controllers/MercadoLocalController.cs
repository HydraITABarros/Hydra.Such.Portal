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

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class MercadoLocalController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;

        public MercadoLocalController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult MercadoLocalList()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.MercadoLocal);

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
        //Listagem das Folhas de Horas consoante o estado
        public JsonResult GetListComprasByEstado([FromBody] Compras ML)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.MercadoLocal);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;

                List<ComprasViewModel> result = new List<ComprasViewModel>();
                if (ML.Estado == 0)
                    result = DBCompras.GetAll();
                else
                    result = DBCompras.GetAllByEstado((int)ML.Estado);

                if (result != null)
                {
                    List<NAVProjectsViewModel> AllProjects = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                    List<LinhasRequisição> AllLines = DBRequestLine.GetAll();
                    List<NAVSupplierViewModels> AllSupliers = DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, "");

                    result.ForEach(Compras =>
                    {
                        Compras.RegiaoMercadoLocalTexto = Compras.RegiaoMercadoLocal == null ? "" : EnumerablesFixed.LocalMarketRegions.Where(y => y.Id == Compras.RegiaoMercadoLocal).FirstOrDefault().Value;
                        Compras.CodigoProdutoTexto = Compras.CodigoProduto == null ? "" : Compras.CodigoProduto;
                        Compras.EstadoTexto = Compras.Estado == null ? "" : EnumerablesFixed.ComprasEstado.Where(y => y.Id == Compras.Estado).FirstOrDefault().Value;

                        if (!string.IsNullOrEmpty(Compras.NoFornecedor))
                            Compras.NoFornecedorTexto = Compras.NoFornecedor == null ? "" : Compras.NoFornecedor + " - " + AllSupliers.Where(x => x.No_ == Compras.NoFornecedor).FirstOrDefault() != null ? AllSupliers.Where(x => x.No_ == Compras.NoFornecedor).FirstOrDefault().Name : "";

                        if (!string.IsNullOrEmpty(Compras.NoRequisicao) && Compras.NoLinhaRequisicao != null)
                            Compras.RecusadoComprasTexto = AllLines.Where(x => x.NºRequisição == Compras.NoRequisicao && x.NºLinha == Compras.NoLinhaRequisicao).FirstOrDefault() != null ? AllLines.Where(x => x.NºRequisição == Compras.NoRequisicao && x.NºLinha == Compras.NoLinhaRequisicao).FirstOrDefault().RecusadoCompras == true ? "Sim" : "Não" : "";

                        if (!string.IsNullOrEmpty(Compras.NoProjeto))
                            Compras.NoProjetoTexto = AllProjects.Where(x => x.No == Compras.NoProjeto).FirstOrDefault() != null ? AllProjects.Where(x => x.No == Compras.NoProjeto).FirstOrDefault().Description : "";
                    });
                }

                return Json(result.OrderByDescending(x => x.ID));
            }

            return Json(null);
        }

        [HttpPost]
        public JsonResult AprovadoToTratado([FromBody] List<Compras> Mercados)
        {
            ErrorHandler result = new ErrorHandler
            {
                eReasonCode = 0,
                eMessage = "Os Registos foram atualizados com sucesso."
            };

            try
            {
                if (Mercados != null && Mercados.Count() > 0)
                {
                    UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.MercadoLocal);
                    if (UPerm.Update == true)
                    {
                        Mercados.ForEach(Mercado =>
                        {
                            Mercado.Estado = 2; //VALIDADO
                            Mercado.DataValidacao = DateTime.Now;
                            Mercado.UtilizadorValidacao = User.Identity.Name;

                            if (DBCompras.Update(Mercado) == null)
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "Ocorreu um erro ao atualizar o registo.";
                            }
                        });
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não tem permissões para alterar o registo.";
                    }
                }
                else
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Não foi possivel ler o registo.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult AprovadoToValidar([FromBody] List<Compras> Mercados)
        {
            ErrorHandler result = new ErrorHandler
            {
                eReasonCode = 0,
                eMessage = "Os Registos foram atualizados com sucesso."
            };

            try
            {
                if (Mercados != null && Mercados.Count() > 0)
                {
                    UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.MercadoLocal);
                    if (UPerm.Update == true)
                    {
                        Mercados.ForEach(Mercado =>
                        {
                            if (Mercado.Responsaveis.ToLower().Contains(User.Identity.Name.ToLower()))
                            {
                                Mercado.Estado = 2; //VALIDADO
                                Mercado.DataValidacao = DateTime.Now;
                                Mercado.UtilizadorValidacao = User.Identity.Name;

                                if (DBCompras.Update(Mercado) == null)
                                {
                                    result.eReasonCode = 6;
                                    result.eMessage = "Ocorreu um erro ao atualizar o registo.";
                                }

                                string NoRequisicao = Mercado.NoRequisicao;
                                int NoLinhaRequisicao = (int)Mercado.NoLinhaRequisicao;

                                LinhasRequisição LinhaRequisicao = DBRequestLine.GetByRequisicaoNoAndLineNo(NoRequisicao, NoLinhaRequisicao);
                                if (LinhaRequisicao != null)
                                {
                                    LinhaRequisicao.ValidadoCompras = true;

                                    if (DBRequestLine.Update(LinhaRequisicao) == null)
                                    {
                                        result.eReasonCode = 5;
                                        result.eMessage = "Ocorreu um erro ao atualizar a Linha de Requisição.";
                                    }
                                }
                                else
                                {
                                    result.eReasonCode = 4;
                                    result.eMessage = "Não foi possivel ler a Linha de Requisição.";
                                }
                            }
                            else
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "O seu id não está no campo Responsáveis, logo não pode alterar o registo.";
                            }
                        });
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não tem permissões para alterar o registo do mercado Local.";
                    }
                }
                else
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Não foi possivel ler o registo do Mercado Local.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult AprovadoToRecusar([FromBody] JObject requestParams)
        {
            ErrorHandler result = new ErrorHandler
            {
                eReasonCode = 0,
                eMessage = "Os Registos foram atualizados com sucesso."
            };

            try
            {
                string rejectionComments = requestParams["rejectReason"].ToString();

                List<ComprasViewModel> Mercados = new List<ComprasViewModel>();
                int count = 0;
                if (requestParams["mercados"].Count() > 0)
                {
                    for (count = 0; count < requestParams["mercados"].Count(); count++)
                    {
                        ComprasViewModel Mercado = new ComprasViewModel
                        {
                            ID = (int)requestParams["mercados"][count]["id"],
                            CodigoProduto = (string)requestParams["mercados"][count]["codigoProduto"],
                            Descricao = (string)requestParams["mercados"][count]["descricao"],
                            Descricao2 = (string)requestParams["mercados"][count]["descricao2"],
                            CodigoUnidadeMedida = (string)requestParams["mercados"][count]["codigoUnidadeMedida"],
                            Quantidade = (decimal)requestParams["mercados"][count]["quantidade"],
                            NoRequisicao = (string)requestParams["mercados"][count]["noRequisicao"],
                            NoLinhaRequisicao = (int)requestParams["mercados"][count]["noLinhaRequisicao"],
                            Urgente = string.IsNullOrEmpty(Convert.ToString(requestParams["mercados"][count]["urgente"])) ? false : (bool)requestParams["mercados"][count]["urgente"],
                            RegiaoMercadoLocal = (string)requestParams["mercados"][count]["regiaoMercadoLocal"],
                            Estado = (int)requestParams["mercados"][count]["estado"],
                            DataCriacao = string.IsNullOrEmpty(Convert.ToString(requestParams["mercados"][count]["dataCriacao"])) ? Convert.ToDateTime("1753/01/01") : (DateTime)requestParams["mercados"][count]["dataCriacao"],
                            UtilizadorCriacao = (string)requestParams["mercados"][count]["utilizadorCriacao"],
                            Responsaveis = (string)requestParams["mercados"][count]["responsaveis"],
                            NoProjeto = (string)requestParams["mercados"][count]["noProjeto"],
                            NoFornecedor = (string)requestParams["mercados"][count]["noFornecedor"],
                            NoEncomenda = (string)requestParams["mercados"][count]["noEncomenda"],
                            DataEncomenda = string.IsNullOrEmpty(Convert.ToString(requestParams["mercados"][count]["dataEncomenda"])) ? Convert.ToDateTime("1753/01/01") : (DateTime)requestParams["mercados"][count]["dataEncomenda"],
                            NoConsultaMercado = (string)requestParams["mercados"][count]["noConsultaMercado"],
                            DataConsultaMercado = string.IsNullOrEmpty(Convert.ToString(requestParams["mercados"][count]["dataConsultaMercado"])) ? Convert.ToDateTime("1753/01/01") : (DateTime)requestParams["mercados"][count]["dataConsultaMercado"],
                            DataValidacao = string.IsNullOrEmpty(Convert.ToString(requestParams["mercados"][count]["dataValidacao"])) ? Convert.ToDateTime("1753/01/01") : (DateTime)requestParams["mercados"][count]["dataValidacao"],
                            UtilizadorValidacao = (string)requestParams["mercados"][count]["utilizadorValidacao"],
                            DataRecusa = string.IsNullOrEmpty(Convert.ToString(requestParams["mercados"][count]["dataRecusa"])) ? Convert.ToDateTime("1753/01/01") : (DateTime)requestParams["mercados"][count]["dataRecusa"],
                            UtilizadorRecusa = (string)requestParams["mercados"][count]["utilizadorRecusa"],
                            DataTratado = string.IsNullOrEmpty(Convert.ToString(requestParams["mercados"][count]["dataTratado"])) ? Convert.ToDateTime("1753/01/01") : (DateTime)requestParams["mercados"][count]["dataTratado"],
                            UtilizadorTratado = (string)requestParams["mercados"][count]["utilizadorTratado"],
                            Recusada = string.IsNullOrEmpty(Convert.ToString(requestParams["mercados"][count]["recusada"])) ? false : (bool)requestParams["mercados"][count]["recusada"]
                        };
                        Mercados.Add(Mercado);
                    }
                }

                if (Mercados != null && Mercados.Count() > 0)
                {
                    UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.MercadoLocal);
                    if (UPerm.Update == true)
                    {
                        Mercados.ForEach(MercadoVM =>
                        {
                            if (MercadoVM.Responsaveis.ToLower().Contains(User.Identity.Name.ToLower()))
                            {
                                Compras Mercado = DBCompras.ParseToCompras(MercadoVM);

                                Mercado.Estado = 3; //RECUSADO
                                Mercado.DataRecusa = DateTime.Now;
                                Mercado.UtilizadorRecusa = User.Identity.Name;

                                if (DBCompras.Update(Mercado) == null)
                                {
                                    result.eReasonCode = 6;
                                    result.eMessage = "Ocorreu um erro ao atualizar o registo.";
                                }

                                string NoRequisicao = Mercado.NoRequisicao;
                                int NoLinhaRequisicao = (int)Mercado.NoLinhaRequisicao;

                                LinhasRequisição LinhaRequisicao = DBRequestLine.GetByRequisicaoNoAndLineNo(NoRequisicao, NoLinhaRequisicao);
                                if (LinhaRequisicao != null)
                                {
                                    LinhaRequisicao.RecusadoCompras = true;
                                    LinhaRequisicao.MotivoRecusaMercLocal = MercadoVM.RecusadaTexto;
                                    LinhaRequisicao.DataRecusaMercLocal = DateTime.Now;
                                    LinhaRequisicao.MercadoLocal = false;
                                    LinhaRequisicao.DataMercadoLocal = null;

                                    if (DBRequestLine.Update(LinhaRequisicao) == null)
                                    {
                                        result.eReasonCode = 5;
                                        result.eMessage = "Ocorreu um erro ao atualizar a Linha de Requisição.";
                                    }
                                }
                                else
                                {
                                    result.eReasonCode = 4;
                                    result.eMessage = "Não foi possivel ler a Linha de Requisição.";
                                }
                            }
                            else
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "O seu id não está no campo Responsáveis, logo não pode alterar o registo.";
                            }
                        });
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não tem permissões para alterar o registo do mercado Local.";
                    }
                }
                else
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Não foi possivel ler o registo do Mercado Local.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult ValidadoToTratado([FromBody] List<Compras> Mercados)
        {
            ErrorHandler result = new ErrorHandler
            {
                eReasonCode = 0,
                eMessage = "Os Registos foram atualizados com sucesso."
            };

            try
            {
                if (Mercados != null && Mercados.Count() > 0)
                {
                    UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.MercadoLocal);
                    if (UPerm.Update == true)
                    {
                        Mercados.ForEach(Mercado =>
                        {
                            if (Mercado.Responsaveis.ToLower().Contains(User.Identity.Name.ToLower()))
                            {
                                Mercado.Estado = 4; //TRATADO
                                Mercado.DataTratado = DateTime.Now;
                                Mercado.UtilizadorTratado = User.Identity.Name;

                                if (DBCompras.Update(Mercado) == null)
                                {
                                    result.eReasonCode = 4;
                                    result.eMessage = "Ocorreu um erro ao atualizar o registo.";
                                }
                            }
                            else
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "O seu id não está no campo Responsáveis, logo não pode alterar o registo.";
                            }
                        });
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não tem permissões para alterar o registo do mercado Local.";
                    }
                }
                else
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Não foi possivel ler o registo do Mercado Local.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult RecusadoToTratado([FromBody] List<Compras> Mercados)
        {
            ErrorHandler result = new ErrorHandler
            {
                eReasonCode = 0,
                eMessage = "Os Registos foram atualizados com sucesso."
            };

            try
            {
                if (Mercados != null && Mercados.Count() > 0)
                {
                    UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.MercadoLocal);
                    if (UPerm.Update == true)
                    {
                        Mercados.ForEach(Mercado =>
                        {
                            if (Mercado.Responsaveis.ToLower().Contains(User.Identity.Name.ToLower()))
                            {
                                Mercado.Estado = 4; //TRATADO
                                Mercado.DataTratado = DateTime.Now;
                                Mercado.UtilizadorTratado = User.Identity.Name;

                                if (DBCompras.Update(Mercado) == null)
                                {
                                    result.eReasonCode = 4;
                                    result.eMessage = "Ocorreu um erro ao atualizar o registo.";
                                }
                            }
                            else
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "O seu id não está no campo Responsáveis, logo não pode alterar o registo.";
                            }
                        });
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não tem permissões para alterar o registo do mercado Local.";
                    }
                }
                else
                {
                    result.eReasonCode = 1;
                    result.eMessage = "Não foi possivel ler o registo do Mercado Local.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(null);
        }

        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_MercadoLocal([FromBody] List<ComprasViewModel> Compras)
        {
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
                ISheet excelSheet = workbook.CreateSheet("Mercado Local");
                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Estado");
                row.CreateCell(1).SetCellValue("Região Mercado Local");
                row.CreateCell(2).SetCellValue("Cód. Produto");
                row.CreateCell(3).SetCellValue("Descrição");
                row.CreateCell(4).SetCellValue("Descrição 2");
                row.CreateCell(5).SetCellValue("Cód. Unid. Medida");
                row.CreateCell(6).SetCellValue("Quantidade");
                row.CreateCell(7).SetCellValue("Nº Requisição");
                row.CreateCell(8).SetCellValue("Nº Linha Requisição");
                row.CreateCell(9).SetCellValue("Urgente");
                row.CreateCell(10).SetCellValue("Data Criação");
                row.CreateCell(11).SetCellValue("Utilizador Criação");
                row.CreateCell(12).SetCellValue("Data Validação");
                row.CreateCell(13).SetCellValue("Utilizador Validador");
                row.CreateCell(14).SetCellValue("Data Recusa");
                row.CreateCell(15).SetCellValue("Utilizador Recusou");
                row.CreateCell(16).SetCellValue("Recusado Compras");
                row.CreateCell(17).SetCellValue("Data Tratado");
                row.CreateCell(18).SetCellValue("Utilizador Tratado");
                row.CreateCell(19).SetCellValue("Nº Fornecedor");
                row.CreateCell(20).SetCellValue("Nº Encomenda");
                row.CreateCell(21).SetCellValue("Nº Consulta Mercado");

                if (Compras != null)
                {
                    int count = 1;
                    foreach (ComprasViewModel item in Compras)
                    {
                        row = excelSheet.CreateRow(count);
                        row.CreateCell(0).SetCellValue(item.EstadoTexto);
                        row.CreateCell(1).SetCellValue(item.RegiaoMercadoLocal);
                        row.CreateCell(2).SetCellValue(item.CodigoProduto);
                        row.CreateCell(3).SetCellValue(item.Descricao);
                        row.CreateCell(4).SetCellValue(item.Descricao2);
                        row.CreateCell(5).SetCellValue(item.CodigoUnidadeMedida);
                        row.CreateCell(6).SetCellValue(item.Quantidade.ToString());
                        row.CreateCell(7).SetCellValue(item.NoRequisicao);
                        row.CreateCell(8).SetCellValue(item.NoLinhaRequisicao.ToString());
                        row.CreateCell(9).SetCellValue(item.UrgenteTexto);
                        row.CreateCell(10).SetCellValue(item.DataCriacaoTexto);
                        row.CreateCell(11).SetCellValue(item.UtilizadorCriacaoTexto);
                        row.CreateCell(12).SetCellValue(item.DataValidacaoTexto);
                        row.CreateCell(13).SetCellValue(item.UtilizadorValidacaoTexto);
                        row.CreateCell(14).SetCellValue(item.DataRecusaTexto);
                        row.CreateCell(15).SetCellValue(item.UtilizadorRecusaTexto);
                        row.CreateCell(16).SetCellValue(item.RecusadoComprasTexto);
                        row.CreateCell(17).SetCellValue(item.DataTratadoTexto);
                        row.CreateCell(18).SetCellValue(item.UtilizadorTratadoTexto);
                        row.CreateCell(19).SetCellValue(item.NoFornecedorTexto);
                        row.CreateCell(20).SetCellValue(item.NoEncomendaTexto);
                        row.CreateCell(21).SetCellValue(item.NoConsultaMercado);
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

        public IActionResult ExportToExcelDownload_MercadoLocal(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Mercado Local.xlsx");
        }

    }
}