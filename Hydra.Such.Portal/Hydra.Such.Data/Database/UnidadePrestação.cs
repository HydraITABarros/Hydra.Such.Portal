using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class UnidadePrestação
    {
        public int Código { get; set; }
        public string Descrição { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }
        public string EmailRegiao12 { get; set; }
        public string EmailRegiao23 { get; set; }
        public string EmailRegiao33 { get; set; }
        public string EmailRegiao43 { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
    }
}
