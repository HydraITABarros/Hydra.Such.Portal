using Hydra.Such.Data.ViewModel;
using System;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Hydra.Such.Data.NAV
{
    public class WSPreInvoiceLine
    {
        static BasicHttpBinding navWSBinding;

        static WSPreInvoiceLine()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }

        public static async Task<WSCreatePreInvoiceLine.Create_Result> CreatePreInvoiceLine(ProjectDiaryViewModel PreInvoiceLineToCreate, NAVWSConfigurations WSConfigurations, string PKey)
        {
            //Mapping Object
            WSCreatePreInvoiceLine.Type TypeValue;
            switch (PreInvoiceLineToCreate.Type)
            {
                case 1:
                    TypeValue = WSCreatePreInvoiceLine.Type.Resource;
                    break;
                case 2:
                    TypeValue = WSCreatePreInvoiceLine.Type.Item;
                    break;
                case 3:
                    TypeValue = WSCreatePreInvoiceLine.Type.G_L_Account;
                    break;
                case 4:
                    TypeValue = WSCreatePreInvoiceLine.Type.Fixed_Asset;
                    break;
                case 5:
                    TypeValue = WSCreatePreInvoiceLine.Type.Charge_Item;
                    break;
                default:
                    TypeValue = WSCreatePreInvoiceLine.Type._blank_;
                    break;

            }

            WSCreatePreInvoiceLine.Create NAVCreate = new WSCreatePreInvoiceLine.Create()
            {
                WsPreInvoiceLine = new WSCreatePreInvoiceLine.WsPreInvoiceLine()
                {
                    Unit_PriceSpecified = true,
                    Unit_Cost_LCYSpecified = true,
                    Document_Type = WSCreatePreInvoiceLine.Document_Type.Invoice,
                    Document_No = PKey,
                    Type = TypeValue,
                    No = PreInvoiceLineToCreate.Code,
                    Description = PreInvoiceLineToCreate.Description,
                    QuantitySpecified = true,
                    Quantity = (int)PreInvoiceLineToCreate.Quantity,
                    TypeSpecified = true,
                    Unit_of_Measure = PreInvoiceLineToCreate.MeasurementUnitCode,
                    Location_Code = PreInvoiceLineToCreate.LocationCode,
                    Unit_Price = (decimal)PreInvoiceLineToCreate.UnitPrice,
                    Unit_Cost_LCY = (decimal)PreInvoiceLineToCreate.UnitCost,
                    Job_No = PreInvoiceLineToCreate.ProjectNo,
                    Job_Journal_Line_No_Portal = PreInvoiceLineToCreate.LineNo,
                    Job_Journal_Line_No_PortalSpecified = true

                }
            };

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_PreInvoiceLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreatePreInvoiceLine.WsPreInvoiceLine_PortClient WS_Client = new WSCreatePreInvoiceLine.WsPreInvoiceLine_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreatePreInvoiceLine.Create_Result result = await WS_Client.CreateAsync(NAVCreate);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
