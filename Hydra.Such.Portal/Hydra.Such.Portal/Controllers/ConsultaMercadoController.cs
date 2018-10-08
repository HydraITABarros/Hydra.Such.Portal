using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.PedidoCotacao;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.ViewModel.PedidoCotacao;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Portal.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using static Hydra.Such.Data.Enumerations;


namespace Hydra.Such.Portal.Controllers
{
    public class ConsultaMercadoController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations configws;
        private readonly GeneralConfigurations _generalConfig;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ConsultaMercadoController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            configws = NAVWSConfigs.Value;
            _generalConfig = appSettingsGeneral.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult ConsultaMercado()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.PedidoCotacao);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UploadURL = _generalConfig.FileUploadFolder;
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
            return Json(list.OrderByDescending(x => x.NumConsultaMercado));
        }


        public IActionResult DetalheConsultaMercado(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.PedidoCotacao);
            ViewBag.reportServerURL = _config.ReportServerURL;

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.No = id ?? "";
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

        [HttpPost]
        public JsonResult EstadoAnteriorConsultaMercado([FromBody] ConsultaMercadoView data)
        {
            if (data != null)
            {
                ConsultaMercado consultaMercado = DBConsultaMercado.GetDetalheConsultaMercado(data.NumConsultaMercado);

                if (consultaMercado != null)
                {
                    List<EnumData> Fases = EnumerablesFixed.Fase;
                    List<EnumData> Estados = EnumerablesFixed.Estado;

                    if (consultaMercado.Fase == Fases[4].Id)
                    {
                        consultaMercado.Estado = Estados[0].Id;
                    }

                    consultaMercado.Fase = (consultaMercado.Fase - 1) >= 0 ? (consultaMercado.Fase - 1) : Fases[0].Id;
                    consultaMercado = DBConsultaMercado.Update(consultaMercado);

                    ConsultaMercadoView result = DBConsultaMercado.CastConsultaMercadoToView(consultaMercado);
                    result.eReasonCode = 0;
                    result.eMessage = "Mudança para Estado Anterior com sucesso!";

                    return Json(result);
                }

                data.eReasonCode = -1;
                data.eMessage = "Aconteceu algo errado e não foi alterado para Estado Anterior!";
                //return GetDetalheConsultaMercado(data);
                return Json(data);
            }

            data.eReasonCode = -1;
            data.eMessage = "Aconteceu algo errado e não foi alterado para Estado Anterior!";
            //return GetDetalheConsultaMercado(data);
            return Json(data);
        }

        [HttpPost]
        public JsonResult EstadoSeguinteConsultaMercado([FromBody] ConsultaMercadoView data)
        {
            if (data != null)
            {
                ConsultaMercado consultaMercado = DBConsultaMercado.GetDetalheConsultaMercado(data.NumConsultaMercado);

                if (consultaMercado != null)
                {
                    List<EnumData> Fases = EnumerablesFixed.Fase;
                    List<EnumData> Estados = EnumerablesFixed.Estado;

                    if (consultaMercado.Fase == Fases[3].Id)
                    {
                        consultaMercado.Estado = Estados[1].Id;
                    }

                    consultaMercado.Fase = (consultaMercado.Fase + 1) <= 4 ? (consultaMercado.Fase + 1) : Fases[4].Id;
                    consultaMercado = DBConsultaMercado.Update(consultaMercado);

                    //Criar uma versão no histórico, com versão incrementada em 1
                    HistoricoConsultaMercado historicoconsultaMercado = DBConsultaMercado.Create(consultaMercado);

                    if (historicoconsultaMercado != null || historicoconsultaMercado.NumConsultaMercado != "")
                    {
                        int _numversao = historicoconsultaMercado.NumVersao;

                        //Histórico Linhas Consulta Mercado
                        foreach (LinhasConsultaMercado lin in consultaMercado.LinhasConsultaMercado)
                        {
                            DBConsultaMercado.Create_Hist(lin,_numversao);
                        }

                        //Histórico Condições Propostas Fornecedores
                        foreach (CondicoesPropostasFornecedores lin in consultaMercado.CondicoesPropostasFornecedores)
                        {
                            DBConsultaMercado.Create_Hist(lin, _numversao);
                        }

                        //Histórico Linhas Condições Propostas Fornecedores
                        foreach (LinhasCondicoesPropostasFornecedores lin in consultaMercado.LinhasCondicoesPropostasFornecedores)
                        {
                            DBConsultaMercado.Create_Hist(lin, _numversao);
                        }

                        //Histórico Selecção Entidades
                        foreach (SeleccaoEntidades lin in consultaMercado.SeleccaoEntidades)
                        {
                            DBConsultaMercado.Create_Hist(lin, _numversao);
                        }
                    }

                    ConsultaMercadoView result = DBConsultaMercado.CastConsultaMercadoToView(consultaMercado);
                    result.eReasonCode = 0;
                    result.eMessage = "Mudança para Estado Seguinte com sucesso!";
                    
                    return Json(result);
                }

                data.eReasonCode = -1;
                data.eMessage = "Aconteceu algo errado e não foi alterado para Estado Seguinte!";
                //return GetDetalheConsultaMercado(data);
                return Json(data);
            }

            data.eReasonCode = -1;
            data.eMessage = "Aconteceu algo errado e não foi alterado para Estado Seguinte!";
            //return GetDetalheConsultaMercado(data);
            return Json(data);
        }

        [HttpPost]
        public JsonResult CopiarConsultaMercado([FromBody] ConsultaMercadoView data)
        {
            if (data != null)
            {
                //ConsultaMercado consultaMercado = DBConsultaMercado.GetDetalheConsultaMercado(data.NumConsultaMercado);
                ConsultaMercado consultaMercado = DBConsultaMercado.Create(User.Identity.Name);

                consultaMercado.CodProjecto = data.CodProjecto;
                consultaMercado.Descricao = data.Descricao;
                consultaMercado.CodRegiao = data.CodRegiao;
                consultaMercado.CodAreaFuncional = data.CodAreaFuncional;
                consultaMercado.CodCentroResponsabilidade = data.CodCentroResponsabilidade;
                consultaMercado.CodActividade = data.CodActividade;
                consultaMercado.DataPedidoCotacao = data.DataPedidoCotacao;
                consultaMercado.FornecedorSelecionado = data.FornecedorSelecionado;
                consultaMercado.NumDocumentoCompra = data.NumDocumentoCompra;
                consultaMercado.CodLocalizacao = data.CodLocalizacao;
                consultaMercado.FiltroActividade = data.FiltroActividade;
                consultaMercado.ValorPedidoCotacao = data.ValorPedidoCotacao;
                consultaMercado.Destino = data.Destino;
                consultaMercado.Estado = data.Estado;
                consultaMercado.UtilizadorRequisicao = data.UtilizadorRequisicao;
                consultaMercado.DataLimite = data.DataLimite;
                consultaMercado.EspecificacaoTecnica = data.EspecificacaoTecnica;
                consultaMercado.Fase = data.Fase;
                consultaMercado.Modalidade = data.Modalidade;
                //consultaMercado.PedidoCotacaoCriadoEm = data.PedidoCotacaoCriadoEm;
                //consultaMercado.PedidoCotacaoCriadoPor = data.PedidoCotacaoCriadoPor;
                consultaMercado.ConsultaEm = data.ConsultaEm;
                consultaMercado.ConsultaPor = data.ConsultaPor;
                consultaMercado.NegociacaoContratacaoEm = data.NegociacaoContratacaoEm;
                consultaMercado.NegociacaoContratacaoPor = data.NegociacaoContratacaoPor;
                consultaMercado.AdjudicacaoEm = data.AdjudicacaoEm;
                consultaMercado.AdjudicacaoPor = data.AdjudicacaoPor;
                consultaMercado.NumRequisicao = data.NumRequisicao;
                consultaMercado.PedidoCotacaoOrigem = data.NumConsultaMercado;
                consultaMercado.ValorAdjudicado = data.ValorAdjudicado;
                consultaMercado.CodFormaPagamento = data.CodFormaPagamento;
                consultaMercado.SeleccaoEfectuada = data.SeleccaoEfectuada;
                consultaMercado.NumEncomenda = data.NumEncomenda;
                consultaMercado.EmailEnviado = data.EmailEnviado;
                consultaMercado.RegiaoMercadoLocal = data.RegiaoMercadoLocal;
                consultaMercado.DataEntregaFornecedor = data.DataEntregaFornecedor;
                consultaMercado.DataRecolha = data.DataRecolha;
                consultaMercado.DataEntregaArmazem = data.DataEntregaArmazem;
                consultaMercado.CodComprador = data.CodComprador;

                consultaMercado = DBConsultaMercado.Update(consultaMercado);

                if (data.LinhasConsultaMercado != null)
                {
                    foreach (LinhasConsultaMercadoView cmv in data.LinhasConsultaMercado)
                    {
                        DBConsultaMercado.Create_Copia(cmv, consultaMercado.NumConsultaMercado, User.Identity.Name);
                    }
                }

                if (data.CondicoesPropostasFornecedores != null)
                {
                    foreach (CondicoesPropostasFornecedoresView cpfv in data.CondicoesPropostasFornecedores)
                    {
                        DBConsultaMercado.Create_Copia(cpfv, consultaMercado.NumConsultaMercado, User.Identity.Name);
                    }
                }


                if (data.LinhasCondicoesPropostasFornecedores != null)
                {
                    foreach (LinhasCondicoesPropostasFornecedoresView lcpfv in data.LinhasCondicoesPropostasFornecedores)
                    {
                        DBConsultaMercado.Create_Copia(lcpfv, consultaMercado.NumConsultaMercado, User.Identity.Name);
                    }
                }

                if (data.SeleccaoEntidades != null)
                {
                    foreach (SeleccaoEntidadesView sev in data.SeleccaoEntidades)
                    {
                        DBConsultaMercado.Create_Copia(sev, consultaMercado.NumConsultaMercado, User.Identity.Name);
                    }
                }

                ConsultaMercadoView result = DBConsultaMercado.CastConsultaMercadoToView(consultaMercado);
                result.eReasonCode = 0;
                result.eMessage = "Consulta de Mercado copiada com sucesso!";

                return Json(result);
            }

            data.eReasonCode = -1;
            data.eMessage = "Por uma razão desconhecida, não foi efectuada qualquer cópia";
            return Json(data);
        }


        [HttpPost]
        public JsonResult FecharPedido([FromBody] ConsultaMercadoView data)
        {
            if (data != null)
            {
                ConsultaMercado consultaMercado = DBConsultaMercado.GetDetalheConsultaMercado(data.NumConsultaMercado);

                if (consultaMercado != null)
                {
                    List<EnumData> Fases = EnumerablesFixed.Fase;
                    List<EnumData> Estados = EnumerablesFixed.Estado;

                    consultaMercado.Estado = Estados[1].Id;
                    consultaMercado.Fase = Fases[4].Id;
                    consultaMercado = DBConsultaMercado.Update(consultaMercado);

                    //Criar uma versão no histórico, com versão incrementada em 1
                    HistoricoConsultaMercado historicoconsultaMercado = DBConsultaMercado.Create(consultaMercado);

                    if (historicoconsultaMercado != null || historicoconsultaMercado.NumConsultaMercado != "")
                    {
                        int _numversao = historicoconsultaMercado.NumVersao;

                        //Histórico Linhas Consulta Mercado
                        foreach (LinhasConsultaMercado lin in consultaMercado.LinhasConsultaMercado)
                        {
                            DBConsultaMercado.Create_Hist(lin, _numversao);
                        }

                        //Histórico Condições Propostas Fornecedores
                        foreach (CondicoesPropostasFornecedores lin in consultaMercado.CondicoesPropostasFornecedores)
                        {
                            DBConsultaMercado.Create_Hist(lin, _numversao);
                        }

                        //Histórico Linhas Condições Propostas Fornecedores
                        foreach (LinhasCondicoesPropostasFornecedores lin in consultaMercado.LinhasCondicoesPropostasFornecedores)
                        {
                            DBConsultaMercado.Create_Hist(lin, _numversao);
                        }

                        //Histórico Selecção Entidades
                        foreach (SeleccaoEntidades lin in consultaMercado.SeleccaoEntidades)
                        {
                            DBConsultaMercado.Create_Hist(lin, _numversao);
                        }
                    }

                    ConsultaMercadoView result = DBConsultaMercado.CastConsultaMercadoToView(consultaMercado);
                    result.eReasonCode = 0;
                    result.eMessage = "Pedido Fechado com sucesso!";

                    return Json(result);
                }

                data.eReasonCode = -1;
                data.eMessage = "Aconteceu algo errado e não foi Fechado o Pedido!";
                //return GetDetalheConsultaMercado(data);
                return Json(data);
            }

            data.eReasonCode = -1;
            data.eMessage = "Aconteceu algo errado e não foi Fechado o Pedido!";
            //return GetDetalheConsultaMercado(data);
            return Json(data);
        }


        [HttpPost]
        public JsonResult ConfirmarPedido([FromBody] ConsultaMercadoView data)
        {
            if (data != null)
            {
                ConsultaMercado consultaMercado = DBConsultaMercado.GetDetalheConsultaMercado(data.NumConsultaMercado);

                if (consultaMercado != null)
                {
                    List<EnumData> Fases = EnumerablesFixed.Fase;

                    consultaMercado.Fase = Fases[1].Id;
                    consultaMercado = DBConsultaMercado.Update(consultaMercado);

                    ConsultaMercadoView result = DBConsultaMercado.CastConsultaMercadoToView(consultaMercado);
                    result.eReasonCode = 0;
                    result.eMessage = "Pedido Confirmado com sucesso!";

                    return Json(result);
                }

                data.eReasonCode = -1;
                data.eMessage = "Aconteceu algo errado e não foi Confirmado o Pedido!";
                //return GetDetalheConsultaMercado(data);
                return Json(data);
            }

            data.eReasonCode = -1;
            data.eMessage = "Aconteceu algo errado e não foi Confirmado o Pedido!";
            //return GetDetalheConsultaMercado(data);
            return Json(data);
        }


        [HttpPost]
        public JsonResult GerarRegistoPropostas([FromBody] ConsultaMercadoView data)
        {
            /*
             Verificar se para a consulta de mercado e para o fornecedor já existe Alternativa > 0
             Inserir registo na tabela "Condicoes_Propostas_Fornecedores"
             Para cada registo acima, inserir as linhas da consulta de mercado na tabela "Linhas_Condicoes_Propostas_Fornecedores"
             */

            if (data != null)
            {
                ConsultaMercado consultaMercado = DBConsultaMercado.GetDetalheConsultaMercado(data.NumConsultaMercado);

                string _Alternativa = string.Empty;
                foreach (SeleccaoEntidades seleccaoEntidades in consultaMercado.SeleccaoEntidades)
                {
                    _Alternativa = DBConsultaMercado.Get_MAX_Alternativa_CondicoesPropostasFornecedores(data.NumConsultaMercado, seleccaoEntidades.CodFornecedor);

                    if (_Alternativa == null)
                    {
                        _Alternativa = "0";
                    }
                    else
                    {
                        _Alternativa = (int.Parse(_Alternativa) + 1).ToString();
                    }

                    //Inserir registo na tabela "Condicoes_Propostas_Fornecedores", com o valor Alternativa calculado acima
                    CondicoesPropostasFornecedores condicoesPropostasFornecedores = DBConsultaMercado.Create(seleccaoEntidades, _Alternativa);

                    //Para cada registo, inserir as linhas da consulta de mercado na tabela "Linhas_Condicoes_Propostas_Fornecedores"
                    foreach (LinhasConsultaMercado linhasConsultaMercado in consultaMercado.LinhasConsultaMercado)
                    {
                        LinhasCondicoesPropostasFornecedores linhasCondicoesPropostasFornecedores = DBConsultaMercado.Create(linhasConsultaMercado, _Alternativa, seleccaoEntidades.CodFornecedor);
                    }
                }


                //NOVO MÉTODO, QUE SUBSTITUI O USO DAS DUAS TABELAS ACIMA, "Condicoes_Propostas_Fornecedores" e "Linhas_Condicoes_Propostas_Fornecedores"
                //GRAVA NA NOVA TABELA "Registo_De_Propostas"
                //Para cada registo, inserir as linhas da consulta de mercado na tabela "Linhas_Condicoes_Propostas_Fornecedores"
                foreach (LinhasConsultaMercado linhasConsultaMercado in consultaMercado.LinhasConsultaMercado)
                {
                    RegistoDePropostas registoDePropostas = DBConsultaMercado.Create(linhasConsultaMercado, _Alternativa, _config.NAVDatabaseName, _config.NAVCompanyName);
                }
                

                consultaMercado = DBConsultaMercado.GetDetalheConsultaMercado(data.NumConsultaMercado);

                data = DBConsultaMercado.CastConsultaMercadoToView(consultaMercado);
                data.eReasonCode = 0;
                data.eMessage = "Foi Gerado o Registo de Proposta!";
                return Json(data);
            }

            data.eReasonCode = -1;
            data.eMessage = "Aconteceu algo errado e não foi possível Gerar o Registo de Proposta!";
            return Json(data);
        }

        [HttpPost]
        public JsonResult CriarEncomenda([FromBody] ConsultaMercadoView data)
        {
            PurchOrderDTO purchOrderDTO = new PurchOrderDTO();
            List<PurchOrderDTO> purchOrders = new List<PurchOrderDTO>();
            List<RegistoDePropostasView> registoDePropostas = new List<RegistoDePropostasView>();

            try
            {
                for (int i = 1; i <= 6; i++)
                {
                    if (i == 1)
                    {
                        registoDePropostas = data.RegistoDePropostas.Where(x => x.Fornecedor1Select.HasValue && x.Fornecedor1Select.Value == true).ToList();

                        if (registoDePropostas.Count() > 0)
                        {
                            //vamos criar a encomenda com as linhas
                            PurchOrderLineDTO purchOrderLineDTO = new PurchOrderLineDTO();
                            List<PurchOrderLineDTO> purchOrderLineDTOs = new List<PurchOrderLineDTO>();

                            string VAT_Fornecedor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == registoDePropostas[0].Fornecedor1Code).FirstOrDefault().VATBusinessPostingGroup;

                            foreach (RegistoDePropostasView registoDePropostasView in registoDePropostas)
                            {
                                purchOrderLineDTO.CenterResponsibilityCode = registoDePropostasView.CodCentroResponsabilidade;
                                purchOrderLineDTO.Code = registoDePropostasView.CodProduto;
                                purchOrderLineDTO.Description = registoDePropostasView.Descricao;
                                purchOrderLineDTO.Description2 = registoDePropostasView.Descricao2 ?? string.Empty;
                                purchOrderLineDTO.DiscountPercentage = 0;
                                purchOrderLineDTO.FunctionalAreaCode = registoDePropostasView.CodAreaFuncional;
                                purchOrderLineDTO.LineId = registoDePropostasView.NumLinha;
                                purchOrderLineDTO.LocationCode = registoDePropostasView.CodLocalizacao;
                                //purchOrderLineDTO.OpenOrderLineNo = registoDePropostasView.NumLinhaConsultaMercado;
                                //purchOrderLineDTO.OpenOrderNo = registoDePropostasView.NumConsultaMercado;
                                purchOrderLineDTO.ProjectNo = registoDePropostasView.NumProjecto;
                                purchOrderLineDTO.QuantityRequired = registoDePropostasView.Quantidade;
                                purchOrderLineDTO.RegionCode = registoDePropostasView.CodRegiao;
                                purchOrderLineDTO.UnitCost = registoDePropostasView.Fornecedor1Preco;
                                purchOrderLineDTO.UnitMeasureCode = data.LinhasConsultaMercado.Where(x => x.NumLinha == registoDePropostasView.NumLinhaConsultaMercado).FirstOrDefault().CodUnidadeMedida;

                                //purchOrderLineDTO.VATBusinessPostingGroup = VAT_Fornecedor;
                                //purchOrderLineDTO.VATProductPostingGroup = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, registoDePropostasView.CodProduto).FirstOrDefault().VATProductPostingGroup;

                                purchOrderLineDTO.VATBusinessPostingGroup = registoDePropostasView.VatbusinessPostingGroup1;
                                purchOrderLineDTO.VATProductPostingGroup = registoDePropostasView.VatproductPostingGroup;

                                purchOrderLineDTOs.Add(purchOrderLineDTO);
                            }

                            PurchOrderDTO purchOrderDTO1 = new PurchOrderDTO()
                            {
                                CenterResponsibilityCode = data.CodCentroResponsabilidade,
                                FunctionalAreaCode = data.CodAreaFuncional,
                                InAdvance = false,
                                LocalMarketRegion = string.Empty,
                                PricesIncludingVAT = false,
                                RegionCode = data.CodRegiao,
                                RequisitionId = data.NumRequisicao,
                                SupplierId = registoDePropostas[0].Fornecedor1Code,
                                Lines = purchOrderLineDTOs
                            };

                            purchOrders.Add(purchOrderDTO1);
                        }
                    }

                    if (i == 2)
                    {
                        registoDePropostas = data.RegistoDePropostas.Where(x => x.Fornecedor2Select.HasValue && x.Fornecedor2Select.Value == true).ToList();

                        if (registoDePropostas.Count() > 0)
                        {
                            //vamos criar a encomenda com as linhas
                            PurchOrderLineDTO purchOrderLineDTO = new PurchOrderLineDTO();
                            List<PurchOrderLineDTO> purchOrderLineDTOs = new List<PurchOrderLineDTO>();

                            string VAT_Fornecedor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == registoDePropostas[0].Fornecedor2Code).FirstOrDefault().VATBusinessPostingGroup;

                            foreach (RegistoDePropostasView registoDePropostasView in registoDePropostas)
                            {
                                purchOrderLineDTO.CenterResponsibilityCode = registoDePropostasView.CodCentroResponsabilidade;
                                purchOrderLineDTO.Code = registoDePropostasView.CodProduto;
                                purchOrderLineDTO.Description = registoDePropostasView.Descricao;
                                purchOrderLineDTO.Description2 = registoDePropostasView.Descricao2 ?? string.Empty;
                                purchOrderLineDTO.DiscountPercentage = 0;
                                purchOrderLineDTO.FunctionalAreaCode = registoDePropostasView.CodAreaFuncional;
                                purchOrderLineDTO.LineId = registoDePropostasView.NumLinha;
                                purchOrderLineDTO.LocationCode = registoDePropostasView.CodLocalizacao;
                                //purchOrderLineDTO.OpenOrderLineNo = registoDePropostasView.NumLinhaConsultaMercado;
                                //purchOrderLineDTO.OpenOrderNo = registoDePropostasView.NumConsultaMercado;
                                purchOrderLineDTO.ProjectNo = registoDePropostasView.NumProjecto;
                                purchOrderLineDTO.QuantityRequired = registoDePropostasView.Quantidade;
                                purchOrderLineDTO.RegionCode = registoDePropostasView.CodRegiao;
                                purchOrderLineDTO.UnitCost = registoDePropostasView.Fornecedor2Preco;
                                purchOrderLineDTO.UnitMeasureCode = data.LinhasConsultaMercado.Where(x => x.NumLinha == registoDePropostasView.NumLinhaConsultaMercado).FirstOrDefault().CodUnidadeMedida;

                                //purchOrderLineDTO.VATBusinessPostingGroup = VAT_Fornecedor;
                                //purchOrderLineDTO.VATProductPostingGroup = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, registoDePropostasView.CodProduto).FirstOrDefault().VATProductPostingGroup;

                                purchOrderLineDTO.VATBusinessPostingGroup = registoDePropostasView.VatbusinessPostingGroup2;
                                purchOrderLineDTO.VATProductPostingGroup = registoDePropostasView.VatproductPostingGroup;

                                purchOrderLineDTOs.Add(purchOrderLineDTO);
                            }

                            PurchOrderDTO purchOrderDTO2 = new PurchOrderDTO()
                            {
                                CenterResponsibilityCode = data.CodCentroResponsabilidade,
                                FunctionalAreaCode = data.CodAreaFuncional,
                                InAdvance = false,
                                LocalMarketRegion = string.Empty,
                                PricesIncludingVAT = false,
                                RegionCode = data.CodRegiao,
                                RequisitionId = data.NumRequisicao,
                                SupplierId = registoDePropostas[0].Fornecedor2Code,
                                Lines = purchOrderLineDTOs
                            };

                            purchOrders.Add(purchOrderDTO2);
                        }
                    }

                    if (i == 3)
                    {
                        registoDePropostas = data.RegistoDePropostas.Where(x => x.Fornecedor3Select.HasValue && x.Fornecedor3Select.Value == true).ToList();

                        if (registoDePropostas.Count() > 0)
                        {
                            //vamos criar a encomenda com as linhas
                            PurchOrderLineDTO purchOrderLineDTO = new PurchOrderLineDTO();
                            List<PurchOrderLineDTO> purchOrderLineDTOs = new List<PurchOrderLineDTO>();

                            string VAT_Fornecedor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == registoDePropostas[0].Fornecedor3Code).FirstOrDefault().VATBusinessPostingGroup;

                            foreach (RegistoDePropostasView registoDePropostasView in registoDePropostas)
                            {
                                purchOrderLineDTO.CenterResponsibilityCode = registoDePropostasView.CodCentroResponsabilidade;
                                purchOrderLineDTO.Code = registoDePropostasView.CodProduto;
                                purchOrderLineDTO.Description = registoDePropostasView.Descricao;
                                purchOrderLineDTO.Description2 = registoDePropostasView.Descricao2 ?? string.Empty;
                                purchOrderLineDTO.DiscountPercentage = 0;
                                purchOrderLineDTO.FunctionalAreaCode = registoDePropostasView.CodAreaFuncional;
                                purchOrderLineDTO.LineId = registoDePropostasView.NumLinha;
                                purchOrderLineDTO.LocationCode = registoDePropostasView.CodLocalizacao;
                                //purchOrderLineDTO.OpenOrderLineNo = registoDePropostasView.NumLinhaConsultaMercado;
                                //purchOrderLineDTO.OpenOrderNo = registoDePropostasView.NumConsultaMercado;
                                purchOrderLineDTO.ProjectNo = registoDePropostasView.NumProjecto;
                                purchOrderLineDTO.QuantityRequired = registoDePropostasView.Quantidade;
                                purchOrderLineDTO.RegionCode = registoDePropostasView.CodRegiao;
                                purchOrderLineDTO.UnitCost = registoDePropostasView.Fornecedor3Preco;
                                purchOrderLineDTO.UnitMeasureCode = data.LinhasConsultaMercado.Where(x => x.NumLinha == registoDePropostasView.NumLinhaConsultaMercado).FirstOrDefault().CodUnidadeMedida;

                                //purchOrderLineDTO.VATBusinessPostingGroup = VAT_Fornecedor;
                                //purchOrderLineDTO.VATProductPostingGroup = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, registoDePropostasView.CodProduto).FirstOrDefault().VATProductPostingGroup;

                                purchOrderLineDTO.VATBusinessPostingGroup = registoDePropostasView.VatbusinessPostingGroup3;
                                purchOrderLineDTO.VATProductPostingGroup = registoDePropostasView.VatproductPostingGroup;

                                purchOrderLineDTOs.Add(purchOrderLineDTO);
                            }

                            PurchOrderDTO purchOrderDTO3 = new PurchOrderDTO()
                            {
                                CenterResponsibilityCode = data.CodCentroResponsabilidade,
                                FunctionalAreaCode = data.CodAreaFuncional,
                                InAdvance = false,
                                LocalMarketRegion = string.Empty,
                                PricesIncludingVAT = false,
                                RegionCode = data.CodRegiao,
                                RequisitionId = data.NumRequisicao,
                                SupplierId = registoDePropostas[0].Fornecedor3Code,
                                Lines = purchOrderLineDTOs
                            };

                            purchOrders.Add(purchOrderDTO3);
                        }
                    }

                    if (i == 4)
                    {
                        registoDePropostas = data.RegistoDePropostas.Where(x => x.Fornecedor4Select.HasValue && x.Fornecedor4Select.Value == true).ToList();

                        if (registoDePropostas.Count() > 0)
                        {
                            //vamos criar a encomenda com as linhas
                            PurchOrderLineDTO purchOrderLineDTO = new PurchOrderLineDTO();
                            List<PurchOrderLineDTO> purchOrderLineDTOs = new List<PurchOrderLineDTO>();

                            string VAT_Fornecedor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == registoDePropostas[0].Fornecedor4Code).FirstOrDefault().VATBusinessPostingGroup;

                            foreach (RegistoDePropostasView registoDePropostasView in registoDePropostas)
                            {
                                purchOrderLineDTO.CenterResponsibilityCode = registoDePropostasView.CodCentroResponsabilidade;
                                purchOrderLineDTO.Code = registoDePropostasView.CodProduto;
                                purchOrderLineDTO.Description = registoDePropostasView.Descricao;
                                purchOrderLineDTO.Description2 = registoDePropostasView.Descricao2 ?? string.Empty;
                                purchOrderLineDTO.DiscountPercentage = 0;
                                purchOrderLineDTO.FunctionalAreaCode = registoDePropostasView.CodAreaFuncional;
                                purchOrderLineDTO.LineId = registoDePropostasView.NumLinha;
                                purchOrderLineDTO.LocationCode = registoDePropostasView.CodLocalizacao;
                                //purchOrderLineDTO.OpenOrderLineNo = registoDePropostasView.NumLinhaConsultaMercado;
                                //purchOrderLineDTO.OpenOrderNo = registoDePropostasView.NumConsultaMercado;
                                purchOrderLineDTO.ProjectNo = registoDePropostasView.NumProjecto;
                                purchOrderLineDTO.QuantityRequired = registoDePropostasView.Quantidade;
                                purchOrderLineDTO.RegionCode = registoDePropostasView.CodRegiao;
                                purchOrderLineDTO.UnitCost = registoDePropostasView.Fornecedor4Preco;
                                purchOrderLineDTO.UnitMeasureCode = data.LinhasConsultaMercado.Where(x => x.NumLinha == registoDePropostasView.NumLinhaConsultaMercado).FirstOrDefault().CodUnidadeMedida;

                                //purchOrderLineDTO.VATBusinessPostingGroup = VAT_Fornecedor;
                                //purchOrderLineDTO.VATProductPostingGroup = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, registoDePropostasView.CodProduto).FirstOrDefault().VATProductPostingGroup;

                                purchOrderLineDTO.VATBusinessPostingGroup = registoDePropostasView.VatbusinessPostingGroup4;
                                purchOrderLineDTO.VATProductPostingGroup = registoDePropostasView.VatproductPostingGroup;


                                purchOrderLineDTOs.Add(purchOrderLineDTO);
                            }

                            PurchOrderDTO purchOrderDTO4 = new PurchOrderDTO()
                            {
                                CenterResponsibilityCode = data.CodCentroResponsabilidade,
                                FunctionalAreaCode = data.CodAreaFuncional,
                                InAdvance = false,
                                LocalMarketRegion = string.Empty,
                                PricesIncludingVAT = false,
                                RegionCode = data.CodRegiao,
                                RequisitionId = data.NumRequisicao,
                                SupplierId = registoDePropostas[0].Fornecedor4Code,
                                Lines = purchOrderLineDTOs
                            };

                            purchOrders.Add(purchOrderDTO4);
                        }
                    }

                    if (i == 5)
                    {
                        registoDePropostas = data.RegistoDePropostas.Where(x => x.Fornecedor5Select.HasValue && x.Fornecedor5Select.Value == true).ToList();

                        if (registoDePropostas.Count() > 0)
                        {
                            //vamos criar a encomenda com as linhas
                            PurchOrderLineDTO purchOrderLineDTO = new PurchOrderLineDTO();
                            List<PurchOrderLineDTO> purchOrderLineDTOs = new List<PurchOrderLineDTO>();

                            string VAT_Fornecedor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == registoDePropostas[0].Fornecedor5Code).FirstOrDefault().VATBusinessPostingGroup;

                            foreach (RegistoDePropostasView registoDePropostasView in registoDePropostas)
                            {
                                purchOrderLineDTO.CenterResponsibilityCode = registoDePropostasView.CodCentroResponsabilidade;
                                purchOrderLineDTO.Code = registoDePropostasView.CodProduto;
                                purchOrderLineDTO.Description = registoDePropostasView.Descricao;
                                purchOrderLineDTO.Description2 = registoDePropostasView.Descricao2 ?? string.Empty;
                                purchOrderLineDTO.DiscountPercentage = 0;
                                purchOrderLineDTO.FunctionalAreaCode = registoDePropostasView.CodAreaFuncional;
                                purchOrderLineDTO.LineId = registoDePropostasView.NumLinha;
                                purchOrderLineDTO.LocationCode = registoDePropostasView.CodLocalizacao;
                                //purchOrderLineDTO.OpenOrderLineNo = registoDePropostasView.NumLinhaConsultaMercado;
                                //purchOrderLineDTO.OpenOrderNo = registoDePropostasView.NumConsultaMercado;
                                purchOrderLineDTO.ProjectNo = registoDePropostasView.NumProjecto;
                                purchOrderLineDTO.QuantityRequired = registoDePropostasView.Quantidade;
                                purchOrderLineDTO.RegionCode = registoDePropostasView.CodRegiao;
                                purchOrderLineDTO.UnitCost = registoDePropostasView.Fornecedor5Preco;
                                purchOrderLineDTO.UnitMeasureCode = data.LinhasConsultaMercado.Where(x => x.NumLinha == registoDePropostasView.NumLinhaConsultaMercado).FirstOrDefault().CodUnidadeMedida;

                                //purchOrderLineDTO.VATBusinessPostingGroup = VAT_Fornecedor;
                                //purchOrderLineDTO.VATProductPostingGroup = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, registoDePropostasView.CodProduto).FirstOrDefault().VATProductPostingGroup;

                                purchOrderLineDTO.VATBusinessPostingGroup = registoDePropostasView.VatbusinessPostingGroup5;
                                purchOrderLineDTO.VATProductPostingGroup = registoDePropostasView.VatproductPostingGroup;


                                purchOrderLineDTOs.Add(purchOrderLineDTO);
                            }

                            PurchOrderDTO purchOrderDTO5 = new PurchOrderDTO()
                            {
                                CenterResponsibilityCode = data.CodCentroResponsabilidade,
                                FunctionalAreaCode = data.CodAreaFuncional,
                                InAdvance = false,
                                LocalMarketRegion = string.Empty,
                                PricesIncludingVAT = false,
                                RegionCode = data.CodRegiao,
                                RequisitionId = data.NumRequisicao,
                                SupplierId = registoDePropostas[0].Fornecedor5Code,
                                Lines = purchOrderLineDTOs
                            };

                            purchOrders.Add(purchOrderDTO5);
                        }
                    }

                    if (i == 6)
                    {
                        registoDePropostas = data.RegistoDePropostas.Where(x => x.Fornecedor6Select.HasValue && x.Fornecedor6Select.Value == true).ToList();

                        if (registoDePropostas.Count() > 0)
                        {
                            //vamos criar a encomenda com as linhas
                            PurchOrderLineDTO purchOrderLineDTO = new PurchOrderLineDTO();
                            List<PurchOrderLineDTO> purchOrderLineDTOs = new List<PurchOrderLineDTO>();

                            string VAT_Fornecedor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == registoDePropostas[0].Fornecedor6Code).FirstOrDefault().VATBusinessPostingGroup;

                            foreach (RegistoDePropostasView registoDePropostasView in registoDePropostas)
                            {
                                purchOrderLineDTO.CenterResponsibilityCode = registoDePropostasView.CodCentroResponsabilidade;
                                purchOrderLineDTO.Code = registoDePropostasView.CodProduto;
                                purchOrderLineDTO.Description = registoDePropostasView.Descricao;
                                purchOrderLineDTO.Description2 = registoDePropostasView.Descricao2 ?? string.Empty;
                                purchOrderLineDTO.DiscountPercentage = 0;
                                purchOrderLineDTO.FunctionalAreaCode = registoDePropostasView.CodAreaFuncional;
                                purchOrderLineDTO.LineId = registoDePropostasView.NumLinha;
                                purchOrderLineDTO.LocationCode = registoDePropostasView.CodLocalizacao;
                                //purchOrderLineDTO.OpenOrderLineNo = registoDePropostasView.NumLinhaConsultaMercado;
                                //purchOrderLineDTO.OpenOrderNo = registoDePropostasView.NumConsultaMercado;
                                purchOrderLineDTO.ProjectNo = registoDePropostasView.NumProjecto;
                                purchOrderLineDTO.QuantityRequired = registoDePropostasView.Quantidade;
                                purchOrderLineDTO.RegionCode = registoDePropostasView.CodRegiao;
                                purchOrderLineDTO.UnitCost = registoDePropostasView.Fornecedor6Preco;
                                purchOrderLineDTO.UnitMeasureCode = data.LinhasConsultaMercado.Where(x => x.NumLinha == registoDePropostasView.NumLinhaConsultaMercado).FirstOrDefault().CodUnidadeMedida;

                                //purchOrderLineDTO.VATBusinessPostingGroup = VAT_Fornecedor;
                                //purchOrderLineDTO.VATProductPostingGroup = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, registoDePropostasView.CodProduto).FirstOrDefault().VATProductPostingGroup;

                                purchOrderLineDTO.VATBusinessPostingGroup = registoDePropostasView.VatbusinessPostingGroup6;
                                purchOrderLineDTO.VATProductPostingGroup = registoDePropostasView.VatproductPostingGroup;
                                
                                purchOrderLineDTOs.Add(purchOrderLineDTO);
                            }

                            PurchOrderDTO purchOrderDTO6 = new PurchOrderDTO()
                            {
                                CenterResponsibilityCode = data.CodCentroResponsabilidade,
                                FunctionalAreaCode = data.CodAreaFuncional,
                                InAdvance = false,
                                LocalMarketRegion = string.Empty,
                                PricesIncludingVAT = false,
                                RegionCode = data.CodRegiao,
                                RequisitionId = data.NumRequisicao,
                                SupplierId = registoDePropostas[0].Fornecedor6Code,
                                Lines = purchOrderLineDTOs
                            };

                            purchOrders.Add(purchOrderDTO6);
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Ocorreu um erro ao agrupar as linhas.");
            }

            if (purchOrders.Count() > 0)
            {
                purchOrders.ForEach(purchOrder =>
                {
                    try
                    {
                        var result = CreateNAVPurchaseOrderFor(purchOrder);
                        if (result.CompletedSuccessfully)
                        {
                            data.eMessages.Add(new TraceInformation(TraceType.Success, "Criada encomenda para o fornecedor núm. " + purchOrder.SupplierId + "; "));
                        }
                    }
                    catch (Exception ex)
                    {
                        data.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro ao criar encomenda para o fornecedor núm. " + purchOrder.SupplierId + ": " + ex.Message));
                    }
                });

                if (data.eMessages.Any(x => x.Type == TraceType.Error))
                {
                    data.eReasonCode = 2;
                    data.eMessage = "Ocorreram erros ao criar encomenda de compra." + Environment.NewLine + data.eMessages[data.eMessages.Count() - 1].Message;
                }
                else
                {
                    data.eReasonCode = 1;
                    data.eMessage = "Encomenda de compra criada com sucesso.";
                }
            }
            else
            {
                data.eReasonCode = 3;
                data.eMessage = "Não existem linhas que cumpram os requisitos de validação do mercado local.";
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> EnviarEmailATodos([FromBody] ConsultaMercadoView data)
        {
            data.eReasonCode = 0;
            data.eMessage = "Email(s) não enviado(s)!";

            //Para cada Fornecedor, criar o pdf da Consulta de Mercado, guardar em algum lado e anexar ao email e enviar!!!
            foreach (SeleccaoEntidadesView fornecedor in data.SeleccaoEntidades)
            {
                string Consulta = data.NumConsultaMercado;
                string Cod = fornecedor.CodFornecedor;

                string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
                string sFileName = @Consulta + "_" + Cod + "_" + ".pdf";

                //var theURL = (_config.ReportServerURL + "ConsultaMercado&rs:Command=Render&rs:format=PDF&CM=" + Consulta + "&Fornecedor=" + Cod);
                var theURL = (_config.ReportServerURL + "ConsultaMercado&CM=" + Consulta + "&Fornecedor=" + Cod + "&rs:Command=Render&rs:format=PDF");

                //WebClient Client = new WebClient
                //{
                //    UseDefaultCredentials = true
                //};

                //OBTER CREDENCIAIS PARA O SERVIDOR DE REPORTS
                Configuração config = DBConfigurations.GetById(1);

                WebClient Client = new WebClient
                {
                    Credentials = new NetworkCredential(config.ReportUsername, config.ReportPassword)
                };

                byte[] myDataBuffer = Client.DownloadData(theURL);

                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    await fs.WriteAsync(myDataBuffer, 0, myDataBuffer.Length);
                }

                Stream _my_stream = new MemoryStream(myDataBuffer);

                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(_my_stream);
                }

                SendEmailsPedidoCotacao Email = new SendEmailsPedidoCotacao
                {
                    DisplayName = User.Identity.Name,
                    Subject = "Pedido de Cotação",
                    From = User.Identity.Name,
                    Anexo = Path.Combine(sWebRootFolder, sFileName)
                };

                //Email.To.Add(data.SeleccaoEntidades.Where(x => x.CodFornecedor == Cod).First().EmailFornecedor);
                Email.To.Add(User.Identity.Name);

                Email.Body = MakeEmailBodyContent("Solicitamos Pedido de Cotação", User.Identity.Name);
                Email.IsBodyHtml = true;

                Email.SendEmail();

                string email = data.SeleccaoEntidades.Where(x => x.CodFornecedor == Cod).First().EmailFornecedor ?? "Fornecedor sem Email definido!";

                if (data.eReasonCode == 0)
                {
                    data.eReasonCode = 1;
                    data.eMessage = "Email enviado com sucesso para:" + Environment.NewLine + email;

                    data.EmailEnviado = true;
                    DBConsultaMercado.Update(data);
                }
                else
                {
                    data.eMessage += Environment.NewLine + email;
                }

                //Actualizar Tabela "Seleccao_Entidades", com Data de Envio Ao Fornecedor e com Utilizador Envio
                fornecedor.DataEnvioAoFornecedor = DateTime.Now;
                fornecedor.UtilizadorEnvio = User.Identity.Name;
                DBConsultaMercado.Update(DBConsultaMercado.CastSeleccaoEntidadesViewToDB(fornecedor));

            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> EnviarEmailAUm([FromBody] JObject requestParams)
        {
            //Para o fornecedor selecionado, criar o pdf da Consulta de Mercado, guardar em algum lado e anexar ao email e enviar!!!
            string Consulta = requestParams["Consulta"].ToString();
            string Cod = requestParams["Cod"].ToString();

            ConsultaMercado consultaMercado = DBConsultaMercado.GetDetalheConsultaMercado(Consulta);
            ConsultaMercadoView data = DBConsultaMercado.CastConsultaMercadoToView(consultaMercado);


            data.eReasonCode = 0;
            data.eMessage = "Email não enviado!";


            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
            string sFileName = @Consulta + "_" + Cod + "_" + ".pdf";

            //var theURL = (_config.ReportServerURL + "ConsultaMercado&rs:Command=Render&rs:format=PDF&CM=" + Consulta + "&Fornecedor=" + Cod);
            var theURL = (_config.ReportServerURL + "ConsultaMercado&CM=" + Consulta + "&Fornecedor=" + Cod + "&rs:Command=Render&rs:format=PDF");

            //WebClient Client = new WebClient
            //{
            //    UseDefaultCredentials = true
            //};

            //OBTER CREDENCIAIS PARA O SERVIDOR DE REPORTS
            Configuração config = DBConfigurations.GetById(1);

            WebClient Client = new WebClient
            {
                Credentials = new NetworkCredential(config.ReportUsername, config.ReportPassword)
            };


            byte[] myDataBuffer = Client.DownloadData(theURL);

            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                await fs.WriteAsync(myDataBuffer, 0, myDataBuffer.Length);
            }

            Stream _my_stream = new MemoryStream(myDataBuffer);

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(_my_stream);
            }

            SendEmailsPedidoCotacao Email = new SendEmailsPedidoCotacao
            {
                DisplayName = User.Identity.Name,
                Subject = "Pedido de Cotação",
                From = User.Identity.Name,
                Anexo = Path.Combine(sWebRootFolder, sFileName)
            };

            //Email.To.Add(data.SeleccaoEntidades.Where(x => x.CodFornecedor == Cod).First().EmailFornecedor);
            Email.To.Add(User.Identity.Name);

            Email.Body = MakeEmailBodyContent("Solicitamos Pedido de Cotação", User.Identity.Name);
            Email.IsBodyHtml = true;
            
            Email.SendEmail();

            string email = data.SeleccaoEntidades.Where(x => x.CodFornecedor == Cod).First().EmailFornecedor ?? "Fornecedor sem Email definido!";

            data.eReasonCode = 1;
            data.eMessage = "Email enviado com sucesso para:" + Environment.NewLine + email;

            data.EmailEnviado = true;
            DBConsultaMercado.Update(data);


            //Actualizar Tabela "Seleccao_Entidades", com Data de Envio Ao Fornecedor e com Utilizador Envio
            SeleccaoEntidadesView fornecedor = data.SeleccaoEntidades.Where(x => x.CodFornecedor == Cod).First();
            fornecedor.DataEnvioAoFornecedor = DateTime.Now;
            fornecedor.UtilizadorEnvio = User.Identity.Name;
            DBConsultaMercado.Update(DBConsultaMercado.CastSeleccaoEntidadesViewToDB(fornecedor));

            return Json(data);
        }

        public static string MakeEmailBodyContent(string BodyText, string SenderName)
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
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Exmos (as) Senhores (as)," +
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
                                                SenderName +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
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

        private GenericResult CreateNAVPurchaseOrderFor(PurchOrderDTO purchOrder)
        {
            GenericResult createPrePurchOrderResult = new GenericResult();

            Task<WSPurchaseInvHeader.Create_Result> createPurchaseHeaderTask = NAVPurchaseHeaderIntermService.CreateAsync(purchOrder, configws);
            createPurchaseHeaderTask.Wait();
            if (createPurchaseHeaderTask.IsCompletedSuccessfully)
            {
                createPrePurchOrderResult.ResultValue = createPurchaseHeaderTask.Result.WSPurchInvHeaderInterm.No;
                purchOrder.NAVPrePurchOrderId = createPrePurchOrderResult.ResultValue;

                bool createPurchaseLinesTask = NAVPurchaseLineService.CreateAndUpdateMultipleAsync(purchOrder, configws);
                if (createPurchaseLinesTask)
                {
                    try
                    {
                        /*
                         *  Swallow errors at this stage as they will be managed in NAV
                         */
                        //Task<WSGenericCodeUnit.FxCabimento_Result> createPurchOrderTask = WSGeneric.CreatePurchaseOrder(purchOrder.NAVPrePurchOrderId, configws);
                        //createPurchOrderTask.Start();
                        ////if (createPurchOrderTask.IsCompletedSuccessfully)
                        ////{
                        ////    createPrePurchOrderResult.CompletedSuccessfully = true;
                        ////}
                    }
                    catch (Exception ex) { }
                    createPrePurchOrderResult.CompletedSuccessfully = true;
                }
            }
            return createPrePurchOrderResult;
        }

        #region Linhas Consulta Mercado

        [HttpPost]
        public JsonResult CreateLinhaConsultaMercado([FromBody] LinhasConsultaMercadoView data)
        {
            bool result = false;
            try
            {
                LinhasConsultaMercado linhaConsultaMercado = new LinhasConsultaMercado
                {
                    CodActividade = data.CodActividade,
                    CodAreaFuncional = data.CodAreaFuncional,
                    CodCentroResponsabilidade = data.CodCentroResponsabilidade,
                    CodLocalizacao = data.CodLocalizacao,
                    CodProduto = data.CodProduto,
                    CodRegiao = data.CodRegiao,
                    CodUnidadeMedida = data.CodUnidadeMedida,
                    CriadoEm = DateTime.Now,
                    CriadoPor = User.Identity.Name,
                    CustoTotalObjectivo = data.CustoTotalObjectivo,
                    CustoTotalPrevisto = data.CustoTotalPrevisto,
                    CustoUnitarioObjectivo = data.CustoUnitarioObjectivo,
                    CustoUnitarioPrevisto = data.CustoUnitarioPrevisto,
                    DataEntregaPrevista = data.DataEntregaPrevista_Show != string.Empty ? DateTime.Parse(data.DataEntregaPrevista_Show) : (DateTime?)null,
                    Descricao = data.Descricao,
                    Descricao2 = data.Descricao2,
                    LinhaRequisicao = data.LinhaRequisicao,
                    ModificadoEm = data.ModificadoEm,
                    ModificadoPor = data.ModificadoPor,
                    NumConsultaMercado = data.NumConsultaMercado,
                    //linhaConsultaMercado.NumLinha = data.NumLinha;
                    NumProjecto = data.NumProjecto,
                    NumRequisicao = data.NumRequisicao,
                    Quantidade = data.Quantidade
                };

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
        public JsonResult UpdateLinhaConsultaMercado([FromBody] LinhasConsultaMercadoView data)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(data.CodProduto))
                {
                    NAVProductsViewModel PROD = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, data.CodProduto).FirstOrDefault();

                    if (PROD != null)
                    {
                        data.Descricao = PROD.Name;
                        data.CodUnidadeMedida = PROD.MeasureUnit;
                    }
                }
                else
                {
                    data.Descricao = null;
                    data.CodUnidadeMedida = null;
                }

                if (data.Quantidade != null && data.CustoUnitarioPrevisto != null)
                    data.CustoTotalPrevisto = Math.Round((decimal)data.Quantidade * (decimal)data.CustoUnitarioPrevisto * 100) / 100;
                else
                    data.CustoTotalPrevisto = null;

                if (data.Quantidade != null && data.CustoUnitarioObjectivo != null)
                    data.CustoTotalObjectivo = Math.Round((decimal)data.Quantidade * (decimal)data.CustoUnitarioObjectivo * 100) / 100;
                else
                    data.CustoTotalObjectivo = null;

                LinhasConsultaMercado linhaConsultaMercado = DBConsultaMercado.CastLinhasConsultaMercadoViewToDB(data);
                
                linhaConsultaMercado.ModificadoEm = DateTime.Now;
                linhaConsultaMercado.ModificadoPor = User.Identity.Name;

                var dbUpdateResult = DBConsultaMercado.Update(linhaConsultaMercado);

                if (dbUpdateResult != null)
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

        #region Selecção Entidades

        [HttpPost]
        public JsonResult CreateLinhaSeleccaoEntidade([FromBody] SeleccaoEntidadesView data)
        {
            bool result = false;
            try
            {

                string _Email = string.Empty;
                try
                {
                    _Email = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == data.CodFornecedor).First().Email;
                }
                catch
                {
                    _Email = string.Empty;
                }


                SeleccaoEntidades seleccaoEntidades = new SeleccaoEntidades
                {
                    CidadeFornecedor = null,
                    CodActividade = data.CodActividade,
                    CodFormaPagamento = data.CodFormaPagamento,
                    CodFornecedor = data.CodFornecedor,
                    CodTermosPagamento = data.CodTermosPagamento,
                    NomeFornecedor = data.NomeFornecedor,
                    NumConsultaMercado = data.NumConsultaMercado,
                    Preferencial = data.Preferencial,
                    Selecionado = true,
                    EmailFornecedor = _Email
                };

                var dbCreateResult = DBConsultaMercado.Create(seleccaoEntidades);

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
        public JsonResult UpdateLinhaSeleccaoEntidade([FromBody] SeleccaoEntidadesView data)
        {
            bool result = false;
            try
            {
                SeleccaoEntidades seleccaoEntidades = DBConsultaMercado.CastSeleccaoEntidadesViewToDB(data);

                string _Email = string.Empty;
                try
                {
                    _Email = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == data.CodFornecedor).First().Email;
                }
                catch
                {
                    _Email = string.Empty;
                }

                seleccaoEntidades.EmailFornecedor = _Email;

                //Verificar se a Data de Receção de Proposta é diferente da existente na BD
                SeleccaoEntidades verificacao_Data = DBConsultaMercado.GetSeleccaoEntidadesID(data.IdSeleccaoEntidades);

                if (verificacao_Data.DataRecepcaoProposta != data.DataRecepcaoProposta)
                {
                    seleccaoEntidades.DataRecepcaoProposta = DateTime.Now;
                    seleccaoEntidades.UtilizadorRecepcaoProposta = User.Identity.Name;
                }

                var dbUpdateResult = DBConsultaMercado.Update(seleccaoEntidades);

                if (dbUpdateResult != null)
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
        public JsonResult UpdateLinhaSeleccaoEntidade_DataRecepcaoProposta([FromBody] SeleccaoEntidadesView data)
        {
            bool result = false;
            try
            {
                SeleccaoEntidades seleccaoEntidades = DBConsultaMercado.CastSeleccaoEntidadesViewToDB(data);

                string _Email = string.Empty;
                try
                {
                    _Email = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(x => x.No_ == data.CodFornecedor).First().Email;
                }
                catch
                {
                    _Email = string.Empty;
                }

                seleccaoEntidades.EmailFornecedor = _Email;
                seleccaoEntidades.DataRecepcaoProposta = DateTime.Parse(data.DataRecepcaoProposta_Show);
                seleccaoEntidades.UtilizadorRecepcaoProposta = User.Identity.Name;

                var dbUpdateResult = DBConsultaMercado.Update(seleccaoEntidades);

                if (dbUpdateResult != null)
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
        public JsonResult DeleteLinhaSeleccaoEntidade([FromBody] SeleccaoEntidades data)
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

        #region Registo de Proposta

        [HttpPost]
        public JsonResult UpdateLinhaRegistoProposta([FromBody] RegistoDePropostasView data)
        {
            bool result = false;
            try
            {
                RegistoDePropostas registoDePropostas = DBConsultaMercado.CastRegistoDePropostasViewToDB(data);

                var dbUpdateResult = DBConsultaMercado.Update(registoDePropostas);

                if (dbUpdateResult != null)
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

        #region ANEXOS

        [HttpPost]
        [Route("ConsultaMercado/FileUpload")]
        [Route("ConsultaMercado/FileUpload/{id}")]
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
                        string filename = Path.GetFileName(file.FileName);
                        //full_filename = id + "_" + filename;
                        full_filename = filename;
                        var path = Path.Combine(_generalConfig.FileUploadFolder, full_filename);
                        using (FileStream dd = new FileStream(path, FileMode.CreateNew))
                        {
                            file.CopyTo(dd);
                            dd.Dispose();

                            Anexos newfile = new Anexos();
                            newfile.NºOrigem = id;
                            newfile.UrlAnexo = full_filename;

                            //TipoOrigem: 1-PréRequisição; 2-Requisição; 3-Contratos; 4-Procedimentos;5-ConsultaMercado
                            newfile.TipoOrigem = 5;

                            newfile.DataHoraCriação = DateTime.Now;
                            newfile.UtilizadorCriação = User.Identity.Name;

                            DBAttachments.Create(newfile);
                            if (newfile.NºLinha == 0)
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json("");
        }

        [HttpPost]
        public JsonResult LoadAttachments([FromBody] JObject requestParams)
        {
            string id = requestParams["id"].ToString();

            List<Anexos> list = DBAttachments.GetById(id);
            List<AttachmentsViewModel> attach = new List<AttachmentsViewModel>();
            list.ForEach(x => attach.Add(DBAttachments.ParseToViewModel(x)));
            return Json(attach);
        }

        [HttpGet]
        public FileStreamResult DownloadFile(string id)
        {
            return new FileStreamResult(new FileStream(_generalConfig.FileUploadFolder + id, FileMode.Open), "application/xlsx");
        }

        [HttpPost]
        public JsonResult DeleteAttachments([FromBody] AttachmentsViewModel requestParams)
        {
            try
            {
                System.IO.File.Delete(_generalConfig.FileUploadFolder + requestParams.Url);
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
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
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
                if (dp["numEncomenda"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Encomenda"); Col = Col + 1; }

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
                        if (dp["numEncomenda"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NumEncomenda == null ? string.Empty : item.NumEncomenda.ToString()); Col = Col + 1; }
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