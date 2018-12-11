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
using WSCreateNAVProject;
using static Hydra.Such.Data.Enumerations;

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
            
            WSCreateNAVProject.Create NAVCreate = new WSCreateNAVProject.Create()
            {
                WSJob = new WSCreateNAVProject.WSJob()
                {
                    No = ProjectToCreate.ProjectNo,
                    Description100 = ProjectToCreate.Description,
                    Bill_to_Customer_No = ProjectToCreate.ClientNo,
                    Estado_eSUCH = (Estado_eSUCH)((int)ProjectToCreate.Status),
                    RegionCode20 = ProjectToCreate.RegionCode,
                    FunctionAreaCode20 = ProjectToCreate.FunctionalAreaCode,
                    ResponsabilityCenterCode20 = ProjectToCreate.ResponsabilityCenterCode,
                    Job_Posting_Group = "",
                    Visivel = ProjectToCreate.Visivel == null ? true : (bool)ProjectToCreate.Visivel,                
                    Status = Status.Open
                    //Project_Manager = ProjectToCreate.ProjectLeader,
                    //Person_Responsible = ProjectToCreate.ProjectResponsible
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

        public static async Task<WSCreateNAVProject.Update_Result> UpdateNavProject(string Key, ProjectDetailsViewModel ProjectToUpdate, NAVWSConfigurations WSConfigurations)
        {   
            WSCreateNAVProject.Update NAVUpdate = new WSCreateNAVProject.Update()
            {
                WSJob = new WSCreateNAVProject.WSJob()
                {
                    Key = Key,
                    No = ProjectToUpdate.ProjectNo,
                    Description100 = ProjectToUpdate.Description,
                    Bill_to_Customer_No = ProjectToUpdate.ClientNo,
                    Estado_eSUCH = (Estado_eSUCH)((int)ProjectToUpdate.Status),
                    RegionCode20 = ProjectToUpdate.RegionCode,
                    FunctionAreaCode20 = ProjectToUpdate.FunctionalAreaCode,
                    ResponsabilityCenterCode20 = ProjectToUpdate.ResponsabilityCenterCode,
                    Job_Posting_Group = ""                    
                    //Project_Manager = ProjectToUpdate.ProjectLeader,
                    //Person_Responsible = ProjectToUpdate.ProjectResponsible
                }
            };


            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Job_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreateNAVProject.WSJob_PortClient WS_Client = new WSCreateNAVProject.WSJob_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreateNAVProject.Update_Result result = await WS_Client.UpdateAsync(NAVUpdate);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<WSCreateNAVProject.Delete_Result> DeleteNavProject(string Key, NAVWSConfigurations WSConfigurations)
        {
            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Job_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreateNAVProject.WSJob_PortClient WS_Client = new WSCreateNAVProject.WSJob_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreateNAVProject.Delete_Result result = await WS_Client.DeleteAsync(Key);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<WSCreateNAVProject.Read_Result> GetNavProject(string ProjectNo, NAVWSConfigurations WSConfigurations)
        {

            //Configure NAV Client
            EndpointAddress WS_URL = new EndpointAddress(WSConfigurations.WS_Job_URL.Replace("Company", WSConfigurations.WS_User_Company));
            WSCreateNAVProject.WSJob_PortClient WS_Client = new WSCreateNAVProject.WSJob_PortClient(navWSBinding, WS_URL);
            WS_Client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            WS_Client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(WSConfigurations.WS_User_Login, WSConfigurations.WS_User_Password, WSConfigurations.WS_User_Domain);

            try
            {
                WSCreateNAVProject.Read_Result result = await WS_Client.ReadAsync(ProjectNo);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
