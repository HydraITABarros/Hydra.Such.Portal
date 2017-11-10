//using System;
//using System.Collections.Generic;
//using System.ServiceModel;
//using System.Text;
//using System.Threading.Tasks;
//using System.Net;

using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Such.Data.NAV
{
    public static class NAVContactsService
    {
        static BasicHttpBinding navWSBinding;

        static NAVContactsService()
        {
            ///Configure Basic Binding to have access to NAV
            //navWSBinding = new BasicHttpBinding();
            //navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            //navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }

        //public static async Task<WSContacts.Create_Result> CreateNavProject(ContactsViewModel ProjectToCreate, NAVWSConfigurations WSConfigurations)
        //{
            ////Mapping WSJob Object
            //WSContacts.Status StatusValue;
            //switch (ProjectToCreate.Status)
            //{
            //    case 1:
            //        StatusValue = WSContacts.Status.Planning;
            //        break;
            //    case 2:
            //        StatusValue = WSContacts.Status.Quote;
            //        break;
            //    case 3:
            //        StatusValue = WSContacts.Status.Open;
            //        break;
            //    default:
            //        StatusValue = WSContacts.Status.Completed;
            //        break;

            //}

            //WSContacts.Create NAVCreate = new WSContacts.Create()
            //{
            //    WSJob = new WSContacts.WSJob()
            //    {
            //        No = ProjectToCreate.ProjectNo,
            //        Description100 = ProjectToCreate.Description,
            //        Bill_to_Customer_No = ProjectToCreate.ClientNo,
            //        Status = StatusValue,
            //        RegionCode20 = ProjectToCreate.RegionCode,
            //        FunctionAreaCode20 = ProjectToCreate.FunctionalAreaCode,
            //        ResponsabilityCenterCode20 = ProjectToCreate.ResponsabilityCenterCode,
            //        Job_Posting_Group = "",
            //        Project_Manager = ProjectToCreate.ProjectLeader,
            //        Person_Responsible = ProjectToCreate.ProjectResponsible
            //    }
            //};


            ////Configure NAV Client
            //EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Job_URL.Replace("Company", WSConfigurations.WS_User_Company));
            //WSContacts.WSJob_PortClient WS_Client = new WSContacts.WSJob_PortClient(navWSBinding, WS_URL);
            //WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            //WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            //try
            //{
            //    WSContacts.Create_Result result = await WS_Client.CreateAsync(NAVCreate);
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

        //}
    }
}
