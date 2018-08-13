using System;
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
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.Logic.ComprasML;
using Hydra.Such.Data.Logic.Approvals;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Data.Extensions;
using Newtonsoft.Json.Linq;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Data.Logic.Telemoveis;

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
        public JsonResult GetApprovalTypes()
        {
            List<EnumData> result = EnumerablesFixed.ApprovalTypes;

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContratTypes()
        {
            List<EnumData> result = EnumerablesFixed.ContractType;

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

        public JsonResult GetProposalsFetchUnitDB()
        {
            List<DDMessage> result = DBFetcUnit.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
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
        public JsonResult GetProposalsFetchUnit()
        {
            List<EnumData> result = EnumerablesFixed.ProposalsFetchUnit;
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
        public JsonResult GetRequisitionsStatus()
        {
            List<EnumData> result = EnumerablesFixed.RequisitionsStatus;
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
            }).ToList();
            return Json(result);
        }
        public JsonResult GetUtilizadoresByArea()
        {
            UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            List<DDMessageString> result = DBUserConfigurations.GetAll().Where(x => x.Rfperfil == (int)userConfig.RFPerfil).Select(x => new DDMessageString()
            {
                id = x.IdUtilizador,
                value = x.Nome
            }).ToList();
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetUtilizadores_NumMec()
        {
            List<DDMessageString> result = DBUserConfigurations.GetAll().Select(x => new DDMessageString()
            {
                id = x.EmployeeNo,
                value = x.Nome
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectTypes()
        {
            List<DDMessage> result = DBProjectTypes.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();
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
            List<DDMessageString> result = DBTabelaConfRecursosFh.GetAll().Where(x => x.Tipo == "1").Select(x => new DDMessageString()
            {
                id = x.CodRecurso,
                value = x.Descricao
            }).ToList();

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
        public JsonResult GetGroupApproval()
        {
            List<DDMessage> result = DBApprovalGroups.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
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
        public JsonResult NecessityDirectShoppingLines()
        {
            List<NAVOpenOrderLinesViewModels> result = new List<NAVOpenOrderLinesViewModels>();
            try
            {
                result = DBNAV2017NecessityDirectShopping.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName).ToList();
                return Json(result);
            }
            catch (Exception e)
            {
                return Json(result);
            }
        }
        [HttpPost]
        public JsonResult getNecessityDirectShoppingLine([FromBody] string numb, string documentNO, int LineNo)
        {
            NAVOpenOrderLinesViewModels getorderline = new NAVOpenOrderLinesViewModels();
            try
            {
                List<NAVOpenOrderLinesViewModels> result = new List<NAVOpenOrderLinesViewModels>();
                result = DBNAV2017NecessityDirectShopping.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName).ToList();

                if (result != null && result.Count > 0 && !string.IsNullOrEmpty(documentNO) && !string.IsNullOrEmpty(numb) && LineNo > 0)
                {
                    foreach (NAVOpenOrderLinesViewModels item in result)
                    {
                        if (documentNO == item.DocumentNO && numb == item.Numb && LineNo == item.Line_No)
                        {
                            getorderline = item;
                        }
                    }
                }
                return Json(getorderline);
            }
            catch (Exception e)
            {
                return Json(getorderline);
            }

        }

        [HttpPost]
        public JsonResult OpenOrderLines([FromBody] DateTime date)
        {
            string data = date.ToString();
            List<NAVOpenOrderLinesViewModels> result = DBNAV2017OpenOrderLines.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, data, "", "50").ToList();
            return Json(result);
        }

       

        [HttpPost]
        public JsonResult GetPurchaseHeader([FromBody] string respcenter)
        {
            List<DDMessageString> result = null;

            var dimValue = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, Dimensions.ResponsabilityCenter, User.Identity.Name, respcenter)
                .FirstOrDefault();

            if (dimValue != null)
            {
                result = DBNAV2017EncomendaAberto.GetByDimValue(_config.NAVDatabaseName, _config.NAVCompanyName, dimValue.DimValueID)
                    .Select(x => new DDMessageString() { id = x.Code })
                    .GroupBy(x => new { x.id })
                    .Select(x => new DDMessageString { id = x.Key.id })
                    .ToList();
            }
            else
                result = new List<DDMessageString>();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetpriceAgreementByDate([FromBody] DateTime date, int produtivityUnitId)
        {
            try
            {
                var prodUnit = DBProductivityUnits.GetById(produtivityUnitId);
                List<LinhasAcordoPrecos> result = DBLinhasAcordoPrecos.GetForDimensions(date, prodUnit.CódigoCentroResponsabilidade, prodUnit.CódigoRegião, prodUnit.CódigoÁreaFuncional);
                return Json(result);
            }
            catch (Exception e)
            {
                return null;
            }
        }
     

        [HttpPost]
        public JsonResult getOpenOrderLine([FromBody] string numb, string documentNO, int LineNo, DateTime? date)
        {
            string data = date.ToString();
            NAVOpenOrderLinesViewModels getorderline = new NAVOpenOrderLinesViewModels();
            try
            {
                List<NAVOpenOrderLinesViewModels> result = new List<NAVOpenOrderLinesViewModels>();
                result = DBNAV2017OpenOrderLines.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, data, "", "").ToList();
                if (result != null && result.Count > 0 &&
                    !string.IsNullOrEmpty(documentNO) &&
                    !string.IsNullOrEmpty(numb)
                    && LineNo > 0)
                {
                    foreach (NAVOpenOrderLinesViewModels item in result)
                    {
                        if (documentNO == item.DocumentNO && numb == item.Numb && LineNo == item.Line_No)
                        {
                            getorderline = item;
                        }
                    }
                }
                return Json(getorderline);
            }
            catch (Exception e)
            {
                return Json(getorderline);
            }

        }
        [HttpPost]
        public JsonResult getOpenOrderLineByHeader([FromBody] string codFuncArea)
        {
            string date = DateTime.Now.ToString();
            NAVOpenOrderLinesViewModels getorderline = new NAVOpenOrderLinesViewModels();
            try
            {
                List<NAVOpenOrderLinesViewModels> result = new List<NAVOpenOrderLinesViewModels>();
                result = DBNAV2017OpenOrderLines.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, date, "", codFuncArea);
         

                return Json(result);

            }
            catch (Exception e)
            {
                return Json(getorderline);
            }

        }

        

        [HttpPost]
        public JsonResult getSupplier([FromBody] string suppliercode)
        {
            List<DDMessageString> result = DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, suppliercode).Select(x => new DDMessageString()
            {
                id = x.No_,
                value = x.Name
            }).ToList();
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetUnitOfMeasureByCode()
        {
            List<DDMessageString> result = DBNAV2017UnitOfMeasure.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
            {
                id = x.code,
                value = x.description
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetUnitOfMeasureByCodeeSUCH()
        {
            List<DDMessageString> result = DBUnidadeMedida.GetAll().Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Description
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetRequestStatus()
        {
            List<DDMessageString> result = Configurations.EnumerablesFixed.RequisitionStatesEnumData.Select(x => new DDMessageString()
            {
                id = Convert.ToString(x.Id),
                value = x.Value
            }).ToList();
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
        public JsonResult GetContacts([FromBody] int type = -1)
        {
            List<DDMessageString> result = DBNAV2017Contacts.GetContacts(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageString()
            {
                id = x.No_,
                value = x.Name
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContactsByType([FromBody] int type)
        {
            List<DDMessageString> result = DBNAV2017Contacts.GetContactsByType(_config.NAVDatabaseName, _config.NAVCompanyName, "", type).Select(x => new DDMessageString()
            {
                id = x.No_,
                value = x.Name
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetCustomerContacts([FromBody] string customerNo)
        {
            List<DDMessageString> result = DBNAV2017Contacts.GetCustomerContacts(_config.NAVDatabaseName, _config.NAVCompanyName, customerNo)
                .Select(x => new DDMessageString()
                {
                    id = x.No_,
                    value = x.Name
                })
                .ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractStatus()
        {
            List<EnumData> result = EnumerablesFixed.ContractStatus;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractTerminationDeadlineNotice()
        {
            List<EnumData> result = EnumerablesFixed.ContractTerminationDeadlineNotice;
            return Json(result);
        }




        [HttpPost]
        public JsonResult GetContractPaymentTerms()
        {
            List<EnumData> result = EnumerablesFixed.ContractPaymentTerms;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetContractTerminationTerms()
        {
            List<EnumData> result = EnumerablesFixed.ContractTerminationTerms;
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
        public JsonResult GetClientServices([FromBody] string ClientNo)
        {
            List<DDMessageString> result = DBClientServices.GetAllFromClientWithDescription(ClientNo).Select(x => new DDMessageString()
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

        [HttpPost]
        public JsonResult GetrequestTypes()
        {
            List<EnumData> result = EnumerablesFixed.requestTypes;
            return Json(result);
        }
        //#endregion

        #region Store Procedures
        [HttpPost]
        public JsonResult GetGroupContProduct()
        {
            List<DDMessageString> result = DBNAV2017GruposContabilisticos.GetGruposContabProduto(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Description
            }).ToList();
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetVATBusinessPostingGroups()
        {
            List<DDMessageString> result = DBNAV2017GruposContabilisticos.GetVATBusinessPostingGroups(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Description
            }).ToList();
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetVATProductsPostingGroups()
        {
            List<DDMessageString> result = DBNAV2017GruposContabilisticos.GetVATProductsPostingGroups(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
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
        public JsonResult GetAllAction()
        {
            List<DDMessage> result = DBActionsConfection.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
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
            List<AcessosLocalizacoes> userLocations = DBAcessosLocalizacoes.GetByUserId(User.Identity.Name);
            var allLocations = DBNAV2017Locations.GetAllLocations(_config.NAVDatabaseName, _config.NAVCompanyName);

            if (userLocations == null || userLocations.Count == 0)
            {
                List<DDMessageRelated> result_all = allLocations.Select(x => new DDMessageRelated()
                {
                    id = x.Code,
                    value = x.Name,
                    extra = Convert.ToString(x.ArmazemCDireta)
                }).ToList();
                return Json(result_all);
            }
            else
            {
                var userLocationsIds = userLocations.Select(x => x.Localizacao).Distinct().ToList();
                List<DDMessageRelated> result_all = allLocations.Where(x => userLocationsIds.Contains(x.Code)).Select(x => new DDMessageRelated()
                {
                    id = x.Code,
                    value = x.Name,
                    extra = Convert.ToString(x.ArmazemCDireta)
                }).ToList();
                return Json(result_all);
            }
        }

        [HttpPost]
        public JsonResult GetAllLocations()
        {
            List<DDMessageRelated> result_all = DBNAV2017Locations.GetAllLocations(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageRelated()
            {
                id = x.Code,
                value = x.Name,
                extra = Convert.ToString(x.ArmazemCDireta)
            }).ToList();
            return Json(result_all);
        }

        [HttpPost]
        public JsonResult GetLocationsValuesFromLines([FromBody] string locationId)
        {
            List<DDMessageString> result = DBNAV2017Locations.GetAllLocations(_config.NAVDatabaseName, _config.NAVCompanyName).Where(y => y.Name == locationId).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Name
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetLocationsPortal()
        {
            List<DDMessageString> result = DBLocations.GetAll().Select(x => new DDMessageString()
            {
                id = x.Código,
                value = x.Nome
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
            List<DDMessageString> result = DBNAV2009Employees.GetAll("", _config.NAV2009DatabaseName, _config.NAV2009CompanyName).Select(x => new DDMessageString()
            {
                id = x.No,
                value = x.Name
            }).ToList();
            return Json(result);
        }

        public JsonResult GetEmployees_FH()
        {
            List<DDMessageRelated> result = DBNAV2009Employees.GetAll("", _config.NAV2009DatabaseName, _config.NAV2009CompanyName).Select(x => new DDMessageRelated()
            {
                id = x.No,
                value = x.No + " - " + x.Name,
                extra = x.Name,
            }).ToList();
            return Json(result);
        }

        /// <summary>
        /// NR 20180228
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllGestorProcesso()
        {
            List<DDMessageString> result = DBNAV2009Employees.GetAllGestorProcesso(_config.NAV2009DatabaseName, _config.NAV2009CompanyName).Select(x => new DDMessageString()
            {
                id = x.No,
                value = x.Name
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
            }).ToList();
            return Json(result);
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

        [HttpPost]
        public JsonResult GetResourcesCodeByContabGr()
        {
            List<DDMessageRelated> result = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "ALIMENTAÇÃ").Select(x => new DDMessageRelated()
            {
                id = x.Code,
                value = x.Name,
                extra = x.MeasureUnit
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetResourcesCodeFH()
        {
            List<DDMessageRelated> result = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "").Select(x => new DDMessageRelated()
            {
                id = x.Code,
                value = x.Code + " - " + x.Name,
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
                value = X.Name + " - " + X.City
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetNAVShippingAddressesByClientNo([FromBody] string ClientNo)
        {
            List<DDMessageString> result = DBNAV2017ShippingAddresses.GetByClientNo(ClientNo, _config.NAVDatabaseName, _config.NAVCompanyName).Select(X => new DDMessageString()
            {
                id = X.Code,
                value = X.Name + " - " + X.Address + " - " + X.City
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        //Retuns a list of NAVAddressesViewModel
        public JsonResult GetNAVShippingAddressesByClientNoAsVM([FromBody] string ClientNo)
        {
            var result = DBNAV2017ShippingAddresses.GetByClientNo(ClientNo, _config.NAVDatabaseName, _config.NAVCompanyName).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetServiceObjects()
        {
            List<DDMessage> sevices = DBServiceObjects.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();

            return Json(sevices);
        }

        [HttpPost]
        public JsonResult GetServiceObjectsByAreaId(string AreaCode)
        {
            List<DDMessage> services = DBServiceObjects.GetAll().Where(x => x.CódÁrea == AreaCode).Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();

            return Json(services);
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
        public JsonResult GetProjects()
        {
            var result = DBProjects.GetAll();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProdUnits()
        {
            List<DDMessageString> result = DBProductivityUnits.ParseListToViewModel(DBProductivityUnits.GetAll()).Select(x => new DDMessageString()
            {
                id = Convert.ToString(x.ProductivityUnitNo),
                value = x.Description
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectNavList()
        {
            List<NAVProjectsViewModel> result = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && (y.ValorDimensão == x.RegionCode || string.IsNullOrEmpty(x.RegionCode))));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && (y.ValorDimensão == x.AreaCode || string.IsNullOrEmpty(x.AreaCode))));
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && (y.ValorDimensão == x.CenterResponsibilityCode || string.IsNullOrEmpty(x.CenterResponsibilityCode))));
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectById([FromBody] string ProjectNo)
        {
            List<NAVProjectsViewModel> result = DBNAV2017Projects.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, ProjectNo).ToList();
            return Json(result);
        }


        [HttpPost]
        public JsonResult GetTipoTrabalhoList()
        {
            List<DDMessageString> result = DBTipoTrabalhoFH.GetAll().Select(x => new DDMessageString()
            {
                id = x.Codigo,
                value = x.Descricao
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
        public JsonResult GetProjectListDiaryDlg()
        {
            List<ProjectListItemViewModel> result = DBProjects.GetAllByAreaToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjDiary_Price()
        {
            List<EnumData> result = EnumerablesFixed.project_diaryPrice;
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
        public JsonResult GetAllClientsComboGrid()
        {
            var result = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetServices()
        {
            List<DDMessageString> result = DBServices.GetAll().Select(x => new DDMessageString()
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


        public JsonResult GetPriceAgreementLines([FromBody]string Area,DateTime DataFornecedor)
        {
            List<LinhasAcordoPrecos> LinhasAcordoPrecos = DBLinhasAcordoPrecos.GetAll().ToList();
 
            List<DDMessageString> result = LinhasAcordoPrecos.Where(x=> x.Area == Area && x.DtValidadeInicio== DataFornecedor).Select(x => new DDMessageString()
            {
                id = x.NoProcedimento,
                value = x.DescricaoProduto
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
        public JsonResult GetPocedimentosAbertoFechado()
        {
            List<EnumData> retval = EnumerablesFixed.ProcedimentosAbertoFechado;

            return Json(retval);
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

        //NR20180629 - Obter Requisições

        [HttpPost]
        public JsonResult GetRequisitions()
        {
            List<RequisitionViewModel> result = DBRequest.GetByState(RequisitionStates.Approved).ParseToViewModel();

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CenterResponsibilityCode));
            return Json(result);
        }


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
        public JsonResult GetViaturas()
        {
            List<DDMessageString> result = DBViatura.GetAllToList().Select(x => new DDMessageString()
            {
                id = x.Matrícula
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
        public JsonResult GetTiposRefeicao()
        {
            List<EnumData> result = DBMealTypes.GetAll().Select(x => new EnumData() {
                Id = x.Código,
                Value = x.Descrição
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetAjudaCustoPartidaChegada()
        {
            List<EnumData> result = EnumerablesFixed.AjudaCustoPartidaChegada;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetTipoHoraFH()
        {
            List<EnumData> result = EnumerablesFixed.GetTipoHoraFH;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetRHRecursosFH()
        {
            List<DDMessageString> result = DBRHRecursosFH.GetAll().Select(x => new DDMessageString()
            {
                id = x.FamiliaRecurso,
                value = x.NomeRecurso
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetOrigemDestinoFH()
        {
            List<DDMessageString> result = DBOrigemDestinoFh.GetAll().Select(x => new DDMessageString()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();
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
        public JsonResult GetCodTipoCustoByTipoCusto_ALL()
        {

            List<DDMessageString> result = DBTabelaConfRecursosFh.GetAll().Select(x => new DDMessageString()
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

        [HttpPost]
        public JsonResult PrecoVendaRecurso_Code()
        {
            List<DDMessageString> result = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "").Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Name
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult PrecoVendaRecurso_CodeTipoTrabalho()
        {
            List<DDMessageString> result = DBTipoTrabalhoFH.GetAll().Select(x => new DDMessageString()
            {
                id = x.Codigo,
                value = x.Descricao
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult PrecoVendaRecurso_CodeTipoTrabalho_FH()
        {
            List<DDMessageRelated> result = DBTipoTrabalhoFH.GetAll().Select(x => new DDMessageRelated()
            {
                id = x.Codigo,
                value = x.Codigo + " - " + x.Descricao,
                extra = x.Descricao
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult PrecoVendaRecurso_Code_FH()
        {
            List<DDMessageString> result = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "").Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Code + " - " + x.Name
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult TipoRequisicoesLista()
        {
            List<DDMessageString> result = DBRequesitionType.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Description
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetNAVJobNo()
        {
            List<DDMessageString> result = DBNAV2017Job.GetJob(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
            {
                id = x.No_

            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetNAVVendor()
        {
            List<DDMessageString> result = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
            {
                id = x.No_,
                value = x.Name

            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetNAVVendorComboGrid()
        {
            List<NAVVendorViewModel> result = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).ToList();
            return Json(result);
        }


        [HttpPost]
        public JsonResult GetMealTypes()
        {
            List<DDMessage> result = DBMealTypes.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetPlaces()
        {
            List<DDMessage> result = DBPlaces.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        public JsonResult GetUnitStockeeping()
        {
            List<DDMessageRelated> result = DBStockkeepingUnit.GetAll().Select(x => new DDMessageRelated()
            {
                id = x.NºProduto,
                value = x.Descrição,
                extra = x.CustoUnitário.ToString(),
                extra2 = x.CódUnidadeMedidaProduto
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProducts()
        {
            List<NAVProductsViewModel> products = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();
            return Json(products);
        }

        [HttpPost]
        public JsonResult GetProductseSUCH()
        {
            List<DDMessageString> result = DBFichaProduto.GetAll().Select(x => new DDMessageString()
            {
                id = x.Nº,
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProductsCode()
        {
            List<DDMessageRelated> result = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageRelated()
            {
                id = x.Code,
                value = x.Name,
                extra = x.MeasureUnit
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProductsForCurrentUser([FromBody] JObject requestParams)
        {
            string rootAreaId = string.Empty;
            string requisitionType = string.Empty;
            string locationCode = string.Empty;
            List<NAVProductsViewModel> products = new List<NAVProductsViewModel>();
            if (requestParams != null)
            {
                rootAreaId = requestParams["rootAreaId"].ToString();
                requisitionType = requestParams["requisitionType"].ToString();
                locationCode = requestParams["locationCode"].ToString();
            }
            else
            {
                products = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();
            }
            //List<NAVDimValueViewModel> userDimensionValues = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name);
            //string allowedProductsFilter = userDimensionValues.GenerateNAVProductFilter(rootAreaId, true);
           
            string allowedProductsFilter = rootAreaId.GenerateNAVProductFilter();
            List<NAVProductsViewModel> productsReqParams = DBNAV2017Products.GetProductsForDimensions(_config.NAVDatabaseName, _config.NAVCompanyName, allowedProductsFilter, requisitionType, locationCode).ToList();
            if (productsReqParams != null && productsReqParams.Count > 0)
            {
                products = productsReqParams;
            }
            
            return Json(products);
        }

        [HttpPost]
        public JsonResult GetAllProductsPortal([FromBody] JObject requestParams)
        {
            string rootAreaId = string.Empty;
            string requisitionType = string.Empty;
            string locationCode = string.Empty;
            List<NAVProductsViewModel> products = new List<NAVProductsViewModel>();
            if (requestParams != null)
            {
                rootAreaId = requestParams["rootAreaId"].ToString();
                requisitionType = requestParams["requisitionType"].ToString();
                locationCode = requestParams["locationCode"].ToString();
            }
            else
            {
                products = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();
            }
            //List<NAVDimValueViewModel> userDimensionValues = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name);
            //string allowedProductsFilter = userDimensionValues.GenerateNAVProductFilter(rootAreaId, true);

            string allowedProductsFilter = rootAreaId.GenerateNAVProductFilter();
            List<NAVProductsViewModel> productsReqParams = DBNAV2017Products.GetProductsForDimensions(_config.NAVDatabaseName, _config.NAVCompanyName, allowedProductsFilter, requisitionType, locationCode).ToList();
            if (productsReqParams != null && productsReqParams.Count > 0)
            {
                products = productsReqParams;
            }

            //ADICIONA OS PRODUTOS DA FICHA DE PRODUTO
            List<FichaProduto> FichaProdutos = DBFichaProduto.GetAll();

            //REMOVE TODOS OS PRODUTOS CUJO O ID ESTEJA NA TABELA FICHA PRODUTO
            products.RemoveAll(x => FichaProdutos.Any(y => y.Nº == x.Code));

            FichaProdutos.ForEach(x =>
            {
                NAVProductsViewModel Product = new NAVProductsViewModel();

                Product.Code = x.Nº;
                Product.Name = x.Descrição;
                Product.MeasureUnit = x.UnidadeMedidaBase;
                Product.UnitCost = x.CustoUnitário;
                Product.LastCostDirect = x.PreçoUnitário;

                products.Add(Product);
            });

            return Json(products.OrderBy(x => x.Code));
        }

        [HttpPost]
        public JsonResult GetClassificationFilesTechniques()
        {
            List<DDMessageString> products = DBClassificationFilesTechniques.GetAllFiles().Select(x => new DDMessageString()
            {
                id = Convert.ToString(x.Código),
                value = x.Descrição
            }).ToList();

            return Json(products);
        }

        [HttpPost]
        public JsonResult GetGroupsClassificationTechniques()
        {
            List<DDMessage> result = DBClassificationFilesTechniques.GetTypeFiles(1).Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetActionsConfection()
        {
            List<DDMessage> result = DBActionsConfection.GetAll().Select(x => new DDMessage()
            {
                id = x.Código,
                value = x.Descrição
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetStockkeepingUnit(string product)
        {
            List<NAVStockKeepingUnitViewModel> StockkeepingUnit = DBNAV2017StockKeepingUnit.GetByProductsNo(_config.NAVDatabaseName, _config.NAVCompanyName, product).ToList();

            return Json(StockkeepingUnit);
        }

        //[HttpPost]
        //public JsonResult GetPurchaseHeader()
        //{
        //    List<DDMessageString> Pheader = DBNAV2017PurchaseHeader.GetPurchaseHeader(_config.NAVDatabaseName, _config.NAVCompanyName).Select(x => new DDMessageString()
        //    {
        //        id = x.No_
        //    }).ToList();

        //    return Json(Pheader);
        //}

        [HttpPost]
        public JsonResult GetLocalMarketRegions()
        {
            List<EnumDataString> result = EnumerablesFixed.LocalMarketRegions;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetCookingTechnique()
        {
            List<EnumData> result = EnumerablesFixed.CookingTechniqueTypes;
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetLinesRecTechnicPlatesType()
        {
            List<EnumData> result = EnumerablesFixed.LinesRecTechnicPlastesType;
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetLPlatesTechnicalFilesType()
        {
            List<EnumData> result = EnumerablesFixed.LPlatesTechnicalFiles_Type;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetRecTechnicPlatesType()
        {
            List<DDMessageString> result = DBRecordTechnicalOfPlates.GetAll().Select(x => new DDMessageString()
            {
                id = x.NºPrato,
                value = x.Descrição
            }).ToList();
            return Json(result);
        }
        [HttpPost]
        public IActionResult GetLinesRecordTechnicalOfPlatesCode([FromBody] string type)
        {
            List<DDMessageRelated> result = new List<DDMessageRelated>();
            switch (type)
            {
                case "1":
                    result = DBRecordTechnicalOfPlates.GetAll().Select(x => new DDMessageRelated() { id = x.NºPrato, value = x.Descrição }).ToList();
                    break;
                case "2":
                    result = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, "").Select(x => new DDMessageRelated() { id = x.Code, value = x.Name }).ToList();
                    break;
            }
            return Json(result);
        }



        // NR 20180223 Procedimentos CCP 
        [HttpPost]
        public JsonResult GetTipoLinhasProdutosCCP()
        {
            List<EnumData> Tipo = EnumerablesFixed.TipoLinhasProdutosCCP;

            return Json(Tipo);
        }

        // NR 20180228 Procedimentos CCP 
        [HttpPost]
        public JsonResult GetObjectoDeContratoCCP()
        {
            List<EnumData> ObjContrato = EnumerablesFixed.ObjectoDeContratoCCP;

            return Json(ObjContrato);
        }

        //ACORDO DE PREÇOS
        [HttpPost]
        public JsonResult Get_AP_FormaEntrega()
        {
            List<EnumData> result = EnumerablesFixed.AP_FormaEntrega;
            return Json(result);
        }

        //ACORDO DE PREÇOS
        [HttpPost]
        public JsonResult Get_AP_TipoPreco()
        {
            List<EnumData> result = EnumerablesFixed.AP_TipoPreco;
            return Json(result);
        }

        //CLIENTE
        [HttpPost]
        public JsonResult GetCustomerTypes()
        {
            List<EnumData> result = EnumerablesFixed.Tipo_Cliente;

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetCustomerNatures()
        {
            List<EnumData> result = EnumerablesFixed.Natureza_Cliente;
            return Json(result);
        }

        public JsonResult GetBillingReceptionStates()
        {
            var items = Data.EnumHelper.GetItemsFor(typeof(BillingReceptionStates));
            List<EnumData> result = items.Select(x => new EnumData { Id = x.Key, Value = x.Value }).ToList();
            return Json(result);
        }

        public JsonResult GetBillingDocumentTypes()
        {
            var items = Data.EnumHelper.GetItemsFor(typeof(BillingDocumentTypes));
            List<EnumData> result = items.Select(x => new EnumData { Id = x.Key, Value = x.Value }).ToList();
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetOrders([FromBody] string supplierId)
        {
            List<Data.ViewModel.Compras.PurchaseHeader> result = DBNAV2017Purchases.GetOrdersBySupplier(_config.NAVDatabaseName, _config.NAVCompanyName, supplierId);
            return Json(result);
        }

        //TELEMÓVEIS
        [HttpPost]
        public JsonResult Get_Telemoveis_Tipo()
        {
            List<EnumData> result = EnumerablesFixed.Telemoveis_Tipo;
            return Json(result);
        }

        [HttpPost]
        public JsonResult Get_Telemoveis_Estado()
        {
            List<EnumData> result = EnumerablesFixed.Telemoveis_Estado;
            return Json(result);
        }

        [HttpPost]
        public JsonResult Get_Telemoveis_Devolvido()
        {
            List<EnumData> result = EnumerablesFixed.Telemoveis_Devolvido;
            return Json(result);
        }

        [HttpPost]
        public JsonResult Get_Cartoes_TipoServico()
        {
            List<EnumData> result = EnumerablesFixed.Cartoes_TipoServico;
            return Json(result);
        }

        [HttpPost]
        public JsonResult Get_Cartoes_Estado()
        {
            List<EnumData> result = EnumerablesFixed.Cartoes_Estado;
            return Json(result);
        }

        [HttpPost]
        public JsonResult Get_Cartoes_GPRS()
        {
            List<EnumData> result = EnumerablesFixed.Cartoes_GPRS;
            return Json(result);
        }

        [HttpPost]
        public JsonResult Get_Cartoes_BarramentoVoz()
        {
            List<EnumData> result = EnumerablesFixed.Cartoes_BarramentoVoz;
            return Json(result);
        }

        [HttpPost]
        public JsonResult Get_Cartoes_TarifarioDados()
        {
            List<EnumData> result = EnumerablesFixed.Cartoes_TarifariosDados;
            return Json(result);
        }

        public JsonResult GetBillingReceptionAreas()
        {
            var items = Data.EnumHelper.GetItemsFor(typeof(BillingReceptionAreas));
            List<EnumData> result = items.Select(x => new EnumData { Id = x.Key, Value = x.Value }).ToList();
            return Json(result);
        }

        public JsonResult GetBillingReceptionUserProfiles()
        {
            var items = Data.EnumHelper.GetItemsFor(typeof(BillingReceptionUserProfiles));
            List<EnumData> result = items.Select(x => new EnumData { Id = x.Key, Value = x.Value }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProdutosUnidadesMedidas()
        {
            List<DDMessageString> result = DBUnidadeMedida.GetAll().Select(x => new DDMessageString()
            {
                id = x.Code,
                value = x.Description
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProdutosTipos()
        {
            List<DDMessage> result = EnumerablesFixed.ProdutosTipo.Select(x => new DDMessage()
            {
                id = x.Id,
                value = x.Value
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProdutosTiposRefeicao()
        {
            List<DDMessageString> result = DBTiposRefeicao.GetAll().Select(x => new DDMessageString()
            {
                id = x.Código.ToString(),
                value = x.Descrição
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult Get_Consulta_Mercado_Destino()
        {
            List<EnumData> result = EnumerablesFixed.Destino;
            return Json(result);
        }

        [HttpPost]
        public JsonResult Get_Consulta_Mercado_Estado()
        {
            List<EnumData> result = EnumerablesFixed.Estado;
            return Json(result);
        }

        [HttpPost]
        public JsonResult Get_Consulta_Mercado_Fase()
        {
            List<EnumData> result = EnumerablesFixed.Fase;
            return Json(result);
        }

        [HttpPost]
        public JsonResult Get_Consulta_Mercado_Modalidade()
        {
            List<EnumData> result = EnumerablesFixed.Modalidade;
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
        public string extra2 { get; set; }
    }

    public class DDMessageRelatedBool
    {
        public string id { get; set; }
        public string value { get; set; }
        public byte extra { get; set; }
    }
}