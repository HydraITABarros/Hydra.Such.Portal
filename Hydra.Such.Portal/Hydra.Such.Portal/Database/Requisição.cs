using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class Requisição
    {
        public Requisição()
        {
            Anexos = new HashSet<Anexos>();
            DiárioDeProjeto = new HashSet<DiárioDeProjeto>();
            LinhasPEncomendaProcedimentosCcp = new HashSet<LinhasPEncomendaProcedimentosCcp>();
            LinhasRequisição = new HashSet<LinhasRequisição>();
        }

        public string NºRequisição { get; set; }
        public int? Área { get; set; }
        public int? Estado { get; set; }
        public string NºProjeto { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string CódigoLocalização { get; set; }
        public string NºFuncionário { get; set; }
        public string Viatura { get; set; }
        public DateTime? DataReceção { get; set; }
        public bool? Urgente { get; set; }
        public bool? Amostra { get; set; }
        public bool? Anexo { get; set; }
        public bool? Imobilizado { get; set; }
        public bool? CompraADinheiro { get; set; }
        public int? CódigoLocalRecolha { get; set; }
        public int? CódigoLocalEntrega { get; set; }
        public string Observações { get; set; }
        public bool? ModeloDeRequisição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public Projetos NºProjetoNavigation { get; set; }
        public Viaturas ViaturaNavigation { get; set; }
        public ICollection<Anexos> Anexos { get; set; }
        public ICollection<DiárioDeProjeto> DiárioDeProjeto { get; set; }
        public ICollection<LinhasPEncomendaProcedimentosCcp> LinhasPEncomendaProcedimentosCcp { get; set; }
        public ICollection<LinhasRequisição> LinhasRequisição { get; set; }
    }
}
