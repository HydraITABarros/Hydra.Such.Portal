using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class Marcas
    {
        public Marcas()
        {
            Telemóveis = new HashSet<Telemóveis>();
            Viaturas = new HashSet<Viaturas>();
        }

        public int CódigoMarca { get; set; }
        public int? Tipo { get; set; }
        public string Descrição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<Telemóveis> Telemóveis { get; set; }
        public ICollection<Viaturas> Viaturas { get; set; }
    }
}
