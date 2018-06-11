using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class RequisiçõesClienteContrato
    {
        public string NºContrato { get; set; }
        public int GrupoFatura { get; set; }
        public string NºProjeto { get; set; }
        public DateTime DataInícioCompromisso { get; set; }
        public DateTime? DataFimCompromisso { get; set; }
        public string NºRequisiçãoCliente { get; set; }
        public DateTime? DataRequisição { get; set; }
        public string NºCompromisso { get; set; }
        public DateTime? DataÚltimaFatura { get; set; }
        public string NºFatura { get; set; }
        public decimal? ValorFatura { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
    }
}
