using System;
using System.Collections.Generic;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using System.Linq;

namespace Hydra.Such.Data.ViewModel.Academia
{
    public class PedidoParticipacaoFormacaoView : PedidoParticipacaoFormacao
    {
        public int AnoPedido { get; set; }
        public int MesPedido { get; set; }

        #region Campos data para texto
        public string DataInicioTxt { get; set; }
        public string DataFimTxt { get; set; }

        public string DataHoraCriacaoTxt { get; set; }
        #endregion

        #region Rastreabilidade das transições de Estado
        public string UtilizadorSubmissao { get; set; }
        public string DataSubmissaoTxt { get; set; }

        public string UtilizadorAprovacaoChefia { get; set; }
        public string DataAprovacaoChefiaTxt { get; set; }

        public string UtilizadorAprovacaoCoordenacao { get; set; }
        public string DataAprovacaoCoordenacaoTxt { get; set; }

        public string UtilizadorAprovacaoDireccao { get; set; }
        public string DataAprovacaoDireccaoTxt { get; set; }

       
        public string UtilizadorParecerAcademia { get; set; }
        public string DataParecerAcademiaTxt { get; set; }

        public string UtilizadorAnaliseAcademia { get; set; }
        public string DataAnaliseAcademiaTxt { get; set; }

        public string UtilizadorAutorizacaoAdmin { get; set; }
        public string DataAutorizacaoAdminTxt { get; set; }

        public string UtilizadorCriacaoInscricao { get; set; }
        public string DataCriacaoInscricaoTxt { get; set; }
        #endregion

        public ICollection<AttachmentsViewModel> AnexosPedido { get; set; }

        public ICollection<Comentario> ComentariosPedido { get; set; }

        public PedidoParticipacaoFormacaoView()
        {

        }
        public PedidoParticipacaoFormacaoView(PedidoParticipacaoFormacao Pedido)
        {
            IdPedido = Pedido.IdPedido;
            Estado = Pedido.Estado;

            AnoPedido = Pedido.DataHoraCriacao.HasValue && Pedido.DataHoraCriacao.Value.Year != 1753 && Pedido.DataHoraCriacao.Value.Year != 1900 ? Pedido.DataHoraCriacao.Value.Year : 0;
            MesPedido = Pedido.DataHoraCriacao.HasValue ? Pedido.DataHoraCriacao.Value.Month : 0;
            
            IdEmpregado = Pedido.IdEmpregado;
            NomeEmpregado = Pedido.NomeEmpregado;
            FuncaoEmpregado = Pedido.FuncaoEmpregado;
            ProjectoEmpregado = Pedido.ProjectoEmpregado;
            IdAreaFuncional = Pedido.IdAreaFuncional;
            AreaFuncionalEmpregado = Pedido.AreaFuncionalEmpregado;
            IdCentroResponsabilidade = Pedido.IdCentroResponsabilidade;
            CentroResponsabilidadeEmpregado = Pedido.CentroResponsabilidadeEmpregado;
            IdEstabelecimento = Pedido.IdEstabelecimento;
            DescricaoEstabelecimento = Pedido.DescricaoEstabelecimento;
            IdAccaoFormacao = Pedido.IdAccaoFormacao;
            DesignacaoAccao = Pedido.DesignacaoAccao;
            LocalRealizacao = Pedido.LocalRealizacao;
            IdSessao = Pedido.IdSessao;
            Horario = Pedido.Horario;
            DataInicio = Pedido.DataInicio;
            DataFim = Pedido.DataFim;
            NumeroTotalHoras = Pedido.NumeroTotalHoras;
            IdEntidadeFormadora = Pedido.IdEntidadeFormadora;
            DescricaoEntidadeFormadora = Pedido.DescricaoEntidadeFormadora;
            CustoInscricao = Pedido.CustoInscricao;
            ValorIva = Pedido.ValorIva;
            CustoDeslocacoes = Pedido.CustoDeslocacoes;
            CustoEstadia = Pedido.CustoEstadia;
            DescricaoConhecimentos = Pedido.DescricaoConhecimentos;
            FundamentacaoChefia = Pedido.FundamentacaoChefia;
            Planeada =  Pedido.Planeada ?? 0;
            CursoPlanoFormacao = Pedido.CursoPlanoFormacao;
            TemDotacaoOrcamental = Pedido.TemDotacaoOrcamental ?? 0;
            ParecerAcademia = Pedido.ParecerAcademia;
            ParecerDotacaoAcademia = Pedido.ParecerDotacaoAcademia;
            UtilizadorCriacao = Pedido.UtilizadorCriacao;
            DataHoraCriacao = Pedido.DataHoraCriacao;
            UtilizadorUltimaModificacao = Pedido.UtilizadorUltimaModificacao;
            DataHoraUltimaModificacao = Pedido.DataHoraUltimaModificacao;            


            RegistosAlteracoes = Pedido.RegistosAlteracoes;
            if (RegistosAlteracoes != null && RegistosAlteracoes.Count > 0)
            {
                // 1. Registo da submissão
                RegistoAlteracoesPedidoFormacao registo = RegistosAlteracoes
                        .OrderBy(r => r.DataHoraAlteracao)
                        .LastOrDefault(r => r.TipoAlteracao == (int)Enumerations.TipoAlteracaoPedidoFormacao.SubmissaoChefia);

                if (registo != null)
                {
                    DataSubmissaoTxt = DateToText(registo.DataHoraAlteracao);
                    UtilizadorSubmissao = registo.UtilizadorAlteracao;
                }                
                
                // 2. Registo da Aprovação pela Chefia
                registo = null;

                
                registo = RegistosAlteracoes
                        .OrderBy(r => r.DataHoraAlteracao)
                        .LastOrDefault(r => r.TipoAlteracao == (int)Enumerations.TipoAlteracaoPedidoFormacao.AprovacaoChefia);

                if (registo != null)
                {
                    UtilizadorAprovacaoChefia = registo.UtilizadorAlteracao;
                    DataAprovacaoChefiaTxt = DateToText(registo.DataHoraAlteracao);
                }
                else
                {
                    registo = RegistosAlteracoes
                            .OrderBy(r => r.DataHoraAlteracao)
                            .LastOrDefault(r => r.TipoAlteracao == (int)Enumerations.TipoAlteracaoPedidoFormacao.AprovacaoCoordenacao);
                    if (registo != null)
                    {
                        UtilizadorAprovacaoChefia = registo.UtilizadorAlteracao;
                        DataAprovacaoChefiaTxt = DateToText(registo.DataHoraAlteracao);

                        UtilizadorAprovacaoCoordenacao = UtilizadorAprovacaoChefia;
                        DataAprovacaoCoordenacaoTxt = DataAprovacaoChefiaTxt;
                    }
                    else
                    {
                        registo = RegistosAlteracoes
                            .OrderBy(r => r.DataHoraAlteracao)
                            .LastOrDefault(r => r.TipoAlteracao == (int)Enumerations.TipoAlteracaoPedidoFormacao.AprovacaoDireccao);
                        if (registo != null)
                        {
                            UtilizadorAprovacaoChefia = registo.UtilizadorAlteracao;
                            DataAprovacaoChefiaTxt = DateToText(registo.DataHoraAlteracao);

                            UtilizadorAprovacaoCoordenacao = UtilizadorAprovacaoChefia;
                            DataAprovacaoCoordenacaoTxt = DataAprovacaoChefiaTxt;

                            UtilizadorAprovacaoDireccao = UtilizadorAprovacaoChefia;
                            DataAprovacaoDireccaoTxt = DataAprovacaoChefiaTxt;

                        }
                    }
                }

                // 3. Registo de Aprovação pelo Coordenador
                registo = null;
                registo = RegistosAlteracoes
                        .OrderBy(r => r.DataHoraAlteracao)
                        .LastOrDefault(r => r.TipoAlteracao == (int)Enumerations.TipoAlteracaoPedidoFormacao.AprovacaoCoordenacao);
                if (registo != null)
                {
                    UtilizadorAprovacaoCoordenacao = registo.UtilizadorAlteracao;
                    DataAprovacaoCoordenacaoTxt = DateToText(registo.DataHoraAlteracao);
                }
                else
                {
                    registo = RegistosAlteracoes
                           .OrderBy(r => r.DataHoraAlteracao)
                           .LastOrDefault(r => r.TipoAlteracao == (int)Enumerations.TipoAlteracaoPedidoFormacao.AprovacaoDireccao);
                    if (registo != null)
                    {
                        UtilizadorAprovacaoCoordenacao = UtilizadorAprovacaoChefia;
                        DataAprovacaoCoordenacaoTxt = DataAprovacaoChefiaTxt;

                        UtilizadorAprovacaoDireccao = UtilizadorAprovacaoChefia;
                        DataAprovacaoDireccaoTxt = DataAprovacaoChefiaTxt;

                    }
                }

                // 4. Registo de Aprovação pelo Director
                registo = null;

                registo = RegistosAlteracoes
                        .OrderBy(r => r.DataHoraAlteracao)
                        .Where(r => r.TipoAlteracao == (int)Enumerations.TipoAlteracaoPedidoFormacao.AprovacaoDireccao)
                        .LastOrDefault();

                if (registo != null)
                {
                    DataAprovacaoDireccaoTxt = DateToText(registo.DataHoraAlteracao);
                    UtilizadorAprovacaoDireccao = registo.UtilizadorAlteracao;
                }

                // 5. Parecer da Academia
                registo = null;

                registo = RegistosAlteracoes
                        .OrderBy(r => r.DataHoraAlteracao)
                        .LastOrDefault(r => r.TipoAlteracao == (int)Enumerations.TipoAlteracaoPedidoFormacao.ParecerAcademia);

                if (registo != null)
                {
                    DataParecerAcademiaTxt = DateToText(registo.DataHoraAlteracao);
                    UtilizadorParecerAcademia = registo.UtilizadorAlteracao;
                }

                // 6. Submissão CA
                registo = null;

                registo = RegistosAlteracoes
                        .OrderBy(r => r.DataHoraAlteracao)
                        .LastOrDefault(r => r.TipoAlteracao == (int)Enumerations.TipoAlteracaoPedidoFormacao.SubmissaoConsAdmin);

                if (registo != null)
                {
                    DataAnaliseAcademiaTxt = DateToText(registo.DataHoraAlteracao);
                    UtilizadorAnaliseAcademia = registo.UtilizadorAlteracao;
                }

                // 7. Aprovação CA
                registo = null;

                registo = RegistosAlteracoes
                        .OrderBy(r => r.DataHoraAlteracao)
                        .LastOrDefault(r => r.TipoAlteracao == (int)Enumerations.TipoAlteracaoPedidoFormacao.AutorizacaoConsAdmin);

                if (registo != null)
                {
                    DataAutorizacaoAdminTxt = DateToText(registo.DataHoraAlteracao);
                    UtilizadorAutorizacaoAdmin = registo.UtilizadorAlteracao;

                }

            }

            DataHoraCriacaoTxt = DateToText(Pedido.DataHoraCriacao);
            DataInicioTxt = DateToText(Pedido.DataInicio);
            DataFimTxt = DateToText(Pedido.DataFim);

            List<Anexos> anexos = DBAttachments.GetById(TipoOrigemAnexos.PedidoFormacao, Pedido.IdPedido);
            AnexosPedido = DBAttachments.ParseToViewModel(anexos);
        }

        private string DateToText(DateTime? dateToCheck)
        {
            try
            {
                return (dateToCheck == DateTime.Parse("1753-01-01") || dateToCheck == DateTime.Parse("1900-01-01") ? string.Empty : dateToCheck.Value.ToString("yyyy-MM-dd"));
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }
        private DateTime TextToDateTime(string textToCheck)
        {
            try
            {
                return string.IsNullOrWhiteSpace(textToCheck) ? DateTime.Parse("1900-01-01") : DateTime.Parse(textToCheck);
            }
            catch (Exception ex)
            {
                return DateTime.Parse("1900-01-01");
            }
        }

        public void GetCourse()
        {
            Accao = DBAcademia.__GetDetailsAccaoFormacao(IdAccaoFormacao);
        }

        public void GetTrackingHistory()
        {

        }

        public void GetComments()
        {
            ComentariosPedido = DBAcademia.__GetCometariosPedido(IdPedido);
        }

        public PedidoParticipacaoFormacao ParseToDb()
        {
            PedidoParticipacaoFormacao pedido = new PedidoParticipacaoFormacao()
            {
                IdPedido = IdPedido,
                Estado = Estado,
                IdEmpregado = IdEmpregado,
                NomeEmpregado = NomeEmpregado,
                FuncaoEmpregado = FuncaoEmpregado,
                ProjectoEmpregado = ProjectoEmpregado,
                IdAreaFuncional = IdAreaFuncional,
                AreaFuncionalEmpregado = AreaFuncionalEmpregado,
                IdCentroResponsabilidade = IdCentroResponsabilidade,
                CentroResponsabilidadeEmpregado = CentroResponsabilidadeEmpregado,
                IdEstabelecimento = IdEstabelecimento,
                DescricaoEstabelecimento = DescricaoEstabelecimento,
                IdAccaoFormacao = IdAccaoFormacao,
                DesignacaoAccao = DesignacaoAccao,
                LocalRealizacao = LocalRealizacao,
                IdSessao = IdSessao,
                Horario = Horario,
                DataInicio = TextToDateTime(DataInicioTxt),
                DataFim = TextToDateTime(DataFimTxt),
                NumeroTotalHoras = NumeroTotalHoras,
                IdEntidadeFormadora = IdEntidadeFormadora,
                DescricaoEntidadeFormadora = DescricaoEntidadeFormadora,
                CustoInscricao = CustoInscricao,
                ValorIva = ValorIva,
                CustoDeslocacoes = CustoDeslocacoes,
                CustoEstadia = CustoEstadia,
                DescricaoConhecimentos = DescricaoConhecimentos,
                FundamentacaoChefia = FundamentacaoChefia,
                Planeada = Planeada ?? 0,
                CursoPlanoFormacao = CursoPlanoFormacao,
                TemDotacaoOrcamental = TemDotacaoOrcamental ?? 0,
                ParecerAcademia = ParecerAcademia,
                ParecerDotacaoAcademia = ParecerDotacaoAcademia,
                UtilizadorCriacao = UtilizadorCriacao,
                DataHoraCriacao = DataHoraCriacao,
                UtilizadorUltimaModificacao = UtilizadorUltimaModificacao,
                DataHoraUltimaModificacao = DataHoraUltimaModificacao
            };

            return pedido;
        }
    }
}
