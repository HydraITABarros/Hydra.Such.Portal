using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class CafetariasRefeitórios
    {
        public int NºUnidadeProdutiva { get; set; }
        public int Tipo { get; set; }
        public int Código { get; set; }
        public DateTime DataInícioExploração { get; set; }
        public DateTime? DataFimExploração { get; set; }
        public string Descrição { get; set; }
        public string CódResponsável { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string Armazém { get; set; }
        public string ArmazémLocal { get; set; }
        public string NºProjeto { get; set; }
        public bool? Ativa { get; set; }
        public DateTime? DataModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public int? NºRefeições { get; set; }

        public Projetos NºProjetoNavigation { get; set; }
        public UnidadesProdutivas NºUnidadeProdutivaNavigation { get; set; }
    }
}
