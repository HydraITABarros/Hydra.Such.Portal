using System;
using System.Collections.Generic;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;

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
        public string UtilizadorSubmissão { get; set; }
        public string DataSubmissaoTxt { get; set; }
        public string UtilizadorAprovacaoChefia { get; set; }
        public string DataAprovacaoChefiaTxt { get; set; }

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
            UtilizadorCriacao = Pedido.UtilizadorCriacao;
            DataHoraCriacao = Pedido.DataHoraCriacao;
            UtilizadorUltimaModificacao = Pedido.UtilizadorUltimaModificacao;
            DataHoraUltimaModificacao = Pedido.DataHoraUltimaModificacao;

            

            //AccaoNavigation = Pedido.AccaoNavigation;

            RegistosAlteracoes = Pedido.RegistosAlteracoes;
            if (RegistosAlteracoes != null && RegistosAlteracoes.Count > 0)
            {
                // TO DO: preencher os campos de rastreabilidade
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
    }
}
