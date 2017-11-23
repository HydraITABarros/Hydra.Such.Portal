﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Logic.Project;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.ViewModel.ProjectView;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.ProjectDiary;
using Hydra.Such.Data.ViewModel.ProjectDiary;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data.Logic.FolhaDeHora;
using Hydra.Such.Data.Logic.Viatura;
using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Logic.Nutrition;

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
        public JsonResult GetServiceGroup([FromBody]string invoiceClientNo, bool allProjs)
        {
            List<ClientServicesViewModel> result = DBClientServices.GetAllServiceGroup(invoiceClientNo, allProjs);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetClientService([FromBody]string invoiceClientNo, bool allProjs)
        {
            List<ClientServicesViewModel> result = DBClientServices.GetAllClientService(invoiceClientNo, allProjs);
            return Json(result);
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
        public JsonResult GetUtilizadores()
        {
            List<DDMessageString> result = DBUserConfigurations.GetAll().Select(x => new DDMessageString()
            {
                id = x.IdUtilizador,
                value = x.Nome
            }).ToList(); ;
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

        [HttpPost]
        public JsonResult GetFolhaDeHoraStatus()
        {
            List<EnumData> result = EnumerablesFixed.FolhaDeHoraStatus;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetFolhaDeHoraTypeDeslocation()
        {
            List<EnumData> result = EnumerablesFixed.FolhaDeHoraTypeDeslocation;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetFolhaDeHoraCodeTypeKms()
        {
            List<EnumDataString> result = EnumerablesFixed.FolhaDeHoraCodeTypeKms;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetFolhaDeHoraDisplacementOutsideCity()
        {
            List<EnumData> result = EnumerablesFixed.FolhaDeHoraDisplacementOutsideCity;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetRequestOrigin()
        {
            List<EnumData> result = EnumerablesFixed.RequestOrigin;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetFolhaDeHoraViaturasMatriculas()
        {
            List<DDMessageString> result = DBViatura.GetAllToList().Select(x => new DDMessageString()
            {
                id = x.Matrícula,
                value = x.Matrícula
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetFolhaDeHoraPercursoOrigemDestino()
        {
            List<DDMessageString> result = DBOrigemDestinoFh.GetAll().Select(x => new DDMessageString()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetFolhaDeHoraAjudaTipoCusto()
        {
            List<EnumData> result = EnumerablesFixed.FolhaDeHoraAjudaTipoCusto;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetFeeUnits()
        {
            List<EnumData> result = EnumerablesFixed.FeeUnits;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetPaymentTerms()
        {
            List<DDMessageString> result = DBNAV2017PaymentTerms.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Description
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContacts()
        {
            List<DDMessageString> result = DBNAV2017Contacts.GetContacts(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageString()
            {
                id = x.No_,
                value = x.Name
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractStatus()
        {
            List<EnumData> result = EnumerablesFixed.ContractStatus;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProposalsStatus()
        {
            List<EnumData> result = EnumerablesFixed.ProposalsStatus;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractChangeStatus()
        {
            List<EnumData> result = EnumerablesFixed.ContractChangeStatus;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractBillingTypes()
        {
            List<EnumData> result = EnumerablesFixed.ContractBillingTypes;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractMaintenanceTypes()
        {
            List<EnumData> result = EnumerablesFixed.ContractMaintenanceTypes;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractInvoicePeriods()
        {
            List<EnumData> result = EnumerablesFixed.ContractInvoicePeriods;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractInvoiceGroups()
        {
            List<EnumData> result = EnumerablesFixed.ContractInvoiceGroups;

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractLineTypes()
        {
            List<EnumData> result = EnumerablesFixed.ContractLineTypes;

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractLineCodes([FromBody] int ContractLineType)
        {
            List<DDMessageRelated> result = new List<DDMessageRelated>();
            switch (ContractLineType)
            {
                case 1:
                    result = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageRelated() { id = x.Code, value = x.Name, extra = x.MeasureUnit }).ToList();
                    break;
                case 2:
                    result = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "").Select(x => new DDMessageRelated() { id = x.Code, value = x.Name, extra = x.MeasureUnit }).ToList();
                    break;
                case 3:
                    result = DBNAV2017CGAccounts.GetAllCGAccounts(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageRelated() { id = x.Code, value = x.Name }).ToList();
                    break;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractDiaryLineCodes([FromBody] int ContractLineType)
        {
            List<DDMessageRelated> result = new List<DDMessageRelated>();
            switch (ContractLineType)
            {
                case 2:
                    result = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageRelated() { id = x.Code, value = x.Name, extra = x.MeasureUnit }).ToList();
                    break;
                case 1:
                    result = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "").Select(x => new DDMessageRelated() { id = x.Code, value = x.Name, extra = x.MeasureUnit }).ToList();
                    break;
                case 3:
                    result = DBNAV2017CGAccounts.GetAllCGAccounts(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageRelated() { id = x.Code, value = x.Name }).ToList();
                    break;
            }
            return Json(result);
        }


        [HttpPost]
        public JsonResult GetClientServices([FromBody] string ClientNo)
        {
            List<DDMessage> result = DBClientServices.GetAllFromClientWithDescription(ClientNo).Select(x => new DDMessage()
            {
                id = x.ServiceCode,
                value = x.ServiceDescription
            }).ToList();

            return Json(result);
        }


        //STORE PROCEDURES
        [HttpPost]
        public JsonResult GetProjectDiaryMovements()
        {
            List<EnumData> result = EnumerablesFixed.ProjectDiaryMovements;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectDiaryTypes()
        {
            List<EnumData> result = EnumerablesFixed.ProjectDiaryTypes;
            return Json(result);
        }
        //#endregion

        #region Store Procedures
        [HttpPost]
        public JsonResult GetGroupContProduct()
        {
            List<DDMessageString> result = DBNAV2017GruposContabProduto.GetGruposContabProduto(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Description
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetGroupContProject()
        {
            List<DDMessageString> result = DBNAV2017CountabGroupProjects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
            {
                id = x,
                value = x
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetRegionCode()
        {
            List<DDMessageString> result = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Name
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetFunctionalAreaCode()
        {

            List<DDMessageString> result = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Name
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetResponsabilityCenterCode()
        {
            List<DDMessageString> result = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Name
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetMeasureUnits()
        {
            List<DDMessageString> result = DBNAV2017MeasureUnit.GetAllMeasureUnit(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Description
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetLocations()
        {
            List<DDMessageString> result = DBNAV2017Locations.GetAllLocations(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Name
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContabGroup()
        {
            List<DDMessageString> result = DBNAV2017ProjectContabGroup.GetAllProjectContabGroup(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Code
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetEmployees()
        {
            List<DDMessageString> result = DBNAV2009Employees.GetAll("", _config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
            {
                id = x.No,
                value = x.Name
            }).ToList();
            return Json(result);
        }

        public JsonResult GetEmployees_FH()
        {
            List<DDMessageRelated> result = DBNAV2009Employees.GetAll("", _config.NAVCompanyName, _config.NAVCompanyName).Select(x => new DDMessageRelated()
            {
                id = x.No,
                value = x.No + " - " + x.Name,
                extra = x.Name,
            }).ToList();
            return Json(result);
        }
        #endregion

        #region TypeOptions
        [HttpPost]
        public JsonResult Alpha()
        {
            List<List<DDMessageString>> result = new List<List<DDMessageString>>();

            List<DDMessageString> resources = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "").Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Name
            }).ToList();
            List<DDMessageString> products = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Name
            }).ToList();
            List<DDMessageString> accounts = DBNAV2017CGAccounts.GetAllCGAccounts(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Name
            }).ToList();

            result.Add(resources);
            result.Add(products);
            result.Add(accounts);

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetCGAccountCode()
        {
            List<DDMessageRelated> result = DBNAV2017CGAccounts.GetAllCGAccounts(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageRelated()
            {
                id = x.Code,
                value = x.Name,
                extra = ""
            }).ToList(); return Json(result);
        }

        [HttpPost]
        public JsonResult GetProductsCode()
        {
            List<DDMessageRelated> result = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageRelated()
            {
                id = x.Code,
                value = x.Name,
                extra = x.MeasureUnit
            }).ToList(); return Json(result);
        }

        [HttpPost]
        public JsonResult GetResourcesCode()
        {
            List<DDMessageRelated> result = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "").Select(x => new DDMessageRelated()
            {
                id = x.Code,
                value = x.Name,
                extra = x.MeasureUnit
            }).ToList();
            return Json(result);
        }
        #endregion


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
        public JsonResult GetNAVContabGroupTypes()
        {

            List<NAVDimValueViewModel> result = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name);
            return Json(result);
        }


        [HttpPost]
        public JsonResult GetNAVShippingAddresses()
        {

            List<DDMessageString> result = DBNAV2017ShippingAddresses.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName).Select(X => new DDMessageString()
            {
                id = X.Code,
                value = X.Name
            }).ToList();
            return Json(result);
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
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectListDiary()
        {
            List<DDMessageString> result = DBProjects.GetAll().Where(x => x.Estado != 5 && x.Estado != 4).Select(x => new DDMessageString()
            {
                id = x.NºProjeto,
                value = x.Descrição
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
            List<DDMessageString> result = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageString()
            {
                id = x.No_,
                value = x.Name
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetServices()
        {
            List<DDMessage> result = DBServices.GetAll().Select(x => new DDMessage()
            {
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
                id = x.NºDeContrato,
                value = x.NºDeContrato
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectContractsByArea(int areaId)
        {
            List<Contratos> lcontracts = DBContracts.GetAll().Where(x => x.TipoContrato == 3 && x.Área == areaId).ToList();
            lcontracts.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);

            List<DDMessageString> result = lcontracts.Select(x => new DDMessageString()
            {
                id = x.NºDeContrato,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectContracts()
        {
            List<Contratos> lcontracts = DBContracts.GetAll().Where(x => x.TipoContrato == 3).ToList();
            lcontracts.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);

            List<DDMessageString> result = lcontracts.Select(x => new DDMessageString()
            {
                id = x.NºDeContrato,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectContractsByClient([FromBody]string clientNo)
        {
            List<Contratos> lcontracts = DBContracts.GetAll().Where(x => x.TipoContrato == 3).ToList();
            lcontracts.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
            lcontracts.RemoveAll(x => x.NºCliente != clientNo);
            List<DDMessageString> result = lcontracts.Select(x => new DDMessageString()
            {
                id = x.NºDeContrato,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        // zpgm.<populate dropdowns to use in Procedimentos CCP 
        [HttpPost]
        public JsonResult GetPocedimentosCcpProcedimentoType()
        {
            List<EnumData> ProcedimentoTypes = EnumerablesFixed.ProcedimentosCcpProcedimentoType;

            return Json(ProcedimentoTypes);
        }

        [HttpPost]
        public JsonResult GetProcedimentosCcpType()
        {
            List<EnumData> CCPTypes = EnumerablesFixed.ProcedimentosCcpType;
            return Json(CCPTypes);
        }

        [HttpPost]
        public JsonResult GetProcedimentosCcpStates()
        {
            List<EnumData> CCPStates = EnumerablesFixed.ProcedimentosCcpStates;

            return Json(CCPStates);
        }

        [HttpPost]
        public JsonResult GetBoolValues()
        {
            List<EnumBoolValues> BoolValues = EnumerablesFixed.BoolValues;

            return Json(BoolValues);
        }
        // zpgm.>

        [HttpPost]
        public JsonResult GetDimensions()
        {
            List<EnumData> dimensions = EnumerablesFixed.Dimension;

            return Json(dimensions);
        }

        [HttpPost]
        public JsonResult GetDimensionValuesFromLines([FromBody] int dimensionId)
        {
            List<DDMessageString> result = DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, dimensionId)
                .Select(x => new DDMessageString()
                {
                    id = x.Code,
                    value = x.Name
                }).ToList();
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetDimensionValues(int dimensionId)
        {
            List<DDMessageString> result = DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, dimensionId)
                .Select(x => new DDMessageString()
                {
                    id = x.Code,
                    value = x.Name
                }).ToList();

            return Json(result);
        }


        [HttpPost]
        public JsonResult GetNutritionCoffeShopTypes()
        {
            List<EnumData> BoolValues = EnumerablesFixed.NutritionCoffeShopTypes;

            return Json(BoolValues);
        }

        [HttpPost]
        public JsonResult GetViaturasTipos()
        {
            List<DDMessageString> result = DBTiposViaturas.GetAll().Select(x => new DDMessageString()
            {
                id = x.CódigoTipo.ToString(),
                value = x.Descrição
            }).ToList();
            return Json(result);
        }


        [HttpPost]
        public JsonResult GetViaturasMarcas()
        {
            List<DDMessageString> result = DBMarcas.GetAll().Select(x => new DDMessageString()
            {
                id = x.CódigoMarca.ToString(),
                value = x.Descrição
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetViaturasModelos()
        {
            List<DDMessageString> result = DBModelos.GetAll().Select(x => new DDMessageString()
            {
                id = x.CódigoModelo.ToString(),
                value = x.Descrição
            }).ToList();
            return Json(result);
        }


        [HttpPost]
        public JsonResult GetViaturasModelosByMarca([FromBody]DDMessage marca)
        {

            List<DDMessageString> result = DBModelos.GetAllByMarca(marca.id).Select(x => new DDMessageString()
            {
                id = x.CódigoModelo.ToString(),
                value = x.Descrição
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetViaturasTiposPropriedade()
        {
            List<EnumData> result = EnumerablesFixed.ViaturasTipoPropriedade;
            return Json(result);
        }


        [HttpPost]
        public JsonResult GetViaturasEstado()
        {
            List<EnumData> result = EnumerablesFixed.ViaturasEstado;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetViaturasCombustivel()
        {
            List<EnumData> result = EnumerablesFixed.ViaturasTipoCombustivel;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetTipoCartoesEApolices()
        {
            List<EnumData> result = EnumerablesFixed.TipoCartoesEApolices;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetViaturasCartaoCombustivel()
        {
            List<DDMessageString> result = DBCartoesEApolices.GetAllByType(2).Select(x => new DDMessageString()
            {
                id = x.Número,
                value = x.Descrição
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetViaturasApolices()
        {
            List<DDMessageString> result = DBCartoesEApolices.GetAllByType(1).Select(x => new DDMessageString()
            {
                id = x.Número,
                value = x.Descrição
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetAjudaCustoTipoCusto()
        {
            List<EnumData> result = EnumerablesFixed.AjudaCustoTipoCusto;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetAjudaCustoRefCusto()
        {
            List<EnumData> result = EnumerablesFixed.AjudaCustoRefCusto;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetTiposMovimento()
        {
            List<EnumData> result = EnumerablesFixed.TipoMovimento;
            return Json(result);
        }
    
        [HttpPost]
        public JsonResult GetAjudaCustoPartidaChegada()
        {
            List<EnumData> result = EnumerablesFixed.AjudaCustoPartidaChegada;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetCodTipoCustoByTipoCusto([FromBody]DDMessage tipoCusto)
        {

            List<DDMessageString> result = DBTabelaConfRecursosFh.GetAll().Where(x => x.Tipo == tipoCusto.id.ToString()).Select(x => new DDMessageString()
            {
                id = x.CodRecurso,
                value = x.CodRecurso + " - " + x.Descricao
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProductivityUnits()
        {
            List<DDMessage> result = DBProductivityUnits.GetAll().Select(x => new DDMessage()
            {
                id = x.NºUnidadeProdutiva,
                value = x.Descrição
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

    public class DDMessageRelated
    {
        public string id { get; set; }
        public string value { get; set; }
        public string extra { get; set; }
    }
}