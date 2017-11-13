using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Telemóveis
    {
        public string ImeiNºSérie { get; set; }
        public int Tipo { get; set; }
        public int? Marca { get; set; }
        public int? EstadoEquipamento { get; set; }
        public int? Estado { get; set; }
        public string Observações { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }

        public Marcas MarcaNavigation { get; set; }
    }
}
