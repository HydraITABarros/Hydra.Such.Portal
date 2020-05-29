using System;
using System.Collections.Generic;
using System.Text;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.ViewModel.Projects
{
    public class ProjectListItemViewModel
    {
        public string ProjectNo { get; set; }
        public DateTime? Date { get; set; }
        public String DateText { get; set; }
        public EstadoProjecto? Status { get; set; }
        public string StatusDescription { get; set; }
        public string Description { get; set; }
        public string ClientNo { get; set; }
        public string ClientName { get; set; }
        public string ClientRegionCode { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        public string ContractoNo { get; set; }
        public int? ProjectTypeCode { get; set; }
        public string ProjectTypeDescription { get; set; }
        public string MovimentosVenda { get; set; }
        public string DescricaoDetalhada { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
