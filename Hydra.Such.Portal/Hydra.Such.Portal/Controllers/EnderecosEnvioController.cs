using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Clients;
using Hydra.Such.Portal.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class EnderecosEnvioController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public EnderecosEnvioController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        [HttpPost]
        public JsonResult Get([FromBody] ClientDetailsViewModel data)
        {

            if (data != null && data.No != null && data.No != "")
            {
                var getShipToAddress = WSShipToAddressService.GetByNoAsync(data.No, _configws);
                try
                {
                    getShipToAddress.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro a obter o endereço de envio no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                var listShipToAddress = getShipToAddress.Result;
                if (listShipToAddress != null)
                {
                    return Json(listShipToAddress);
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
        public JsonResult Create([FromBody] ShipToAddressViewModel data)
        {
            if (data != null)
            {
                //data.Country_Region_Code = null;
                var createTask = WSShipToAddressService.CreateAsync(data, _configws);
                try
                {
                    createTask.Wait();
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao criar a linha no NAV.";
                    data.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));
                    return Json(data);
                }

                var result = createTask.Result;
                if (result == null)
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro ao criar a linha no NAV.";
                    return Json(data);
                }

                data.eReasonCode = 1;

                var ShipToAddress = WSShipToAddressService.MapShipToAddressViewModel(result.WSShipToAddress);

                if (ShipToAddress != null)
                {
                    ShipToAddress.eReasonCode = 1;
                    return Json(ShipToAddress);
                }

            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult Update([FromBody] List<ShipToAddressViewModel> listData)
        {
            if (listData != null)
            {
                var retval = new ErrorHandler();
                var Customer_No = "";
                foreach (var data in listData)
                {
                    Customer_No = data.Customer_No;
                    var updateTask = WSShipToAddressService.UpdateAsync(data, _configws);
                    try
                    {
                        updateTask.Wait();
                    }
                    catch (Exception ex)
                    {
                        retval.eReasonCode = 3;
                        retval.eMessage = "Ocorreu um erro a actualizar a linha com o Código: \"" + data.Code.ToString() + "\"";
                        retval.eMessages.Add(new TraceInformation(TraceType.Error, ex.Message));

                        return Json(retval);
                        //break; // get out of the loop
                    }

                };


                var getShipToAddress = WSShipToAddressService.GetByNoAsync(Customer_No, _configws);
                getShipToAddress.Wait();
                var listShipToAddress = getShipToAddress.Result;
                if (listShipToAddress != null)
                {
                    return Json(listShipToAddress);
                }

            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult Delete([FromBody] ShipToAddressViewModel data)
        {
            if (data != null)
            {
                var deleteTask = WSShipToAddressService.DeleteAsync(data.Customer_No, data.Code, _configws);
                try
                {
                    deleteTask.Wait();
                }
                catch (Exception ex)
                {
                    return Json(new ErrorHandler()
                    {
                        eReasonCode = 3,
                        eMessage = "Ocorreu um erro a apagar a linha."
                    });
                }
                return Json(new ErrorHandler()
                {
                    eReasonCode = 0,
                    eMessage = "Linha removida com sucesso."
                });

            }
            return Json(false);
        }

    }
}