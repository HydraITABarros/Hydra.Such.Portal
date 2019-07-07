using Hydra.Such.Data.Evolution.DatabaseReference;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Hydra.Such.Portal.ViewModels
{
    public class MaintenanceOrderViewModel
    {
        [NotMapped]
        public bool IsToExecute
        {
            get { return (this.Status == 0); }
        }

        [NotMapped]
        public bool isPreventive;

        [NotMapped]
        public List<Utilizador> Technicals;

        [NotMapped]
        public string ClientName { get; set; }

        [NotMapped]
        public string InstitutionName { get; set; }

        [NotMapped]
        public string ServiceName { get; set; }

        [NotMapped]
        public string havePreventive { get; set; }



        [Key]
        public string No { get; set; }
        public string Description { get; set; }
        public string OrderType { get; set; }
        public string ContractNo { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public int? Priority { get; set; }
        public int? Status { get; set; }
        public DateTime? PostingDate { get; set; }
        public string ShortcutDimension1Code { get; set; }
        public string ShortcutDimension2Code { get; set; }
        public string ResponsibleEmployee { get; set; }
        public string EnteredBy { get; set; }
        public string MaintenanceResponsible { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DocumentDate { get; set; }
        public DateTime? ExpectedFinishingDate { get; set; }
        public DateTime? ExpectedStartingDate { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? FinishingDate { get; set; }
        public DateTime? FinishingTime { get; set; }
        public string NoSeries { get; set; }
        public string TipoContactoCliente { get; set; }
        public string JobPostingGroup { get; set; }
        public string ShortcutDimension3Code { get; set; }
        public string ShortcutDimension4Code { get; set; }
        public int? Urgência { get; set; }
        public int? PrioridadeObra { get; set; }
        public DateTime? DataReabertura { get; set; }
        public DateTime? DataPedidoReparação { get; set; }
        public string FechadoPor { get; set; }
        public string ReabertoPor { get; set; }
        public string ConfigResponsavel { get; set; }
        public string UserChefeProjecto { get; set; }
        public string UserResponsavel { get; set; }
        public DateTime? DataResponsavel { get; set; }
        public int? IdClienteEvolution { get; set; }
        public int? IdInstituicaoEvolution { get; set; }
        public int? IdServicoEvolution { get; set; }
        public int? IdTecnico1 { get; set; }
        public int? IdTecnico2 { get; set; }
        public int? IdTecnico3 { get; set; }
        public int? IdTecnico4 { get; set; }
        public int? IdTecnico5 { get; set; }
    }
}
