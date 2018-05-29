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
        public JsonResult GetList([FromBody] JObject requestParams)
        {
            //int AreaId = int.Parse(requestParams["areaid"].ToString());

            List<ClientDetailsViewModel> result = new List<ClientDetailsViewModel>();// TODO abastos: get client list;

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
        public JsonResult Get([FromBody] ClientDetailsViewModel data)
        {

            if (data != null)
            {
                ClientDetailsViewModel client = new ClientDetailsViewModel(); // WSClient.GetByNoAsync(data.No);

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
            try
            {
                if (data != null)
                {
                    //TODO abastos: Get Client Numeration
                    data.No = "todo";

                    if (data.No != null)
                    {

                        //TODO abastos: Create Client
                        var client = "todo";

                        if (client == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro ao criar o cliente no NAV.";
                        }
                        else
                        {
                            data.eReasonCode = 1;
                        }
                    }
                    else
                    {
                        data.eReasonCode = 5;
                        data.eMessage = "A numeração configurada não é compativel com a inserida.";
                    }
                    if (data.eReasonCode != 1)
                    {
                        data.No = "";
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao criar o cliente";
            }
            return Json(data);

        }

        [HttpPost]
        public JsonResult Update([FromBody] ClientDetailsViewModel data)
        {

            if (data != null)
            {
                try
                {
                    if (data != null)
                    {
                        //TODO abastos: Update Client
                        var client = "todo";

                        if (client == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro ao atualizar o cliente no NAV.";
                        }
                        else
                        {
                            data.eReasonCode = 1;
                        }

                    }
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 4;
                    data.eMessage = "Ocorreu um erro ao atualizar o cliente";
                }
                return Json(data);                
            }
            return Json(false);
        }



        [HttpPost]
        public JsonResult Delete([FromBody] ClientDetailsViewModel data)
        {
            // TODO abastos:            
            return Json(false);
        }
        #endregion
        
    }
}