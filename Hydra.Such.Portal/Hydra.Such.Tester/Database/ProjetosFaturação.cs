using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class ProjetosFaturação
    {
        public int NºUnidadeProdutiva { get; set; }
        public string NºProjeto { get; set; }
        public bool? Ativo { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public Projetos NºProjetoNavigation { get; set; }
        public UnidadesProdutivas NºUnidadeProdutivaNavigation { get; set; }
    }
}
