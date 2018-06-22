using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class PreçosServiçosCliente
    {
        public string Cliente { get; set; }
        public string Nome { get; set; }
        public string Nome2 { get; set; }
        public string CodServCliente { get; set; }
        public string DescriçãoServiço { get; set; }
        public decimal? PreçoVenda { get; set; }
        public decimal? PreçoDeCusto { get; set; }
        public DateTime? Data { get; set; }
        public string Recurso { get; set; }
        public string DescriçãoDoRecurso { get; set; }
        public string UnidadeMedida { get; set; }
        public string TipoRefeição { get; set; }
        public string DescriçãoTipoRefeição { get; set; }
        public string CodigoRegião { get; set; }
        public string CodigoArea { get; set; }
        public string CodigoCentroResponsabilidade { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public Serviços CodServClienteNavigation { get; set; }
    }
}
