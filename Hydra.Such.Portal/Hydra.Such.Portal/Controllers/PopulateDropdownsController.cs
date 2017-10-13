using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.ViewModel;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.ViewModel.ProjectView;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.ProjectDiary;
using Hydra.Such.Data.ViewModel.ProjectDiary;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic.Contracts;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class PopulateDropdownsController : Controller
    {
        private readonly NAVConfigurations _config;

        public PopulateDropdownsController(IOptions<NAVConfigurations> appSettings)
        {
            _config = appSettings.Value;
        }

        [HttpPost]
        public JsonResult GetNumerations()
        {
            List<DDMessage> result = DBNumerationConfigurations.GetAll().Select(x => new DDMessage()
            {
                id = x.Id,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetAreas()
        {
            List<EnumData> result = EnumerablesFixed.Areas;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetFeatures()
        {
            List<EnumData> result = EnumerablesFixed.Features;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectStatus()
        {
            List<EnumData> result = EnumerablesFixed.ProjectStatus;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectCategories()
        {
            List<EnumData> result = EnumerablesFixed.ProjectCategories;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectTypes()
        {
            List<DDMessage> result = DBProjectTypes.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList(); ;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetUserProfileModels()
        {
            List<DDMessage> result = DBProfileModels.GetAll().Select(x => new DDMessage()
            {
                id = x.Id,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        //STORE PROCEDURES
        [HttpPost]
        public JsonResult GetGroupContProduct()
        {
            List<NAVGroupContProductViewModel> result = DBNAV2017GruposContabProduto.GetGruposContabProduto(_config.NAVDatabaseName, _config.NAVCompanyName);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetRegionCode()
        {
            List<DDMessageString> result = DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 1).Select(x => new DDMessageString() {
                id = x.Code,
                value = x.Name
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetFunctionalAreaCode()
        {

            List<DDMessageString> result = DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 2).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Name
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetResponsabilityCenterCode()
        {
            List<DDMessageString> result = DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 3).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Name
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetCGAccountCode (string accountNo)
        {
            List<NAVCGAccountViewModel> result = DBNAV2017CGAccounts.GetAllCGAccounts(_config.NAVDatabaseName, _config.NAVCompanyName, accountNo);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetMeasureUnits()
        {
            List<NAVMeasureUnitViewModel> result = DBNAV2017MeasureUnit.GetAllMeasureUnit(_config.NAVDatabaseName, _config.NAVCompanyName);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetLocations()
        {
            List<NAVLocationsViewModel> result = DBNAV2017Locations.GetAllLocations(_config.NAVDatabaseName, _config.NAVCompanyName);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContabGroup()
        {
            List<NAVContabGroupViewModel> result = DBNAV2017ProjectContabGroup.GetAllProjectContabGroup(_config.NAVDatabaseName, _config.NAVCompanyName);
            return Json(result);
        }
        //STORE PROCEDURES
        [HttpPost]
        public JsonResult GetNAVContabGroupTypes()
        {

            List<NAVDimValueViewModel> result = DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 2);
            return Json(result);
        }


        [HttpPost]
        public JsonResult GetMoveType()
        {
            List<DDMessage> ResponsabilityCenter = new List<DDMessage>(){
                new DDMessage()
                {
                    id = 1,
                    value = "Consumo"
                },

                new DDMessage()
                {
                    id = 2,
                    value = "Venda"
                },
            };

            return Json(ResponsabilityCenter);
        }

        [HttpPost]
        public JsonResult GetProjectType()
        {
            List<DDMessage> ResponsabilityCenter = DBProjectTypes.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();

            return Json(ResponsabilityCenter);
        }

        [HttpPost]
        public JsonResult GetServiceObjects()
        {
            List<DDMessage> ResponsabilityCenter = DBServiceObjects.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();
            
            return Json(ResponsabilityCenter);
        }

        [HttpPost]
        public JsonResult GetServiceObjectsByAreaId(string AreaCode)
        {
            List<DDMessage> ResponsabilityCenter = DBServiceObjects.GetAll().Where(x => x.CódÁrea == AreaCode).Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();

            return Json(ResponsabilityCenter);
        }


        [HttpPost]
        public JsonResult GetProjectList()
        {
            List<DDMessageString> result = DBProjects.GetAll().Select(x => new DDMessageString()
            {
                id = x.NºProjeto,
                value = x.NºProjeto
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContabGroupTypesOM_Type()
        {
            List<EnumData> result = EnumerablesFixed.ContabGroupTypesOM_Type;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContabGroupTypesOM_FailType()
        {
            List<EnumData> result = EnumerablesFixed.ContabGroupTypesOM_FailType;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetCountabGroupTypes()
        {
            List<DDMessage> result = DBCountabGroupTypes.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetCountabGroupTypesOM()
        {
            List<DDMessage> result = DBCountabGroupTypesOM.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }



        [HttpPost]
        public JsonResult GetAllClients()
        {
            List<NAVClientsViewModel> result = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetServices()
        {
            List<DDMessage> result = DBServices.GetAll().Select(x => new DDMessage() {
                id = x.Código,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractsByArea(int areaId)
        {
            List<DDMessageString> result = DBContracts.GetAll().Where(x => x.Área == areaId).Select(x => new DDMessageString()
            {
                id = x.NºContrato,
                value = x.NºContrato
            }).ToList();

            return Json(result);
        }
    }
    
    public class DDMessage
    {
        public int id { get; set; }
        public string value { get; set; }
    }

    public class DDMessageString
    {
        public string id { get; set; }
        public string value { get; set; }
    }
}