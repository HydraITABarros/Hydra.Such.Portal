using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class ComprasViewModel
    {
        public int ID { get; set; }
        public string CodigoProduto { get; set; }
        public string Descricao { get; set; }
        public string Descricao2 { get; set; }
        public string CodigoUnidadeMedida { get; set; }
        public decimal? Quantidade { get; set; }
        public string NoRequisicao { get; set; }
        public int? NoLinhaRequisicao { get; set; }
        public bool? Urgente { get; set; }
        public string UrgenteTexto { get; set; }
        public string RegiaoMercadoLocal { get; set; }
        public int? Estado { get; set; }
        public string EstadoTexto { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string HoraCriacaoTexto { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorCriacaoTexto { get; set; }
        public string Responsaveis { get; set; }
        public string NoProjeto { get; set; }
        public string NoProjetoTexto { get; set; }
        public string NoFornecedor { get; set; }
        public string NoFornecedorTexto { get; set; }
        public string NoEncomenda { get; set; }
        public string NoEncomendaTexto { get; set; }
        public DateTime? DataEncomenda { get; set; }
        public string DataEncomendaTexto { get; set; }
        public string HoraEncomendaTexto { get; set; }
        public string NoConsultaMercado { get; set; }
        public DateTime? DataConsultaMercado { get; set; }
        public string DataConsultaMercadoTexto { get; set; }
        public string HoraConsultaMercadoTexto { get; set; }
        public DateTime? DataValidacao { get; set; }
        public string DataValidacaoTexto { get; set; }
        public string HoraValidacaoTexto { get; set; }
        public string UtilizadorValidacao { get; set; }
        public string UtilizadorValidacaoTexto { get; set; }
        public DateTime? DataRecusa { get; set; }
        public string DataRecusaTexto { get; set; }
        public string HoraRecusaTexto { get; set; }
        public string UtilizadorRecusa { get; set; }
        public string UtilizadorRecusaTexto { get; set; }
        public DateTime? DataTratado { get; set; }
        public string DataTratadoTexto { get; set; }
        public string HoraTratadoTexto { get; set; }
        public string UtilizadorTratado { get; set; }
        public string UtilizadorTratadoTexto { get; set; }
        public bool? Recusada { get; set; }
        public string RecusadaTexto { get; set; }
        public string RecusadoComprasTexto { get; set; }
        public DateTime? DataMercadoLocal { get; set; }
        public string DataMercadoLocalTexto { get; set; }
        public string HoraMercadoLocalTexto { get; set; }
    }
}
