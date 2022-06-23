using System;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas2Afetacao
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public int? IDAreaReal { get; set; }
        public string LocalExato { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}
