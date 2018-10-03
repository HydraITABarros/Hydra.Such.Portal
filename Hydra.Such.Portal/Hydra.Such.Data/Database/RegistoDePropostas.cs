using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class RegistoDePropostas
    {
        public int NumLinha { get; set; }
        public string NumConsultaMercado { get; set; }
        public int? NumLinhaConsultaMercado { get; set; }
        public string Alternativa { get; set; }
        public string CodProduto { get; set; }
        public string VatproductPostingGroup { get; set; }
        public string Descricao { get; set; }
        public string Descricao2 { get; set; }
        public string NumProjecto { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string CodActividade { get; set; }
        public string CodLocalizacao { get; set; }
        public decimal? Quantidade { get; set; }
        public string Fornecedor1Code { get; set; }
        public string Fornecedor1Nome { get; set; }
        public string VatbusinessPostingGroup1 { get; set; }
        public decimal? Fornecedor1Preco { get; set; }
        public bool? Fornecedor1Select { get; set; }
        public string Fornecedor2Code { get; set; }
        public string Fornecedor2Nome { get; set; }
        public string VatbusinessPostingGroup2 { get; set; }
        public decimal? Fornecedor2Preco { get; set; }
        public bool? Fornecedor2Select { get; set; }
        public string Fornecedor3Code { get; set; }
        public string Fornecedor3Nome { get; set; }
        public string VatbusinessPostingGroup3 { get; set; }
        public decimal? Fornecedor3Preco { get; set; }
        public bool? Fornecedor3Select { get; set; }
        public string Fornecedor4Code { get; set; }
        public string Fornecedor4Nome { get; set; }
        public string VatbusinessPostingGroup4 { get; set; }
        public decimal? Fornecedor4Preco { get; set; }
        public bool? Fornecedor4Select { get; set; }
        public string Fornecedor5Code { get; set; }
        public string Fornecedor5Nome { get; set; }
        public string VatbusinessPostingGroup5 { get; set; }
        public decimal? Fornecedor5Preco { get; set; }
        public bool? Fornecedor5Select { get; set; }
        public string Fornecedor6Code { get; set; }
        public string Fornecedor6Nome { get; set; }
        public string VatbusinessPostingGroup6 { get; set; }
        public decimal? Fornecedor6Preco { get; set; }
        public bool? Fornecedor6Select { get; set; }

        public ConsultaMercado NumConsultaMercadoNavigation { get; set; }
    }
}
