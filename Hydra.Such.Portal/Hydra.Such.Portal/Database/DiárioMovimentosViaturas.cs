using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class DiárioMovimentosViaturas
    {
        public int NºLinha { get; set; }
        public string Utilizador { get; set; }
        public string Matrícula { get; set; }
        public DateTime? DataRegisto { get; set; }
        public int? TipoMovimento { get; set; }
        public decimal? Quantidade { get; set; }
        public string Recurso { get; set; }
        public string Descrição { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public Viaturas MatrículaNavigation { get; set; }
    }
}
