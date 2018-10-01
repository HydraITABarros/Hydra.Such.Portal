using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class RequisicoesRegAlteracoes
    {
        public int Id { get; set; }
        public string NºRequisição { get; set; }
        public int Estado { get; set; }
        public DateTime ModificadoEm { get; set; }
        public string ModificadoPor { get; set; }

        public Requisição NºRequisiçãoNavigation { get; set; }
    }
}
