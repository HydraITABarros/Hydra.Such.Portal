using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class PermissoesDefault
    {
        public int NivelAcesso { get; set; }
        public bool? AcessoAdministracao { get; set; }
        public int? ModuloFolhaObra { get; set; }
        public int? ModuloFichaEquip { get; set; }
        public int? ModuloRequisicoes { get; set; }
        public int? ModuloRegistoDiario { get; set; }
        public int? ModuloContratos { get; set; }
        public int? ModuloPlaneamento { get; set; }
        public bool? ValidadorEquipamento { get; set; }
        public int? ModuloClientes { get; set; }
        public int? ModuloFornecedores { get; set; }
        public int? ModuloInstituicoes { get; set; }
        public int? ModuloServicos { get; set; }
        public int? ModuloDadosEstatisticos { get; set; }
        public int? ModuloHabilitacoes { get; set; }
        public int? ModuloFormacoesCompetencias { get; set; }
        public int? ModuloUtilizadores { get; set; }
        public int? ModuloReplicarPlaneamento { get; set; }
        public int? ModuloEmm { get; set; }
        public int? ModuloSolicitacoes { get; set; }

        public Acessos ModuloClientesNavigation { get; set; }
        public Acessos ModuloContratosNavigation { get; set; }
        public Acessos ModuloDadosEstatisticosNavigation { get; set; }
        public Acessos ModuloEmmNavigation { get; set; }
        public Acessos ModuloFichaEquipNavigation { get; set; }
        public Acessos ModuloFolhaObraNavigation { get; set; }
        public Acessos ModuloFormacoesCompetenciasNavigation { get; set; }
        public Acessos ModuloFornecedoresNavigation { get; set; }
        public Acessos ModuloHabilitacoesNavigation { get; set; }
        public Acessos ModuloInstituicoesNavigation { get; set; }
        public Acessos ModuloPlaneamentoNavigation { get; set; }
        public Acessos ModuloRegistoDiarioNavigation { get; set; }
        public Acessos ModuloReplicarPlaneamentoNavigation { get; set; }
        public Acessos ModuloRequisicoesNavigation { get; set; }
        public Acessos ModuloServicosNavigation { get; set; }
        public Acessos ModuloSolicitacoesNavigation { get; set; }
        public Acessos ModuloUtilizadoresNavigation { get; set; }
        public NivelAcessoTipo NivelAcessoNavigation { get; set; }
    }
}
