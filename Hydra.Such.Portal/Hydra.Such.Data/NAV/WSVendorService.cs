using AutoMapper;
using Hydra.Such.Data.ViewModel.Fornecedores;
using System;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using WSVendorNAV;

namespace Hydra.Such.Data.NAV
{
    public static class WSVendorService
    {
        static BasicHttpBinding navWSBinding;

        static WSVendorService()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            navWSBinding.MaxReceivedMessageSize = int.MaxValue;
        }

        public static async Task<FornecedorDetailsViewModel> GetByNoAsync(string VendorNo, NAVWSConfigurations WSConfigurations)
        {
            if (VendorNo == null)
                throw new ArgumentNullException("VendorNo");

            //Configure NAV Vendor
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Vendor_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSVendorNAV.WSVendor_PortClient WS_Vendor = new WSVendorNAV.WSVendor_PortClient(navWSBinding, WS_URL);
            WS_Vendor.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Vendor.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSVendorNAV.Read_Result result = await WS_Vendor.ReadAsync(VendorNo);
                var WSVendor = result.WSVendor;

                var vendor = MapVendorNAVToVendorModel(WSVendor);
                return vendor;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSVendorNAV.ReadMultiple_Result> GetAllAsync(int size, NAVWSConfigurations WSConfigurations)
        {
            //Configure NAV Vendor
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Vendor_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSVendorNAV.WSVendor_PortClient WS_Vendor = new WSVendorNAV.WSVendor_PortClient(navWSBinding, WS_URL);
            WS_Vendor.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Vendor.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            var filter = new WSVendorNAV.WSVendor_Filter { Field = WSVendorNAV.WSVendor_Fields.No, Criteria = "200*" };
            var filterArray = new WSVendorNAV.WSVendor_Filter[] { filter };

            try
            {
                WSVendorNAV.ReadMultiple_Result result = await WS_Vendor.ReadMultipleAsync(filterArray, null, size);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSVendorNAV.Create_Result> CreateAsync(FornecedorDetailsViewModel vendor, NAVWSConfigurations WSConfigurations)
        {
            if (vendor == null)
                throw new ArgumentNullException("vendor");

            WSVendorNAV.Create navCreate = new WSVendorNAV.Create()
            {
                WSVendor = MapVendorModelToVendorNAV(vendor)
            };
            navCreate.WSVendor.No = null;

            //Configure NAV Vendor
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Vendor_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSVendorNAV.WSVendor_PortClient ws_Vendor = new WSVendorNAV.WSVendor_PortClient(navWSBinding, ws_URL);
            ws_Vendor.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Vendor.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                navCreate.WSVendor.Utilizador_Alteracao_eSUCH = vendor.Utilizador_Alteracao_eSUCH;

                navCreate.WSVendor.BlockedSpecified = true;
                navCreate.WSVendor.Bloqueado_Falta_PagamentoSpecified = true;
                navCreate.WSVendor.Bloqueado_UrgenteSpecified = true;
                navCreate.WSVendor.CertificaçãoSpecified = true;
                navCreate.WSVendor.ClassificaçãoSpecified = true;
                navCreate.WSVendor.CriticidadeSpecified = true;
                navCreate.WSVendor.Interface_ComprasSpecified = true;
                navCreate.WSVendor.PontuaçãoSpecified = true;
                navCreate.WSVendor.PreferencialSpecified = true;

                WSVendorNAV.Create_Result result = await ws_Vendor.CreateAsync(navCreate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSVendorNAV.Update_Result> UpdateAsync(FornecedorDetailsViewModel vendor, NAVWSConfigurations WSConfigurations)
        {
            if (vendor == null)
                throw new ArgumentNullException("vendor");


            WSVendorNAV.Update navUpdate = new WSVendorNAV.Update()
            {
                WSVendor = MapVendorModelToVendorNAV(vendor)
            };

            //Configure NAV Vendor
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Vendor_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSVendorNAV.WSVendor_PortClient ws_Vendor = new WSVendorNAV.WSVendor_PortClient(navWSBinding, ws_URL);
            ws_Vendor.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Vendor.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            WSVendorNAV.Read_Result resultRead = await ws_Vendor.ReadAsync(navUpdate.WSVendor.No);
            navUpdate.WSVendor.Key = resultRead.WSVendor.Key;

            try
            {
                navUpdate.WSVendor.Utilizador_Alteracao_eSUCH = vendor.Utilizador_Alteracao_eSUCH;

                navUpdate.WSVendor.BlockedSpecified = true;
                navUpdate.WSVendor.Bloqueado_Falta_PagamentoSpecified = true;
                navUpdate.WSVendor.Bloqueado_UrgenteSpecified = true;
                navUpdate.WSVendor.CertificaçãoSpecified = true;
                navUpdate.WSVendor.ClassificaçãoSpecified = true;
                navUpdate.WSVendor.CriticidadeSpecified = true;
                navUpdate.WSVendor.Interface_ComprasSpecified = true;
                navUpdate.WSVendor.PontuaçãoSpecified = true;
                navUpdate.WSVendor.PreferencialSpecified = true;

                WSVendorNAV.Update_Result result = await ws_Vendor.UpdateAsync(navUpdate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSVendorNAV.Delete_Result> DeleteAsync(string VendorNo, NAVWSConfigurations WSConfigurations)
        {
            if (VendorNo == null)
                throw new ArgumentNullException("VendorNo");

            //Configure NAV Vendor
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_Vendor_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSVendorNAV.WSVendor_PortClient ws_Vendor = new WSVendorNAV.WSVendor_PortClient(navWSBinding, ws_URL);
            ws_Vendor.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Vendor.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSVendorNAV.Read_Result resultRead = await ws_Vendor.ReadAsync(VendorNo);
                WSVendorNAV.Delete_Result result = await ws_Vendor.DeleteAsync(resultRead.WSVendor.Key);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #region Mappers

        public static FornecedorDetailsViewModel MapVendorNAVToVendorModel(WSVendorNAV.WSVendor VendorNAV)
        {
            FornecedorDetailsViewModel VendorVM = new FornecedorDetailsViewModel();
            if (VendorNAV != null)
            {
                VendorVM.No = VendorNAV.No;
                VendorVM.Name = VendorNAV.Name;
                VendorVM.FullAddress = VendorNAV.Address + " " + VendorNAV.Address_2;
                VendorVM.PostCode = VendorNAV.Post_Code;
                VendorVM.City = VendorNAV.City;
                VendorVM.Country = VendorNAV.Country_Region_Code;
                VendorVM.Phone = VendorNAV.Phone_No;
                VendorVM.Email = VendorNAV.E_Mail;
                VendorVM.Fax = VendorNAV.Fax_No;
                VendorVM.HomePage = VendorNAV.Home_Page;
                VendorVM.VATRegistrationNo = VendorNAV.VAT_Registration_No;
                VendorVM.PaymentTermsCode = VendorNAV.Payment_Terms_Code;
                VendorVM.PaymentMethodCode = VendorNAV.Payment_Method_Code;
                VendorVM.NoClienteAssociado = VendorNAV.No_Cliente_Assoc;
                VendorVM.Blocked = (int)VendorNAV.Blocked;
                VendorVM.Address = VendorNAV.Address;
                VendorVM.Address_2 = VendorNAV.Address_2;
                VendorVM.Distrito = VendorNAV.County;
                VendorVM.Criticidade = (int)VendorNAV.Criticidade;
                VendorVM.Observacoes = VendorNAV.Observacoes;
            }

            return VendorVM;
        }

        public static WSVendorNAV.WSVendor MapVendorModelToVendorNAV(FornecedorDetailsViewModel VendorModel)
        {
            WSVendorNAV.WSVendor VendorNAV = new WSVendorNAV.WSVendor();
            if (VendorModel != null)
            {
                VendorNAV.No = VendorModel.No;
                VendorNAV.Name = VendorModel.Name;
                VendorNAV.Post_Code = VendorModel.PostCode;
                VendorNAV.City = VendorModel.City;
                VendorNAV.Country_Region_Code = VendorModel.Country;
                VendorNAV.Phone_No = VendorModel.Phone;
                VendorNAV.E_Mail = VendorModel.Email;
                VendorNAV.Fax_No = VendorModel.Fax;
                VendorNAV.Home_Page = VendorModel.HomePage;
                VendorNAV.VAT_Registration_No = VendorModel.VATRegistrationNo; 
                VendorNAV.Payment_Terms_Code = VendorModel.PaymentTermsCode;
                VendorNAV.Payment_Method_Code = VendorModel.PaymentMethodCode;
                VendorNAV.No_Cliente_Assoc = VendorModel.NoClienteAssociado;
                VendorNAV.Blocked = (Blocked)VendorModel.Blocked;
                VendorNAV.Address = VendorModel.Address;
                VendorNAV.Address_2 = VendorModel.Address_2;
                VendorNAV.County = VendorModel.Distrito;
                VendorNAV.Criticidade = (Criticidade)VendorModel.Criticidade;
                VendorNAV.Observacoes = VendorModel.Observacoes;
            }

            return VendorNAV;
        }

        #endregion

    }
}
