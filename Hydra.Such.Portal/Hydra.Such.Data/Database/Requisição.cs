using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Requisição
    {
        public string NºRequisição { get; set; }
        public int? Área { get; set; }
        public int? Estado { get; set; }
        public string NºProjeto { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string CódigoLocalização { get; set; }
        public string NºFuncionário { get; set; }
        public string Viatura { get; set; }
        public DateTime? DataReceção { get; set; }
        public bool? Urgente { get; set; }
        public bool? Amostra { get; set; }
        public bool? Anexo { get; set; }
        public bool? Imobilizado { get; set; }
        public bool? CompraADinheiro { get; set; }
        public int? CódigoLocalRecolha { get; set; }
        public int? CódigoLocalEntrega { get; set; }
        public string Observações { get; set; }
        public bool? ModeloDeRequisição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public bool? CabimentoOrçamental { get; set; }
        public bool? Exclusivo { get; set; }
        public bool? JáExecutado { get; set; }
        public bool? Equipamento { get; set; }
        public bool? ReposiçãoDeStock { get; set; }
        public bool? Reclamação { get; set; }
        public string NºRequisiçãoReclamada { get; set; }
        public string ResponsávelCriação { get; set; }
        public string ResponsávelAprovação { get; set; }
        public string ResponsávelValidação { get; set; }
        public string ResponsávelReceção { get; set; }
        public DateTime? DataAprovação { get; set; }
        public DateTime? DataValidação { get; set; }
        public string UnidadeProdutivaAlimentação { get; set; }
        public bool? RequisiçãoNutrição { get; set; }
        public bool? RequisiçãoDetergentes { get; set; }
        public string NºProcedimentoCcp { get; set; }
        public string Aprovadores { get; set; }
        public bool? MercadoLocal { get; set; }
        public string RegiãoMercadoLocal { get; set; }
        public bool? ReparaçãoComGarantia { get; set; }
        public bool? Emm { get; set; }
        public DateTime? DataEntregaArmazém { get; set; }
        public int? LocalDeRecolha { get; set; }
        public string MoradaRecolha { get; set; }
        public string Morada2Recolha { get; set; }
        public string CódigoPostalRecolha { get; set; }
        public string LocalidadeRecolha { get; set; }
        public string ContatoRecolha { get; set; }
        public string ResponsávelReceçãoRecolha { get; set; }
        public int? LocalEntrega { get; set; }
        public string MoradaEntrega { get; set; }
        public string Morada2Entrega { get; set; }
        public string CódigoPostalEntrega { get; set; }
        public string LocalidadeEntrega { get; set; }
        public string ContatoEntrega { get; set; }
        public string ResponsávelReceçãoReceção { get; set; }
        public string NºFatura { get; set; }
        public DateTime? DataMercadoLocal { get; set; }
        public DateTime? DataRequisição { get; set; }
        public string NºConsultaMercado { get; set; }
        public string NºEncomenda { get; set; }
        public bool? Orçamento { get; set; }
        public decimal? ValorEstimado { get; set; }
        public bool? PrecoIvaincluido { get; set; }
        public bool? Adiantamento { get; set; }
        public bool? PedirOrcamento { get; set; }

        public Projetos NºProjetoNavigation { get; set; }
        public Viaturas ViaturaNavigation { get; set; }
        public ICollection<Anexos> Anexos { get; set; }
        public ICollection<DiárioDeProjeto> DiárioDeProjeto { get; set; }
        public ICollection<HistoricoLinhasConsultaMercado> HistoricoLinhasConsultaMercado { get; set; }
        public ICollection<LinhasConsultaMercado> LinhasConsultaMercado { get; set; }
        public ICollection<LinhasPEncomendaProcedimentosCcp> LinhasPEncomendaProcedimentosCcp { get; set; }
        public ICollection<LinhasRequisição> LinhasRequisição { get; set; }
        public ICollection<MovimentosDeProjeto> MovimentosDeProjeto { get; set; }
        public ICollection<PréMovimentosProjeto> PréMovimentosProjeto { get; set; }
        public ICollection<RequisicoesRegAlteracoes> RequisicoesRegAlteracoes { get; set; }
    }
}
