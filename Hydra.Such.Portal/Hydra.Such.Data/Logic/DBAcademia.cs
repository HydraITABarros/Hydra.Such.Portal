﻿using Hydra.Such.Data.Database;
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
        public string DescRegiaoNav2017 { get; set; }
        public string AreaNav2017 { get; set; } 
        public string DescAreaNav2017 { get; set; }
        public string CrespNav2017 { get; set; }
        public string DescCrespNav2017 { get; set; }
        public string Projecto { get; set; }
        public string Funcao { get; set; }
        public string CodEstabelecimento { get; set; }
        public string DescricaoEstabelecimento { get; set; }
        public ICollection<PedidoParticipacaoFormacao> PedidosFormacao { get; set; }

        public Formando()
        {

        }

        public Formando(string employeeNo)
        {
            Formando f = DBAcademia.__GetDetailsFormando(employeeNo);
            No = f.No;
            Name = f.Name;
            Regiao = f.Regiao;
            Area = f.Area;
            Cresp = f.Cresp;
            RegiaoNav2017 = f.RegiaoNav2017;
            DescRegiaoNav2017 = f.DescRegiaoNav2017;
            AreaNav2017 = f.AreaNav2017;
            DescAreaNav2017 = f.DescAreaNav2017;
            CrespNav2017 = f.CrespNav2017;
            DescCrespNav2017 = f.DescCrespNav2017;
            Projecto = f.Projecto;
            Funcao = f.Funcao;
            CodEstabelecimento = f.CodEstabelecimento;
            DescricaoEstabelecimento = f.DescricaoEstabelecimento;

            PedidosFormacao = DBAcademia.__GetAllPedidosFormacao(f.No);
        }

        public bool AlreadyRegisteredForCourse(string courseId, ref string requestId)
        {
            requestId = string.Empty;

            if (!string.IsNullOrEmpty(courseId))
            {
                if (PedidosFormacao != null && PedidosFormacao.Count > 0)
                {
                    try
                    {
                        requestId = PedidosFormacao.Where(p => p.IdAccaoFormacao == courseId && p.Estado <= (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado).FirstOrDefault().IdPedido;
                        return true;
                    }
                    catch (Exception ex)
                    {

                        return false;
                    }                    
                }
            }

            return false;
        }
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
        public string EmployeeNo { get; set; }
        public Enumerations.TipoUtilizadorFluxoPedidoFormacao TipoUtilizadorGlobal { get; set; }
        public List<string> AreasChefia { get; set; }
        public List<string> CRespChefia { get; set; }
        public List<string> AreasDirige { get; set; }
        public List<ConfiguracaoAprovacaoAcademia> ConfiguracaoAprovAcademia { get; set; }

        public ConfiguracaoAprovacaoUtilizador(ConfigUtilizadores userConfig, int type)
        {
            IdUtilizador = userConfig.IdUtilizador;
            EmployeeNo = userConfig.EmployeeNo;

            try
            {
                TipoUtilizadorGlobal = (Enumerations.TipoUtilizadorFluxoPedidoFormacao)userConfig.TipoUtilizadorFormacao.Value;
            }
            catch (Exception)
            {

                TipoUtilizadorGlobal = Enumerations.TipoUtilizadorFluxoPedidoFormacao.Formando;
            }
            

            ConfiguracaoAprovAcademia = new List<ConfiguracaoAprovacaoAcademia>();

            AreasChefia = new List<string>();
            CRespChefia = new List<string>();
            AreasDirige = new List<string>();

            List<UtilizadoresGruposAprovação> grupos = DBApprovalUserGroup.GetByUser(userConfig.IdUtilizador);
            List<ConfiguraçãoAprovações> configAprovadores = DBApprovalConfigurations.GetAllByType(type);

           if(configAprovadores != null && configAprovadores.Count > 0)
            {
                foreach(var c in configAprovadores)
                {
                    if(grupos.Where(g => g.GrupoAprovação == c.GrupoAprovação).FirstOrDefault() != null)
                    {
                        if(c.NívelAprovação == 1 || c.NívelAprovação == 2)
                        {
                            ConfiguracaoAprovAcademia.Add(new ConfiguracaoAprovacaoAcademia(userConfig.IdUtilizador, c));
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
        public static PedidoParticipacaoFormacao __CriarPedidoFormacao(AccaoFormacao accao, Formando formando, ConfigUtilizadores user)
        {
            if (accao == null)
            {
                return null;
            }

            Configuração config = DBConfigurations.GetById(1);
            int numeracaoPedidos = config.NumeracaoPedidoFormacao == null ? 0 : config.NumeracaoPedidoFormacao.Value;

            if (numeracaoPedidos > 0)
            {
                try
                {
                    //Formando formando = __GetDetailsFormando(user.EmployeeNo);
                    //Formando formando = new Formando(user.EmployeeNo);

                    PedidoParticipacaoFormacao pedido = new PedidoParticipacaoFormacao()
                    {
                        IdPedido = DBNumerationConfigurations.GetNextNumeration(numeracaoPedidos, true, false),
                        Estado = (int)Enumerations.EstadoPedidoFormacao.PedidoCriado,
                        IdEmpregado = formando.No,
                        NomeEmpregado = formando.Name,
                        FuncaoEmpregado = formando.Funcao,
                        ProjectoEmpregado = formando.Projecto,
                        IdAreaFuncional = formando.AreaNav2017,
                        AreaFuncionalEmpregado = formando.DescAreaNav2017,
                        IdCentroResponsabilidade = formando.CrespNav2017,
                        CentroResponsabilidadeEmpregado = formando.DescCrespNav2017,
                        IdEstabelecimento = formando.CodEstabelecimento,
                        DescricaoEstabelecimento = formando.DescricaoEstabelecimento,
                        IdAccaoFormacao = accao.IdAccao,
                        DesignacaoAccao = accao.DesignacaoAccao,
                        DataInicio = accao.DataInicio,
                        DataFim = accao.DataFim,
                        NumeroTotalHoras = accao.NumeroTotalHoras,
                        UtilizadorCriacao = user.IdUtilizador,
                        DataHoraCriacao = DateTime.Now                        
                    };

                    if (accao.Entidade != null)
                    {
                        pedido.IdEntidadeFormadora = accao.Entidade.IdEntidade;
                        pedido.DescricaoEntidadeFormadora = accao.Entidade.DescricaoEntidade;
                    }

                    using (var _ctx = new SuchDBContext())
                    {
                        _ctx.PedidoParticipacaoFormacao.Add(pedido);
                        _ctx.SaveChanges();

                        return pedido;
                    }
                }
                catch (Exception ex)
                {

                    return null;
                }                
            }

            return null;
        }
        public static PedidoParticipacaoFormacao __CriarPedidoFormacao(ConfigUtilizadores user)
        {
            try
            {
                Configuração config = DBConfigurations.GetById(1);
                int numeracaoPedidos = config.NumeracaoPedidoFormacao == null ? 0 : config.NumeracaoPedidoFormacao.Value;

                if (numeracaoPedidos > 0)
                {
                    Formando formando = __GetDetailsFormando(user.EmployeeNo);

                    PedidoParticipacaoFormacao pedido = new PedidoParticipacaoFormacao()
                    {
                        IdPedido = DBNumerationConfigurations.GetNextNumeration(numeracaoPedidos, true, false),
                        Estado = (int)Enumerations.EstadoPedidoFormacao.PedidoCriado,
                        IdEmpregado = formando.No,
                        NomeEmpregado = formando.Name,
                        FuncaoEmpregado = formando.Funcao,
                        ProjectoEmpregado = formando.Projecto,
                        IdAreaFuncional = formando.AreaNav2017,
                        AreaFuncionalEmpregado = formando.DescAreaNav2017,
                        IdCentroResponsabilidade = formando.CrespNav2017,
                        CentroResponsabilidadeEmpregado = formando.DescCrespNav2017,
                        IdEstabelecimento = formando.CodEstabelecimento,
                        DescricaoEstabelecimento = formando.DescricaoEstabelecimento,
                        UtilizadorCriacao = user.IdUtilizador,
                        DataHoraCriacao = DateTime.Now
                    };

                    using (var _ctx = new SuchDBContext())
                    {
                        _ctx.PedidoParticipacaoFormacao.Add(pedido);
                        _ctx.SaveChanges();

                        return pedido;
                    }
                }
            }
            catch (Exception)
            {

                return null;
            }
            return null;
        }

        public static bool __CriarRegistoAlteracaoPedidoFormacao(PedidoParticipacaoFormacao pedidoFormacao, int tipoAlteracao, string descricao, string userName)
        {
            int lineNo = 10;

            if (pedidoFormacao == null)
                return false;

            if (pedidoFormacao.RegistosAlteracoes == null && pedidoFormacao.RegistosAlteracoes.Count() == 0)
            {
                pedidoFormacao.RegistosAlteracoes = __GetRegistosAlteracaoPedido(pedidoFormacao.IdPedido);
            }

            if (pedidoFormacao.RegistosAlteracoes != null && pedidoFormacao.RegistosAlteracoes.Count() > 0)
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
                UtilizadorAlteracao = userName,
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

        public static List<AccaoFormacao> __GetAllAccoesFormacao(string idTema, DateTime aposData)
        {
            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    return _ctx.AccaoFormacao.Where(a => a.IdTema == idTema && a.DataInicio > aposData).ToList();
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
                    return _ctx.SessaoAccaoFormacao.Where(s => s.IdAccao == idAccao)
                        .OrderByDescending(a => a.DataSessao)
                        .ThenByDescending(a => a.HoraInicioSessao).ToList();
                }
                catch (Exception ex)
                {

                    return null;
                }
            }
        }
        public static AccaoFormacao __GetDetailsAccaoFormacao(string accaoId)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    AccaoFormacao accao = _ctx.AccaoFormacao.Where(a => a.IdAccao == accaoId).LastOrDefault();

                    accao.SessoesFormacao = _ctx.SessaoAccaoFormacao.Where(s => s.IdAccao == accaoId)
                        .OrderByDescending(a => a.DataSessao)
                        .ThenByDescending(a => a.HoraInicioSessao)
                        .ToList(); ;

                    if (!string.IsNullOrEmpty(accao.IdEntidadeFormadora))
                    {
                        accao.Entidade = _ctx.EntidadeFormadora.Where(e => e.IdEntidade == accao.IdEntidadeFormadora).LastOrDefault();
                    }

                    return accao;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<TemaFormacao> __GetCatalogo(bool onlyActives = false)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    if (onlyActives)
                    {
                        List<TemaFormacao> temas = _ctx.TemaFormacao.Where(t => t.Activo == 1).ToList();
                        foreach (var item in temas)
                        {
                            item.AccoesTema = __GetAllAccoesFormacao(item.IdTema, DateTime.Now)
                                .Where(a => a.Activa.Value == 1).ToList();

                            if (item.AccoesTema == null || (item.AccoesTema != null && item.AccoesTema.Count == 0))
                            {
                                item.Activo = 0;
                                __UpdateTemaFormacao(item);
                            }
                        }
                        return temas.Where(t => t.Activo.Value == 1).ToList();
                        
                    }
                    else
                    {
                        return _ctx.TemaFormacao.ToList();
                    }

                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static TemaFormacao __GetDetailsTema(string idTema)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    TemaFormacao tema = _ctx.TemaFormacao.Where(t => t.IdTema == idTema).FirstOrDefault();

                    tema.AccoesTema = _ctx.AccaoFormacao.Where(a => a.IdTema == idTema)
                        .OrderByDescending(a => a.DataInicio)
                        .ThenByDescending(a => a.CodigoInterno)
                        .ToList();

                    foreach (var item in tema.AccoesTema)
                    {
                        item.SessoesFormacao = _ctx.SessaoAccaoFormacao.Where(s => s.IdAccao == item.IdAccao).ToList();
                    }
                    return tema;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static EntidadeFormadora __GetDetailsEntidade(string idEntidade)
        {
            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    return _ctx.EntidadeFormadora.Where(e => e.IdEntidade == idEntidade).FirstOrDefault();
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
        public static List<PedidoParticipacaoFormacao> __GetAllPedidosFormacao(string userName, bool? onlyActives)
        {
            if (string.IsNullOrEmpty(userName))
                return null;

            try
            {
                if (onlyActives.HasValue)
                {
                    if (onlyActives.Value)
                    {
                        // são considerados activos todos os pedidos que não estejam finalizados ou cancelados
                        using (var _ctx = new SuchDBContext())
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
                else
                {
                    // todos os pedidos do utilizador, independentemente do estado
                    using (var _ctx = new SuchDBContext())
                    {
                        return _ctx.PedidoParticipacaoFormacao.Where(p => p.UtilizadorCriacao == userName).ToList();
                    }
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PedidoParticipacaoFormacao> __GetAllPedidosFormacao(string employeeNo)
        {
            if (!string.IsNullOrEmpty(employeeNo))
            {
                try
                {
                    using (var _ctx = new SuchDBContext())
                    {
                        return _ctx.PedidoParticipacaoFormacao.Where(p => p.IdEmpregado == employeeNo).ToList();
                    }
                }
                catch (Exception ex)
                {

                    return null;
                }
            }

            return null;
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
                                                                p => p.IdEmpregado != cfgUser.EmployeeNo &&
                                                                p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado  &&
                                                                areasChefia.Contains(p.AreaFuncionalEmpregado) &&
                                                                cresps.Contains(p.CentroResponsabilidadeEmpregado)
                                                            ).ToList();
                        }
                        else
                        {
                            // pedidos submetidos
                            return _ctx.PedidoParticipacaoFormacao.Where(
                                                                p => p.IdEmpregado != cfgUser.EmployeeNo &&
                                                                p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoSubmetido  &&
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
                            // pedidos submetidos ou devolvidos pela Academia para completar
                            return _ctx.PedidoParticipacaoFormacao.Where(
                                                                p => (p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoAprovadoChefia ||
                                                                 p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoRejeitadoAcademia) &&
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

        public static PedidoParticipacaoFormacao __GetDetailsPedidoFormacao(string pedidoId)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    PedidoParticipacaoFormacao pedido = _ctx.PedidoParticipacaoFormacao.Where(p => p.IdPedido == pedidoId).LastOrDefault();

                    pedido.RegistosAlteracoes = _ctx.RegistoAlteracoesPedidoFormacao.Where(r => r.IdPedidoFormacao == pedidoId).ToList();

                    if (!string.IsNullOrEmpty(pedido.IdAccaoFormacao))
                    {
                        pedido.Accao = _ctx.AccaoFormacao.Where(a => a.IdAccao == pedido.IdAccaoFormacao).FirstOrDefault();
                    }
                    
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
                            new SqlParameter("@EmployeeNo", null)
                    };

                    IEnumerable<dynamic> data = _ctxExt.execStoredProcedure("exec NAV2009Formandos @EmployeeNo", parameters);

                    foreach (var tmp in data)
                    {
                        if (!tmp.NoMecanografico.Equals(DBNull.Value))
                        {
                            Formando formando = new Formando()
                            {
                                No = (string)tmp.NoMecanografico,
                                Name = tmp.NomeCompleto.Equals(DBNull.Value) ? string.Empty : (string)tmp.NomeCompleto,
                                Regiao = tmp.Regiao.Equals(DBNull.Value) ? string.Empty : (string)tmp.Regiao,
                                Area = tmp.Area.Equals(DBNull.Value) ? string.Empty : (string)tmp.Area,
                                Cresp = tmp.CResp.Equals(DBNull.Value) ? string.Empty : (string)tmp.CResp,
                                RegiaoNav2017 = tmp.RegiaoNav2017.Equals(DBNull.Value) ? string.Empty : (string)tmp.RegiaoNav2017,
                                DescRegiaoNav2017 = tmp.DescRegiaoNav2017.Equals(DBNull.Value) ? string.Empty : (string)tmp.DescRegiaoNav2017,
                                AreaNav2017 = tmp.AreaNav2017.Equals(DBNull.Value) ? string.Empty : (string)tmp.AreaNav2017,
                                DescAreaNav2017 = tmp.DescAreaNav2017.Equals(DBNull.Value) ? string.Empty : (string)tmp.DescAreaNav2017,
                                CrespNav2017 = tmp.CRespNav2017.Equals(DBNull.Value) ? string.Empty : (string)tmp.CRespNav2017,
                                DescCrespNav2017 = tmp.DescCRespNav2017.Equals(DBNull.Value) ? string.Empty : (string)tmp.DescCRespNav2017,
                                Projecto = tmp.NoProjecto.Equals(DBNull.Value) ? string.Empty : (string)tmp.NoProjecto,
                                Funcao = tmp.Funcao.Equals(DBNull.Value) ? string.Empty : (string)tmp.Funcao,
                                CodEstabelecimento = tmp.CodEstabelecimento.Equals(DBNull.Value) ? string.Empty : (string)tmp.CodEstabelecimento,
                                DescricaoEstabelecimento = tmp.DescEstabelecimento.Equals(DBNull.Value)? string.Empty : (string)tmp.DescEstabelecimento
                            };

                            result.Add(formando);
                        }
                    }

                    if (origin == chief)
                    {
                        result = result.Where(r => cfgUser.AreasChefia.Contains(r.AreaNav2017)).ToList()
                            .Where(r => cfgUser.CRespChefia.Contains(r.CrespNav2017)).ToList();
                    }

                    if (origin == director)
                    {
                        result = result.Where(r => cfgUser.AreasDirige.Contains(r.AreaNav2017)).ToList();
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
                            new SqlParameter("@EmployeeNo", employeeNo)
                    };

                    dynamic data = _ctxExt.execStoredProcedure("exec NAV2009Formandos @EmployeeNo", parameters).FirstOrDefault();

                    if (data != null)
                    {
                        if (!data.NoMecanografico.Equals(DBNull.Value))
                        {
                            Formando formando = new Formando()
                            {
                                No = (string)data.NoMecanografico,
                                Name = data.NomeCompleto.Equals(DBNull.Value) ? string.Empty : (string)data.NomeCompleto,
                                Regiao = data.Regiao.Equals(DBNull.Value) ? string.Empty : (string)data.Regiao,
                                Area = data.Area.Equals(DBNull.Value) ? string.Empty : (string)data.Area,
                                Cresp = data.CResp.Equals(DBNull.Value) ? string.Empty : (string)data.CResp,
                                RegiaoNav2017 = data.RegiaoNav2017.Equals(DBNull.Value) ? string.Empty : (string)data.RegiaoNav2017,
                                DescRegiaoNav2017 = data.DescRegiaoNav2017.Equals(DBNull.Value) ? string.Empty : (string)data.DescRegiaoNav2017,
                                AreaNav2017 = data.AreaNav2017.Equals(DBNull.Value) ? string.Empty : (string)data.AreaNav2017,
                                DescAreaNav2017 = data.DescAreaNav2017.Equals(DBNull.Value) ? string.Empty : (string)data.DescAreaNav2017,
                                CrespNav2017 = data.CRespNav2017.Equals(DBNull.Value) ? string.Empty : (string)data.CRespNav2017,
                                DescCrespNav2017 = data.DescCRespNav2017.Equals(DBNull.Value) ? string.Empty : (string)data.DescCRespNav2017,
                                Projecto = data.NoProjecto.Equals(DBNull.Value) ? string.Empty : (string)data.NoProjecto,
                                Funcao = data.Funcao.Equals(DBNull.Value) ? string.Empty : (string)data.Funcao,
                                CodEstabelecimento = data.CodEstabelecimento.Equals(DBNull.Value) ? string.Empty : (string)data.CodEstabelecimento,
                                DescricaoEstabelecimento = data.DescEstabelecimento.Equals(DBNull.Value) ? string.Empty : (string)data.DescEstabelecimento
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

        public static bool __UpdateAccaoFormacao(AccaoFormacao accao)
        {
            if (accao == null)
                return false;

            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    _ctx.AccaoFormacao.Update(accao);
                    _ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool __UpdateAccaoFormacao(AccaoFormacaoView accaoV)
        {
            try
            {
                foreach (var item in accaoV.ImagensAccao)
                {
                    DBAttachments.Update(DBAttachments.ParseToDB(item));
                }
                return __UpdateAccaoFormacao(accaoV.ParseToDb());
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

        public static bool __UpdateTemaFormacao(TemaFormacaoView temaV)
        {
            try
            {
                TemaFormacao tema = temaV.ParseToDb();
                foreach(var t in temaV.ImagensTema)
                {
                    DBAttachments.Update(DBAttachments.ParseToDB(t));
                }

                foreach (var item in temaV.Accoes)
                {
                    __UpdateAccaoFormacao(item.ParseToDb());
                }
                return __UpdateTemaFormacao(tema);
            }
            catch (Exception ex)
            {

                throw;
            }
            return false;
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
