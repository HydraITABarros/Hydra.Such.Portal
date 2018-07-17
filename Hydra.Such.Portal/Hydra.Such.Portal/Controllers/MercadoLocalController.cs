using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Compras;
using Hydra.Such.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class MercadoLocalController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public MercadoLocalController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        public IActionResult MercadoLocalList()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.MercadoLocal);

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

        [HttpPost]
        //Listagem das Folhas de Horas consoante o estado
        public JsonResult GetListComprasByEstado([FromBody] ComprasModel ML)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.MercadoLocal);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;

                List<ComprasViewModel> result = new List<ComprasViewModel>();
                if (ML.Estado == 0)
                    result = DBCompras.GetAll();
                else
                    result = DBCompras.GetAllByEstado((int)ML.Estado);

                if (result != null)
                {
                    result.ForEach(Compras =>
                    {
                        Compras.EstadoTexto = Compras.Estado == null ? "" : EnumerablesFixed.ComprasEstado.Where(y => y.Id == Compras.Estado).FirstOrDefault().Value;
                        Compras.NoFornecedorTexto = Compras.NoFornecedor == null ? "" : DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, Compras.NoFornecedor).FirstOrDefault().Name;
                    });
                }

                    return Json(result.OrderByDescending(x => x.ID));
                }

            return Json(null);
        }

        public JsonResult AprovadoToTratado([FromBody] List<ComprasModel> Mercados)
        {
            if (Mercados != null)
            {
                UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.MercadoLocal);
                if (UPerm.Update == true)
                {
                    Mercados.ForEach(Mercado =>
                    {
                        Mercado.Estado = 2;
                        Mercado.DataValidacao = DateTime.Now;
                        Mercado.UtilizadorValidacao = User.Identity.Name;

                        DBCompras.Update(Mercado);
                    });
                }
            }

            return Json(null);
        }

        public JsonResult AprovadoToValidar([FromBody] List<ComprasModel> data)
        {
            return Json(null);
        }

        public JsonResult AprovadoToRecusar([FromBody] List<ComprasModel> data)
        {
            return Json(null);
        }

        public JsonResult ValidadoToTratado([FromBody] List<ComprasModel> data)
        {
            return Json(null);
        }

        public JsonResult RecusadoToTratado([FromBody] List<ComprasModel> data)
        {
            return Json(null);
        }

    }
}