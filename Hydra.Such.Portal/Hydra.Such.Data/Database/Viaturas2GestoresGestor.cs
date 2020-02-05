using System;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas2GestoresGestor
    {
        public int ID { get; set; }
        public string Gestor { get; set; }
        public string SearchName { get; set; }
        public string NoMecanografico { get; set; }
        public string Mail { get; set; }
        public int? IDTipo { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}
