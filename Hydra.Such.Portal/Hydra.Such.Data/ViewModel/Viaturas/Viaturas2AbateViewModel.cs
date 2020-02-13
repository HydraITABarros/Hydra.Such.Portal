using System;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class Viaturas2AbateViewModel : ErrorHandler
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public int? IDTipoAtoAdministrativo { get; set; }
        public string TipoAtoAdministrativo { get; set; }
        public string NoRegisto { get; set; }
        public int? IDDescricaoAto { get; set; }
        public string DescricaoAto { get; set; }
        public string Fundamentacao { get; set; }
        public string Autor { get; set; }
        public DateTime? Data { get; set; }
        public string DataTexto { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }
    }
}
