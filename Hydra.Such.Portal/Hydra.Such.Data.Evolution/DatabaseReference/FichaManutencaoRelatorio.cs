using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class FichaManutencaoRelatorio
    {
        public int Id { get; set; }
        public string Om { get; set; }
        public int? IdEquipamento { get; set; }
        public int? Rotina { get; set; }
        public string Codigo { get; set; }
        public string Versao { get; set; }
        public int? EstadoFinal { get; set; }
        public string Observacao { get; set; }
        public string AssinaturaTecnico { get; set; }
        public int? IdAssinaturaTecnico { get; set; }
        public string AssinaturaCliente { get; set; }
        public bool? AssinaturaClienteManual { get; set; }
        public string AssinaturaSie { get; set; }
        public bool? AssinaturaSieIgualCliente { get; set; }
        public int? CriadoPor { get; set; }
        public DateTime? CriadoEm { get; set; }
        public int? ActualizadoPor { get; set; }
        public DateTime? ActualizadoEm { get; set; }
        public string RelatorioTrabalho { get; set; }
    }
}
