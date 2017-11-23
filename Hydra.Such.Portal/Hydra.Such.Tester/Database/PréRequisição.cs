using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class PréRequisição
    {
        public PréRequisição()
        {
            Anexos = new HashSet<Anexos>();
            LinhasPréRequisição = new HashSet<LinhasPréRequisição>();
        }

        public string NºPréRequisição { get; set; }
        public int? Área { get; set; }
        public int? TipoRequisição { get; set; }
        public string NºProjeto { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public bool? Urgente { get; set; }
        public bool? Amostra { get; set; }
        public bool? Anexo { get; set; }
        public bool? Imobilizado { get; set; }
        public bool? CompraADinheiro { get; set; }
        public int? CódigoLocalRecolha { get; set; }
        public int? CódigoLocalEntrega { get; set; }
        public string Observações { get; set; }
        public bool? ModeloDePréRequisição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public Locais CódigoLocalEntregaNavigation { get; set; }
        public Locais CódigoLocalRecolhaNavigation { get; set; }
        public Projetos NºProjetoNavigation { get; set; }
        public TiposRequisições TipoRequisiçãoNavigation { get; set; }
        public ICollection<Anexos> Anexos { get; set; }
        public ICollection<LinhasPréRequisição> LinhasPréRequisição { get; set; }
    }
}
