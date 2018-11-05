using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hydra.Such.Data.ViewModel.GuiaTransporte
{
    public class GuiaTransporteNavViewModel
    {
        public string NoGuiaTransporte { get; set; }
        public int Tipo { get; set; }
        public string NoCliente { get; set; }
        public string CodEnvio { get; set; }
        public string NomeCliente { get; set; }
        public string NomeCliente2 { get; set; }
        public string Cidade { get; set; }
        public string CodPostal { get; set; }
        public string NifCliente { get; set; }
        public string SourceCode { get; set; }
        public string NoRequisicao { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "MM-dd-yyyy")]
        public DateTime DataGuia { get; set; }

        public DateTime DataSaida { get; set; }

        public string ReportedBy { get; set; }
        public string NoProjecto { get; set; }
        public string OrdemTransferencia { get; set; }

        public string Observacoes { get; set; }
        public string Origem { get; set; }
        public string ResponsabilityCenter { get; set; }
        public decimal QuantidadeTotal{ get; set;}
        public decimal PesoTotal { get; set; }

        public string Utilizador { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }

        public TimeSpan HoraCarga { get; set; }
        public DateTime DataCarga { get; set; }
        public string PaisCarga { get; set; }
        public string LocalDescarga { get; set; }
        public string LocalDescarga1 { get; set; }
        public string CodPostalDescarga { get; set; }
        public TimeSpan HoraDescarga { get; set; }
        public DateTime DataDescarga { get; set; }
        public string Viatura { get; set; }
        public string PaisDescarga { get; set; }
        public string GlobalDimension1Code { get; set; }
        public string GlobalDimension2Code { get; set; }
        public string GlobalDimension3Code { get; set; }
        public string NoGuiaOriginalInterface { get; set; }
        public int GuiaTransporteInterface { get; set; }
        public int DimensionSetId { get; set; }
        public DateTime ShipmentStartDate { get; set; }
        public bool Historico { get; set; }
        public string CodPais { get; set; }
        public string Telefone { get; set; }
        public string MaintenanceOrderNo { get; set; }

        public ICollection<LinhaGuiaTransporteNavViewModel> LinhasGuiaTransporte { get; set; }

    }
}
