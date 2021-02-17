using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Projects
{
    public class AuthorizedProjectViewModel : ErrorHandler
    {
        private string dataPrestacaoServico = string.Empty;

        public string CodProjeto { get; set; }
        public int GrupoFactura { get; set; }
        public string Descricao { get; set; }
        public string NomeCliente { get; set; }
        public string CodCliente { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string CodContrato { get; set; }
        public string CodEnderecoEnvio { get; set; }
        public string GrupoContabilisticoObra { get; set; }
        public string GrupoContabilisticoProjeto { get; set; }
        public string NumSerie { get; set; }
        public string Utilizador { get; set; }
        public string DataAutorizacao { get; set; }
        public string Observacoes { get; set; }
        public string Observacoes1 { get; set; }
        public string PedidoCliente { get; set; }
        public int? Opcao { get; set; }
        public string DataPedido { get; set; }
        public string DescricaoGrupo { get; set; }
        public string CodTermosPagamento { get; set; }
        public string Diversos { get; set; }
        public string NumCompromisso { get; set; }
        public string SituacoesPendentes { get; set; }
        public string DataPrestacaoServico { get; set; }
        public string DataPrestacaoServicoFim { get; set; }
        //{
        //    get { return this.dataPrestacaoServico; }
        //    set
        //    {
        //        DateTime servDate;
        //        if (DateTime.TryParse(value, out servDate))
        //        {
        //            string monthName = servDate.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("pt-PT"));
        //            this.DataServPrestado = string.Format("{0}/{1}", monthName.ToUpper(), servDate.Year.ToString());
        //        }
        //        this.dataPrestacaoServico = value;
        //    }
        //}
        /// <summary>
        /// Formato extenso ex: Janeiro/2018
        /// </summary>
        public string DataServPrestado { get; set; }
        public string CodMetodoPagamento { get; set; }
        public bool Faturado { get; set; }
        public decimal ValorAutorizado { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
