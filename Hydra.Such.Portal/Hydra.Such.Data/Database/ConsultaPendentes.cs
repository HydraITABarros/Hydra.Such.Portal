using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ConsultaPendentes
    {
        public string IdUser { get; set; }
        public int TipoDocumento { get; set; }
        public string NoDocumento { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public DateTime? DataValidacao { get; set; }
        public string RegistoMercadoLocal { get; set; }
        public string ConsultaMercado { get; set; }
        public string Requisicao { get; set; }
        public string UserAprovacao { get; set; }
        public string UserValidacao { get; set; }
        public string EstadoRequisicao { get; set; }
        public string UserCriacao { get; set; }
        public string Regiao { get; set; }
        public string Area { get; set; }
        public string CentroResponsabilidade { get; set; }
        public string Cliente { get; set; }
        public string NomeCliente { get; set; }
        public string NomeCliente2 { get; set; }
        public string Fornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public string NomeFornecedor2 { get; set; }
        public string NoProjeto { get; set; }
        public DateTime? DataRespostaFornecedor { get; set; }
        public DateTime? DataEnvioArea { get; set; }
        public DateTime? DataRececaoArea { get; set; }
        public DateTime? DataEnvioMercadoLocal { get; set; }
        public DateTime? DataDisponibilizado { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public string NoEncomenda { get; set; }
        public int? Encomendas { get; set; }
        public int? ConsultasMercado { get; set; }
        public int? EncomendasEmHistorico { get; set; }
        public int? ConsultasEmHistorico { get; set; }
        public DateTime? DataRececaoEsperada { get; set; }
        public DateTime? DataEntregaArmazem { get; set; }
        public DateTime? DataConsultaMercado { get; set; }
        public bool? FornecedorBloqueado { get; set; }
        public DateTime? DataEncomenda { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public DateTime? DataHoraAlteracao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
