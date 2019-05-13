using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class MaintenanceCatalog
    {
        public byte[] Timestamp { get; set; }
        public int Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public byte ManutCorrectiva { get; set; }
        public byte ManutPreventiva { get; set; }
        public int FaultReasonType { get; set; }
        public byte TempoResposta { get; set; }
        public byte TempoImobilização { get; set; }
        public byte TempoEfectivoReparação { get; set; }
        public byte TempoFechoObras { get; set; }
        public byte TempoFacturação { get; set; }
        public byte TempoOcupColaboradores { get; set; }
        public byte ValorCustoVenda { get; set; }
        public byte TaxaCumprimentoCats { get; set; }
        public byte TaxaCoberturaCats { get; set; }
        public byte TaxaCumprimentoRotinasMp { get; set; }
        public byte IncidenciasAvarias { get; set; }
        public byte OrdensEmCurso { get; set; }
    }
}
