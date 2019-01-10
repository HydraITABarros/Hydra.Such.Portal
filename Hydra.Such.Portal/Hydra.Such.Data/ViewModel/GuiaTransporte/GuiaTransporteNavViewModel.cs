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
        #region class properties
        public string NoGuiaTransporte { get; set; }
        public string Address { get; set; }
        public string Cidade { get; set; }
        public string City { get; set; }
        public string CodEnvio { get; set; }
        public string CodPais { get; set; }
        public string CodPostal { get; set; }
        public string CodPostalDescarga { get; set; }

        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime DataCarga { get; set; }
        public string DataCargaTxt { get; set; }
        
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime DataDescarga { get; set; }
        public string DataDescargaTxt { get; set; }

        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime DataGuia { get; set; }
        public string DataGuiaTxt { get; set; }

        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime DataObservacoesAdicionais { get; set; }
        public string DataObsAdicionaisTxt { get; set; }

        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime DataSaida { get; set; }
        public string DataSaidaTxt { get; set; }

        public int DimensionSetId { get; set; }
        public string GlobalDimension1Code { get; set; }
        public string GlobalDimension2Code { get; set; }
        public string GlobalDimension3Code { get; set; }
        public int GuiaTransporteInterface { get; set; }
        public bool Historico { get; set; }
        public TimeSpan HoraCarga { get; set; }
        public TimeSpan HoraDescarga { get; set; }
        public TimeSpan HoraObservacoesAdicionais { get; set; }
        public string LocalDescarga { get; set; }
        public string LocalDescarga1 { get; set; }
        public string MaintenanceOrderNo { get; set; }
        public string MoradaCliente { get; set; }
        public string MoradaCliente2 { get; set; }
        public string Name { get; set; }
        public string NifCliente { get; set; }
        public string NoCliente { get; set; }
        public string NoGuiaOriginalInterface { get; set; }
        public string NomeCliente { get; set; }
        public string NomeCliente2 { get; set; }
        public string NoProjecto { get; set; }
        public string NoRequisicao { get; set; }
        public string Observacoes { get; set; }
        public string ObservacoesAdicionais { get; set; }
        public string OrdemTransferencia { get; set; }
        public string Origem { get; set; }
        public string PaisCarga { get; set; }
        public string PaisDescarga { get; set; }
        public decimal PesoTotal { get; set; }
        public string PostCode { get; set; }
        public decimal QuantidadeTotal { get; set; }
        public string ReportedBy { get; set; }
        public string Requisicao { get; set; }
        public string NoSolicitacao { get; set; }

        public string ResponsabilityCenter { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime ShipmentStartDate { get; set; }
        public string ShipmentStartDateTxt { get; set; }

        public TimeSpan ShipmentStartTime { get; set; }
        public string SourceCode { get; set; }
        public string Telefone { get; set; }
        public int Tipo { get; set; }
        public string TipoDescription { get; set; }
        public string UserObservacoesAdicionai { get; set; }
        public string Utilizador { get; set; }
        public string VATRegistrationNo { get; set; }

        public string Viatura { get; set; }

        public List<LinhaGuiaTransporteNavViewModel> LinhasGuiaTransporte { get; set; }
        public FiscalAuthorityCommunicationLog FiscalCommunicationLog { get; set; }
        #endregion

        #region class methods
        private string DateToText(DateTime dateToCheck)
        {
            try
            {
                return dateToCheck.CompareTo(DateTime.Parse("1900-01-01")) == 0 || dateToCheck.CompareTo(DateTime.Parse("1753-01-01")) == 0 ? string.Empty : dateToCheck.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {

                return string.Empty;
            }
             
        }

        private DateTime TextToDateTime(string textToCheck)
        {
            try
            {
                return string.IsNullOrWhiteSpace(textToCheck) ? DateTime.Parse("1900-01-01") : DateTime.Parse(textToCheck);
            }
            catch (Exception ex)
            {
                return DateTime.Parse("1900-01-01");
            }
        }

        public void CastDateTimePropertiesToString()
        {
            DataCargaTxt = DateToText(DataCarga);
            DataDescargaTxt = DateToText(DataDescarga);
            DataGuiaTxt = DateToText(DataGuia);
            DataObsAdicionaisTxt = DateToText(DataObservacoesAdicionais);
            DataSaidaTxt = DateToText(DataSaida);
            ShipmentStartDateTxt = DateToText(ShipmentStartDate);
        }

        public void CastDateTimeStringPropertiesToDateTime()
        {
            DataCarga = TextToDateTime(DataCargaTxt);
            DataDescarga = TextToDateTime(DataDescargaTxt);
            DataGuia = TextToDateTime(DataGuiaTxt);
            DataObservacoesAdicionais = TextToDateTime(DataObsAdicionaisTxt);
            DataSaida = TextToDateTime(DataSaidaTxt);
            ShipmentStartDate = TextToDateTime(ShipmentStartDateTxt);
        }
        #endregion
        

    }
}
