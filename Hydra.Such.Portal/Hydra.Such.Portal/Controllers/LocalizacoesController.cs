using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Portal.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Controllers
{
    //TODO: Confirmar se é para renomear para Existências
    public class LocalizacoesController : Controller
    {
        private readonly NAVConfigurations _config;

        public LocalizacoesController(IOptions<NAVConfigurations> appSettings)
        {
            _config = appSettings.Value;
        }

        public IActionResult Index()
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Existencias);

            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UserPermissions = userPermissions;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        [HttpPost]
        public JsonResult GetLocations()
        {
            List<LocationViewModel> result = DBLocations.GetAll().ParseToViewModel();
            result.ForEach(x =>
            {
                x.RegionText = !string.IsNullOrEmpty(x.Region) ? DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, Enumerations.Dimensions.Region, User.Identity.Name, x.Region).FirstOrDefault().Name : "";
                x.LockedText = x.Locked.HasValue ? x.Locked == true ? "Sim" : "Não" : "";
            });

            return Json(result);
        }

        public IActionResult Detalhes(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Existencias);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.LocationCode = id;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        [HttpPost]
        public JsonResult GetLocationData([FromBody] LocationViewModel item)
        {
            LocationViewModel result = new LocationViewModel();
            if (item != null && !string.IsNullOrEmpty(item.Code))
                result = DBLocations.ParseToViewModel(DBLocations.GetById(item.Code));
            return Json(result);
        }

        // 100 - Sucesso
        // 101 - Campo Código é obrigatório
        // 102 - Ocorreu um erro desconhecido
        [HttpPost]
        public JsonResult CreateLocation([FromBody] LocationViewModel item)
        {
            LocationViewModel result = new LocationViewModel();
            if (item != null)
            {
                if ( !string.IsNullOrEmpty(item.Code))
                {
                    result.CreateUser = User.Identity.Name;
                    result = DBLocations.ParseToViewModel(DBLocations.Create(DBLocations.ParseToDatabase(item)));

                    if (result != null)
                    {
                        result.eReasonCode = 100;
                        result.eMessage = "Localização criada com sucesso.";
                    }
                    else
                    {
                        result.eReasonCode = 102;
                        result.eMessage = "Ocorreu um erro ao criar a localização.";
                    }
                }
                else
                {
                    result.eReasonCode = 101;
                    result.eMessage = "É obrigatório preencher o campo Código.";
                }
            }
            return Json(result);
        }

        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido
        [HttpPost]
        public JsonResult UpdateLocation([FromBody] LocationViewModel item)
        {
            LocationViewModel result = new LocationViewModel();
            if (item != null)
            {
                Localizações CLocation = DBLocations.GetById(item.Code);
                CLocation.Nome = item.Name;
                CLocation.Endereço = item.Address;
                CLocation.Cidade = item.City;
                CLocation.Telefone = item.MobilePhone;
                CLocation.NºFax = item.Fax;
                CLocation.Contato = item.Contact;
                CLocation.CódPostal = item.ZipCode;
                CLocation.Email = item.Email;
                CLocation.Bloqueado = item.Locked;
                CLocation.Região = item.Region;
                CLocation.Área = item.Area;
                CLocation.CentroResponsabilidade = item.ResponsabilityCenter;
                CLocation.LocalFornecedor = item.SupplierLocation;
                CLocation.CódigoLocalEntrega = item.ShipLocationCode;
                CLocation.ResponsávelArmazém = item.WarehouseManager;
                CLocation.ArmazémAmbiente = item.WarehouseEnvironment;
                CLocation.DataHoraModificação = DateTime.Now;
                CLocation.UtilizadorModificação = User.Identity.Name;
                
                result = DBLocations.ParseToViewModel(DBLocations.Update(CLocation));

                if (result != null)
                {
                    result.eReasonCode = 100;
                    result.eMessage = "Localização atualizada com sucesso.";
                }
                else
                {
                    result.eReasonCode = 101;
                    result.eMessage = "Ocorreu um erro ao atualizar a localização.";
                }
            }
            return Json(result);
        }

        // 100 - Sucesso
        // 101 - Ocorreu um erro desconhecido
        [HttpPost]
        public JsonResult DeleteLocation([FromBody] LocationViewModel item)
        {
            LocationViewModel result = new LocationViewModel();
            if (item != null)
            {
                Localizações CLocation = DBLocations.GetById(item.Code);

                if (DBLocations.Delete(CLocation))
                {
                    result.eReasonCode = 100;
                    result.eMessage = "Localização removida com sucesso.";
                }
                else
                {
                    result.eReasonCode = 101;
                    result.eMessage = "Ocorreu um erro ao remover a localização.";
                }
            }
            return Json(result);
        }
    }
}