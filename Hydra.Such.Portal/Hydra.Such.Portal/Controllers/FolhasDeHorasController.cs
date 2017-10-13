using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Database;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.NAV;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class FolhasDeHorasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public FolhasDeHorasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        //#region Home
        //public IActionResult Index()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public JsonResult GetListFolhasDeHorasByArea([FromBody] int id)
        //{
        //    List<FolhaDeHoraListItemViewModel> result = DBFolhasDeHoras.GetAllByAreaToList(id);

        //    result.ForEach(x =>
        //    {
        //        x.StatusDescription = EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
        //    });
        //    return Json(result);
        //}
        //#endregion

        //#region Details
        //public IActionResult Detalhes(String id)
        //{
        //    return View();
        //}

        //[HttpPost]
        //public JsonResult GetFolhaDeHoraDetails([FromBody] FolhaDeHoraDetailsViewModel data)
        //{

        //    if (data != null)
        //    {
        //        FolhasDeHoras cFolhaDeHora = DBFolhasDeHoras.GetById(data.FolhaDeHoraNo);

        //        if (cFolhaDeHora != null)
        //        {
        //            FolhaDeHoraDetailsViewModel result = new FolhaDeHoraDetailsViewModel()
        //            {
        //            };

        //            return Json(result);
        //        }

        //        return Json(new FolhaDeHoraDetailsViewModel());
        //    }
        //    return Json(false);
        //}

        //#endregion





















    }
}
