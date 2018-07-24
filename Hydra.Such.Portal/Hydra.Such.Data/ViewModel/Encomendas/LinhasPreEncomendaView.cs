using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Encomendas
{
    public class LinhasPreEncomendaView : ErrorHandler
    {
        public string NumPreEncomenda { get; set; }
        public int NumLinhaPreEncomenda { get; set; }
        public string CodigoProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public string CodigoLocalizacao { get; set; }
        public string CodigoUnidadeMedida { get; set; }
        public decimal? QuantidadeDisponibilizada { get; set; }
        public decimal? CustoUnitario { get; set; }
        public string NumFornecedor { get; set; }
        public string CodigoRegiao { get; set; }
        public string CodigoAreaFuncional { get; set; }
        public string CodigoCentroResponsabilidade { get; set; }
        public string NumRequisicao { get; set; }
        public int? NumLinhaRequisicao { get; set; }
        public string NumProjeto { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }

        //Campos tratados
        public string DataHoraCriacao_Show { get; set; }
        public string DataHoraModificacao_Show { get; set; }
        public string NomeFornecedor_Show { get; set; }
    }
}
