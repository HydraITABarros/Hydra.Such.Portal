using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Contracts;
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

        public static async Task<WSCreatePreInvoice.Create_Result> CreatePreInvoice(ProjectDiaryViewModel PreInvoiceToCreate, NAVWSConfigurations WSConfigurations)
        {
            WSCreatePreInvoice.Create NAVCreate = new WSCreatePreInvoice.Create()
            {
                WSPreInvoice = new WSCreatePreInvoice.WSPreInvoice()
                {
                    Sell_to_Customer_No = "10000",//PreInvoiceToCreate.InvoiceToClientNo,
                    VAT_Registration_No = "789456278"
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


        public static async Task<WSCreatePreInvoice.Create_Result> CreateContractInvoice(AutorizarFaturaçãoContratos CreateInvoice, NAVWSConfigurations WSConfigurations)
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
                    Shipment_Start_TimeSpecified = true
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
