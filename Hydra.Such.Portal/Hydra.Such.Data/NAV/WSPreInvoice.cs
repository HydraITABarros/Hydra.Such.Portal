using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Contracts;
using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Hydra.Such.Data.NAV
{
    public static class WSPreInvoice
    {
        static BasicHttpBinding navWSBinding;

        static WSPreInvoice()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }

        public static async Task<WSCreatePreInvoice.Create_Result> CreatePreInvoice(SPInvoiceListViewModel PreInvoiceToCreate, NAVWSConfigurations WSConfigurations)
        {
            WSCreatePreInvoice.Create NAVCreate = new WSCreatePreInvoice.Create()
            {
                WSPreInvoice = new WSCreatePreInvoice.WSPreInvoice()
                {
                    Document_Type = WSCreatePreInvoice.Document_Type.Invoice,
                    Document_TypeSpecified = true,
                    Sell_to_Customer_No = PreInvoiceToCreate.InvoiceToClientNo,
                    VAT_Registration_No = PreInvoiceToCreate.ClientVATReg,
                    Contract_No=PreInvoiceToCreate.DocumentNo
                    
                   
                }
            };

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoice_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoice.WSPreInvoice_PortClient WS_Client = new WSCreatePreInvoice.WSPreInvoice_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);


            return await WS_Client.CreateAsync(NAVCreate);
            
        }

        public static async Task<WSCreatePreInvoice.Create_Result> CreatePreInvoiceHeader(NAVSalesHeaderViewModel PreInvoiceToCreate, NAVWSConfigurations WSConfigurations)
        {
            WSCreatePreInvoice.Create NAVCreate = new WSCreatePreInvoice.Create()
            {

                WSPreInvoice = new WSCreatePreInvoice.WSPreInvoice()
                {
                    Sell_to_Customer_No = PreInvoiceToCreate.Sell_toCustomerNo,
                    Document_Date = PreInvoiceToCreate.DocumentDate,
                    Shipment_Date = PreInvoiceToCreate.ShipmentDate,
                    Periodo_de_Fact_Contrato = PreInvoiceToCreate.PeriododeFact_Contrato,
                    Valor_Contrato = PreInvoiceToCreate.ValorContrato,
                    Ship_to_Address = PreInvoiceToCreate.Ship_toAddress,
                    Ship_to_Post_Code = PreInvoiceToCreate.Ship_toPostCode,
                    Currency_Code = PreInvoiceToCreate.CurrencyCode,
                    Due_Date = PreInvoiceToCreate.DueDate,
                    Payment_Terms_Code = PreInvoiceToCreate.PaymentTermsCode,
                    Payment_Method_Code = PreInvoiceToCreate.PaymentMethodCode,
                    Responsibility_Center = PreInvoiceToCreate.ResponsibilityCenter,
                    No_Compromisso = PreInvoiceToCreate.No_Compromisso,
                    Codigo_Pedido = PreInvoiceToCreate.CodigoPedido,
                    Data_Encomenda = PreInvoiceToCreate.DataEncomenda,
                    Data_Serv_Prestado = PreInvoiceToCreate.DataServ_Prestado,
                    Observacoes = PreInvoiceToCreate.Observacoes,
                    Contract_No = PreInvoiceToCreate.ContractNo,
                    Document_Type = WSCreatePreInvoice.Document_Type.Invoice,
                    Factura_CAF= PreInvoiceToCreate.FacturaCAF,
                    User_pre_registo_2009=PreInvoiceToCreate.Userpreregisto2009,
                    Posting_Date= PreInvoiceToCreate.PostingDate,
                    Document_TypeSpecified = true,
                    Posting_DateSpecified=true,
                    Shipment_DateSpecified=true,
                    Shipment_Start_TimeSpecified=true,
                    ReportID_OriginalSpecified=true,
                    Valor_ContratoSpecified=true,
                    Document_DateSpecified=true,
                    Factura_CAFSpecified=true,
                    Due_DateSpecified=true,
                    Order_DateSpecified=true,
                    Data_EncomendaSpecified=true,
                    
                    //Dimensions
                    ResponsabilityCenterCode20 = PreInvoiceToCreate.ResponsibilityCenter,
                    FunctionAreaCode20 = PreInvoiceToCreate.Area,
                    RegionCode20 = PreInvoiceToCreate.ReasonCode,
                   
                   

                }
            };

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoice_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoice.WSPreInvoice_PortClient WS_Client = new WSCreatePreInvoice.WSPreInvoice_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreatePreInvoice.Create_Result result = await WS_Client.CreateAsync(NAVCreate);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<WSCreatePreInvoice.Create_Result> CreateContractInvoice(AutorizarFaturaçãoContratos CreateInvoice, NAVWSConfigurations WSConfigurations,string ContractInvoicePeriod, string InvoiceBorrowed)
        {
            DateTime now = DateTime.Now;
            WSCreatePreInvoice.Create NAVCreate = new WSCreatePreInvoice.Create()
            {
                WSPreInvoice = new WSCreatePreInvoice.WSPreInvoice()
                {
                    Sell_to_Customer_No = CreateInvoice.NºCliente,
                    Document_Date = DateTime.Today,
                    Document_DateSpecified = true,
                    Shipment_Date = now,
                    Shipment_DateSpecified = true,
                    Shipment_Start_Time = now.AddHours(1),
                    Shipment_Start_TimeSpecified = true,
                    Document_Type = WSCreatePreInvoice.Document_Type.Invoice,
                    Document_TypeSpecified = true,
                    Posting_Date = CreateInvoice.DataDeRegisto ?? DateTime.Now,
                    Periodo_de_Fact_Contrato = ContractInvoicePeriod,
                    Data_Serv_Prestado = InvoiceBorrowed

                }
            };

            // Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoice_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoice.WSPreInvoice_PortClient WS_Client = new WSCreatePreInvoice.WSPreInvoice_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreatePreInvoice.Create_Result result = await WS_Client.CreateAsync(NAVCreate);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static async Task<WSCreatePreInvoice.CreateMultiple_Result> CreateMultipleContractInvoice(List<AutorizarFaturaçãoContratos> CreateList, NAVWSConfigurations WSConfigurations)
        {
            WSCreatePreInvoice.WSPreInvoice[] parsedList = CreateList.Select(
                x => new WSCreatePreInvoice.WSPreInvoice
                {
                    Sell_to_Customer_No = x.NºCliente,
                    Document_Date = x.DataDeRegisto.Value,
                    Due_Date = x.DataDeExpiração.Value
                }).ToArray();

            WSCreatePreInvoice.CreateMultiple NAVCreate = new WSCreatePreInvoice.CreateMultiple(parsedList);

            // Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoice_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoice.WSPreInvoice_PortClient WS_Client = new WSCreatePreInvoice.WSPreInvoice_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreatePreInvoice.CreateMultiple_Result result = await WS_Client.CreateMultipleAsync(NAVCreate);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

      


        public static async Task<WSCreatePreInvoice.Delete_Result> DeletePreInvoiceLineList(String HeaderNo, NAVWSConfigurations WSConfigurations)
        {

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoiceLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoice.WSPreInvoice_PortClient WS_Client = new WSCreatePreInvoice.WSPreInvoice_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreatePreInvoice.Delete_Result result = await WS_Client.DeleteAsync(HeaderNo);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
