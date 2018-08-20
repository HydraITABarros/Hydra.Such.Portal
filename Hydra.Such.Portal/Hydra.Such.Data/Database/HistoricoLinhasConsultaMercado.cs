using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class HistoricoLinhasConsultaMercado
    {
        public int NumLinha { get; set; }
        public string NumConsultaMercado { get; set; }
        public int NumVersao { get; set; }
        public string CodProduto { get; set; }
        public string Descricao { get; set; }
        public string NumProjecto { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string CodActividade { get; set; }
        public string CodLocalizacao { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? CustoUnitarioPrevisto { get; set; }
        public decimal? CustoTotalPrevisto { get; set; }
        public decimal? CustoUnitarioObjectivo { get; set; }
        public decimal? CustoTotalObjectivo { get; set; }
        public string CodUnidadeMedida { get; set; }
        public DateTime? DataEntregaPrevista { get; set; }
        public string NumRequisicao { get; set; }
        public int? LinhaRequisicao { get; set; }
        public DateTime? CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? ModificadoEm { get; set; }
        public string ModificadoPor { get; set; }

        public Actividades CodActividadeNavigation { get; set; }
        public Projetos NumProjectoNavigation { get; set; }
        public Requisição NumRequisicaoNavigation { get; set; }
    }
}
