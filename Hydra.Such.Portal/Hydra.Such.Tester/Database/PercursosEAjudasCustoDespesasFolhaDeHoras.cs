using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class PercursosEAjudasCustoDespesasFolhaDeHoras
    {
        public string NºFolhaDeHoras { get; set; }
        public int CodPercursoAjuda { get; set; }
        public int NºLinha { get; set; }
        public string Origem { get; set; }
        public string OrigemDescricao { get; set; }
        public string Destino { get; set; }
        public string DestinoDescricao { get; set; }
        public DateTime? DataViagem { get; set; }
        public string Justificação { get; set; }
        public decimal? Distância { get; set; }
        public decimal? DistanciaPrevista { get; set; }
        public decimal? CustoUnitário { get; set; }
        public decimal? CustoTotal { get; set; }
        public int? TipoCusto { get; set; }
        public int? CodTipoCusto { get; set; }
        public string Descrição { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? PreçoUnitário { get; set; }
        public decimal? PrecoVenda { get; set; }
        public string RúbricaSalarial { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraModificação { get; set; }

        public FolhasDeHoras NºFolhaDeHorasNavigation { get; set; }
    }
}
