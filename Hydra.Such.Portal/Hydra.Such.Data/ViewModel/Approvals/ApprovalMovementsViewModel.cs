using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Approvals
{
    public class ApprovalMovementsViewModel
    {

        public int MovementNo { get; set; }
        public int? Type { get; set; }
        public string TypeText { get; set; }
        public int? Area { get; set; }
        public string AreaText { get; set; }
        public string FunctionalArea { get; set; }
        public string FunctionalAreaText { get; set; }
        public string ResponsabilityCenter { get; set; }
        public string ResponsabilityCenterText { get; set; }
        public string Region { get; set; }
        public string RegionText { get; set; }
        public string Number { get; set; }
        public string NumberLink { get; set; }
        public string RequisicaoProjectNo { get; set; }
        public string RequisicaoClientNo { get; set; }
        public string RequisicaoClientName { get; set; }
        public string RequisicaoAcordosPrecos { get; set; }
        public string RequisicaoUrgente { get; set; }
        public string RequisicaoOrcamentoEmAnexo { get; set; }
        public string RequisicaoImobilizado { get; set; }
        public string RequisicaoExclusivo { get; set; }
        public string RequisicaoJaExecutado { get; set; }
        public string RequisicaoAmostra { get; set; }
        public string RequisicaoEquipamento { get; set; }
        public string RequisicaoReposicaoDeStock { get; set; }
        public string RequisicaoPrecoIvaIncluido { get; set; }
        public string RequisicaoAdiantamento { get; set; }
        public string RequisicaoPedirOrcamento { get; set; }
        public string RequisicaoRoupaManutencao { get; set; }
        public string RequestUser { get; set; }
        public decimal? Value { get; set; }
        public DateTime? DateTimeApprove { get; set; }
        public DateTime? DateTimeCreate { get; set; }
        public string UserCreate { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string UserUpdate { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public string ReproveReason { get; set; }
        public int Level { get; set; }
        public string FHDatePartida { get; set; }
        public string FHDateChegada { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
