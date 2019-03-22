using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class ProjectosAutorizadosFact
    {
        public byte[] Timestamp { get; set; }
        public string No { get; set; }
        public int GrupoFactura { get; set; }
        public string SearchDescription { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string BillToCustomerNo { get; set; }
        public int Status { get; set; }
        public string GlobalDimension1Code { get; set; }
        public string GlobalDimension2Code { get; set; }
        public string JobPostingGroup { get; set; }
        public string NoSeries { get; set; }
        public byte Facturável { get; set; }
        public string ContractNo { get; set; }
        public string Motorista { get; set; }
        public string AreaFilter { get; set; }
        public string ShipToCode { get; set; }
        public int TipoProjecto { get; set; }
        public byte OnlyForMaintInvoicing { get; set; }
        public string ShortcutDimension3Code { get; set; }
        public string ShortcutDimension4Code { get; set; }
        public string Utilizador { get; set; }
        public DateTime DataAutorização { get; set; }
        public string DataServPrestado { get; set; }
        public string Observações { get; set; }
        public string Observações1 { get; set; }
        public int FiltroArea { get; set; }
        public string TipoGrupoContabProjecto { get; set; }
        public string TipoGrupoContabOmProjecto { get; set; }
        public string PedidoDoCliente { get; set; }
        public int Opção { get; set; }
        public DateTime DataPedido { get; set; }
        public string DescriçãoGrupo { get; set; }
        public string PaymentTermsCode { get; set; }
        public string Diversos { get; set; }
        public string NoCompromisso { get; set; }
        public string SituaçõesPendentes { get; set; }
        public DateTime DataPrestacaoServico { get; set; }
        public string PaymentMethodCode { get; set; }
    }
}
