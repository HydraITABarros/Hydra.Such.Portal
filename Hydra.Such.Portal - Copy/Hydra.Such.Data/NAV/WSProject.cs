using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Projects;
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
    public static class WSProject
    {
        static BasicHttpBinding navWSBinding;

        static WSProject()
        {
            ///Configure Basic Binding to have access to NAV
            navWSBinding = new BasicHttpBinding();
            navWSBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            navWSBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        }

        public static async Task<WSCreateNAVProject.Create_Result> CreateNavProject(ProjectDetailsViewModel ProjectToCreate, NAVWSConfigurations WSConfigurations)
        {
            //Mapping WSJob Object
            WSCreateNAVProject.Status StatusValue;
            switch (ProjectToCreate.Status)
            {
                case 1:
                    StatusValue = WSCreateNAVProject.Status.Planning;
                    break;
                case 2:
                    StatusValue = WSCreateNAVProject.Status.Quote;
                    break;
                case 3:
                    StatusValue = WSCreateNAVProject.Status.Open;
                    break;
                default:
                    StatusValue = WSCreateNAVProject.Status.Completed;
                    break;

            }
            
            WSCreateNAVProject.Create NAVCreate = new WSCreateNAVProject.Create()
            {
                WSJob = new WSCreateNAVProject.WSJob()
                {
                    No = ProjectToCreate.ProjectNo,
                    Description100 = ProjectToCreate.Description,
                    Bill_to_Customer_No = ProjectToCreate.ClientNo,
                    Status = StatusValue,
                    RegionCode20 = ProjectToCreate.RegionCode,
                    FunctionAreaCode20 = ProjectToCreate.FunctionalAreaCode,
                    ResponsabilityCenterCode20 = ProjectToCreate.ResponsabilityCenterCode,
                    Job_Posting_Group = "",
                    Project_Manager = ProjectToCreate.ProjectLeader,
                    Person_Responsible = ProjectToCreate.ProjectResponsible
                }
            };


            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Job_URL.Replace("Company",WSConfigurations.WS_User_Company));
            WSCreateNAVProject.WSJob_PortClient WS_Client = new WSCreateNAVProject.WSJob_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreateNAVProject.Create_Result result = await WS_Client.CreateAsync(NAVCreate);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
