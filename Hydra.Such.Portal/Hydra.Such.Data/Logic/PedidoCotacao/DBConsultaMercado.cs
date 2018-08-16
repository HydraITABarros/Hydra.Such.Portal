using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.PedidoCotacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Hydra.Such.Data.Logic.PedidoCotacao
{
    public class DBConsultaMercado
    {
        #region Actividades

        public static List<Actividades> GetAllActividadesToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Actividades.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ActividadesView CastActividadesToView(Actividades ObjectToTransform)
        {
            Actividades actividades = new Actividades();

            try
            {
                using (var ctx = new SuchDBContext())
                {
                    actividades = ctx.Actividades.Where(p => p.CodActividade == ObjectToTransform.CodActividade).FirstOrDefault();
                }
            }
            catch (Exception e)
            {

            }

            ActividadesView view = new ActividadesView()
            {
                CodActividade = ObjectToTransform.CodActividade,
                Descricao = ObjectToTransform.Descricao
            };

            return view;
        }

        public static Actividades GetDetalheActividades(string CodActividade)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Actividades.Where(p => p.CodActividade == CodActividade).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Actividades Create(Actividades ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Actividades.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Actividades Update(Actividades ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Actividades.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Actividades Delete(Actividades ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Actividades.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return ObjectToDelete;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        #endregion


        #region Actividades_por_Fornecedor

        public static List<ActividadesPorFornecedor> GetAllActividadesPorFornecedorToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ActividadesPorFornecedor.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ActividadesPorFornecedorView CastActividadesPorFornecedorToView(ActividadesPorFornecedor ObjectToTransform)
        {
            ActividadesPorFornecedor actividadesPorFornecedor = new ActividadesPorFornecedor();

            try
            {
                using (var ctx = new SuchDBContext())
                {
                    actividadesPorFornecedor = ctx.ActividadesPorFornecedor.Where(p => p.Id == ObjectToTransform.Id).FirstOrDefault();
                }
            }
            catch (Exception e)
            {

            }

            ActividadesPorFornecedorView view = new ActividadesPorFornecedorView()
            {
                Id = ObjectToTransform.Id,
                CodActividade = ObjectToTransform.CodActividade,
                CodFornecedor = ObjectToTransform.CodFornecedor
            };

            return view;
        }

        public static ActividadesPorFornecedor GetDetalheActividadesPorFornecedor(string Id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ActividadesPorFornecedor.Where(p => p.Id == int.Parse(Id)).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ActividadesPorFornecedor Create(ActividadesPorFornecedor ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ActividadesPorFornecedor.Add(ObjectToCreate);
                    ctx.SaveChanges();

                    ObjectToCreate = ctx.ActividadesPorFornecedor.Where(p => p.CodActividade == ObjectToCreate.CodActividade).Where(p => p.CodFornecedor == ObjectToCreate.CodFornecedor).LastOrDefault();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ActividadesPorFornecedor Update(ActividadesPorFornecedor ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ActividadesPorFornecedor.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ActividadesPorFornecedor Delete(ActividadesPorFornecedor ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ActividadesPorFornecedor.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return ObjectToDelete;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        #endregion


        #region Actividades_por_Produto

        public static List<ActividadesPorProduto> GetAllActividadesPorProdutoToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ActividadesPorProduto.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ActividadesPorProdutoView CastActividadesPorProdutoToView(ActividadesPorProduto ObjectToTransform)
        {
            ActividadesPorProduto actividadesPorProduto = new ActividadesPorProduto();

            try
            {
                using (var ctx = new SuchDBContext())
                {
                    actividadesPorProduto = ctx.ActividadesPorProduto.Where(p => p.Id == ObjectToTransform.Id).FirstOrDefault();
                }
            }
            catch (Exception e)
            {

            }

            ActividadesPorProdutoView view = new ActividadesPorProdutoView()
            {
                Id = ObjectToTransform.Id,
                CodActividade = ObjectToTransform.CodActividade,
                CodProduto = ObjectToTransform.CodProduto
            };

            return view;
        }

        public static ActividadesPorProduto GetDetalheActividadesPorProduto(string Id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ActividadesPorProduto.Where(p => p.Id == int.Parse(Id)).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ActividadesPorProduto Create(ActividadesPorProduto ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ActividadesPorProduto.Add(ObjectToCreate);
                    ctx.SaveChanges();

                    ObjectToCreate = ctx.ActividadesPorProduto.Where(p => p.CodActividade == ObjectToCreate.CodActividade).Where(p => p.CodProduto == ObjectToCreate.CodProduto).LastOrDefault();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ActividadesPorProduto Update(ActividadesPorProduto ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ActividadesPorProduto.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ActividadesPorProduto Delete(ActividadesPorProduto ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ActividadesPorProduto.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return ObjectToDelete;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        #endregion


        #region Consulta_Mercado

        public static List<ConsultaMercado> GetAllConsultaMercadoToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConsultaMercado.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ConsultaMercado GetDetalheConsultaMercado(string NumConsultaMercado)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConsultaMercado.Where(p => p.NumConsultaMercado == NumConsultaMercado).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ConsultaMercado Create(ConsultaMercado ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.PedidoCotacaoCriadoEm = DateTime.Now;
                    ctx.ConsultaMercado.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ConsultaMercado Update(ConsultaMercado ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConsultaMercado.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConsultaMercado Delete(ConsultaMercado ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConsultaMercado.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return ObjectToDelete;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ConsultaMercadoView> GetAllConsultaMercadoViewToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConsultaMercado.Select(ConsultaMercado => new ConsultaMercadoView()
                    {
                        NumConsultaMercado = ConsultaMercado.NumConsultaMercado,
                        CodProjecto = ConsultaMercado.CodProjecto,
                        Descricao = ConsultaMercado.Descricao,
                        CodRegiao = ConsultaMercado.CodRegiao,
                        CodAreaFuncional = ConsultaMercado.CodAreaFuncional,
                        CodCentroResponsabilidade = ConsultaMercado.CodCentroResponsabilidade,
                        CodActividade = ConsultaMercado.CodActividade,
                        //DataPedidoCotacao = ConsultaMercado.DataPedidoCotacao == null ? default(DateTime) : Convert.ToDateTime(ConsultaMercado.DataPedidoCotacao.Value.ToString("yyyy-MM-dd")),
                        DataPedidoCotacao = ConsultaMercado.DataPedidoCotacao,
                        FornecedorSelecionado = ConsultaMercado.FornecedorSelecionado,
                        NumDocumentoCompra = ConsultaMercado.NumDocumentoCompra,
                        CodLocalizacao = ConsultaMercado.CodLocalizacao,
                        FiltroActividade = ConsultaMercado.FiltroActividade,
                        ValorPedidoCotacao = ConsultaMercado.ValorPedidoCotacao,
                        Destino = ConsultaMercado.Destino,
                        Estado = ConsultaMercado.Estado,
                        UtilizadorRequisicao = ConsultaMercado.UtilizadorRequisicao,
                        DataLimite = ConsultaMercado.DataLimite,
                        EspecificacaoTecnica = ConsultaMercado.EspecificacaoTecnica,
                        Fase = ConsultaMercado.Fase,
                        Modalidade = ConsultaMercado.Modalidade,
                        PedidoCotacaoCriadoEm = ConsultaMercado.PedidoCotacaoCriadoEm,
                        PedidoCotacaoCriadoPor = ConsultaMercado.PedidoCotacaoCriadoPor,
                        ConsultaEm = ConsultaMercado.ConsultaEm,
                        ConsultaPor = ConsultaMercado.ConsultaPor,
                        NegociacaoContratacaoEm = ConsultaMercado.NegociacaoContratacaoEm,
                        NegociacaoContratacaoPor = ConsultaMercado.NegociacaoContratacaoPor,
                        AdjudicacaoEm = ConsultaMercado.AdjudicacaoEm,
                        AdjudicacaoPor = ConsultaMercado.AdjudicacaoPor,
                        NumRequisicao = ConsultaMercado.NumRequisicao,
                        PedidoCotacaoOrigem = ConsultaMercado.PedidoCotacaoOrigem,
                        ValorAdjudicado = ConsultaMercado.ValorAdjudicado,
                        CodFormaPagamento = ConsultaMercado.CodFormaPagamento,
                        SeleccaoEfectuada = ConsultaMercado.SeleccaoEfectuada,
                        Destino_Show = ConsultaMercado.Destino == 1 ? "Armazém" : ConsultaMercado.Destino == 2 ? "Projeto" : string.Empty,
                        Estado_Show = ConsultaMercado.Estado == 0 ? "Aberto" : ConsultaMercado.Estado == 1 ? "Liberto" : string.Empty,
                        Fase_Show = ConsultaMercado.Fase == 0 ? "Abertura" : ConsultaMercado.Fase == 1 ? "Consulta" : ConsultaMercado.Fase == 2 ? "Negociação e Contratação" : ConsultaMercado.Fase == 3 ? "Adjudicação" : ConsultaMercado.Fase == 4 ? "Fecho" : string.Empty,
                        Modalidade_Show = ConsultaMercado.Modalidade == 0 ? "Consulta Alargada" : ConsultaMercado.Modalidade == 1 ? "Ajuste Direto" : string.Empty,
                        DataPedidoCotacao_Show = ConsultaMercado.DataPedidoCotacao == null ? "" : ConsultaMercado.DataPedidoCotacao.Value.ToString("yyyy-MM-dd"),
                        DataLimite_Show = ConsultaMercado.DataLimite == null ? "" : ConsultaMercado.DataLimite.Value.ToString("yyyy-MM-dd"),
                        PedidoCotacaoCriadoEm_Show = ConsultaMercado.PedidoCotacaoCriadoEm == null ? "" : ConsultaMercado.PedidoCotacaoCriadoEm.Value.ToString("yyyy-MM-dd"),
                        ConsultaEm_Show = ConsultaMercado.ConsultaEm == null ? "" : ConsultaMercado.ConsultaEm.Value.ToString("yyyy-MM-dd"),
                        NegociacaoContratacaoEm_Show = ConsultaMercado.NegociacaoContratacaoEm == null ? "" : ConsultaMercado.NegociacaoContratacaoEm.Value.ToString("yyyy-MM-dd"),
                        AdjudicacaoEm_Show = ConsultaMercado.AdjudicacaoEm == null ? "" : ConsultaMercado.AdjudicacaoEm.Value.ToString("yyyy-MM-dd")
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ConsultaMercadoView CastConsultaMercadoToView(ConsultaMercado ObjectToTransform)
        {
            ConsultaMercado consultaMercado = new ConsultaMercado();
            List<LinhasConsultaMercado> linhasConsultaMercado = new List<LinhasConsultaMercado>();
            List<CondicoesPropostasFornecedores> condicoesPropostasFornecedores = new List<CondicoesPropostasFornecedores>();
            List<LinhasCondicoesPropostasFornecedores> linhasCondicoesPropostasFornecedores = new List<LinhasCondicoesPropostasFornecedores>();
            List<SeleccaoEntidades> seleccaoEntidades = new List<SeleccaoEntidades>();
            //HistoricoConsultaMercado historicoConsultaMercado = new HistoricoConsultaMercado();
            string historicoConsultaMercado = string.Empty;

            try
            {
                using (var ctx = new SuchDBContext())
                {
                    consultaMercado = ctx.ConsultaMercado.Where(p => p.NumConsultaMercado == ObjectToTransform.NumConsultaMercado).FirstOrDefault();
                    linhasConsultaMercado = ctx.LinhasConsultaMercado.Where(p => p.NumConsultaMercado == consultaMercado.NumConsultaMercado).ToList();
                    condicoesPropostasFornecedores = ctx.CondicoesPropostasFornecedores.Where(p => p.NumConsultaMercado == consultaMercado.NumConsultaMercado).ToList();
                    linhasCondicoesPropostasFornecedores = ctx.LinhasCondicoesPropostasFornecedores.Where(p => p.NumConsultaMercado == consultaMercado.NumConsultaMercado).ToList();
                    seleccaoEntidades = ctx.SeleccaoEntidades.Where(p => p.NumConsultaMercado == consultaMercado.NumConsultaMercado).ToList();

                    historicoConsultaMercado = ctx.HistoricoConsultaMercado.Where(p => p.NumConsultaMercado == consultaMercado.NumConsultaMercado).Max(p => p.NumVersao).ToString();
                }
            }
            catch (Exception e)
            {

            }

            ConsultaMercadoView view = new ConsultaMercadoView()
            {
                NumConsultaMercado = ObjectToTransform.NumConsultaMercado,
                CodProjecto = ObjectToTransform.CodProjecto,
                Descricao = ObjectToTransform.Descricao,
                CodRegiao = ObjectToTransform.CodRegiao,
                CodAreaFuncional = ObjectToTransform.CodAreaFuncional,
                CodCentroResponsabilidade = ObjectToTransform.CodCentroResponsabilidade,
                CodActividade = ObjectToTransform.CodActividade,
                //DataPedidoCotacao = ObjectToTransform.DataPedidoCotacao == null ? default(DateTime) : Convert.ToDateTime(ObjectToTransform.DataPedidoCotacao.Value.ToString("yyyy-MM-dd")),
                DataPedidoCotacao = ObjectToTransform.DataPedidoCotacao,
                FornecedorSelecionado = ObjectToTransform.FornecedorSelecionado,
                NumDocumentoCompra = ObjectToTransform.NumDocumentoCompra,
                CodLocalizacao = ObjectToTransform.CodLocalizacao,
                FiltroActividade = ObjectToTransform.FiltroActividade,
                ValorPedidoCotacao = ObjectToTransform.ValorPedidoCotacao,
                Destino = ObjectToTransform.Destino,
                Estado = ObjectToTransform.Estado,
                UtilizadorRequisicao = ObjectToTransform.UtilizadorRequisicao,
                DataLimite = ObjectToTransform.DataLimite,
                EspecificacaoTecnica = ObjectToTransform.EspecificacaoTecnica,
                Fase = ObjectToTransform.Fase,
                Modalidade = ObjectToTransform.Modalidade,
                PedidoCotacaoCriadoEm = ObjectToTransform.PedidoCotacaoCriadoEm,
                PedidoCotacaoCriadoPor = ObjectToTransform.PedidoCotacaoCriadoPor,
                ConsultaEm = ObjectToTransform.ConsultaEm,
                ConsultaPor = ObjectToTransform.ConsultaPor,
                NegociacaoContratacaoEm = ObjectToTransform.NegociacaoContratacaoEm,
                NegociacaoContratacaoPor = ObjectToTransform.NegociacaoContratacaoPor,
                AdjudicacaoEm = ObjectToTransform.AdjudicacaoEm,
                AdjudicacaoPor = ObjectToTransform.AdjudicacaoPor,
                NumRequisicao = ObjectToTransform.NumRequisicao,
                PedidoCotacaoOrigem = ObjectToTransform.PedidoCotacaoOrigem,
                ValorAdjudicado = ObjectToTransform.ValorAdjudicado,
                CodFormaPagamento = ObjectToTransform.CodFormaPagamento,
                SeleccaoEfectuada = ObjectToTransform.SeleccaoEfectuada,
                Destino_Show = ObjectToTransform.Destino == 1 ? "Armazém" : ObjectToTransform.Destino == 2 ? "Projeto" : string.Empty,
                Estado_Show = ObjectToTransform.Estado == 0 ? "Aberto" : ObjectToTransform.Estado == 1 ? "Liberto" : string.Empty,
                Fase_Show = ObjectToTransform.Fase == 0 ? "Abertura" : ObjectToTransform.Fase == 1 ? "Consulta" : ObjectToTransform.Fase == 2 ? "Negociação e Contratação" : ObjectToTransform.Fase == 3 ? "Adjudicação" : ObjectToTransform.Fase == 4 ? "Fecho" : string.Empty,
                Modalidade_Show = ObjectToTransform.Modalidade == 0 ? "Consulta Alargada" : ObjectToTransform.Modalidade == 1 ? "Ajuste Direto" : string.Empty,
                NumVersoesArquivadas_CalcField = historicoConsultaMercado == null ? "0" : historicoConsultaMercado == string.Empty ? "0" : historicoConsultaMercado,
                DataPedidoCotacao_Show = ObjectToTransform.DataPedidoCotacao == null ? "" : ObjectToTransform.DataPedidoCotacao.Value.ToString("yyyy-MM-dd"),
                DataLimite_Show = ObjectToTransform.DataLimite == null ? "" : ObjectToTransform.DataLimite.Value.ToString("yyyy-MM-dd"),
                PedidoCotacaoCriadoEm_Show = ObjectToTransform.PedidoCotacaoCriadoEm == null ? "" : ObjectToTransform.PedidoCotacaoCriadoEm.Value.ToString("yyyy-MM-dd"),
                ConsultaEm_Show = ObjectToTransform.ConsultaEm == null ? "" : ObjectToTransform.ConsultaEm.Value.ToString("yyyy-MM-dd"),
                NegociacaoContratacaoEm_Show = ObjectToTransform.NegociacaoContratacaoEm == null ? "" : ObjectToTransform.NegociacaoContratacaoEm.Value.ToString("yyyy-MM-dd"),
                AdjudicacaoEm_Show = ObjectToTransform.AdjudicacaoEm == null ? "" : ObjectToTransform.AdjudicacaoEm.Value.ToString("yyyy-MM-dd")
            };

            if (linhasConsultaMercado != null && linhasConsultaMercado.Count > 0)
            {
                List<LinhasConsultaMercadoView> linhasConsultaMercadoList = new List<LinhasConsultaMercadoView>();
                foreach (var lcm in linhasConsultaMercado)
                {
                    linhasConsultaMercadoList.Add(CastLinhasConsultaMercadoToView(lcm));
                }

                view.LinhasConsultaMercado = linhasConsultaMercadoList;
            }

            if (condicoesPropostasFornecedores != null && condicoesPropostasFornecedores.Count > 0)
            {
                List<CondicoesPropostasFornecedoresView> CondicoesPropostasFornecedoresList = new List<CondicoesPropostasFornecedoresView>();
                foreach (var cpf in condicoesPropostasFornecedores)
                {
                    CondicoesPropostasFornecedoresList.Add(CastCondicoesPropostasFornecedoresToView(cpf));
                }

                view.CondicoesPropostasFornecedores = CondicoesPropostasFornecedoresList;
            }

            if (linhasCondicoesPropostasFornecedores != null && linhasCondicoesPropostasFornecedores.Count > 0)
            {
                List<LinhasCondicoesPropostasFornecedoresView> LinhasCondicoesPropostasFornecedoresList = new List<LinhasCondicoesPropostasFornecedoresView>();
                foreach (var lcpf in linhasCondicoesPropostasFornecedores)
                {
                    LinhasCondicoesPropostasFornecedoresList.Add(CastLinhasCondicoesPropostasFornecedoresToView(lcpf));
                }

                view.LinhasCondicoesPropostasFornecedores = LinhasCondicoesPropostasFornecedoresList;
            }

            if (seleccaoEntidades != null && seleccaoEntidades.Count > 0)
            {
                List<SeleccaoEntidadesView> seleccaoEntidadesList = new List<SeleccaoEntidadesView>();
                foreach (var se in seleccaoEntidades)
                {
                    seleccaoEntidadesList.Add(CastSeleccaoEntidadesToView(se));
                }

                view.SeleccaoEntidades = seleccaoEntidadesList;
            }

            return view;
        }

        #endregion


        #region Linhas_Consulta_Mercado

        public static LinhasConsultaMercadoView CastLinhasConsultaMercadoToView(LinhasConsultaMercado ObjectToTransform)
        {
            LinhasConsultaMercado linhasConsultaMercado = new LinhasConsultaMercado();
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    linhasConsultaMercado = ctx.LinhasConsultaMercado.Where(p => p.NumLinha == ObjectToTransform.NumLinha).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                
            }

            LinhasConsultaMercadoView view = new LinhasConsultaMercadoView()
            {
                NumLinha = ObjectToTransform.NumLinha,
                NumConsultaMercado = ObjectToTransform.NumConsultaMercado,
                CodProduto = ObjectToTransform.CodProduto,
                Descricao = ObjectToTransform.Descricao,
                NumProjecto = ObjectToTransform.NumProjecto,
                CodRegiao = ObjectToTransform.CodRegiao,
                CodAreaFuncional = ObjectToTransform.CodAreaFuncional,
                CodCentroResponsabilidade = ObjectToTransform.CodCentroResponsabilidade,
                CodActividade = ObjectToTransform.CodActividade,
                CodLocalizacao = ObjectToTransform.CodLocalizacao,
                Quantidade = ObjectToTransform.Quantidade,
                CustoUnitarioPrevisto = ObjectToTransform.CustoUnitarioPrevisto,
                CustoTotalPrevisto = ObjectToTransform.CustoTotalPrevisto,
                CustoUnitarioObjectivo = ObjectToTransform.CustoUnitarioObjectivo,
                CustoTotalObjectivo = ObjectToTransform.CustoTotalObjectivo,
                CodUnidadeMedida = ObjectToTransform.CodUnidadeMedida,
                DataEntregaPrevista = ObjectToTransform.DataEntregaPrevista,
                NumRequisicao = ObjectToTransform.NumRequisicao,
                LinhaRequisicao = ObjectToTransform.LinhaRequisicao,
                CriadoEm = ObjectToTransform.CriadoEm,
                CriadoPor = ObjectToTransform.CriadoPor,
                ModificadoEm = ObjectToTransform.ModificadoEm,
                ModificadoPor = ObjectToTransform.ModificadoPor,
                DataEntregaPrevista_Show = ObjectToTransform.DataEntregaPrevista == null ? "" : ObjectToTransform.DataEntregaPrevista.Value.ToString("yyyy-MM-dd")
            };

            return view;
        }

        #endregion


        #region Condicoes_Propostas_Fornecedores

        public static CondicoesPropostasFornecedoresView CastCondicoesPropostasFornecedoresToView(CondicoesPropostasFornecedores ObjectToTransform)
        {
            CondicoesPropostasFornecedores condicoesPropostasFornecedores = new CondicoesPropostasFornecedores();
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    condicoesPropostasFornecedores = ctx.CondicoesPropostasFornecedores.Where(p => p.IdCondicaoPropostaFornecedores == ObjectToTransform.IdCondicaoPropostaFornecedores).FirstOrDefault();
                }
            }
            catch (Exception e)
            {

            }

            CondicoesPropostasFornecedoresView view = new CondicoesPropostasFornecedoresView()
            {
                IdCondicaoPropostaFornecedores = ObjectToTransform.IdCondicaoPropostaFornecedores,
                NumConsultaMercado = ObjectToTransform.NumConsultaMercado,
                CodFornecedor = ObjectToTransform.CodFornecedor,
                Alternativa = ObjectToTransform.Alternativa,
                CodActividade = ObjectToTransform.CodActividade,
                ValidadeProposta = ObjectToTransform.ValidadeProposta,
                CodTermosPagamento = ObjectToTransform.CodTermosPagamento,
                CodFormaPagamento = ObjectToTransform.CodFormaPagamento,
                FornecedorSelecionado = ObjectToTransform.FornecedorSelecionado,
                DataProposta = ObjectToTransform.DataProposta,
                NumProjecto = ObjectToTransform.NumProjecto,
                Preferencial = ObjectToTransform.Preferencial,
                NomeFornecedor = ObjectToTransform.NomeFornecedor,
                EnviarPedidoProposta = ObjectToTransform.EnviarPedidoProposta,
                Notificado = ObjectToTransform.Notificado
            };

            return view;
        }

        #endregion


        #region Linhas_Condicoes_Propostas_Fornecedores

        public static LinhasCondicoesPropostasFornecedoresView CastLinhasCondicoesPropostasFornecedoresToView(LinhasCondicoesPropostasFornecedores ObjectToTransform)
        {
            LinhasCondicoesPropostasFornecedores linhasCondicoesPropostasFornecedores = new LinhasCondicoesPropostasFornecedores();
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    linhasCondicoesPropostasFornecedores = ctx.LinhasCondicoesPropostasFornecedores.Where(p => p.NumLinha == ObjectToTransform.NumLinha).FirstOrDefault();
                }
            }
            catch (Exception e)
            {

            }

            LinhasCondicoesPropostasFornecedoresView view = new LinhasCondicoesPropostasFornecedoresView()
            {
                NumLinha = ObjectToTransform.NumLinha,
                NumConsultaMercado = ObjectToTransform.NumConsultaMercado,
                CodFornecedor = ObjectToTransform.CodFornecedor,
                Alternativa = ObjectToTransform.Alternativa,
                CodProduto = ObjectToTransform.CodProduto,
                Quantidade = ObjectToTransform.Quantidade,
                NumProjecto = ObjectToTransform.NumProjecto,
                CodLocalizacao = ObjectToTransform.CodLocalizacao,
                PrecoFornecedor = ObjectToTransform.PrecoFornecedor,
                DataEntregaPrevista = ObjectToTransform.DataEntregaPrevista,
                Validade = ObjectToTransform.Validade,
                QuantidadeAAdjudicar = ObjectToTransform.QuantidadeAAdjudicar,
                MotivoRejeicao = ObjectToTransform.MotivoRejeicao,
                QuantidadeAdjudicada = ObjectToTransform.QuantidadeAdjudicada,
                QuantidadeAEncomendar = ObjectToTransform.QuantidadeAEncomendar,
                QuantidadeEncomendada = ObjectToTransform.QuantidadeEncomendada,
                CodUnidadeMedida = ObjectToTransform.CodUnidadeMedida,
                PrazoEntrega = ObjectToTransform.PrazoEntrega,
                CodActividade = ObjectToTransform.CodActividade,
                PercentagemDescontoLinha = ObjectToTransform.PercentagemDescontoLinha,
                ValorAdjudicadoDl = ObjectToTransform.ValorAdjudicadoDl,
                EstadoRespostaFornecedor = ObjectToTransform.EstadoRespostaFornecedor,
                DataEntregaPrometida = ObjectToTransform.DataEntregaPrometida,
                RespostaFornecedor = ObjectToTransform.RespostaFornecedor,
                QuantidadeRespondida = ObjectToTransform.QuantidadeRespondida
            };

            return view;
        }

        #endregion


        #region Seleccao_Entidades

        public static SeleccaoEntidadesView CastSeleccaoEntidadesToView(SeleccaoEntidades ObjectToTransform)
        {
            SeleccaoEntidades seleccaoEntidades = new SeleccaoEntidades();
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    seleccaoEntidades = ctx.SeleccaoEntidades.Where(p => p.IdSeleccaoEntidades == ObjectToTransform.IdSeleccaoEntidades).FirstOrDefault();
                }
            }
            catch (Exception e)
            {

            }

            SeleccaoEntidadesView view = new SeleccaoEntidadesView()
            {
                IdSeleccaoEntidades = ObjectToTransform.IdSeleccaoEntidades,
                NumConsultaMercado = ObjectToTransform.NumConsultaMercado,
                CodFornecedor = ObjectToTransform.CodFornecedor,
                NomeFornecedor = ObjectToTransform.NomeFornecedor,
                CodActividade = ObjectToTransform.CodActividade,
                CidadeFornecedor = ObjectToTransform.CidadeFornecedor,
                CodTermosPagamento = ObjectToTransform.CodTermosPagamento,
                CodFormaPagamento = ObjectToTransform.CodFormaPagamento,
                Selecionado = ObjectToTransform.Selecionado,
                Preferencial = ObjectToTransform.Preferencial
            };

            return view;
        }

        #endregion


        #region Historico_Consulta_Mercado

        #endregion


        #region Historico_Linhas_Consulta_Mercado

        #endregion


        #region Historico_Condicoes_Propostas_Fornecedores

        #endregion


        #region Historico_Linhas_Condicoes_Propostas_Fornecedores

        #endregion


        #region Historico_Seleccao_Entidades

        #endregion

    }
}
