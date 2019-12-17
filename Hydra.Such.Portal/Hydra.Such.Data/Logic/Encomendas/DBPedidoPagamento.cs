using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Encomendas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Hydra.Such.Data.NAV;

namespace Hydra.Such.Data.Logic.Encomendas
{
    public static class DBPedidoPagamento
    {
        #region CRUD
        public static List<PedidosPagamento> GetAllPedidosPagamento()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PedidosPagamento.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<PedidosPagamento> GetAllPedidosPagamentoByEncomenda(string Encomenda)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PedidosPagamento.Where(x => x.NoEncomenda == Encomenda).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<PedidosPagamento> GetAllPedidosPagamentoByEstado(int Estado)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PedidosPagamento.Where(x => x.Estado == Estado).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<PedidosPagamento> GetAllPedidosPagamentoByArquivo(bool Arquivado)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PedidosPagamento.Where(x => x.Arquivado == Arquivado).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static PedidosPagamento GetIDPedidosPagamento(int NoPedido)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PedidosPagamento.Where(p => p.NoPedido == NoPedido).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static PedidosPagamento Update(PedidosPagamento ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.PedidosPagamento.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static PedidosPagamento Create(PedidosPagamento ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.PedidosPagamento.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion


        public static PedidosPagamentoViewModel ParseToViewModel(this PedidosPagamento ObjectToTransform)
        {
            if (ObjectToTransform != null)
            {
                return new PedidosPagamentoViewModel()
                {
                    NoPedido = ObjectToTransform.NoPedido,
                    Data = ObjectToTransform.Data,
                    //DataText = ObjectToTransform.Data.HasValue ? Convert.ToDateTime(ObjectToTransform.Data).ToShortDateString() : "",
                    DataText = ObjectToTransform.Data == null ? "" : ObjectToTransform.Data.Value.ToString("yyyy-MM-dd"),
                    Tipo = ObjectToTransform.Tipo,
                    Estado = ObjectToTransform.Estado,
                    EstadoText = ObjectToTransform.Estado.HasValue ? ObjectToTransform.Estado == 1 ? "Inicial" : ObjectToTransform.Estado == 2 ? "Em Aprovação" : ObjectToTransform.Estado == 3 ? "Aprovado" : ObjectToTransform.Estado == 4 ? "Validado" : ObjectToTransform.Estado == 5 ? "Anulado" : ObjectToTransform.Estado == 6 ? "Liquidado" : ObjectToTransform.Estado == 7 ? "Arquivado" : "" : "",
                    Aprovado = ObjectToTransform.Aprovado == null ? false : (bool)ObjectToTransform.Aprovado,
                    AprovadoText = ObjectToTransform.Aprovado.HasValue ? ObjectToTransform.Aprovado == true ? "Sim" : "Não" : "Não",
                    Valor = ObjectToTransform.Valor,
                    NoEncomenda = ObjectToTransform.NoEncomenda,
                    NoRequisicao = ObjectToTransform.NoRequisicao,
                    CodigoFornecedor = ObjectToTransform.CodigoFornecedor,
                    Fornecedor = ObjectToTransform.Fornecedor,
                    NIB = ObjectToTransform.NIB,
                    IBAN = ObjectToTransform.IBAN,
                    DataPedido = ObjectToTransform.DataPedido,
                    DataPedidoText = ObjectToTransform.DataPedido == null ? "" : ObjectToTransform.DataPedido.Value.ToString("yyyy-MM-dd"),
                    UserPedido = ObjectToTransform.UserPedido,
                    UserAprovacao = ObjectToTransform.UserAprovacao,
                    DataAprovacao = ObjectToTransform.DataAprovacao,
                    DataAprovacaoText = ObjectToTransform.DataAprovacao == null ? "" : ObjectToTransform.DataAprovacao.Value.ToString("yyyy-MM-dd"),
                    UserFinanceiros = ObjectToTransform.UserFinanceiros,
                    DataDisponibilizacao = ObjectToTransform.DataDisponibilizacao,
                    DataDisponibilizacaoText = ObjectToTransform.DataDisponibilizacao == null ? "" : ObjectToTransform.DataDisponibilizacao.Value.ToString("yyyy-MM-dd"),
                    Descricao = ObjectToTransform.Descricao,
                    DataEnvioAprovacao = ObjectToTransform.DataEnvioAprovacao,
                    DataEnvioAprovacaoText = ObjectToTransform.DataEnvioAprovacao == null ? "" : ObjectToTransform.DataEnvioAprovacao.Value.ToString("yyyy-MM-dd"),
                    DataValidacao = ObjectToTransform.DataValidacao,
                    DataValidacaoText = ObjectToTransform.DataValidacao == null ? "" : ObjectToTransform.DataValidacao.Value.ToString("yyyy-MM-dd"),
                    UserValidacao = ObjectToTransform.UserValidacao,
                    BloqueadoFaltaPagamento = ObjectToTransform.BloqueadoFaltaPagamento == null ? false : (bool)ObjectToTransform.BloqueadoFaltaPagamento,
                    BloqueadoFaltaPagamentoText = ObjectToTransform.BloqueadoFaltaPagamento.HasValue ? ObjectToTransform.BloqueadoFaltaPagamento == true ? "Sim" : "Não" : "Não",
                    Aprovadores = ObjectToTransform.Aprovadores,
                    ValorEncomenda = ObjectToTransform.ValorEncomenda,
                    UserLiquidado = ObjectToTransform.UserLiquidado,
                    DataLiquidado = ObjectToTransform.DataLiquidado,
                    DataLiquidadoText = ObjectToTransform.DataLiquidado == null ? "" : ObjectToTransform.DataLiquidado.Value.ToString("yyyy-MM-dd"),
                    Arquivado = ObjectToTransform.Arquivado == null ? false : (bool)ObjectToTransform.Arquivado,
                    ArquivadoText = ObjectToTransform.Arquivado.HasValue ? ObjectToTransform.Arquivado == true ? "Sim" : "Não" : "Não",
                    UserArquivo = ObjectToTransform.UserArquivo,
                    DataArquivo = ObjectToTransform.DataArquivo,
                    DataArquivoText = ObjectToTransform.DataArquivo == null ? "" : ObjectToTransform.DataArquivo.Value.ToString("yyyy-MM-dd"),
                    MotivoAnulacao = ObjectToTransform.MotivoAnulacao,
                    Resolvido = ObjectToTransform.Resolvido == null ? false : (bool)ObjectToTransform.Resolvido,
                    ResolvidoText = ObjectToTransform.Resolvido.HasValue ? ObjectToTransform.Resolvido == true ? "Sim" : "Não" : "Não",
                    Prioritario = ObjectToTransform.Prioritario == null ? false : (bool)ObjectToTransform.Prioritario,
                    PrioritarioText = ObjectToTransform.Prioritario.HasValue ? ObjectToTransform.Prioritario == true ? "Sim" : "Não" : "Não",
                    DataPrioridade = ObjectToTransform.DataPrioridade,
                    DataPrioridadeText = ObjectToTransform.DataPrioridade == null ? "" : ObjectToTransform.DataPrioridade.Value.ToString("yyyy-MM-dd"),
                    NumeroTransferencia = ObjectToTransform.NumeroTransferencia,
                    RegiaoMercadoLocal = ObjectToTransform.RegiaoMercadoLocal,
                    RegiaoMercadoLocalText = !string.IsNullOrEmpty(ObjectToTransform.RegiaoMercadoLocal) ? ObjectToTransform.RegiaoMercadoLocal == "2" ? "Lisboa" : ObjectToTransform.RegiaoMercadoLocal == "3" ? "Porto" : ObjectToTransform.RegiaoMercadoLocal == "4" ? "Coimbra" : "" : "",
                    UtilizadorCriacao = ObjectToTransform.UtilizadorCriacao,
                    DataCriacao = ObjectToTransform.DataCriacao,
                    UtilizadorModificacao = ObjectToTransform.UtilizadorModificacao,
                    DataModificacao = ObjectToTransform.DataModificacao
                };
            }

            return null;
        }

        public static List<PedidosPagamentoViewModel> ParseToViewModel(this List<PedidosPagamento> items)
        {
            List<PedidosPagamentoViewModel> parsedItems = new List<PedidosPagamentoViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static PedidosPagamento ParseToDB(this PedidosPagamentoViewModel ObjectToTransform)
        {
            if (ObjectToTransform != null)
            {
                return new PedidosPagamento()
                {
                    NoPedido = ObjectToTransform.NoPedido,
                    Data = ObjectToTransform.Data,
                    Tipo = ObjectToTransform.Tipo,
                    Estado = ObjectToTransform.Estado,
                    Aprovado = ObjectToTransform.Aprovado == null ? false : (bool)ObjectToTransform.Aprovado,
                    Valor = ObjectToTransform.Valor,
                    NoEncomenda = ObjectToTransform.NoEncomenda,
                    NoRequisicao = ObjectToTransform.NoRequisicao,
                    CodigoFornecedor = ObjectToTransform.CodigoFornecedor,
                    Fornecedor = ObjectToTransform.Fornecedor,
                    NIB = ObjectToTransform.NIB,
                    IBAN = ObjectToTransform.IBAN,
                    DataPedido = ObjectToTransform.DataPedido,
                    UserPedido = ObjectToTransform.UserPedido,
                    UserAprovacao = ObjectToTransform.UserAprovacao,
                    DataAprovacao = ObjectToTransform.DataAprovacao,
                    UserFinanceiros = ObjectToTransform.UserFinanceiros,
                    DataDisponibilizacao = ObjectToTransform.DataDisponibilizacao,
                    Descricao = ObjectToTransform.Descricao,
                    DataEnvioAprovacao = ObjectToTransform.DataEnvioAprovacao,
                    DataValidacao = ObjectToTransform.DataValidacao,
                    UserValidacao = ObjectToTransform.UserValidacao,
                    BloqueadoFaltaPagamento = ObjectToTransform.BloqueadoFaltaPagamento == null ? false : (bool)ObjectToTransform.BloqueadoFaltaPagamento,
                    Aprovadores = ObjectToTransform.Aprovadores,
                    ValorEncomenda = ObjectToTransform.ValorEncomenda,
                    UserLiquidado = ObjectToTransform.UserLiquidado,
                    DataLiquidado = ObjectToTransform.DataLiquidado,
                    Arquivado = ObjectToTransform.Arquivado == null ? false : (bool)ObjectToTransform.Arquivado,
                    UserArquivo = ObjectToTransform.UserArquivo,
                    DataArquivo = ObjectToTransform.DataArquivo,
                    MotivoAnulacao = ObjectToTransform.MotivoAnulacao,
                    Resolvido = ObjectToTransform.Resolvido == null ? false : (bool)ObjectToTransform.Resolvido,
                    Prioritario = ObjectToTransform.Prioritario == null ? false : (bool)ObjectToTransform.Prioritario,
                    DataPrioridade = ObjectToTransform.DataPrioridade,
                    NumeroTransferencia = ObjectToTransform.NumeroTransferencia,
                    RegiaoMercadoLocal = ObjectToTransform.RegiaoMercadoLocal,
                    UtilizadorCriacao = ObjectToTransform.UtilizadorCriacao,
                    DataCriacao = ObjectToTransform.DataCriacao,
                    UtilizadorModificacao = ObjectToTransform.UtilizadorModificacao,
                    DataModificacao = ObjectToTransform.DataModificacao
                };
            }

            return null;
        }

        public static List<PedidosPagamento> ParseToDB(this List<PedidosPagamentoViewModel> items)
        {
            List<PedidosPagamento> parsedItems = new List<PedidosPagamento>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
    }
}
