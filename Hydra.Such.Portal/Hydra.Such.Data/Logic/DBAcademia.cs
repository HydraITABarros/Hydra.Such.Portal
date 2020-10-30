using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Academia;
using Hydra.Such.Data.Logic.Approvals;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Such.Data.Logic
{
    public class Formando : NAVEmployeeViewModel
    {
        public string RegiaoNav2017 { get; set; }
        public string AreaNav2017 { get; set; } 
        public string CrespNav2017 { get; set; }
        public string Projecto { get; set; }
        public string Funcao { get; set; }
        public string CodEstabelecimento { get; set; }
        public string DescricaoEstabelecimento { get; set; }
    }

    public class ConfiguracaoAprovacaoAcademia : ConfiguraçãoAprovações
    {
        public string IdUtilizador { get; set; }
        public Enumerations.TipoUtilizadorFluxoPedidoFormacao TipoUtilizadorConfiguracao { get; set; }

        public ConfiguracaoAprovacaoAcademia(string userName, ConfiguraçãoAprovações config)
        {
            IdUtilizador = userName;
            if (config.NívelAprovação == 1)
                TipoUtilizadorConfiguracao = Enumerations.TipoUtilizadorFluxoPedidoFormacao.AprovadorChefia;

            if (config.NívelAprovação == 2)
                TipoUtilizadorConfiguracao = Enumerations.TipoUtilizadorFluxoPedidoFormacao.AprovadorDireccao;

            if (config.NívelAprovação != 1 && config.NívelAprovação != 2)
                TipoUtilizadorConfiguracao = Enumerations.TipoUtilizadorFluxoPedidoFormacao.Formando;

            Id = config.Id;
            Tipo = config.Tipo;
            Área = config.Área;
            CódigoÁreaFuncional = config.CódigoÁreaFuncional;
            CódigoRegião = config.CódigoRegião;
            CódigoCentroResponsabilidade = config.CódigoCentroResponsabilidade;
            NívelAprovação = config.NívelAprovação;
            UtilizadorAprovação = config.UtilizadorAprovação;
            GrupoAprovação = config.GrupoAprovação;
        }
    }

    public class ConfiguracaoAprovacaoUtilizador
    {
        public string IdUtilizador { get; set; }
        public Enumerations.TipoUtilizadorFluxoPedidoFormacao TipoUtilizadorGlobal { get; set; }
        public List<string> AreasChefia { get; set; }
        public List<string> CRespChefia { get; set; }
        public List<string> AreasDirige { get; set; }
        public List<ConfiguracaoAprovacaoAcademia> ConfiguracaoAprovAcademia { get; set; }

        public ConfiguracaoAprovacaoUtilizador(string userName, int globalUserType, int type)
        {
            IdUtilizador = userName;
            try
            {
                TipoUtilizadorGlobal = (Enumerations.TipoUtilizadorFluxoPedidoFormacao)globalUserType;
            }
            catch (Exception)
            {

                TipoUtilizadorGlobal = Enumerations.TipoUtilizadorFluxoPedidoFormacao.Formando;
            }
            

            ConfiguracaoAprovAcademia = new List<ConfiguracaoAprovacaoAcademia>();

            AreasChefia = new List<string>();
            CRespChefia = new List<string>();
            AreasDirige = new List<string>();

            List<UtilizadoresGruposAprovação> grupos = DBApprovalUserGroup.GetByUser(userName);
            List<ConfiguraçãoAprovações> configAprovadores = DBApprovalConfigurations.GetAllByType(type);

           if(configAprovadores != null && configAprovadores.Count > 0)
            {
                foreach(var c in configAprovadores)
                {
                    if(grupos.Where(g => g.GrupoAprovação == c.GrupoAprovação).FirstOrDefault() != null)
                    {
                        if(c.NívelAprovação == 1 || c.NívelAprovação == 2)
                        {
                            ConfiguracaoAprovAcademia.Add(new ConfiguracaoAprovacaoAcademia(userName, c));
                            if (c.NívelAprovação.Value == 1)
                            {
                                if (!string.IsNullOrWhiteSpace(c.CódigoÁreaFuncional))
                                    AreasChefia.Add(c.CódigoÁreaFuncional);

                                if (!string.IsNullOrWhiteSpace(c.CódigoCentroResponsabilidade))
                                    CRespChefia.Add(c.CódigoCentroResponsabilidade);
                            }

                            if (c.NívelAprovação.Value == 2)
                            {
                                if (!string.IsNullOrWhiteSpace(c.CódigoÁreaFuncional))
                                    AreasDirige.Add(c.CódigoÁreaFuncional);
                            }
                        }                                              
                        
                    }
                }

                AreasChefia = AreasChefia != null && AreasChefia.Count > 0 ? AreasChefia.Distinct().ToList() : null;
                CRespChefia = CRespChefia != null && CRespChefia.Count > 0 ? CRespChefia.Distinct().ToList() : null;
                AreasDirige = AreasDirige != null && AreasDirige.Count > 0 ? AreasDirige.Distinct().ToList() : null;
            }
        }

        public bool IsChief()
        {
            // só é Chefia se tiver Área(s) e CResp(s) preenchido
            return ConfiguracaoAprovAcademia.Where(
                    c => c.TipoUtilizadorConfiguracao == Enumerations.TipoUtilizadorFluxoPedidoFormacao.AprovadorChefia).FirstOrDefault() == null ||
                    (AreasChefia == null && CRespChefia == null) ? false : true;
        }

        public bool IsDirector()
        {
            return ConfiguracaoAprovAcademia.Where(
                    c => c.TipoUtilizadorConfiguracao == Enumerations.TipoUtilizadorFluxoPedidoFormacao.AprovadorDireccao).FirstOrDefault() == null ||
                    AreasDirige == null ? false : true;
        }
    }   
    

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
        public static List<AccaoFormacao> __GetAllAccoesFormacao(DateTime aposData)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    return _ctx.AccaoFormacao.Where(a => a.DataInicio > aposData).ToList(); 
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AccaoFormacao> __GetAllAccoesFormacao(string idTema)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    return _ctx.AccaoFormacao.Where(a => a.IdTema == idTema).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<SessaoAccaoFormacao> __GetSessoesFormacao(string idAccao)
        {
            using(var _ctx = new SuchDBContext())
            {
                try
                {
                    return _ctx.SessaoAccaoFormacao.Where(s => s.IdAccao == idAccao).ToList();
                }
                catch (Exception ex)
                {

                    return null;
                }
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
                        accao.EntidadeNavigation = _ctx.EntidadeFormadora.Where(e => e.IdEntidade == accao.IdEntidadeFormadora).LastOrDefault();
                    }

                    return accao;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<TemaFormacao> __GetCatalogo()
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    return _ctx.TemaFormacao.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        /// <summary>
        /// Este método deverá ser utilizado para obter todos os pedidos do utilizador
        /// </summary>
        /// <param name="userName">O nome do utilizdor</param>
        /// <param name="onlyActives">Apenas os pedidos que não estejam no estado Finalizado, ou em Curso</param>
        /// <returns></returns>
        public static List<PedidoParticipacaoFormacao> __GetAllPedidosFormacao(string userName, bool onlyActives)
        {
            if (string.IsNullOrEmpty(userName))
                return null;

            try
            {
                if (onlyActives)
                {
                    // são considerados activos todos os pedidos que não estejam finalizados ou cancelados
                    using(var _ctx = new SuchDBContext())
                    {
                        return _ctx.PedidoParticipacaoFormacao.Where(p => p.UtilizadorCriacao == userName && p.Estado.Value < (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado).ToList();
                    }
                }
                else
                {
                    // todos os pedidos do utilizador que estejam finalizados ou cancelados
                    using (var _ctx = new SuchDBContext())
                    {
                        return _ctx.PedidoParticipacaoFormacao.Where(p => p.UtilizadorCriacao == userName && p.Estado.Value >= (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Este método deverá ser utilizado nos Fluxos de Aprovação: 
        ///     Chefia, 
        ///     Director 
        ///     CA
        /// </summary>
        /// <param name="cfgUser">Detalhes da configuração de aprovação do utilizador, relativamente à Formação</param>
        /// <param name="origin">A origem do pedido do utilizador</param>
        /// <param name="onlyCompleted">Pedidos terminados ou em curso</param>
        /// <returns></returns>
        public static List<PedidoParticipacaoFormacao> __GetAllPedidosFormacao(ConfiguracaoAprovacaoUtilizador cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade origin, bool onlyCompleted)
        {
            try
            {
                string areasDireccao = string.Join(",", cfgUser.AreasDirige);
                string areasChefia = string.Join(",", cfgUser.AreasChefia);
                string cresps = string.Join(",", cfgUser.CRespChefia);

                using(var _ctx = new SuchDBContext())
                {
                    if(origin == Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia && cfgUser.IsChief())
                    {
                        if (onlyCompleted)
                        {
                            // pedidos Finalizados
                            return _ctx.PedidoParticipacaoFormacao.Where(
                                                                p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado &&
                                                                areasChefia.Contains(p.AreaFuncionalEmpregado) &&
                                                                cresps.Contains(p.CentroResponsabilidadeEmpregado)
                                                            ).ToList();
                        }
                        else
                        {
                            // pedidos submetidos
                            return _ctx.PedidoParticipacaoFormacao.Where(
                                                                p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoSubmetido &&
                                                                areasChefia.Contains(p.AreaFuncionalEmpregado) &&
                                                                cresps.Contains(p.CentroResponsabilidadeEmpregado)
                                                            ).ToList();
                        }
                        
                    }

                    if (origin == Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuDirector && cfgUser.IsDirector())
                    {
                        if (onlyCompleted)
                        {
                            // pedidos Finalizados
                            return _ctx.PedidoParticipacaoFormacao.Where(
                                                                p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado &&
                                                                areasDireccao.Contains(p.AreaFuncionalEmpregado)
                                                            ).ToList();
                        }
                        else
                        {
                            // pedidos submetidos
                            return _ctx.PedidoParticipacaoFormacao.Where(
                                                                p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoAprovadoChefia &&
                                                                areasDireccao.Contains(p.AreaFuncionalEmpregado)
                                                            ).ToList();
                        }

                    }

                    if(origin == Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuCA && cfgUser.TipoUtilizadorGlobal == Enumerations.TipoUtilizadorFluxoPedidoFormacao.ConselhoAdministracao)
                    {
                        if (onlyCompleted)
                        {
                            // pedidos Finalizados
                            return _ctx.PedidoParticipacaoFormacao.Where(p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado).ToList();
                        }
                        else
                        {
                            // pedidos Analisados
                            return _ctx.PedidoParticipacaoFormacao.Where(p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoAnalisadoAcademia).ToList();
                        }
                    }
                }

                return null;

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        /// <summary>
        /// Este método deverá ser utilizado para obter os Pedidos que estão no Fluxo de tratamento pela Academia: 
        ///     Aprovado Direcção, 
        ///     Devolvido Direcção, 
        ///     Analisado,
        ///     Autorizado,
        ///     Rejeitado CA
        /// </summary>
        /// <param name="origin">A origem do pedido do utilizador</param>
        /// <param name="state">O estado dos pedidos que se pretendem obter</param>
        /// <param name="onlyCompleted">Apenas Pedidos terminados</param>
        /// <returns></returns>
        public static List<PedidoParticipacaoFormacao> __GetAllPedidosFormacao(Enumerations.AcademiaOrigemAcessoFuncionalidade origin, int state, bool onlyCompleted)
        {
            if (origin != Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuGestao)
                return null;

            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    if (onlyCompleted)
                    {
                        // pedidos Finalizados
                        return _ctx.PedidoParticipacaoFormacao.Where(p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado).ToList();
                    }
                    else
                    {
                        // Os pedidos finalizados são tratados no ciclo if acima
                        if (state < (int)Enumerations.EstadoPedidoFormacao.PedidoAprovadoDireccao && state > (int)Enumerations.EstadoPedidoFormacao.PedidoRejeitadoCA)
                            return null;

                        return _ctx.PedidoParticipacaoFormacao.Where(p => p.Estado.Value == state).ToList();


                    }
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

        public static List<Formando> __GetAllFormandos(ConfiguracaoAprovacaoUtilizador cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade origin)
        {
            Enumerations.AcademiaOrigemAcessoFuncionalidade chief = Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia;
            Enumerations.AcademiaOrigemAcessoFuncionalidade director = Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuDirector;

            if ((origin == chief && (cfgUser.AreasChefia == null || cfgUser.AreasChefia.Count() == 0)) ||
                (origin == chief && (cfgUser.CRespChefia == null || cfgUser.CRespChefia.Count() == 0)))
            {
                return null;
            }

            if(origin == director && (cfgUser.AreasDirige == null || cfgUser.AreasDirige.Count() == 0))
            {
                return null;
            }

            try
            {
                using(var _ctxExt = new SuchDBContextExtention())
                {
                    List<Formando> result = new List<Formando>();

                    var parameters = new[]
                    {
                            new SqlParameter("@EmployeeNo", null),
                            new SqlParameter("@Regions", null),
                            new SqlParameter("@FunctionalAreas",
                                origin == chief? string.Join(",", cfgUser.AreasChefia) :
                                (origin == director? string.Join(",", cfgUser.AreasDirige) : null)),
                            new SqlParameter("@RespCenters", origin == chief? string.Join(",", cfgUser.CRespChefia) : null)
                    };

                    IEnumerable<dynamic> data = _ctxExt.execStoredProcedure("exec NAV2009Formandos @EmployeeNo, @Regions, @FunctionalAreas, @RespCenters", parameters);

                    foreach (var tmp in data)
                    {
                        if (!tmp.NoMecanografico.Equals(DBNull.Value))
                        {
                            Formando formando = new Formando()
                            {
                                No = (string)tmp.NoMecanografico,
                                Name = tmp.NomeCompleto.Equals(DBNull.Value) ? "" : (string)tmp.NomeCompleto,
                                Regiao = tmp.Regiao.Equals(DBNull.Value) ? "" : (string)tmp.Regiao,
                                Area = tmp.Area.Equals(DBNull.Value) ? "" : (string)tmp.Area,
                                Cresp = tmp.CResp.Equals(DBNull.Value) ? "" : (string)tmp.CResp,
                                RegiaoNav2017 = tmp.RegiaoNav2017.Equals(DBNull.Value) ? "" : (string)tmp.RegiaoNav2017,
                                AreaNav2017 = tmp.AreaNav2017.Equals(DBNull.Value) ? "" : (string)tmp.AreaNav2017,
                                CrespNav2017 = tmp.CRespNav2017.Equals(DBNull.Value) ? "" : (string)tmp.CRespNav2017,
                                Projecto = tmp.NoProjecto.Equals(DBNull.Value) ? "" : (string)tmp.NoProjecto,
                                Funcao = tmp.Funcao.Equals(DBNull.Value) ? "" : (string)tmp.Funcao,
                                CodEstabelecimento = tmp.CodEstabelecimento.Equals(DBNull.Value) ? "" : (string)tmp.CodEstabelecimento,
                                DescricaoEstabelecimento = tmp.DescEstabelecimento.Equals(DBNull.Value)? "" : (string)tmp.DescEstabelecimento
                            };

                            result.Add(formando);
                        }
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Formando __GetDetailsFormando(string employeeNo)
        {
            if (string.IsNullOrWhiteSpace(employeeNo))
                return null;
            try
            {
                using(var _ctxExt = new SuchDBContextExtention())
                {
                    
                    var parameters = new[]
                    {
                            new SqlParameter("@EmployeeNo", employeeNo),
                            new SqlParameter("@Regions", null),
                            new SqlParameter("@FunctionalAreas", null),
                            new SqlParameter("@RespCenters", null)
                    };

                    dynamic data = _ctxExt.execStoredProcedure("exec NAV2009Formandos @EmployeeNo, @Regions, @FunctionalAreas, @RespCenters", parameters).FirstOrDefault();

                    if (data != null)
                    {
                        if (!data.NoMecanografico.Equals(DBNull.Value))
                        {
                            Formando formando = new Formando()
                            {
                                No = (string)data.NoMecanografico,
                                Name = data.NomeCompleto.Equals(DBNull.Value) ? "" : (string)data.NomeCompleto,
                                Regiao = data.Regiao.Equals(DBNull.Value) ? "" : (string)data.Regiao,
                                Area = data.Area.Equals(DBNull.Value) ? "" : (string)data.Area,
                                Cresp = data.CResp.Equals(DBNull.Value) ? "" : (string)data.CResp,
                                RegiaoNav2017 = data.RegiaoNav2017.Equals(DBNull.Value) ? "" : (string)data.RegiaoNav2017,
                                AreaNav2017 = data.AreaNav2017.Equals(DBNull.Value) ? "" : (string)data.AreaNav2017,
                                CrespNav2017 = data.CRespNav2017.Equals(DBNull.Value) ? "" : (string)data.CRespNav2017,
                                Projecto = data.NoProjecto.Equals(DBNull.Value) ? "" : (string)data.NoProjecto,
                                Funcao = data.Funcao.Equals(DBNull.Value) ? "" : (string)data.Funcao,
                                CodEstabelecimento = data.CodEstabelecimento.Equals(DBNull.Value) ? "" : (string)data.CodEstabelecimento,
                                DescricaoEstabelecimento = data.DescEstabelecimento.Equals(DBNull.Value) ? "" : (string)data.DescEstabelecimento
                            };

                            return formando;
                        } 
                    }

                }
            }
            catch (Exception ex)
            {

                return null;
            }

            return null;
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

                    return true;
                }
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

        public static bool __UpdateTemaFormacao(TemaFormacao tema)
        {
            if (tema == null)
                return false;

            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    _ctx.TemaFormacao.Update(tema);
                    _ctx.SaveChanges();

                    return true;
                }
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
