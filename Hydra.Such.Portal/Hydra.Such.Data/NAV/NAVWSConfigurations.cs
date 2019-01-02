using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.NAV
{
    public class NAVWSConfigurations
    {
        public string WS_User_Company { get; set; }
        public string WS_User_Login { get; set; }
        public string WS_User_Password { get; set; }
        public string WS_User_Domain { get; set; }
        public string WS_Generic_URL { get; set; }
        public string WS_Job_URL { get; set; }
        public string WS_JobJournalLine_URL { get; set; }
        public string WS_PreBlanketOrder_URL { get; set; }
        public string WS_PreInvoice_URL { get; set; }
        public string WS_PreInvoiceLine_URL { get; set; }
        public string WS_PrePurchase_URL { get; set; }
        public string WS_Contacts_URL { get; set; }
        public string WS_PurchaseInvIntermHeader_URL { get; set; }
        public string WS_PurchaseInvLine_URL { get; set; }
        public string WS_TransferShipmentHeader_URL { get; set; }
        public string WS_TransferShipmentLine_URL { get; set; }
        public string WS_Client_URL { get; set; }
        public string WS_Customer_URL { get; set; }
        public string WS_ShipToAddress_URL { get; set; }
        public string WS_PurchaseHeaderDocs_URL { get; set; }

        // zpgm.20122018
        public string Ws_SuchNav2017_URL { get; set; }
    }
}
