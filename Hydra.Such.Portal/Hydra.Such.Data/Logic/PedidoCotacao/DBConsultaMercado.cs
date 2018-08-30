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
                    ConsultaMercado consultaMercado = ctx.ConsultaMercado.Where(p => p.NumConsultaMercado == NumConsultaMercado).FirstOrDefault();
                    List<LinhasConsultaMercado> linhasConsultaMercado = new List<LinhasConsultaMercado>();
                    List<CondicoesPropostasFornecedores> condicoesPropostasFornecedores = new List<CondicoesPropostasFornecedores>();
                    List<LinhasCondicoesPropostasFornecedores> linhasCondicoesPropostasFornecedores = new List<LinhasCondicoesPropostasFornecedores>();
                    List<SeleccaoEntidades> seleccaoEntidades = new List<SeleccaoEntidades>();

                    linhasConsultaMercado = ctx.LinhasConsultaMercado.Where(p => p.NumConsultaMercado == consultaMercado.NumConsultaMercado).ToList();
                    condicoesPropostasFornecedores = ctx.CondicoesPropostasFornecedores.Where(p => p.NumConsultaMercado == consultaMercado.NumConsultaMercado).ToList();
                    linhasCondicoesPropostasFornecedores = ctx.LinhasCondicoesPropostasFornecedores.Where(p => p.NumConsultaMercado == consultaMercado.NumConsultaMercado).ToList();
                    seleccaoEntidades = ctx.SeleccaoEntidades.Where(p => p.NumConsultaMercado == consultaMercado.NumConsultaMercado).ToList();

                    foreach (LinhasConsultaMercado lin in linhasConsultaMercado)
                    {
                        consultaMercado.LinhasConsultaMercado.Add(lin);
                    }

                    foreach (CondicoesPropostasFornecedores lin in condicoesPropostasFornecedores)
                    {
                        consultaMercado.CondicoesPropostasFornecedores.Add(lin);
                    }

                    foreach (LinhasCondicoesPropostasFornecedores lin in linhasCondicoesPropostasFornecedores)
                    {
                        consultaMercado.LinhasCondicoesPropostasFornecedores.Add(lin);
                    }

                    foreach (SeleccaoEntidades lin in seleccaoEntidades)
                    {
                        consultaMercado.SeleccaoEntidades.Add(lin);
                    }


                    return consultaMercado;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //public static ConsultaMercado Create(ConsultaMercado ObjectToCreate)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            ObjectToCreate.PedidoCotacaoCriadoEm = DateTime.Now;
        //            ctx.ConsultaMercado.Add(ObjectToCreate);
        //            ctx.SaveChanges();
        //        }

        //        return ObjectToCreate;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        public static ConsultaMercado Create(string UserID)
        {
            try
            {
                Configuração config = DBConfigurations.GetById(1);

                ConsultaMercado consultaMercado = new ConsultaMercado()
                {
                    NumConsultaMercado = DBNumerationConfigurations.GetNextNumeration(config.ConsultaMercado.Value, true, false),
                    PedidoCotacaoCriadoPor = UserID,
                    PedidoCotacaoCriadoEm = DateTime.Now
                };

                using (var ctx = new SuchDBContext())
                {
                    ctx.ConsultaMercado.Add(consultaMercado);
                    ctx.SaveChanges();
                }


                ConfiguraçãoNumerações ConfigNum = DBNumerationConfigurations.GetById(config.ConsultaMercado.Value);
                ConfigNum.ÚltimoNºUsado = consultaMercado.NumConsultaMercado;
                ConfigNum.UtilizadorModificação = UserID;
                DBNumerationConfigurations.Update(ConfigNum);

                return consultaMercado;
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

        public static ConsultaMercado Update(ConsultaMercadoView consultaMercadoView)
        {
            SuchDBContext _context = new SuchDBContext();
            try
            {
                ConsultaMercado consultaMercado = CastConsultaMercadoViewToConsultaMercado(consultaMercadoView);

                _context.ConsultaMercado.Update(consultaMercado);
                _context.SaveChanges();
                
                return consultaMercado;
            }
            catch (Exception e)
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

        public static bool Delete(string NumConsultaMercado)
        {
            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.LinhasConsultaMercado.RemoveRange(_context.LinhasConsultaMercado.Where(lcm => lcm.NumConsultaMercado == NumConsultaMercado));
                _context.CondicoesPropostasFornecedores.RemoveRange(_context.CondicoesPropostasFornecedores.Where(cpf => cpf.NumConsultaMercado == NumConsultaMercado));
                _context.LinhasCondicoesPropostasFornecedores.RemoveRange(_context.LinhasCondicoesPropostasFornecedores.Where(lcpf => lcpf.NumConsultaMercado == NumConsultaMercado));
                _context.SeleccaoEntidades.RemoveRange(_context.SeleccaoEntidades.Where(se => se.NumConsultaMercado == NumConsultaMercado));
                _context.ConsultaMercado.RemoveRange(_context.ConsultaMercado.Where(cm => cm.NumConsultaMercado == NumConsultaMercado));

                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
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

        public static ConsultaMercado CastConsultaMercadoViewToConsultaMercado(ConsultaMercadoView ObjectToTransform)
        {
            ConsultaMercado consultaMercado = new ConsultaMercado()
            {
                NumConsultaMercado = ObjectToTransform.NumConsultaMercado,
                CodProjecto = ObjectToTransform.CodProjecto,
                Descricao = ObjectToTransform.Descricao,
                CodRegiao = ObjectToTransform.CodRegiao,
                CodAreaFuncional = ObjectToTransform.CodAreaFuncional,
                CodCentroResponsabilidade = ObjectToTransform.CodCentroResponsabilidade,
                CodActividade = ObjectToTransform.CodActividade,
                //DataPedidoCotacao = ObjectToTransform.DataPedidoCotacao,
                DataPedidoCotacao = ObjectToTransform.DataPedidoCotacao_Show != string.Empty ? DateTime.Parse(ObjectToTransform.DataPedidoCotacao_Show) : (DateTime?)null,
                FornecedorSelecionado = ObjectToTransform.FornecedorSelecionado,
                NumDocumentoCompra = ObjectToTransform.NumDocumentoCompra,
                CodLocalizacao = ObjectToTransform.CodLocalizacao,
                FiltroActividade = ObjectToTransform.FiltroActividade,
                ValorPedidoCotacao = ObjectToTransform.ValorPedidoCotacao,
                Destino = ObjectToTransform.Destino,
                Estado = ObjectToTransform.Estado,
                UtilizadorRequisicao = ObjectToTransform.UtilizadorRequisicao,
                //DataLimite = ObjectToTransform.DataLimite,
                DataLimite = ObjectToTransform.DataLimite_Show != string.Empty ? DateTime.Parse(ObjectToTransform.DataLimite_Show) : (DateTime?)null,
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
                SeleccaoEfectuada = ObjectToTransform.SeleccaoEfectuada
            };

            //Falta o cast das icollections


            return (consultaMercado);

        }

        #endregion


        #region Linhas_Consulta_Mercado


        public static LinhasConsultaMercado Create(LinhasConsultaMercado ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.CriadoEm = DateTime.Now;
                    ctx.LinhasConsultaMercado.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static LinhasConsultaMercado Create_Copia(LinhasConsultaMercadoView ObjectToCreate, string NumConsultaMercado, string UserID)
        {
            try
            {
                LinhasConsultaMercado linhasConsultaMercado = CastLinhasConsultaMercadoViewToDB(ObjectToCreate);

                using (var ctx = new SuchDBContext())
                {
                    linhasConsultaMercado.CriadoEm = DateTime.Now;
                    linhasConsultaMercado.CriadoPor = UserID;
                    linhasConsultaMercado.NumConsultaMercado = NumConsultaMercado;

                    ctx.LinhasConsultaMercado.Add(linhasConsultaMercado);
                    ctx.SaveChanges();
                }

                return linhasConsultaMercado;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static LinhasConsultaMercado Update(LinhasConsultaMercado ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasConsultaMercado.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static LinhasConsultaMercado Delete(LinhasConsultaMercado ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasConsultaMercado.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return ObjectToDelete;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

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

        public static LinhasConsultaMercado CastLinhasConsultaMercadoViewToDB(LinhasConsultaMercadoView ObjectToTransform)
        {
            LinhasConsultaMercado linhasConsultaMercado = new LinhasConsultaMercado()
            {
                CodActividade = ObjectToTransform.CodActividade,
                CodAreaFuncional = ObjectToTransform.CodAreaFuncional,
                CodCentroResponsabilidade = ObjectToTransform.CodCentroResponsabilidade,
                CodLocalizacao = ObjectToTransform.CodLocalizacao,
                CodProduto = ObjectToTransform.CodProduto,
                CodRegiao = ObjectToTransform.CodRegiao,
                CodUnidadeMedida = ObjectToTransform.CodUnidadeMedida,
                CriadoEm = ObjectToTransform.CriadoEm,
                CriadoPor = ObjectToTransform.CriadoPor,
                CustoTotalObjectivo = ObjectToTransform.CustoTotalObjectivo,
                CustoTotalPrevisto = ObjectToTransform.CustoTotalPrevisto,
                CustoUnitarioObjectivo = ObjectToTransform.CustoUnitarioObjectivo,
                CustoUnitarioPrevisto = ObjectToTransform.CustoUnitarioPrevisto,
                DataEntregaPrevista = ObjectToTransform.DataEntregaPrevista,
                Descricao = ObjectToTransform.Descricao,
                LinhaRequisicao = ObjectToTransform.LinhaRequisicao,
                ModificadoEm = ObjectToTransform.ModificadoEm,
                ModificadoPor = ObjectToTransform.ModificadoPor,
                NumConsultaMercado = ObjectToTransform.NumConsultaMercado,
                //NumLinha = ObjectToTransform.NumLinha,
                NumProjecto = ObjectToTransform.NumProjecto,
                NumRequisicao = ObjectToTransform.NumRequisicao,
                Quantidade = ObjectToTransform.Quantidade
            };

            return linhasConsultaMercado;
        }

        #endregion


        #region Condicoes_Propostas_Fornecedores


        public static string Get_MAX_Alternativa_CondicoesPropostasFornecedores(string NumConsultaMercado, string CodFornecedor)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.CondicoesPropostasFornecedores.Where(p => p.NumConsultaMercado == NumConsultaMercado).Where(p => p.CodFornecedor == CodFornecedor).Max(p => p.Alternativa);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public static CondicoesPropostasFornecedores Create(SeleccaoEntidades seleccaoEntidades, string _alternativa)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    CondicoesPropostasFornecedores condicoesPropostasFornecedores = new CondicoesPropostasFornecedores()
                    {
                        Alternativa = _alternativa,
                        CodActividade = seleccaoEntidades.CodActividade,
                        CodFormaPagamento = seleccaoEntidades.CodFormaPagamento,
                        CodFornecedor = seleccaoEntidades.CodFornecedor,
                        CodTermosPagamento = seleccaoEntidades.CodTermosPagamento,
                        DataProposta = DateTime.Now,
                        NomeFornecedor = seleccaoEntidades.NomeFornecedor,
                        NumConsultaMercado = seleccaoEntidades.NumConsultaMercado,
                        NumProjecto = seleccaoEntidades.NumConsultaMercadoNavigation.CodProjecto,
                        Preferencial = seleccaoEntidades.Preferencial
                    };

                    ctx.CondicoesPropostasFornecedores.Add(condicoesPropostasFornecedores);
                    ctx.SaveChanges();

                    return condicoesPropostasFornecedores;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }



        public static CondicoesPropostasFornecedores Create_Copia(CondicoesPropostasFornecedoresView ObjectToCreate, string NumConsultaMercado, string UserID)
        {
            try
            {
                CondicoesPropostasFornecedores condicoesPropostasFornecedores = CastCondicoesPropostasFornecedoresViewToDB(ObjectToCreate);

                using (var ctx = new SuchDBContext())
                {
                    condicoesPropostasFornecedores.NumConsultaMercado = NumConsultaMercado;

                    ctx.CondicoesPropostasFornecedores.Add(condicoesPropostasFornecedores);
                    ctx.SaveChanges();
                }

                return condicoesPropostasFornecedores;
            }
            catch (Exception e)
            {
                return null;
            }
        }

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

        public static CondicoesPropostasFornecedores CastCondicoesPropostasFornecedoresViewToDB(CondicoesPropostasFornecedoresView ObjectToTransform)
        {
            CondicoesPropostasFornecedores condicoesPropostasFornecedores = new CondicoesPropostasFornecedores()
            {
                Alternativa = ObjectToTransform.Alternativa,
                CodActividade = ObjectToTransform.CodActividade,
                CodFormaPagamento = ObjectToTransform.CodFormaPagamento,
                CodFornecedor = ObjectToTransform.CodFornecedor,
                CodTermosPagamento = ObjectToTransform.CodTermosPagamento,
                DataProposta = ObjectToTransform.DataProposta,
                EnviarPedidoProposta = ObjectToTransform.EnviarPedidoProposta,
                FornecedorSelecionado = ObjectToTransform.FornecedorSelecionado,
                NomeFornecedor = ObjectToTransform.NomeFornecedor,
                Notificado = ObjectToTransform.Notificado,
                NumConsultaMercado = ObjectToTransform.NumConsultaMercado,
                NumProjecto = ObjectToTransform.NumProjecto,
                Preferencial = ObjectToTransform.Preferencial,
                ValidadeProposta = ObjectToTransform.ValidadeProposta
            };

            return condicoesPropostasFornecedores;
        }

        #endregion


        #region Linhas_Condicoes_Propostas_Fornecedores


        public static LinhasCondicoesPropostasFornecedores Create(LinhasConsultaMercado linhasConsultaMercado, string _alternativa, string _codFornecedor)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    LinhasCondicoesPropostasFornecedores linhasCondicoesPropostasFornecedores = new LinhasCondicoesPropostasFornecedores()
                    {
                        Alternativa = _alternativa,
                        CodActividade = linhasConsultaMercado.CodActividade,
                        CodFornecedor = _codFornecedor,
                        CodLocalizacao = linhasConsultaMercado.CodLocalizacao,
                        CodProduto = linhasConsultaMercado.CodProduto,
                        CodUnidadeMedida = linhasConsultaMercado.CodUnidadeMedida,
                        DataEntregaPrevista = linhasConsultaMercado.DataEntregaPrevista,
                        EstadoRespostaFornecedor = 0,   //0-Pendente; 1-Aprovado; 2-Rejeitado
                        MotivoRejeicao = string.Empty,
                        NumConsultaMercado = linhasConsultaMercado.NumConsultaMercado,
                        NumProjecto = linhasConsultaMercado.NumProjecto,
                        PrazoEntrega = string.Empty,
                        Quantidade = linhasConsultaMercado.Quantidade,
                        Validade = string.Empty
                    };

                    ctx.LinhasCondicoesPropostasFornecedores.Add(linhasCondicoesPropostasFornecedores);
                    ctx.SaveChanges();

                    return linhasCondicoesPropostasFornecedores;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public static LinhasCondicoesPropostasFornecedores Create_Copia(LinhasCondicoesPropostasFornecedoresView ObjectToCreate, string NumConsultaMercado, string UserID)
        {
            try
            {
                LinhasCondicoesPropostasFornecedores linhasCondicoesPropostasFornecedores = CastLinhasCondicoesPropostasFornecedoresViewToDB(ObjectToCreate);

                using (var ctx = new SuchDBContext())
                {
                    linhasCondicoesPropostasFornecedores.NumConsultaMercado = NumConsultaMercado;
                    
                    ctx.LinhasCondicoesPropostasFornecedores.Add(linhasCondicoesPropostasFornecedores);
                    ctx.SaveChanges();
                }

                return linhasCondicoesPropostasFornecedores;
            }
            catch (Exception e)
            {
                return null;
            }
        }

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

        public static LinhasCondicoesPropostasFornecedores CastLinhasCondicoesPropostasFornecedoresViewToDB(LinhasCondicoesPropostasFornecedoresView ObjectToTransform)
        {
            LinhasCondicoesPropostasFornecedores linhasCondicoesPropostasFornecedores = new LinhasCondicoesPropostasFornecedores()
            {
                Alternativa = ObjectToTransform.Alternativa,
                CodActividade = ObjectToTransform.CodActividade,
                CodFornecedor = ObjectToTransform.CodFornecedor,
                CodLocalizacao = ObjectToTransform.CodLocalizacao,
                CodProduto = ObjectToTransform.CodProduto,
                CodUnidadeMedida = ObjectToTransform.CodUnidadeMedida,
                DataEntregaPrevista = ObjectToTransform.DataEntregaPrevista,
                DataEntregaPrometida = ObjectToTransform.DataEntregaPrometida,
                EstadoRespostaFornecedor = ObjectToTransform.EstadoRespostaFornecedor,
                MotivoRejeicao = ObjectToTransform.MotivoRejeicao,
                NumConsultaMercado = ObjectToTransform.NumConsultaMercado,
                //NumLinha = ObjectToTransform.NumLinha,
                NumProjecto = ObjectToTransform.NumProjecto,
                PercentagemDescontoLinha = ObjectToTransform.PercentagemDescontoLinha,
                PrazoEntrega = ObjectToTransform.PrazoEntrega,
                PrecoFornecedor = ObjectToTransform.PrecoFornecedor,
                Quantidade = ObjectToTransform.Quantidade,
                QuantidadeAAdjudicar = ObjectToTransform.QuantidadeAAdjudicar,
                QuantidadeAdjudicada = ObjectToTransform.QuantidadeAdjudicada,
                QuantidadeAEncomendar = ObjectToTransform.QuantidadeAEncomendar,
                QuantidadeEncomendada = ObjectToTransform.QuantidadeEncomendada,
                QuantidadeRespondida = ObjectToTransform.QuantidadeRespondida,
                RespostaFornecedor = ObjectToTransform.RespostaFornecedor,
                Validade = ObjectToTransform.Validade,
                ValorAdjudicadoDl = ObjectToTransform.ValorAdjudicadoDl
            };

            return linhasCondicoesPropostasFornecedores;
        }

        #endregion


        #region Seleccao_Entidades


        public static SeleccaoEntidades Create(SeleccaoEntidades ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.SeleccaoEntidades.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static SeleccaoEntidades Create_Copia(SeleccaoEntidadesView ObjectToCreate, string NumConsultaMercado, string UserID)
        {
            try
            {
                SeleccaoEntidades seleccaoEntidades = CastSeleccaoEntidadesViewToDB(ObjectToCreate);

                using (var ctx = new SuchDBContext())
                {
                    seleccaoEntidades.NumConsultaMercado = NumConsultaMercado;

                    ctx.SeleccaoEntidades.Add(seleccaoEntidades);
                    ctx.SaveChanges();
                }

                return seleccaoEntidades;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static SeleccaoEntidades Delete(SeleccaoEntidades ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.SeleccaoEntidades.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return ObjectToDelete;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

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

        public static SeleccaoEntidades CastSeleccaoEntidadesViewToDB(SeleccaoEntidadesView ObjectToTransform)
        {
            SeleccaoEntidades seleccaoEntidades = new SeleccaoEntidades()
            {
                CidadeFornecedor = ObjectToTransform.CidadeFornecedor,
                CodActividade = ObjectToTransform.CodActividade,
                CodFormaPagamento = ObjectToTransform.CodFormaPagamento,
                CodFornecedor = ObjectToTransform.CodFornecedor,
                CodTermosPagamento = ObjectToTransform.CodTermosPagamento,
                NomeFornecedor = ObjectToTransform.NomeFornecedor,
                NumConsultaMercado = ObjectToTransform.NumConsultaMercado,
                Preferencial = ObjectToTransform.Preferencial,
                Selecionado = ObjectToTransform.Selecionado
            };

            return seleccaoEntidades;
        }

        #endregion


        #region Historico_Consulta_Mercado

        public static HistoricoConsultaMercado Create(ConsultaMercado ObjectToCreate)
        {
            int _MaxVersao = 0;

            try
            {
                using (var ctx = new SuchDBContext())
                {
                    _MaxVersao = ctx.HistoricoConsultaMercado.Where(p => p.NumConsultaMercado == ObjectToCreate.NumConsultaMercado).Max(p => p.NumVersao);
                }

            }
            catch (Exception ex)
            {
                _MaxVersao = 0;
            }
            

            HistoricoConsultaMercado historicoConsultaMercado = new HistoricoConsultaMercado()
            {
                NumConsultaMercado = ObjectToCreate.NumConsultaMercado,
                NumVersao = (_MaxVersao + 1),
                CodProjecto = ObjectToCreate.CodProjecto,
                Descricao = ObjectToCreate.Descricao,
                CodRegiao = ObjectToCreate.CodRegiao,
                CodAreaFuncional = ObjectToCreate.CodAreaFuncional,
                CodCentroResponsabilidade = ObjectToCreate.CodCentroResponsabilidade,
                CodActividade = ObjectToCreate.CodActividade,
                DataPedidoCotacao = ObjectToCreate.DataPedidoCotacao,
                FornecedorSelecionado = ObjectToCreate.FornecedorSelecionado,
                NumDocumentoCompra = ObjectToCreate.NumDocumentoCompra,
                CodLocalizacao = ObjectToCreate.CodLocalizacao,
                FiltroActividade = ObjectToCreate.FiltroActividade,
                ValorPedidoCotacao = ObjectToCreate.ValorPedidoCotacao,
                Destino = ObjectToCreate.Destino,
                Estado = ObjectToCreate.Estado,
                UtilizadorRequisicao = ObjectToCreate.UtilizadorRequisicao,
                DataLimite = ObjectToCreate.DataLimite,
                EspecificacaoTecnica = ObjectToCreate.EspecificacaoTecnica,
                Fase = ObjectToCreate.Fase,
                Modalidade = ObjectToCreate.Modalidade,
                PedidoCotacaoCriadoEm = ObjectToCreate.PedidoCotacaoCriadoEm,
                PedidoCotacaoCriadoPor = ObjectToCreate.PedidoCotacaoCriadoPor,
                ConsultaEm = ObjectToCreate.ConsultaEm,
                ConsultaPor = ObjectToCreate.ConsultaPor,
                NegociacaoContratacaoEm = ObjectToCreate.NegociacaoContratacaoEm,
                NegociacaoContratacaoPor = ObjectToCreate.NegociacaoContratacaoPor,
                AdjudicacaoEm = ObjectToCreate.AdjudicacaoEm,
                AdjudicacaoPor = ObjectToCreate.AdjudicacaoPor,
                NumRequisicao = ObjectToCreate.NumRequisicao,
                PedidoCotacaoOrigem = ObjectToCreate.PedidoCotacaoOrigem,
                ValorAdjudicado = ObjectToCreate.ValorAdjudicado,
                CodFormaPagamento = ObjectToCreate.CodFormaPagamento,
                SeleccaoEfectuada = ObjectToCreate.SeleccaoEfectuada
            };

            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.HistoricoConsultaMercado.Add(historicoConsultaMercado);

                _context.SaveChanges();
                return historicoConsultaMercado;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion


        #region Historico_Linhas_Consulta_Mercado

        public static HistoricoLinhasConsultaMercado Create_Hist(LinhasConsultaMercado ObjectToCreate, int numversao)
        {
            HistoricoLinhasConsultaMercado historicoLinhasConsultaMercado = new HistoricoLinhasConsultaMercado()
            {
                NumLinha = ObjectToCreate.NumLinha,
                NumConsultaMercado = ObjectToCreate.NumConsultaMercado,
                NumVersao = numversao,
                CodProduto = ObjectToCreate.CodProduto,
                Descricao = ObjectToCreate.Descricao,
                NumProjecto = ObjectToCreate.NumProjecto,
                CodRegiao = ObjectToCreate.CodRegiao,
                CodAreaFuncional = ObjectToCreate.CodAreaFuncional,
                CodCentroResponsabilidade = ObjectToCreate.CodCentroResponsabilidade,
                CodActividade = ObjectToCreate.CodActividade,
                CodLocalizacao = ObjectToCreate.CodLocalizacao,
                Quantidade = ObjectToCreate.Quantidade,
                CustoUnitarioPrevisto = ObjectToCreate.CustoUnitarioPrevisto,
                CustoTotalPrevisto = ObjectToCreate.CustoTotalPrevisto,
                CustoUnitarioObjectivo = ObjectToCreate.CustoUnitarioObjectivo,
                CustoTotalObjectivo = ObjectToCreate.CustoTotalObjectivo,
                CodUnidadeMedida = ObjectToCreate.CodUnidadeMedida,
                DataEntregaPrevista = ObjectToCreate.DataEntregaPrevista,
                NumRequisicao = ObjectToCreate.NumRequisicao,
                LinhaRequisicao = ObjectToCreate.LinhaRequisicao,
                CriadoEm = ObjectToCreate.CriadoEm,
                CriadoPor = ObjectToCreate.CriadoPor,
                ModificadoEm = ObjectToCreate.ModificadoEm,
                ModificadoPor = ObjectToCreate.ModificadoPor
            };

            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.HistoricoLinhasConsultaMercado.Add(historicoLinhasConsultaMercado);

                _context.SaveChanges();
                return historicoLinhasConsultaMercado;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion


        #region Historico_Condicoes_Propostas_Fornecedores

        public static HistoricoCondicoesPropostasFornecedores Create_Hist(CondicoesPropostasFornecedores ObjectToCreate, int numversao)
        {
            HistoricoCondicoesPropostasFornecedores historicoCondicoesPropostasFornecedores = new HistoricoCondicoesPropostasFornecedores()
            {
                IdCondicaoPropostaFornecedores = ObjectToCreate.IdCondicaoPropostaFornecedores,
                NumConsultaMercado = ObjectToCreate.NumConsultaMercado,
                NumVersao = numversao,
                CodFornecedor = ObjectToCreate.CodFornecedor,
                Alternativa = ObjectToCreate.Alternativa,
                CodActividade = ObjectToCreate.CodActividade,
                ValidadeProposta = ObjectToCreate.ValidadeProposta,
                CodTermosPagamento = ObjectToCreate.CodTermosPagamento,
                CodFormaPagamento = ObjectToCreate.CodFormaPagamento,
                FornecedorSelecionado = ObjectToCreate.FornecedorSelecionado,
                DataProposta = ObjectToCreate.DataProposta,
                NumProjecto = ObjectToCreate.NumProjecto,
                Preferencial = ObjectToCreate.Preferencial,
                NomeFornecedor = ObjectToCreate.NomeFornecedor,
                EnviarPedidoProposta = ObjectToCreate.EnviarPedidoProposta,
                Notificado = ObjectToCreate.Notificado
            };

            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.HistoricoCondicoesPropostasFornecedores.Add(historicoCondicoesPropostasFornecedores);

                _context.SaveChanges();
                return historicoCondicoesPropostasFornecedores;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion


        #region Historico_Linhas_Condicoes_Propostas_Fornecedores

        public static HistoricoLinhasCondicoesPropostasFornecedores Create_Hist(LinhasCondicoesPropostasFornecedores ObjectToCreate, int numversao)
        {
            HistoricoLinhasCondicoesPropostasFornecedores historicoLinhasCondicoesPropostasFornecedores = new HistoricoLinhasCondicoesPropostasFornecedores()
            {
                NumLinha = ObjectToCreate.NumLinha,
                NumConsultaMercado = ObjectToCreate.NumConsultaMercado,
                NumVersao = numversao,
                CodFornecedor = ObjectToCreate.CodFornecedor,
                Alternativa = ObjectToCreate.Alternativa,
                CodProduto = ObjectToCreate.CodProduto,
                Quantidade = ObjectToCreate.Quantidade,
                NumProjecto = ObjectToCreate.NumProjecto,
                CodLocalizacao = ObjectToCreate.CodLocalizacao,
                PrecoFornecedor = ObjectToCreate.PrecoFornecedor,
                DataEntregaPrevista = ObjectToCreate.DataEntregaPrevista,
                Validade = ObjectToCreate.Validade,
                QuantidadeAAdjudicar = ObjectToCreate.QuantidadeAAdjudicar,
                MotivoRejeicao = ObjectToCreate.MotivoRejeicao,
                QuantidadeAdjudicada = ObjectToCreate.QuantidadeAdjudicada,
                QuantidadeAEncomendar = ObjectToCreate.QuantidadeAEncomendar,
                QuantidadeEncomendada = ObjectToCreate.QuantidadeEncomendada,
                CodUnidadeMedida = ObjectToCreate.CodUnidadeMedida,
                PrazoEntrega = ObjectToCreate.PrazoEntrega,
                CodActividade = ObjectToCreate.CodActividade,
                PercentagemDescontoLinha = ObjectToCreate.PercentagemDescontoLinha,
                ValorAdjudicadoDl = ObjectToCreate.ValorAdjudicadoDl,
                EstadoRespostaFornecedor = ObjectToCreate.EstadoRespostaFornecedor,
                DataEntregaPrometida = ObjectToCreate.DataEntregaPrometida,
                RespostaFornecedor = ObjectToCreate.RespostaFornecedor,
                QuantidadeRespondida = ObjectToCreate.QuantidadeRespondida
            };

            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.HistoricoLinhasCondicoesPropostasFornecedores.Add(historicoLinhasCondicoesPropostasFornecedores);

                _context.SaveChanges();
                return historicoLinhasCondicoesPropostasFornecedores;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion


        #region Historico_Seleccao_Entidades

        public static HistoricoSeleccaoEntidades Create_Hist(SeleccaoEntidades ObjectToCreate, int numversao)
        {
            HistoricoSeleccaoEntidades historicoSeleccaoEntidades = new HistoricoSeleccaoEntidades()
            {
                IdSeleccaoEntidades = ObjectToCreate.IdSeleccaoEntidades,
                NumConsultaMercado = ObjectToCreate.NumConsultaMercado,
                NumVersao = numversao,
                CodFornecedor = ObjectToCreate.CodFornecedor,
                NomeFornecedor = ObjectToCreate.NomeFornecedor,
                CodActividade = ObjectToCreate.CodActividade,
                CidadeFornecedor = ObjectToCreate.CidadeFornecedor,
                CodTermosPagamento = ObjectToCreate.CodTermosPagamento,
                CodFormaPagamento = ObjectToCreate.CodFormaPagamento,
                Selecionado = ObjectToCreate.Selecionado,
                Preferencial = ObjectToCreate.Preferencial
            };

            SuchDBContext _context = new SuchDBContext();

            try
            {
                _context.HistoricoSeleccaoEntidades.Add(historicoSeleccaoEntidades);

                _context.SaveChanges();
                return historicoSeleccaoEntidades;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

    }
}
