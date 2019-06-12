using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class LinhaFolhaHoras
    {
        public byte[] Timestamp { get; set; }
        public long NºFolhaHoras { get; set; }
        public int TipoCusto { get; set; }
        public int NºLinha { get; set; }
        public string CódTipoCusto { get; set; }
        public decimal Quantidade { get; set; }
        public decimal CustoUnitário { get; set; }
        public decimal CustoTotal { get; set; }
        public decimal PreçoUnitário { get; set; }
        public decimal PreçoVenda { get; set; }
        public string CódOrigem { get; set; }
        public string CódDestino { get; set; }
        public decimal Distância { get; set; }
        public decimal DistânciaPrevista { get; set; }
        public byte RegistarSubsídiosEPrémios { get; set; }
        public string Observação { get; set; }
        public string RubricaSalarial2 { get; set; }
        public DateTime DataDespesa { get; set; }
        public byte CalculoAutomático { get; set; }
        public string Matricula { get; set; }
    }
}
