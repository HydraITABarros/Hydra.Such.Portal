using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class MovimentosProjectoAutorizados
    {
        public int NumMovimento { get; set; }
        public DateTime DataRegisto { get; set; }
        public int Tipo { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal Quantidade { get; set; }
        public string CodUnidadeMedida { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal PrecoTotal { get; set; }
        public string CodProjeto { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string CodContrato { get; set; }
        public string CodGrupoServico { get; set; }
        public string CodServCliente { get; set; }
        public string DescServCliente { get; set; }
        public string NumGuiaResiduosGar { get; set; }
        public string NumGuiaExterna { get; set; }
        public DateTime? DataConsumo { get; set; }
        public int TipoRefeicao { get; set; }
        public int TipoRecurso { get; set; }
        public string NumDocumento { get; set; }
        public decimal? PrecoCusto { get; set; }
        public decimal? CustoTotal { get; set; }
        public string CodCliente { get; set; }
        public int? GrupoFactura { get; set; }
        public string GrupoFaturaDescricao { get; set; }
    }
}
