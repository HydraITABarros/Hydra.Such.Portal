using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
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

        public virtual Acessos ModuloClientesNavigation { get; set; }
        public virtual Acessos ModuloContratosNavigation { get; set; }
        public virtual Acessos ModuloDadosEstatisticosNavigation { get; set; }
        public virtual Acessos ModuloEmmNavigation { get; set; }
        public virtual Acessos ModuloFichaEquipNavigation { get; set; }
        public virtual Acessos ModuloFolhaObraNavigation { get; set; }
        public virtual Acessos ModuloFormacoesCompetenciasNavigation { get; set; }
        public virtual Acessos ModuloFornecedoresNavigation { get; set; }
        public virtual Acessos ModuloHabilitacoesNavigation { get; set; }
        public virtual Acessos ModuloInstituicoesNavigation { get; set; }
        public virtual Acessos ModuloPlaneamentoNavigation { get; set; }
        public virtual Acessos ModuloRegistoDiarioNavigation { get; set; }
        public virtual Acessos ModuloReplicarPlaneamentoNavigation { get; set; }
        public virtual Acessos ModuloRequisicoesNavigation { get; set; }
        public virtual Acessos ModuloServicosNavigation { get; set; }
        public virtual Acessos ModuloSolicitacoesNavigation { get; set; }
        public virtual Acessos ModuloUtilizadoresNavigation { get; set; }
        public virtual NivelAcessoTipo NivelAcessoNavigation { get; set; }
    }
}
