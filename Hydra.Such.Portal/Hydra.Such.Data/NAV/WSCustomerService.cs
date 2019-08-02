using AutoMapper;
using Hydra.Such.Data.ViewModel;
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
            //navWSBinding.MaxReceivedMessageSize = 20971520;
            navWSBinding.MaxReceivedMessageSize = int.MaxValue;

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
                var WSCustomer = result.WSCustomer;

                var client = MapCustomerNAVToCustomerModel(WSCustomer);
                return client;
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

            var filter = new WSCustomerNAV.WSCustomer_Filter { Field = WSCustomerNAV.WSCustomer_Fields.No, Criteria = "200*" };
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


            client.Cliente_Interno = false;
            WSCustomerNAV.Create navCreate = new WSCustomerNAV.Create()
            {
                WSCustomer = MapCustomerModelToCustomerNAV(client)
            };
            navCreate.WSCustomer.No = null;

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

                navCreate.WSCustomer.Utilizador_Alteracao_eSUCH = client.Utilizador_Alteracao_eSUCH;

                navCreate.WSCustomer.Abrigo_Lei_CompromissoSpecified = true;
                navCreate.WSCustomer.BlockedSpecified = true;
                navCreate.WSCustomer.Cliente_AssociadoSpecified = true;
                navCreate.WSCustomer.Cliente_InternoSpecified = true;
                navCreate.WSCustomer.Cliente_NacionalSpecified = true;
                navCreate.WSCustomer.Data_Cliente_PaiSpecified = true;
                navCreate.WSCustomer.Natureza_ClienteSpecified = true;
                navCreate.WSCustomer.Regiao_ClienteSpecified = true;
                navCreate.WSCustomer.Taxa_AprovisionamentoSpecified = true;
                navCreate.WSCustomer.Tipo_ClienteSpecified = true;

                WSCustomerNAV.Create_Result result = await ws_Client.CreateAsync(navCreate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSCustomerNAV.Update_Result> UpdateAsync(ClientDetailsViewModel client, NAVWSConfigurations WSConfigurations)
        {
            if (client == null)
                throw new ArgumentNullException("client");


            client.Cliente_Interno = false;
            WSCustomerNAV.Update navUpdate = new WSCustomerNAV.Update()
            {
                WSCustomer = MapCustomerModelToCustomerNAV(client)
            };

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Customer_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCustomerNAV.WSCustomer_PortClient ws_Client = new WSCustomerNAV.WSCustomer_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            WSCustomerNAV.Read_Result resultRead = await ws_Client.ReadAsync(navUpdate.WSCustomer.No);
            navUpdate.WSCustomer.Key = resultRead.WSCustomer.Key;

            try
            {
                navUpdate.WSCustomer.Utilizador_Alteracao_eSUCH = client.Utilizador_Alteracao_eSUCH;

                navUpdate.WSCustomer.Abrigo_Lei_CompromissoSpecified = true;
                navUpdate.WSCustomer.BlockedSpecified = true;
                navUpdate.WSCustomer.Cliente_AssociadoSpecified = true;
                navUpdate.WSCustomer.Cliente_InternoSpecified = true;
                navUpdate.WSCustomer.Cliente_NacionalSpecified = true;
                navUpdate.WSCustomer.Data_Cliente_PaiSpecified = true;
                navUpdate.WSCustomer.Natureza_ClienteSpecified = true;
                navUpdate.WSCustomer.Regiao_ClienteSpecified = true;
                navUpdate.WSCustomer.Taxa_AprovisionamentoSpecified = true;
                navUpdate.WSCustomer.Tipo_ClienteSpecified = true;

                WSCustomerNAV.Update_Result result = await ws_Client.UpdateAsync(navUpdate);
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

        #region Mappers

        public static ClientDetailsViewModel MapCustomerNAVToCustomerModel(WSCustomerNAV.WSCustomer CustomerNAV)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WSCustomerNAV.WSCustomer, ClientDetailsViewModel>();
                /*.ForMember(dest => dest.Regiao_Cliente, opts => opts.MapFrom(src => src.Regiao_Cliente != null ? Enum.Parse(typeof(Regiao_Cliente?), src.Regiao_Cliente) : null));*/
            }).CreateMapper();

            var CustomerModel = mapper.Map<WSCustomerNAV.WSCustomer, ClientDetailsViewModel>(CustomerNAV);

            return CustomerModel;
        }

        public static WSCustomerNAV.WSCustomer MapCustomerModelToCustomerNAV(ClientDetailsViewModel CustomerModel)
        {
            var mapper = new MapperConfiguration(cfg =>
                cfg.CreateMap<ClientDetailsViewModel, WSCustomerNAV.WSCustomer>()/*.ForMember(x => x.No, opt => opt.Ignore())*/
            ).CreateMapper();

            var CustomerNAV = mapper.Map<ClientDetailsViewModel, WSCustomerNAV.WSCustomer>(CustomerModel);

            return CustomerNAV;
        }

        #endregion

    }
}
