using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Encomendas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Hydra.Such.Data.NAV;

namespace Hydra.Such.Data.Logic.Encomendas
{
    public class DBPedidoPagamento
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

        #endregion


        public static PedidosPagamentoViewModel CastLinhasPreEncomendaToView(PedidosPagamento ObjectToTransform)
        {
            string _fornecedor = string.Empty;

            PedidosPagamentoViewModel Pedido = new PedidosPagamentoViewModel()
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

            return Pedido;
        }

        public static PedidosPagamento CastLinhasPreEncomendaToDB(PedidosPagamentoViewModel ObjectToTransform)
        {
            string _fornecedor = string.Empty;

            PedidosPagamento Pedido = new PedidosPagamento()
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

            return Pedido;
        }

    }
}
