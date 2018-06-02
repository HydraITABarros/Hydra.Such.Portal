using Hydra.Such.Data.ViewModel.Clients;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Such.Data.NAV
{
    public static class WSShipToAddressService
    {
        static BasicHttpBinding navWSBinding;

        static WSShipToAddressService()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            navWSBinding.MaxReceivedMessageSize = 20971520;
            /*navWSBinding.MaxBufferSize = 20971520;
            navWSBinding.MaxBufferPoolSize = 20971520;*/
        }

        public static async Task<WSShipToAddressNAV.WSShipToAddress[]> GetByNoAsync(string CustomerNo, NAVWSConfigurations WSConfigurations)
        {
            if (CustomerNo == null)
                throw new ArgumentNullException("CustomerNo");

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_ShipToAddress_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSShipToAddressNAV.WSShipToAddress_PortClient WS_ShipToAddress = new WSShipToAddressNAV.WSShipToAddress_PortClient(navWSBinding, WS_URL);
            WS_ShipToAddress.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_ShipToAddress.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            var filter = new WSShipToAddressNAV.WSShipToAddress_Filter { Field = WSShipToAddressNAV.WSShipToAddress_Fields.Customer_No, Criteria = CustomerNo };
            var filterArray = new WSShipToAddressNAV.WSShipToAddress_Filter[] { filter };

            try
            {
                WSShipToAddressNAV.ReadMultiple_Result result = await WS_ShipToAddress.ReadMultipleAsync(filterArray, null, 0);  //WS_ShipToAddress.ReadAsync(CustomerNo);
                return result.ReadMultiple_Result1;

                //var WSCustomer = result.WSCustomer;

                //var client = MapCustomerNAVToCustomerModel(WSCustomer);
                //return client;
                //return WSCustomer;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSShipToAddressNAV.Create_Result> CreateAsync(ClientDetailsViewModel client, NAVWSConfigurations WSConfigurations)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            WSShipToAddressNAV.Create navCreate = new WSShipToAddressNAV.Create()
            {
                //WSCustomer = MapCustomerModelToCustomerNAV(client)
            };

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_ShipToAddress_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSShipToAddressNAV.WSShipToAddress_PortClient WS_ShipToAddress = new WSShipToAddressNAV.WSShipToAddress_PortClient(navWSBinding, ws_URL);
            WS_ShipToAddress.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_ShipToAddress.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSShipToAddressNAV.Create_Result result = await WS_ShipToAddress.CreateAsync(navCreate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSShipToAddressNAV.Update_Result> UpdateAsync(ClientDetailsViewModel client, NAVWSConfigurations WSConfigurations)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            WSShipToAddressNAV.Update navUpdate = new WSShipToAddressNAV.Update()
            {
                //WSCustomer = MapCustomerModelToCustomerNAV(client)
            };

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_ShipToAddress_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSShipToAddressNAV.WSShipToAddress_PortClient WS_ShipToAddress = new WSShipToAddressNAV.WSShipToAddress_PortClient(navWSBinding, ws_URL);
            WS_ShipToAddress.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_ShipToAddress.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            WSShipToAddressNAV.Read_Result resultRead = await WS_ShipToAddress.ReadAsync(navUpdate.WSShipToAddress.Customer_No, navUpdate.WSShipToAddress.Code);
            navUpdate.WSShipToAddress.Key = resultRead.WSShipToAddress.Key;

            try
            {
                WSShipToAddressNAV.Update_Result result = await WS_ShipToAddress.UpdateAsync(navUpdate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSShipToAddressNAV.Delete_Result> DeleteAsync(string CustomerNo, string Code, NAVWSConfigurations WSConfigurations)
        {
            if (CustomerNo == null)
                throw new ArgumentNullException("CustomerNo");

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_ShipToAddress_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSShipToAddressNAV.WSShipToAddress_PortClient WS_ShipToAddress = new WSShipToAddressNAV.WSShipToAddress_PortClient(navWSBinding, ws_URL);
            WS_ShipToAddress.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_ShipToAddress.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSShipToAddressNAV.Read_Result resultRead = await WS_ShipToAddress.ReadAsync(CustomerNo, Code);
                WSShipToAddressNAV.Delete_Result result = await WS_ShipToAddress.DeleteAsync(resultRead.WSShipToAddress.Key);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
