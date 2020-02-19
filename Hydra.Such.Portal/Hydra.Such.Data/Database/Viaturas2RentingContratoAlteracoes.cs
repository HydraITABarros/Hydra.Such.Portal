using System;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas2RentingContratoAlteracoes
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public int IDContrato { get; set; }
        public DateTime? DataPedido { get; set; }
        public string AutorizadoFINLOG { get; set; }
        public DateTime? DataAlteracaoKm { get; set; }
        public Decimal? KmAlteracao { get; set; }
        public string MensalidadeAlteracao { get; set; }
        public string Observacoes { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}
