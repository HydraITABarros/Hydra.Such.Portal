using System;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas2Modelos
    {
        public int ID { get; set; }
        public int IDMarca { get; set; }
        public string Modelo { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}
