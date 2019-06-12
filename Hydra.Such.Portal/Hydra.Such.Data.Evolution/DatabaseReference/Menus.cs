using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class Menus
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Formulario { get; set; }
        public int? NivelAcesso { get; set; }
        public bool? AcessoAdministracao { get; set; }
        public int? AcessoTotal { get; set; }
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
        public int? Emms { get; set; }
    }
}
