﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class LinhasAcordoPrecosViewModel : ErrorHandler
    {
        public string NoProcedimento { get; set; }
        public string NoFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public string NoSubFornecedor { get; set; }
        public string NomeSubFornecedor { get; set; }
        public string CodProduto { get; set; }
        public DateTime DtValidadeInicio { get; set; }
        public string DtValidadeInicioTexto { get; set; }
        public DateTime? DtValidadeFim { get; set; }
        public string DtValidadeFimTexto { get; set; }
        public string Cresp { get; set; }
        public string CrespNome { get; set; }
        public string Area { get; set; }
        public string AreaNome { get; set; }
        public string Regiao { get; set; }
        public string RegiaoNome { get; set; }
        public string Localizacao { get; set; }
        public string LocalizacaoNome { get; set; }
        public decimal? CustoUnitario { get; set; }
        public string CustoUnitarioTexto { get; set; }
        public decimal? CustoUnitarioSubFornecedor { get; set; }
        public string CustoUnitarioSubFornecedorTexto { get; set; }
        public string DescricaoProduto { get; set; }
        public string Um { get; set; }
        public decimal? QtdPorUm { get; set; }
        public string QtdPorUmTexto { get; set; }
        public decimal? PesoUnitario { get; set; }
        public string PesoUnitarioTexto { get; set; }
        public string CodProdutoFornecedor { get; set; }
        public string DescricaoProdFornecedor { get; set; }
        public int? FormaEntrega { get; set; }
        public string FormaEntregaTexto { get; set; }
        public string UserId { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public int? TipoPreco { get; set; }
        public string TipoPrecoTexto { get; set; }
        public string GrupoRegistoIvaProduto { get; set; }
        public string GrupoRegistoIvaProdutoTexto { get; set; }
        public string CodCategoriaProduto { get; set; }
        public decimal? TaxaIVA { get; set; }
        public string NoContrato { get; set; }
        public int? Interface { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
