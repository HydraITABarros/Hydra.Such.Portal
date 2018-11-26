using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;
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

        public static void DeleteNavDiaryLines(Guid TransactID, NAVWSConfigurations WSConfigurations)
        {
            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_JobJournalLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreateProjectDiaryLine.WSJobJournalLine_PortClient WS_Client = new WSCreateProjectDiaryLine.WSJobJournalLine_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            var filter = new WSCreateProjectDiaryLine.WSJobJournalLine_Filter { Field = WSCreateProjectDiaryLine.WSJobJournalLine_Fields.Portal_Transaction_No, Criteria = TransactID.ToString() };
            var filterArray = new WSCreateProjectDiaryLine.WSJobJournalLine_Filter[] { filter };

            var result =  WS_Client.ReadMultipleAsync(filterArray, null, 100);
            
            if (result != null && result.Result != null)
            {
                foreach (var line in result.Result.ReadMultiple_Result1)
                {
                    WS_Client.DeleteAsync(line.Key);
                }
            }
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
                    Document_No = "ES_" + y.ProjectNo,
                    TypeSpecified = true,
                    Type = getType(Convert.ToInt32(y.Type)),
                    Description100 = y.Description,
                    FunctionAreaCode20 = y.FunctionalAreaCode,
                    ResponsabilityCenterCode20 = y.ResponsabilityCenterCode,
                    RegionCode20 = y.RegionCode,
                    Location_Code = y.LocationCode,
                    No = y.Code,
                    Posting_DateSpecified = true,
                    Posting_Date = string.IsNullOrEmpty(y.Date) ? DateTime.Now : DateTime.Parse(y.Date),
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
           
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_JobJournalLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreateProjectDiaryLine.WSJobJournalLine_PortClient WS_Client = new WSCreateProjectDiaryLine.WSJobJournalLine_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                foreach (var test in NAVCreate.WSJobJournalLine_List)
                {
                    WSCreateProjectDiaryLine.Create toCreate = new WSCreateProjectDiaryLine.Create();
                    toCreate.WSJobJournalLine = test;
                    WSCreateProjectDiaryLine.Create_Result result = await WS_Client.CreateAsync(toCreate);

                    if (result != null)
                    {
                        WSCreateProjectDiaryLine.Update toUpdate = new WSCreateProjectDiaryLine.Update()
                        {
                            WSJobJournalLine = new WSCreateProjectDiaryLine.WSJobJournalLine()
                            {
                                Key = result.WSJobJournalLine.Key,
                                Line_No = result.WSJobJournalLine.Line_No,
                                Portal_Transaction_No = TransactID.ToString(),
                                Job_No = result.WSJobJournalLine.Job_No,
                                Document_DateSpecified = result.WSJobJournalLine.Document_DateSpecified,
                                Document_Date = result.WSJobJournalLine.Document_Date,
                                Document_No = result.WSJobJournalLine.Document_No,
                                TypeSpecified = true,
                                Type = result.WSJobJournalLine.Type,
                                Description100 = result.WSJobJournalLine.Description100,
                                FunctionAreaCode20 = result.WSJobJournalLine.FunctionAreaCode20,
                                ResponsabilityCenterCode20 = result.WSJobJournalLine.ResponsabilityCenterCode20,
                                RegionCode20 = result.WSJobJournalLine.RegionCode20,
                                Location_Code = result.WSJobJournalLine.Location_Code,
                                No = result.WSJobJournalLine.No,
                                Posting_DateSpecified = true,
                                Posting_Date = result.WSJobJournalLine.Posting_Date,
                                Unit_of_Measure_Code = result.WSJobJournalLine.Unit_of_Measure_Code,
                                ChargeableSpecified = true,
                                Chargeable = result.WSJobJournalLine.Chargeable,
                                QuantitySpecified = true,
                                Quantity = result.WSJobJournalLine.Quantity,
                                Unit_Cost = test.Unit_Cost,
                                Unit_CostSpecified = true,
                                Unit_Price = test.Unit_Price,
                                Unit_PriceSpecified = true,
                            }
                        };
                        WS_Client = new WSCreateProjectDiaryLine.WSJobJournalLine_PortClient(navWSBinding, WS_URL);
                        WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
                        WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);
                        WSCreateProjectDiaryLine.Update_Result resultUpdate = await WS_Client.UpdateAsync(toUpdate);

                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<WSCreateProjectDiaryLine.CreateMultiple_Result> CreateNavDiaryLines(List<RequisitionLineViewModel> items, Guid transactionId, NAVWSConfigurations WSConfigurations)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            List<WSCreateProjectDiaryLine.WSJobJournalLine> jobJournalLines = new List<WSCreateProjectDiaryLine.WSJobJournalLine>();
            int lineNo = 0;
            foreach (var item in items)
            {
                WSCreateProjectDiaryLine.WSJobJournalLine line = new WSCreateProjectDiaryLine.WSJobJournalLine();
                line.Line_No = lineNo += 1000;
                line.Line_NoSpecified = true;
                line.Document_No = "ES_" + item.RequestNo;//.ProjectNo;
                line.Job_No = item.ProjectNo;
                line.Document_Date = item.CreateDateTime.HasValue ? item.CreateDateTime.Value : DateTime.Now;
                line.Document_DateSpecified = true;
                line.Posting_Date = DateTime.Now;
                line.Posting_DateSpecified = true;
                line.Entry_Type = WSCreateProjectDiaryLine.Entry_Type.Usage;
                line.Entry_TypeSpecified = true;
                line.Type = WSCreateProjectDiaryLine.Type.Item;
                line.TypeSpecified = true;
                line.No = item.Code;
                line.Description100 = item.Description;
                line.Quantity = item.QuantityReceived.Value;
                line.QuantitySpecified = true;
                line.Unit_of_Measure_Code = item.UnitMeasureCode;
                line.Location_Code = item.LocalCode;
                line.RegionCode20 = item.RegionCode;
                line.FunctionAreaCode20 = item.FunctionalAreaCode;
                line.Portal_Transaction_No = transactionId.ToString();
                jobJournalLines.Add(line);
            }

            WSCreateProjectDiaryLine.CreateMultiple navCreate = new WSCreateProjectDiaryLine.CreateMultiple(jobJournalLines.ToArray());


            //Configure NAV Client
            EndpointAddress ws_URL = new EndpointAddress(WSConfigurations.WS_JobJournalLine_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreateProjectDiaryLine.WSJobJournalLine_PortClient ws_Client = new WSCreateProjectDiaryLine.WSJobJournalLine_PortClient(navWSBinding, ws_URL);
            ws_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            ws_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
            WSCreateProjectDiaryLine.CreateMultiple_Result result = await ws_Client.CreateMultipleAsync(navCreate);
            return result;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

        }

        public static async Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> RegsiterNavDiaryLines(Guid TransactID, NAVWSConfigurations WSConfigurations)
        {
            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Generic_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSGenericCodeUnit.WsGeneric_PortClient WS_Client = new WSGenericCodeUnit.WsGeneric_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
                WSGenericCodeUnit.FxPostJobJrnlLines_Result result = await WS_Client.FxPostJobJrnlLinesAsync(TransactID.ToString());

                return result;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
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
                    return WSCreateProjectDiaryLine.Type.Item;
                case 2:
                    return WSCreateProjectDiaryLine.Type.Resource;
                case 3:
                    return WSCreateProjectDiaryLine.Type.G_L_Account;
                default:
                    return WSCreateProjectDiaryLine.Type.Item;
            }
        }
    }
}