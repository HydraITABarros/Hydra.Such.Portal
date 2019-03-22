using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Acessos
    {
        public Acessos()
        {
            PermissoesDefaultModuloClientesNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloContratosNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloDadosEstatisticosNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloEmmNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloFichaEquipNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloFolhaObraNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloFormacoesCompetenciasNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloFornecedoresNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloHabilitacoesNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloInstituicoesNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloPlaneamentoNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloRegistoDiarioNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloReplicarPlaneamentoNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloRequisicoesNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloServicosNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloSolicitacoesNavigation = new HashSet<PermissoesDefault>();
            PermissoesDefaultModuloUtilizadoresNavigation = new HashSet<PermissoesDefault>();
        }

        public int IdAcesso { get; set; }
        public string Nome { get; set; }

        public ICollection<PermissoesDefault> PermissoesDefaultModuloClientesNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloContratosNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloDadosEstatisticosNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloEmmNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloFichaEquipNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloFolhaObraNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloFormacoesCompetenciasNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloFornecedoresNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloHabilitacoesNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloInstituicoesNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloPlaneamentoNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloRegistoDiarioNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloReplicarPlaneamentoNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloRequisicoesNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloServicosNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloSolicitacoesNavigation { get; set; }
        public ICollection<PermissoesDefault> PermissoesDefaultModuloUtilizadoresNavigation { get; set; }
    }
}
