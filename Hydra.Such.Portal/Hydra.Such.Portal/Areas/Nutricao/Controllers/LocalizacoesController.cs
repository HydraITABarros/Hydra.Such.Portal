using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.ViewModel.Nutrition;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Such.Portal.Areas.Nutricao.Controllers
{
    public class LocalizacoesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetLocations()
        {
            List<LocationViewModel> result = DBLocations.GetAll().ParseToViewModel();
            return Json(result);
        }

        public IActionResult Details()
        {
            return View();
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


    }
}