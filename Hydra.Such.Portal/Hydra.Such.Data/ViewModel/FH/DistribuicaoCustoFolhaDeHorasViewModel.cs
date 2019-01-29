using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.FH
{
    public partial class DistribuicaoCustoFolhaDeHorasViewModel
    {
        public string NoFolhasDeHoras { get; set; }
        public int NoLinhaPercursosEAjudasCustoDespesas { get; set; }
        public int NoLinha { get; set; }
        public int? TipoObra { get; set; }
        public string NoObra { get; set; }
        public decimal? PercentagemValor { get; set; }
        public decimal? Valor { get; set; }
        public decimal? TotalValor { get; set; }
        public decimal? TotalPercentagemValor { get; set; }
        public decimal? KmTotais { get; set; }
        public decimal? KmDistancia { get; set; }
        public decimal? Quantidade { get; set; }
        public string CodigoRegiao { get; set; }
        public string CodigoAreaFuncional { get; set; }
        public string CodigoCentroResponsabilidade { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
