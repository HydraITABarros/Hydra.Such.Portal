using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class Viaturas2InspecoesViewModel : ErrorHandler
    {
        public int ID { get; set; }
        public string Matricula { get; set; }
        public DateTime? DataInspecao { get; set; }
        public string DataInspecaoTexto { get; set; }
        public Decimal? KmInspecao { get; set; }
        public int? IDResultado { get; set; }
        public string Resultado { get; set; }
        public DateTime? ProximaInspecao { get; set; }
        public string ProximaInspecaoTexto { get; set; }
        public string Observacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }
    }
}
