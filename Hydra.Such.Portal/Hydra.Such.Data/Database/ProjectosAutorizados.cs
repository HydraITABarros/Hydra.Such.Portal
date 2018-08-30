using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ProjectosAutorizados
    {
        public string CodProjeto { get; set; }
        public int GrupoFactura { get; set; }
        public string Descricao { get; set; }
        public string CodCliente { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string CodContrato { get; set; }
        public string CodEnderecoEnvio { get; set; }
        public string GrupoContabilisticoObra { get; set; }
        public string GrupoContabilisticoProjeto { get; set; }
        public string NumSerie { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public string DataServPrestado { get; set; }
        public string Observacoes { get; set; }
        public string Observacoes1 { get; set; }
        public string PedidoCliente { get; set; }
        public int? Opção { get; set; }
        public DateTime? DataPedido { get; set; }
        public string DescricaoGrupo { get; set; }
        public string CodTermosPagamento { get; set; }
        public string Diversos { get; set; }
        public string NumCompromisso { get; set; }
        public string SituacoesPendentes { get; set; }
        public DateTime? DataPrestacaoServico { get; set; }
        public string CodMetodoPagamento { get; set; }
        public bool Faturado { get; set; }
    }
}
