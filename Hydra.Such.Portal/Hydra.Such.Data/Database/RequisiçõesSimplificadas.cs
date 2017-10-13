using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class RequisiçõesSimplificadas
    {
        public RequisiçõesSimplificadas()
        {
            LinhasRequisiçõesSimplificadas = new HashSet<LinhasRequisiçõesSimplificadas>();
        }

        public string NºRequisição { get; set; }
        public int? Estado { get; set; }
        public DateTime? DataHoraRequisição { get; set; }
        public DateTime? DataRegisto { get; set; }
        public string CódLocalização { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string NºProjeto { get; set; }
        public DateTime? DataHoraAprovação { get; set; }
        public DateTime? DataHoraEnvio { get; set; }
        public DateTime? DataHoraDisponibilização { get; set; }
        public string ResponsávelCriação { get; set; }
        public string ResponsávelAprovação { get; set; }
        public string ResponsávelEnvio { get; set; }
        public string ResponsávelReceção { get; set; }
        public bool? Imprimir { get; set; }
        public bool? Anexo { get; set; }
        public string NºFuncionário { get; set; }
        public bool? Urgente { get; set; }
        public int? NºUnidadeProdutiva { get; set; }
        public string Observações { get; set; }
        public bool? Terminada { get; set; }
        public string ResponsávelVisar { get; set; }
        public DateTime? DataHoraVisar { get; set; }
        public bool? Autorizada { get; set; }
        public string ResponsávelAutorização { get; set; }
        public DateTime? DataHoraAutorização { get; set; }
        public string Visadores { get; set; }
        public bool? DataReceçãoLinhas { get; set; }
        public bool? RequisiçãoNutrição { get; set; }
        public DateTime? DataReceçãoEsperada { get; set; }
        public bool? RequisiçãoModelo { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public Projetos NºProjetoNavigation { get; set; }
        public UnidadesProdutivas NºUnidadeProdutivaNavigation { get; set; }
        public ICollection<LinhasRequisiçõesSimplificadas> LinhasRequisiçõesSimplificadas { get; set; }
    }
}
