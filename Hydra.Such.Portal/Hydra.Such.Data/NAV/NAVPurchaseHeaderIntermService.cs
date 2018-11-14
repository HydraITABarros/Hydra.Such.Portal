using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using Hydra.Such.Data.ViewModel.Compras;

namespace Hydra.Such.Data.NAV
{
    public static class NAVPurchaseHeaderIntermService
    {
        static BasicHttpBinding navWSBinding;

        static NAVPurchaseHeaderIntermService()
        {
            // Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows; 
        }

        public static async Task<WSPurchaseInvHeader.Create_Result> CreateAsync(PurchOrderDTO purchFromSupplier, NAVWSConfigurations WSConfigurations)
        {
            if (purchFromSupplier == null)
                throw new ArgumentNullException("purchFromSupplier");

            int localMarketRegion;
            if (int.TryParse(purchFromSupplier.LocalMarketRegion, out localMarketRegion))
                localMarketRegion--;

            WSPurchaseInvHeader.Create navCreate = new WSPurchaseInvHeader.Create()
            {
                WSPurchInvHeaderInterm = new WSPurchaseInvHeader.WSPurchInvHeaderInterm()
                {
                    Buy_from_Vendor_No = purchFromSupplier.SupplierId,
                    Pay_to_Vendor_No = purchFromSupplier.SupplierId,
                    LocationCode = purchFromSupplier.LocationCode,
                    RegionCode20 = purchFromSupplier.RegionCode,
                    FunctionAreaCode20 = purchFromSupplier.FunctionalAreaCode,
                    ResponsabilityCenterCode20 = purchFromSupplier.CenterResponsibilityCode,
                    //Your_Reference = purchFromSupplier
                    //Observacoes = purchFromSupplier;
                    //Purchaser_Code = purchFromSupplier;
                    //N_Consulta = purchFromSupplier;
                    //Expected_Receipt_Date = purchFromSupplier;
                    //Expected_Receipt_DateSpecified = true;
                    //Responsibility_Center = purchFromSupplier;
                    //V_Prop_Num = purchFromSupplier;
                    //Ship_to_Name = purchFromSupplier;
                    //Ship_to_Name_2 = purchFromSupplier;
                    //Ship_to_Address = purchFromSupplier;
                    //Ship_to_Address_2 = purchFromSupplier;
                    //Ship_to_City = purchFromSupplier;
                    //Ship_to_Contact = purchFromSupplier;
                    //Ship_to_Post_Code = purchFromSupplier;
                    //Ship_to_County = purchFromSupplier;
                    //Ship_to_Country_Region_Code = purchFromSupplier;
                    Requisition_No = purchFromSupplier.RequisitionId,
                    //Payment_Terms_Code = purchFromSupplier;
                    Mercado_Local_Regiao = (WSPurchaseInvHeader.Mercado_Local_Regiao)localMarketRegion,
                    Mercado_Local_RegiaoSpecified = true,
                    //Motivo_Anulacao_Encomenda = purchFromSupplier,
                    //Motivo_Anulacao_EncomendaSpecified = true;
                    //Encomenda_Origem_NAV17 = purchFromSupplier,
                    //Utilizador_Criacao = purchFromSupplier,
                    Prices_Including_VAT = purchFromSupplier.PricesIncludingVAT.HasValue ? purchFromSupplier.PricesIncludingVAT.Value : false,
                    Prices_Including_VATSpecified = purchFromSupplier.PricesIncludingVAT.HasValue,
                    Down_Payment = purchFromSupplier.InAdvance.HasValue ? purchFromSupplier.InAdvance.Value : false,
                    Down_PaymentSpecified = purchFromSupplier.InAdvance.HasValue,
                    Vendor_Mail = purchFromSupplier.Vendor_Mail,
                    N_Consulta = purchFromSupplier.NAVPrePurchOrderId,
                    Purchaser_Code = purchFromSupplier.NAVPrePurchOrderId


                }
            };
            
            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_PurchaseInvIntermHeader_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSPurchaseInvHeader.WSPurchInvHeaderInterm_PortClient ws_Client = new WSPurchaseInvHeader.WSPurchInvHeaderInterm_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
                return await ws_Client.CreateAsync(navCreate);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        public static async Task<WSPurchaseInvHeader.Create_Result> CreateAsync(PurchOrderDTO purchFromSupplier, NAVWSConfigurations WSConfigurations, DateTime DataRececao)
        {
            if (purchFromSupplier == null)
                throw new ArgumentNullException("purchFromSupplier");

            int localMarketRegion;
            if (int.TryParse(purchFromSupplier.LocalMarketRegion, out localMarketRegion))
                localMarketRegion--;

            WSPurchaseInvHeader.Create navCreate = new WSPurchaseInvHeader.Create()
            {
                WSPurchInvHeaderInterm = new WSPurchaseInvHeader.WSPurchInvHeaderInterm()
                {
                    Buy_from_Vendor_No = purchFromSupplier.SupplierId,
                    Pay_to_Vendor_No = purchFromSupplier.SupplierId,
                    LocationCode = purchFromSupplier.LocationCode,
                    RegionCode20 = purchFromSupplier.RegionCode,
                    FunctionAreaCode20 = purchFromSupplier.FunctionalAreaCode,
                    ResponsabilityCenterCode20 = purchFromSupplier.CenterResponsibilityCode,
                    //Your_Reference = purchFromSupplier
                    //Observacoes = purchFromSupplier;
                    //Purchaser_Code = purchFromSupplier;
                    //N_Consulta = purchFromSupplier;
                    //Expected_Receipt_Date = purchFromSupplier;
                    //Expected_Receipt_DateSpecified = true;
                    //Responsibility_Center = purchFromSupplier;
                    //V_Prop_Num = purchFromSupplier;
                    //Ship_to_Name = purchFromSupplier;
                    //Ship_to_Name_2 = purchFromSupplier;
                    //Ship_to_Address = purchFromSupplier;
                    //Ship_to_Address_2 = purchFromSupplier;
                    //Ship_to_City = purchFromSupplier;
                    //Ship_to_Contact = purchFromSupplier;
                    //Ship_to_Post_Code = purchFromSupplier;
                    //Ship_to_County = purchFromSupplier;
                    //Ship_to_Country_Region_Code = purchFromSupplier;
                    Requisition_No = purchFromSupplier.RequisitionId,
                    //Payment_Terms_Code = purchFromSupplier;
                    Mercado_Local_Regiao = (WSPurchaseInvHeader.Mercado_Local_Regiao)localMarketRegion,
                    Mercado_Local_RegiaoSpecified = true,
                    //Motivo_Anulacao_Encomenda = purchFromSupplier,
                    //Motivo_Anulacao_EncomendaSpecified = true;
                    //Encomenda_Origem_NAV17 = purchFromSupplier,
                    //Utilizador_Criacao = purchFromSupplier,
                    Prices_Including_VAT = purchFromSupplier.PricesIncludingVAT.HasValue ? purchFromSupplier.PricesIncludingVAT.Value : false,
                    Prices_Including_VATSpecified = purchFromSupplier.PricesIncludingVAT.HasValue,
                    Down_Payment = purchFromSupplier.InAdvance.HasValue ? purchFromSupplier.InAdvance.Value : false,
                    Down_PaymentSpecified = purchFromSupplier.InAdvance.HasValue,
                    Vendor_Mail = purchFromSupplier.Vendor_Mail,
                    N_Consulta = purchFromSupplier.NAVPrePurchOrderId,
                    Expected_Receipt_Date = DataRececao,
                    Expected_Receipt_DateSpecified = true,
                    Purchaser_Code = purchFromSupplier.NAVPrePurchOrderId
                }
            };

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_PurchaseInvIntermHeader_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSPurchaseInvHeader.WSPurchInvHeaderInterm_PortClient ws_Client = new WSPurchaseInvHeader.WSPurchInvHeaderInterm_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
                return await ws_Client.CreateAsync(navCreate);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        public static async Task<WSPurchaseInvHeader.Create_Result> CreateAsync_CM(PurchOrderDTO purchFromSupplier, NAVWSConfigurations WSConfigurations, string Consulta, string Observacoes)
        {
            if (purchFromSupplier == null)
                throw new ArgumentNullException("purchFromSupplier");

            int localMarketRegion;
            if (int.TryParse(purchFromSupplier.LocalMarketRegion, out localMarketRegion))
                localMarketRegion--;

            WSPurchaseInvHeader.Create navCreate = new WSPurchaseInvHeader.Create()
            {
                WSPurchInvHeaderInterm = new WSPurchaseInvHeader.WSPurchInvHeaderInterm()
                {
                    Buy_from_Vendor_No = purchFromSupplier.SupplierId,
                    Pay_to_Vendor_No = purchFromSupplier.SupplierId,
                    LocationCode = purchFromSupplier.LocationCode,
                    RegionCode20 = purchFromSupplier.RegionCode,
                    FunctionAreaCode20 = purchFromSupplier.FunctionalAreaCode,
                    ResponsabilityCenterCode20 = purchFromSupplier.CenterResponsibilityCode,
                    //Your_Reference = purchFromSupplier
                    //Observacoes = purchFromSupplier;
                    //Purchaser_Code = purchFromSupplier;
                    //N_Consulta = purchFromSupplier;
                    //Expected_Receipt_Date = purchFromSupplier;
                    //Expected_Receipt_DateSpecified = true;
                    //Responsibility_Center = purchFromSupplier;
                    //V_Prop_Num = purchFromSupplier;
                    //Ship_to_Name = purchFromSupplier;
                    //Ship_to_Name_2 = purchFromSupplier;
                    //Ship_to_Address = purchFromSupplier;
                    //Ship_to_Address_2 = purchFromSupplier;
                    //Ship_to_City = purchFromSupplier;
                    //Ship_to_Contact = purchFromSupplier;
                    //Ship_to_Post_Code = purchFromSupplier;
                    //Ship_to_County = purchFromSupplier;
                    //Ship_to_Country_Region_Code = purchFromSupplier;
                    Requisition_No = purchFromSupplier.RequisitionId,
                    //Payment_Terms_Code = purchFromSupplier;
                    Mercado_Local_Regiao = (WSPurchaseInvHeader.Mercado_Local_Regiao)localMarketRegion,
                    Mercado_Local_RegiaoSpecified = true,
                    //Motivo_Anulacao_Encomenda = purchFromSupplier,
                    //Motivo_Anulacao_EncomendaSpecified = true;
                    //Encomenda_Origem_NAV17 = purchFromSupplier,
                    //Utilizador_Criacao = purchFromSupplier,
                    Prices_Including_VAT = purchFromSupplier.PricesIncludingVAT.HasValue ? purchFromSupplier.PricesIncludingVAT.Value : false,
                    Prices_Including_VATSpecified = purchFromSupplier.PricesIncludingVAT.HasValue,
                    Down_Payment = purchFromSupplier.InAdvance.HasValue ? purchFromSupplier.InAdvance.Value : false,
                    Down_PaymentSpecified = purchFromSupplier.InAdvance.HasValue,
                    Vendor_Mail = purchFromSupplier.Vendor_Mail,
                    N_Consulta = Consulta,
                    Observacoes = Observacoes,
                    Purchaser_Code = purchFromSupplier.NAVPrePurchOrderId
                }
            };

            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_PurchaseInvIntermHeader_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSPurchaseInvHeader.WSPurchInvHeaderInterm_PortClient ws_Client = new WSPurchaseInvHeader.WSPurchInvHeaderInterm_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
            return await ws_Client.CreateAsync(navCreate);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
    }
}
