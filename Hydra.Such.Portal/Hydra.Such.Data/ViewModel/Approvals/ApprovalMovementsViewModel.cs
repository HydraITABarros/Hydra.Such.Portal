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
        public bool? RequisicaoAcordosPrecos { get; set; }
        public bool? RequisicaoUrgente { get; set; }
        public bool? RequisicaoOrcamentoEmAnexo { get; set; }
        public bool? RequisicaoImobilizado { get; set; }
        public bool? RequisicaoExclusivo { get; set; }
        public bool? RequisicaoJaExecutado { get; set; }
        public bool? RequisicaoAmostra { get; set; }
        public bool? RequisicaoEquipamento { get; set; }
        public bool? RequisicaoReposicaoDeStock { get; set; }
        public bool? RequisicaoPrecoIvaIncluido { get; set; }
        public bool? RequisicaoAdiantamento { get; set; }
        public bool? RequisicaoPedirOrcamento { get; set; }
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
