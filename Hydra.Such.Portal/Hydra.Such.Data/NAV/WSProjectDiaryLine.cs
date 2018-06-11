using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Hydra.Such.Data.NAV
{
    public static class WSProjectDiaryLine
    {
        static BasicHttpBinding navWSBinding;

        static WSProjectDiaryLine()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

        }

        public static async Task<WSCreateProjectDiaryLine.CreateMultiple_Result> CreateNavDiaryLines(List<ProjectDiaryViewModel> DiaryLines, Guid TransactID, NAVWSConfigurations WSConfigurations)
        {
            WSCreateProjectDiaryLine.CreateMultiple NAVCreate = new WSCreateProjectDiaryLine.CreateMultiple()
            {                
                WSJobJournalLine_List = DiaryLines.Select(y => new WSCreateProjectDiaryLine.WSJobJournalLine()
                {
                    Job_No = y.ProjectNo,                  
                    Document_DateSpecified = string.IsNullOrEmpty(y.Date) ? false : true,
                    Document_Date = string.IsNullOrEmpty(y.Date) ? DateTime.Now : DateTime.Parse(y.Date),
                    //Entry_TypeSpecified = true,
                    //Entry_Type = getMoveType(Convert.ToInt32(y.MovementType)),
                    TypeSpecified = true,
                    Type = getType(Convert.ToInt32(y.Type)),
                    Description100 = y.Description,
                    FunctionAreaCode20 = y.FunctionalAreaCode,
                    ResponsabilityCenterCode20 = y.ResponsabilityCenterCode,
                    RegionCode20 = y.RegionCode,
                    Location_Code = y.LocationCode,
                    Unit_of_Measure_Code = y.MeasurementUnitCode,
                    ChargeableSpecified = true,
                    Chargeable = Convert.ToBoolean(y.Billable),
                    QuantitySpecified = true,
                    Quantity = Convert.ToDecimal(y.Quantity),
                    Unit_CostSpecified = true,
                    Unit_Cost = Convert.ToDecimal(y.UnitCost),
                    //Total_CostSpecified = true,
                    //Total_Cost = Convert.ToDecimal(y.TotalCost),
                    Unit_PriceSpecified = true,
                    Unit_Price = Convert.ToDecimal(y.UnitPrice),
                    //Total_PriceSpecified = true,
                    //Total_Price = Convert.ToDecimal(y.TotalPrice),
                    Portal_Transaction_No = TransactID.ToString()
                }).ToArray()
            };

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_JobJournalLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreateProjectDiaryLine.WSJobJournalLine_PortClient WS_Client = new WSCreateProjectDiaryLine.WSJobJournalLine_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);
            
            try
            {
                WSCreateProjectDiaryLine.CreateMultiple_Result result = await WS_Client.CreateMultipleAsync(NAVCreate);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> RegsiterNavDiaryLines(Guid TransactID, NAVWSConfigurations WSConfigurations)
        {
            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Generic_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSGenericCodeUnit.WsGeneric_PortClient WS_Client = new WSGenericCodeUnit.WsGeneric_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSGenericCodeUnit.FxPostJobJrnlLines_Result result = await WS_Client.FxPostJobJrnlLinesAsync(TransactID.ToString());

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static WSCreateProjectDiaryLine.Entry_Type getMoveType(int moveType)
        {
            switch (moveType)
            {
                case 1:
                    return WSCreateProjectDiaryLine.Entry_Type.Usage;
                case 2:
                    return WSCreateProjectDiaryLine.Entry_Type.Sale;
                default:
                    return WSCreateProjectDiaryLine.Entry_Type.Usage;
            }
        }
        private static WSCreateProjectDiaryLine.Type getType(int type)
        {
            switch (type)
            {
                case 1:
                    return WSCreateProjectDiaryLine.Type.Resource;
                case 2:
                    return WSCreateProjectDiaryLine.Type.Item;
                case 3:
                    return WSCreateProjectDiaryLine.Type.G_L_Account;
                default:
                    return WSCreateProjectDiaryLine.Type.Item;
            }
        }
    }
}