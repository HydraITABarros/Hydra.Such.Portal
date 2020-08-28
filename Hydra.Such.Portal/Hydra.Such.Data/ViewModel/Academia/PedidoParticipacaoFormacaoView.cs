//using Hydra.Such.Data.Extensions;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Text;
using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.Academia
{
    public class PedidoParticipacaoFormacaoView : PedidoParticipacaoFormacao
    {
        
        #region Campos data para texto
        public string DataInicioTxt { get; set; }
        public string DataFimTxt { get; set; }

        public string DataHoraCriacaoTxt { get; set; }
        #endregion

        #region Rastreabilidade das transições de Estado
        public string UtilizadorAprovacaoChefia { get; set; }
        public string DataAprovacaoChefiaTxt { get; set; }

        public string UtilizadorAprovacaoDireccao { get; set; }
        public string DataAprovacaoDireccaoTxt { get; set; }

        public string UtilizadorAnaliseAcademia { get; set; }
        public string DataAnaliseAcademiaTxt { get; set; }

        public string UtilizadorAutorizacaoAdmin { get; set; }
        public string DataAutorizacaoAdminTxt { get; set; }

        public string UtilizadorCriacaoInscricao { get; set; }
        public string DataCriacaoInscricaoTxt { get; set; }
        #endregion

        public PedidoParticipacaoFormacaoView(PedidoParticipacaoFormacao Pedido)
        {
            this.IdPedido = Pedido.IdPedido;
            this.Estado = Pedido.Estado;
            this.IdEmpregado = Pedido.IdEmpregado;
            this.NomeEmpregado = Pedido.NomeEmpregado;
            this.FuncaoEmpregado = Pedido.FuncaoEmpregado;
            this.ProjectoEmpregado = Pedido.ProjectoEmpregado;
            this.IdAreaFuncional = Pedido.IdAreaFuncional;
            this.AreaFuncionalEmpregado = Pedido.AreaFuncionalEmpregado;
            this.IdCentroResponsabilidade = Pedido.IdCentroResponsabilidade;
            this.CentroResponsabilidadeEmpregado = Pedido.CentroResponsabilidadeEmpregado;
            this.IdEstabelecimento = Pedido.IdEstabelecimento;
            this.DescricaoEstabelecimento = Pedido.DescricaoEstabelecimento;
            this.IdAccaoFormacao = Pedido.IdAccaoFormacao;
            this.DesignacaoAccao = Pedido.DesignacaoAccao;
            this.LocalRealizacao = Pedido.LocalRealizacao;
            this.DataInicio = Pedido.DataInicio;
            this.DataFim = Pedido.DataFim;
            this.NumeroTotalHoras = Pedido.NumeroTotalHoras;
            this.IdEntidadeFormadora = Pedido.IdEntidadeFormadora;
            this.DescricaoEntidadeFormadora = Pedido.DescricaoEntidadeFormadora;
            this.CustoInscricao = Pedido.CustoInscricao;
            this.ValorIva = Pedido.ValorIva;
            this.CustoDeslocacoes = Pedido.CustoDeslocacoes;
            this.CustoEstadia = Pedido.CustoEstadia;
            this.DescricaoConhecimentos = Pedido.DescricaoConhecimentos;
            this.FundamentacaoChefia = Pedido.FundamentacaoChefia;
            this.Planeada = Pedido.Planeada;
            this.TemDotacaoOrcamental = Pedido.TemDotacaoOrcamental;
            this.ParecerAcademia = Pedido.ParecerAcademia;
            this.UtilizadorCriacao = Pedido.UtilizadorCriacao;
            this.DataHoraCriacao = Pedido.DataHoraCriacao;
            this.UtilizadorUltimaModificacao = Pedido.UtilizadorUltimaModificacao;
            this.DataHoraUltimaModificacao = Pedido.DataHoraUltimaModificacao;

            //this.AccaoNavigation = Pedido.AccaoNavigation;

            this.RegistosAlteracoes = Pedido.RegistosAlteracoes;

            this.DataHoraCriacaoTxt = DateToText(Pedido.DataHoraCriacao);
            this.DataInicioTxt = DateToText(Pedido.DataInicio);
            this.DataFimTxt = DateToText(Pedido.DataFim);
        }

        private string DateToText(DateTime? dateToCheck)
        {
            try
            {
                return (dateToCheck == DateTime.Parse("1753-01-01") || dateToCheck == DateTime.Parse("1900-01-01") ? string.Empty : dateToCheck.ToString());
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
