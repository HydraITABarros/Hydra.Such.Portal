using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.FH
{
    public class PercursosEAjudasCustoDespesasFolhaDeHorasViewModel
    {
        public string FolhaDeHorasNo { get; set; }
        public int? TipoCusto { get; set; }
        public int? LinhaNo { get; set; }
        public string Descricao { get; set; }
        public string Origem { get; set; }
        public string Destino { get; set; }
        public DateTime? DataViagem { get; set; }
        public string DataViagemTexto { get; set; }
        public decimal Distancia { get; set; }
        public decimal Quantidade { get; set; }
        public decimal CustoUnitario { get; set; }
        public decimal CustoTotal { get; set; }
        public decimal PrecoUnitario { get; set; }
        public string Justificacao { get; set; }
        public string RubricaSalarial { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string DataHoraCriacaoTexto { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string DataHoraModificacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
