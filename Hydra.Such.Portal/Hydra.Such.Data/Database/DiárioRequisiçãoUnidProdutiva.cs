﻿using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class DiárioRequisiçãoUnidProdutiva
    {
        public int NºLinha { get; set; }
        public int? NºUnidadeProdutiva { get; set; }
        public string NºProduto { get; set; }
        public string Descrição { get; set; }
        public string Descricao2 { get; set; }
        public string CódUnidadeMedida { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? QuantidadeDisponivel { get; set; }
        public decimal? QuantidadeReservada { get; set; }
        public decimal? CustoUnitárioDireto { get; set; }
        public decimal? Valor { get; set; }
        public string NºProjeto { get; set; }
        public string NºFornecedor { get; set; }
        public string NoSubFornecedor { get; set; }
        public int? TipoRefeição { get; set; }
        public bool? TabelaPreçosFornecedor { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataReceçãoEsperada { get; set; }
        public DateTime? DataPPreçoFornecedor { get; set; }
        public string CodigoLocalização { get; set; }
        public decimal? QuantidadePorUnidMedida { get; set; }
        public string CodigoProdutoFornecedor { get; set; }
        public string DescriçãoProdutoFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public string NomeSubFornecedor { get; set; }
        public string NºEncomendaAberto { get; set; }
        public string NºLinhaEncomendaAberto { get; set; }
        public string DescriçãoUnidadeProduto { get; set; }
        public string NºDocumento { get; set; }
        public string Observações { get; set; }
        public string GrupoRegistoIvaProduto { get; set; }
        public int? Tipo { get; set; }
        public int? Interface { get; set; }
        public decimal? CustoUnitarioSubFornecedor { get; set; }
        public string NoContrato { get; set; }

        public UnidadesProdutivas NºUnidadeProdutivaNavigation { get; set; }
        public TiposRefeição TipoRefeiçãoNavigation { get; set; }
    }
}
