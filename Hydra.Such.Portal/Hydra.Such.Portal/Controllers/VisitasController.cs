using Hydra.Such.Data.Logic;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Portal.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class VisitasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GeneralConfigurations _generalConfig;

        public VisitasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment, IOptions<GeneralConfigurations> appSettingsGeneral)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
            _generalConfig = appSettingsGeneral.Value;
        }

        public IActionResult Visitas_List(string VisitaNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Visitas);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult Visitas_Details(string VisitaNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Visitas);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.VisitaNo = string.IsNullOrEmpty(VisitaNo) ? "" : VisitaNo;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
    }
}