using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ProcessosDisciplinaresInquérito
    {
        public int Tipo { get; set; }
        public int AnoProcesso { get; set; }
        public string NºDoProcesso { get; set; }
        public int? TipoDeProcesso { get; set; }
        public int? Descritor { get; set; }
        public DateTime? DataEntrada { get; set; }
        public DateTime? DataArquivo { get; set; }
        public string EntidadeRemetente { get; set; }
        public string NºDeProcesso { get; set; }
        public string Interessado { get; set; }
        public string DocumentoAssociado { get; set; }
        public DateTime? DataDocumento { get; set; }
        public int? EstadoDoProcesso { get; set; }
        public string Observações { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataCriaçãoModificação { get; set; }
        public string Nome { get; set; }
        public string Serviço { get; set; }
        public string Infração { get; set; }
        public string Sanção { get; set; }
        public bool? Reincidência { get; set; }
        public int? NºInstrutor { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }

        public Instrutores NºInstrutorNavigation { get; set; }
    }
}
