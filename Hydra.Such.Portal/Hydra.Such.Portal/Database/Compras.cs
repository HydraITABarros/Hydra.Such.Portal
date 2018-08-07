using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class Compras
    {
        public int Id { get; set; }
        public string CodigoProduto { get; set; }
        public string Descricao { get; set; }
        public string Descricao2 { get; set; }
        public string CodigoUnidadeMedida { get; set; }
        public decimal? Quantidade { get; set; }
        public string NoRequisicao { get; set; }
        public int? NoLinhaRequisicao { get; set; }
        public bool? Urgente { get; set; }
        public string RegiaoMercadoLocal { get; set; }
        public int? Estado { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string Responsaveis { get; set; }
        public string NoProjeto { get; set; }
        public string NoFornecedor { get; set; }
        public string NoEncomenda { get; set; }
        public DateTime? DataEncomenda { get; set; }
        public string NoConsultaMercado { get; set; }
        public DateTime? DataConsultaMercado { get; set; }
        public DateTime? DataValidacao { get; set; }
        public string UtilizadorValidacao { get; set; }
        public DateTime? DataRecusa { get; set; }
        public string UtilizadorRecusa { get; set; }
        public DateTime? DataTratado { get; set; }
        public string UtilizadorTratado { get; set; }
        public bool? Recusada { get; set; }
        public DateTime? DataMercadoLocal { get; set; }

        public ConfigMercadoLocal RegiaoMercadoLocalNavigation { get; set; }
    }
}
