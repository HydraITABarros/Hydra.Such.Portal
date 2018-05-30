using AutoMapper;
using Hydra.Such.Data.ViewModel.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Such.Data.NAV
{
    public static class WSCustomerService
    {
        static BasicHttpBinding navWSBinding;

        static WSCustomerService()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            navWSBinding.MaxReceivedMessageSize = 20971520;
            /*navWSBinding.MaxBufferSize = 20971520;
            navWSBinding.MaxBufferPoolSize = 20971520;*/
        }

        public static async Task<ClientDetailsViewModel> GetByNoAsync(string CustomerNo, NAVWSConfigurations WSConfigurations)
        {
            if (CustomerNo == null)
                throw new ArgumentNullException("CustomerNo");

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Customer_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCustomerNAV.WSCustomer_PortClient WS_Client = new WSCustomerNAV.WSCustomer_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCustomerNAV.Read_Result result = await WS_Client.ReadAsync(CustomerNo);

                var origin = result.WSCustomer;

                var mapper = new MapperConfiguration(cfg =>
                    cfg.CreateMap<WSCustomerNAV.WSCustomer, ClientDetailsViewModel>()
                ).CreateMapper();

                var dest = mapper.Map<WSCustomerNAV.WSCustomer, ClientDetailsViewModel>(origin);

                return dest;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSCustomerNAV.ReadMultiple_Result> GetAllAsync(int size, NAVWSConfigurations WSConfigurations)
        {
            //if (size == null)
            //    throw new ArgumentNullException("client");

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Customer_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCustomerNAV.WSCustomer_PortClient WS_Client = new WSCustomerNAV.WSCustomer_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            var filter = new WSCustomerNAV.WSCustomer_Filter { Field = WSCustomerNAV.WSCustomer_Fields.No, Criteria = "20066*" };
            var filterArray = new WSCustomerNAV.WSCustomer_Filter[] { filter };

            try
            {
                WSCustomerNAV.ReadMultiple_Result result = await WS_Client.ReadMultipleAsync(filterArray, null, size);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSCustomerNAV.Create_Result> CreateAsync(ClientDetailsViewModel client, NAVWSConfigurations WSConfigurations)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            var mapper = new MapperConfiguration(cfg =>
                    cfg.CreateMap<ClientDetailsViewModel, WSCustomerNAV.WSCustomer>()
                ).CreateMapper();

            WSCustomerNAV.Create navCreate = new WSCustomerNAV.Create()
            {
                WSCustomer = mapper.Map<ClientDetailsViewModel, WSCustomerNAV.WSCustomer>(client)
            };

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Customer_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCustomerNAV.WSCustomer_PortClient ws_Client = new WSCustomerNAV.WSCustomer_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                /*var lastClientResult = await ws_Client.ReadMultipleAsync(null, null, -1);

                string lastClientNo = null, newClientNo = "1"; //default client No
                if (lastClientResult != null)
                    lastClientNo = lastClientResult.ReadMultiple_Result1[0].No;

                if (lastClientNo != null)
                {
                    var digits = from c in lastClientNo
                                 where Char.IsDigit(c)
                                 select c;

                    var alphas = from c in lastClientNo
                                 where !Char.IsDigit(c)
                                 select c;

                    if (alphas.Count() > 0)
                        newClientNo = String.Join("", alphas);

                    if (digits.Count() > 0)
                    {
                        var newNum = int.Parse(String.Join("", digits)) + 1;
                        newClientNo += newNum.ToString();
                    }

                }

                navCreate.WSCustomer.No = newClientNo;*/

                WSCustomerNAV.Create_Result result = await ws_Client.CreateAsync(navCreate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSCustomerNAV.Delete_Result> DeleteAsync(string CustomerNo, NAVWSConfigurations WSConfigurations)
        {
            if (CustomerNo == null)
                throw new ArgumentNullException("CustomerNo");

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Customer_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCustomerNAV.WSCustomer_PortClient ws_Client = new WSCustomerNAV.WSCustomer_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCustomerNAV.Read_Result resultRead = await ws_Client.ReadAsync(CustomerNo);
                WSCustomerNAV.Delete_Result result = await ws_Client.DeleteAsync(resultRead.WSCustomer.Key);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
