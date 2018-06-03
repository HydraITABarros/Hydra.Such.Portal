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

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Get([FromBody] ClientDetailsViewModel data)
        {
            if (data != null && data.No != null)
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
    }
}