//------------------------------------------------------------------------------
// <gerado automaticamente>
//     Esse código foi gerado por uma ferramenta.
//     //
//     As alterações no arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </gerado automaticamente>
//------------------------------------------------------------------------------

namespace WSCreatePreInvoiceLine
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", ConfigurationName="WSCreatePreInvoiceLine.WsPreInvoiceLine_Port")]
    public interface WsPreInvoiceLine_Port
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/page/wspreinvoiceline:Read", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.Read_Result> ReadAsync(WSCreatePreInvoiceLine.Read request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/page/wspreinvoiceline:ReadByRecId", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.ReadByRecId_Result> ReadByRecIdAsync(WSCreatePreInvoiceLine.ReadByRecId request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/page/wspreinvoiceline:ReadMultiple", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.ReadMultiple_Result> ReadMultipleAsync(WSCreatePreInvoiceLine.ReadMultiple request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/page/wspreinvoiceline:IsUpdated", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.IsUpdated_Result> IsUpdatedAsync(WSCreatePreInvoiceLine.IsUpdated request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/page/wspreinvoiceline:GetRecIdFromKey", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.GetRecIdFromKey_Result> GetRecIdFromKeyAsync(WSCreatePreInvoiceLine.GetRecIdFromKey request);
        
        // CODEGEN: Gerando contrato de mensagem porque a operação tem vários valores retornados.
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/page/wspreinvoiceline:Create", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.Create_Result> CreateAsync(WSCreatePreInvoiceLine.Create request);
        
        // CODEGEN: Gerando contrato de mensagem porque a operação tem vários valores retornados.
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/page/wspreinvoiceline:CreateMultiple", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.CreateMultiple_Result> CreateMultipleAsync(WSCreatePreInvoiceLine.CreateMultiple request);
        
        // CODEGEN: Gerando contrato de mensagem porque a operação tem vários valores retornados.
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/page/wspreinvoiceline:Update", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.Update_Result> UpdateAsync(WSCreatePreInvoiceLine.Update request);
        
        // CODEGEN: Gerando contrato de mensagem porque a operação tem vários valores retornados.
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/page/wspreinvoiceline:UpdateMultiple", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.UpdateMultiple_Result> UpdateMultipleAsync(WSCreatePreInvoiceLine.UpdateMultiple request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/page/wspreinvoiceline:Delete", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.Delete_Result> DeleteAsync(WSCreatePreInvoiceLine.Delete request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline")]
    public partial class WsPreInvoiceLine
    {
        
        private string keyField;
        
        private string sell_to_Customer_NoField;
        
        private string document_NoField;
        
        private Document_Type document_TypeField;
        
        private bool document_TypeFieldSpecified;
        
        private int line_NoField;
        
        private bool line_NoFieldSpecified;
        
        private Type typeField;
        
        private bool typeFieldSpecified;
        
        private string noField;
        
        private string location_CodeField;
        
        private string posting_GroupField;
        
        private System.DateTime shipment_DateField;
        
        private bool shipment_DateFieldSpecified;
        
        private string descriptionField;
        
        private string description_2Field;
        
        private string unit_of_MeasureField;
        
        private decimal quantityField;
        
        private bool quantityFieldSpecified;
        
        private decimal outstanding_QuantityField;
        
        private bool outstanding_QuantityFieldSpecified;
        
        private decimal qty_to_InvoiceField;
        
        private bool qty_to_InvoiceFieldSpecified;
        
        private decimal qty_to_ShipField;
        
        private bool qty_to_ShipFieldSpecified;
        
        private decimal unit_PriceField;
        
        private bool unit_PriceFieldSpecified;
        
        private decimal unit_Cost_LCYField;
        
        private bool unit_Cost_LCYFieldSpecified;
        
        private decimal vAT_PercentField;
        
        private bool vAT_PercentFieldSpecified;
        
        private decimal line_Discount_PercentField;
        
        private bool line_Discount_PercentFieldSpecified;
        
        private decimal line_Discount_AmountField;
        
        private bool line_Discount_AmountFieldSpecified;
        
        private decimal amountField;
        
        private bool amountFieldSpecified;
        
        private decimal amount_Including_VATField;
        
        private bool amount_Including_VATFieldSpecified;
        
        private bool allow_Invoice_DiscField;
        
        private bool allow_Invoice_DiscFieldSpecified;
        
        private string job_NoField;
        
        private int job_Journal_Line_No_PortalField;
        
        private bool job_Journal_Line_No_PortalFieldSpecified;
        
        private string regionCode20Field;
        
        private string functionAreaCode20Field;
        
        private string responsabilityCenterCode20Field;
        
        private string contract_No_PortalField;
        
        private string meal_Type_DescriptionField;
        
        private string service_Group_DescriptionField;
        
        private string waste_Shipment_NoField;
        
        private string external_Shipment_NoField;
        
        private System.DateTime consumption_DateField;
        
        private bool consumption_DateFieldSpecified;
        
        private string service_Contract_NoField;
        
        private int nº_Linha_OMField;
        
        private bool nº_Linha_OMFieldSpecified;
        
        private string nº_Objecto_RefField;
        
        private string nº_Cliente_OMField;
        
        private string nº_Guia_Resíduos_GARField;
        
        private string nº_Guia_ExternaField;
        
        private string tipo_RefeicaoField;
        
        private bool factura_de_ContratoField;
        
        private bool factura_de_ContratoFieldSpecified;
        
        private bool factura_CAFField;
        
        private bool factura_CAFFieldSpecified;
        
        private string nº_Objecto_Ref_2Field;
        
        private Tipo_Objecto_Ref tipo_Objecto_RefField;
        
        private bool tipo_Objecto_RefFieldSpecified;
        
        private string grupo_ServiçoField;
        
        private string cod_Serv_ClienteField;
        
        private string des_Serv_ClienteField;
        
        private Tipo_Recurso tipo_RecursoField;
        
        private bool tipo_RecursoFieldSpecified;
        
        private System.DateTime data_Registo_DiarioField;
        
        private bool data_Registo_DiarioFieldSpecified;
        
        private string contract_NoField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Sell_to_Customer_No
        {
            get
            {
                return this.sell_to_Customer_NoField;
            }
            set
            {
                this.sell_to_Customer_NoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Document_No
        {
            get
            {
                return this.document_NoField;
            }
            set
            {
                this.document_NoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public Document_Type Document_Type
        {
            get
            {
                return this.document_TypeField;
            }
            set
            {
                this.document_TypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Document_TypeSpecified
        {
            get
            {
                return this.document_TypeFieldSpecified;
            }
            set
            {
                this.document_TypeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int Line_No
        {
            get
            {
                return this.line_NoField;
            }
            set
            {
                this.line_NoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Line_NoSpecified
        {
            get
            {
                return this.line_NoFieldSpecified;
            }
            set
            {
                this.line_NoFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public Type Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TypeSpecified
        {
            get
            {
                return this.typeFieldSpecified;
            }
            set
            {
                this.typeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string No
        {
            get
            {
                return this.noField;
            }
            set
            {
                this.noField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string Location_Code
        {
            get
            {
                return this.location_CodeField;
            }
            set
            {
                this.location_CodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string Posting_Group
        {
            get
            {
                return this.posting_GroupField;
            }
            set
            {
                this.posting_GroupField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", Order=9)]
        public System.DateTime Shipment_Date
        {
            get
            {
                return this.shipment_DateField;
            }
            set
            {
                this.shipment_DateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Shipment_DateSpecified
        {
            get
            {
                return this.shipment_DateFieldSpecified;
            }
            set
            {
                this.shipment_DateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public string Description_2
        {
            get
            {
                return this.description_2Field;
            }
            set
            {
                this.description_2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public string Unit_of_Measure
        {
            get
            {
                return this.unit_of_MeasureField;
            }
            set
            {
                this.unit_of_MeasureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public decimal Quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool QuantitySpecified
        {
            get
            {
                return this.quantityFieldSpecified;
            }
            set
            {
                this.quantityFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public decimal Outstanding_Quantity
        {
            get
            {
                return this.outstanding_QuantityField;
            }
            set
            {
                this.outstanding_QuantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Outstanding_QuantitySpecified
        {
            get
            {
                return this.outstanding_QuantityFieldSpecified;
            }
            set
            {
                this.outstanding_QuantityFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=15)]
        public decimal Qty_to_Invoice
        {
            get
            {
                return this.qty_to_InvoiceField;
            }
            set
            {
                this.qty_to_InvoiceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Qty_to_InvoiceSpecified
        {
            get
            {
                return this.qty_to_InvoiceFieldSpecified;
            }
            set
            {
                this.qty_to_InvoiceFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=16)]
        public decimal Qty_to_Ship
        {
            get
            {
                return this.qty_to_ShipField;
            }
            set
            {
                this.qty_to_ShipField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Qty_to_ShipSpecified
        {
            get
            {
                return this.qty_to_ShipFieldSpecified;
            }
            set
            {
                this.qty_to_ShipFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=17)]
        public decimal Unit_Price
        {
            get
            {
                return this.unit_PriceField;
            }
            set
            {
                this.unit_PriceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Unit_PriceSpecified
        {
            get
            {
                return this.unit_PriceFieldSpecified;
            }
            set
            {
                this.unit_PriceFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=18)]
        public decimal Unit_Cost_LCY
        {
            get
            {
                return this.unit_Cost_LCYField;
            }
            set
            {
                this.unit_Cost_LCYField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Unit_Cost_LCYSpecified
        {
            get
            {
                return this.unit_Cost_LCYFieldSpecified;
            }
            set
            {
                this.unit_Cost_LCYFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=19)]
        public decimal VAT_Percent
        {
            get
            {
                return this.vAT_PercentField;
            }
            set
            {
                this.vAT_PercentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VAT_PercentSpecified
        {
            get
            {
                return this.vAT_PercentFieldSpecified;
            }
            set
            {
                this.vAT_PercentFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=20)]
        public decimal Line_Discount_Percent
        {
            get
            {
                return this.line_Discount_PercentField;
            }
            set
            {
                this.line_Discount_PercentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Line_Discount_PercentSpecified
        {
            get
            {
                return this.line_Discount_PercentFieldSpecified;
            }
            set
            {
                this.line_Discount_PercentFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=21)]
        public decimal Line_Discount_Amount
        {
            get
            {
                return this.line_Discount_AmountField;
            }
            set
            {
                this.line_Discount_AmountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Line_Discount_AmountSpecified
        {
            get
            {
                return this.line_Discount_AmountFieldSpecified;
            }
            set
            {
                this.line_Discount_AmountFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=22)]
        public decimal Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AmountSpecified
        {
            get
            {
                return this.amountFieldSpecified;
            }
            set
            {
                this.amountFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=23)]
        public decimal Amount_Including_VAT
        {
            get
            {
                return this.amount_Including_VATField;
            }
            set
            {
                this.amount_Including_VATField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Amount_Including_VATSpecified
        {
            get
            {
                return this.amount_Including_VATFieldSpecified;
            }
            set
            {
                this.amount_Including_VATFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=24)]
        public bool Allow_Invoice_Disc
        {
            get
            {
                return this.allow_Invoice_DiscField;
            }
            set
            {
                this.allow_Invoice_DiscField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Allow_Invoice_DiscSpecified
        {
            get
            {
                return this.allow_Invoice_DiscFieldSpecified;
            }
            set
            {
                this.allow_Invoice_DiscFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=25)]
        public string Job_No
        {
            get
            {
                return this.job_NoField;
            }
            set
            {
                this.job_NoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=26)]
        public int Job_Journal_Line_No_Portal
        {
            get
            {
                return this.job_Journal_Line_No_PortalField;
            }
            set
            {
                this.job_Journal_Line_No_PortalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Job_Journal_Line_No_PortalSpecified
        {
            get
            {
                return this.job_Journal_Line_No_PortalFieldSpecified;
            }
            set
            {
                this.job_Journal_Line_No_PortalFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=27)]
        public string RegionCode20
        {
            get
            {
                return this.regionCode20Field;
            }
            set
            {
                this.regionCode20Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=28)]
        public string FunctionAreaCode20
        {
            get
            {
                return this.functionAreaCode20Field;
            }
            set
            {
                this.functionAreaCode20Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=29)]
        public string ResponsabilityCenterCode20
        {
            get
            {
                return this.responsabilityCenterCode20Field;
            }
            set
            {
                this.responsabilityCenterCode20Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=30)]
        public string Contract_No_Portal
        {
            get
            {
                return this.contract_No_PortalField;
            }
            set
            {
                this.contract_No_PortalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=31)]
        public string Meal_Type_Description
        {
            get
            {
                return this.meal_Type_DescriptionField;
            }
            set
            {
                this.meal_Type_DescriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=32)]
        public string Service_Group_Description
        {
            get
            {
                return this.service_Group_DescriptionField;
            }
            set
            {
                this.service_Group_DescriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=33)]
        public string Waste_Shipment_No
        {
            get
            {
                return this.waste_Shipment_NoField;
            }
            set
            {
                this.waste_Shipment_NoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=34)]
        public string External_Shipment_No
        {
            get
            {
                return this.external_Shipment_NoField;
            }
            set
            {
                this.external_Shipment_NoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", Order=35)]
        public System.DateTime Consumption_Date
        {
            get
            {
                return this.consumption_DateField;
            }
            set
            {
                this.consumption_DateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Consumption_DateSpecified
        {
            get
            {
                return this.consumption_DateFieldSpecified;
            }
            set
            {
                this.consumption_DateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=36)]
        public string Service_Contract_No
        {
            get
            {
                return this.service_Contract_NoField;
            }
            set
            {
                this.service_Contract_NoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=37)]
        public int Nº_Linha_OM
        {
            get
            {
                return this.nº_Linha_OMField;
            }
            set
            {
                this.nº_Linha_OMField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Nº_Linha_OMSpecified
        {
            get
            {
                return this.nº_Linha_OMFieldSpecified;
            }
            set
            {
                this.nº_Linha_OMFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=38)]
        public string Nº_Objecto_Ref
        {
            get
            {
                return this.nº_Objecto_RefField;
            }
            set
            {
                this.nº_Objecto_RefField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=39)]
        public string Nº_Cliente_OM
        {
            get
            {
                return this.nº_Cliente_OMField;
            }
            set
            {
                this.nº_Cliente_OMField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=40)]
        public string Nº_Guia_Resíduos_GAR
        {
            get
            {
                return this.nº_Guia_Resíduos_GARField;
            }
            set
            {
                this.nº_Guia_Resíduos_GARField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=41)]
        public string Nº_Guia_Externa
        {
            get
            {
                return this.nº_Guia_ExternaField;
            }
            set
            {
                this.nº_Guia_ExternaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=42)]
        public string Tipo_Refeicao
        {
            get
            {
                return this.tipo_RefeicaoField;
            }
            set
            {
                this.tipo_RefeicaoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=43)]
        public bool Factura_de_Contrato
        {
            get
            {
                return this.factura_de_ContratoField;
            }
            set
            {
                this.factura_de_ContratoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Factura_de_ContratoSpecified
        {
            get
            {
                return this.factura_de_ContratoFieldSpecified;
            }
            set
            {
                this.factura_de_ContratoFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=44)]
        public bool Factura_CAF
        {
            get
            {
                return this.factura_CAFField;
            }
            set
            {
                this.factura_CAFField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Factura_CAFSpecified
        {
            get
            {
                return this.factura_CAFFieldSpecified;
            }
            set
            {
                this.factura_CAFFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=45)]
        public string Nº_Objecto_Ref_2
        {
            get
            {
                return this.nº_Objecto_Ref_2Field;
            }
            set
            {
                this.nº_Objecto_Ref_2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=46)]
        public Tipo_Objecto_Ref Tipo_Objecto_Ref
        {
            get
            {
                return this.tipo_Objecto_RefField;
            }
            set
            {
                this.tipo_Objecto_RefField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Tipo_Objecto_RefSpecified
        {
            get
            {
                return this.tipo_Objecto_RefFieldSpecified;
            }
            set
            {
                this.tipo_Objecto_RefFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=47)]
        public string Grupo_Serviço
        {
            get
            {
                return this.grupo_ServiçoField;
            }
            set
            {
                this.grupo_ServiçoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=48)]
        public string Cod_Serv_Cliente
        {
            get
            {
                return this.cod_Serv_ClienteField;
            }
            set
            {
                this.cod_Serv_ClienteField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=49)]
        public string Des_Serv_Cliente
        {
            get
            {
                return this.des_Serv_ClienteField;
            }
            set
            {
                this.des_Serv_ClienteField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=50)]
        public Tipo_Recurso Tipo_Recurso
        {
            get
            {
                return this.tipo_RecursoField;
            }
            set
            {
                this.tipo_RecursoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Tipo_RecursoSpecified
        {
            get
            {
                return this.tipo_RecursoFieldSpecified;
            }
            set
            {
                this.tipo_RecursoFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", Order=51)]
        public System.DateTime Data_Registo_Diario
        {
            get
            {
                return this.data_Registo_DiarioField;
            }
            set
            {
                this.data_Registo_DiarioField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Data_Registo_DiarioSpecified
        {
            get
            {
                return this.data_Registo_DiarioFieldSpecified;
            }
            set
            {
                this.data_Registo_DiarioFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=52)]
        public string Contract_No
        {
            get
            {
                return this.contract_NoField;
            }
            set
            {
                this.contract_NoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline")]
    public enum Document_Type
    {
        
        /// <remarks/>
        Quote,
        
        /// <remarks/>
        Order,
        
        /// <remarks/>
        Invoice,
        
        /// <remarks/>
        Credit_Memo,
        
        /// <remarks/>
        Blanket_Order,
        
        /// <remarks/>
        Return_Order,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline")]
    public enum Type
    {
        
        /// <remarks/>
        _blank_,
        
        /// <remarks/>
        G_L_Account,
        
        /// <remarks/>
        Item,
        
        /// <remarks/>
        Resource,
        
        /// <remarks/>
        Fixed_Asset,
        
        /// <remarks/>
        Charge_Item,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline")]
    public enum Tipo_Objecto_Ref
    {
        
        /// <remarks/>
        N_A,
        
        /// <remarks/>
        Equipamento,
        
        /// <remarks/>
        Localização_Funcional,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline")]
    public enum Tipo_Recurso
    {
        
        /// <remarks/>
        Custo,
        
        /// <remarks/>
        Mão_de_Obra,
        
        /// <remarks/>
        ServicoExterno,
        
        /// <remarks/>
        Invoicing,
        
        /// <remarks/>
        N_A,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline")]
    public partial class WsPreInvoiceLine_Filter
    {
        
        private WsPreInvoiceLine_Fields fieldField;
        
        private string criteriaField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public WsPreInvoiceLine_Fields Field
        {
            get
            {
                return this.fieldField;
            }
            set
            {
                this.fieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Criteria
        {
            get
            {
                return this.criteriaField;
            }
            set
            {
                this.criteriaField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline")]
    public enum WsPreInvoiceLine_Fields
    {
        
        /// <remarks/>
        Sell_to_Customer_No,
        
        /// <remarks/>
        Document_No,
        
        /// <remarks/>
        Document_Type,
        
        /// <remarks/>
        Line_No,
        
        /// <remarks/>
        Type,
        
        /// <remarks/>
        No,
        
        /// <remarks/>
        Location_Code,
        
        /// <remarks/>
        Posting_Group,
        
        /// <remarks/>
        Shipment_Date,
        
        /// <remarks/>
        Description,
        
        /// <remarks/>
        Description_2,
        
        /// <remarks/>
        Unit_of_Measure,
        
        /// <remarks/>
        Quantity,
        
        /// <remarks/>
        Outstanding_Quantity,
        
        /// <remarks/>
        Qty_to_Invoice,
        
        /// <remarks/>
        Qty_to_Ship,
        
        /// <remarks/>
        Unit_Price,
        
        /// <remarks/>
        Unit_Cost_LCY,
        
        /// <remarks/>
        VAT_Percent,
        
        /// <remarks/>
        Line_Discount_Percent,
        
        /// <remarks/>
        Line_Discount_Amount,
        
        /// <remarks/>
        Amount,
        
        /// <remarks/>
        Amount_Including_VAT,
        
        /// <remarks/>
        Allow_Invoice_Disc,
        
        /// <remarks/>
        Job_No,
        
        /// <remarks/>
        Job_Journal_Line_No_Portal,
        
        /// <remarks/>
        RegionCode20,
        
        /// <remarks/>
        FunctionAreaCode20,
        
        /// <remarks/>
        ResponsabilityCenterCode20,
        
        /// <remarks/>
        Contract_No_Portal,
        
        /// <remarks/>
        Meal_Type_Description,
        
        /// <remarks/>
        Service_Group_Description,
        
        /// <remarks/>
        Waste_Shipment_No,
        
        /// <remarks/>
        External_Shipment_No,
        
        /// <remarks/>
        Consumption_Date,
        
        /// <remarks/>
        Service_Contract_No,
        
        /// <remarks/>
        N_x00BA__Linha_OM,
        
        /// <remarks/>
        N_x00BA__Objecto_Ref,
        
        /// <remarks/>
        N_x00BA__Cliente_OM,
        
        /// <remarks/>
        N_x00BA__Guia_Resíduos_GAR,
        
        /// <remarks/>
        N_x00BA__Guia_Externa,
        
        /// <remarks/>
        Tipo_Refeicao,
        
        /// <remarks/>
        Factura_de_Contrato,
        
        /// <remarks/>
        Factura_CAF,
        
        /// <remarks/>
        N_x00BA__Objecto_Ref_2,
        
        /// <remarks/>
        Tipo_Objecto_Ref,
        
        /// <remarks/>
        Grupo_Serviço,
        
        /// <remarks/>
        Cod_Serv_Cliente,
        
        /// <remarks/>
        Des_Serv_Cliente,
        
        /// <remarks/>
        Tipo_Recurso,
        
        /// <remarks/>
        Data_Registo_Diario,
        
        /// <remarks/>
        Contract_No,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Read", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class Read
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public string Document_Type;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=1)]
        public string Document_No;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=2)]
        public int Line_No;
        
        public Read()
        {
        }
        
        public Read(string Document_Type, string Document_No, int Line_No)
        {
            this.Document_Type = Document_Type;
            this.Document_No = Document_No;
            this.Line_No = Line_No;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Read_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class Read_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public WSCreatePreInvoiceLine.WsPreInvoiceLine WsPreInvoiceLine;
        
        public Read_Result()
        {
        }
        
        public Read_Result(WSCreatePreInvoiceLine.WsPreInvoiceLine WsPreInvoiceLine)
        {
            this.WsPreInvoiceLine = WsPreInvoiceLine;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ReadByRecId", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class ReadByRecId
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public string recId;
        
        public ReadByRecId()
        {
        }
        
        public ReadByRecId(string recId)
        {
            this.recId = recId;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ReadByRecId_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class ReadByRecId_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public WSCreatePreInvoiceLine.WsPreInvoiceLine WsPreInvoiceLine;
        
        public ReadByRecId_Result()
        {
        }
        
        public ReadByRecId_Result(WSCreatePreInvoiceLine.WsPreInvoiceLine WsPreInvoiceLine)
        {
            this.WsPreInvoiceLine = WsPreInvoiceLine;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ReadMultiple", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class ReadMultiple
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("filter")]
        public WSCreatePreInvoiceLine.WsPreInvoiceLine_Filter[] filter;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=1)]
        public string bookmarkKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=2)]
        public int setSize;
        
        public ReadMultiple()
        {
        }
        
        public ReadMultiple(WSCreatePreInvoiceLine.WsPreInvoiceLine_Filter[] filter, string bookmarkKey, int setSize)
        {
            this.filter = filter;
            this.bookmarkKey = bookmarkKey;
            this.setSize = setSize;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ReadMultiple_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class ReadMultiple_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ReadMultiple_Result", Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public WSCreatePreInvoiceLine.WsPreInvoiceLine[] ReadMultiple_Result1;
        
        public ReadMultiple_Result()
        {
        }
        
        public ReadMultiple_Result(WSCreatePreInvoiceLine.WsPreInvoiceLine[] ReadMultiple_Result1)
        {
            this.ReadMultiple_Result1 = ReadMultiple_Result1;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="IsUpdated", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class IsUpdated
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public string Key;
        
        public IsUpdated()
        {
        }
        
        public IsUpdated(string Key)
        {
            this.Key = Key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="IsUpdated_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class IsUpdated_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="IsUpdated_Result", Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public bool IsUpdated_Result1;
        
        public IsUpdated_Result()
        {
        }
        
        public IsUpdated_Result(bool IsUpdated_Result1)
        {
            this.IsUpdated_Result1 = IsUpdated_Result1;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetRecIdFromKey", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class GetRecIdFromKey
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public string Key;
        
        public GetRecIdFromKey()
        {
        }
        
        public GetRecIdFromKey(string Key)
        {
            this.Key = Key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetRecIdFromKey_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class GetRecIdFromKey_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetRecIdFromKey_Result", Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public string GetRecIdFromKey_Result1;
        
        public GetRecIdFromKey_Result()
        {
        }
        
        public GetRecIdFromKey_Result(string GetRecIdFromKey_Result1)
        {
            this.GetRecIdFromKey_Result1 = GetRecIdFromKey_Result1;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Create", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class Create
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public WSCreatePreInvoiceLine.WsPreInvoiceLine WsPreInvoiceLine;
        
        public Create()
        {
        }
        
        public Create(WSCreatePreInvoiceLine.WsPreInvoiceLine WsPreInvoiceLine)
        {
            this.WsPreInvoiceLine = WsPreInvoiceLine;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Create_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class Create_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public WSCreatePreInvoiceLine.WsPreInvoiceLine WsPreInvoiceLine;
        
        public Create_Result()
        {
        }
        
        public Create_Result(WSCreatePreInvoiceLine.WsPreInvoiceLine WsPreInvoiceLine)
        {
            this.WsPreInvoiceLine = WsPreInvoiceLine;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CreateMultiple", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class CreateMultiple
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public WSCreatePreInvoiceLine.WsPreInvoiceLine[] WsPreInvoiceLine_List;
        
        public CreateMultiple()
        {
        }
        
        public CreateMultiple(WSCreatePreInvoiceLine.WsPreInvoiceLine[] WsPreInvoiceLine_List)
        {
            this.WsPreInvoiceLine_List = WsPreInvoiceLine_List;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CreateMultiple_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class CreateMultiple_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public WSCreatePreInvoiceLine.WsPreInvoiceLine[] WsPreInvoiceLine_List;
        
        public CreateMultiple_Result()
        {
        }
        
        public CreateMultiple_Result(WSCreatePreInvoiceLine.WsPreInvoiceLine[] WsPreInvoiceLine_List)
        {
            this.WsPreInvoiceLine_List = WsPreInvoiceLine_List;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Update", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class Update
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public WSCreatePreInvoiceLine.WsPreInvoiceLine WsPreInvoiceLine;
        
        public Update()
        {
        }
        
        public Update(WSCreatePreInvoiceLine.WsPreInvoiceLine WsPreInvoiceLine)
        {
            this.WsPreInvoiceLine = WsPreInvoiceLine;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Update_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class Update_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public WSCreatePreInvoiceLine.WsPreInvoiceLine WsPreInvoiceLine;
        
        public Update_Result()
        {
        }
        
        public Update_Result(WSCreatePreInvoiceLine.WsPreInvoiceLine WsPreInvoiceLine)
        {
            this.WsPreInvoiceLine = WsPreInvoiceLine;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UpdateMultiple", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class UpdateMultiple
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public WSCreatePreInvoiceLine.WsPreInvoiceLine[] WsPreInvoiceLine_List;
        
        public UpdateMultiple()
        {
        }
        
        public UpdateMultiple(WSCreatePreInvoiceLine.WsPreInvoiceLine[] WsPreInvoiceLine_List)
        {
            this.WsPreInvoiceLine_List = WsPreInvoiceLine_List;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UpdateMultiple_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class UpdateMultiple_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public WSCreatePreInvoiceLine.WsPreInvoiceLine[] WsPreInvoiceLine_List;
        
        public UpdateMultiple_Result()
        {
        }
        
        public UpdateMultiple_Result(WSCreatePreInvoiceLine.WsPreInvoiceLine[] WsPreInvoiceLine_List)
        {
            this.WsPreInvoiceLine_List = WsPreInvoiceLine_List;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Delete", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class Delete
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public string Key;
        
        public Delete()
        {
        }
        
        public Delete(string Key)
        {
            this.Key = Key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Delete_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", IsWrapped=true)]
    public partial class Delete_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Delete_Result", Namespace="urn:microsoft-dynamics-schemas/page/wspreinvoiceline", Order=0)]
        public bool Delete_Result1;
        
        public Delete_Result()
        {
        }
        
        public Delete_Result(bool Delete_Result1)
        {
            this.Delete_Result1 = Delete_Result1;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public interface WsPreInvoiceLine_PortChannel : WSCreatePreInvoiceLine.WsPreInvoiceLine_Port, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public partial class WsPreInvoiceLine_PortClient : System.ServiceModel.ClientBase<WSCreatePreInvoiceLine.WsPreInvoiceLine_Port>, WSCreatePreInvoiceLine.WsPreInvoiceLine_Port
    {
        
    /// <summary>
    /// Implemente este método parcial para configurar o ponto de extremidade de serviço.
    /// </summary>
    /// <param name="serviceEndpoint">O ponto de extremidade a ser configurado</param>
    /// <param name="clientCredentials">As credenciais do cliente</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public WsPreInvoiceLine_PortClient() : 
                base(WsPreInvoiceLine_PortClient.GetDefaultBinding(), WsPreInvoiceLine_PortClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.WsPreInvoiceLine_Port.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WsPreInvoiceLine_PortClient(EndpointConfiguration endpointConfiguration) : 
                base(WsPreInvoiceLine_PortClient.GetBindingForEndpoint(endpointConfiguration), WsPreInvoiceLine_PortClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WsPreInvoiceLine_PortClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(WsPreInvoiceLine_PortClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WsPreInvoiceLine_PortClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(WsPreInvoiceLine_PortClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WsPreInvoiceLine_PortClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.Read_Result> WSCreatePreInvoiceLine.WsPreInvoiceLine_Port.ReadAsync(WSCreatePreInvoiceLine.Read request)
        {
            return base.Channel.ReadAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSCreatePreInvoiceLine.Read_Result> ReadAsync(string Document_Type, string Document_No, int Line_No)
        {
            WSCreatePreInvoiceLine.Read inValue = new WSCreatePreInvoiceLine.Read();
            inValue.Document_Type = Document_Type;
            inValue.Document_No = Document_No;
            inValue.Line_No = Line_No;
            return ((WSCreatePreInvoiceLine.WsPreInvoiceLine_Port)(this)).ReadAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.ReadByRecId_Result> WSCreatePreInvoiceLine.WsPreInvoiceLine_Port.ReadByRecIdAsync(WSCreatePreInvoiceLine.ReadByRecId request)
        {
            return base.Channel.ReadByRecIdAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSCreatePreInvoiceLine.ReadByRecId_Result> ReadByRecIdAsync(string recId)
        {
            WSCreatePreInvoiceLine.ReadByRecId inValue = new WSCreatePreInvoiceLine.ReadByRecId();
            inValue.recId = recId;
            return ((WSCreatePreInvoiceLine.WsPreInvoiceLine_Port)(this)).ReadByRecIdAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.ReadMultiple_Result> WSCreatePreInvoiceLine.WsPreInvoiceLine_Port.ReadMultipleAsync(WSCreatePreInvoiceLine.ReadMultiple request)
        {
            return base.Channel.ReadMultipleAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSCreatePreInvoiceLine.ReadMultiple_Result> ReadMultipleAsync(WSCreatePreInvoiceLine.WsPreInvoiceLine_Filter[] filter, string bookmarkKey, int setSize)
        {
            WSCreatePreInvoiceLine.ReadMultiple inValue = new WSCreatePreInvoiceLine.ReadMultiple();
            inValue.filter = filter;
            inValue.bookmarkKey = bookmarkKey;
            inValue.setSize = setSize;
            return ((WSCreatePreInvoiceLine.WsPreInvoiceLine_Port)(this)).ReadMultipleAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.IsUpdated_Result> WSCreatePreInvoiceLine.WsPreInvoiceLine_Port.IsUpdatedAsync(WSCreatePreInvoiceLine.IsUpdated request)
        {
            return base.Channel.IsUpdatedAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSCreatePreInvoiceLine.IsUpdated_Result> IsUpdatedAsync(string Key)
        {
            WSCreatePreInvoiceLine.IsUpdated inValue = new WSCreatePreInvoiceLine.IsUpdated();
            inValue.Key = Key;
            return ((WSCreatePreInvoiceLine.WsPreInvoiceLine_Port)(this)).IsUpdatedAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.GetRecIdFromKey_Result> WSCreatePreInvoiceLine.WsPreInvoiceLine_Port.GetRecIdFromKeyAsync(WSCreatePreInvoiceLine.GetRecIdFromKey request)
        {
            return base.Channel.GetRecIdFromKeyAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSCreatePreInvoiceLine.GetRecIdFromKey_Result> GetRecIdFromKeyAsync(string Key)
        {
            WSCreatePreInvoiceLine.GetRecIdFromKey inValue = new WSCreatePreInvoiceLine.GetRecIdFromKey();
            inValue.Key = Key;
            return ((WSCreatePreInvoiceLine.WsPreInvoiceLine_Port)(this)).GetRecIdFromKeyAsync(inValue);
        }
        
        public System.Threading.Tasks.Task<WSCreatePreInvoiceLine.Create_Result> CreateAsync(WSCreatePreInvoiceLine.Create request)
        {
            return base.Channel.CreateAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSCreatePreInvoiceLine.CreateMultiple_Result> CreateMultipleAsync(WSCreatePreInvoiceLine.CreateMultiple request)
        {
            return base.Channel.CreateMultipleAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSCreatePreInvoiceLine.Update_Result> UpdateAsync(WSCreatePreInvoiceLine.Update request)
        {
            return base.Channel.UpdateAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSCreatePreInvoiceLine.UpdateMultiple_Result> UpdateMultipleAsync(WSCreatePreInvoiceLine.UpdateMultiple request)
        {
            return base.Channel.UpdateMultipleAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSCreatePreInvoiceLine.Delete_Result> WSCreatePreInvoiceLine.WsPreInvoiceLine_Port.DeleteAsync(WSCreatePreInvoiceLine.Delete request)
        {
            return base.Channel.DeleteAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSCreatePreInvoiceLine.Delete_Result> DeleteAsync(string Key)
        {
            WSCreatePreInvoiceLine.Delete inValue = new WSCreatePreInvoiceLine.Delete();
            inValue.Key = Key;
            return ((WSCreatePreInvoiceLine.WsPreInvoiceLine_Port)(this)).DeleteAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.WsPreInvoiceLine_Port))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Não foi possível encontrar o ponto de extremidade com o nome \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.WsPreInvoiceLine_Port))
            {
                return new System.ServiceModel.EndpointAddress("http://such-navdev.such.local:7057/DynamicsNAV100_QUAL/WS/SUCH - Qualidade/Page/W" +
                        "sPreInvoiceLine");
            }
            throw new System.InvalidOperationException(string.Format("Não foi possível encontrar o ponto de extremidade com o nome \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return WsPreInvoiceLine_PortClient.GetBindingForEndpoint(EndpointConfiguration.WsPreInvoiceLine_Port);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return WsPreInvoiceLine_PortClient.GetEndpointAddress(EndpointConfiguration.WsPreInvoiceLine_Port);
        }
        
        public enum EndpointConfiguration
        {
            
            WsPreInvoiceLine_Port,
        }
    }
}
