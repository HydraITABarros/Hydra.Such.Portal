using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.FH
{
    public class LinhasFolhaHorasViewModel
    {
        public string NoFolhaHoras { get; set; }
        public int NoLinha { get; set; }
        public int? TipoCusto { get; set; }
        public string DescricaoTipoCusto { get; set; }
        public string CodTipoCusto { get; set; }
        public string DescricaoCodTipoCusto { get; set; }
        public string NoProjeto { get; set; }
        public string ProjetoDescricao { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? CustoUnitario { get; set; }
        public decimal? CustoTotal { get; set; }
        public decimal? PrecoUnitario { get; set; }
        public decimal? PrecoVenda { get; set; }
        public string CodOrigem { get; set; }
        public string DescricaoOrigem { get; set; }
        public string CodDestino { get; set; }
        public string DescricaoDestino { get; set; }
        public decimal? Distancia { get; set; }
        public decimal? DistanciaPrevista { get; set; }
        public bool? RegiaoAutonoma { get; set; }
        public string RubricaSalarial { get; set; }
        public bool? RegistarSubsidiosPremios { get; set; }
        public string Observacao { get; set; }
        public string RubricaSalarial2 { get; set; }
        public DateTime? DataDespesa { get; set; }
        public string DataDespesaTexto { get; set; }
        public string Funcionario { get; set; }
        public string CodRegiao { get; set; }
        public string CodArea { get; set; }
        public string CodCresp { get; set; }
        public bool? CalculoAutomatico { get; set; }
        public string Matricula { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string DataHoraCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string DataHoraModificacaoTexto { get; set; }
    }
}
