using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Academia;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Such.Data.Logic
{
    /// <summary>
    /// 14-07-2020
    /// Classe responsável pelos CRUD relacionados com Pedidos de Participação Em Formação (PPF)
    /// 
    /// /!\/!\/!\/!\
    ///     Os métodos deverão ser chamados com os campos que permitem avaliar os dados de criação/modificação, dos objectos, já preenchidos.
    /// /!\/!\/!\/!\
    /// </summary>
    public static class DBAcademia
    {
        #region Creates
        public static bool __CriarPedidoFormacao(PedidoParticipacaoFormacao pedidoFormacao, string userName)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    _ctx.PedidoParticipacaoFormacao.Add(pedidoFormacao);
                    _ctx.SaveChanges();
                }

                if(__CriarRegistoAlteracaoPedidoFormacao(pedidoFormacao, 0, "Criação", userName))
                {

                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        public static bool __CriarRegistoAlteracaoPedidoFormacao(PedidoParticipacaoFormacao pedidoFormacao, int tipoAlteracao, string descricao, string usernaName)
        {
            int lineNo = 10;

            if (pedidoFormacao == null)
                return false;

            if (pedidoFormacao.RegistosAlteracoes == null)
            {
                pedidoFormacao.RegistosAlteracoes = __GetRegistosAlteracaoPedido(pedidoFormacao.IdPedido);
            }

            if (pedidoFormacao.RegistosAlteracoes != null)
            {
                RegistoAlteracoesPedidoFormacao log = pedidoFormacao.RegistosAlteracoes.OrderBy(r => r.IdPedidoFormacao).ThenBy(r => r.IdRegisto).LastOrDefault();
                lineNo += log.IdRegisto.Value;
            }

            RegistoAlteracoesPedidoFormacao changeLog = new RegistoAlteracoesPedidoFormacao()
            {
                IdPedidoFormacao = pedidoFormacao.IdPedido,
                IdRegisto = lineNo,
                TipoAlteracao = tipoAlteracao,
                DescricaoAlteracao = descricao,
                UtilizadorAlteracao = usernaName,
                DataHoraAlteracao = DateTime.Now
            };


            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    _ctx.RegistoAlteracoesPedidoFormacao.Add(changeLog);
                    _ctx.SaveChanges();
                }

                pedidoFormacao.RegistosAlteracoes.Add(changeLog);
               

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }   
        #endregion

        #region Reads
        public static List<AccaoFormacao> __GetAllAccoesFormacao()
        {
            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    List<AccaoFormacao> trainingCourses = _ctx.AccaoFormacao.ToList();

                    foreach (var a in trainingCourses)
                    {
                        a.SessoesFormacao = _ctx.SessaoAccaoFormacao.Where(t => t.IdAccao == a.IdAccao).ToList();
                    }

                    return trainingCourses;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static List<AccaoFormacao> __GetAllAccoesFormacao(DateTime aposData)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    List<AccaoFormacao> accoesFormacao = _ctx.AccaoFormacao.Where(a => a.DataInicio > aposData).ToList();
                    
                    foreach(var a in accoesFormacao)
                    {
                        a.SessoesFormacao = _ctx.SessaoAccaoFormacao.Where(t => t.IdAccao == a.IdAccao).ToList();
                    }

                    return accoesFormacao;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static AccaoFormacao __GetDetalhesAccaoFormacao(string accaoId)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    AccaoFormacao accao = _ctx.AccaoFormacao.Where(a => a.IdAccao == accaoId).LastOrDefault();

                    accao.SessoesFormacao = _ctx.SessaoAccaoFormacao.Where(s => s.IdAccao == accaoId).ToList();

                    if (!string.IsNullOrEmpty(accao.IdEntidadeFormadora))
                    {
                        accao.EntidadeNavigation = _ctx.EntidadeFormadora.LastOrDefault();
                    }

                    return accao;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public static List<PedidoParticipacaoFormacao> __GetAllPedidosFormacao()
        {
            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    return _ctx.PedidoParticipacaoFormacao.ToList();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PedidoParticipacaoFormacao __GetDetalhesPedidoFormacao(string pedidoId)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    PedidoParticipacaoFormacao pedido = _ctx.PedidoParticipacaoFormacao.Where(p => p.IdPedido == pedidoId).LastOrDefault();

                    pedido.RegistosAlteracoes = __GetRegistosAlteracaoPedido(pedidoId);

                    pedido.AccaoNavigation = __GetDetalhesAccaoFormacao(pedido.IdAccaoFormacao);
                    
                    return pedido;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<RegistoAlteracoesPedidoFormacao> __GetRegistosAlteracaoPedido(string pedidoId)
        {
            if (string.IsNullOrEmpty(pedidoId))
                return null;

            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    return _ctx.RegistoAlteracoesPedidoFormacao.Where(r => r.IdPedidoFormacao == pedidoId).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion

        #region Updates
        public static bool __UpdatePedidoFormacao(PedidoParticipacaoFormacao pedido)
        {
            if (pedido == null)
                return false;

            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    _ctx.PedidoParticipacaoFormacao.Update(pedido);
                    _ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

            
        }

        public static bool __UpdatePedidoFormacao(PedidoParticipacaoFormacao pedido, int tipoAlteracao, string descricaoAlteracao, string userName)
        {
            if (pedido == null)
                return false;

            if(pedido.RegistosAlteracoes == null)
            {
                pedido.RegistosAlteracoes = __GetRegistosAlteracaoPedido(pedido.IdPedido);
            }

            if (pedido.RegistosAlteracoes == null)
                return false;

            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    _ctx.PedidoParticipacaoFormacao.Update(pedido);
                    _ctx.SaveChanges();
                }


                __CriarRegistoAlteracaoPedidoFormacao(pedido, tipoAlteracao, descricaoAlteracao, userName);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        #endregion

        #region Deletes
        public static bool __DeletePedidoFormacao(string pedidoId)
        {
            using(var _ctx = new SuchDBContext())
            {
                PedidoParticipacaoFormacao pedido = _ctx.PedidoParticipacaoFormacao.Where(p => p.IdPedido == pedidoId).LastOrDefault();
                if(pedido != null)
                {
                    pedido.RegistosAlteracoes = _ctx.RegistoAlteracoesPedidoFormacao.Where(r => r.IdPedidoFormacao == pedidoId).ToList();

                    if(pedido.RegistosAlteracoes != null)
                    {
                        _ctx.RegistoAlteracoesPedidoFormacao.RemoveRange(pedido.RegistosAlteracoes);
                    }

                    _ctx.PedidoParticipacaoFormacao.Remove(pedido);
                    _ctx.SaveChanges();

                }
            }
            return false;
        }
        #endregion

    }
}
