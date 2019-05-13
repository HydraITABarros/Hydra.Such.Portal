using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Solicitacoes
    {
        public int IdSolicitacao { get; set; }
        public int? IdCliente { get; set; }
        public string IdContrato { get; set; }
        public int? IdInstituicao { get; set; }
        public int? IdServico { get; set; }
        public string ContactoNome { get; set; }
        public string ContactoEmail { get; set; }
        public string ContactoTelefone { get; set; }
        public string ContactoFax { get; set; }
        public DateTime? DataPedido { get; set; }
        public string TipoSolicitacao { get; set; }
        public int? Prioridade { get; set; }
        public string ReferenciaCliente { get; set; }
        public string Localizacao { get; set; }
        public int? IdEquipamento { get; set; }
        public string DescricaoAnomalia { get; set; }
        public int? IdResponsavelTriagem { get; set; }
        public int? Estado { get; set; }
        public string EstadoAnotacao { get; set; }
        public string OmGerada { get; set; }
        public string OrcamentoGerado { get; set; }
        public int? IdUserCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string TipoEquipamento { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string NumSerie { get; set; }
        public string NumInventario { get; set; }
        public int? Equipa { get; set; }
        public int? IdContratoLinha { get; set; }
        public bool Prevencao { get; set; }
        public bool Orcamentar { get; set; }
        public bool ManutencaoPreventiva { get; set; }
        public DateTime? DataFecho { get; set; }
    }
}
