﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     //
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WSGenericCodeUnit
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", ConfigurationName="WSGenericCodeUnit.WsGeneric_Port")]
    public interface WsGeneric_Port
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/WsGeneric:FxGetStock_ItemLocation", ReplyAction="*")]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxGetStock_ItemLocation_Result> FxGetStock_ItemLocationAsync(WSGenericCodeUnit.FxGetStock_ItemLocation request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/WsGeneric:FxPostJobJrnlLines", ReplyAction="*")]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> FxPostJobJrnlLinesAsync(WSGenericCodeUnit.FxPostJobJrnlLines request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/WsGeneric:FxPostInvoice", ReplyAction="*")]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxPostInvoice_Result> FxPostInvoiceAsync(WSGenericCodeUnit.FxPostInvoice request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/WsGeneric:FxPostPurchOrderReceiptLines", ReplyAction="*")]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxPostPurchOrderReceiptLines_Result> FxPostPurchOrderReceiptLinesAsync(WSGenericCodeUnit.FxPostPurchOrderReceiptLines request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/WsGeneric:FxContact2Customer", ReplyAction="*")]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxContact2Customer_Result> FxContact2CustomerAsync(WSGenericCodeUnit.FxContact2Customer request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/WsGeneric:FxCabimento", ReplyAction="*")]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxCabimento_Result> FxCabimentoAsync(WSGenericCodeUnit.FxCabimento request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/WsGeneric:FxPostShipmentDoc", ReplyAction="*")]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxPostShipmentDoc_Result> FxPostShipmentDocAsync(WSGenericCodeUnit.FxPostShipmentDoc request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxGetStock_ItemLocation", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxGetStock_ItemLocation
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", Order=0)]
        public string pItemNo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", Order=1)]
        public string pLocationCode;
        
        public FxGetStock_ItemLocation()
        {
        }
        
        public FxGetStock_ItemLocation(string pItemNo, string pLocationCode)
        {
            this.pItemNo = pItemNo;
            this.pLocationCode = pLocationCode;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxGetStock_ItemLocation_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxGetStock_ItemLocation_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", Order=0)]
        public decimal return_value;
        
        public FxGetStock_ItemLocation_Result()
        {
        }
        
        public FxGetStock_ItemLocation_Result(decimal return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxPostJobJrnlLines", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxPostJobJrnlLines
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", Order=0)]
        public string pTransactionNo;
        
        public FxPostJobJrnlLines()
        {
        }
        
        public FxPostJobJrnlLines(string pTransactionNo)
        {
            this.pTransactionNo = pTransactionNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxPostJobJrnlLines_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxPostJobJrnlLines_Result
    {
        
        public FxPostJobJrnlLines_Result()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxPostInvoice", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxPostInvoice
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", Order=0)]
        public string pInvoiceNo;
        
        public FxPostInvoice()
        {
        }
        
        public FxPostInvoice(string pInvoiceNo)
        {
            this.pInvoiceNo = pInvoiceNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxPostInvoice_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxPostInvoice_Result
    {
        
        public FxPostInvoice_Result()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxPostPurchOrderReceiptLines", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxPostPurchOrderReceiptLines
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", Order=0)]
        public string pXMLDoc;
        
        public FxPostPurchOrderReceiptLines()
        {
        }
        
        public FxPostPurchOrderReceiptLines(string pXMLDoc)
        {
            this.pXMLDoc = pXMLDoc;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxPostPurchOrderReceiptLines_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxPostPurchOrderReceiptLines_Result
    {
        
        public FxPostPurchOrderReceiptLines_Result()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxContact2Customer", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxContact2Customer
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", Order=0)]
        public string pContactNo;
        
        public FxContact2Customer()
        {
        }
        
        public FxContact2Customer(string pContactNo)
        {
            this.pContactNo = pContactNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxContact2Customer_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxContact2Customer_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", Order=0)]
        public string return_value;
        
        public FxContact2Customer_Result()
        {
        }
        
        public FxContact2Customer_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxCabimento", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxCabimento
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", Order=0)]
        public string pPurchHeaderNo;
        
        public FxCabimento()
        {
        }
        
        public FxCabimento(string pPurchHeaderNo)
        {
            this.pPurchHeaderNo = pPurchHeaderNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxCabimento_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxCabimento_Result
    {
        
        public FxCabimento_Result()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxPostShipmentDoc", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxPostShipmentDoc
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", Order=0)]
        public string pShipmentDocNo;
        
        public FxPostShipmentDoc()
        {
        }
        
        public FxPostShipmentDoc(string pShipmentDocNo)
        {
            this.pShipmentDocNo = pShipmentDocNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FxPostShipmentDoc_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", IsWrapped=true)]
    public partial class FxPostShipmentDoc_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/WsGeneric", Order=0)]
        public string return_value;
        
        public FxPostShipmentDoc_Result()
        {
        }
        
        public FxPostShipmentDoc_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    public interface WsGeneric_PortChannel : WSGenericCodeUnit.WsGeneric_Port, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    public partial class WsGeneric_PortClient : System.ServiceModel.ClientBase<WSGenericCodeUnit.WsGeneric_Port>, WSGenericCodeUnit.WsGeneric_Port
    {
        
    /// <summary>
    /// Implement this partial method to configure the service endpoint.
    /// </summary>
    /// <param name="serviceEndpoint">The endpoint to configure</param>
    /// <param name="clientCredentials">The client credentials</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public WsGeneric_PortClient() : 
                base(WsGeneric_PortClient.GetDefaultBinding(), WsGeneric_PortClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.WsGeneric_Port.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WsGeneric_PortClient(EndpointConfiguration endpointConfiguration) : 
                base(WsGeneric_PortClient.GetBindingForEndpoint(endpointConfiguration), WsGeneric_PortClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WsGeneric_PortClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(WsGeneric_PortClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WsGeneric_PortClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(WsGeneric_PortClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WsGeneric_PortClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxGetStock_ItemLocation_Result> WSGenericCodeUnit.WsGeneric_Port.FxGetStock_ItemLocationAsync(WSGenericCodeUnit.FxGetStock_ItemLocation request)
        {
            return base.Channel.FxGetStock_ItemLocationAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSGenericCodeUnit.FxGetStock_ItemLocation_Result> FxGetStock_ItemLocationAsync(string pItemNo, string pLocationCode)
        {
            WSGenericCodeUnit.FxGetStock_ItemLocation inValue = new WSGenericCodeUnit.FxGetStock_ItemLocation();
            inValue.pItemNo = pItemNo;
            inValue.pLocationCode = pLocationCode;
            return ((WSGenericCodeUnit.WsGeneric_Port)(this)).FxGetStock_ItemLocationAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> WSGenericCodeUnit.WsGeneric_Port.FxPostJobJrnlLinesAsync(WSGenericCodeUnit.FxPostJobJrnlLines request)
        {
            return base.Channel.FxPostJobJrnlLinesAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> FxPostJobJrnlLinesAsync(string pTransactionNo)
        {
            WSGenericCodeUnit.FxPostJobJrnlLines inValue = new WSGenericCodeUnit.FxPostJobJrnlLines();
            inValue.pTransactionNo = pTransactionNo;
            return ((WSGenericCodeUnit.WsGeneric_Port)(this)).FxPostJobJrnlLinesAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxPostInvoice_Result> WSGenericCodeUnit.WsGeneric_Port.FxPostInvoiceAsync(WSGenericCodeUnit.FxPostInvoice request)
        {
            return base.Channel.FxPostInvoiceAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSGenericCodeUnit.FxPostInvoice_Result> FxPostInvoiceAsync(string pInvoiceNo)
        {
            WSGenericCodeUnit.FxPostInvoice inValue = new WSGenericCodeUnit.FxPostInvoice();
            inValue.pInvoiceNo = pInvoiceNo;
            return ((WSGenericCodeUnit.WsGeneric_Port)(this)).FxPostInvoiceAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxPostPurchOrderReceiptLines_Result> WSGenericCodeUnit.WsGeneric_Port.FxPostPurchOrderReceiptLinesAsync(WSGenericCodeUnit.FxPostPurchOrderReceiptLines request)
        {
            return base.Channel.FxPostPurchOrderReceiptLinesAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSGenericCodeUnit.FxPostPurchOrderReceiptLines_Result> FxPostPurchOrderReceiptLinesAsync(string pXMLDoc)
        {
            WSGenericCodeUnit.FxPostPurchOrderReceiptLines inValue = new WSGenericCodeUnit.FxPostPurchOrderReceiptLines();
            inValue.pXMLDoc = pXMLDoc;
            return ((WSGenericCodeUnit.WsGeneric_Port)(this)).FxPostPurchOrderReceiptLinesAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxContact2Customer_Result> WSGenericCodeUnit.WsGeneric_Port.FxContact2CustomerAsync(WSGenericCodeUnit.FxContact2Customer request)
        {
            return base.Channel.FxContact2CustomerAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSGenericCodeUnit.FxContact2Customer_Result> FxContact2CustomerAsync(string pContactNo)
        {
            WSGenericCodeUnit.FxContact2Customer inValue = new WSGenericCodeUnit.FxContact2Customer();
            inValue.pContactNo = pContactNo;
            return ((WSGenericCodeUnit.WsGeneric_Port)(this)).FxContact2CustomerAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxCabimento_Result> WSGenericCodeUnit.WsGeneric_Port.FxCabimentoAsync(WSGenericCodeUnit.FxCabimento request)
        {
            return base.Channel.FxCabimentoAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSGenericCodeUnit.FxCabimento_Result> FxCabimentoAsync(string pPurchHeaderNo)
        {
            WSGenericCodeUnit.FxCabimento inValue = new WSGenericCodeUnit.FxCabimento();
            inValue.pPurchHeaderNo = pPurchHeaderNo;
            return ((WSGenericCodeUnit.WsGeneric_Port)(this)).FxCabimentoAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WSGenericCodeUnit.FxPostShipmentDoc_Result> WSGenericCodeUnit.WsGeneric_Port.FxPostShipmentDocAsync(WSGenericCodeUnit.FxPostShipmentDoc request)
        {
            return base.Channel.FxPostShipmentDocAsync(request);
        }
        
        public System.Threading.Tasks.Task<WSGenericCodeUnit.FxPostShipmentDoc_Result> FxPostShipmentDocAsync(string pShipmentDocNo)
        {
            WSGenericCodeUnit.FxPostShipmentDoc inValue = new WSGenericCodeUnit.FxPostShipmentDoc();
            inValue.pShipmentDocNo = pShipmentDocNo;
            return ((WSGenericCodeUnit.WsGeneric_Port)(this)).FxPostShipmentDocAsync(inValue);
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
            if ((endpointConfiguration == EndpointConfiguration.WsGeneric_Port))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.WsGeneric_Port))
            {
                return new System.ServiceModel.EndpointAddress("http://such-navsql.such.local:8047/DynamicsNAV100_DEV/WS/ReplaceWithAPercentEncod" +
                        "edCompanyName/Codeunit/WsGeneric");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return WsGeneric_PortClient.GetBindingForEndpoint(EndpointConfiguration.WsGeneric_Port);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return WsGeneric_PortClient.GetEndpointAddress(EndpointConfiguration.WsGeneric_Port);
        }
        
        public enum EndpointConfiguration
        {
            
            WsGeneric_Port,
        }
    }
}
