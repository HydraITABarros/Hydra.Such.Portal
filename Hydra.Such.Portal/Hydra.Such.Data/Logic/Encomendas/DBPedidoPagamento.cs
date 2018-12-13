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
                    DataText = ObjectToTransform.Data.HasValue ? Convert.ToDateTime(ObjectToTransform.Data).ToShortDateString() : "",
                    Tipo = ObjectToTransform.Tipo,
                    Estado = ObjectToTransform.Estado,
                    Aprovado = ObjectToTransform.Aprovado == null ? false : (bool)ObjectToTransform.Aprovado,
                    AprovadoText = ObjectToTransform.Aprovado.HasValue ? ObjectToTransform.Aprovado == true ? "Sim" : "Não" : "Não",
                    Valor = ObjectToTransform.Valor,
                    NoEncomenda = ObjectToTransform.NoEncomenda,
                    CodigoFornecedor = ObjectToTransform.CodigoFornecedor,
                    Fornecedor = ObjectToTransform.Fornecedor,
                    NIB = ObjectToTransform.NIB,
                    IBAN = ObjectToTransform.IBAN,
                    DataPedido = ObjectToTransform.DataPedido,
                    DataPedidoText = ObjectToTransform.DataPedido.HasValue ? Convert.ToDateTime(ObjectToTransform.DataPedido).ToShortDateString() : "",
                    UserPedido = ObjectToTransform.UserPedido,
                    UserAprovacao = ObjectToTransform.UserAprovacao,
                    DataAprovacao = ObjectToTransform.DataAprovacao,
                    DataAprovacaoText = ObjectToTransform.DataAprovacao.HasValue ? Convert.ToDateTime(ObjectToTransform.DataAprovacao).ToShortDateString() : "",
                    UserFinanceiros = ObjectToTransform.UserFinanceiros,
                    DataDisponibilizacao = ObjectToTransform.DataDisponibilizacao,
                    DataDisponibilizacaoText = ObjectToTransform.DataDisponibilizacao.HasValue ? Convert.ToDateTime(ObjectToTransform.DataDisponibilizacao).ToShortDateString() : "",
                    Descricao = ObjectToTransform.Descricao,
                    DataEnvioAprovacao = ObjectToTransform.DataEnvioAprovacao,
                    DataEnvioAprovacaoText = ObjectToTransform.DataEnvioAprovacao.HasValue ? Convert.ToDateTime(ObjectToTransform.DataEnvioAprovacao).ToShortDateString() : "",
                    DataValidacao = ObjectToTransform.DataValidacao,
                    DataValidacaoText = ObjectToTransform.DataValidacao.HasValue ? Convert.ToDateTime(ObjectToTransform.DataValidacao).ToShortDateString() : "",
                    UserValidacao = ObjectToTransform.UserValidacao,
                    BloqueadoFaltaPagamento = ObjectToTransform.BloqueadoFaltaPagamento == null ? false : (bool)ObjectToTransform.BloqueadoFaltaPagamento,
                    BloqueadoFaltaPagamentoText = ObjectToTransform.BloqueadoFaltaPagamento.HasValue ? ObjectToTransform.BloqueadoFaltaPagamento == true ? "Sim" : "Não" : "Não",
                    Aprovadores = ObjectToTransform.Aprovadores,
                    ValorEncomenda = ObjectToTransform.ValorEncomenda,
                    Arquivado = ObjectToTransform.Arquivado == null ? false : (bool)ObjectToTransform.Arquivado,
                    ArquivadoText = ObjectToTransform.Arquivado.HasValue ? ObjectToTransform.Arquivado == true ? "Sim" : "Não" : "Não",
                    UserArquivo = ObjectToTransform.UserArquivo,
                    DataArquivo = ObjectToTransform.DataArquivo,
                    DataArquivoText = ObjectToTransform.DataArquivo.HasValue ? Convert.ToDateTime(ObjectToTransform.DataArquivo).ToShortDateString() : "",
                    MotivoAnulacao = ObjectToTransform.MotivoAnulacao,
                    Resolvido = ObjectToTransform.Resolvido == null ? false : (bool)ObjectToTransform.Resolvido,
                    ResolvidoText = ObjectToTransform.Resolvido.HasValue ? ObjectToTransform.Resolvido == true ? "Sim" : "Não" : "Não",
                    Prioritario = ObjectToTransform.Prioritario == null ? false : (bool)ObjectToTransform.Prioritario,
                    PrioritarioText = ObjectToTransform.Prioritario.HasValue ? ObjectToTransform.Prioritario == true ? "Sim" : "Não" : "Não",
                    DataPrioridade = ObjectToTransform.DataPrioridade,
                    DataPrioridadeText = ObjectToTransform.DataPrioridade.HasValue ? Convert.ToDateTime(ObjectToTransform.DataPrioridade).ToShortDateString() : "",
                    NumeroTransferencia = ObjectToTransform.NumeroTransferencia,
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
                    Arquivado = ObjectToTransform.Arquivado == null ? false : (bool)ObjectToTransform.Arquivado,
                    UserArquivo = ObjectToTransform.UserArquivo,
                    DataArquivo = ObjectToTransform.DataArquivo,
                    MotivoAnulacao = ObjectToTransform.MotivoAnulacao,
                    Resolvido = ObjectToTransform.Resolvido == null ? false : (bool)ObjectToTransform.Resolvido,
                    Prioritario = ObjectToTransform.Prioritario == null ? false : (bool)ObjectToTransform.Prioritario,
                    DataPrioridade = ObjectToTransform.DataPrioridade,
                    NumeroTransferencia = ObjectToTransform.NumeroTransferencia,
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
