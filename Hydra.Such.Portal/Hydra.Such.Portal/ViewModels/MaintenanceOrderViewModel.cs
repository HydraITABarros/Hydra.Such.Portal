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
        public bool isPreventive;
        public List<Utilizador> Technicals;
        public string ClientName { get; set; }
        public string InstitutionName { get; set; }
        public string ServiceName { get; set; }
        public string havePreventive { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public string OrderType { get; set; }
        public string ContractNo { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public int? Status { get; set; }
        public string ResponsibleEmployee { get; set; }
        public string MaintenanceResponsible { get; set; }
        public DateTime? OrderDate { get; set; }
        public string NoSeries { get; set; }
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
