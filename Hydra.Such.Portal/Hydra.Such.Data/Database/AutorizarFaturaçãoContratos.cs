using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class AutorizarFaturaçãoContratos
    {
        public string NºContrato { get; set; }
        public int GrupoFatura { get; set; }
        public string Descrição { get; set; }
        public string NºCliente { get; set; }
        public int? NºDeFaturasAEmitir { get; set; }
        public decimal? TotalAFaturar { get; set; }
        public decimal? ValorDoContrato { get; set; }
        public decimal? ValorPorFaturar { get; set; }
        public decimal? ValorFaturado { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataDeExpiração { get; set; }
        public DateTime? DataPróximaFatura { get; set; }
        public DateTime? DataDeRegisto { get; set; }
        public int? Estado { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataModificação { get; set; }
    }
}
