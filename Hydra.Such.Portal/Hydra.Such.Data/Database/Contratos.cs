using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Contratos
    {
        public Contratos()
        {
            LinhasContratos = new HashSet<LinhasContratos>();
        }

        public int TipoContrato { get; set; }
        public string NºDeContrato { get; set; }
        public int NºVersão { get; set; }
        public int? Área { get; set; }
        public string Descrição { get; set; }
        public int? Estado { get; set; }
        public int? EstadoAlteração { get; set; }
        public string NºCliente { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string CódEndereçoEnvio { get; set; }
        public string EnvioANome { get; set; }
        public string EnvioAEndereço { get; set; }
        public string EnvioACódPostal { get; set; }
        public string EnvioALocalidade { get; set; }
        public int? PeríodoFatura { get; set; }
        public DateTime? ÚltimaDataFatura { get; set; }
        public DateTime? PróximaDataFatura { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataExpiração { get; set; }
        public bool? JuntarFaturas { get; set; }
        public string PróximoPeríodoFact { get; set; }
        public bool? LinhasContratoEmFact { get; set; }
        public string CódTermosPagamento { get; set; }
        public int? TipoProposta { get; set; }
        public int? TipoFaturação { get; set; }
        public int? TipoContratoManut { get; set; }
        public string NºRequisiçãoDoCliente { get; set; }
        public DateTime? DataReceçãoRequisição { get; set; }
        public string NºCompromisso { get; set; }
        public decimal? TaxaAprovisionamento { get; set; }
        public decimal? Mc { get; set; }
        public decimal? TaxaDeslocação { get; set; }
        public bool? ContratoAvençaFixa { get; set; }
        public int? ObjetoServiço { get; set; }
        public bool? ContratoAvençaVariável { get; set; }
        public string Notas { get; set; }
        public string NºContrato { get; set; }
        public DateTime? DataInícioContrato { get; set; }
        public DateTime? DataFimContrato { get; set; }
        public string DescriçãoDuraçãoContrato { get; set; }
        public DateTime? DataInício1ºContrato { get; set; }
        public string Referência1ºContrato { get; set; }
        public DateTime? DuraçãoMáxContrato { get; set; }
        public int? RescisãoPrazoAviso { get; set; }
        public int? CondiçõesParaRenovação { get; set; }
        public string CondiçõesRenovaçãoOutra { get; set; }
        public int? CondiçõesPagamento { get; set; }
        public string CondiçõesPagamentoOutra { get; set; }
        public bool? AssinadoPeloCliente { get; set; }
        public bool? Juros { get; set; }
        public DateTime? DataDaAssinatura { get; set; }
        public DateTime? DataEnvioCliente { get; set; }
        public int? UnidadePrestação { get; set; }
        public string ReferênciaContrato { get; set; }
        public decimal? ValorTotalProposta { get; set; }
        public string LocalArquivoFísico { get; set; }
        public string NºOportunidade { get; set; }
        public string NºProposta { get; set; }
        public string NºContato { get; set; }
        public DateTime? DataEstadoProposta { get; set; }
        public int? OrigemDoPedido { get; set; }
        public string DescOrigemDoPedido { get; set; }
        public string NumeraçãoInterna { get; set; }
        public DateTime? DataAlteraçãoProposta { get; set; }
        public DateTime? DataHoraLimiteEsclarecimentos { get; set; }
        public DateTime? DataHoraErrosEOmissões { get; set; }
        public DateTime? DataHoraRelatórioFinal { get; set; }
        public DateTime? DataHoraHabilitaçãoDocumental { get; set; }
        public bool? NºComprimissoObrigatório { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ObjetosDeServiço ObjetoServiçoNavigation { get; set; }
        public ICollection<LinhasContratos> LinhasContratos { get; set; }
    }
}
