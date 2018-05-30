using AutoMapper;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Such.Data.NAV
{
    public static class WSClient
    {
        static BasicHttpBinding navWSBinding;

        static WSClient()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            navWSBinding.MaxReceivedMessageSize = 20971520;
            /*navWSBinding.MaxBufferSize = 20971520;
            navWSBinding.MaxBufferPoolSize = 20971520;*/
        }

        public static async Task<WSClientNAV.Create_Result> CreateAsync(ClientDetailsViewModel client, NAVWSConfigurations WSConfigurations)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            var mapper = new MapperConfiguration(cfg =>
                    cfg.CreateMap<ClientDetailsViewModel, WSClientNAV.teste>()
                ).CreateMapper();

            WSClientNAV.Create navCreate = new WSClientNAV.Create()
            {
                teste = mapper.Map<ClientDetailsViewModel, WSClientNAV.teste>(client)
                /*teste = new WSClientNAV.teste 
                {
                    //No = client.No,
                    Name = client.Name,
                    Address = client.Address,
                    Address_2 = client.Address_2,
                    Post_Code = client.Post_Code,
                    City = client.City,
                    Phone_No = client.Phone_No,
                    E_Mail = client.E_Mail,
                    Fax_No = client.Fax_No,
                    Home_Page = client.Home_Page,
                    County = client.County,
                    VAT_Registration_No = client.VAT_Registration_No,
                    Cliente_Associado = client.Cliente_Associado,
                    Associado_A_N = client.Associado_A_N,
                    Tipo_Cliente = client.Tipo_Cliente,
                    Natureza_Cliente = client.Natureza_Cliente
                }*/
            };

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Client_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSClientNAV.teste_PortClient ws_Client = new WSClientNAV.teste_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                var lastClientResult = await ws_Client.ReadMultipleAsync(null, null, -1);

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

                    if(alphas.Count() > 0)
                        newClientNo = String.Join("", alphas);

                    if (digits.Count() > 0)
                    {
                        var newNum = int.Parse(String.Join("", digits)) + 1;
                        newClientNo += newNum.ToString();
                    }

                }

                navCreate.teste.No = newClientNo;

                WSClientNAV.Create_Result result = await ws_Client.CreateAsync(navCreate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSClientNAV.Update_Result> UpdateAsync(ClientDetailsViewModel client, NAVWSConfigurations WSConfigurations)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            WSClientNAV.Update navUpdate = new WSClientNAV.Update()
            {
                teste = new WSClientNAV.teste
                {
                    //No = client.No,
                    Name = client.Name,
                    Address = client.Address,
                    Address_2 = client.Address_2,
                    Post_Code = client.Post_Code,
                    City = client.City,
                    Phone_No = client.Phone_No,
                    E_Mail = client.E_Mail,
                    Fax_No = client.Fax_No,
                    Home_Page = client.Home_Page,
                    County = client.County,
                    VAT_Registration_No = client.VAT_Registration_No,
                    Cliente_Associado = client.Cliente_Associado,
                    Associado_A_N = client.Associado_A_N,
                    Tipo_Cliente = client.Tipo_Cliente,
                    Natureza_Cliente = client.Natureza_Cliente
                }
            };

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Client_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSClientNAV.teste_PortClient ws_Client = new WSClientNAV.teste_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            WSClientNAV.Read_Result resultRead = await ws_Client.ReadAsync(navUpdate.teste.No);
            navUpdate.teste.Key = resultRead.teste.Key;

            try
            {
                WSClientNAV.Update_Result result = await ws_Client.UpdateAsync(navUpdate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSClientNAV.Delete_Result> DeleteAsync(string ClientNo, NAVWSConfigurations WSConfigurations)
        {
            if (ClientNo == null)
                throw new ArgumentNullException("client");

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Client_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSClientNAV.teste_PortClient ws_Client = new WSClientNAV.teste_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSClientNAV.Read_Result resultRead = await ws_Client.ReadAsync(ClientNo);
                WSClientNAV.Delete_Result result = await ws_Client.DeleteAsync(resultRead.teste.Key);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<ClientDetailsViewModel> GetByNoAsync(string ClientNo, NAVWSConfigurations WSConfigurations)
        {
            if (ClientNo == null)
                throw new ArgumentNullException("ClientNo");

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Client_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSClientNAV.teste_PortClient WS_Client = new WSClientNAV.teste_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSClientNAV.Read_Result result = await WS_Client.ReadAsync(ClientNo);

                var origin = result.teste;

                var mapper = new MapperConfiguration(cfg =>
                    cfg.CreateMap<WSClientNAV.teste, ClientDetailsViewModel>()
                ).CreateMapper();

                var dest = mapper.Map<WSClientNAV.teste, ClientDetailsViewModel>(origin);

                return dest;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSClientNAV.ReadMultiple_Result> GetAllAsync(int size, NAVWSConfigurations WSConfigurations)
        {
            //if (size == null)
            //    throw new ArgumentNullException("client");

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Client_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSClientNAV.teste_PortClient WS_Client = new WSClientNAV.teste_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //var filter = new WSClientNAV.teste_Filter { Field = WSClientNAV.teste_Fields.No, Criteria = null };
            //WSClientNAV.teste_Filter[] filterArray = new WSClientNAV.teste_Filter[] { filter };
            
            try
            {
                WSClientNAV.ReadMultiple_Result result = await WS_Client.ReadMultipleAsync(null, null, size);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }



    }
}
