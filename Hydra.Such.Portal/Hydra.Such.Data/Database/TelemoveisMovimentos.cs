using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class TelemoveisMovimentos
    {
        public int NumMovimento { get; set; }
        public string NumCartao { get; set; }
        public string Data { get; set; }
        public string NumFactura { get; set; }
        public decimal ValorComIva { get; set; }
        public DateTime DataFactura { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string Utilizador { get; set; }
        public DateTime DataRegisto { get; set; }
        public string NumDocumento { get; set; }
        public decimal Valor { get; set; }
    }
}
