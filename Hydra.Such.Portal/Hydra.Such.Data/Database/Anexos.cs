using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Anexos
    {
        public TipoOrigemAnexos TipoOrigem { get; set; }
        public string NºOrigem { get; set; }
        public int NºLinha { get; set; }
        public string UrlAnexo { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public PréRequisição NºOrigemNavigation { get; set; }
    }

    public enum TipoOrigemAnexos
    {
        PreRequisicao = 1,
        Requisicao = 2,
        Contratos = 3,
        Procedimentos = 4,
        ConsultaMercado = 5,
        Oportunidades = 6,
        Propostas = 7,
        Orcamentos = 8
    }
}
