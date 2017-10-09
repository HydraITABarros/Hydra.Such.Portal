using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
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
    }
}
