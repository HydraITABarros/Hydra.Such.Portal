using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;
using Microsoft.AspNetCore.Mvc;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Areas.Compras.Controllers
{
    public class ModelosRequisicaoController : Controller
    {
        //[Area("Compras")]
        //public IActionResult Index()
        //{
        //    UserAccessesViewModel userPermissions =
        //        DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.ModelosRequisicao);
        //    if (userPermissions != null && userPermissions.Read.Value)
        //    {
        //        ViewBag.UPermissions = userPermissions;
        //        return View();
        //    }
        //    else
        //    {
        //        return Redirect(Url.Content("~/Error/AccessDenied"));
        //    }
        //}
        
        //[Area("Compras")]
        //public IActionResult Detalhes()
        //{
        //    UserAccessesViewModel userPermissions =
        //        DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Compras, Enumerations.Features.ModelosRequisicao);
        //    if (userPermissions != null && userPermissions.Read.Value)
        //    {
        //        ViewBag.UPermissions = userPermissions;
        //        return View();
        //    }
        //    else
        //    {
        //        return Redirect(Url.Content("~/Error/AccessDenied"));
        //    }
        //}

        //[HttpPost]
        //[Area("Compras")]
        //public JsonResult GetRequisitionTemplates()
        //{
        //    List<RequisitionViewModel> result = DBRequestModels.GetAll().ParseToViewModel();

        //    //Apply User Dimensions Validations
        //    List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
        //    //Regions
        //    if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
        //        result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
        //    //FunctionalAreas
        //    if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
        //        result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
        //    //ResponsabilityCenter
        //    if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
        //        result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CenterResponsibilityCode));
        //    return Json(result);
        //}

        //[HttpPost]
        //[Area("Compras")]
        //public JsonResult GetRequisition([FromBody] string id)
        //{
        //    RequisitionViewModel item;
        //    if (!string.IsNullOrEmpty(id) && id != "0")
        //    {
        //        item = DBRequestModels.GetById(id).ParseToViewModel();
        //    }
        //    else
        //        item = new RequisitionViewModel();

        //    return Json(item);
        //}
    }
}