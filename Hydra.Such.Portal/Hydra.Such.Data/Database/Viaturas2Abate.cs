using System;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas2Abate
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public int? IDTipoAtoAdministrativo { get; set; }
        public string NoRegisto { get; set; }
        public int? IDDescricaoAto { get; set; }
        public string Fundamentacao { get; set; }
        public string Autor { get; set; }
        public DateTime? Data { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}
