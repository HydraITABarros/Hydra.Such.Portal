using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class LinhasContratos
    {
        public int TipoContrato { get; set; }
        public string NºContrato { get; set; }
        public int NºVersão { get; set; }
        public int NºLinha { get; set; }
        public int? Tipo { get; set; }
        public string Código { get; set; }
        public string Descrição { get; set; }
        public decimal? Quantidade { get; set; }
        public string CódUnidadeMedida { get; set; }
        public decimal? PreçoUnitário { get; set; }
        public decimal? DescontoLinha { get; set; }
        public bool? Faturável { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public decimal? Periodicidade { get; set; }
        public decimal? NºHorasIntervenção { get; set; }
        public decimal? NºTécnicos { get; set; }
        public int? TipoProposta { get; set; }
        public DateTime? DataInícioVersão { get; set; }
        public DateTime? DataFimVersão { get; set; }
        public string NºResponsável { get; set; }
        public int? CódServiçoCliente { get; set; }
        public int? GrupoFatura { get; set; }
        public bool? CriaContrato { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }

        public Contratos Contratos { get; set; }
    }
}
