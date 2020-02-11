using System;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas2CartaoCombustivel
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public int? IDEmpresa { get; set; }
        public string NoCartao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Plafon { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}
