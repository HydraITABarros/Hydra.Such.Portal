using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.NAV
{
    public class WSProjectDiaryLine
    {
        public bool CreateLine (ProjectDiaryViewModel dp, int operation)
        {
            // Create Transaction ID

            // WS to Create Line in NAV
            WSCreateProjectDiaryLine.WSJobJournalLine ws = new WSCreateProjectDiaryLine.WSJobJournalLine
            {
                //ws.Line_No = transactionID;
                Document_No = dp.ProjectNo,
                Document_Date = dp.Date.Value,
                //ws.Ledger_Entry_Type = dp.MovementType;
                //Type = x.Type,
                //TypeCode = dp.Code;
                Description100 = dp.Description,
                Quantity = dp.Quantity.Value,
                Unit_of_Measure_Code = dp.MeasurementUnitCode,
                Location_Code = dp.LocationCode,
                RegionCode20 = dp.RegionCode,
                FunctionAreaCode20 = dp.FunctionalAreaCode,
                ResponsabilityCenterCode20 = dp.ResponsabilityCenterCode,
                // user ?
                Unit_Cost = dp.UnitCost.Value,
                Total_Cost = dp.TotalCost.Value,
                Unit_Price = dp.UnitPrice.Value,
                Total_Price = dp.TotalPrice.Value,
                Chargeable = dp.Billable.Value
                // x.InvoiceToClientNo
            };

            try
            {
                if (operation == 0)
                {
                    // Create Line

                    // Success
                    return true;

                }
                else if(operation == 1)
                {
                    // Update

                    // Success
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        

    }
}