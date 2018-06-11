using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data.Logic.Project;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.NAV;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data;
using static Hydra.Such.Data.Enumerations;
using System.Net;
using Hydra.Such.Data.ViewModel.Clients;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public ClientesController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        #region List
        [HttpPost]
        public JsonResult Get([FromBody] JObject requestParams)
        {
            //int AreaId = int.Parse(requestParams["areaid"].ToString());
            List<ClientDetailsViewModel> result = new List<ClientDetailsViewModel>();// TODO abastos: get client list;
            result = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(c => new ClientDetailsViewModel()
            {
                No = c.No_,
                Name = c.Name,
                Address = c.Address,
                Post_Code = c.PostCode,
                City = c.City,
                Regiao_Cliente = c.Country_RegionCode

            }).ToList();
            /*
            //Apply User Dimensions Validations
            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.ResponsabilityCenterCode));
            */
            return Json(result);
        }

        #endregion

        #region Details

        [HttpPost]
        public JsonResult GetDetails([FromBody] ClientDetailsViewModel data)
        {
            if (data != null && data.No != null)
            {
                var getClientTask = WSCustomerService.GetByNoAsync(data.No, _configws);
                try
                {
                    getClientTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro a obter o cliente no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                ClientDetailsViewModel client = getClientTask.Result;
                if (client != null)
                {
                    return Json(client);
                }

            }
            return Json(false);
        }


        //eReason = 1 -> Sucess
        //eReason = 2 -> Error creating Project on Databse 
        //eReason = 3 -> Error creating Project on NAV 
        //eReason = 4 -> Unknow Error 
        //eReason = 5 -> Error getting Numeration 

        [HttpPost]
        public JsonResult Create([FromBody] ClientDetailsViewModel data)
        {
            
            if (data != null)
            {
                var createClientTask = WSCustomerService.CreateAsync(data, _configws);
                try
                {
                    createClientTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao criar o cliente no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                var result = createClientTask.Result;
                if (result == null)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao criar o cliente no NAV.";
                    return Json(data);
                }

                data.eReasonCode = 1;

                var client = WSCustomerService.MapCustomerNAVToCustomerModel(result.WSCustomer);
                if (client != null)
                {
                    client.eReasonCode = 1;
                    return Json(client);
                }

            }
            return Json(data);
        }
        
        [HttpPost]
        public JsonResult Update([FromBody] ClientDetailsViewModel data)
        {
            if (data != null)
            {
                var updateClientTask = WSCustomerService.UpdateAsync(data, _configws);
                try
                {
                    updateClientTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao atualizar o cliente no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                var result = updateClientTask.Result;
                if (result == null)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao atualizar o cliente no NAV.";
                    return Json(data);
                }

                var client = WSCustomerService.MapCustomerNAVToCustomerModel(result.WSCustomer);
                if (client != null)
                {
                    client.eReasonCode = 1;
                    return Json(client);
                }

            }
            return Json(false);
        }


        [HttpPost]
        public JsonResult Delete([FromBody] ClientDetailsViewModel data)
        {
            if (data != null && data.No != null)
            {
                var deleteClientTask = WSCustomerService.DeleteAsync(data.No, _configws);
                try
                {
                    deleteClientTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro a apagar o cliente no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                var result = deleteClientTask.Result;
                if (result != null)
                {
                    return Json(new ErrorHandler()
                    {
                        eReasonCode = 0,
                        eMessage = "Projeto removido com sucesso."
                    });
                }

            }
            return Json(false);
        }
        #endregion

    }
}