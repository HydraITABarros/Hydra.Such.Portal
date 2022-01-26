using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.Contracts;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data.Database;
using Microsoft.Extensions.Options;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Newtonsoft.Json.Linq;
using Hydra.Such.Portal.Services;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data;
using Newtonsoft.Json;
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Data.ViewModel.Clients;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using Hydra.Such.Data.Logic.ProjectDiary;
using Hydra.Such.Data.ViewModel.ProjectDiary;
using Hydra.Such.Data.Logic.Approvals;

namespace Hydra.Such.Portal.Controllers
{
    public class ContratosController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly GeneralConfigurations _generalConfig;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ContratosController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            _generalConfig = appSettingsGeneral.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        #region Contratos
        public IActionResult Index(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Contratos);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UploadURL = _generalConfig.FileUploadFolder;
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult ContratosByCliente(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Contratos);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ClienteNo = id ?? "";
                ViewBag.UploadURL = _generalConfig.FileUploadFolder;
                ViewBag.UPermissions = UPerm;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult PropostasByCliente(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Contratos);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ClienteNo = id ?? "";
                ViewBag.UploadURL = _generalConfig.FileUploadFolder;
                ViewBag.UPermissions = UPerm;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult OportunidadesByCliente(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Contratos);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ClienteNo = id ?? "";
                ViewBag.UploadURL = _generalConfig.FileUploadFolder;
                ViewBag.UPermissions = UPerm;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult IndexInternos(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ContratosInternos);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UploadURL = _generalConfig.FileUploadFolder;
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult IndexQuotas(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ContratosQuotas);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UploadURL = _generalConfig.FileUploadFolder;
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult ListContratosLinhas(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.LinhasContratos);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UploadURL = _generalConfig.FileUploadFolder;
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult ListContratosInternosLinhas(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.LinhasContratosInternos);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UploadURL = _generalConfig.FileUploadFolder;
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        //public IActionResult Details(string id, string version)
        //{
        //    UserAccessesViewModel UPerm = DBUserAccesses.ParseToViewModel(DBUserAccesses.GetByUserId(User.Identity.Name).Where(x => x.Área == 1 && x.Funcionalidade == 2).FirstOrDefault());
        //    if (UPerm != null && UPerm.Read.Value)
        //    {
        //        ViewBag.ContractNo = id ?? "";
        //        ViewBag.VersionNo = version ?? "";
        //        ViewBag.UPermissions = UPerm;
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("AccessDenied", "Error");
        //    }



        //}

        public IActionResult DetalhesContrato(string id, string version = "", bool isHistoric = false)
        {
            bool hist = isHistoric;
            string ifHistoric;

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Contratos);
            UserAccessesViewModel UPermCriarFatura = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ContratosCriarFatura);
            UserAccessesViewModel UPermContratosRequisicoesCliente = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ContratosRequisicoesCliente);
            ConfigUtilizadores UTilizador = DBUserConfigurations.GetById(User.Identity.Name);

            if (UPerm != null && UPerm.Read.Value)
            {
                Contratos cContract = null;
                if (version != "")
                    cContract = DBContracts.GetByIdAndVersion(id, int.Parse(version));
                else
                    cContract = DBContracts.GetByIdLastVersion(id);

                //if (cContract != null && (cContract.Arquivado == true || cContract.EstadoAlteração == 2))
                if (cContract != null && cContract.Arquivado == true)
                {
                    UPerm.Update = false;
                    UPerm.Delete = false;
                }
                if (hist == true)
                {
                    ViewBag.Historic = "(Histórico) ";
                    ifHistoric = "true";
                }
                else
                {
                    ViewBag.Historic = "";
                    ifHistoric = "false";
                }

                ViewBag.ifHistoric = ifHistoric;
                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.UPermissions = UPerm;
                ViewBag.UPermissionsCriarFatura = UPermCriarFatura;
                ViewBag.UPermContratosRequisicoesCliente = UPermContratosRequisicoesCliente;
                ViewBag.VerFaturas = UTilizador != null ? UTilizador.VerFaturas.HasValue ? UTilizador.VerFaturas : false : false;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesContratoInternos(string id, string version = "", bool isHistoric = false)
        {
            bool hist = isHistoric;
            string ifHistoric;

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ContratosInternos);
            if (UPerm != null && UPerm.Read.Value)
            {
                Contratos cContract = null;
                if (version != "")
                    cContract = DBContracts.GetByIdAndVersion(id, int.Parse(version));
                else
                    cContract = DBContracts.GetByIdLastVersion(id);

                //if (cContract != null && (cContract.Arquivado == true || cContract.EstadoAlteração == 2))
                if (cContract != null && cContract.Arquivado == true)
                {
                    UPerm.Update = false;
                    UPerm.Delete = false;
                }
                if (hist == true)
                {
                    ViewBag.Historic = "(Histórico) ";
                    ifHistoric = "true";
                }
                else
                {
                    ViewBag.Historic = "";
                    ifHistoric = "false";
                }

                ViewBag.ifHistoric = ifHistoric;
                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesContratoQuotas(string id, string version = "", bool isHistoric = false)
        {
            bool hist = isHistoric;
            string ifHistoric;

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ContratosQuotas);
            if (UPerm != null && UPerm.Read.Value)
            {
                Contratos cContract = null;
                if (version != "")
                    cContract = DBContracts.GetByIdAndVersion(id, int.Parse(version));
                else
                    cContract = DBContracts.GetByIdLastVersion(id);

                //if (cContract != null && (cContract.Arquivado == true || cContract.EstadoAlteração == 2))
                if (cContract != null && cContract.Arquivado == true)
                {
                    UPerm.Update = false;
                    UPerm.Delete = false;
                }
                if (hist == true)
                {
                    ViewBag.Historic = "(Histórico) ";
                    ifHistoric = "true";
                }
                else
                {
                    ViewBag.Historic = "";
                    ifHistoric = "false";
                }

                ViewBag.ifHistoric = ifHistoric;
                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult AvencaFixa()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ContratosAvencaFixa);

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

        public JsonResult GetListContractsByArea([FromBody] JObject requestParams)
        {
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();
            int Historic = 0;
            if (requestParams["Historic"] != null)
                Historic = int.Parse(requestParams["Historic"].ToString());

            List<Contratos> ContractsList = null;

            if ((Archived == 0 || ContractNo == "") && (Historic == 0))
            {
                if (requestParams["Type"] != null)
                {
                    int type = int.Parse(requestParams["Type"].ToString());
                    ContractsList = DBContracts.GetAllByContractTypeAndType(ContractType.Contract, type);
                }
                else
                {
                    ContractsList = DBContracts.GetAllByContractType(ContractType.Contract);
                }
                ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
            }
            else if (Historic == 1)
            {
                ContractsList = DBContracts.GetAllHistoric((int)ContractType.Contract);
            }
            else
            {
                ContractsList = DBContracts.GetByNo(ContractNo, true);
            }

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));
            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional));
            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));


            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x, _config.NAVDatabaseName, _config.NAVCompanyName)));

            List<EnumData> status = EnumerablesFixed.ContractStatus;
            result.ForEach(x => { x.StatusDescription = status.Where(y => y.Id == x.Status).Select(y => y.Value).FirstOrDefault(); });

            return Json(result);
        }

        public JsonResult GetListContracts([FromBody] JObject requestParams)
        {
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();
            int Historic = 0;
            if (requestParams["Historic"] != null)
                Historic = int.Parse(requestParams["Historic"].ToString());

            List<Contratos> ContractsList = new List<Contratos>();
            List<ContractViewModel> result = new List<ContractViewModel>();

            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);

            if ((Archived == 0 || ContractNo == "") && (Historic == 0))
            {
                int type = 1; //CONTRATOS
                //ContractsList = DBContracts.GetAllByContractTypeAndType((int)ContractType.Contract, type);
                ContractsList = DBContracts.GetAllListContractByContractTypeAndType(userDimensions, (int)ContractType.Contract, type);


                ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
            }
            else if (Historic == 1)
            {
                //ContractsList = DBContracts.GetAllHistoric((int)ContractType.Contract);
                ContractsList = DBContracts.GetAllListContractHistoric(userDimensions, (int)ContractType.Contract);
                ContractsList.RemoveAll(x => x.Tipo != 1);
            }
            else
            {
                ContractsList = DBContracts.GetByNo(ContractNo, true);
            }

            //Apply User Dimensions Validations
            //List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            //if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
            //    ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));

            ////FunctionalAreas
            //if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
            //    ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && (y.ValorDimensão == x.CódigoÁreaFuncional || string.IsNullOrEmpty(x.CódigoÁreaFuncional))));

            ////ResponsabilityCenter
            //if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
            //    ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && (y.ValorDimensão == x.CódigoCentroResponsabilidade || string.IsNullOrEmpty(x.CódigoCentroResponsabilidade))));

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x)));
            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<EnumData> status = EnumerablesFixed.ContractStatus;
            List<UnidadePrestação> AllUnidadesPrestacao = DBFetcUnit.GetAll();
            List<EnumData> AllEstadosAlteracao = EnumerablesFixed.ContractChangeStatus;
            List<EnumData> AllRescisoes = EnumerablesFixed.ContractTerminationDeadlineNotice;
            List<EnumData> AllCondicoesRenovacao = EnumerablesFixed.ContractTerminationTerms;
            List<EnumData> AllCondicoesPagamento = EnumerablesFixed.ContractPaymentTerms;
            List<EnumData> ALLTiposFaturacao = EnumerablesFixed.ContractBillingTypes;
            List<EnumData> AllPeriodosFatura = EnumerablesFixed.ContractInvoicePeriods;
            List<LinhasContratos> AllLines = DBContractLines.GetAll();
            List<LinhasContratos> AllLinesContract = null;

            result.ForEach(x =>
            {
                x.ClientName = !string.IsNullOrEmpty(x.ClientNo) ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault().Name : "" : "";
                x.StatusDescription = x.Status != null ? status.Where(y => y.Id == x.Status).FirstOrDefault() != null ? status.Where(y => y.Id == x.Status).FirstOrDefault().Value : "" : "";
                x.FixedVowsAgreementText = x.FixedVowsAgreement.HasValue ? x.FixedVowsAgreement == true ? "Sim" : "Não" : "Não";

                x.ProvisionUnitText = x.ProvisionUnit != null ? AllUnidadesPrestacao.Where(y => y.Código == x.ProvisionUnit).FirstOrDefault() != null ? AllUnidadesPrestacao.Where(y => y.Código == x.ProvisionUnit).FirstOrDefault().Descrição : "" : "";
                x.ChangeStatusText = x.ChangeStatus != null ? AllEstadosAlteracao.Where(y => y.Id == x.ChangeStatus).FirstOrDefault() != null ? AllEstadosAlteracao.Where(y => y.Id == x.ChangeStatus).FirstOrDefault().Value : "" : "";
                x.TerminationTermNoticeText = x.TerminationTermNotice != null ? AllRescisoes.Where(y => y.Id == x.TerminationTermNotice).FirstOrDefault() != null ? AllRescisoes.Where(y => y.Id == x.TerminationTermNotice).FirstOrDefault().Value : "" : "";
                x.RenovationConditionsText = x.RenovationConditions != null ? AllCondicoesRenovacao.Where(y => y.Id == x.RenovationConditions).FirstOrDefault() != null ? AllCondicoesRenovacao.Where(y => y.Id == x.RenovationConditions).FirstOrDefault().Value : "" : "";
                x.PaymentTermsText = x.PaymentTerms != null ? AllCondicoesPagamento.Where(y => y.Id == x.PaymentTerms).FirstOrDefault() != null ? AllCondicoesPagamento.Where(y => y.Id == x.PaymentTerms).FirstOrDefault().Value : "" : "";
                x.CustomerSignedText = x.CustomerSigned.HasValue ? x.CustomerSigned == true ? "Sim" : "Não" : "Não";
                x.InterestsText = x.Interests.HasValue ? x.Interests == true ? "Sim" : "Não" : "Não";
                x.BillingTypeText = x.BillingType != null ? ALLTiposFaturacao.Where(y => y.Id == x.BillingType).FirstOrDefault() != null ? ALLTiposFaturacao.Where(y => y.Id == x.BillingType).FirstOrDefault().Value : "" : "";
                x.InvocePeriodText = x.InvocePeriod != null ? AllPeriodosFatura.Where(y => y.Id == x.InvocePeriod).FirstOrDefault() != null ? AllPeriodosFatura.Where(y => y.Id == x.InvocePeriod).FirstOrDefault().Value : "" : "";

                AllLinesContract = AllLines.Where(y => y.NºContrato == x.ContractNo && y.NºVersão == x.VersionNo).ToList();
                if (AllLinesContract != null && AllLinesContract.Count > 0)
                    x.SomatorioLinhas = AllLinesContract.Sum(y => (y.Quantidade.HasValue ? (decimal)y.Quantidade : 0) * (y.PreçoUnitário.HasValue ? (decimal)y.PreçoUnitário : 0));
            });

            return Json(result);
        }

        public JsonResult GetListContractsByCliente([FromBody] JObject requestParams)
        {
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();
            int Historic = 0;
            if (requestParams["Historic"] != null)
                Historic = int.Parse(requestParams["Historic"].ToString());
            string ClienteNo = requestParams["ClienteNo"].ToString();

            List<Contratos> ContractsList = null;

            if ((Archived == 0 || ContractNo == "") && (Historic == 0))
            {
                int type = 1; //CONTRATOS
                ContractsList = DBContracts.GetAllByContractTypeAndType(ContractType.Contract, type);
                ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
            }
            else if (Historic == 1)
            {
                ContractsList = DBContracts.GetAllHistoric((int)ContractType.Contract);
                ContractsList.RemoveAll(x => x.Tipo != 1);
            }
            else
            {
                ContractsList = DBContracts.GetByNo(ContractNo, true);
            }

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));

            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && (y.ValorDimensão == x.CódigoÁreaFuncional || string.IsNullOrEmpty(x.CódigoÁreaFuncional))));

            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));

            //Cliente
            if (!string.IsNullOrEmpty(ClienteNo))
                ContractsList.RemoveAll(x => x.NºCliente != ClienteNo);

            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x)));
            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<EnumData> status = EnumerablesFixed.ContractStatus;

            result.ForEach(x =>
            {
                x.ClientName = !string.IsNullOrEmpty(x.ClientNo) ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault().Name : "" : "";
                x.StatusDescription = x.Status != null ? status.Where(y => y.Id == x.Status).FirstOrDefault() != null ? status.Where(y => y.Id == x.Status).FirstOrDefault().Value : "" : "";
            });

            return Json(result);
        }

        public JsonResult GetListContractsLines([FromBody] JObject requestParams)
        {
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();
            int Historic = 0;
            if (requestParams["Historic"] != null)
                Historic = int.Parse(requestParams["Historic"].ToString());

            List<Contratos> ContractsList = null;
            List<LinhasContratos> ContractsLinesList = null;
            Contratos contrato = new Contratos();
            //int type = 1; //CONTRATOS

            if ((Archived == 0 || ContractNo == "") && (Historic == 0))
            {
                int type = 1; //CONTRATOS
                ContractsList = DBContracts.GetAll().Where(x => x.TipoContrato == (int)ContractType.Contract && x.Tipo == type).ToList();
                ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);

                ContractsLinesList = DBContractLines.GetAll().Where(x => x.TipoContrato == (int)ContractType.Contract).ToList();
                ContractsLinesList.RemoveAll(x => ContractsList.Find(y => y.NºDeContrato == x.NºContrato && y.NºVersão == x.NºVersão) == null);
            }
            else if (Historic == 1)
            {
                ContractsList = DBContracts.GetAll().Where(x => x.TipoContrato == (int)ContractType.Contract && x.Historico == true).ToList();
                ContractsList.RemoveAll(x => x.Tipo != 1);

                ContractsLinesList = DBContractLines.GetAll().Where(x => x.TipoContrato == (int)ContractType.Contract).ToList();
                ContractsLinesList.RemoveAll(x => ContractsList.Find(y => y.NºDeContrato == x.NºContrato && y.NºVersão == x.NºVersão) == null);
            }

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsLinesList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));

            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsLinesList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && (y.ValorDimensão == x.CódigoÁreaFuncional || string.IsNullOrEmpty(x.CódigoÁreaFuncional))));

            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsLinesList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));

            List<ContractLineViewModel> result = new List<ContractLineViewModel>();

            ContractsLinesList.ForEach(x => result.Add(DBContractLines.ParseToViewModel(x)));

            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<EnumData> AllStatus = EnumerablesFixed.ContractALLStatus;
            List<EnumData> AllContractBillingTypes = EnumerablesFixed.ContractBillingTypes;
            //List<ClientServicesViewModel> AllClientServices = new List<ClientServicesViewModel>();

            result.ForEach(x =>
            {
                contrato = ContractsList.Find(y => y.NºDeContrato == x.ContractNo && y.NºVersão == x.VersionNo);
                if (contrato != null)
                {
                    x.ContratoClienteCode = !string.IsNullOrEmpty(contrato.NºCliente) ? contrato.NºCliente : "";
                    x.ContratoClienteNome = !string.IsNullOrEmpty(contrato.NºCliente) ? AllClients.Where(y => y.No_ == contrato.NºCliente) != null ? AllClients.Where(y => y.No_ == contrato.NºCliente).FirstOrDefault().Name : "" : "";



                    x.ContractoEstado = contrato.Estado != null ? AllStatus.Where(y => y.Id == contrato.Estado) != null ? AllStatus.Where(y => y.Id == contrato.Estado).FirstOrDefault().Value : "" : "";
                    x.ContractEndereco = !string.IsNullOrEmpty(contrato.EnvioAEndereço) ? contrato.EnvioAEndereço : "";
                    x.ContratoCodigoPostal = !string.IsNullOrEmpty(contrato.EnvioACódPostal) ? contrato.EnvioACódPostal : "";
                    x.ContratoTipo = contrato.TipoContrato != null ? contrato.TipoContrato.ToString() : "";
                    x.ContratoAvencaFixa = contrato.ContratoAvençaFixa.HasValue ? contrato.ContratoAvençaFixa == true ? "Sim" : "Não" : "Não";
                    x.ContratoDataExpiracao = contrato.DataExpiração.HasValue ? Convert.ToDateTime(contrato.DataExpiração).ToShortDateString() : "";
                    x.ContratoTipoFaturacao = contrato.TipoFaturação != null ? AllContractBillingTypes.Where(y => y.Id == contrato.TipoFaturação) != null ? AllContractBillingTypes.Where(y => y.Id == contrato.TipoFaturação).FirstOrDefault().Value : "" : "";

                    //AllClientServices = DBClientServices.GetAllFromClientWithDescription(contrato.NºCliente);
                    //if (AllClientServices != null && AllClientServices.Count > 0 && x.ServiceClientNo != null && x.ServiceClientNo != "0")
                    //    x.ServiceClientName = AllClientServices.Where(y => y.ServiceCode == x.ServiceClientNo) != null ? AllClientServices.Where(y => y.ServiceCode == x.ServiceClientNo).FirstOrDefault().ServiceDescription : "";
                }
                x.ContratoTipo = !string.IsNullOrEmpty(x.ContratoTipo) ? x.ContratoTipo == "1" ? "Oportunidade" : x.ContratoTipo == "2" ? "Proposta" : x.ContratoTipo == "3" ? "Contrato" : "" : "";
            });

            return Json(result.OrderBy(x => x.ContractNo).ThenBy(y => y.VersionNo).ThenBy(z => z.LineNo));
        }

        public JsonResult GetListContractsInternos([FromBody] JObject requestParams)
        {
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();
            int Historic = 0;
            if (requestParams["Historic"] != null)
                Historic = int.Parse(requestParams["Historic"].ToString());

            List<Contratos> ContractsList = null;

            if ((Archived == 0 || ContractNo == "") && (Historic == 0))
            {
                int type = 2; //CONTRATOS INTERNOS
                ContractsList = DBContracts.GetAllByContractTypeAndType(ContractType.Contract, type);

                ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
            }
            else if (Historic == 1)
            {
                ContractsList = DBContracts.GetAllHistoric((int)ContractType.Contract);
                ContractsList.RemoveAll(x => x.Tipo != 2);
            }
            else
            {
                ContractsList = DBContracts.GetByNo(ContractNo, true);
            }

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));
            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional));
            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));

            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x)));
            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<EnumData> status = EnumerablesFixed.ContractStatus;

            result.ForEach(x =>
            {
                x.ClientName = !string.IsNullOrEmpty(x.ClientNo) ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault().Name : "" : "";
                x.StatusDescription = x.Status != null ? status.Where(y => y.Id == x.Status).FirstOrDefault() != null ? status.Where(y => y.Id == x.Status).FirstOrDefault().Value : "" : "";
            });

            return Json(result);
        }

        public JsonResult GetListContractsInternosLines([FromBody] JObject requestParams)
        {
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();
            int Historic = 0;
            if (requestParams["Historic"] != null)
                Historic = int.Parse(requestParams["Historic"].ToString());

            List<Contratos> ContractsList = null;
            List<LinhasContratos> ContractsLinesList = null;
            Contratos contrato = new Contratos();
            //int type = 1; //CONTRATOS

            if ((Archived == 0 || ContractNo == "") && (Historic == 0))
            {
                int type = 2; //CONTRATOS INTERNOS
                ContractsList = DBContracts.GetAll().Where(x => x.TipoContrato == (int)ContractType.Contract && x.Tipo == type).ToList();
                ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);

                ContractsLinesList = DBContractLines.GetAll().Where(x => x.TipoContrato == (int)ContractType.Contract).ToList();
                ContractsLinesList.RemoveAll(x => ContractsList.Find(y => y.NºDeContrato == x.NºContrato && y.NºVersão == x.NºVersão) == null);
            }
            else if (Historic == 1)
            {
                int type = 2; //CONTRATOS INTERNOS
                ContractsList = DBContracts.GetAll().Where(x => x.TipoContrato == (int)ContractType.Contract && x.Tipo == type && x.Historico == true).ToList();
                ContractsList.RemoveAll(x => x.Tipo != 1);

                ContractsLinesList = DBContractLines.GetAll().Where(x => x.TipoContrato == (int)ContractType.Contract).ToList();
                ContractsLinesList.RemoveAll(x => ContractsList.Find(y => y.NºDeContrato == x.NºContrato && y.NºVersão == x.NºVersão) == null);
            }

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsLinesList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));

            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsLinesList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && (y.ValorDimensão == x.CódigoÁreaFuncional || string.IsNullOrEmpty(x.CódigoÁreaFuncional))));

            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsLinesList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));

            List<ContractLineViewModel> result = new List<ContractLineViewModel>();

            ContractsLinesList.ForEach(x => result.Add(DBContractLines.ParseToViewModel(x)));

            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<EnumData> AllStatus = EnumerablesFixed.ContractStatus;
            List<EnumData> AllContractBillingTypes = EnumerablesFixed.ContractBillingTypes;
            //List<ClientServicesViewModel> AllClientServices = new List<ClientServicesViewModel>();

            result.ForEach(x =>
            {
                contrato = ContractsList.Find(y => y.NºDeContrato == x.ContractNo && y.NºVersão == x.VersionNo);
                if (contrato != null)
                {
                    x.ContratoClienteCode = !string.IsNullOrEmpty(contrato.NºCliente) ? contrato.NºCliente : "";
                    x.ContratoClienteNome = !string.IsNullOrEmpty(contrato.NºCliente) ? AllClients.Where(y => y.No_ == contrato.NºCliente) != null ? AllClients.Where(y => y.No_ == contrato.NºCliente).FirstOrDefault().Name : "" : "";



                    x.ContractoEstado = contrato.Estado != null ? AllStatus.Where(y => y.Id == contrato.Estado) != null ? AllStatus.Where(y => y.Id == contrato.Estado).FirstOrDefault().Value : "" : "";
                    x.ContractEndereco = !string.IsNullOrEmpty(contrato.EnvioAEndereço) ? contrato.EnvioAEndereço : "";
                    x.ContratoCodigoPostal = !string.IsNullOrEmpty(contrato.EnvioACódPostal) ? contrato.EnvioACódPostal : "";
                    x.ContratoTipo = contrato.TipoContrato != null ? contrato.TipoContrato.ToString() : "";
                    x.ContratoAvencaFixa = contrato.ContratoAvençaFixa.HasValue ? contrato.ContratoAvençaFixa == true ? "Sim" : "Não" : "Não";
                    x.ContratoDataExpiracao = contrato.DataExpiração.HasValue ? Convert.ToDateTime(contrato.DataExpiração).ToShortDateString() : "";
                    x.ContratoTipoFaturacao = contrato.TipoFaturação != null ? AllContractBillingTypes.Where(y => y.Id == contrato.TipoFaturação) != null ? AllContractBillingTypes.Where(y => y.Id == contrato.TipoFaturação).FirstOrDefault().Value : "" : "";

                    //AllClientServices = DBClientServices.GetAllFromClientWithDescription(contrato.NºCliente);
                    //if (AllClientServices != null && AllClientServices.Count > 0 && x.ServiceClientNo != null && x.ServiceClientNo != "0")
                    //    x.ServiceClientName = AllClientServices.Where(y => y.ServiceCode == x.ServiceClientNo) != null ? AllClientServices.Where(y => y.ServiceCode == x.ServiceClientNo).FirstOrDefault().ServiceDescription : "";
                }
                x.ContratoTipo = !string.IsNullOrEmpty(x.ContratoTipo) ? x.ContratoTipo == "1" ? "Oportunidade" : x.ContratoTipo == "2" ? "Proposta" : x.ContratoTipo == "3" ? "Contrato" : "" : "";
            });

            return Json(result.OrderBy(x => x.ContractNo).ThenBy(y => y.VersionNo).ThenBy(z => z.LineNo));
        }

        public JsonResult GetListContractsQuotas([FromBody] JObject requestParams)
        {
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();
            int Historic = 0;
            if (requestParams["Historic"] != null)
                Historic = int.Parse(requestParams["Historic"].ToString());

            List<Contratos> ContractsList = null;

            if ((Archived == 0 || ContractNo == "") && (Historic == 0))
            {
                int type = 3; //CONTRATOS QUOTAS
                ContractsList = DBContracts.GetAllByContractTypeAndType(ContractType.Contract, type);
                ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
            }
            else if (Historic == 1)
            {
                ContractsList = DBContracts.GetAllHistoric((int)ContractType.Contract);
                ContractsList.RemoveAll(x => x.Tipo != 3);
            }
            else
            {
                ContractsList = DBContracts.GetByNo(ContractNo, true);
            }

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));
            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional));
            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));


            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x)));
            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<EnumData> status = EnumerablesFixed.ContractALLStatus;

            result.ForEach(x =>
            {
                x.ClientName = !string.IsNullOrEmpty(x.ClientNo) ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault().Name : "" : "";
                x.StatusDescription = x.Status != null ? status.Where(y => y.Id == x.Status).FirstOrDefault() != null ? status.Where(y => y.Id == x.Status).FirstOrDefault().Value : "" : "";
            });

            return Json(result);
        }

        [HttpPost]
        public JsonResult DataInicioChanged([FromBody] ContractViewModel data)
        {
            bool result = true;
            if (data != null && !string.IsNullOrEmpty(data.ContractNo) && data.VersionNo > 0)
            {
                List<LinhasContratos> AllLines = DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo);
                AllLines.ForEach(line =>
                {
                    line.DataInícioVersão = Convert.ToDateTime(data.StartData);
                    line.UtilizadorModificação = User.Identity.Name;
                    if (DBContractLines.Update(line) == null)
                    {
                        result = false;
                    }
                });

                if (result == true)
                {
                    if (DBContracts.Update(DBContracts.ParseToDB(data)) == null)
                    {
                        result = false;
                    }
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DataFimChanged([FromBody] ContractViewModel data)
        {
            bool result = true;
            if (data != null && !string.IsNullOrEmpty(data.ContractNo) && data.VersionNo > 0)
            {
                List<LinhasContratos> AllLines = DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo);
                AllLines.ForEach(line =>
                {
                    line.DataFimVersão = Convert.ToDateTime(data.DueDate);
                    line.UtilizadorModificação = User.Identity.Name;
                    if (DBContractLines.Update(line) == null)
                    {
                        result = false;
                    }
                });

                if (result == true)
                {
                    if (DBContracts.Update(DBContracts.ParseToDB(data)) == null)
                    {
                        result = false;
                    }
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult CodeRegiaoChanged([FromBody] ContractViewModel data)
        {
            bool result = true;
            if (data != null && !string.IsNullOrEmpty(data.ContractNo) && data.VersionNo > 0)
            {
                List<LinhasContratos> AllLines = DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo);
                AllLines.ForEach(line =>
                {
                    line.CódigoRegião = !string.IsNullOrEmpty(data.CodeRegion) ? data.CodeRegion : "";
                    line.UtilizadorModificação = User.Identity.Name;
                    if (DBContractLines.Update(line) == null)
                    {
                        result = false;
                    }
                });

                if (result == true)
                {
                    if (DBContracts.Update(DBContracts.ParseToDB(data)) == null)
                    {
                        result = false;
                    }
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult CodeAreaChanged([FromBody] ContractViewModel data)
        {
            bool result = true;
            if (data != null && !string.IsNullOrEmpty(data.ContractNo) && data.VersionNo > 0)
            {
                List<LinhasContratos> AllLines = DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo);
                AllLines.ForEach(line =>
                {
                    line.CódigoÁreaFuncional = !string.IsNullOrEmpty(data.CodeFunctionalArea) ? data.CodeFunctionalArea : "";
                    line.UtilizadorModificação = User.Identity.Name;
                    if (DBContractLines.Update(line) == null)
                    {
                        result = false;
                    }
                });

                if (result == true)
                {
                    if (DBContracts.Update(DBContracts.ParseToDB(data)) == null)
                    {
                        result = false;
                    }
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult CodeCrespChanged([FromBody] ContractViewModel data)
        {
            bool result = true;
            if (data != null && !string.IsNullOrEmpty(data.ContractNo) && data.VersionNo > 0)
            {
                List<LinhasContratos> AllLines = DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo);
                AllLines.ForEach(line =>
                {
                    line.CódigoCentroResponsabilidade = !string.IsNullOrEmpty(data.CodeResponsabilityCenter) ? data.CodeResponsabilityCenter : "";
                    line.UtilizadorModificação = User.Identity.Name;
                    if (DBContractLines.Update(line) == null)
                    {
                        result = false;
                    }
                });

                if (result == true)
                {
                    if (DBContracts.Update(DBContracts.ParseToDB(data)) == null)
                    {
                        result = false;
                    }
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateNumeration([FromBody] ContractViewModel data)
        {
            //Get Project Numeration
            Configuração Cfg = DBConfigurations.GetById(1);
            int ProjectNumerationConfigurationId = 0;

            switch (data.ContractType)
            {
                case 1:
                    ProjectNumerationConfigurationId = Cfg.NumeraçãoOportunidades.Value;
                    break;
                case 2:
                    ProjectNumerationConfigurationId = Cfg.NumeraçãoPropostas.Value;
                    break;
                case 3:
                    ProjectNumerationConfigurationId = Cfg.NumeraçãoContratos.Value;
                    break;
                default:
                    break;
            }

            ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);

            //Validate if ProjectNo is valid
            if (!(data.ContractNo == "" || data.ContractNo == null) && !CfgNumeration.Manual.Value)
            {
                return Json("A numeração configurada para contratos não permite inserção manual.");
            }
            else if (data.ContractNo == "" && !CfgNumeration.Automático.Value)
            {
                return Json("É obrigatório inserir o Nº de Contrato.");
            }

            return Json("");
        }

        [HttpPost]
        public JsonResult GetContractDetails([FromBody] ContractViewModel data)
        {
            if (data != null)
            {
                Contratos cContract = null;
                ContractType? contractType = null;
                if (EnumHelper.ValidateRange(typeof(ContractType), data.ContractType))
                    contractType = (ContractType)data.ContractType;

                if (data.VersionNo != 0)
                {
                    cContract = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo, contractType);
                }
                else
                {
                    cContract = DBContracts.GetByIdLastVersion(data.ContractNo, contractType);
                }

                ContractViewModel result = new ContractViewModel();


                if (cContract != null)
                {
                    result = DBContracts.ParseToViewModel(cContract, _config.NAVDatabaseName, _config.NAVCompanyName);


                    //GET CLIENT REQUISITIONS
                    List<RequisiçõesClienteContrato> ClientRequisition = DBContractClientRequisition.GetByContract(result.ContractNo).OrderByDescending(x => x.DataInícioCompromisso).ThenBy(y => y.GrupoFatura).ToList();
                    result.ClientRequisitions = new List<ContractClientRequisitionViewModel>();

                    if (ClientRequisition != null)
                    {
                        ClientRequisition.ForEach(x => result.ClientRequisitions.Add(DBContractClientRequisition.ParseToViewModel(x)));
                    }

                    //GET INVOICE TEXTS
                    List<TextoFaturaContrato> InvoicesTexts = DBContractInvoiceText.GetByContract(result.ContractNo);
                    result.InvoiceTexts = new List<ContractInvoiceTextViewModel>();

                    if (InvoicesTexts != null)
                    {
                        InvoicesTexts.ForEach(x => result.InvoiceTexts.Add(DBContractInvoiceText.ParseToViewModel(x)));
                    }
                }
                else
                {
                    if (data.ContractType == 1)
                        result.Status = 1;
                    result.ClientRequisitions = new List<ContractClientRequisitionViewModel>();
                    result.InvoiceTexts = new List<ContractInvoiceTextViewModel>();
                }

                if (result != null && !string.IsNullOrEmpty(result.ClientNo) && !string.IsNullOrEmpty(result.CodeShippingAddress))
                {
                    NAVAddressesViewModel SHIP = DBNAV2017ShippingAddresses.GetByClientAndCode(result.ClientNo, result.CodeShippingAddress, _config.NAVDatabaseName, _config.NAVCompanyName);
                    if (SHIP != null)
                    {
                        result.ShippingName = SHIP.Name1;
                        result.ShippingName2 = SHIP.Name2;
                        result.ShippingAddress = SHIP.Address1;
                        result.ShippingAddress2 = SHIP.Address2;
                        result.ShippingZipCode = SHIP.ZipCode;
                        result.ShippingLocality = SHIP.City;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(result.ClientNo))
                    {
                        NAVClientsViewModel cli = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, result.ClientNo);
                        if (cli != null)
                        {
                            result.ShippingName = cli.Name;
                            result.ShippingName2 = "";
                            result.ShippingAddress = cli.Address1;
                            result.ShippingAddress2 = cli.Address2;
                            result.ShippingZipCode = cli.PostCode;
                            result.ShippingLocality = cli.City;
                        }
                        else
                        {
                            result.eReasonCode = 2;
                            result.eMessage = "Cliente não encontrado no NAV.";
                        }
                    }
                    else
                    {
                            result.ShippingName = "";
                            result.ShippingName2 = "";
                            result.ShippingAddress = "";
                            result.ShippingAddress2 = "";
                            result.ShippingZipCode = "";
                            result.ShippingLocality = "";
                    }
                }

                result.Type = 1; //CONTRATOS

                return Json(result);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult GetContractDetailsInternos([FromBody] ContractViewModel data)
        {
            if (data != null)
            {
                Contratos cContract = null;
                ContractType? contractType = null;
                if (EnumHelper.ValidateRange(typeof(ContractType), data.ContractType))
                    contractType = (ContractType)data.ContractType;
                ;
                if (data.VersionNo != 0)
                {
                    cContract = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo, contractType);
                }
                else
                {
                    cContract = DBContracts.GetByIdLastVersion(data.ContractNo, contractType);
                }

                ContractViewModel result = new ContractViewModel();


                if (cContract != null)
                {
                    result = DBContracts.ParseToViewModel(cContract, _config.NAVDatabaseName, _config.NAVCompanyName);


                    //GET CLIENT REQUISITIONS
                    List<RequisiçõesClienteContrato> ClientRequisition = DBContractClientRequisition.GetByContract(result.ContractNo);
                    result.ClientRequisitions = new List<ContractClientRequisitionViewModel>();

                    if (ClientRequisition != null)
                    {
                        ClientRequisition.ForEach(x => result.ClientRequisitions.Add(DBContractClientRequisition.ParseToViewModel(x)));
                    }

                    //GET INVOICE TEXTS
                    List<TextoFaturaContrato> InvoicesTexts = DBContractInvoiceText.GetByContract(result.ContractNo);
                    result.InvoiceTexts = new List<ContractInvoiceTextViewModel>();

                    if (InvoicesTexts != null)
                    {
                        InvoicesTexts.ForEach(x => result.InvoiceTexts.Add(DBContractInvoiceText.ParseToViewModel(x)));
                    }
                }
                else
                {
                    if (data.ContractType == 1)
                        result.Status = 1;
                    result.ClientRequisitions = new List<ContractClientRequisitionViewModel>();
                    result.InvoiceTexts = new List<ContractInvoiceTextViewModel>();
                }

                if (result != null && !string.IsNullOrEmpty(result.ClientNo) && !string.IsNullOrEmpty(result.CodeShippingAddress))
                {
                    NAVAddressesViewModel SHIP = DBNAV2017ShippingAddresses.GetByClientAndCode(result.ClientNo, result.CodeShippingAddress, _config.NAVDatabaseName, _config.NAVCompanyName);
                    if (SHIP != null)
                    {
                        result.ShippingName = SHIP.Name1;
                        result.ShippingName2 = SHIP.Name2;
                        result.ShippingAddress = SHIP.Address1;
                        result.ShippingAddress2 = SHIP.Address2;
                        result.ShippingZipCode = SHIP.ZipCode;
                        result.ShippingLocality = SHIP.City;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(result.ClientNo))
                    {
                        NAVClientsViewModel cli = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, result.ClientNo);
                        if (cli != null)
                        {
                            result.ShippingName = cli.Name;
                            result.ShippingName2 = "";
                            result.ShippingAddress = cli.Address1;
                            result.ShippingAddress2 = cli.Address2;
                            result.ShippingZipCode = cli.PostCode;
                            result.ShippingLocality = cli.City;
                        }
                    }
                    else
                    {
                        result.ShippingName = "";
                        result.ShippingName2 = "";
                        result.ShippingAddress = "";
                        result.ShippingAddress2 = "";
                        result.ShippingZipCode = "";
                        result.ShippingLocality = "";
                    }
                }

                result.Type = 2; //INTERNOS

                return Json(result);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult GetContractDetailsQuotas([FromBody] ContractViewModel data)
        {
            if (data != null)
            {
                Contratos cContract = null;
                ContractType? contractType = null;
                if (EnumHelper.ValidateRange(typeof(ContractType), data.ContractType))
                    contractType = (ContractType)data.ContractType;
                ;
                if (data.VersionNo != 0)
                {
                    cContract = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo, contractType);
                }
                else
                {
                    cContract = DBContracts.GetByIdLastVersion(data.ContractNo, contractType);
                }

                ContractViewModel result = new ContractViewModel();


                if (cContract != null)
                {
                    result = DBContracts.ParseToViewModel(cContract, _config.NAVDatabaseName, _config.NAVCompanyName);


                    //GET CLIENT REQUISITIONS
                    List<RequisiçõesClienteContrato> ClientRequisition = DBContractClientRequisition.GetByContract(result.ContractNo);
                    result.ClientRequisitions = new List<ContractClientRequisitionViewModel>();

                    if (ClientRequisition != null)
                    {
                        ClientRequisition.ForEach(x => result.ClientRequisitions.Add(DBContractClientRequisition.ParseToViewModel(x)));
                    }

                    //GET INVOICE TEXTS
                    List<TextoFaturaContrato> InvoicesTexts = DBContractInvoiceText.GetByContract(result.ContractNo);
                    result.InvoiceTexts = new List<ContractInvoiceTextViewModel>();

                    if (InvoicesTexts != null)
                    {
                        InvoicesTexts.ForEach(x => result.InvoiceTexts.Add(DBContractInvoiceText.ParseToViewModel(x)));
                    }
                }
                else
                {
                    if (data.ContractType == 1)
                        result.Status = 1;
                    result.ClientRequisitions = new List<ContractClientRequisitionViewModel>();
                    result.InvoiceTexts = new List<ContractInvoiceTextViewModel>();
                }

                result.Type = 3; //QUOTAS

                return Json(result);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult CreateContract([FromBody] ContractViewModel data)
        {
            try
            {
                if (data != null)
                {
                    //Get Contract Numeration
                    bool autoGenId = false;
                    Configuração Configs = DBConfigurations.GetById(1);
                    int ProjectNumerationConfigurationId = 0;

                    switch (data.ContractType)
                    {
                        case 1:
                            ProjectNumerationConfigurationId = Configs.NumeraçãoOportunidades.Value;
                            break;
                        case 2:
                            ProjectNumerationConfigurationId = Configs.NumeraçãoPropostas.Value;
                            break;
                        case 3:
                            switch (data.Type)
                            {
                                case 1:
                                    ProjectNumerationConfigurationId = Configs.NumeraçãoContratos.Value;
                                    break;
                                case 2:
                                    ProjectNumerationConfigurationId = Configs.NumeracaoContratosInternos.Value;
                                    break;
                                case 3:
                                    ProjectNumerationConfigurationId = Configs.NumeracaoContratosQuotas.Value;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    if (data.ContractNo == "" || data.ContractNo == null)
                    {
                        autoGenId = true;
                        data.ContractNo = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, autoGenId, false);
                    }

                    if (data.ContractNo != null)
                    {
                        data.Filed = false;
                        data.History = false;
                        Contratos cContract = DBContracts.ParseToDB(data);
                        cContract.TipoContrato = data.ContractType;
                        cContract.UtilizadorCriação = User.Identity.Name;
                        //Create Contract On Database
                        cContract = DBContracts.Create(cContract);

                        if (cContract == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro ao criar o contrato.";
                        }
                        else
                        {
                            //Create Client Contract Requisitions
                            data.ClientRequisitions.ForEach(r =>
                            {
                                r.ContractNo = cContract.NºDeContrato;
                                r.CreateUser = User.Identity.Name;
                                DBContractClientRequisition.Create(DBContractClientRequisition.ParseToDB(r));
                            });

                            //Create Contract Invoice Texts
                            data.InvoiceTexts.ForEach(r =>
                            {
                                r.ContractNo = cContract.NºDeContrato;
                                r.CreateUser = User.Identity.Name;
                                DBContractInvoiceText.Create(DBContractInvoiceText.ParseToDB(r));
                            });
                            if (autoGenId)
                            {
                                //Update Last Numeration Used
                                ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                                ConfigNumerations.ÚltimoNºUsado = data.ContractNo;
                                ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                                DBNumerationConfigurations.Update(ConfigNumerations);
                            }
                            data.eReasonCode = 1;
                        }
                    }
                    else
                    {
                        data.eReasonCode = 5;
                        data.eMessage = "A numeração configurada não é compativel com a inserida.";
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao criar o contrato";
            }
            return Json(data);

        }

        [HttpPost]
        public JsonResult UpdateContract([FromBody] ContractViewModel data)
        {
            try
            {
                if (data != null)
                {
                    data.eReasonCode = 1;
                    data.eMessage = "Contrato atualizado com sucesso.";

                    if (data.ContractNo != null)
                    {
                        //Contratos cContract = DBContracts.ParseToDB(data);
                        Contratos ContratoDB = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo);

                        if (ContratoDB != null)
                        {

                            if (data.ChangeStatus == 1 && ContratoDB.EstadoAlteração == 2) //1 = Aberto - 2 = Bloqueado
                            {
                                
                                data.SomatorioLinhas = (decimal)DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo).Sum(x => x.PreçoUnitário == null ? 0 : x.PreçoUnitário);
                            }

                            if (data.ChangeStatus == 2 && ContratoDB.EstadoAlteração == 1) //1 = Aberto - 2 = Bloqueado
                            {
                                if (data.CodeFunctionalArea != null && data.CodeFunctionalArea == "22") //22 = Gestão e Tratamento de Roupa Hospitalar
                                {
                                    decimal SomatorioLinhasOriginal = ContratoDB.SomatorioLinhas == null ? 0 : (decimal)ContratoDB.SomatorioLinhas;
                                    decimal SomatorioLinhasAtual = (decimal)DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo).Sum(x => x.PreçoUnitário == null ? 0 : x.PreçoUnitário);

                                    if (SomatorioLinhasOriginal != SomatorioLinhasAtual)
                                    {
                                        //ENVIAR EMAIL
                                        ConfiguracaoParametros EmailTo = DBConfiguracaoParametros.GetByParametro("ContratosRoupaEmailTo");
                                        ConfiguracaoParametros EmailCC1 = DBConfiguracaoParametros.GetByParametro("ContratosRoupaEmailCC1");
                                        ConfiguracaoParametros EmailCC2 = DBConfiguracaoParametros.GetByParametro("ContratosRoupaEmailCC2");
                                        ConfiguracaoParametros EmailCC3 = DBConfiguracaoParametros.GetByParametro("ContratosRoupaEmailCC3");

                                        SendEmailApprovals Email = new SendEmailApprovals();

                                        Email.Subject = "eSUCH - O valor do Contrato Nº " + data.ContractNo.ToString() + " foi atualizado.";
                                        Email.From = User.Identity.Name;
                                        if (EmailTo != null && !string.IsNullOrEmpty(EmailTo.Valor))
                                            Email.To.Add(EmailTo.Valor);
                                        if (EmailCC1 != null && !string.IsNullOrEmpty(EmailCC1.Valor))
                                            Email.CC.Add(EmailCC1.Valor);
                                        if (EmailCC2 != null && !string.IsNullOrEmpty(EmailCC2.Valor))
                                            Email.CC.Add(EmailCC2.Valor);
                                        if (EmailCC3 != null && !string.IsNullOrEmpty(EmailCC3.Valor))
                                            Email.CC.Add(EmailCC3.Valor);
                                        Email.Body = MakeEmailBodyContent("O valor do Contrato Nº " + data.ContractNo.ToString() + " foi atualizado.");
                                        Email.IsBodyHtml = true;

                                        Email.SendEmail_Simple();
                                    }
                                }
                            }

                            ContratoDB = DBContracts.ParseToDB(data);
                            ContratoDB.UtilizadorModificação = User.Identity.Name;
                            ContratoDB = DBContracts.Update(ContratoDB);

                            //Create/Update Contract Client Requests
                            List<RequisiçõesClienteContrato> RCC =
                                DBContractClientRequisition.GetByContract(ContratoDB.NºDeContrato);

                            List<RequisiçõesClienteContrato> RCCToDelete = RCC
                            .Where(x => !data.ClientRequisitions.Any(
                                y => x.NºContrato == y.ContractNo &&
                                        x.GrupoFatura == y.InvoiceGroup &&
                                        x.NºProjeto == y.ProjectNo &&
                                        x.DataInícioCompromisso == DateTime.Parse(y.StartDate))).ToList();

                            data.ClientRequisitions.ForEach(y =>
                            {
                                RequisiçõesClienteContrato RCCO =
                                    RCC.Where(x => x.NºContrato == y.ContractNo &&
                                                   x.GrupoFatura == y.InvoiceGroup &&
                                                   x.NºProjeto == y.ProjectNo &&
                                                   x.DataInícioCompromisso == DateTime.Parse(y.StartDate))
                                        .FirstOrDefault();
                                if (RCCO != null)
                                {
                                    if (DateTime.Parse(y.StartDate) <= DateTime.Parse(y.EndDate))
                                    {
                                        RCCO.NºContrato = y.ContractNo;
                                        RCCO.GrupoFatura = y.InvoiceGroup;
                                        RCCO.NºProjeto = y.ProjectNo;

                                        RCCO.DataInícioCompromisso = DateTime.Parse(y.StartDate);
                                        RCCO.DataFimCompromisso = y.EndDate != "" ? DateTime.Parse(y.EndDate) : (DateTime?)null;
                                        RCCO.NºRequisiçãoCliente = y.ClientRequisitionNo;
                                        RCCO.DataRequisição = y.RequisitionDate != "" ? DateTime.Parse(y.RequisitionDate) : (DateTime?)null;
                                        RCCO.NºCompromisso = y.PromiseNo;
                                        RCCO.DataÚltimaFatura = y.LastInvoiceDate != "" ? DateTime.Parse(y.LastInvoiceDate) : (DateTime?)null;
                                        RCCO.NºFatura = y.InvoiceNo;
                                        RCCO.ValorFatura = y.InvoiceValue;
                                        RCCO.UtilizadorModificação = User.Identity.Name;
                                        DBContractClientRequisition.Update(RCCO);
                                    }
                                    else
                                    {
                                        data.eReasonCode = 3;
                                        data.eMessage = "A data Inicio Compromisso não pode ser superior á data Fim Compromisso.";
                                    }
                                }
                                else
                                {
                                    if (DateTime.Parse(y.StartDate) <= DateTime.Parse(y.EndDate))
                                    {
                                        y.CreateUser = User.Identity.Name;
                                        DBContractClientRequisition.Create(DBContractClientRequisition.ParseToDB(y));
                                    }
                                    else
                                    {
                                        data.eReasonCode = 4;
                                        data.eMessage = "A data Inicio Compromisso não pode ser superior á data Fim Compromisso.";
                                    }
                                }
                            });

                            //Delete Contract Client Requests
                            if (data.eReasonCode == 1)
                                RCCToDelete.ForEach(x => DBContractClientRequisition.Delete(x));

                            //Create/Update Contract Invoice Texts
                            List<TextoFaturaContrato> CIT = DBContractInvoiceText.GetByContract(ContratoDB.NºDeContrato);
                            List<TextoFaturaContrato> CITToDelete =
                                CIT.Where(x => !data.InvoiceTexts.Any(
                                    y => x.GrupoFatura == y.InvoiceGroup && x.NºProjeto == y.ProjectNo &&
                                         x.NºContrato == y.ContractNo)).ToList();

                            data.InvoiceTexts.ForEach(y =>
                            {
                                TextoFaturaContrato CITO = CIT
                                    .Where(x => x.GrupoFatura == y.InvoiceGroup && x.NºProjeto == y.ProjectNo &&
                                                x.NºContrato == y.ContractNo).FirstOrDefault();
                                if (CITO != null)
                                {
                                    CITO.NºContrato = y.ContractNo;
                                    CITO.GrupoFatura = y.InvoiceGroup;
                                    CITO.NºProjeto = y.ProjectNo;
                                    CITO.TextoFatura = y.InvoiceText;
                                    CITO.UtilizadorModificação = User.Identity.Name;
                                    DBContractInvoiceText.Update(CITO);
                                }
                                else
                                {
                                    y.CreateUser = User.Identity.Name;
                                    DBContractInvoiceText.Create(DBContractInvoiceText.ParseToDB(y));
                                }
                            });

                            //Delete Contract Invoice Texts
                            CITToDelete.ForEach(x => DBContractInvoiceText.Delete(x));
                        }
                        //data.eReasonCode = 1;
                        //data.eMessage = "Contrato atualizado com sucesso.";
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao atualizar o contrato.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteContract([FromBody] ContractViewModel data)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (data != null)
                {
                    //Verify if contract have Invoices Or Projects
                    bool haveContracts = DBProjects.GetByContract(data.ContractNo).Count > 0;
                    bool haveInvoices = DBContractInvoices.GetByContractNo(data.ContractNo).Count > 0;

                    if (haveContracts || haveInvoices)
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não é possivel remover o contrato pois possui faturas e/ou projetos associados.";
                    }
                    else
                    {
                        // Delete Contract Lines
                        DBContractLines.DeleteAllFromContract(data.ContractNo);

                        // Delete Contract Invoice Texts
                        DBContractInvoiceText.DeleteAllFromContract(data.ContractNo);

                        // Delete Contract Client Requisitions
                        DBContractClientRequisition.DeleteAllFromContract(data.ContractNo);

                        // Delete Contract 
                        data.UpdateUser = User.Identity.Name;
                        DBContracts.Update(DBContracts.ParseToDB(data));
                        DBContracts.DeleteByContractNo(data.ContractNo);


                        result.eReasonCode = 1;
                        result.eMessage = "Contrato eliminado com sucesso.";
                    }

                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro ao eliminar o contrato.";
            }
            return Json(result);

        }

        [HttpPost]
        public JsonResult ArchiveContract([FromBody] ContractViewModel data)
        {

            if (data != null)
            {
                ContractsService serv = new ContractsService(User.Identity.Name);
                data = serv.ArchiveContract(data);
            }
            else
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao arquivar.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult UnArchiveContract([FromBody] ContractViewModel data)
        {
            data.eReasonCode = 2;
            data.eMessage = "Ocorreu um erro ao ativar.";

            if (data != null)
            {
                data.Filed = false;
                var updated = DBContracts.Update(DBContracts.ParseToDB(data));
                if (updated != null)
                {
                    data = DBContracts.ParseToViewModel(updated, _config.NAVDatabaseName, _config.NAVCompanyName);
                    data.eReasonCode = 1;
                    data.eMessage = "Ativado com sucesso.";
                }
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult SendContractToHistory([FromBody] ContractViewModel data)
        {

            if (data != null)
            {
                ContractsService serv = new ContractsService(User.Identity.Name);
                Contratos cContract = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo);

                if (cContract != null)
                {
                    try
                    {
                        //Create new contract and update old
                        cContract.UtilizadorModificação = User.Identity.Name;
                        cContract.Arquivado = true;
                        cContract.Historico = true;
                        DBContracts.Update(cContract);

                        data.eReasonCode = 1;
                        data.eMessage = "Contrato enviado para histórico com sucesso.";
                        return Json(data);
                    }
                    catch (Exception)
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "Ocorreu um erro ao enviar para histórico.";
                    }
                }
            }
            else
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao enviar para histórico.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateProposalContract([FromBody] JObject requestParams)
        {
            ContractViewModel proposal = new ContractViewModel();
            proposal.eReasonCode = 99;
            proposal.eMessage = "Ocorreu um erro ao atualizar a proposta.";

            try
            {
                bool partialUpdate = false;

                if (requestParams != null)
                {
                    proposal = requestParams["proposal"].ToObject<ContractViewModel>();
                    partialUpdate = requestParams["partialUpdate"].ToObject<bool>();
                }

                if (proposal != null)
                {
                    Contratos OldContract = DBContracts.GetByIdLastVersion(proposal.RelatedContract);

                    if (OldContract != null)
                    {
                        //1 » Passar o contrato atual para Histórico
                        OldContract.UtilizadorModificação = User.Identity.Name;
                        OldContract.Arquivado = true;
                        OldContract.Historico = true;
                        if (DBContracts.Update(OldContract) == null)
                        {
                            proposal.eReasonCode = 2;
                            proposal.eMessage = "Ocorreu um erro ao atualizar o contrato original.";
                            return Json(proposal);
                        }

                        //2 » Criar um novo contrato com os dados da proposta
                        Contratos NewContract = new Contratos
                        {
                            TipoContrato = 3,
                            NºDeContrato = OldContract.NºDeContrato,
                            NºVersão = OldContract.NºVersão + 1,

                            Área = proposal.Area,
                            Descrição = proposal.Description,
                            Estado = 4, //Assinado
                            EstadoAlteração = 1, //Aberto
                            NºCliente = proposal.ClientNo,
                            CódigoRegião = proposal.CodeRegion,
                            CódigoÁreaFuncional = proposal.CodeFunctionalArea,
                            CódigoCentroResponsabilidade = proposal.CodeResponsabilityCenter,
                            CódEndereçoEnvio = proposal.CodeShippingAddress,
                            EnvioANome = proposal.ShippingName,
                            EnvioAEndereço = proposal.ShippingAddress,
                            EnvioACódPostal = proposal.ShippingZipCode,
                            EnvioALocalidade = proposal.ShippingLocality,
                            PeríodoFatura = proposal.InvocePeriod,
                            ÚltimaDataFatura = string.IsNullOrEmpty(proposal.LastInvoiceDate) ? (DateTime?)null : DateTime.Parse(proposal.LastInvoiceDate),
                            PróximaDataFatura = string.IsNullOrEmpty(proposal.NextInvoiceDate) ? (DateTime?)null : DateTime.Parse(proposal.NextInvoiceDate),
                            DataInicial = string.IsNullOrEmpty(proposal.StartData) ? (DateTime?)null : DateTime.Parse(proposal.StartData),
                            DataExpiração = string.IsNullOrEmpty(proposal.DueDate) ? (DateTime?)null : DateTime.Parse(proposal.DueDate),
                            JuntarFaturas = proposal.BatchInvoices,
                            PróximoPeríodoFact = proposal.NextBillingPeriod,
                            LinhasContratoEmFact = proposal.ContractLinesInBilling,
                            CódTermosPagamento = proposal.CodePaymentTerms,
                            TipoProposta = proposal.ProposalType,
                            TipoFaturação = proposal.BillingType,
                            TipoContratoManut = proposal.MaintenanceContractType,
                            NºRequisiçãoDoCliente = proposal.ClientRequisitionNo,
                            DataReceçãoRequisição = string.IsNullOrEmpty(proposal.ReceiptDateRequisition) ? (DateTime?)null : DateTime.Parse(proposal.ReceiptDateRequisition),
                            NºCompromisso = proposal.PromiseNo,
                            TaxaAprovisionamento = proposal.ProvisioningFee,
                            Mc = proposal.Mc,
                            TaxaDeslocação = proposal.DisplacementFee,
                            ContratoAvençaFixa = proposal.FixedVowsAgreement,
                            ObjetoServiço = proposal.ServiceObject,
                            ContratoAvençaVariável = proposal.VariableAvengeAgrement,
                            Notas = proposal.Notes,
                            NºContrato = proposal.ContractNo,
                            DataInícioContrato = string.IsNullOrEmpty(proposal.ContractStartDate) ? (DateTime?)null : DateTime.Parse(proposal.ContractStartDate),
                            DataFimContrato = string.IsNullOrEmpty(proposal.ContractEndDate) ? (DateTime?)null : DateTime.Parse(proposal.ContractEndDate),
                            DescriçãoDuraçãoContrato = proposal.ContractDurationDescription,
                            DataInício1ºContrato = string.IsNullOrEmpty(proposal.StartDateFirstContract) ? (DateTime?)null : DateTime.Parse(proposal.StartDateFirstContract),
                            Referência1ºContrato = proposal.FirstContractReference,
                            DuraçãoMáxContrato = string.IsNullOrEmpty(proposal.ContractMaxDuration) ? (DateTime?)null : DateTime.Parse(proposal.ContractMaxDuration),
                            RescisãoPrazoAviso = proposal.TerminationTermNotice,
                            CondiçõesParaRenovação = proposal.RenovationConditions,
                            CondiçõesRenovaçãoOutra = proposal.RenovationConditionsAnother,
                            CondiçõesPagamento = proposal.PaymentTerms,
                            CondiçõesPagamentoOutra = proposal.PaymentTermsAnother,
                            AssinadoPeloCliente = proposal.CustomerSigned,
                            Juros = proposal.Interests,
                            DataDaAssinatura = string.IsNullOrEmpty(proposal.SignatureDate) ? (DateTime?)null : DateTime.Parse(proposal.SignatureDate),
                            DataEnvioCliente = string.IsNullOrEmpty(proposal.CustomerShipmentDate) ? (DateTime?)null : DateTime.Parse(proposal.CustomerShipmentDate),
                            UnidadePrestação = proposal.ProvisionUnit,
                            ReferênciaContrato = proposal.ContractReference,
                            ValorTotalProposta = proposal.TotalProposalValue,
                            LocalArquivoFísico = proposal.PhysicalFileLocation,
                            NºOportunidade = proposal.OportunityNo,
                            NºProposta = proposal.ContractNo,
                            NºContato = proposal.ContactNo,
                            DataEstadoProposta = string.IsNullOrEmpty(proposal.DateProposedState) ? (DateTime?)null : DateTime.Parse(proposal.DateProposedState),
                            OrigemDoPedido = proposal.OrderOrigin,
                            DescOrigemDoPedido = proposal.OrdOrderSource,
                            NumeraçãoInterna = proposal.InternalNumeration,
                            DataAlteraçãoProposta = string.IsNullOrEmpty(proposal.ProposalChangeDate) ? (DateTime?)null : DateTime.Parse(proposal.ProposalChangeDate),
                            DataHoraLimiteEsclarecimentos = string.IsNullOrEmpty(proposal.LimitClarificationDate) ? (DateTime?)null : DateTime.Parse(proposal.LimitClarificationDate),
                            DataHoraErrosEOmissões = string.IsNullOrEmpty(proposal.ErrorsOmissionsDate) ? (DateTime?)null : DateTime.Parse(proposal.ErrorsOmissionsDate),
                            DataHoraRelatórioFinal = string.IsNullOrEmpty(proposal.FinalReportDate) ? (DateTime?)null : DateTime.Parse(proposal.FinalReportDate),
                            DataHoraHabilitaçãoDocumental = string.IsNullOrEmpty(proposal.DocumentationHabilitationDate) ? (DateTime?)null : DateTime.Parse(proposal.DocumentationHabilitationDate),
                            DataHoraEntregaProposta = string.IsNullOrEmpty(proposal.ProposalDelivery) ? (DateTime?)null : DateTime.Parse(proposal.ProposalDelivery),
                            NºComprimissoObrigatório = proposal.CompulsoryCompulsoryNo,
                            DataHoraCriação = DateTime.Now,
                            DataHoraModificação = (DateTime?)null,
                            UtilizadorCriação = User.Identity.Name,
                            UtilizadorModificação = null,
                            Arquivado = false,
                            ValorBaseProcedimento = proposal.BaseValueProcedure,
                            AudiênciaPrévia = string.IsNullOrEmpty(proposal.PreviousHearing) ? (DateTime?)null : DateTime.Parse(proposal.PreviousHearing),
                            RazãoArquivo = proposal.ArchiveReason,
                            Historico = false,
                            Tipo = proposal.Type,
                            NºVep = proposal.NoVEP,
                            TextoFatura = proposal.TextoFatura
                        };
                        if (DBContracts.Create(NewContract) == null)
                        {
                            proposal.eReasonCode = 3;
                            proposal.eMessage = "Ocorreu um erro ao criar o novo contrato.";
                            return Json(proposal);
                        }

                        //Passar as Linhas da Proposta para o novo Contrato
                        List<LinhasContratos> proposalLines = DBContractLines.GetAllByActiveContract(proposal.ContractNo, proposal.VersionNo);
                        if (proposalLines != null && proposalLines.Count() > 0)
                        {
                            proposalLines.ForEach(x =>
                            {
                                if (partialUpdate == true)
                                {
                                    if (x.CriaContrato == true)
                                    {
                                        x.TipoContrato = 3;
                                        x.NºContrato = NewContract.NºDeContrato;
                                        x.NºVersão = NewContract.NºVersão;
                                        x.DataHoraCriação = DateTime.Now;
                                        x.GrupoFatura = x.GrupoFatura == null ? 0 : x.GrupoFatura;
                                        DBContractLines.Create(x);
                                    }
                                }
                                else
                                {
                                    x.TipoContrato = 3;
                                    x.NºContrato = NewContract.NºDeContrato;
                                    x.NºVersão = NewContract.NºVersão;
                                    x.DataHoraCriação = DateTime.Now;
                                    x.GrupoFatura = x.GrupoFatura == null ? 0 : x.GrupoFatura;
                                    DBContractLines.Create(x);
                                }
                            });
                        }

                        //3 » Passa a proposta para histórico
                        Contratos proposalToUpdate = DBContracts.GetByIdAndVersion(proposal.ContractNo, proposal.VersionNo);

                        if (proposalToUpdate != null)
                        {
                            proposalToUpdate.Estado = 6; //Renovada
                            proposalToUpdate.DataExpiração = DateTime.Parse(proposal.DueDate);
                            proposalToUpdate.UtilizadorModificação = User.Identity.Name;
                            proposalToUpdate.Arquivado = true;

                            if (DBContracts.Update(proposalToUpdate) == null)
                            {
                                proposal.eReasonCode = 4;
                                proposal.eMessage = "Ocorreu um erro ao actualizar a proposta original.";
                                return Json(proposal);
                            }
                        }
                        else
                        {
                            proposal.eReasonCode = 5;
                            proposal.eMessage = "Ocorreu um erro ao obter a proposta original.";
                            return Json(proposal);
                        }

                        proposal.eReasonCode = 1;
                        proposal.eMessage = "Proposta atualizada com sucesso.";
                        return Json(proposal);
                    }
                    else
                    {
                        proposal.eReasonCode = 6;
                        proposal.eMessage = "Não foi possível obter o contrato da proposta.";
                    }
                }
                else
                {
                    proposal.eReasonCode = 7;
                    proposal.eMessage = "Ocorreu um erro na passagem dos dados da proposta.";
                }
                return Json(proposal);
            }
            catch (Exception)
            {
                proposal.eReasonCode = 99;
                proposal.eMessage = "Ocorreu um erro ao atualizar a proposta.";
            }

            return Json(proposal);


            //CODIGO ORIGINAL DA HYDRA
            /*
            •	Se o estado da proposta estiver aberta, perguntar se quer mudar para Enviada ou para Revista (tem de mudar para um destes estados obrigatoriamente);
            •	Arquiva a proposta;
            •	Pergunta que linhas vai passar para o contrato, tal como faz no tornar em contrato parcial;
            •	Atualiza o contrato com os dados da proposta (as linhas do contrato atuais são eliminadas e criadas novamente com as linhas da proposta, passa apenas as linhas marcadas como sendo para passar);
            •	Muda o estado da proposta para Renovada se for total, ou para Parcialmente Aceite se for parcial (se selecionou todas as linhas ou não);
            */
            //Contratos proposalToUpdate = DBContracts.GetByIdAndVersion(proposal.ContractNo, proposal.VersionNo);
            //proposalToUpdate.Estado = proposal.Status;
            //proposalToUpdate.DataExpiração = DateTime.Parse(proposal.DueDate);
            //proposalToUpdate.UtilizadorModificação = User.Identity.Name;
            //proposalToUpdate.Arquivado = true;
            //proposalToUpdate = DBContracts.Update(proposalToUpdate);
            //if (proposalToUpdate != null)
            //{
            //    proposalToUpdate.NºVersão = proposal.VersionNo + 1;
            //    proposalToUpdate.UtilizadorCriação = User.Identity.Name;
            //    proposalToUpdate.UtilizadorModificação = "";

            //    proposalToUpdate.DataHoraModificação = null;
            //    proposalToUpdate.Arquivado = false;
            //    DBContracts.Create(proposalToUpdate);

            //    //Duplicate proposal Lines
            //    List<LinhasContratos> proposalLines = DBContractLines.GetAllByActiveContract(proposal.ContractNo, proposal.VersionNo);
            //    proposalLines.ForEach(x =>
            //    {
            //        x.NºVersão = proposalToUpdate.NºVersão;
            //        DBContractLines.Create(x);
            //    });

            //    if (DBContractLines.DeleteAllFromContract(proposal.RelatedContract))
            //    {
            //        if (partialUpdate)
            //        {
            //            proposalLines.RemoveAll(x => !x.CriaContrato.HasValue || !x.CriaContrato.Value);
            //            proposalToUpdate.Estado = 8;
            //        }
            //        else
            //        {
            //            proposalToUpdate.Estado = 6;
            //        }
            //        DBContracts.Update(proposalToUpdate);
            //        proposalLines.ForEach(x =>
            //        {
            //            LinhasContratos newline = ParseToNewModel(x);
            //            newline.TipoContrato = contract.TipoContrato;
            //            newline.NºContrato = contract.NºContrato;
            //            newline.NºVersão = contract.NºVersão;
            //            DBContractLines.Create(newline);
            //        });

            //        proposal = DBContracts.ParseToViewModel(proposalToUpdate, _config.NAVDatabaseName, _config.NAVCompanyName);

            //        proposal.eReasonCode = 1;
            //        proposal.eMessage = "Contrato atualizado com sucesso.";
            //        return Json(proposal);
            //    }
            //    else
            //    {
            //        proposal.eReasonCode = 2;
            //        proposal.eMessage = "Ocorreu um erro ao atualizar as linhas do contrato.";
            //    }
            //}
            //else
            //{
            //    proposal.eReasonCode = 2;
            //    proposal.eMessage = "Ocorreu um erro ao atualizar o contrato.";
            //}

            //return Json(proposal);
        }

        [HttpPost]
        public JsonResult GetContractNavData([FromBody] ContractViewModel data)
        {
            if (data != null)
            {
                NAVContractDetailsViewModel result = DBNAV2017ContractDetails.GetContractByNo(data.ContractNo, _config.NAVDatabaseName, _config.NAVCompanyName);

                decimal ValorPeriodo = 0;
                decimal SomatorioLinhas = 0;
                decimal tmpQuantidade = 0;
                decimal tmpPrecoUnitario = 0;

                List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo);
                if (ContractLines != null && ContractLines.Count > 0)
                {
                    ContractLines.ForEach(linha =>
                    {
                        tmpQuantidade = linha.Quantidade.HasValue ? (decimal)linha.Quantidade : 0;
                        tmpPrecoUnitario = linha.PreçoUnitário.HasValue ? (decimal)linha.PreçoUnitário : 0;

                        ValorPeriodo = ValorPeriodo + (tmpQuantidade * tmpPrecoUnitario);
                        SomatorioLinhas = SomatorioLinhas + (tmpQuantidade * tmpPrecoUnitario);
                    });
                }

                result.SomatorioLinhas = SomatorioLinhas;

                if (data.InvocePeriod == 1)
                    result.VPeriodFatura = ValorPeriodo * 1;
                if (data.InvocePeriod == 2)
                    result.VPeriodFatura = ValorPeriodo * 2;
                if (data.InvocePeriod == 3)
                    result.VPeriodFatura = ValorPeriodo * 3;
                if (data.InvocePeriod == 4)
                    result.VPeriodFatura = ValorPeriodo * 6;
                if (data.InvocePeriod == 5)
                    result.VPeriodFatura = ValorPeriodo * 12;
                if (data.InvocePeriod == 6)
                    result.VPeriodFatura = 0;

                return Json(result);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult GetContractLines([FromBody] ContractViewModel data)
        {
            if (data != null)
            {
                List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo);

                ContractLineHelperViewModel result = new ContractLineHelperViewModel
                {
                    ContractNo = data.ContractNo,
                    VersionNo = data.VersionNo,
                    Lines = new List<ContractLineViewModel>()
                };

                if (ContractLines != null)
                {
                    ContractLines.ForEach(x => result.Lines.Add(DBContractLines.ParseToViewModel(x)));
                }
                return Json(result);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult UpdateContractLines([FromBody] ContractLineHelperViewModel data)
        {
            try
            {
                if (data != null)
                {
                    List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo);

                    List<LinhasContratos> CLToDelete = ContractLines.Where(y => !data.Lines.Any(x => x.ContractType == y.TipoContrato && x.ContractNo == y.NºContrato && x.VersionNo == y.NºVersão && x.LineNo == y.NºLinha)).ToList();
                    CLToDelete.ForEach(x =>
                    {
                        x.UtilizadorModificação = User.Identity.Name;
                        DBContractLines.Update(x);
                    });
                    CLToDelete.ForEach(x => DBContractLines.Delete(x));

                    data.Lines.ForEach(x =>
                    {
                        LinhasContratos CLine = ContractLines.Where(y => x.ContractType == y.TipoContrato && x.ContractNo == y.NºContrato && x.VersionNo == y.NºVersão && x.LineNo == y.NºLinha).FirstOrDefault();

                        if (CLine != null)
                        {
                            CLine.TipoContrato = x.ContractType;
                            CLine.NºContrato = x.ContractNo;
                            CLine.NºVersão = x.VersionNo;
                            CLine.NºLinha = x.LineNo;
                            CLine.Tipo = x.Type;
                            CLine.Código = x.Code;
                            CLine.Descrição = x.Description;
                            CLine.Descricao2 = x.Description2;
                            CLine.Quantidade = x.Quantity;
                            CLine.CódUnidadeMedida = x.CodeMeasureUnit;
                            CLine.PreçoUnitário = x.UnitPrice;
                            CLine.DescontoLinha = x.LineDiscount;
                            CLine.Faturável = x.Billable;
                            CLine.CódigoRegião = x.CodeRegion;
                            CLine.CódigoÁreaFuncional = x.CodeFunctionalArea;
                            CLine.CódigoCentroResponsabilidade = x.CodeResponsabilityCenter;
                            CLine.Periodicidade = x.Frequency;
                            CLine.NºHorasIntervenção = x.InterventionHours;
                            CLine.NºTécnicos = x.TotalTechinicians;
                            CLine.TipoProposta = x.ProposalType;
                            CLine.DataInícioVersão = x.VersionStartDate != null && x.VersionStartDate != "" ? DateTime.Parse(x.VersionStartDate) : (DateTime?)null;
                            CLine.DataFimVersão = x.VersionEndDate != null && x.VersionStartDate != "" ? DateTime.Parse(x.VersionEndDate) : (DateTime?)null;
                            CLine.NºResponsável = x.ResponsibleNo;
                            CLine.CódServiçoCliente = x.ServiceClientNo;
                            CLine.GrupoFatura = x.InvoiceGroup != null ? x.InvoiceGroup : 0;
                            CLine.CriaContrato = x.CreateContract;
                            CLine.NºProjeto = x.ProjectNo;
                            CLine.UtilizadorModificação = User.Identity.Name;
                            DBContractLines.Update(CLine);
                        }
                        else
                        {
                            x.CreateUser = User.Identity.Name;
                            x = DBContractLines.ParseToViewModel(DBContractLines.Create(DBContractLines.ParseToDB(x)));
                        }
                    });

                    data.eReasonCode = 1;
                    data.eMessage = "Linhas de contrato atualizadas com sucesso.";
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao atualizar as linhas de contrato.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult DuplicarContractLines([FromBody] ContractLineViewModel linha)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 0;
            result.eMessage = "Ocorreu um erro ao duplicar a linha.";

            try
            {
                if (linha != null && linha.ContractType > 0 && !string.IsNullOrEmpty(linha.ContractNo) && linha.VersionNo > 0 && linha.LineNo > 0)
                {
                    LinhasContratos LinhaOriginal = DBContractLines.GetById(linha.ContractType, linha.ContractNo, linha.VersionNo, linha.LineNo);
                    LinhasContratos LinhaDuplicada = new LinhasContratos();

                    LinhaDuplicada = LinhaOriginal;
                    LinhaDuplicada.NºLinha = 0;
                    LinhaDuplicada.UtilizadorCriação = User.Identity.Name;
                    LinhaDuplicada.DataHoraCriação = DateTime.Now;
                    LinhaDuplicada.UtilizadorModificação = null;
                    LinhaDuplicada.DataHoraModificação = null;

                    if (DBContractLines.Create(LinhaDuplicada) != null)
                    {
                        result.eReasonCode = 1;
                        result.eMessage = "A duplicação da Linha com sucesso.";
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Ocorreu um erro ao criar a linha duplicada.";
                    }
                }
                else
                {
                    result.eReasonCode = 3;
                    result.eMessage = "Falta informação para duplicar a linha.";
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";

                return Json(result);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateContractPrices([FromBody] UpdateContractPricesRequest updatePricesRequest)
        {
            ContractViewModel updatedContract;
            try
            {
                ContractsService serv = new ContractsService(User.Identity.Name);
                updatedContract = serv.UpdatePrices(updatePricesRequest);
            }
            catch
            {
                updatedContract = new ContractViewModel
                {
                    eReasonCode = 2,
                    eMessage = "Ocorreu um erro ao criar a proposta"
                };
            }
            return Json(updatedContract);
        }

        [HttpPost]
        public JsonResult RemoveFromHistoric([FromBody] ContractViewModel data)
        {
            try
            {
                if (data != null)
                {
                    if (data.ContractNo != null)
                    {
                        //Contratos cContract = DBContracts.ParseToDB(data);
                        Contratos ContratoDB = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo);


                        if (ContratoDB != null)
                        {
                            ContratoDB.Historico = false;
                            ContratoDB.Arquivado = false;
                            ContratoDB = DBContracts.Update(ContratoDB);
                        }
                        data.eReasonCode = 1;
                        data.eMessage = "Contrato atualizado com sucesso.";
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao atualizar o contrato.";
            }
            return Json(data);
        }

        public IActionResult CreateProjectContract(string id, string versionNo = "")
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
            if (UPerm != null && UPerm.Read.Value)
            {
                Contratos ContratoDB = DBContracts.GetByIdAndVersion(id, int.Parse(versionNo));

                ViewBag.UPermissions = UPerm;
                ViewBag.CodeRegion = ContratoDB.CódigoRegião;
                ViewBag.FuncArea = ContratoDB.CódigoÁreaFuncional;
                ViewBag.RespCode = ContratoDB.CódigoCentroResponsabilidade;
                ViewBag.CodClient = ContratoDB.NºCliente;
                ViewBag.ContractNo = id;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult DeleteClientRequisition([FromBody] ContractClientRequisitionViewModel requisition)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (requisition != null)
                {
                    requisition.UpdateUser = User.Identity.Name;
                    DBContractClientRequisition.Update(DBContractClientRequisition.ParseToDB(requisition));
                    DBContractClientRequisition.Delete(DBContractClientRequisition.ParseToDB(requisition));

                    requisition.eReasonCode = 1;
                    requisition.eMessage = "Requisição eliminada com sucesso.";
                }
                else
                {
                    requisition = new ContractClientRequisitionViewModel();
                    requisition.eReasonCode = 2;
                    requisition.eMessage = "Não foi possivel obter a requisição.";
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro ao eliminar a requisição.";
            }
            return Json(requisition);

        }
        #endregion

        #region Oportunidades

        public IActionResult Oportunidades(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Oportunidades);

            if (UPerm != null && UPerm.Read.Value)
            {

                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesOportunidade(string id, string version = "", string isHistoric = "")
        {
            string hist = isHistoric;

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Oportunidades);
            if (UPerm != null && UPerm.Read.Value)
            {
                Contratos cContract = null;
                if (version != "")
                    cContract = DBContracts.GetByIdAndVersion(id, int.Parse(version));
                else
                    cContract = DBContracts.GetByIdLastVersion(id);

                //if (cContract != null && cContract.Arquivado == true)
                //{
                //UPerm.Update = false;
                //UPerm.Delete = false;
                //}
                if (hist == "true")
                {
                    ViewBag.Historic = "(Histórico) ";
                }
                else
                {
                    ViewBag.Historic = "";
                }
                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.UPermissions = UPerm;
                ViewBag.reportServerURL = _config.ReportServerURL;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetListOportunities([FromBody] JObject requestParams)
        {
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();

            List<Contratos> ContractsList = null;

            if (Archived == 0)
            {
                ContractsList = DBContracts.GetAllByContractType(ContractType.Oportunity);
                ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
            }
            else
            {
                ContractsList = DBContracts.GetAllByContractType(ContractType.Oportunity);
                ContractsList.RemoveAll(x => x.Arquivado.HasValue && !x.Arquivado.Value);

                ContractsList.RemoveAll(x => (x.Arquivado.HasValue && !x.Arquivado.Value) || (!x.Arquivado.HasValue));

            }

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);

            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));

            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional));

            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));

            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x)));

            List<EnumData> status = EnumerablesFixed.ProposalsStatus;
            List<EnumData> origemPedido = EnumerablesFixed.RequestOrigin;
            List<Contactos> AllContacts = DBContacts.GetAll();
            List<UnidadePrestação> ALLFetcUnit = DBFetcUnit.GetAll();
            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");

            result.ForEach(x =>
            {
                x.ClientName = !string.IsNullOrEmpty(x.ClientNo) ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault().Name : "" : "";
                x.StatusDescription = x.Status != null ? status.Where(y => y.Id == x.Status).Select(y => y.Value).FirstOrDefault() : "";
                x.OrderOriginText = x.OrderOrigin != null ? origemPedido.Where(y => y.Id == x.OrderOrigin).Select(y => y.Value).FirstOrDefault() : "";
                x.ProvisionUnitText = x.ProvisionUnit != null ? ALLFetcUnit.Where(y => y.Código == x.ProvisionUnit).FirstOrDefault() != null ? ALLFetcUnit.Where(y => y.Código == x.ProvisionUnit).FirstOrDefault().Descrição : "" : "";
                //x.ContactNoText = DBNAV2017Contacts.GetContactsByType(_config.NAVDatabaseName, _config.NAVCompanyName, x.ContactNo, 0).FirstOrDefault() != null ? DBNAV2017Contacts.GetContactsByType(_config.NAVDatabaseName, _config.NAVCompanyName, x.ContactNo, 0).FirstOrDefault().Name : "";
                x.ContactNoText = !string.IsNullOrEmpty(x.ContactNo) ? AllContacts.Where(y => y.No == x.ContactNo).FirstOrDefault() != null ? AllContacts.Where(y => y.No == x.ContactNo).FirstOrDefault().Nome : "" : "";
            });

            return Json(result);
        }

        public JsonResult GetListOportunitiesByCliente([FromBody] JObject requestParams)
        {
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();
            string ClienteNo = requestParams["ClienteNo"].ToString();

            List<Contratos> ContractsList = null;

            if (Archived == 0)
            {
                ContractsList = DBContracts.GetAllByContractType(ContractType.Oportunity);
                ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
            }
            else
            {
                ContractsList = DBContracts.GetAllByContractType(ContractType.Oportunity);
                ContractsList.RemoveAll(x => x.Arquivado.HasValue && !x.Arquivado.Value);

                ContractsList.RemoveAll(x => (x.Arquivado.HasValue && !x.Arquivado.Value) || (!x.Arquivado.HasValue));

            }

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);

            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));

            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional));

            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));

            //Cliente
            if (!string.IsNullOrEmpty(ClienteNo))
                ContractsList.RemoveAll(x => x.NºCliente != ClienteNo);

            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x)));

            List<EnumData> status = EnumerablesFixed.ProposalsStatus;
            List<EnumData> origemPedido = EnumerablesFixed.RequestOrigin;
            List<Contactos> AllContacts = DBContacts.GetAll();
            List<UnidadePrestação> ALLFetcUnit = DBFetcUnit.GetAll();
            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");

            result.ForEach(x =>
            {
                x.ClientName = !string.IsNullOrEmpty(x.ClientNo) ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault().Name : "" : "";
                x.StatusDescription = x.Status != null ? status.Where(y => y.Id == x.Status).Select(y => y.Value).FirstOrDefault() : "";
                x.OrderOriginText = x.OrderOrigin != null ? origemPedido.Where(y => y.Id == x.OrderOrigin).Select(y => y.Value).FirstOrDefault() : "";
                x.ProvisionUnitText = x.ProvisionUnit != null ? ALLFetcUnit.Where(y => y.Código == x.ProvisionUnit).FirstOrDefault() != null ? ALLFetcUnit.Where(y => y.Código == x.ProvisionUnit).FirstOrDefault().Descrição : "" : "";
                //x.ContactNoText = DBNAV2017Contacts.GetContactsByType(_config.NAVDatabaseName, _config.NAVCompanyName, x.ContactNo, 0).FirstOrDefault() != null ? DBNAV2017Contacts.GetContactsByType(_config.NAVDatabaseName, _config.NAVCompanyName, x.ContactNo, 0).FirstOrDefault().Name : "";
                x.ContactNoText = !string.IsNullOrEmpty(x.ContactNo) ? AllContacts.Where(y => y.No == x.ContactNo).FirstOrDefault() != null ? AllContacts.Where(y => y.No == x.ContactNo).FirstOrDefault().Nome : "" : "";
            });

            return Json(result);
        }

        #endregion

        #region Avenca Fixa

        public JsonResult GetAllAvencaFixa()
        {
            List<AutorizarFaturaçãoContratos> ALLcontractList = DBContractInvoices.GetAll();
            foreach (AutorizarFaturaçãoContratos fc in ALLcontractList)
            {
                List<LinhasFaturaçãoContrato> contractInvoiceLines = DBInvoiceContractLines.GetById(fc.NºContrato);
                decimal sum = (decimal)contractInvoiceLines.Where(x => x.GrupoFatura == fc.GrupoFatura).Sum(x => x.ValorVenda).Value;

                if (sum <= 0 && !fc.Situação.Contains("Valor da Fatura está a 0!"))
                {
                    fc.Situação = fc.Situação + "Valor da Fatura está a 0!";
                    fc.UtilizadorModificação = User.Identity.Name;
                    fc.DataHoraModificação = DateTime.Now;
                    DBAuthorizeInvoiceContracts.Update(fc);
                }
            }
            List<AutorizarFaturaçãoContratos> contractList = DBContractInvoices.GetAllInvoice();
            List<FaturacaoContratosViewModel> result = new List<FaturacaoContratosViewModel>();
            foreach (var item in contractList)
            {
                string StartDate = "";
                string ExpiryDate = "";
                int? InvoicePeriod = null;
                Contratos AFixaContract = DBContracts.GetByIdAvencaFixa(item.NºContrato);
                if (AFixaContract != null)
                {
                    StartDate = AFixaContract.DataInicial.HasValue ? AFixaContract.DataInicial.Value.ToString("yyyy-MM-dd") : "";
                    ExpiryDate = AFixaContract.DataExpiração.HasValue ? AFixaContract.DataExpiração.Value.ToString("yyyy-MM-dd") : "";
                    InvoicePeriod = AFixaContract.PeríodoFatura;
                }
                //Client Name -> NAV
                String cliName = DBNAV2017Clients.GetClientNameByNo(item.NºCliente, _config.NAVDatabaseName, _config.NAVCompanyName);
                string DocNo_ = "";
                // Valor Fatura

                List<LinhasFaturaçãoContrato> contractInvoiceLines = DBInvoiceContractLines.GetById(item.NºContrato);
                Decimal sum = 0;
                Decimal Count = 0;
                if (contractInvoiceLines != null && contractInvoiceLines.Count > 0)
                {
                    sum = contractInvoiceLines.Where(x => x.GrupoFatura == item.GrupoFatura).Sum(x => x.ValorVenda).Value;
                    Count = contractInvoiceLines.Where(x => x.GrupoFatura == item.GrupoFatura).Count();
                }

                result.Add(new FaturacaoContratosViewModel
                {
                    Document_No = DocNo_,
                    ContractNo = item.NºContrato,
                    Description = item.Descrição,
                    ClientNo = item.NºCliente,
                    ClientName = cliName,
                    InvoiceValue = Math.Round(sum, 2),
                    NumberOfInvoices = item.NºDeFaturasAEmitir,
                    InvoiceTotal = Math.Round(((decimal)item.NºDeFaturasAEmitir * sum), 2),
                    ContractValue = item.ValorDoContrato,
                    ValueToInvoice = item.ValorPorFaturar,
                    BilledValue = item.ValorFaturado,
                    RegionCode = item.CódigoRegião,
                    InvoiceGroupCount = Count,
                    InvoiceGroupValue = item.GrupoFatura,
                    FunctionalAreaCode = item.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = item.CódigoCentroResponsabilidade,
                    RegisterDate = item.DataPróximaFatura.HasValue ? item.DataPróximaFatura.Value.ToString("yyyy-MM-dd") : "",
                    StartDate = StartDate,
                    ExpiryDate = ExpiryDate,
                    InvoicePeriod = InvoicePeriod
                });
            }

            return Json(result);
        }

        public JsonResult GetPedingAvencaFixa()
        {
            //find Invoice Value = 0
            List<AutorizarFaturaçãoContratos> ALLcontractList = DBContractInvoices.GetAll();
            foreach (AutorizarFaturaçãoContratos fc in ALLcontractList)
            {
                List<LinhasFaturaçãoContrato> contractInvoiceLines = DBInvoiceContractLines.GetById(fc.NºContrato);
                Decimal sum = contractInvoiceLines.Where(x => x.GrupoFatura == fc.GrupoFatura).Sum(x => x.ValorVenda).Value;
                if (sum <= 0 && !fc.Situação.Contains("Valor da Fatura está a 0!"))
                {
                    fc.Situação = fc.Situação + "Valor da Fatura está a 0!";
                    fc.UtilizadorModificação = User.Identity.Name;
                    fc.DataHoraModificação = DateTime.Now;
                    DBAuthorizeInvoiceContracts.Update(fc);
                }
            }

            List<AutorizarFaturaçãoContratos> contractList = DBAuthorizeInvoiceContracts.GetPedding();
            List<FaturacaoContratosViewModel> result = new List<FaturacaoContratosViewModel>();

            foreach (var item in contractList)
            {
                string StartDate = "";
                string ExpiryDate = "";
                int? InvoicePeriod = null;
                Contratos AFixaContract = DBContracts.GetByIdAvencaFixa(item.NºContrato);
                if (AFixaContract != null)
                {
                    StartDate = AFixaContract.DataInicial.HasValue ? AFixaContract.DataInicial.Value.ToString("yyyy-MM-dd") : "";
                    ExpiryDate = AFixaContract.DataExpiração.HasValue ? AFixaContract.DataExpiração.Value.ToString("yyyy-MM-dd") : "";
                    InvoicePeriod = AFixaContract.PeríodoFatura;
                }
                //Estado Pendente
                string DocNo_ = "";
                string cliName = DBNAV2017Clients.GetClientNameByNo(item.NºCliente, _config.NAVDatabaseName, _config.NAVCompanyName);
                //Pre registadas

                //Alteração pedida pelo Marco Marcelo no dia 27-05-2021
                //List<NAVSalesLinesViewModel> SLines = DBNAV2017SalesLine.FindSalesLine(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºContrato, item.NºCliente);
                //if (SLines.Count > 0)
                //{
                //    DocNo_ = SLines.LastOrDefault().DocNo;
                //}
                NAVSalesHeaderViewModel SLines = DBNAV2017SalesHeader.GetSalesHeader(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºContrato, NAVBaseDocumentTypes.Fatura);
                if (SLines != null)
                {
                    DocNo_ = SLines.No;
                }

                // Valor Fatura
                List<LinhasFaturaçãoContrato> contractInvoiceLines = DBInvoiceContractLines.GetById(item.NºContrato);
                Decimal sum = contractInvoiceLines.Where(x => x.GrupoFatura == item.GrupoFatura).Sum(x => x.ValorVenda).Value;
                Decimal Count = contractInvoiceLines.Where(x => x.GrupoFatura == item.GrupoFatura).Count();
                result.Add(new FaturacaoContratosViewModel
                {
                    Document_No = DocNo_,
                    ContractNo = item.NºContrato,
                    Description = item.Descrição,
                    ClientNo = item.NºCliente,
                    ClientName = cliName,
                    InvoiceValue = Math.Round(sum, 2),
                    NumberOfInvoices = item.NºDeFaturasAEmitir,
                    InvoiceTotal = Math.Round(((decimal)item.NºDeFaturasAEmitir * sum), 2),
                    ContractValue = item.ValorDoContrato,
                    ValueToInvoice = item.ValorPorFaturar,
                    BilledValue = item.ValorFaturado,
                    RegionCode = item.CódigoRegião,
                    Situation = item.Situação,
                    InvoiceGroupCount = Count,
                    InvoiceGroupValue = item.GrupoFatura,
                    FunctionalAreaCode = item.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = item.CódigoCentroResponsabilidade,
                    RegisterDate = item.DataPróximaFatura.HasValue ? item.DataPróximaFatura.Value.ToString("yyyy-MM-dd") : "",
                    StartDate = StartDate,
                    ExpiryDate = ExpiryDate,
                    InvoicePeriod = InvoicePeriod
                });

            }
            return Json(result);
        }

        public JsonResult GenerateInvoice([FromBody] List<FaturacaoContratosViewModel> data, string dateCont)
        {
            //AMARO COMENTAR
            //string teste = "";

            // Delete All lines From "Autorizar Faturação Contratos" & "Linhas Faturação Contrato"
            DBAuthorizeInvoiceContracts.DeleteAllAllowedInvoiceAndLines();

            List<Contratos> contractList = DBContracts.GetAllAvencaFixa2();
            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");

            //AMARO COMENTAR
            //contractList.RemoveAll(x => x.NºDeContrato != "VC210074");

            foreach (var item in contractList)
            {
                //AMARO COMENTAR
                //if (item.NºDeContrato == "VC200132")
                //    teste = "";

                if (!string.IsNullOrEmpty(item.NºCliente))
                {
                    NAVClientsViewModel Client = AllClients.Where(x => x.No_ == item.NºCliente).FirstOrDefault();
                    if (Client != null)
                    {
                        if (Client.AbrigoLeiCompromisso == true)
                            item.NºComprimissoObrigatório = false;
                        else
                            item.NºComprimissoObrigatório = true;

                        DBContracts.Update(item);
                    }

                }

                List<NAVSalesLinesViewModel> contractSalesLinesInNAV = DBNAV2017SalesLine.FindSalesLine(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºDeContrato, item.NºCliente);
                List<LinhasContratos> contractLines = DBContractLines.GetAllByNoTypeVersion(item.NºDeContrato, item.TipoContrato, item.NºVersão, true);
                contractLines.OrderBy(x => x.NºContrato).ThenBy(y => y.GrupoFatura);

                String ContractNoDuplicate = "";
                int InvoiceGroupDuplicate = -1;
                DateTime current = Convert.ToDateTime(dateCont);
                DateTime lastDay = Convert.ToDateTime(dateCont);
                string Problema = "";
                Decimal lineQuantity = 1;
                int? totalInvoiceGroups = contractLines.Where(x => x.NºContrato == item.NºDeContrato).Select(x => x.GrupoFatura).Distinct().Count();

                if (contractLines != null && contractLines.Count > 0)
                {
                    foreach (var contractLine in contractLines)
                    {
                        //AMARO COMENTAR
                        //if (item.NºDeContrato == "VC200132")
                        //{
                        //    teste = "";
                        //}

                        int? totalLinesForCurrentInvoiceGroup = contractLines.Where(x => x.NºContrato == contractLine.NºContrato && x.GrupoFatura == contractLine.GrupoFatura).Count();
                        if (ContractNoDuplicate != contractLine.NºContrato || InvoiceGroupDuplicate != contractLine.GrupoFatura)
                        {
                            ContractNoDuplicate = contractLine.NºContrato;
                            InvoiceGroupDuplicate = contractLine.GrupoFatura == null ? 0 : contractLine.GrupoFatura.Value;
                            Decimal contractVal = 0;
                            if (item.TipoContrato != 1 || item.TipoContrato != 4)
                            {
                                int NumMeses = 0;

                                if (item.DataExpiração != null && item.DataExpiração.Value != null && item.DataExpiração.ToString() != "" && item.DataInicial != null && item.DataInicial.Value != null && item.DataInicial.ToString() != "")
                                {
                                    NumMeses = ((item.DataExpiração.Value.Year - item.DataInicial.Value.Year) * 12) + item.DataExpiração.Value.Month - item.DataInicial.Value.Month;
                                }
                                if (NumMeses == 0)
                                {
                                    NumMeses = 1;
                                }
                                decimal SumPrice = 0;
                                foreach (LinhasContratos itm in contractLines)
                                {
                                    if (itm.PreçoUnitário != null)
                                    {
                                        if (SumPrice != 0)
                                        {
                                            SumPrice = SumPrice + itm.PreçoUnitário.Value;
                                        }
                                        else
                                        {
                                            SumPrice = itm.PreçoUnitário.Value;
                                        }

                                    }
                                }
                                contractVal = Math.Round(NumMeses * SumPrice, 2);
                            }

                            List<NAVSalesInvoiceLinesViewModel> salesList = null;
                            List<NAVSalesCrMemoLinesViewModel> crMemo = null;
                            if (item.DataInicial != null && item.DataExpiração != null)
                            {
                                salesList = DBNAV2017SalesInvoiceLine.GetSalesInvoiceLines(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºDeContrato, item.DataInicial.Value, item.DataExpiração.Value);
                                crMemo = DBNAV2017SalesCrMemo.GetSalesCrMemoLines(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºDeContrato, item.DataInicial.Value, item.DataExpiração.Value);
                            }

                            DateTime nextInvoiceDate = DateTime.MinValue;// lastDay;
                            DateTime? lastInvoiceDate = null;
                            int invoiceNumber = 0;
                            current = Convert.ToDateTime(dateCont);

                            if (item.DataExpiração != null && current >= item.DataExpiração)
                            {
                                current = item.DataExpiração.Value;
                            }

                            //Se a data do Fim de Compromisso nas Requisições de Cliente for inferior à data da Geração faturação fica esta como data limite para a Faturação
                            RequisiçõesClienteContrato ReqClienteAux = DBContractClientRequisition.GetByContract(ContractNoDuplicate).
                                OrderByDescending(x => x.DataFimCompromisso).ToList().
                                Find(x => x.GrupoFatura == InvoiceGroupDuplicate && x.DataInícioCompromisso <= current);
                            if (ReqClienteAux != null && ReqClienteAux.DataFimCompromisso.HasValue && current > ReqClienteAux.DataFimCompromisso)
                                current = (DateTime)ReqClienteAux.DataFimCompromisso;

                            #region Obter Data ultima fatura
                            //Contratos com 1 única fatura - Tentar obter a data da ultima fatura a partir da Data da Ultima fatura do contrato;
                            //if (totalInvoiceGroups.HasValue && totalInvoiceGroups.Value == 1)
                            //{
                            //    if (item.ÚltimaDataFatura.HasValue)
                            //        nextInvoiceDate = item.ÚltimaDataFatura.Value;
                            //}
                            //else
                            //{
                            //    //Contratos com n Faturas - Tentar obter a data da ultima fatura a partir das requisições de cliente;
                            //    if (totalLinesForCurrentInvoiceGroup.HasValue && totalLinesForCurrentInvoiceGroup.Value > 0)
                            //    {
                            //        lastInvoiceDate = DBContractClientRequisition.GetLatsInvoiceDateFor(item.NºDeContrato, contractLine.GrupoFatura);
                            //        if (lastInvoiceDate.HasValue)
                            //        {
                            //            nextInvoiceDate = lastInvoiceDate.Value;
                            //        }
                            //    }
                            //}

                            //Independentemente do nº de faturas a emitir, procurar sempre a data da última fatura nas Requisições de Cliente e se não existir
                            //então ir buscar a data da última fatura ao contrato
                            lastInvoiceDate = DBContractClientRequisition.GetLatsInvoiceDateFor(item.NºDeContrato, contractLine.GrupoFatura);
                            if (lastInvoiceDate.HasValue)
                            {
                                nextInvoiceDate = lastInvoiceDate.Value;
                            }
                            else
                            {
                                if (item.ÚltimaDataFatura.HasValue)
                                    nextInvoiceDate = item.ÚltimaDataFatura.Value;
                            }

                            //Se ainda não houver nenhuma fatura do contrato (data ultima fatura vazia) - Tentar obter a data da ultima fatura a partir da Data Inicio do Contrato. 
                            if (nextInvoiceDate.Equals(DateTime.MinValue))
                            {
                                nextInvoiceDate = item.DataInicial.HasValue ? item.DataInicial.Value.AddDays(-1) : lastDay;
                            }

                            //Para a data de faturação, o utilizador insere o ultimo dia do mês. Adicionar um dia para evitar contabilizar o mês selecionado.
                            //TODO: Verificar se existe necessidade de aplicar regra mais sólida 
                            //AMARO TESTE é necessário esta regra???
                            //if (GetMonthDiff(nextInvoiceDate, lastDay) == 0)
                            //    nextInvoiceDate = nextInvoiceDate.AddDays(1);

                            #endregion

                            if (contractLine.Quantidade != 0)
                            {
                                lineQuantity = contractLine.Quantidade == null ? lineQuantity : contractLine.Quantidade.Value;
                            }

                            int MonthDiff = 0;
                            int rest = 0;
                            int AddMonth = 0;
                            DateTime LastInvoice = lastDay;
                            if (item.PeríodoFatura != null || item.PeríodoFatura != 0)
                            {
                                switch (item.PeríodoFatura)
                                {
                                    case 1://Mensal
                                        MonthDiff = (GetMonthDiff(current, nextInvoiceDate));
                                        if (MonthDiff >= 0)
                                        {
                                            invoiceNumber = MonthDiff / 1;
                                            if (LastInvoice == item.DataExpiração)
                                            {
                                                nextInvoiceDate = LastInvoice;
                                                lineQuantity = lineQuantity * 1;
                                            }
                                            else
                                            {
                                                nextInvoiceDate = nextInvoiceDate.AddMonths(1);
                                                lineQuantity = lineQuantity * 1;

                                            }
                                        }
                                        else
                                        {
                                            invoiceNumber = 0;
                                            nextInvoiceDate = nextInvoiceDate.AddMonths(1);
                                            lineQuantity = lineQuantity * 1;
                                        }
                                        break;
                                    case 2://Bimensal
                                        MonthDiff = (GetMonthDiff(current, nextInvoiceDate));
                                        if (MonthDiff >= 0)
                                        {
                                            rest = MonthDiff % 2;
                                            AddMonth = 2 - rest;
                                            if (AddMonth != 2)
                                            {
                                                LastInvoice = current.AddMonths(AddMonth);
                                            }
                                            else
                                            {
                                                LastInvoice = current;
                                                AddMonth = 0;
                                            }
                                            invoiceNumber = MonthDiff / 2;
                                            if (LastInvoice == item.DataExpiração)
                                            {
                                                nextInvoiceDate = LastInvoice;
                                            }
                                            else
                                            {
                                                if (AddMonth == 0)
                                                {
                                                    nextInvoiceDate = nextInvoiceDate.AddMonths(2);
                                                }
                                                else
                                                {
                                                    nextInvoiceDate = LastInvoice;
                                                }

                                            }
                                            lineQuantity = lineQuantity * 2;
                                        }
                                        else
                                        {
                                            invoiceNumber = 0;
                                            nextInvoiceDate = nextInvoiceDate.AddMonths(2);
                                            lineQuantity = lineQuantity * 2;
                                        }
                                        break;
                                    case 3://Trimestral
                                        MonthDiff = (GetMonthDiff(current, nextInvoiceDate));
                                        if (MonthDiff >= 0)
                                        {
                                            rest = MonthDiff % 3;
                                            AddMonth = 3 - rest;
                                            if (AddMonth != 3)
                                            {
                                                LastInvoice = current.AddMonths(AddMonth);
                                            }
                                            else
                                            {
                                                LastInvoice = current;
                                                AddMonth = 0;
                                            }
                                            invoiceNumber = MonthDiff / 3;
                                            if (LastInvoice == item.DataExpiração)
                                            {
                                                nextInvoiceDate = LastInvoice;
                                            }
                                            else
                                            {
                                                if (AddMonth == 0)
                                                {
                                                    nextInvoiceDate = nextInvoiceDate.AddMonths(3);
                                                }
                                                else
                                                {
                                                    nextInvoiceDate = LastInvoice;
                                                }

                                            }
                                            lineQuantity = lineQuantity * 3;
                                        }
                                        else
                                        {
                                            invoiceNumber = 0;
                                            nextInvoiceDate = nextInvoiceDate.AddMonths(3);
                                            lineQuantity = lineQuantity * 3;
                                        }
                                        break;
                                    case 4://Semestral
                                        MonthDiff = (GetMonthDiff(current, nextInvoiceDate));
                                        if (MonthDiff >= 0)
                                        {
                                            rest = MonthDiff % 6;
                                            AddMonth = 6 - rest;
                                            if (AddMonth != 6)
                                            {
                                                if (MonthDiff <= 6)
                                                    LastInvoice = current.AddMonths(AddMonth);
                                                else
                                                {
                                                    //FATURA A EMITIR NOS MESES SEGUINTES
                                                    LastInvoice = nextInvoiceDate;
                                                    AddMonth = 0;
                                                }
                                            }
                                            else
                                            {
                                                LastInvoice = current;
                                                AddMonth = 0;
                                            }
                                            invoiceNumber = MonthDiff / 6;
                                            if (LastInvoice == item.DataExpiração)
                                            {
                                                nextInvoiceDate = LastInvoice;
                                            }
                                            else
                                            {
                                                if (AddMonth == 0)
                                                {
                                                    nextInvoiceDate = nextInvoiceDate.AddMonths(6);
                                                }
                                                else
                                                {
                                                    nextInvoiceDate = LastInvoice;
                                                }
                                            }
                                            lineQuantity = lineQuantity * 6;
                                        }
                                        else
                                        {
                                            invoiceNumber = 0;
                                            nextInvoiceDate = nextInvoiceDate.AddMonths(6);
                                            lineQuantity = lineQuantity * 6;
                                        }
                                        break;
                                    case 5://Anual
                                        MonthDiff = (GetMonthDiff(current, nextInvoiceDate));
                                        if (MonthDiff >= 0)
                                        {
                                            rest = MonthDiff % 12;
                                            AddMonth = 12 - rest;
                                            if (AddMonth != 12)
                                            {
                                                LastInvoice = current.AddMonths(AddMonth);
                                            }
                                            else
                                            {
                                                LastInvoice = current;
                                                AddMonth = 0;
                                            }
                                            invoiceNumber = MonthDiff / 12;
                                            if (LastInvoice == item.DataExpiração)
                                            {
                                                nextInvoiceDate = LastInvoice;
                                            }
                                            else
                                            {
                                                if (AddMonth == 0)
                                                {
                                                    nextInvoiceDate = nextInvoiceDate.AddMonths(12);
                                                }
                                                else
                                                {
                                                    nextInvoiceDate = LastInvoice;
                                                }
                                            }
                                            lineQuantity = lineQuantity * 12;
                                        }
                                        else
                                        {
                                            invoiceNumber = 0;
                                            nextInvoiceDate = nextInvoiceDate.AddMonths(12);
                                            lineQuantity = lineQuantity * 12;
                                        }
                                        break;
                                    case 6:
                                        //Nenhum
                                        break;
                                    default:
                                        break;
                                }
                            }

                            #region  Validações para registar situações
                            Problema = "";
                            if (item.TipoFaturação != 1 && item.TipoFaturação != 4)
                            {
                                Problema += " Tipo de fatura mal definido!";
                            }

                            if (item.Estado != 4)
                            {
                                Problema += " Contrato Não Assinado!";
                            }
                            if (item.EstadoAlteração == 1)
                            {
                                Problema += " Contrato Aberto!";
                            }

                            if (lastDay < item.DataInicial || lastDay > item.DataExpiração)
                            {
                                Problema += " Contrato Não Vigente!";
                            }
                            if (nextInvoiceDate < item.DataInicial)
                            {
                                Problema += " Data da próxima fatura inferior á data de Início da Versão!";
                            }
                            if (nextInvoiceDate > item.DataExpiração)
                            {
                                Problema += " Data da próxima fatura superior á data de Expiração!";
                            }
                            if (item.CódigoRegião == "")
                            {
                                Problema += " Dimensões Bloqueadas!";
                            }
                            if (item.CódTermosPagamento == "")
                            {
                                Problema += " Falta Código Termos Pagamento!";
                            }
                            if (item.EnvioAEndereço == "")
                            {
                                Problema += " Falta Morada!";
                            }
                            bool verifica = false;
                            if (item.NºComprimissoObrigatório == false || item.NºComprimissoObrigatório == null)
                            {
                                foreach (RequisiçõesClienteContrato req in DBContractClientRequisition.GetByContract(item.NºContrato))
                                {
                                    if (req.GrupoFatura == contractLine.GrupoFatura && req.DataInícioCompromisso == item.DataInícioContrato && req.DataFimCompromisso == item.DataFimContrato)
                                    {
                                        if (req.NºCompromisso == "" || req.NºCompromisso == null)
                                            verifica = true;
                                    }
                                }
                            }
                            if (verifica == true)
                                Problema += " Falta Nº Compromisso!";

                            if (!Problema.Contains("Fatura no Pre-Registo!"))
                            {
                                NAVSalesHeaderViewModel result = DBNAV2017SalesHeader.GetSalesHeaderByGrupoFatura(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºDeContrato, NAVBaseDocumentTypes.Fatura, contractLine.GrupoFatura.Value);
                                if (result != null)
                                {
                                    Problema += " Fatura no Pre-Registo!";
                                }
                            }

                            if (!string.IsNullOrEmpty(item.NºCliente))
                            {
                                Task<ClientDetailsViewModel> postNAV = WSCustomerService.GetByNoAsync(item.NºCliente, _configws);
                                postNAV.Wait();
                                if (postNAV.IsCompletedSuccessfully == true && postNAV.Result != null)
                                {
                                    if (postNAV.Result.Blocked == Blocked.Invoice || postNAV.Result.Blocked == Blocked.All)
                                    {
                                        Problema += " Cliente Bloqueado!";
                                    }
                                }

                                NAVClientsViewModel cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºCliente);
                                if (cliente != null)
                                {
                                    if (string.IsNullOrEmpty(cliente.VATRegistrationNo_))
                                    {
                                        Problema += " Cliente falta Nº Contribuinte!";
                                    }
                                }
                            }

                            Decimal invoicePeriod = salesList != null ? salesList.Sum(x => x.Amount) : 0;
                            Decimal creditPeriod = crMemo != null ? crMemo.Sum(x => x.Amount) : 0;

                            decimal valFatura = invoicePeriod - creditPeriod;
                            decimal ValorPorFatura = (contractVal - (invoicePeriod - creditPeriod));

                            if (valFatura > ValorPorFatura)
                            {
                                Problema += " Valor Não Disponível!";
                            }


                            List<RequisiçõesClienteContrato> ListaReqClientes = new List<RequisiçõesClienteContrato>();
                            RequisiçõesClienteContrato ReqCliente = new RequisiçõesClienteContrato();
                            ListaReqClientes = DBContractClientRequisition.GetByContract(item.NºDeContrato);
                            ReqCliente = ListaReqClientes.Find(x => x.GrupoFatura == contractLine.GrupoFatura
                                && x.DataInícioCompromisso <= nextInvoiceDate
                                && x.DataFimCompromisso >= nextInvoiceDate);
                            if (ReqCliente != null)
                            {
                                if (string.IsNullOrEmpty(ReqCliente.NºRequisiçãoCliente))
                                {
                                    Problema += " Falta Nota Encomenda!";
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(item.NºRequisiçãoDoCliente))
                                {
                                    Problema += " Falta Nota Encomenda!";
                                }
                            }

                            //Se existir algum problema
                            if (!string.IsNullOrEmpty(Problema))
                            {
                                if (!Problema.Contains("Data da próxima fatura superior á data de Expiração"))
                                {
                                    if (!Problema.Contains("Contrato Não Vigente"))
                                    {
                                        int GrupoFatura = contractLine.GrupoFatura == null ? 0 : contractLine.GrupoFatura.Value;

                                        List<LinhasContratos> AllLines = DBContractLines.GetAllByContractAndVersionAndGroup(item.NºDeContrato, item.NºVersão, GrupoFatura);

                                        if (AllLines != null && AllLines.Count > 0)
                                        {
                                            DateTime MaxLineDataFimVersao = AllLines.OrderByDescending(x => x.DataFimVersão).FirstOrDefault().DataFimVersão.HasValue ? Convert.ToDateTime(AllLines.OrderByDescending(x => x.DataFimVersão).FirstOrDefault().DataFimVersão) : DateTime.MinValue;

                                            if (MaxLineDataFimVersao > DateTime.MinValue)
                                            {
                                                if (nextInvoiceDate > MaxLineDataFimVersao)
                                                {
                                                    Problema = "Contrato Não Vigente - Grupo de Fatura " + GrupoFatura.ToString();
                                                }
                                            }
                                        }
                                    }
                                    else
                                        Problema = "Contrato Não Vigente!";
                                }
                            }
                            #endregion

                            AutorizarFaturaçãoContratos newInvoiceContract = new AutorizarFaturaçãoContratos
                            {
                                NºContrato = item.NºDeContrato,
                                GrupoFatura = contractLine.GrupoFatura == null ? 0 : contractLine.GrupoFatura.Value,
                                Descrição = item.Descrição,
                                NºCliente = item.NºCliente,
                                CódigoRegião = item.CódigoRegião,
                                CódigoÁreaFuncional = item.CódigoÁreaFuncional,
                                CódigoCentroResponsabilidade = item.CódigoCentroResponsabilidade,
                                ValorDoContrato = contractVal,
                                ValorFaturado = valFatura,
                                ValorPorFaturar = ValorPorFatura,
                                NºDeFaturasAEmitir = invoiceNumber,
                                DataPróximaFatura = nextInvoiceDate,
                                DataDeRegisto = lastDay,
                                Estado = item.Estado,
                                Situação = Problema,
                                DataHoraCriação = DateTime.Now,
                                UtilizadorCriação = User.Identity.Name,

                                NoRequisicaoDoCliente = ReqCliente != null ? ReqCliente.NºRequisiçãoCliente : item.NºRequisiçãoDoCliente,
                                DataRececaoRequisicao = ReqCliente != null ? ReqCliente.DataRequisição : item.DataReceçãoRequisição,
                                NoCompromisso = ReqCliente != null ? ReqCliente.NºCompromisso : item.NºCompromisso,
                            };
                            //AMARO TESTE TRY
                            try
                            {
                                DBAuthorizeInvoiceContracts.Create(newInvoiceContract);
                            }
                            catch (Exception ex)
                            {
                                return Json(false);
                            }
                        }
                        else
                        {
                            if (contractLine.Quantidade.HasValue && item.PeríodoFatura.HasValue)
                            {
                                if (contractLine.Quantidade == 0)
                                    contractLine.Quantidade = lineQuantity;

                                lineQuantity = Convert.ToDecimal(contractLine.Quantidade * (item.PeríodoFatura == 6 ? 0 : item.PeríodoFatura == 5 ? 12 : item.PeríodoFatura == 4 ? 6 : item.PeríodoFatura));
                            }
                        }

                        if (lineQuantity == 0)
                        {
                            Problema = " Sem Valor!";
                        }

                        LinhasFaturaçãoContrato newInvoiceLine = new LinhasFaturaçãoContrato
                        {
                            NºContrato = contractLine.NºContrato,
                            NºProjeto = contractLine.NºProjeto,
                            GrupoFatura = contractLine.GrupoFatura == null ? 0 : contractLine.GrupoFatura.Value, //06-03-2019 Antes estava -1
                            NºLinha = contractLine.NºLinha,
                            Tipo = contractLine.Tipo.ToString(),
                            Código = contractLine.Código,
                            Descrição = contractLine.Descrição,
                            Descricao2 = contractLine.Descricao2,
                            Quantidade = lineQuantity,
                            CódUnidadeMedida = contractLine.CódUnidadeMedida,
                            PreçoUnitário = contractLine.PreçoUnitário,
                            ValorVenda = (lineQuantity * contractLine.PreçoUnitário),
                            CódigoRegião = contractLine.CódigoRegião,
                            CódigoÁreaFuncional = contractLine.CódigoÁreaFuncional,
                            CódigoCentroResponsabilidade = contractLine.CódigoCentroResponsabilidade,
                            CódigoServiço = contractLine.CódServiçoCliente,
                            DataHoraCriação = DateTime.Now,
                            UtilizadorCriação = User.Identity.Name
                        };
                        //AMARO TESTE TRY
                        try
                        {
                            DBInvoiceContractLines.Create(newInvoiceLine);
                        }
                        catch (Exception ex)
                        {
                            return Json(false);
                        }
                    }
                }
                else
                {
                    AutorizarFaturaçãoContratos newInvoiceContract = new AutorizarFaturaçãoContratos
                    {
                        NºContrato = item.NºDeContrato,
                        GrupoFatura = 0,
                        Descrição = item.Descrição,
                        NºCliente = item.NºCliente,
                        CódigoRegião = item.CódigoRegião,
                        CódigoÁreaFuncional = item.CódigoÁreaFuncional,
                        CódigoCentroResponsabilidade = item.CódigoCentroResponsabilidade,
                        ValorDoContrato = 0,
                        ValorFaturado = 0,
                        ValorPorFaturar = 0,
                        NºDeFaturasAEmitir = 0,
                        //DataPróximaFatura = nextInvoiceDate,
                        DataDeRegisto = lastDay,
                        Estado = item.Estado,
                        Situação = "O Contrato não tem linhas faturáveis",
                        DataHoraCriação = DateTime.Now,
                        UtilizadorCriação = User.Identity.Name,

                        NoRequisicaoDoCliente = item.NºRequisiçãoDoCliente,
                        DataRececaoRequisicao = item.DataReceçãoRequisição,
                        NoCompromisso = item.NºCompromisso,
                    };
                    //AMARO TESTE TRY
                    try
                    {
                        DBAuthorizeInvoiceContracts.Create(newInvoiceContract);
                    }
                    catch (Exception ex)
                    {
                        return Json(false);
                    }
                }
            }
            return Json(true);
        }

        public JsonResult CountInvoice([FromBody] List<FaturacaoContratosViewModel> data)
        {
            string execDetails = string.Empty;
            string errorMessage = string.Empty;
            bool hasErrors = false;
            ErrorHandler result = new ErrorHandler();
            bool PricesIncludingVAT = false;

            List<LinhasFaturaçãoContrato> lineList = DBInvoiceContractLines.GetAll();
            List<AutorizarFaturaçãoContratos> contractList = new List<AutorizarFaturaçãoContratos>();
            foreach (FaturacaoContratosViewModel itm in data)
            {
                List<AutorizarFaturaçãoContratos> contract_List = new List<AutorizarFaturaçãoContratos>();

                if (itm.InvoiceGroupValue >= 0)
                //ALTERAÇÂO FEITA 08-01-2021 if (itm.InvoiceGroupValue > 0)
                    contract_List = DBAuthorizeInvoiceContracts.GetAllByContGroup(itm.ContractNo).Where(x => x.GrupoFatura == itm.InvoiceGroupValue).ToList();
                else
                    contract_List = DBAuthorizeInvoiceContracts.GetAllByContGroup(itm.ContractNo);

                if (contract_List != null && contract_List.Count > 0)
                {
                    foreach (AutorizarFaturaçãoContratos item in contract_List)
                    {
                        if (item.NºDeFaturasAEmitir > 0)
                            contractList.Add(item);
                    }
                }
            }

            foreach (var item in contractList)
            {
                if (item.NºDeFaturasAEmitir == 1)
                {
                    int? CountLines = data.Where(x => x.ContractNo == item.NºContrato && x.InvoiceGroupValue == item.GrupoFatura).Count();
                    string ContractInvoicePeriod = "";
                    string InvoiceBorrowed = "";
                    string Month = "";
                    string Year = "";
                    DateTime Lastdate = item.DataDeRegisto.Value;
                    DateTime DataDocumento = item.DataDeRegisto.Value;
                    Contratos contractLine = DBContracts.GetByIdAvencaFixa(item.NºContrato);

                    DateTime StContractDate = new DateTime();
                    StContractDate = (DateTime)item.DataPróximaFatura;

                    if (Lastdate != StContractDate)
                        Lastdate = StContractDate;
                    Month = StContractDate.ToString("MMMM").ToUpper();
                    Year = StContractDate.Year.ToString();
                    if (CountLines != null && CountLines > 1)
                    {
                        RequisiçõesClienteContrato GetReqClientCont = DBContractClientRequisition.GetByContractAndGroup(item.NºContrato, item.GrupoFatura);
                        if (GetReqClientCont != null)
                        {
                            Lastdate = (new DateTime(Lastdate.Year, Lastdate.Month, 1)).AddMonths(1).AddDays(-1);
                            ContractInvoicePeriod = Lastdate.ToString("dd/MM/yy");
                        }
                    }
                    else
                    {
                        if (contractLine != null)
                        {
                            if (!String.IsNullOrEmpty(contractLine.PróximoPeríodoFact))
                            {

                                int findDate = contractLine.PróximoPeríodoFact.IndexOf("-");
                                if (findDate == 2)
                                {
                                    contractLine.PróximoPeríodoFact = contractLine.PróximoPeríodoFact.Replace(" ", "");
                                    if (contractLine.PróximoPeríodoFact.Length == 8)
                                    {
                                        ContractInvoicePeriod = contractLine.PróximoPeríodoFact;
                                    }
                                }
                                else if (findDate == 4)
                                {
                                    string proxperFacRep = contractLine.PróximoPeríodoFact.Replace(" ", "");
                                    string[] ProxPerFac = proxperFacRep.Split('a');
                                    if (ProxPerFac.Count() == 2 && proxperFacRep.Length == 17)
                                    {
                                        ContractInvoicePeriod = contractLine.PróximoPeríodoFact;
                                    }

                                }
                            }
                        }
                        Lastdate = (new DateTime(Lastdate.Year, Lastdate.Month, 1)).AddMonths(1).AddDays(-1);
                    }

                    if (item.Situação == "" || item.Situação == null)
                    {
                        Task<WSCreateNAVProject.Read_Result> Project = WSProject.GetNavProject(item.NºContrato, _configws);
                        Project.Wait();
                        if (Project.IsCompletedSuccessfully && Project.Result.WSJob == null)
                        {
                            ProjectDetailsViewModel proj = new ProjectDetailsViewModel();
                            proj.ProjectNo = item.NºContrato;
                            proj.ClientNo = item.NºCliente;
                            proj.Status = EstadoProjecto.Encomenda;
                            proj.RegionCode = item.CódigoRegião;
                            proj.ResponsabilityCenterCode = item.CódigoCentroResponsabilidade;
                            proj.FunctionalAreaCode = item.CódigoÁreaFuncional;
                            proj.Description = item.Descrição;
                            proj.Visivel = false;

                            Task<WSCreateNAVProject.Create_Result> createProject = WSProject.CreateNavProject(proj, _configws);
                            createProject.Wait();
                        }
                        item.UtilizadorCriação = User.Identity.Name;


                        //AMARO TEXTO FATURA
                        string obs = "";
                        List<TextoFaturaContrato> ListTextoFatura = DBContractInvoiceText.GetByContractAndGrupoFatura(contractLine.NºDeContrato, item.GrupoFatura);

                        if (ListTextoFatura != null && ListTextoFatura.Count() > 0)
                        {

                            foreach (TextoFaturaContrato texts in ListTextoFatura)
                            {
                                obs += texts.TextoFatura;
                            }
                        }
                        if (!string.IsNullOrEmpty(obs))
                        {
                            item.Descrição = obs;
                        }
                        else
                        {
                            item.Descrição = contractLine.TextoFatura;
                        }
                        obs = "";

                        //AMARO 04/05/219
                        //Validação cajo existam Requisições Cliente tem que existir pelo menos uma no período da fatura, se não existir sai do processo e retorna mensagem de erro
                        List<RequisiçõesClienteContrato> ListaContratos = new List<RequisiçõesClienteContrato>();
                        RequisiçõesClienteContrato Reqcontract = new RequisiçõesClienteContrato();
                        RequisiçõesClienteContrato ReqLastcontract = new RequisiçõesClienteContrato();
                        Reqcontract = null;
                        DateTime DataInicioFatura;
                        DateTime DataFimFatura = Lastdate;

                        ListaContratos = DBContractClientRequisition.GetByContract(contractLine.NºDeContrato);
                        if (ListaContratos != null && ListaContratos.Count > 0)
                        {
                            Reqcontract = ListaContratos.OrderByDescending(x => x.DataÚltimaFatura).ToList().Find(x => x.GrupoFatura == item.GrupoFatura
                                && x.DataInícioCompromisso <= Lastdate
                                && x.DataFimCompromisso >= Lastdate);

                            if (Reqcontract == null)
                            {
                                result.eReasonCode = 99;
                                result.eMessage = "Não foi encontrada a Requisição Cliente do Contrato Nº " + item.NºContrato + " para o grupo de fatura Nº " + item.GrupoFatura.ToString() + " para o período " + Lastdate.ToShortDateString();
                                return Json(result);
                            }
                            else
                            {
                                ReqLastcontract = ListaContratos.OrderByDescending(x => x.DataÚltimaFatura).ToList().Find(x => x.GrupoFatura == item.GrupoFatura && x.DataÚltimaFatura.HasValue);
                                if (ReqLastcontract != null)
                                    DataInicioFatura = Convert.ToDateTime(ReqLastcontract.DataÚltimaFatura).AddDays(1);
                                else
                                    DataInicioFatura = Convert.ToDateTime(contractLine.DataInicial);
                            }
                        }
                        else
                        {
                            DataInicioFatura = contractLine.ÚltimaDataFatura.HasValue ? Convert.ToDateTime(contractLine.ÚltimaDataFatura).AddDays(1) : Convert.ToDateTime(contractLine.DataInicial);
                        }

                        item.NoRequisicaoDoCliente = Reqcontract != null ? Reqcontract.NºRequisiçãoCliente : contractLine.NºRequisiçãoDoCliente;
                        item.NoCompromisso = Reqcontract != null ? Reqcontract.NºCompromisso : contractLine.NºCompromisso;
                        item.DataRececaoRequisicao = Reqcontract != null ? Reqcontract.DataRequisição : contractLine.DataReceçãoRequisição;
                        item.NºContrato = contractLine.NºDeContrato;
                        item.CódigoRegião = contractLine.CódigoRegião;
                        item.CódigoÁreaFuncional = contractLine.CódigoÁreaFuncional;
                        item.CódigoCentroResponsabilidade = contractLine.CódigoCentroResponsabilidade;

                        InvoiceBorrowed = GetTextoFatura(DataInicioFatura, DataFimFatura);

                        //AMARO DUEDATE = DataExpiração
                        item.DataDeExpiração = DataDocumento;
                        string CodTermosPagamento = contractLine.CódTermosPagamento;
                        if (!string.IsNullOrEmpty(contractLine.CódTermosPagamento))
                        {
                            NAVPaymentTermsViewModels PaymentTerms = DBNAV2017PaymentTerms.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, contractLine.CódTermosPagamento).FirstOrDefault();
                            if (PaymentTerms != null)
                            {
                                string AUXDueDateCalculation = PaymentTerms.DueDateCalculation;
                                AUXDueDateCalculation = AUXDueDateCalculation.Substring(0, AUXDueDateCalculation.IndexOf(''));

                                if (!string.IsNullOrEmpty(AUXDueDateCalculation) && Double.TryParse(AUXDueDateCalculation, out double num))
                                {
                                    item.DataDeExpiração = DataDocumento.AddDays(Convert.ToDouble(AUXDueDateCalculation));
                                }
                            }
                        }

                        if (contractLine.FaturaPrecosIvaIncluido == true)
                            PricesIncludingVAT = true;
                        else
                            PricesIncludingVAT = false;

                        //AMARO Ship to Address = Endereços de Envio
                        string Ship_to_Code = string.Empty;
                        Contratos cont = DBContracts.GetByIdLastVersion(item.NºContrato);
                        NAVClientsViewModel Cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºCliente);

                        if (cont != null && !string.IsNullOrEmpty(cont.CódEndereçoEnvio))
                        {
                            item.NºCliente = !string.IsNullOrEmpty(cont.NºCliente) ? cont.NºCliente : "";
                            Ship_to_Code = !string.IsNullOrEmpty(cont.CódEndereçoEnvio) ? cont.CódEndereçoEnvio : "";
                        }
                        else
                        {
                            if (Cliente != null)
                            {
                                item.NºCliente = !string.IsNullOrEmpty(Cliente.No_) ? Cliente.No_ : "";
                                Ship_to_Code = "";
                            }
                        }

                        //AMARO Clientes Internos
                        if (Cliente != null && Cliente.InternalClient == true)
                        {
                            item.CódigoRegião = Cliente.RegionCode;
                            item.CódigoÁreaFuncional = Cliente.FunctionalAreaCode;
                            item.CódigoCentroResponsabilidade = Cliente.ResponsabilityCenterCode;
                        }

                        //Contratos Tipo Quotas
                        string MetdoPagamento = "";
                        if (cont != null && cont.Tipo == 3)
                        {
                            MetdoPagamento = !string.IsNullOrEmpty(cont.CódFormaPagamento) ? cont.CódFormaPagamento : DBConfiguracaoParametros.GetByParametro("QuotasMetPagamento").Valor;
                            item.Descrição = item.Descrição.Replace("<MES>", DataInicioFatura.ToString("MMMM").ToUpper());
                            item.Descrição = item.Descrição.Replace("<ANO>", DataInicioFatura.Year.ToString());
                        }



                        Task<WSCreatePreInvoice.Create_Result> InvoiceHeader = WSPreInvoice.CreateContractInvoice(item, _configws, ContractInvoicePeriod, InvoiceBorrowed, CodTermosPagamento, MetdoPagamento, PricesIncludingVAT, Ship_to_Code);
                        InvoiceHeader.Wait();

                        if (InvoiceHeader.IsCompletedSuccessfully && InvoiceHeader != null && InvoiceHeader.Result != null)
                        {
                            if (Reqcontract != null)
                            {
                                Reqcontract.DataÚltimaFatura = Lastdate;
                                DBContractClientRequisition.Update(Reqcontract);
                            }
                            else
                            {
                                contractLine.ÚltimaDataFatura = Lastdate;
                                DBContracts.Update(contractLine);
                            }

                            //Estado Pendente
                            //11-12-2018 ARomao@such.pt
                            //A pedido do Marco Marcelo o contrato nunca pode mudar de estado
                            //item.Estado = 3;

                            String InvoiceHeaderNo = InvoiceHeader.Result.WSPreInvoice.No;
                            List<LinhasFaturaçãoContrato> itemList = lineList.Where(x => x.NºContrato == item.NºContrato && x.GrupoFatura == item.GrupoFatura).ToList();

                            if (itemList.Count > 0)
                            {
                                if (contractLine.FaturaPrecosIvaIncluido == true)
                                {
                                    string NoCliente = string.Empty;
                                    string GrupoIVA = string.Empty;
                                    string GrupoCliente = string.Empty;
                                    decimal IVA = new decimal();
                                    foreach (var linha in itemList)
                                    {
                                        NoCliente = contractLine.NºCliente;
                                        GrupoIVA = string.Empty;
                                        GrupoCliente = string.Empty;
                                        IVA = 0;

                                        if (!string.IsNullOrEmpty(linha.Código))
                                        {
                                            NAVResourcesViewModel Resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, linha.Código, "", 0, "").FirstOrDefault();
                                            if (Resource != null)
                                                GrupoIVA = Resource.VATProductPostingGroup;
                                            else
                                            {
                                                execDetails += " Erro ao contabilizar: Não foi possível encontrar o recurso Nº: " + linha.Código + " para o Nº de contrato: " + contractLine.NºContrato;
                                                result.eReasonCode = 2;
                                                result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails));
                                                return Json(result);
                                            }

                                            if (!string.IsNullOrEmpty(NoCliente) && !string.IsNullOrEmpty(GrupoIVA))
                                            {
                                                NAVClientsViewModel cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, NoCliente);
                                                if (cliente != null)
                                                    GrupoCliente = cliente.VATBusinessPostingGroup;
                                                else
                                                {
                                                    execDetails += " Erro ao contabilizar: Não foi possível encontrar o cliente Nº: " + Cliente + " para o Nº de contrato: " + contractLine.NºContrato;
                                                    result.eReasonCode = 2;
                                                    result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails));
                                                    return Json(result);
                                                }

                                                if (!string.IsNullOrEmpty(GrupoCliente))
                                                    IVA = DBNAV2017VATPostingSetup.GetIVA(_config.NAVDatabaseName, _config.NAVCompanyName, GrupoCliente, GrupoIVA);

                                                if (IVA > 0)
                                                    IVA = (IVA / 100) + 1;
                                            }
                                        }

                                        if (IVA > 0)
                                            linha.PreçoUnitário = linha.PreçoUnitário * IVA;

                                        linha.Data_Inicio_Serv_Prestado = DataInicioFatura;
                                        linha.Data_Fim_Serv_Prestado = DataFimFatura;

                                        if (cont != null && cont.Tipo == 3)
                                        {
                                            linha.Descrição = linha.Descrição.Replace("<MES>", DataInicioFatura.ToString("MMMM").ToUpper());
                                            linha.Descrição = linha.Descrição.Replace("<ANO>", DataInicioFatura.Year.ToString());
                                        }

                                    }
                                }
                                else
                                {
                                    foreach (var linha in itemList)
                                    {
                                        linha.Data_Inicio_Serv_Prestado = DataInicioFatura;
                                        linha.Data_Fim_Serv_Prestado = DataFimFatura;

                                        if (cont != null && cont.Tipo == 3)
                                        {
                                            linha.Descrição = linha.Descrição.Replace("<MES>", DataInicioFatura.ToString("MMMM").ToUpper());
                                            linha.Descrição = linha.Descrição.Replace("<ANO>", DataInicioFatura.Year.ToString());
                                        }
                                    }
                                }

                                try
                                {
                                    Task<WSCreatePreInvoiceLine.CreateMultiple_Result> InvoiceLines = WSPreInvoiceLine.CreatePreInvoiceLineList(itemList, InvoiceHeaderNo, InvoiceBorrowed, _configws);
                                    InvoiceLines.Wait();
                                }
                                catch (Exception ex)
                                {
                                    //09/07/02019
                                    //A Pedido do Marco Marcelo quando o WebService devolve uma mensagem com o texto "maximum message size quota"
                                    //assume-se que o mesmo foi executado com sucesso e continua.
                                    errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                                    if (!errorMessage.ToLower().Contains("maximum message size quota".ToLower()))
                                    {
                                        if (!hasErrors)
                                            hasErrors = true;

                                        execDetails += " Erro ao criar as linhas: ";
                                        errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                        result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails + errorMessage));
                                    }
                                }
                            }
                        }
                        else
                        {
                            return Json(false);
                        }
                    }
                }
                else
                {
                    //AMARO INICIO NOVO PROCESSO
                    if (item.NºDeFaturasAEmitir > 1)
                    {
                        bool FirstTime = true;
                        bool FirstTimeLine = true;
                        string LinhaDescricaoOriginal = "";
                        string LinhaDescricao2Original = "";
                        for (int i = 1; i <= item.NºDeFaturasAEmitir; i++)
                        {
                            int? CountLines = data.Where(x => x.ContractNo == item.NºContrato && x.InvoiceGroupValue == item.GrupoFatura).Count();
                            string ContractInvoicePeriod = "";
                            string InvoiceBorrowed = "";
                            string Month = "";
                            string Year = "";
                            DateTime Lastdate = item.DataDeRegisto.Value;
                            DateTime DataDocumento = item.DataDeRegisto.Value;
                            DateTime StContractDate = new DateTime();
                            Contratos contractLine = DBContracts.GetByIdAvencaFixa(item.NºContrato);

                            DateTime todayStart = new DateTime();
                            DateTime todayEnd = new DateTime();
                            //if (FirstTime == true)
                            //{
                            //    StContractDate = (DateTime)item.DataPróximaFatura;

                            //    FirstTime = false;
                            //}
                            //else
                            //{
                                List<RequisiçõesClienteContrato> ListaReqClientes = new List<RequisiçõesClienteContrato>();
                                RequisiçõesClienteContrato ReqCliente = new RequisiçõesClienteContrato();
                                ListaReqClientes = DBContractClientRequisition.GetByContract(contractLine.NºDeContrato);
                                if (ListaReqClientes != null && ListaReqClientes.Count > 0)
                                {
                                    ReqCliente = ListaReqClientes.OrderByDescending(x => x.DataÚltimaFatura).ToList().Find(x => x.GrupoFatura == item.GrupoFatura);
                                    if (ReqCliente != null && ReqCliente.DataÚltimaFatura.HasValue)
                                    {
                                        todayEnd = (DateTime)ReqCliente.DataÚltimaFatura;
                                    }
                                    else
                                    {
                                        todayEnd = contractLine.ÚltimaDataFatura != null ? (DateTime)contractLine.ÚltimaDataFatura : Convert.ToDateTime(contractLine.DataInicial).AddDays(-1); //DateTime.Now;
                                    }
                                }
                                else
                                {
                                    todayEnd = contractLine.ÚltimaDataFatura != null ? (DateTime)contractLine.ÚltimaDataFatura : Convert.ToDateTime(contractLine.DataInicial).AddDays(-1); //DateTime.Now;
                                }

                                todayStart = todayEnd.AddDays(1);
                                StContractDate = todayStart;

                                if (contractLine.DataInicial != null && contractLine.DataExpiração != null)// && item.DataPróximaFatura == null)
                                {
                                    //today = (DateTime)contractLine.ÚltimaDataFatura;
                                    //today = contractLine.ÚltimaDataFatura != null ? (DateTime)contractLine.ÚltimaDataFatura : Convert.ToDateTime(contractLine.DataInicial).AddDays(-1);

                                    //Mensal 
                                    if (contractLine.PeríodoFatura == 1)
                                    {
                                        todayEnd = todayEnd.AddMonths(1);
                                    }
                                    //Bimensal 
                                    if (contractLine.PeríodoFatura == 2)
                                    {
                                        todayEnd = todayEnd.AddMonths(2);
                                    }
                                    //Trimestral 
                                    if (contractLine.PeríodoFatura == 3)
                                    {
                                        todayEnd = todayEnd.AddMonths(3);
                                    }
                                    //Semestral 
                                    if (contractLine.PeríodoFatura == 4)
                                    {
                                        todayEnd = todayEnd.AddMonths(6);
                                    }
                                    //Anual 
                                    if (contractLine.PeríodoFatura == 5)
                                    {
                                        todayEnd = todayEnd.AddMonths(12);
                                    }

                                    if (todayStart < contractLine.DataInicial)
                                    {
                                        StContractDate = (DateTime)contractLine.DataInicial;
                                    }
                                    else if (todayEnd > contractLine.DataExpiração)
                                    {
                                        Lastdate = (DateTime)contractLine.DataExpiração;
                                    }
                                    else
                                    {
                                        Lastdate = todayEnd;
                                    }
                                }
                            //}

                            //if (Lastdate != StContractDate)
                            //{
                            //    Lastdate = StContractDate;
                            //}

                            Month = StContractDate.ToString("MMMM").ToUpper();
                            Year = StContractDate.Year.ToString();
                            //InvoiceBorrowed = Month + "/" + Year;
                            InvoiceBorrowed = GetTextoFatura(StContractDate, Lastdate);

                            if (CountLines != null && CountLines > 1)
                            {
                                RequisiçõesClienteContrato GetReqClientCont = DBContractClientRequisition.GetByContractAndGroup(item.NºContrato, item.GrupoFatura);
                                if (GetReqClientCont != null)
                                {
                                    Lastdate = (new DateTime(Lastdate.Year, Lastdate.Month, 1)).AddMonths(1).AddDays(-1);
                                    ContractInvoicePeriod = Lastdate.ToString("dd/MM/yy");
                                    //GetReqClientCont.DataÚltimaFatura = Lastdate;
                                    //DBContractClientRequisition.Update(GetReqClientCont);
                                }
                            }
                            else
                            {
                                Lastdate = (new DateTime(Lastdate.Year, Lastdate.Month, 1)).AddMonths(1).AddDays(-1);
                                ContractInvoicePeriod = Lastdate.ToString("dd/MM/yy");
                                //contractLine.ÚltimaDataFatura = Lastdate;
                                //DBContracts.Update(contractLine);
                            }

                            if (item.Situação == "" || item.Situação == null)
                            {
                                Task<WSCreateNAVProject.Read_Result> Project = WSProject.GetNavProject(item.NºContrato, _configws);
                                Project.Wait();
                                if (Project.IsCompletedSuccessfully && Project.Result.WSJob == null)
                                {
                                    ProjectDetailsViewModel proj = new ProjectDetailsViewModel();
                                    proj.ProjectNo = item.NºContrato;
                                    proj.ClientNo = item.NºCliente;
                                    proj.Status = EstadoProjecto.Encomenda;
                                    proj.RegionCode = item.CódigoRegião;
                                    proj.ResponsabilityCenterCode = item.CódigoCentroResponsabilidade;
                                    proj.FunctionalAreaCode = item.CódigoÁreaFuncional;
                                    proj.Description = item.Descrição;
                                    proj.Visivel = false;

                                    Task<WSCreateNAVProject.Create_Result> createProject = WSProject.CreateNavProject(proj, _configws);
                                    createProject.Wait();
                                }
                                item.UtilizadorCriação = User.Identity.Name;

                                //AMARO TEXTO FATURA
                                string obs = "";
                                List<TextoFaturaContrato> ListTextoFatura = DBContractInvoiceText.GetByContractAndGrupoFatura(contractLine.NºDeContrato, item.GrupoFatura);
                                if (ListTextoFatura != null && ListTextoFatura.Count() > 0)
                                {

                                    foreach (TextoFaturaContrato texts in ListTextoFatura)
                                    {
                                        obs += texts.TextoFatura;
                                    }
                                }
                                if (!string.IsNullOrEmpty(obs))
                                {
                                    item.Descrição = obs;
                                }
                                else
                                {
                                    item.Descrição = contractLine.TextoFatura;
                                }
                                obs = "";

                                //AMARO 04/05/219
                                //Validação cajo existam Requisições Cliente tem que existir pelo menos uma no período da fatura, se não existir sai do processo e retorna mensagem de erro
                                List<RequisiçõesClienteContrato> ListaContratos = new List<RequisiçõesClienteContrato>();
                                RequisiçõesClienteContrato Reqcontract = new RequisiçõesClienteContrato();
                                Reqcontract = null;

                                ListaContratos = DBContractClientRequisition.GetByContract(contractLine.NºDeContrato);
                                if (ListaContratos != null && ListaContratos.Count > 0)
                                {
                                    Reqcontract = ListaContratos.Find(x => x.GrupoFatura == item.GrupoFatura
                                        && x.DataInícioCompromisso <= Lastdate
                                        && x.DataFimCompromisso >= Lastdate);

                                    if (Reqcontract == null)
                                    {
                                        result.eReasonCode = 99;
                                        result.eMessage = "Não foi encontrada a Requisição Cliente do Contrato Nº " + item.NºContrato + " para o grupo de fatura Nº " + item.GrupoFatura.ToString() + " para o período " + Lastdate.ToShortDateString();
                                        return Json(result);
                                    }
                                }

                                //if (contractLine.ÚltimaDataFatura == null)
                                //{
                                //    contractLine.ÚltimaDataFatura = Lastdate;

                                //    ListaContratos = DBContractClientRequisition.GetByContract(contractLine.NºDeContrato);

                                //    Reqcontract = ListaContratos.Find(x => x.GrupoFatura == item.GrupoFatura
                                //        && x.DataInícioCompromisso <= contractLine.ÚltimaDataFatura
                                //        && x.DataFimCompromisso >= contractLine.ÚltimaDataFatura);
                                //}
                                //else
                                //{
                                //    ListaContratos = DBContractClientRequisition.GetByContract(contractLine.NºDeContrato);

                                //    Reqcontract = ListaContratos.Find(x => x.GrupoFatura == item.GrupoFatura
                                //        && x.DataInícioCompromisso <= contractLine.ÚltimaDataFatura
                                //        && x.DataFimCompromisso >= contractLine.ÚltimaDataFatura);

                                //}

                                item.NoRequisicaoDoCliente = Reqcontract != null ? Reqcontract.NºRequisiçãoCliente : contractLine.NºRequisiçãoDoCliente;
                                item.NoCompromisso = Reqcontract != null ? Reqcontract.NºCompromisso : contractLine.NºCompromisso;
                                item.DataRececaoRequisicao = Reqcontract != null ? Reqcontract.DataRequisição : contractLine.DataReceçãoRequisição;
                                item.NºContrato = contractLine.NºDeContrato;
                                item.CódigoRegião = contractLine.CódigoRegião;
                                item.CódigoÁreaFuncional = contractLine.CódigoÁreaFuncional;
                                item.CódigoCentroResponsabilidade = contractLine.CódigoCentroResponsabilidade;

                                //AMARO DUEDATE = DataExpiração
                                item.DataDeExpiração = DataDocumento;
                                string CodTermosPagamento = contractLine.CódTermosPagamento;
                                if (!string.IsNullOrEmpty(contractLine.CódTermosPagamento))
                                {
                                    NAVPaymentTermsViewModels PaymentTerms = DBNAV2017PaymentTerms.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, contractLine.CódTermosPagamento).FirstOrDefault();
                                    if (PaymentTerms != null)
                                    {
                                        string AUXDueDateCalculation = PaymentTerms.DueDateCalculation;
                                        AUXDueDateCalculation = AUXDueDateCalculation.Substring(0, AUXDueDateCalculation.IndexOf(''));

                                        if (!string.IsNullOrEmpty(AUXDueDateCalculation) && Double.TryParse(AUXDueDateCalculation, out double num))
                                        {
                                            item.DataDeExpiração = DataDocumento.AddDays(Convert.ToDouble(AUXDueDateCalculation));
                                        }
                                    }
                                }

                                if (contractLine.FaturaPrecosIvaIncluido == true)
                                    PricesIncludingVAT = true;
                                else
                                    PricesIncludingVAT = false;

                                //AMARO Ship to Address = Endereços de Envio
                                string Ship_to_Code = string.Empty;
                                Contratos cont = DBContracts.GetByIdLastVersion(item.NºContrato);
                                NAVClientsViewModel Cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºCliente);

                                if (cont != null && !string.IsNullOrEmpty(cont.CódEndereçoEnvio))
                                {
                                    item.NºCliente = !string.IsNullOrEmpty(cont.NºCliente) ? cont.NºCliente : "";
                                    Ship_to_Code = !string.IsNullOrEmpty(cont.CódEndereçoEnvio) ? cont.CódEndereçoEnvio : "";
                                }
                                else
                                {
                                    if (Cliente != null)
                                    {
                                        item.NºCliente = !string.IsNullOrEmpty(Cliente.No_) ? Cliente.No_ : "";
                                        Ship_to_Code = "";
                                    }
                                }

                                //AMARO Clientes Internos
                                if (Cliente != null && Cliente.InternalClient == true)
                                {
                                    item.CódigoRegião = Cliente.RegionCode;
                                    item.CódigoÁreaFuncional = Cliente.FunctionalAreaCode;
                                    item.CódigoCentroResponsabilidade = Cliente.ResponsabilityCenterCode;
                                }

                                //Contratos Tipo Quotas
                                string MetdoPagamento = "";
                                if (cont != null && cont.Tipo == 3)
                                {
                                    MetdoPagamento = !string.IsNullOrEmpty(cont.CódFormaPagamento) ? cont.CódFormaPagamento : DBConfiguracaoParametros.GetByParametro("QuotasMetPagamento").Valor;
                                    item.Descrição = item.Descrição.Replace("<MES>", StContractDate.ToString("MMMM").ToUpper());
                                    item.Descrição = item.Descrição.Replace("<ANO>", StContractDate.Year.ToString());
                                }

                                InvoiceBorrowed = GetTextoFatura(StContractDate, Lastdate);

                                //1
                                Task<WSCreatePreInvoice.Create_Result> InvoiceHeader = WSPreInvoice.CreateContractInvoice(item, _configws, ContractInvoicePeriod, InvoiceBorrowed, CodTermosPagamento, MetdoPagamento, PricesIncludingVAT, Ship_to_Code);
                                InvoiceHeader.Wait();

                                if (InvoiceHeader.IsCompletedSuccessfully && InvoiceHeader != null && InvoiceHeader.Result != null)
                                {
                                    if (Reqcontract != null)
                                    {
                                        Reqcontract.DataÚltimaFatura = Lastdate;
                                        DBContractClientRequisition.Update(Reqcontract);
                                    }
                                    else
                                    {
                                        contractLine.ÚltimaDataFatura = Lastdate;
                                        DBContracts.Update(contractLine);
                                    }

                                    String InvoiceHeaderNo = InvoiceHeader.Result.WSPreInvoice.No;
                                    List<LinhasFaturaçãoContrato> itemList = lineList.Where(x => x.NºContrato == item.NºContrato && x.GrupoFatura == item.GrupoFatura).ToList();

                                    if (itemList.Count > 0)
                                    {
                                        if (contractLine.FaturaPrecosIvaIncluido == true)
                                        {
                                            string NoCliente = string.Empty;
                                            string GrupoIVA = string.Empty;
                                            string GrupoCliente = string.Empty;
                                            decimal IVA = new decimal();
                                            foreach (var linha in itemList)
                                            {
                                                NoCliente = contractLine.NºCliente;
                                                GrupoIVA = string.Empty;
                                                GrupoCliente = string.Empty;
                                                IVA = 0;

                                                if (!string.IsNullOrEmpty(linha.Código))
                                                {
                                                    NAVResourcesViewModel Resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, linha.Código, "", 0, "").FirstOrDefault();
                                                    if (Resource != null)
                                                        GrupoIVA = Resource.VATProductPostingGroup;
                                                    else
                                                    {
                                                        execDetails += " Erro ao contabilizar: Não foi possível encontrar o recurso Nº: " + linha.Código + " para o Nº de contrato: " + contractLine.NºContrato;
                                                        result.eReasonCode = 2;
                                                        result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails));
                                                        return Json(result);
                                                    }

                                                    if (!string.IsNullOrEmpty(NoCliente) && !string.IsNullOrEmpty(GrupoIVA))
                                                    {
                                                        NAVClientsViewModel cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, NoCliente);
                                                        if (cliente != null)
                                                            GrupoCliente = cliente.VATBusinessPostingGroup;
                                                        else
                                                        {
                                                            execDetails += " Erro ao contabilizar: Não foi possível encontrar o cliente Nº: " + Cliente + " para o Nº de contrato: " + contractLine.NºContrato;
                                                            result.eReasonCode = 2;
                                                            result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails));
                                                            return Json(result);
                                                        }

                                                        if (!string.IsNullOrEmpty(GrupoCliente))
                                                            IVA = DBNAV2017VATPostingSetup.GetIVA(_config.NAVDatabaseName, _config.NAVCompanyName, GrupoCliente, GrupoIVA);

                                                        if (IVA > 0)
                                                            IVA = (IVA / 100) + 1;
                                                    }
                                                }

                                                if (IVA > 0 && i == 1)
                                                    linha.PreçoUnitário = linha.PreçoUnitário * IVA;

                                                linha.Data_Inicio_Serv_Prestado = StContractDate;
                                                linha.Data_Fim_Serv_Prestado = Lastdate;

                                                if (cont != null && cont.Tipo == 3)
                                                {
                                                    if (FirstTimeLine == true)
                                                    {
                                                        LinhaDescricaoOriginal = linha.Descrição;
                                                        LinhaDescricao2Original = linha.Descricao2;
                                                        FirstTimeLine = false;
                                                    }
                                                    else
                                                    {
                                                        linha.Descrição = LinhaDescricaoOriginal;
                                                        linha.Descricao2 = LinhaDescricao2Original;
                                                    }

                                                    linha.Descrição = linha.Descrição.Replace("<MES>", StContractDate.ToString("MMMM").ToUpper());
                                                    linha.Descrição = linha.Descrição.Replace("<ANO>", StContractDate.Year.ToString());
                                                    linha.Descricao2 = linha.Descricao2.Replace("<MES>", StContractDate.ToString("MMMM").ToUpper());
                                                    linha.Descricao2 = linha.Descricao2.Replace("<ANO>", StContractDate.Year.ToString());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (var linha in itemList)
                                            {
                                                linha.Data_Inicio_Serv_Prestado = StContractDate;
                                                linha.Data_Fim_Serv_Prestado = Lastdate;

                                                if (cont != null && cont.Tipo == 3)
                                                {
                                                    if (FirstTimeLine == true)
                                                    {
                                                        LinhaDescricaoOriginal = linha.Descrição;
                                                        LinhaDescricao2Original = linha.Descricao2;
                                                        FirstTimeLine = false;
                                                    }
                                                    else
                                                    {
                                                        linha.Descrição = LinhaDescricaoOriginal;
                                                        linha.Descricao2 = LinhaDescricao2Original;
                                                    }

                                                    linha.Descrição = linha.Descrição.Replace("<MES>", StContractDate.ToString("MMMM").ToUpper());
                                                    linha.Descrição = linha.Descrição.Replace("<ANO>", StContractDate.Year.ToString());

                                                    linha.Descricao2 = linha.Descricao2.Replace("<MES>", StContractDate.ToString("MMMM").ToUpper());
                                                    linha.Descricao2 = linha.Descricao2.Replace("<ANO>", StContractDate.Year.ToString());
                                                }
                                            }
                                        }

                                        try
                                        {
                                            Task<WSCreatePreInvoiceLine.CreateMultiple_Result> InvoiceLines = WSPreInvoiceLine.CreatePreInvoiceLineList(itemList, InvoiceHeaderNo, InvoiceBorrowed, _configws);
                                            InvoiceLines.Wait();
                                        }
                                        catch (Exception ex)
                                        {
                                            //09/07/02019
                                            //A Pedido do Marco Marcelo quando o WebService devolve uma mensagem com o texto "maximum message size quota"
                                            //assume-se que o mesmo foi executado com sucesso.
                                            errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                                            if (errorMessage.ToLower().Contains("maximum message size quota".ToLower()))
                                            {
                                                return Json(true);
                                            }
                                            else
                                            {
                                                if (!hasErrors)
                                                    hasErrors = true;

                                                execDetails += " Erro ao criar as linhas: ";
                                                errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                                result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails + errorMessage));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    return Json(false);
                                }
                            }
                        }
                    }
                    //FIM DO NOVO PROCESSO
                }
            }
            if (result.eMessages.Count > 0)
            {
                return Json(result);
            }
            else
            {
                foreach (FaturacaoContratosViewModel toDelete in data)
                {
                    DBAuthorizeInvoiceContracts.DeleteByID(toDelete.ContractNo, (int)toDelete.InvoiceGroupValue);
                }

                return Json(true);
            }
        }
        #endregion

        #region Propostas
        public IActionResult Propostas(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Propostas);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesProposta(string id, string version = "", string isHistoric = "")
        {
            string hist = isHistoric;

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Propostas);
            if (UPerm != null && UPerm.Read.Value)
            {
                Contratos cContract = null;
                if (version != "")
                    cContract = DBContracts.GetByIdAndVersion(id, int.Parse(version));
                else
                    cContract = DBContracts.GetByIdLastVersion(id);

                if (hist == "true")
                {
                    ViewBag.Historic = "(Histórico)";
                }
                else
                {
                    ViewBag.Historic = "";
                }

                ViewBag.ID = hist == "true" ? 1 : 0;
                ViewBag.Archived = hist == "true" ? true : false;
                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.reportServerURL = _config.ReportServerURL;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetListContractsProposals([FromBody] JObject requestParams)
        {
            //int AreaId = int.Parse(requestParams["AreaId"].ToString());
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();
            int showLevel = int.Parse(requestParams["showLevel"].ToString());

            List<Contratos> ContractsList = null;

            if (ContractNo == "")
            {
                ContractsList = DBContracts.GetAllByContractType(ContractType.Proposal);

                if (Archived == 0)
                {
                    ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
                }
                else
                {
                    //ARQUIVO
                    ContractsList.RemoveAll(x => (x.Arquivado.HasValue && !x.Arquivado.Value) || (!x.Arquivado.HasValue));
                }
            }
            else
            {
                ContractsList = DBContracts.GetByNo(ContractNo, true);
            }

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);

            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));

            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional));

            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));

            List<ContractViewModel> result = new List<ContractViewModel>();

            if (showLevel == 2) //Canceladas
            {
                ContractsList.RemoveAll(x => !x.Estado.HasValue || x.Estado.Value != 5);
            }
            else if (showLevel == 3) //Perdidas
            {
                ContractsList.RemoveAll(x => !x.Estado.HasValue || x.Estado.Value != 4);
            }

            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<EnumData> status = EnumerablesFixed.ProposalsStatus;

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x)));
            result.ForEach(x =>
            {
                x.ClientName = !string.IsNullOrEmpty(x.ClientNo) ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault().Name : "" : "";
                x.StatusDescription = x.Status != null ? status.Where(y => y.Id == x.Status).FirstOrDefault() != null ? status.Where(y => y.Id == x.Status).FirstOrDefault().Value : "" : "";
                //x.CodeRegion = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name, x.CodeRegion).FirstOrDefault().Name ?? "";
                //x.CodeFunctionalArea = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name, x.CodeFunctionalArea).FirstOrDefault().Name ?? "";
                //x.CodeResponsabilityCenter = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name, x.CodeResponsabilityCenter).FirstOrDefault().Name ?? "";
            });

            return Json(result.OrderByDescending(x => x.ContractNo));
        }

        public JsonResult GetListContractsProposalsByCliente([FromBody] JObject requestParams)
        {
            //int AreaId = int.Parse(requestParams["AreaId"].ToString());

            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"] != null ? requestParams["ContractNo"].ToString() : "";
            string ClienteNo = requestParams["ClienteNo"] != null ? requestParams["ClienteNo"].ToString() : "";
            int showLevel = int.Parse(requestParams["showLevel"].ToString());

            List<Contratos> ContractsList = null;

            if (ContractNo == "")
            {
                ContractsList = DBContracts.GetAllByContractType(ContractType.Proposal);

                if (Archived == 0)
                {
                    ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
                }
                else
                {
                    //ARQUIVO
                    ContractsList.RemoveAll(x => (x.Arquivado.HasValue && !x.Arquivado.Value) || (!x.Arquivado.HasValue));
                }
            }
            else
            {
                ContractsList = DBContracts.GetByNo(ContractNo, true);
            }

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);

            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));

            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional));

            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));

            //Cliente
            if (!string.IsNullOrEmpty(ClienteNo))
                ContractsList.RemoveAll(x => x.NºCliente != ClienteNo);

            List<ContractViewModel> result = new List<ContractViewModel>();

            if (showLevel == 2) //Canceladas
            {
                ContractsList.RemoveAll(x => !x.Estado.HasValue || x.Estado.Value != 5);
            }
            else if (showLevel == 3) //Perdidas
            {
                ContractsList.RemoveAll(x => !x.Estado.HasValue || x.Estado.Value != 4);
            }

            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<EnumData> status = EnumerablesFixed.ProposalsStatus;

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x)));
            result.ForEach(x =>
            {
                x.ClientName = !string.IsNullOrEmpty(x.ClientNo) ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault().Name : "" : "";
                x.StatusDescription = x.Status != null ? status.Where(y => y.Id == x.Status).FirstOrDefault() != null ? status.Where(y => y.Id == x.Status).FirstOrDefault().Value : "" : "";
                //x.CodeRegion = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name, x.CodeRegion).FirstOrDefault().Name ?? "";
                //x.CodeFunctionalArea = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name, x.CodeFunctionalArea).FirstOrDefault().Name ?? "";
                //x.CodeResponsabilityCenter = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name, x.CodeResponsabilityCenter).FirstOrDefault().Name ?? "";
            });

            return Json(result.OrderByDescending(x => x.ContractNo));
        }

        public JsonResult GetListContractsProposalsById([FromBody] JObject requestParams)
        {
            string ContractNo = requestParams["ContractNo"].ToString();

            List<Contratos> ContractsList = null;
            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList = DBContracts.GetAllByContractProposalsNo(ContractNo);
            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x)));
            result.ForEach(x =>
            {
                x.ClientName = !string.IsNullOrEmpty(x.ClientNo) ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault().Name : "" : "";
            });

            return Json(result.OrderByDescending(x => x.ContractNo));
        }

        public JsonResult GetContractsProposalById([FromBody] string ContractNo)
        {
            Contratos ContractsList = null;
            bool haveProposals = false;

            ContractsList = DBContracts.GetContractProposalsNo(ContractNo);

            if (ContractsList != null)
            {
                haveProposals = true;
            }

            return Json(haveProposals);
        }

        public JsonResult GetListContractsAllProposals()
        {
            List<Contratos> ContractsList = DBContracts.GetAllByContractType(ContractType.Proposal);

            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x, _config.NAVDatabaseName, _config.NAVCompanyName)));

            List<EnumData> status = EnumerablesFixed.ProposalsStatus;
            result.ForEach(x => { x.StatusDescription = status.Where(y => y.Id == x.Status).Select(y => y.Value).FirstOrDefault(); });
            return Json(result);
        }

        public JsonResult GetProposalDetails([FromBody] JObject requestParams)
        {
            int contractVersion = int.Parse(requestParams["Version"].ToString());
            string contractNo = requestParams["ContractNo"].ToString();


            ContractViewModel result = DBContracts.ParseToViewModel(DBContracts.GetByIdAndVersion(contractNo, contractVersion), _config.NAVDatabaseName, _config.NAVCompanyName);

            List<EnumData> status = EnumerablesFixed.ProposalsStatus;
            result.StatusDescription = status.Where(y => y.Id == result.Status).Select(y => y.Value).FirstOrDefault();

            result.ContractNo = "";
            result.ContractReference = "";
            result.VersionNo = 1;
            result.OportunityNo = "";
            return Json(result);
        }

        [HttpPost]
        public JsonResult SetProposalStatus([FromBody] ContractViewModel item)
        {
            if (item != null)
            {
                ProposalsService serv = new ProposalsService(User.Identity.Name);
                item = serv.SetStatus(item);
            }
            return Json(item);
        }

        [HttpPost]
        public JsonResult CreateProposalFromContract([FromBody] JObject requestParams)
        {
            string contractId = requestParams["contractId"].ToString();
            int version = int.Parse(requestParams["versionNo"].ToString());
            string percentage = requestParams["percentageToApllyInLines"].ToString().Replace(".", ",");
            decimal percentageToApllyInLines = decimal.MinValue;

            if (!string.IsNullOrEmpty(percentage))
            {
                decimal.TryParse(percentage, out percentageToApllyInLines);
            }

            ErrorHandler result = new ErrorHandler();
            try
            {
                ProposalsService serv = new ProposalsService(User.Identity.Name);
                result = serv.CreateProposalFromContract(contractId, version, percentageToApllyInLines);
            }
            catch
            {
                result = new ErrorHandler()
                {
                    eReasonCode = 2,
                    eMessage = "Ocorreu um erro ao criar a proposta",
                };
            }
            return Json(result);
        }

        public IActionResult PropostasContrato(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Propostas);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.ContractNo = string.IsNullOrEmpty(id) ? string.Empty : id;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult OpenProposta([FromBody] ContractViewModel Proposta)
        {
            if (Proposta != null)
            {
                if (Proposta.Status != 1 && (Proposta.OldStatus == null || Proposta.OldStatus == -1))
                {
                    Proposta.OldStatus = Proposta.Status;
                    Proposta.Status = 1; //"Aberta"
                    Proposta.UpdateUser = User.Identity.Name;

                    if (DBContracts.Update(DBContracts.ParseToDB(Proposta)) != null)
                    {
                        Proposta.eReasonCode = 1;
                        Proposta.eMessage = "A Proposta foi aberta com sucesso.";
                    }
                    else
                    {
                        Proposta.eReasonCode = 2;
                        Proposta.eMessage = "Ocorreu um erro ao atualizar a Proposta.";
                    }
                }
                else
                {
                    Proposta.eReasonCode = 3;
                    Proposta.eMessage = "Não é permitido abrir a Proposta, pois a Proposta já está Aberta.";
                }
            }
            else
            {
                Proposta.eReasonCode = 4;
                Proposta.eMessage = "Não foi possível obter os dados da Proposta.";
            }

            return Json(Proposta);
        }

        [HttpPost]
        public JsonResult CloseProposta([FromBody] ContractViewModel Proposta)
        {
            if (Proposta != null)
            {
                if (Proposta.OldStatus >= 0)
                {
                    Proposta.Status = Proposta.OldStatus;
                    Proposta.OldStatus = -1; //FECHADA
                    Proposta.UpdateUser = User.Identity.Name;

                    if (DBContracts.Update(DBContracts.ParseToDB(Proposta)) != null)
                    {
                        Proposta.eReasonCode = 1;
                        Proposta.eMessage = "A Proposta foi fechada com sucesso.";
                    }
                    else
                    {
                        Proposta.eReasonCode = 2;
                        Proposta.eMessage = "Ocorreu um erro ao atualizar a Proposta.";
                    }
                }
                else
                {
                    Proposta.eReasonCode = 3;
                    Proposta.eMessage = "Não é permitido fechar a Proposta, pois a Proposta não foi Aberta.";
                }
            }
            else
            {
                Proposta.eReasonCode = 4;
                Proposta.eMessage = "Não foi possível obter os dados da Proposta.";
            }

            return Json(Proposta);
        }

        #endregion

        #region Invoice

        public DateTime getDatePeriodPayment(DateTime startDate, int payPeriod)
        {
            DateTime lastDate;
            if (payPeriod == 1)
            {
                lastDate = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1).AddDays(-1);
            }
            else if (payPeriod == 2)
            {
                startDate = startDate.AddMonths(1);
                lastDate = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1).AddDays(-1);
            }
            else if (payPeriod == 3)
            {
                startDate = startDate.AddMonths(2);
                lastDate = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1).AddDays(-1);
            }
            else if (payPeriod == 4)
            {
                startDate = startDate.AddMonths(5);
                lastDate = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1).AddDays(-1);
            }
            else if (payPeriod == 5)
            {
                lastDate = new DateTime(startDate.Year, 12, 31);
            }
            else
            {
                lastDate = startDate;
            }
            return lastDate;
        }

        [HttpPost]
        public JsonResult CreateInvoiceHeaderFromContract([FromBody] JObject requestParams, string dateCont)
        {
            bool registado = false;
            ErrorHandler result = new ErrorHandler();
            ContractViewModel Contract = null;
            List<int> groups = new List<int>();
            int Codgroup = 0;
            string obs = "";
            string execDetails = string.Empty;
            string errorMessage = string.Empty;
            bool hasErrors = false;
            DateTime DataInicioFatura = DateTime.MinValue;
            DateTime DataFimFatura = DateTime.MinValue;
            RequisiçõesClienteContrato ClientRequisition = new RequisiçõesClienteContrato();
            List<RequisiçõesClienteContrato> ClientRequisitionByContract = new List<RequisiçõesClienteContrato>();
            List<RequisiçõesClienteContrato> ClientRequisitionByContractAndGroup = new List<RequisiçõesClienteContrato>();
            List<RequisiçõesClienteContrato> ClientRequisitionByContractAndGroupAndDate = new List<RequisiçõesClienteContrato>();


            if (requestParams["Contrato"].ToString() != null && requestParams["LinhasContrato"].ToString() != null)
            {
                DateTime lastDay = Convert.ToDateTime(dateCont);
                Contract = JsonConvert.DeserializeObject<ContractViewModel>(requestParams["Contrato"].ToString());
                List<ContractLineViewModel> ContractLines = JsonConvert.DeserializeObject<List<ContractLineViewModel>>(requestParams["LinhasContrato"].ToString());
                string groupInvoice = requestParams["GrupoFatura"].ToString();

                bool createGroup = true;
                if (groupInvoice != null && groupInvoice != "")
                {
                    Codgroup = Convert.ToInt32(groupInvoice);
                    if (ContractLines.Find(x => x.InvoiceGroup == Codgroup) == null)
                    {
                        createGroup = false;
                    }
                }

                if (createGroup == true)
                {
                    //Create Project if existe
                    ConfigUtilizadores userConfig = DBUserConfigurations.GetById(User.Identity.Name);
                    Task<WSCreateNAVProject.Read_Result> Project = WSProject.GetNavProject(Contract.ContractNo, _configws);
                    Project.Wait();
                    if (Project.IsCompletedSuccessfully && Project.Result.WSJob == null)
                    {
                        ProjectDetailsViewModel proj = new ProjectDetailsViewModel();
                        proj.ProjectNo = Contract.ContractNo;
                        proj.ClientNo = Contract.ClientNo;
                        proj.Status = EstadoProjecto.Encomenda;
                        proj.RegionCode = Contract.CodeRegion;
                        proj.ResponsabilityCenterCode = Contract.CodeResponsabilityCenter;
                        proj.FunctionalAreaCode = Contract.CodeFunctionalArea;
                        proj.Description = Contract.Description;
                        proj.Visivel = false;
                        try
                        {
                            Task<WSCreateNAVProject.Create_Result> createProject = WSProject.CreateNavProject(proj, _configws);
                            createProject.Wait();
                        }
                        catch (Exception ex)
                        {
                            result.eReasonCode = 3;
                            result.eMessage = "Ocorreu um erro ao criar projeto: " + ex.Message;
                            return Json(result);
                        }
                    }

                    //Escolheu Grupo de fatura = Só trata de 1 grupo
                    if (groupInvoice != null && groupInvoice != "")
                    {
                        //Obter a Data de Fim para a nova fatura
                        ClientRequisitionByContract = DBContractClientRequisition.GetAll().Where(x => x.NºContrato == Contract.ContractNo).ToList();
                        if (ClientRequisitionByContract == null || ClientRequisitionByContract.Count() == 0)
                        {
                            DataFimFatura = !string.IsNullOrEmpty(Contract.LastInvoiceDate) ? Convert.ToDateTime(Contract.LastInvoiceDate) : !string.IsNullOrEmpty(Contract.ContractStartDate) ? Convert.ToDateTime(Contract.ContractStartDate) : DateTime.MinValue;
                        }
                        else
                        {
                            ClientRequisitionByContractAndGroup = DBContractClientRequisition.GetAll().Where(x => x.NºContrato == Contract.ContractNo && x.GrupoFatura == int.Parse(groupInvoice)).ToList();
                            if (ClientRequisitionByContractAndGroup != null && ClientRequisitionByContractAndGroup.Count() > 0)
                            {
                                DataFimFatura = ClientRequisitionByContractAndGroup.OrderByDescending(x => x.DataÚltimaFatura).FirstOrDefault().DataÚltimaFatura.HasValue ? (DateTime)ClientRequisitionByContractAndGroup.OrderByDescending(x => x.DataÚltimaFatura).FirstOrDefault().DataÚltimaFatura : DateTime.MinValue;
                                if (DataFimFatura == DateTime.MinValue)
                                {
                                    //ORIGINAL
                                    //DataFimFatura = !string.IsNullOrEmpty(Contract.StartData) ? Convert.ToDateTime(Contract.StartData) : DateTime.MinValue;
                                    DataFimFatura = ClientRequisitionByContractAndGroup.OrderByDescending(x => x.DataÚltimaFatura).FirstOrDefault().DataInícioCompromisso;
                                }
                            }
                        }

                        if (DataFimFatura != DateTime.MinValue)
                        {
                            DataInicioFatura = DataFimFatura.AddDays(1);
                            if (DataFimFatura.Day == 1)
                            {
                                DataFimFatura = getDatePeriodPayment(DataFimFatura, Contract.InvocePeriod ?? 0);

                            }
                            else
                            {
                                DataFimFatura = new DateTime(DataFimFatura.Year, DataFimFatura.Month, 1).AddMonths(1);
                                DataFimFatura = getDatePeriodPayment(DataFimFatura, Contract.InvocePeriod ?? 0);
                            }

                            if (ClientRequisitionByContractAndGroup != null && ClientRequisitionByContractAndGroup.Count() > 0)
                            {
                                ClientRequisitionByContractAndGroupAndDate = DBContractClientRequisition.GetAll().Where(x => x.NºContrato == Contract.ContractNo && x.GrupoFatura == int.Parse(groupInvoice) && x.DataInícioCompromisso <= DataFimFatura && x.DataFimCompromisso >= DataFimFatura).ToList();

                                if (ClientRequisitionByContractAndGroupAndDate == null || ClientRequisitionByContractAndGroupAndDate.Count() == 0)
                                {
                                    result.eReasonCode = 3;
                                    result.eMessage = "Não existem Requisições de Cliente válidas para a data da fatura.";
                                    return Json(result);
                                }
                            }
                        }
                        else
                        {
                            result.eReasonCode = 3;
                            result.eMessage = "Não foi encontrada uma data de inicio para a para o grupo de fatura escolhido.";
                            return Json(result);
                        }
                        Contract.LastInvoiceDate = DataFimFatura.ToString("dd/MM/yyyy");


                        //CREATE SALES HEADER
                        NAVSalesHeaderViewModel PreInvoiceToCreate = new NAVSalesHeaderViewModel();
                        PreInvoiceToCreate.PeriododeFact_Contrato = DataInicioFatura.ToString("dd/MM/yyyy") + " a " + DataFimFatura.ToString("dd/MM/yyyy");

                        //string mes = DataFimFatura.ToString("MMMM");
                        //if (Contract.Type != 3)
                        //    PreInvoiceToCreate.DataServ_Prestado = String.Format("{0}/{1}", mes.ToUpper(), DataFimFatura.Year);
                        //else
                        //{
                        //    string TextoFatura = Contract.TextoFatura;
                        //    if (!string.IsNullOrEmpty(TextoFatura))
                        //    {
                        //        TextoFatura = TextoFatura.Replace("<MES>", mes.ToUpper());
                        //        TextoFatura = TextoFatura.Replace("<ANO>", DataFimFatura.Year.ToString());
                        //        PreInvoiceToCreate.DataServ_Prestado = TextoFatura;
                        //    }
                        //}
                        PreInvoiceToCreate.DataServ_Prestado = GetTextoFatura(DataInicioFatura, DataFimFatura);

                        PreInvoiceToCreate.Sell_toCustomerNo = Contract.ClientNo;
                        PreInvoiceToCreate.DocumentDate = lastDay;
                        if (Contract.CustomerShipmentDate != null && Contract.CustomerShipmentDate != "")
                            PreInvoiceToCreate.ShipmentDate = DateTime.Parse(Contract.CustomerShipmentDate);

                        PreInvoiceToCreate.ValorContrato = Contract.TotalValue ?? 0;
                        PreInvoiceToCreate.Ship_toCode = !string.IsNullOrEmpty(Contract.CodeShippingAddress) ? Contract.CodeShippingAddress : "";
                        //PreInvoiceToCreate.Ship_toAddress = Contract.ShippingAddress;
                        //PreInvoiceToCreate.Ship_toPostCode = Contract.ShippingZipCode;
                        PreInvoiceToCreate.DueDate = lastDay;

                        NAVPaymentTermsViewModels PaymentTerms = DBNAV2017PaymentTerms.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, Contract.CodePaymentTerms).FirstOrDefault();
                        if (PaymentTerms != null)
                        {
                            string AUXDueDateCalculation = PaymentTerms.DueDateCalculation;
                            AUXDueDateCalculation = AUXDueDateCalculation.Substring(0, AUXDueDateCalculation.IndexOf(''));

                            if (!string.IsNullOrEmpty(AUXDueDateCalculation) && Double.TryParse(AUXDueDateCalculation, out double num))
                            {
                                PreInvoiceToCreate.DueDate = lastDay.AddDays(Convert.ToDouble(AUXDueDateCalculation));
                            }
                        }
                        PreInvoiceToCreate.PaymentTermsCode = Contract.CodePaymentTerms;

                        PreInvoiceToCreate.No_Compromisso = Contract.PromiseNo;
                        PreInvoiceToCreate.CodigoPedido = Contract.ClientRequisitionNo;
                        if (Contract.ReceiptDateRequisition != null && Contract.ReceiptDateRequisition != "")
                            PreInvoiceToCreate.DataEncomenda = DateTime.Parse(Contract.ReceiptDateRequisition);

                        PreInvoiceToCreate.ContractNo = Contract.ContractNo;
                        PreInvoiceToCreate.FacturaCAF = false;
                        PreInvoiceToCreate.Userpreregisto2009 = User.Identity.Name;
                        PreInvoiceToCreate.PostingDate = lastDay;
                        PreInvoiceToCreate.ResponsabilityCenterCode20 = Contract.CodeResponsabilityCenter;
                        PreInvoiceToCreate.FunctionAreaCode20 = Contract.CodeFunctionalArea;
                        PreInvoiceToCreate.RegionCode20 = Contract.CodeRegion;
                        PreInvoiceToCreate.ResponsibilityCenter = userConfig.CentroDeResponsabilidade;
                        PreInvoiceToCreate.PostingNoSeries = userConfig.NumSerieFaturas;
                        PreInvoiceToCreate.PaymentMethodCode = !string.IsNullOrEmpty(Contract.CodePaymentMethod) ? Contract.CodePaymentMethod : "";

                        Codgroup = Convert.ToInt32(groupInvoice);
                        if (ContractLines.Find(x => x.InvoiceGroup == Codgroup) != null)
                        {
                            foreach (ContractInvoiceTextViewModel texts in Contract.InvoiceTexts)
                            {
                                if (texts.InvoiceGroup == Convert.ToInt32(groupInvoice))
                                {
                                    obs += texts.InvoiceText;
                                }
                            }
                            if (!string.IsNullOrEmpty(obs))
                            {
                                PreInvoiceToCreate.Observacoes = !string.IsNullOrEmpty(obs) ? obs : "";
                            }
                            else
                            {
                                PreInvoiceToCreate.Observacoes = !string.IsNullOrEmpty(Contract.TextoFatura) ? Contract.TextoFatura : "";
                            }
                            obs = "";

                            //Obter os Campos das Requisições de Cliente
                            ClientRequisition = DBContractClientRequisition.GetAll().Where(x => x.NºContrato == Contract.ContractNo && x.GrupoFatura == Codgroup && x.DataInícioCompromisso <= Convert.ToDateTime(Contract.LastInvoiceDate) && x.DataFimCompromisso >= Convert.ToDateTime(Contract.LastInvoiceDate)).FirstOrDefault();
                            if (ClientRequisition != null)
                            {
                                PreInvoiceToCreate.No_Compromisso = !string.IsNullOrEmpty(ClientRequisition.NºCompromisso) ? ClientRequisition.NºCompromisso : "";
                                if (ClientRequisition.DataRequisição != null)
                                    PreInvoiceToCreate.DataEncomenda = (DateTime)ClientRequisition.DataRequisição;
                                PreInvoiceToCreate.CodigoPedido = !string.IsNullOrEmpty(ClientRequisition.NºRequisiçãoCliente) ? ClientRequisition.NºRequisiçãoCliente : "";
                            }

                            //AMARO Clientes Internos
                            NAVClientsViewModel ClienteNAV = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, Contract.ClientNo);
                            if (ClienteNAV != null && ClienteNAV.InternalClient == true)
                            {
                                PreInvoiceToCreate.RegionCode20 = ClienteNAV.RegionCode;
                                PreInvoiceToCreate.FunctionAreaCode20 = ClienteNAV.FunctionalAreaCode;
                                PreInvoiceToCreate.ResponsabilityCenterCode20 = ClienteNAV.ResponsabilityCenterCode;
                            }
                            
                            //Contratos Tipo Quotas
                            if (Contract != null && Contract.Type == 3)
                            {
                                PreInvoiceToCreate.PaymentMethodCode = !string.IsNullOrEmpty(Contract.CodePaymentMethod) ? Contract.CodePaymentMethod : DBConfiguracaoParametros.GetByParametro("QuotasMetPagamento").Valor;
                            }

                            Task<WSCreatePreInvoice.Create_Result> InvoiceHeader = WSPreInvoice.CreatePreInvoiceHeader(PreInvoiceToCreate, _configws);//, Codgroup);
                            InvoiceHeader.Wait();

                            if (InvoiceHeader.IsCompletedSuccessfully && InvoiceHeader.Result != null)
                            {

                                string cod = InvoiceHeader.Result.WSPreInvoice.No;
                                List<LinhasFaturaçãoContrato> LinhasFaturacao = new List<LinhasFaturaçãoContrato>();
                                foreach (ContractLineViewModel line in ContractLines)
                                {
                                    //CREATE SALES LINES
                                    if (line.Billable == true && Codgroup == line.InvoiceGroup)
                                    {
                                        if (line.Quantity == null || line.Quantity == 0)
                                            line.Quantity = 1;

                                        LinhasFaturaçãoContrato PreInvoiceLinesToCreate = new LinhasFaturaçãoContrato();
                                        PreInvoiceLinesToCreate.Tipo = line.Type.Value.ToString();
                                        PreInvoiceLinesToCreate.Código = line.Code;
                                        PreInvoiceLinesToCreate.Descrição = line.Description;
                                        PreInvoiceLinesToCreate.Descricao2 = !string.IsNullOrEmpty(line.Description2) && line.Description2.Length > 50 ? line.Description2.Substring(0, 49) : !string.IsNullOrEmpty(line.Description2) ? line.Description2 : "";
                                        PreInvoiceLinesToCreate.CódUnidadeMedida = line.CodeMeasureUnit;
                                        PreInvoiceLinesToCreate.CódigoÁreaFuncional = line.CodeFunctionalArea;
                                        PreInvoiceLinesToCreate.CódigoRegião = line.CodeRegion;
                                        PreInvoiceLinesToCreate.CódigoCentroResponsabilidade = line.CodeResponsabilityCenter;
                                        PreInvoiceLinesToCreate.NºContrato = Contract.ContractNo;
                                        PreInvoiceLinesToCreate.NºProjeto = Contract.ContractNo;
                                        PreInvoiceLinesToCreate.CódigoServiço = line.ServiceClientNo;
                                        PreInvoiceLinesToCreate.Quantidade = line.Quantity * (Contract.InvocePeriod == 6 ? 0 : Contract.InvocePeriod == 5 ? 12 : Contract.InvocePeriod == 4 ? 6 : Contract.InvocePeriod);
                                        PreInvoiceLinesToCreate.PreçoUnitário = line.UnitPrice;
                                        PreInvoiceLinesToCreate.Data_Inicio_Serv_Prestado = DataInicioFatura;
                                        PreInvoiceLinesToCreate.Data_Fim_Serv_Prestado = DataFimFatura;
                                        LinhasFaturacao.Add(PreInvoiceLinesToCreate);
                                    }
                                }

                                if (Contract.FaturaPrecosIvaIncluido == true)
                                {
                                    string Cliente = string.Empty;
                                    string GrupoIVA = string.Empty;
                                    string GrupoCliente = string.Empty;
                                    decimal IVA = new decimal();
                                    foreach (var item in LinhasFaturacao)
                                    {
                                        Cliente = Contract.ClientNo;
                                        GrupoIVA = string.Empty;
                                        GrupoCliente = string.Empty;
                                        IVA = 0;

                                        if (!string.IsNullOrEmpty(item.Código))
                                        {
                                            NAVResourcesViewModel Resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, item.Código, "", 0, "").FirstOrDefault();
                                            if (Resource != null)
                                                GrupoIVA = Resource.VATProductPostingGroup;
                                            else
                                            {
                                                execDetails += " Erro ao criar a fatura: Não foi possível encontrar o recurso Nº: " + item.Código;
                                                result.eReasonCode = 2;
                                                result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails));
                                                return Json(result);
                                            }

                                            if (!string.IsNullOrEmpty(Cliente) && !string.IsNullOrEmpty(GrupoIVA))
                                            {
                                                NAVClientsViewModel cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, Cliente);
                                                if (cliente != null)
                                                    GrupoCliente = cliente.VATBusinessPostingGroup;
                                                else
                                                {
                                                    execDetails += " Erro ao criar a fatura: Não foi possível encontrar o cliente Nº: " + Cliente;
                                                    result.eReasonCode = 2;
                                                    result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails));
                                                    return Json(result);
                                                }

                                                if (!string.IsNullOrEmpty(GrupoCliente))
                                                    IVA = DBNAV2017VATPostingSetup.GetIVA(_config.NAVDatabaseName, _config.NAVCompanyName, GrupoCliente, GrupoIVA);

                                                if (IVA > 0)
                                                    IVA = (IVA / 100) + 1;
                                            }
                                        }

                                        if (IVA > 0)
                                            item.PreçoUnitário = item.PreçoUnitário * IVA;
                                    }
                                }

                                try
                                {
                                    Task<WSCreatePreInvoiceLine.CreateMultiple_Result> InvoiceLines = WSPreInvoiceLine.CreatePreInvoiceLineList(LinhasFaturacao, cod, _configws);
                                    InvoiceLines.Wait();
                                    if (InvoiceLines.IsCompletedSuccessfully)
                                    {
                                        registado = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (!ex.Message.ToLower().Contains("maximum message size quota".ToLower()))
                                    {
                                        if (!hasErrors)
                                            hasErrors = true;

                                        execDetails += " Erro ao criar as linhas: ";
                                        errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                        result.eReasonCode = 2;
                                        result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails + errorMessage));
                                    }
                                    else
                                    {
                                        registado = true;
                                    }
                                }
                            }
                            else
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "Não foi possivel obter dados do NAV na criação da fatura.";
                                return Json(result);
                            }
                        }
                    }
                    else
                    {
                        //Não escolheu grupo de fatura = TODOS OS GRUPOS
                        foreach (ContractLineViewModel line in ContractLines)
                        {
                            if (!line.InvoiceGroup.HasValue)
                                line.InvoiceGroup = 0;

                            if (!groups.Contains(line.InvoiceGroup.Value))
                            {
                                groups.Add(line.InvoiceGroup ?? 0);
                            }
                        }

                        foreach (int group in groups)
                        {
                            //Obter a Data de Fim para a nova fatura
                            ClientRequisitionByContract = DBContractClientRequisition.GetAll().Where(x => x.NºContrato == Contract.ContractNo).ToList();
                            if (ClientRequisitionByContract == null || ClientRequisitionByContract.Count() == 0)
                            {
                                DataFimFatura = !string.IsNullOrEmpty(Contract.LastInvoiceDate) ? Convert.ToDateTime(Contract.LastInvoiceDate) : !string.IsNullOrEmpty(Contract.ContractStartDate) ? Convert.ToDateTime(Contract.ContractStartDate) : DateTime.MinValue;
                            }
                            else
                            {
                                ClientRequisitionByContractAndGroup = DBContractClientRequisition.GetAll().Where(x => x.NºContrato == Contract.ContractNo && x.GrupoFatura == group).ToList();
                                if (ClientRequisitionByContractAndGroup != null && ClientRequisitionByContractAndGroup.Count() > 0)
                                {
                                    DataFimFatura = ClientRequisitionByContractAndGroup.OrderByDescending(x => x.DataÚltimaFatura).FirstOrDefault().DataÚltimaFatura.HasValue ? (DateTime)ClientRequisitionByContractAndGroup.OrderByDescending(x => x.DataÚltimaFatura).FirstOrDefault().DataÚltimaFatura : DateTime.MinValue;
                                    if (DataFimFatura == DateTime.MinValue)
                                    {
                                        //ORIGINAL
                                        //DataFimFatura = !string.IsNullOrEmpty(Contract.StartData) ? Convert.ToDateTime(Contract.StartData) : DateTime.MinValue;
                                        DataFimFatura = ClientRequisitionByContractAndGroup.OrderByDescending(x => x.DataÚltimaFatura).FirstOrDefault().DataInícioCompromisso;
                                    }
                                }
                            }

                            if (DataFimFatura != DateTime.MinValue)
                            {
                                DataInicioFatura = DataFimFatura.AddDays(1);
                                if (DataFimFatura.Day == 1)
                                {
                                    DataFimFatura = getDatePeriodPayment(DataFimFatura, Contract.InvocePeriod ?? 0);

                                }
                                else
                                {
                                    DataFimFatura = new DateTime(DataFimFatura.Year, DataFimFatura.Month, 1).AddMonths(1);
                                    DataFimFatura = getDatePeriodPayment(DataFimFatura, Contract.InvocePeriod ?? 0);
                                }

                                if (ClientRequisitionByContractAndGroup != null && ClientRequisitionByContractAndGroup.Count() > 0)
                                {
                                    ClientRequisitionByContractAndGroupAndDate = DBContractClientRequisition.GetAll().Where(x => x.NºContrato == Contract.ContractNo && x.GrupoFatura == group && x.DataInícioCompromisso <= DataFimFatura && x.DataFimCompromisso >= DataFimFatura).ToList();

                                    if (ClientRequisitionByContractAndGroupAndDate == null || ClientRequisitionByContractAndGroupAndDate.Count() == 0)
                                    {
                                        result.eReasonCode = 3;
                                        result.eMessage = "Não existem Requisições de Cliente válidas para a data da fatura.";
                                        return Json(result);
                                    }
                                }
                            }
                            else
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "Não foi encontrada uma data de inicio para a para o grupo de fatura escolhido.";
                                return Json(result);
                            }
                            Contract.LastInvoiceDate = DataFimFatura.ToString("dd/MM/yyyy");


                            //CREATE SALES HEADER
                            NAVSalesHeaderViewModel PreInvoiceToCreate = new NAVSalesHeaderViewModel();
                            PreInvoiceToCreate.PeriododeFact_Contrato = DataInicioFatura.ToString("dd/MM/yyyy") + " a " + DataFimFatura.ToString("dd/MM/yyyy");
                            string mes = DataFimFatura.ToString("MMMM");
                            //PreInvoiceToCreate.DataServ_Prestado = String.Format("{0}/{1}", mes.ToUpper(), DataFimFatura.Year);
                            PreInvoiceToCreate.DataServ_Prestado = GetTextoFatura(DataInicioFatura, DataFimFatura);

                            PreInvoiceToCreate.Sell_toCustomerNo = Contract.ClientNo;
                            PreInvoiceToCreate.DocumentDate = lastDay;
                            if (Contract.CustomerShipmentDate != null && Contract.CustomerShipmentDate != "")
                                PreInvoiceToCreate.ShipmentDate = DateTime.Parse(Contract.CustomerShipmentDate);

                            PreInvoiceToCreate.ValorContrato = Contract.TotalValue ?? 0;
                            PreInvoiceToCreate.Ship_toCode = !string.IsNullOrEmpty(Contract.CodeShippingAddress) ? Contract.CodeShippingAddress : "";
                            //PreInvoiceToCreate.Ship_toAddress = Contract.ShippingAddress;
                            //PreInvoiceToCreate.Ship_toPostCode = Contract.ShippingZipCode;
                            PreInvoiceToCreate.DueDate = lastDay;

                            NAVPaymentTermsViewModels PaymentTerms = DBNAV2017PaymentTerms.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, Contract.CodePaymentTerms).FirstOrDefault();
                            if (PaymentTerms != null)
                            {
                                string AUXDueDateCalculation = PaymentTerms.DueDateCalculation;
                                AUXDueDateCalculation = AUXDueDateCalculation.Substring(0, AUXDueDateCalculation.IndexOf(''));

                                if (!string.IsNullOrEmpty(AUXDueDateCalculation) && Double.TryParse(AUXDueDateCalculation, out double num))
                                {
                                    PreInvoiceToCreate.DueDate = lastDay.AddDays(Convert.ToDouble(AUXDueDateCalculation));
                                }
                            }
                            PreInvoiceToCreate.PaymentTermsCode = Contract.CodePaymentTerms;

                            PreInvoiceToCreate.No_Compromisso = Contract.PromiseNo;
                            PreInvoiceToCreate.CodigoPedido = Contract.ClientRequisitionNo;
                            if (Contract.ReceiptDateRequisition != null && Contract.ReceiptDateRequisition != "")
                                PreInvoiceToCreate.DataEncomenda = DateTime.Parse(Contract.ReceiptDateRequisition);

                            PreInvoiceToCreate.ContractNo = Contract.ContractNo;
                            PreInvoiceToCreate.FacturaCAF = false;
                            PreInvoiceToCreate.Userpreregisto2009 = User.Identity.Name;
                            PreInvoiceToCreate.PostingDate = lastDay;
                            PreInvoiceToCreate.ResponsabilityCenterCode20 = Contract.CodeResponsabilityCenter;
                            PreInvoiceToCreate.FunctionAreaCode20 = Contract.CodeFunctionalArea;
                            PreInvoiceToCreate.RegionCode20 = Contract.CodeRegion;
                            PreInvoiceToCreate.ResponsibilityCenter = userConfig.CentroDeResponsabilidade;
                            PreInvoiceToCreate.PostingNoSeries = userConfig.NumSerieFaturas;
                            PreInvoiceToCreate.PaymentMethodCode = !string.IsNullOrEmpty(Contract.CodePaymentMethod) ? Contract.CodePaymentMethod : "";

                            foreach (ContractInvoiceTextViewModel texts in Contract.InvoiceTexts)
                            {
                                if (texts.InvoiceGroup == Convert.ToInt32(group))
                                {
                                    obs += texts.InvoiceText;
                                }
                            }

                            if (!string.IsNullOrEmpty(obs))
                            {
                                PreInvoiceToCreate.Observacoes = !string.IsNullOrEmpty(obs) ? obs : "";
                            }
                            else
                            {
                                PreInvoiceToCreate.Observacoes = !string.IsNullOrEmpty(Contract.TextoFatura) ? Contract.TextoFatura : "";
                            }
                            obs = "";

                            if (Contract.FaturaPrecosIvaIncluido == true)
                                PreInvoiceToCreate.PricesIncludingVAT = 1;
                            else
                                PreInvoiceToCreate.PricesIncludingVAT = 0;

                            //Obter os Campos das Requisições de Cliente
                            ClientRequisition = DBContractClientRequisition.GetAll().Where(x => x.NºContrato == Contract.ContractNo && x.GrupoFatura == group && x.DataInícioCompromisso <= Convert.ToDateTime(Contract.LastInvoiceDate) && x.DataFimCompromisso >= Convert.ToDateTime(Contract.LastInvoiceDate)).FirstOrDefault();
                            if (ClientRequisition != null)
                            {
                                PreInvoiceToCreate.No_Compromisso = !string.IsNullOrEmpty(ClientRequisition.NºCompromisso) ? ClientRequisition.NºCompromisso : "";
                                if (ClientRequisition.DataRequisição != null)
                                    PreInvoiceToCreate.DataEncomenda = (DateTime)ClientRequisition.DataRequisição;
                                PreInvoiceToCreate.CodigoPedido = !string.IsNullOrEmpty(ClientRequisition.NºRequisiçãoCliente) ? ClientRequisition.NºRequisiçãoCliente : "";
                            }

                            //AMARO Clientes Internos
                            NAVClientsViewModel ClienteNAV = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, Contract.ClientNo);
                            if (ClienteNAV != null && ClienteNAV.InternalClient == true)
                            {
                                PreInvoiceToCreate.RegionCode20 = ClienteNAV.RegionCode;
                                PreInvoiceToCreate.FunctionAreaCode20 = ClienteNAV.FunctionalAreaCode;
                                PreInvoiceToCreate.ResponsabilityCenterCode20 = ClienteNAV.ResponsabilityCenterCode;
                            }

                            //Contratos Tipo Quotas
                            if (Contract != null && Contract.Type == 3)
                            {
                                PreInvoiceToCreate.PaymentMethodCode = !string.IsNullOrEmpty(Contract.CodePaymentMethod) ? Contract.CodePaymentMethod : DBConfiguracaoParametros.GetByParametro("QuotasMetPagamento").Valor;
                            }

                            Task<WSCreatePreInvoice.Create_Result> InvoiceHeader = WSPreInvoice.CreatePreInvoiceHeader(PreInvoiceToCreate, _configws); //, group);
                            InvoiceHeader.Wait();
                            if (InvoiceHeader.IsCompletedSuccessfully && InvoiceHeader.Result != null)
                            {

                                string cod = InvoiceHeader.Result.WSPreInvoice.No;
                                List<LinhasFaturaçãoContrato> LinhasFaturacao = new List<LinhasFaturaçãoContrato>();
                                foreach (ContractLineViewModel line in ContractLines)
                                {
                                    //CREATE SALES LINES
                                    if (line.Billable == true && group == line.InvoiceGroup)
                                    {
                                        LinhasFaturaçãoContrato PreInvoiceLinesToCreate = new LinhasFaturaçãoContrato();
                                        PreInvoiceLinesToCreate.Tipo = line.Type.HasValue ? line.Type.Value.ToString() : "2";
                                        PreInvoiceLinesToCreate.Código = line.Code;
                                        PreInvoiceLinesToCreate.Descrição = line.Description;
                                        PreInvoiceLinesToCreate.Descricao2 = !string.IsNullOrEmpty(line.Description2) && line.Description2.Length > 50 ? line.Description2.Substring(0, 49) : string.IsNullOrEmpty(line.Description2) ? "" : line.Description2;
                                        PreInvoiceLinesToCreate.CódUnidadeMedida = line.CodeMeasureUnit;
                                        PreInvoiceLinesToCreate.CódigoÁreaFuncional = line.CodeFunctionalArea;
                                        PreInvoiceLinesToCreate.CódigoRegião = line.CodeRegion;
                                        PreInvoiceLinesToCreate.CódigoCentroResponsabilidade = line.CodeResponsabilityCenter;
                                        PreInvoiceLinesToCreate.NºContrato = Contract.ContractNo;
                                        PreInvoiceLinesToCreate.NºProjeto = Contract.ContractNo;
                                        PreInvoiceLinesToCreate.CódigoServiço = line.ServiceClientNo;
                                        PreInvoiceLinesToCreate.Quantidade = line.Quantity * (Contract.InvocePeriod == 6 ? 0 : Contract.InvocePeriod == 5 ? 12 : Contract.InvocePeriod == 4 ? 6 : Contract.InvocePeriod);
                                        PreInvoiceLinesToCreate.PreçoUnitário = line.UnitPrice;
                                        PreInvoiceLinesToCreate.GrupoFatura = line.InvoiceGroup ?? 0;
                                        PreInvoiceLinesToCreate.Data_Inicio_Serv_Prestado = DataInicioFatura;
                                        PreInvoiceLinesToCreate.Data_Fim_Serv_Prestado = DataFimFatura;
                                        LinhasFaturacao.Add(PreInvoiceLinesToCreate);
                                    }
                                }

                                if (Contract.FaturaPrecosIvaIncluido == true)
                                {
                                    string NoCliente = string.Empty;
                                    string GrupoIVA = string.Empty;
                                    string GrupoCliente = string.Empty;
                                    decimal IVA = new decimal();
                                    foreach (var item in LinhasFaturacao)
                                    {
                                        NoCliente = Contract.ClientNo;
                                        GrupoIVA = string.Empty;
                                        GrupoCliente = string.Empty;
                                        IVA = 0;

                                        if (!string.IsNullOrEmpty(item.Código))
                                        {
                                            NAVResourcesViewModel Resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, item.Código, "", 0, "").FirstOrDefault();
                                            if (Resource != null)
                                                GrupoIVA = Resource.VATProductPostingGroup;
                                            else
                                            {
                                                execDetails += " Erro ao contabilizar: Não foi possível encontrar o recurso Nº: " + item.Código + " para o Nº de contrato: " + NoCliente;
                                                result.eReasonCode = 2;
                                                result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails));
                                                return Json(result);
                                            }

                                            if (!string.IsNullOrEmpty(NoCliente) && !string.IsNullOrEmpty(GrupoIVA))
                                            {
                                                NAVClientsViewModel cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, NoCliente);
                                                if (cliente != null)
                                                    GrupoCliente = cliente.VATBusinessPostingGroup;
                                                else
                                                {
                                                    execDetails += " Erro ao criar a fatura: Não foi possível encontrar o cliente Nº: " + NoCliente;
                                                    result.eReasonCode = 2;
                                                    result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails));
                                                    return Json(result);
                                                }

                                                if (!string.IsNullOrEmpty(GrupoCliente))
                                                    IVA = DBNAV2017VATPostingSetup.GetIVA(_config.NAVDatabaseName, _config.NAVCompanyName, GrupoCliente, GrupoIVA);

                                                if (IVA > 0)
                                                    IVA = (IVA / 100) + 1;
                                            }
                                        }

                                        if (IVA > 0)
                                            item.PreçoUnitário = item.PreçoUnitário * IVA;
                                    }
                                }

                                try
                                {
                                    Task<WSCreatePreInvoiceLine.CreateMultiple_Result> InvoiceLines = WSPreInvoiceLine.CreatePreInvoiceLineList(LinhasFaturacao, cod, _configws);
                                    InvoiceLines.Wait();
                                    if (InvoiceLines.IsCompletedSuccessfully)
                                    {
                                        registado = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (!ex.Message.ToLower().Contains("maximum message size quota".ToLower()))
                                    {
                                        if (!hasErrors)
                                            hasErrors = true;

                                        execDetails += " Erro ao criar as linhas: ";
                                        errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                        result.eReasonCode = 2;
                                        result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails + errorMessage));
                                    }
                                    else
                                    {
                                        registado = true;
                                    }
                                }
                            }
                            else
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "Não foi piossivel obter dados do NAV na criação da fatura.";
                                return Json(result);
                            }
                        }
                    }
                }
            }

            if (result.eMessages.Count > 0)
            {
                return Json(result);
            }
            else
            {
                if (Contract != null && registado)
                {
                    if (groups != null && groups.Count() > 0)
                    {
                        foreach (int group in groups)
                        {
                            //GET CLIENT REQUISITIONS
                            ClientRequisition = DBContractClientRequisition.GetAll().Where(x => x.NºContrato == Contract.ContractNo && x.GrupoFatura == group && x.DataInícioCompromisso <= Convert.ToDateTime(Contract.LastInvoiceDate) && x.DataFimCompromisso >= Convert.ToDateTime(Contract.LastInvoiceDate)).FirstOrDefault();
                            if (ClientRequisition != null)
                            {
                                ClientRequisition.DataÚltimaFatura = !string.IsNullOrEmpty(Contract.LastInvoiceDate) ? Convert.ToDateTime(Contract.LastInvoiceDate) : (DateTime?)null;
                                if (DBContractClientRequisition.Update(ClientRequisition) != null)
                                    return Json(registado);
                                else
                                {
                                    result.eReasonCode = 5;
                                    result.eMessage = "Ocorreu um erro ao atualizar a Requisição de Cliente.";
                                    return Json(result);
                                }
                            }
                        }
                    }
                    else
                    {
                        ClientRequisition = DBContractClientRequisition.GetAll().Where(x => x.NºContrato == Contract.ContractNo && x.GrupoFatura == Codgroup && x.DataInícioCompromisso <= Convert.ToDateTime(Contract.LastInvoiceDate) && x.DataFimCompromisso >= Convert.ToDateTime(Contract.LastInvoiceDate)).FirstOrDefault();
                        if (ClientRequisition != null)
                        {
                            ClientRequisition.DataÚltimaFatura = !string.IsNullOrEmpty(Contract.LastInvoiceDate) ? Convert.ToDateTime(Contract.LastInvoiceDate) : (DateTime?)null;
                            if (DBContractClientRequisition.Update(ClientRequisition) != null)
                            {
                                DBContracts.Update(DBContracts.ParseToDB(Contract));

                                return Json(registado);

                            }
                            else
                            {
                                result.eReasonCode = 5;
                                result.eMessage = "Ocorreu um erro ao atualizar a Requisição de Cliente.";
                                return Json(result);
                            }
                        }
                    }
                }

                if (Contract != null && registado)
                    DBContracts.Update(DBContracts.ParseToDB(Contract));

                return Json(registado);
            }
        }

        public JsonResult ExitSalesHeader([FromBody] JObject requestParams)
        {

            string Contract = requestParams["Contrato"].ToString();
            NAVSalesHeaderViewModel result = new NAVSalesHeaderViewModel();
            result.eReasonCode = 0;
            result.eMessage = "";
            try
            {
                result = DBNAV2017SalesHeader.GetSalesHeader(_config.NAVDatabaseName, _config.NAVCompanyName, Contract, NAVBaseDocumentTypes.Fatura);
            }
            catch
            {

                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro ao pesquisar Vendas";

            }

            if (result != null)
            {
                result.eReasonCode = 1;
                result.eMessage = "Existem facturas não registadas ligadas a este contrato. Deseja continuar?";
            }
            else if (result == null)
            {
                result = new NAVSalesHeaderViewModel();
                result.eReasonCode = 0;
                result.eMessage = "";
            }
            return Json(result);
        }

        #endregion

        public JsonResult ParseContractType([FromBody] JObject requestParams)
        {
            // Parse Header
            String contractNo = requestParams["HeaderNo"].ToString();
            String versionNo = requestParams["VersionNo"].ToString();

            int originType = int.Parse(requestParams["OriginType"].ToString());
            int contractType = int.Parse(requestParams["HeaderType"].ToString());

            Boolean isParcial = Boolean.Parse(requestParams["Parcial"].ToString());


            String newNumeration = "";

            if (contractNo != null && contractNo != "" &&
                versionNo != null && versionNo != "" &&
                originType != 0 && contractType != 0)
            {
                Contratos thisHeader = DBContracts.GetByIdAndVersion(contractNo, int.Parse(versionNo));
                Contratos thiscontract = DBContracts.GetByIdLastVersion(thisHeader.NºDeContrato);

                if (thisHeader != null)
                {
                    newNumeration = DBNumerationConfigurations.GetNextNumeration(GetNumeration(contractType), true, false);
                    try
                    {
                        thisHeader.Arquivado = false;

                        if (originType == 2)
                        {
                            List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(contractNo, int.Parse(versionNo)).OrderBy(x => x.NºLinha).ToList();
                            try
                            {
                                if (isParcial)
                                {
                                    thisHeader.Estado = 8;
                                }
                                else
                                {
                                    thisHeader.Estado = 7;
                                }
                                thisHeader.TipoContrato = originType;
                                thisHeader.Tipo = 1; //CONTRATO
                                thisHeader.NºDeContrato = contractNo;
                                thisHeader.NºContrato = newNumeration;
                                thisHeader.NºProposta = contractNo;
                                thisHeader.Arquivado = true;
                                DBContracts.Update(thisHeader);


                                thisHeader.Arquivado = false;
                                thisHeader.TipoContrato = contractType;
                                thisHeader.NºProposta = contractNo;
                                thisHeader.NºDeContrato = newNumeration;
                                thisHeader.EstadoAlteração = 1;
                                thisHeader.DataEnvioCliente = null;
                                //thisHeader.DataEnvioCliente = thiscontract != null ? thiscontract.DataEnvioCliente.HasValue ? thiscontract.DataEnvioCliente : (DateTime?)null : (DateTime?)null;
                                thisHeader.DataDaAssinatura = thiscontract != null ? thiscontract.DataDaAssinatura.HasValue ? thiscontract.DataDaAssinatura : (DateTime?)null : (DateTime?)null;
                                thisHeader.NºVersão = 1;
                                thisHeader.NºContrato = "";
                                //thisHeader.Estado = ????? ABARROS
                                string create = DBContracts.Create(thisHeader).NºDeContrato;

                                if (create != null)
                                {
                                    if (isParcial)
                                    {
                                        ContractLines.RemoveAll(x => !x.CriaContrato.HasValue || !x.CriaContrato.Value);
                                    }
                                    foreach (var contractlinestocreate in ContractLines)
                                    {
                                        LinhasContratos newline = ParseToNewModel(contractlinestocreate);

                                        newline.TipoContrato = contractType;
                                        newline.NºContrato = newNumeration;
                                        newline.NºVersão = int.Parse(versionNo);
                                        newline.NºLinha = 0;
                                        newline.GrupoFatura = newline.GrupoFatura == null ? 0 : newline.GrupoFatura;
                                        DBContractLines.Create(newline);
                                    }
                                    if (string.IsNullOrEmpty(thisHeader.NºCliente) && !string.IsNullOrEmpty(thisHeader.NºContato))
                                    {
                                        //convert contact to custumer
                                        Task<WSGenericCodeUnit.FxContact2Customer_Result> convertToCustumerTask = WSGeneric.ConvertToCustomer(thisHeader.NºContato, _configws);
                                        convertToCustumerTask.Wait();
                                        if (convertToCustumerTask.IsCompletedSuccessfully)
                                        {
                                            thisHeader.NºCliente = convertToCustumerTask.Result.return_value;
                                            DBContracts.Update(thisHeader);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                return null;
                            }

                            //Update Last Numeration Used
                            ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(GetNumeration(contractType));
                            ConfigNumerations.ÚltimoNºUsado = newNumeration;
                            ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(ConfigNumerations);

                        }
                        else if (originType == 1)
                        {
                            try
                            {
                                List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(contractNo, int.Parse(versionNo)).OrderBy(x => x.NºLinha).ToList();


                                thisHeader.TipoContrato = originType;
                                thisHeader.Tipo = 1; //CONTRATO
                                thisHeader.NºOportunidade = contractNo;
                                thisHeader.NºProposta = newNumeration;
                                thisHeader.NºDeContrato = contractNo;
                                thisHeader.Arquivado = true;
                                DBContracts.Update(thisHeader);

                                thisHeader.Arquivado = false;
                                thisHeader.TipoContrato = contractType;
                                thisHeader.NºOportunidade = contractNo;
                                thisHeader.NºProposta = newNumeration;
                                thisHeader.NºDeContrato = newNumeration;
                                thisHeader.DataEnvioCliente = null;
                                thisHeader.Estado = 1;
                                //thisHeader.ReferênciaContrato = thisHeader.NºContrato;
                                var create = DBContracts.Create(thisHeader).NºDeContrato;

                                if (create != null)
                                {
                                    foreach (var contractlinestocreate in ContractLines)
                                    {
                                        LinhasContratos newline = ParseToNewModel(contractlinestocreate);

                                        newline.NºLinha = 0;
                                        newline.TipoContrato = contractType;
                                        newline.NºContrato = newNumeration;
                                        newline.NºVersão = int.Parse(versionNo);
                                        newline.GrupoFatura = newline.GrupoFatura == null ? 0 : newline.GrupoFatura;
                                        DBContractLines.Create(newline);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                return null;
                            }

                            //Update Last Numeration Used
                            ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(GetNumeration(contractType));
                            ConfigNumerations.ÚltimoNºUsado = newNumeration;
                            ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(ConfigNumerations);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json("Erro ao criar cabeçalho do contrato.");
                    }

                    return Json(newNumeration);
                }
                else
                {
                    return Json("Erro ao tentar aceder à informação do presente proposta.");
                }
            }
            return Json("Informação em falta para converter para contrato.");
        }

        private static LinhasContratos ParseToNewModel(LinhasContratos contractlinestocreate)
        {
            LinhasContratos newline = new LinhasContratos();
            newline.Contratos = contractlinestocreate.Contratos;
            newline.CriaContrato = contractlinestocreate.CriaContrato;
            newline.Código = contractlinestocreate.Código;
            newline.CódigoCentroResponsabilidade = contractlinestocreate.CódigoCentroResponsabilidade;
            newline.CódigoRegião = contractlinestocreate.CódigoRegião;
            newline.CódigoÁreaFuncional = contractlinestocreate.CódigoÁreaFuncional;
            newline.CódServiçoCliente = contractlinestocreate.CódServiçoCliente;
            newline.CódUnidadeMedida = contractlinestocreate.CódUnidadeMedida;
            newline.DataFimVersão = contractlinestocreate.DataFimVersão;
            newline.DataInícioVersão = contractlinestocreate.DataInícioVersão;
            newline.DescontoLinha = contractlinestocreate.DescontoLinha;
            newline.Descrição = contractlinestocreate.Descrição;
            newline.Descricao2 = contractlinestocreate.Descricao2;
            newline.Faturável = contractlinestocreate.Faturável;
            newline.GrupoFatura = contractlinestocreate.GrupoFatura;
            newline.NºHorasIntervenção = contractlinestocreate.NºHorasIntervenção;
            newline.NºResponsável = contractlinestocreate.NºResponsável;
            newline.NºTécnicos = contractlinestocreate.NºTécnicos;
            newline.NºVersão = contractlinestocreate.NºVersão;
            newline.Periodicidade = contractlinestocreate.Periodicidade;
            newline.PreçoUnitário = contractlinestocreate.PreçoUnitário;
            newline.Quantidade = contractlinestocreate.Quantidade;
            newline.Tipo = contractlinestocreate.Tipo;
            newline.TipoProposta = contractlinestocreate.TipoProposta;

            return newline;
        }

        private static int GetNumeration(int type)
        {
            //Get Contract Numeration
            Configuração Configs = DBConfigurations.GetById(1);
            int ProjectNumerationConfigurationId = 0;

            switch (type)
            {
                case 1:
                    ProjectNumerationConfigurationId = Configs.NumeraçãoOportunidades.Value;
                    break;
                case 2:
                    ProjectNumerationConfigurationId = Configs.NumeraçãoPropostas.Value;
                    break;
                case 3:
                    ProjectNumerationConfigurationId = Configs.NumeraçãoContratos.Value;
                    break;
                default:
                    ProjectNumerationConfigurationId = 0;
                    break;
            }
            return ProjectNumerationConfigurationId;
        }

        private static int GetMonthDiff(DateTime NewDate, DateTime OldDate)
        {
            return ((NewDate.Year - OldDate.Year) * 12) + NewDate.Month - OldDate.Month;
        }


        public IActionResult ProjetosContrato(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.ContractId = string.IsNullOrEmpty(id) ? string.Empty : id;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetInvoiceHeaderList([FromBody] string contractNo)
        {
            List<NAVContractInvoiceHeaderViewModel> result = new List<NAVContractInvoiceHeaderViewModel>();
            result = DBNAV2017ContractDetails.GetContractInvoiceHeaderByNo(contractNo, _config.NAVDatabaseName, _config.NAVCompanyName);

            return Json(result);
        }

        public JsonResult GetNotasCreditoRegistadas([FromBody] string contractNo)
        {
            List<NAVContractInvoiceHeaderViewModel> result = new List<NAVContractInvoiceHeaderViewModel>();
            result = DBNAV2017ContractDetails.GetNotasCreditoRegistadas(contractNo, _config.NAVDatabaseName, _config.NAVCompanyName);

            return Json(result);
        }

        public JsonResult GetInvoiceLinesList([FromBody] string contractNo)
        {
            List<NAVContractInvoiceLinesViewModel> result = new List<NAVContractInvoiceLinesViewModel>();
            result = DBNAV2017ContractDetails.GetContractInvoiceLinesByNo(contractNo, _config.NAVDatabaseName, _config.NAVCompanyName);
            foreach (var temp in result)
            {
                if (temp.DataRegistoDiario != null)
                {
                    temp.DataRegistoDiarioSTR = temp.DataRegistoDiario.Value.Year != 1753 ? temp.DataRegistoDiario.Value.ToString("yyyy-MM-dd") : "";
                }
            }
            return Json(result);
        }

        public JsonResult GetNotasdeCreditoLinesList([FromBody] string contractNo)
        {
            List<NAVContractInvoiceLinesViewModel> result = new List<NAVContractInvoiceLinesViewModel>();
            result = DBNAV2017ContractDetails.GetNotasdeCreditoLinesByNo(contractNo, _config.NAVDatabaseName, _config.NAVCompanyName);
            foreach (var temp in result)
            {
                if (temp.DataRegistoDiario != null)
                {
                    temp.DataRegistoDiarioSTR = temp.DataRegistoDiario.Value.Year != 1753 ? temp.DataRegistoDiario.Value.ToString("yyyy-MM-dd") : "";
                }
            }
            return Json(result);
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Oportunidades([FromBody] List<ContractViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Oportunidades");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["contractNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Oportunidade");
                    Col = Col + 1;
                }
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Âmbito do Serviço");
                    Col = Col + 1;
                }
                if (dp["clientNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Cliente");
                    Col = Col + 1;
                }
                if (dp["clientName"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome Cliente");
                    Col = Col + 1;
                }
                if (dp["contactNoText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Contato");
                    Col = Col + 1;
                }
                if (dp["orderOriginText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Origem do Pedido");
                    Col = Col + 1;
                }
                if (dp["ordOrderSource"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Origem do Pedido Descrição");
                    Col = Col + 1;
                }
                if (dp["proposalNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Proposta");
                    Col = Col + 1;
                }
                //if (dp["versionNo"]["hidden"].ToString() == "False")
                //{
                //    row.CreateCell(Col).SetCellValue("Nº Versão");
                //    Col = Col + 1;
                //}
                if (dp["startData"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Inicial Prevista");
                    Col = Col + 1;
                }
                if (dp["dueDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Expiração Prevista");
                    Col = Col + 1;
                }
                if (dp["customerShipmentDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Envio");
                    Col = Col + 1;
                }
                if (dp["proposalChangeDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Alteração");
                    Col = Col + 1;
                }
                if (dp["internalNumeration"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Numeração Interna");
                    Col = Col + 1;
                }
                if (dp["totalProposalValue"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor Total");
                    Col = Col + 1;
                }
                if (dp["codeRegion"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Região");
                    Col = Col + 1;
                }
                if (dp["codeFunctionalArea"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Área Funcional");
                    Col = Col + 1;
                }
                if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Centro Respon.");
                    Col = Col + 1;
                }
                if (dp["provisionUnitText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Unidade de Prestação");
                    Col = Col + 1;
                }
                if (dp["noVEP"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº VEP");
                    Col = Col + 1;
                }
                if (dp["baseValueProcedure"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor Base Procedimento");
                    Col = Col + 1;
                }
                if (dp["notes"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Notas");
                    Col = Col + 1;
                }
                if (dp["limitClarificationDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data - Limite Esclarecimentos");
                    Col = Col + 1;
                }
                if (dp["limitClarificationTime"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Hora - Limite Esclarecimentos");
                    Col = Col + 1;
                }
                if (dp["errorsOmissionsDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data - Erros e Omissões");
                    Col = Col + 1;
                }
                if (dp["errorsOmissionsTime"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Hora - Erros e Omissões");
                    Col = Col + 1;
                }
                if (dp["proposalDelivery"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data - Entrega Proposta");
                    Col = Col + 1;
                }
                if (dp["proposalDeliveryTime"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Hora - Entrega Propost");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ContractViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["contractNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ContractNo);
                            Col = Col + 1;
                        }
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["clientNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientNo);
                            Col = Col + 1;
                        }
                        if (dp["clientName"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientName);
                            Col = Col + 1;
                        }
                        if (dp["contactNoText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ContactNoText);
                            Col = Col + 1;
                        }
                        if (dp["orderOriginText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.OrderOriginText);
                            Col = Col + 1;
                        }
                        if (dp["ordOrderSource"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.OrdOrderSource);
                            Col = Col + 1;
                        }
                        if (dp["proposalNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProposalNo);
                            Col = Col + 1;
                        }
                        //if (dp["versionNo"]["hidden"].ToString() == "False")
                        //{
                        //    row.CreateCell(Col).SetCellValue(item.VersionNo);
                        //    Col = Col + 1;
                        //}
                        if (dp["startData"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.StartData);
                            Col = Col + 1;
                        }
                        if (dp["dueDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DueDate);
                            Col = Col + 1;
                        }
                        if (dp["customerShipmentDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CustomerShipmentDate);
                            Col = Col + 1;
                        }
                        if (dp["proposalChangeDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProposalChangeDate);
                            Col = Col + 1;
                        }
                        if (dp["internalNumeration"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.InternalNumeration);
                            Col = Col + 1;
                        }
                        if (dp["totalProposalValue"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TotalProposalValue.ToString());
                            Col = Col + 1;
                        }
                        if (dp["codeRegion"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeRegion);
                            Col = Col + 1;
                        }
                        if (dp["codeFunctionalArea"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeFunctionalArea);
                            Col = Col + 1;
                        }
                        if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeResponsabilityCenter);
                            Col = Col + 1;
                        }
                        if (dp["provisionUnitText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProvisionUnitText);
                            Col = Col + 1;
                        }
                        if (dp["noVEP"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.NoVEP);
                            Col = Col + 1;
                        }
                        if (dp["baseValueProcedure"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.BaseValueProcedure.ToString());
                            Col = Col + 1;
                        }
                        if (dp["notes"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Notes);
                            Col = Col + 1;
                        }
                        if (dp["limitClarificationDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.LimitClarificationDate);
                            Col = Col + 1;
                        }
                        if (dp["limitClarificationTime"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.LimitClarificationTime);
                            Col = Col + 1;
                        }
                        if (dp["errorsOmissionsDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ErrorsOmissionsDate);
                            Col = Col + 1;
                        }
                        if (dp["errorsOmissionsTime"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ErrorsOmissionsTime);
                            Col = Col + 1;
                        }
                        if (dp["proposalDelivery"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProposalDelivery);
                            Col = Col + 1;
                        }
                        if (dp["proposalDeliveryTime"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProposalDeliveryTime);
                            Col = Col + 1;
                        }
                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_Oportunidades(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Oportunidades.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Propostas([FromBody] List<ContractViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Propostas");
                IRow row = excelSheet.CreateRow(0);

                var columns = dp.AsJEnumerable().ToList();
                for (int i = 0; i < columns.Count; i++)
                {
                    var column = columns[i];
                    var isHidden = true;
                    var label = "";
                    try
                    {
                        isHidden = (bool)column.First()["hidden"];
                        label = (string)column.First()["label"];
                    }
                    catch { }

                    if (!isHidden)
                    {
                        row.CreateCell(i).SetCellValue(label);
                    }
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ContractViewModel item in Lista)
                    {
                        row = excelSheet.CreateRow(count);

                        for (int i = 0; i < columns.Count; i++)
                        {
                            var column = columns[i];
                            var isHidden = true;
                            try { isHidden = (bool)column.First()["hidden"]; } catch { }

                            if (!isHidden)
                            {
                                var columnPath = column.Path.ToString();
                                columnPath = columnPath.First().ToString().ToUpper() + String.Join("", columnPath.Skip(1));
                                object value = null;
                                try { value = item.GetType().GetProperty(columnPath).GetValue(item, null); } catch { }
                                if (value == null) try { value = item.GetType().GetProperty(columnPath.ToUpper()).GetValue(item, null); } catch { }

                                if ((new[] { "TotalProposalValue", "BaseValueProcedure" }).Contains(columnPath))
                                {
                                    row.CreateCell(i).SetCellValue((double)(value != null ? (decimal)value : 0));
                                }
                                else
                                {
                                    row.CreateCell(i).SetCellValue(value?.ToString());
                                }
                            }
                        }

                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_Propostas(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Propostas.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Contratos([FromBody] List<ContractViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Contratos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["contractNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Contrato"); Col = Col + 1; }
                if (dp["versionNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Versão"); Col = Col + 1; }
                if (dp["type"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo de contrato"); Col = Col + 1; }
                if (dp["description"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Âmbito dos Serviços"); Col = Col + 1; }
                if (dp["clientNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Cliente"); Col = Col + 1; }
                if (dp["clientName"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nome Cliente"); Col = Col + 1; }
                if (dp["provisionUnitText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Unidade de Prestação"); Col = Col + 1; }
                if (dp["codeRegion"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Região"); Col = Col + 1; }
                if (dp["codeFunctionalArea"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Área Funcional"); Col = Col + 1; }
                if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Centro Respon."); Col = Col + 1; }
                if (dp["proposalNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Proposta"); Col = Col + 1; }
                if (dp["codePaymentTerms"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Termos Pagamento"); Col = Col + 1; }
                if (dp["startDateFirstContract"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Inicio 1º Contrato"); Col = Col + 1; }
                if (dp["firstContractReference"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Referência 1º Contrato"); Col = Col + 1; }
                if (dp["notes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Notas"); Col = Col + 1; }
                if (dp["startData"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Início Versão"); Col = Col + 1; }
                if (dp["dueDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Fim Versão"); Col = Col + 1; }
                if (dp["statusDescription"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Estado"); Col = Col + 1; }
                if (dp["changeStatusText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Estado Alteração"); Col = Col + 1; }
                if (dp["relatedContract"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Referência do contrato"); Col = Col + 1; }
                if (dp["contractStartDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Inicio Contrato"); Col = Col + 1; }
                if (dp["contractEndDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Final Contrato"); Col = Col + 1; }
                if (dp["customerShipmentDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data envio cliente"); Col = Col + 1; }
                if (dp["terminationTermNoticeText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Rescisão (Prazo de Aviso)"); Col = Col + 1; }
                if (dp["renovationConditionsText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Condições para renovação"); Col = Col + 1; }
                if (dp["renovationConditionsAnother"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Condições para renovação (Outras)"); Col = Col + 1; }
                if (dp["paymentTermsText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Condições Pagamento"); Col = Col + 1; }
                if (dp["paymentTermsAnother"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Condições Pagamento (Outras)"); Col = Col + 1; }
                if (dp["signatureDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Assinatura"); Col = Col + 1; }
                if (dp["contractMaxDuration"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Duração máxima contrato"); Col = Col + 1; }
                if (dp["customerSignedText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Assinado Cliente"); Col = Col + 1; }
                if (dp["interestsText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Juros"); Col = Col + 1; }
                if (dp["contractDurationDescription"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição Duração Contrato"); Col = Col + 1; }
                if (dp["billingTypeText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo Faturação"); Col = Col + 1; }
                if (dp["invocePeriodText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Período Fatura"); Col = Col + 1; }
                if (dp["fixedVowsAgreementText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Contrato Avença Fixa"); Col = Col + 1; }
                if (dp["somatorioLinhas"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Somatório das Linhas"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ContractViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["contractNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContractNo); Col = Col + 1; }
                        if (dp["versionNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.VersionNo); Col = Col + 1; }
                        if (dp["type"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Type); Col = Col + 1; }
                        if (dp["description"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Description); Col = Col + 1; }
                        if (dp["clientNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClientNo); Col = Col + 1; }
                        if (dp["clientName"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClientName); Col = Col + 1; }
                        if (dp["provisionUnitText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ProvisionUnitText); Col = Col + 1; }
                        if (dp["codeRegion"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodeRegion); Col = Col + 1; }
                        if (dp["codeFunctionalArea"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodeFunctionalArea); Col = Col + 1; }
                        if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodeResponsabilityCenter); Col = Col + 1; }
                        if (dp["proposalNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ProposalNo); Col = Col + 1; }
                        if (dp["codePaymentTerms"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodePaymentTerms); Col = Col + 1; }
                        if (dp["startDateFirstContract"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.StartDateFirstContract); Col = Col + 1; }
                        if (dp["firstContractReference"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FirstContractReference); Col = Col + 1; }
                        if (dp["notes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Notes); Col = Col + 1; }
                        if (dp["startData"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.StartData); Col = Col + 1; }
                        if (dp["dueDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DueDate); Col = Col + 1; }
                        if (dp["statusDescription"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.StatusDescription); Col = Col + 1; }
                        if (dp["changeStatusText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ChangeStatusText); Col = Col + 1; }
                        if (dp["relatedContract"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RelatedContract); Col = Col + 1; }
                        if (dp["contractStartDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContractStartDate); Col = Col + 1; }
                        if (dp["contractEndDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContractEndDate); Col = Col + 1; }
                        if (dp["customerShipmentDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CustomerShipmentDate); Col = Col + 1; }
                        if (dp["terminationTermNoticeText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.TerminationTermNoticeText); Col = Col + 1; }
                        if (dp["renovationConditionsText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RenovationConditionsText); Col = Col + 1; }
                        if (dp["renovationConditionsAnother"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RenovationConditionsAnother); Col = Col + 1; }
                        if (dp["paymentTermsText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PaymentTermsText); Col = Col + 1; }
                        if (dp["paymentTermsAnother"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PaymentTermsAnother); Col = Col + 1; }
                        if (dp["signatureDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.SignatureDate); Col = Col + 1; }
                        if (dp["contractMaxDuration"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContractMaxDuration); Col = Col + 1; }
                        if (dp["customerSignedText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CustomerSignedText); Col = Col + 1; }
                        if (dp["interestsText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.InterestsText); Col = Col + 1; }
                        if (dp["contractDurationDescription"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContractDurationDescription); Col = Col + 1; }
                        if (dp["billingTypeText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.BillingTypeText); Col = Col + 1; }
                        if (dp["invocePeriodText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.InvocePeriodText); Col = Col + 1; }
                        if (dp["fixedVowsAgreementText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FixedVowsAgreementText); Col = Col + 1; }
                        if (dp["somatorioLinhas"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.SomatorioLinhas.ToString()); Col = Col + 1; }
                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_Contratos(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contratos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_ContratosCliente([FromBody] List<ContractViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Contratos por Cliente");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["contractNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Contrato");
                    Col = Col + 1;
                }
                if (dp["type"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo");
                    Col = Col + 1;
                }
                if (dp["startData"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Início Versão");
                    Col = Col + 1;
                }
                if (dp["dueDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Fim Versão");
                    Col = Col + 1;
                }
                if (dp["clientNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Cliente");
                    Col = Col + 1;
                }
                if (dp["clientName"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome Cliente");
                    Col = Col + 1;
                }
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Âmbito dos Serviços");
                    Col = Col + 1;
                }
                if (dp["statusDescription"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Estado");
                    Col = Col + 1;
                }
                if (dp["codeRegion"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Região");
                    Col = Col + 1;
                }
                if (dp["codeFunctionalArea"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Área Funcional");
                    Col = Col + 1;
                }
                if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["versionNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Versão");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ContractViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["contractNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ContractNo);
                            Col = Col + 1;
                        }
                        if (dp["type"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Type);
                            Col = Col + 1;
                        }
                        if (dp["startData"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.StartData);
                            Col = Col + 1;
                        }
                        if (dp["dueDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DueDate);
                            Col = Col + 1;
                        }
                        if (dp["clientNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientNo);
                            Col = Col + 1;
                        }
                        if (dp["clientName"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientName);
                            Col = Col + 1;
                        }
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["statusDescription"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.StatusDescription);
                            Col = Col + 1;
                        }
                        if (dp["codeRegion"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeRegion);
                            Col = Col + 1;
                        }
                        if (dp["codeFunctionalArea"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeFunctionalArea);
                            Col = Col + 1;
                        }
                        if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeResponsabilityCenter);
                            Col = Col + 1;
                        }
                        if (dp["versionNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.VersionNo);
                            Col = Col + 1;
                        }
                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_ContratosCliente(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contratos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_ContratosLinhas([FromBody] List<ContractLineViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Linhas de Contratos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["contratoClienteCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cliente"); Col = Col + 1; }
                if (dp["contratoClienteNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nome Cliente"); Col = Col + 1; }
                if (dp["contractEndereco"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Endereço"); Col = Col + 1; }
                if (dp["contractNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Contrato"); Col = Col + 1; }
                if (dp["versionNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Versão"); Col = Col + 1; }
                if (dp["contractoEstado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Estado"); Col = Col + 1; }
                if (dp["contratoCodigoPostal"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Postal"); Col = Col + 1; }
                if (dp["contratoTipo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo Contrato"); Col = Col + 1; }
                if (dp["contratoAvencaFixa"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Avença Fixa"); Col = Col + 1; }
                if (dp["lineNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Linha"); Col = Col + 1; }
                if (dp["code"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº"); Col = Col + 1; }
                if (dp["contratoTipoFaturacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo"); Col = Col + 1; }
                if (dp["description"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição"); Col = Col + 1; }
                if (dp["description2"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição 2"); Col = Col + 1; }
                if (dp["quantity"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Quantidade"); Col = Col + 1; }
                if (dp["codeMeasureUnit"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Unidade Medida"); Col = Col + 1; }
                if (dp["unitPrice"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Preço Unitário"); Col = Col + 1; }
                if (dp["codeRegion"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Região"); Col = Col + 1; }
                if (dp["codeFunctionalArea"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Área Funcional"); Col = Col + 1; }
                if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Centro Responsabilidade"); Col = Col + 1; }
                if (dp["serviceClientNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Serv. Cliente"); Col = Col + 1; }
                if (dp["serviceClientName"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Des. Serv. Cliente"); Col = Col + 1; }
                if (dp["invoiceGroup"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Grupo Fatura"); Col = Col + 1; }
                if (dp["versionStartDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Inicio Versão"); Col = Col + 1; }
                if (dp["versionEndDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Fim Versão"); Col = Col + 1; }
                if (dp["contratoDataExpiracao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Expiração Contrato"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ContractLineViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["contratoClienteCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoClienteCode); Col = Col + 1; }
                        if (dp["contratoClienteNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoClienteNome); Col = Col + 1; }
                        if (dp["contractEndereco"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContractEndereco); Col = Col + 1; }
                        if (dp["contractNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContractNo); Col = Col + 1; }
                        if (dp["versionNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.VersionNo); Col = Col + 1; }
                        if (dp["contractoEstado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContractoEstado); Col = Col + 1; }
                        if (dp["contratoCodigoPostal"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoCodigoPostal); Col = Col + 1; }
                        if (dp["contratoTipo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoTipo); Col = Col + 1; }
                        if (dp["contratoAvencaFixa"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoAvencaFixa); Col = Col + 1; }
                        if (dp["lineNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.LineNo); Col = Col + 1; }
                        if (dp["code"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Code); Col = Col + 1; }
                        if (dp["contratoTipoFaturacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoTipoFaturacao); Col = Col + 1; }
                        if (dp["description"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Description); Col = Col + 1; }
                        if (dp["description2"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Description2); Col = Col + 1; }
                        if (dp["quantity"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Quantity.ToString()); Col = Col + 1; }
                        if (dp["codeMeasureUnit"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodeMeasureUnit); Col = Col + 1; }
                        if (dp["unitPrice"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UnitPrice.ToString()); Col = Col + 1; }
                        if (dp["codeRegion"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodeRegion); Col = Col + 1; }
                        if (dp["codeFunctionalArea"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodeFunctionalArea); Col = Col + 1; }
                        if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodeResponsabilityCenter); Col = Col + 1; }
                        if (dp["serviceClientNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ServiceClientNo); Col = Col + 1; }
                        if (dp["serviceClientName"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ServiceClientName); Col = Col + 1; }
                        if (dp["invoiceGroup"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.InvoiceGroup.ToString()); Col = Col + 1; }
                        if (dp["versionStartDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.VersionStartDate); Col = Col + 1; }
                        if (dp["versionEndDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.VersionEndDate); Col = Col + 1; }
                        if (dp["contratoDataExpiracao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoDataExpiracao); Col = Col + 1; }

                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_ContratosLinhas(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Linhas de Contratos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_ContratosInternosLinhas([FromBody] List<ContractLineViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Linhas de Contratos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["contratoClienteCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cliente"); Col = Col + 1; }
                if (dp["contratoClienteNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nome Cliente"); Col = Col + 1; }
                if (dp["contractEndereco"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Endereço"); Col = Col + 1; }
                if (dp["contractNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Contrato"); Col = Col + 1; }
                if (dp["versionNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Versão"); Col = Col + 1; }
                if (dp["contractoEstado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Estado"); Col = Col + 1; }
                if (dp["contratoCodigoPostal"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Postal"); Col = Col + 1; }
                if (dp["contratoTipo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo Contrato"); Col = Col + 1; }
                if (dp["contratoAvencaFixa"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Avença Fixa"); Col = Col + 1; }
                if (dp["lineNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Linha"); Col = Col + 1; }
                if (dp["code"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº"); Col = Col + 1; }
                if (dp["contratoTipoFaturacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo"); Col = Col + 1; }
                if (dp["description"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição"); Col = Col + 1; }
                if (dp["description2"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição 2"); Col = Col + 1; }
                if (dp["quantity"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Quantidade"); Col = Col + 1; }
                if (dp["codeMeasureUnit"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Unidade Medida"); Col = Col + 1; }
                if (dp["unitPrice"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Preço Unitário"); Col = Col + 1; }
                if (dp["codeRegion"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Região"); Col = Col + 1; }
                if (dp["codeFunctionalArea"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Área Funcional"); Col = Col + 1; }
                if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Centro Responsabilidade"); Col = Col + 1; }
                if (dp["serviceClientNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Serv. Cliente"); Col = Col + 1; }
                if (dp["serviceClientName"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Des. Serv. Cliente"); Col = Col + 1; }
                if (dp["invoiceGroup"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Grupo Fatura"); Col = Col + 1; }
                if (dp["versionStartDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Inicio Versão"); Col = Col + 1; }
                if (dp["versionEndDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Fim Versão"); Col = Col + 1; }
                if (dp["contratoDataExpiracao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Expiração Contrato"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ContractLineViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["contratoClienteCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoClienteCode); Col = Col + 1; }
                        if (dp["contratoClienteNome"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoClienteNome); Col = Col + 1; }
                        if (dp["contractEndereco"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContractEndereco); Col = Col + 1; }
                        if (dp["contractNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContractNo); Col = Col + 1; }
                        if (dp["versionNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.VersionNo); Col = Col + 1; }
                        if (dp["contractoEstado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContractoEstado); Col = Col + 1; }
                        if (dp["contratoCodigoPostal"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoCodigoPostal); Col = Col + 1; }
                        if (dp["contratoTipo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoTipo); Col = Col + 1; }
                        if (dp["contratoAvencaFixa"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoAvencaFixa); Col = Col + 1; }
                        if (dp["lineNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.LineNo); Col = Col + 1; }
                        if (dp["code"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Code); Col = Col + 1; }
                        if (dp["contratoTipoFaturacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoTipoFaturacao); Col = Col + 1; }
                        if (dp["description"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Description); Col = Col + 1; }
                        if (dp["description2"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Description2); Col = Col + 1; }
                        if (dp["quantity"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Quantity.ToString()); Col = Col + 1; }
                        if (dp["codeMeasureUnit"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodeMeasureUnit); Col = Col + 1; }
                        if (dp["unitPrice"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UnitPrice.ToString()); Col = Col + 1; }
                        if (dp["codeRegion"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodeRegion); Col = Col + 1; }
                        if (dp["codeFunctionalArea"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodeFunctionalArea); Col = Col + 1; }
                        if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodeResponsabilityCenter); Col = Col + 1; }
                        if (dp["serviceClientNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ServiceClientNo); Col = Col + 1; }
                        if (dp["serviceClientName"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ServiceClientName); Col = Col + 1; }
                        if (dp["invoiceGroup"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.InvoiceGroup.ToString()); Col = Col + 1; }
                        if (dp["versionStartDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.VersionStartDate); Col = Col + 1; }
                        if (dp["versionEndDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.VersionEndDate); Col = Col + 1; }
                        if (dp["contratoDataExpiracao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContratoDataExpiracao); Col = Col + 1; }

                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_ContratosInternosLinhas(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Linhas de Contratos Internos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_ContratosQuotas([FromBody] List<ContractViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Contratos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["contractNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Contrato");
                    Col = Col + 1;
                }
                if (dp["type"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo");
                    Col = Col + 1;
                }
                if (dp["startData"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Inicio");
                    Col = Col + 1;
                }
                if (dp["dueDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Fim");
                    Col = Col + 1;
                }
                if (dp["clientNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Cliente");
                    Col = Col + 1;
                }
                if (dp["clientName"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome Cliente");
                    Col = Col + 1;
                }
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Âmbito dos Serviços");
                    Col = Col + 1;
                }
                if (dp["statusDescription"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Estado");
                    Col = Col + 1;
                }
                if (dp["codeRegion"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Região");
                    Col = Col + 1;
                }
                if (dp["codeFunctionalArea"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Área Funcional");
                    Col = Col + 1;
                }
                if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["versionNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Versão");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ContractViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["contractNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ContractNo);
                            Col = Col + 1;
                        }
                        if (dp["type"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Type);
                            Col = Col + 1;
                        }
                        if (dp["startData"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.StartData);
                            Col = Col + 1;
                        }
                        if (dp["dueDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DueDate);
                            Col = Col + 1;
                        }
                        if (dp["clientNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientNo);
                            Col = Col + 1;
                        }
                        if (dp["clientName"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientName);
                            Col = Col + 1;
                        }
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["statusDescription"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.StatusDescription);
                            Col = Col + 1;
                        }
                        if (dp["codeRegion"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeRegion);
                            Col = Col + 1;
                        }
                        if (dp["codeFunctionalArea"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeFunctionalArea);
                            Col = Col + 1;
                        }
                        if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeResponsabilityCenter);
                            Col = Col + 1;
                        }
                        if (dp["versionNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.VersionNo);
                            Col = Col + 1;
                        }
                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_ContratosQuotas(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contratos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_ContratosInternos([FromBody] List<ContractViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Contratos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["contractNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Contrato");
                    Col = Col + 1;
                }
                if (dp["type"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo");
                    Col = Col + 1;
                }
                if (dp["startData"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Inicio");
                    Col = Col + 1;
                }
                if (dp["dueDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Fim");
                    Col = Col + 1;
                }
                if (dp["clientNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Cliente");
                    Col = Col + 1;
                }
                if (dp["clientName"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome Cliente");
                    Col = Col + 1;
                }
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Âmbito dos Serviços");
                    Col = Col + 1;
                }
                if (dp["statusDescription"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Estado");
                    Col = Col + 1;
                }
                if (dp["codeRegion"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Região");
                    Col = Col + 1;
                }
                if (dp["codeFunctionalArea"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Área Funcional");
                    Col = Col + 1;
                }
                if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["versionNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Versão");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ContractViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["contractNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ContractNo);
                            Col = Col + 1;
                        }
                        if (dp["type"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Type);
                            Col = Col + 1;
                        }
                        if (dp["startData"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.StartData);
                            Col = Col + 1;
                        }
                        if (dp["dueDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DueDate);
                            Col = Col + 1;
                        }
                        if (dp["clientNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientNo);
                            Col = Col + 1;
                        }
                        if (dp["clientName"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientName);
                            Col = Col + 1;
                        }
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["statusDescription"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.StatusDescription);
                            Col = Col + 1;
                        }
                        if (dp["codeRegion"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeRegion);
                            Col = Col + 1;
                        }
                        if (dp["codeFunctionalArea"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeFunctionalArea);
                            Col = Col + 1;
                        }
                        if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeResponsabilityCenter);
                            Col = Col + 1;
                        }
                        if (dp["versionNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.VersionNo);
                            Col = Col + 1;
                        }
                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_ContratosInternos(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contratos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_AvencaFixa([FromBody] List<FaturacaoContratosViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Avença Fixa");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["contractNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Contrato");
                    Col = Col + 1;
                }
                if (dp["situation"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Situação");
                    Col = Col + 1;
                }
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Descrição");
                    Col = Col + 1;
                }
                if (dp["clientNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Cliente");
                    Col = Col + 1;
                }
                if (dp["clientName"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome Cliente");
                    Col = Col + 1;
                }
                if (dp["invoiceValue"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor da Fatura");
                    Col = Col + 1;
                }
                if (dp["numberOfInvoices"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Faturas a Emitir");
                    Col = Col + 1;
                }
                if (dp["invoiceTotal"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Total a Faturar");
                    Col = Col + 1;
                }
                if (dp["contractValue"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor do Contrato");
                    Col = Col + 1;
                }
                if (dp["valueToInvoice"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor por Faturar");
                    Col = Col + 1;
                }
                //if (dp["billedValue"]["hidden"].ToString() == "False")
                //{
                //    row.CreateCell(Col).SetCellValue("Valor Faturado");
                //    Col = Col + 1;
                //}
                if (dp["regionCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Região");
                    Col = Col + 1;
                }
                if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Área Funcional");
                    Col = Col + 1;
                }
                if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["startDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Inicial");
                    Col = Col + 1;
                }
                if (dp["expiryDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Expiração");
                    Col = Col + 1;
                }
                if (dp["registerDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Próxima Fatura");
                    Col = Col + 1;
                }
                if (dp["invoicePeriod"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Período Fatura");
                    Col = Col + 1;
                }
                if (dp["invoiceGroupValue"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Grupo Fatura");
                    Col = Col + 1;
                }
                if (dp["invoiceGroupCount"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Quantidade de Linhas Agrupadas");
                    Col = Col + 1;
                }
                if (dp["document_No"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (FaturacaoContratosViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["contractNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ContractNo);
                            Col = Col + 1;
                        }
                        if (dp["situation"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Situation);
                            Col = Col + 1;
                        }
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["clientNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientNo);
                            Col = Col + 1;
                        }
                        if (dp["clientName"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientName);
                            Col = Col + 1;
                        }
                        if (dp["invoiceValue"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.InvoiceValue.ToString());
                            Col = Col + 1;
                        }
                        if (dp["numberOfInvoices"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.NumberOfInvoices.ToString());
                            Col = Col + 1;
                        }
                        if (dp["invoiceTotal"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.InvoiceTotal.ToString());
                            Col = Col + 1;
                        }
                        if (dp["contractValue"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ContractValue.ToString());
                            Col = Col + 1;
                        }
                        if (dp["valueToInvoice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ValueToInvoice.ToString());
                            Col = Col + 1;
                        }
                        //if (dp["billedValue"]["hidden"].ToString() == "False")
                        //{
                        //    row.CreateCell(Col).SetCellValue(item.BilledValue.ToString());
                        //    Col = Col + 1;
                        //}
                        if (dp["regionCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RegionCode);
                            Col = Col + 1;
                        }
                        if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FunctionalAreaCode);
                            Col = Col + 1;
                        }
                        if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ResponsabilityCenterCode);
                            Col = Col + 1;
                        }
                        if (dp["startDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.StartDate);
                            Col = Col + 1;
                        }
                        if (dp["expiryDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ExpiryDate);
                            Col = Col + 1;
                        }
                        if (dp["registerDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RegisterDate);
                            Col = Col + 1;
                        }
                        if (dp["invoicePeriod"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.InvoicePeriod.ToString());
                            Col = Col + 1;
                        }
                        if (dp["invoiceGroupValue"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.InvoiceGroupValue.ToString());
                            Col = Col + 1;
                        }
                        if (dp["invoiceGroupCount"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.InvoiceGroupCount.ToString());
                            Col = Col + 1;
                        }
                        if (dp["document_No"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Document_No);
                            Col = Col + 1;
                        }
                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_AvencaFixa(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Avença Fixa.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        #region ANEXOS

        [HttpPost]
        [Route("Contratos/FileUpload")]
        [Route("Contratos/FileUpload/{id}")]
        [Route("Contratos/FileUpload/{id}/{linha}")]
        public JsonResult FileUpload(string id, int linha)
        {
            try
            {
                var files = Request.Form.Files;
                string full_filename;
                foreach (var file in files)
                {
                    try
                    {
                        string filename = Path.GetFileName(file.FileName);
                        //full_filename = id + "_" + filename;
                        full_filename = filename;
                        var path = Path.Combine(_generalConfig.FileUploadFolder + "Contratos\\", full_filename);
                        using (FileStream dd = new FileStream(path, FileMode.CreateNew))
                        {
                            file.CopyTo(dd);
                            dd.Dispose();

                            Anexos newfile = new Anexos();
                            newfile.NºOrigem = id;
                            newfile.UrlAnexo = full_filename;
                            newfile.Visivel = true;

                            //TipoOrigem: 1-PréRequisição; 2-Requisição; 3-Contratos; 4-Procedimentos;5-ConsultaMercado 
                            newfile.TipoOrigem = TipoOrigemAnexos.Contratos;

                            newfile.DataHoraCriação = DateTime.Now;
                            newfile.UtilizadorCriação = User.Identity.Name;

                            DBAttachments.Create(newfile);
                            if (newfile.NºLinha == 0)
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return Json("");
        }

        [HttpPost]
        public JsonResult LoadAttachments([FromBody] JObject requestParams)
        {
            string id = requestParams["id"].ToString();
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Contratos);

            List<Anexos> list = DBAttachments.GetById(id);

            if (UPerm != null && UPerm.Update == false)
            {
                list.RemoveAll(x => x.Visivel != true);
            }

            List<AttachmentsViewModel> attach = new List<AttachmentsViewModel>();
            list.ForEach(x => attach.Add(DBAttachments.ParseToViewModel(x)));
            return Json(attach);
        }

        [HttpPost]
        public JsonResult DeleteAttachments([FromBody] AttachmentsViewModel requestParams)
        {
            try
            {
                System.IO.File.Delete(_generalConfig.FileUploadFolder + "Contratos\\" + requestParams.Url);
                DBAttachments.Delete(DBAttachments.ParseToDB(requestParams));
                requestParams.eReasonCode = 1;

            }
            catch (Exception ex)
            {
                requestParams.eReasonCode = 2;
                return Json(requestParams);
            }
            return Json(requestParams);
        }

        [HttpPost]
        public JsonResult UpdateAnexoVisivel([FromBody] AttachmentsViewModel Anexo)
        {
            try
            {
                if (Anexo != null && Anexo.DocLineNo > 0)
                {
                    Anexos ANEX = DBAttachments.GetByNoLinha(Anexo.DocLineNo);
                    if (ANEX != null)
                    {
                        ANEX.Visivel = Anexo.Visivel;
                        ANEX.UtilizadorModificação = User.Identity.Name;
                        ANEX.DataHoraModificação = DateTime.Now;
                        if (DBAttachments.Update(ANEX) != null)
                            return Json(true);
                        else
                            return Json(false);
                    }
                    else
                        return Json(false);
                }
                else
                    return Json(false);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        #endregion

        [HttpPost]
        public JsonResult GetAddressData([FromBody] JObject requestParams)
        {
            NAVAddressesViewModel result = new NAVAddressesViewModel();

            string clientcode = requestParams["clientcode"].ToString();
            string addresscode = requestParams["addresscode"].ToString();

            if (!string.IsNullOrEmpty(clientcode) && !string.IsNullOrEmpty(addresscode))
            {
                NAVAddressesViewModel SHIP = DBNAV2017ShippingAddresses.GetByClientAndCode(clientcode, addresscode, _config.NAVDatabaseName, _config.NAVCompanyName);
                if (SHIP != null)
                {
                    result.Name = SHIP.Name1;
                    result.Name2 = SHIP.Name2;
                    result.Address = SHIP.Address1;
                    result.Address2 = SHIP.Address2;
                    result.ZipCode = SHIP.ZipCode;
                    result.City = SHIP.City;

                    return Json(result);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(clientcode))
                {
                    NAVClientsViewModel cli = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, clientcode);
                    if (cli != null)
                    {
                        result.Name = cli.Name;
                        result.Name2 = "";
                        result.Address = cli.Address1;
                        result.Address2 = cli.Address2;
                        result.ZipCode = cli.PostCode;
                        result.City = cli.City;

                        return Json(result);
                    }
                }
                else
                {
                    result.Name = "";
                    result.Name2 = "";
                    result.Address = "";
                    result.Address2 = "";
                    result.ZipCode = "";
                    result.City = "";

                    return Json(result);
                }
            }
            return Json(result);

            //if (!string.IsNullOrEmpty(requestParams["clientcode"].ToString()) && !string.IsNullOrEmpty(requestParams["addresscode"].ToString()))
            //{
            //    NAVAddressesViewModel result = DBNAV2017ShippingAddresses.GetByClientAndCode(requestParams["clientcode"].ToString(), requestParams["addresscode"].ToString(), _config.NAVDatabaseName, _config.NAVCompanyName);

            //    return Json(result);
            //}

            //return Json(null);
        }

        [HttpPost]
        public async Task<JsonResult> ExportRequisicoesCliente([FromBody] ContractViewModel data)
        {
            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            List<Projetos> AllProjects = DBProjects.GetAll();

            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Requisições Cliente");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Nº Contrato");
                row.CreateCell(1).SetCellValue("Âmbito dos Serviços");
                row.CreateCell(2).SetCellValue("Nº Cliente");
                row.CreateCell(3).SetCellValue("Cliente");
                row.CreateCell(4).SetCellValue("Cod. Região");
                row.CreateCell(5).SetCellValue("Início Compromisso");
                row.CreateCell(6).SetCellValue("Fim Compromisso");
                row.CreateCell(7).SetCellValue("Nº Requisição Cliente");
                row.CreateCell(8).SetCellValue("Data Requisição");
                row.CreateCell(9).SetCellValue("Nº Compromisso");
                row.CreateCell(10).SetCellValue("Grupo Fatura");
                row.CreateCell(11).SetCellValue("Nº Projeto");
                row.CreateCell(12).SetCellValue("Descrição");
                row.CreateCell(13).SetCellValue("Data Última Fatura");

                if (data != null)
                {
                    int count = 1;

                    if (data.ClientRequisitions != null && data.ClientRequisitions.Count > 0)
                    {
                        foreach (ContractClientRequisitionViewModel item in data.ClientRequisitions)
                        {
                            row = excelSheet.CreateRow(count);

                            row.CreateCell(0).SetCellValue(data.ContractNo);
                            row.CreateCell(1).SetCellValue(data.Description);
                            row.CreateCell(2).SetCellValue(data.ClientNo);
                            row.CreateCell(3).SetCellValue(data.ClientName);
                            row.CreateCell(4).SetCellValue(data.CodeRegion);
                            row.CreateCell(5).SetCellValue(item.StartDate);
                            row.CreateCell(6).SetCellValue(item.EndDate);
                            row.CreateCell(7).SetCellValue(item.ClientRequisitionNo);
                            row.CreateCell(8).SetCellValue(item.RequisitionDate);
                            row.CreateCell(9).SetCellValue(item.PromiseNo);
                            row.CreateCell(10).SetCellValue(item.InvoiceGroup);
                            row.CreateCell(11).SetCellValue(item.ProjectNo);
                            if (!string.IsNullOrEmpty(item.ProjectNo))
                            {
                                Projetos Project = AllProjects.Where(x => x.NºProjeto == item.ProjectNo).FirstOrDefault();
                                if (Project != null && !string.IsNullOrEmpty(Project.Descrição))
                                    row.CreateCell(12).SetCellValue(Project.Descrição);
                            }
                            row.CreateCell(13).SetCellValue(item.LastInvoiceDate);

                            count++;
                        }
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return Json(sFileName);
        }
        //2
        public IActionResult ExportToRequisicoesClienteDownload(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\" + sFileName;
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }







        [HttpPost]
        public async Task<JsonResult> ExportToLastNotaEncomenda([FromBody] List<ContractViewModel> Lista)
        {
            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            List<Projetos> AllProjects = DBProjects.GetAll();

            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Requisições Cliente");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Nº Contrato");
                row.CreateCell(1).SetCellValue("Âmbito dos Serviços");
                row.CreateCell(2).SetCellValue("Nº Cliente");
                row.CreateCell(3).SetCellValue("Cliente");
                row.CreateCell(4).SetCellValue("Cod. Região");
                row.CreateCell(5).SetCellValue("Início Compromisso");
                row.CreateCell(6).SetCellValue("Fim Compromisso");
                row.CreateCell(7).SetCellValue("Nº Requisição Cliente");
                row.CreateCell(8).SetCellValue("Data Requisição");
                row.CreateCell(9).SetCellValue("Nº Compromisso");
                row.CreateCell(10).SetCellValue("Grupo Fatura");
                row.CreateCell(11).SetCellValue("Nº Projeto");
                row.CreateCell(12).SetCellValue("Descrição");
                row.CreateCell(13).SetCellValue("Data Última Fatura");

                if (Lista != null && Lista.Count > 0)
                {
                    int count = 1;
                    bool newLine = true;
                    foreach (ContractViewModel contrato in Lista)
                    {
                        if (contrato != null)
                        {
                            row = excelSheet.CreateRow(count);

                            row.CreateCell(0).SetCellValue(contrato.ContractNo);
                            row.CreateCell(1).SetCellValue(contrato.Description);
                            row.CreateCell(2).SetCellValue(contrato.ClientNo);
                            row.CreateCell(3).SetCellValue(contrato.ClientName);
                            row.CreateCell(4).SetCellValue(contrato.CodeRegion);

                            List<RequisiçõesClienteContrato> AllClientRequisitions = DBContractClientRequisition.GetByContract(contrato.ContractNo).OrderBy(x => x.GrupoFatura).ThenByDescending(x => x.DataInícioCompromisso) .ToList();

                            if (AllClientRequisitions != null && AllClientRequisitions.Count > 0)
                            {
                                //RequisiçõesClienteContrato ClientRequisition = AllClientRequisitions.OrderByDescending(x => x.DataInícioCompromisso).FirstOrDefault();
                                var ClientRequisitionGroup = AllClientRequisitions.GroupBy(x => new { x.GrupoFatura, x.NºProjeto },
                                     x => x,
                                     (Key, items) => new
                                     {
                                         GruoFatura = Key.GrupoFatura,
                                         Projeto = Key.NºProjeto,
                                         Items = items
                                     }).ToList();

                                foreach (var ClientRequisition in ClientRequisitionGroup)
                                {
                                    if (ClientRequisition != null)
                                    {
                                        if (ClientRequisition.Items.ToList().Count > 0)
                                        {
                                            //ContractClientRequisitionViewModel ClientRequisitionVM = DBContractClientRequisition.ParseToViewModel(ClientRequisition);
                                            ContractClientRequisitionViewModel ClientRequisitionVM = DBContractClientRequisition.ParseToViewModel(ClientRequisition.Items.First());

                                            row.CreateCell(0).SetCellValue(contrato.ContractNo);
                                            row.CreateCell(1).SetCellValue(contrato.Description);
                                            row.CreateCell(2).SetCellValue(contrato.ClientNo);
                                            row.CreateCell(3).SetCellValue(contrato.ClientName);
                                            row.CreateCell(4).SetCellValue(contrato.CodeRegion);
                                            row.CreateCell(5).SetCellValue(ClientRequisitionVM.StartDate);
                                            row.CreateCell(6).SetCellValue(ClientRequisitionVM.EndDate);
                                            row.CreateCell(7).SetCellValue(ClientRequisitionVM.ClientRequisitionNo);
                                            row.CreateCell(8).SetCellValue(ClientRequisitionVM.RequisitionDate);
                                            row.CreateCell(9).SetCellValue(ClientRequisitionVM.PromiseNo);
                                            row.CreateCell(10).SetCellValue(ClientRequisitionVM.InvoiceGroup);
                                            row.CreateCell(11).SetCellValue(ClientRequisitionVM.ProjectNo);
                                            if (!string.IsNullOrEmpty(ClientRequisitionVM.ProjectNo))
                                            {
                                                Projetos Project = AllProjects.Where(x => x.NºProjeto == ClientRequisitionVM.ProjectNo).FirstOrDefault();
                                                if (Project != null && !string.IsNullOrEmpty(Project.Descrição))
                                                    row.CreateCell(12).SetCellValue(Project.Descrição);
                                            }
                                            row.CreateCell(13).SetCellValue(ClientRequisitionVM.LastInvoiceDate);

                                            count++;
                                            row = excelSheet.CreateRow(count);
                                            newLine = false;
                                        }
                                    }
                                }
                            }

                            if (newLine == true)
                                count++;
                            else
                                newLine = true;
                        }
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return Json(sFileName);
        }
        //2
        public IActionResult ExportLastNotaEncomendaDownload(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\" + sFileName;
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }










        [HttpPost]
        public async Task<JsonResult> ExportToAllNotaEncomenda([FromBody] List<ContractViewModel> Lista)
        {
            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            List<Projetos> AllProjects = DBProjects.GetAll();

            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Requisições Cliente");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Nº Contrato");
                row.CreateCell(1).SetCellValue("Âmbito dos Serviços");
                row.CreateCell(2).SetCellValue("Nº Cliente");
                row.CreateCell(3).SetCellValue("Cliente");
                row.CreateCell(4).SetCellValue("Cod. Região");
                row.CreateCell(5).SetCellValue("Início Compromisso");
                row.CreateCell(6).SetCellValue("Fim Compromisso");
                row.CreateCell(7).SetCellValue("Nº Requisição Cliente");
                row.CreateCell(8).SetCellValue("Data Requisição");
                row.CreateCell(9).SetCellValue("Nº Compromisso");
                row.CreateCell(10).SetCellValue("Grupo Fatura");
                row.CreateCell(11).SetCellValue("Nº Projeto");
                row.CreateCell(12).SetCellValue("Descrição");
                row.CreateCell(13).SetCellValue("Data Última Fatura");

                if (Lista != null && Lista.Count > 0)
                {
                    int count = 1;
                    bool newLine = true;
                    foreach (ContractViewModel contrato in Lista)
                    {
                        if (contrato != null)
                        {
                            row = excelSheet.CreateRow(count);

                            row.CreateCell(0).SetCellValue(contrato.ContractNo);
                            row.CreateCell(1).SetCellValue(contrato.Description);
                            row.CreateCell(2).SetCellValue(contrato.ClientNo);
                            row.CreateCell(3).SetCellValue(contrato.ClientName);
                            row.CreateCell(4).SetCellValue(contrato.CodeRegion);

                            List<RequisiçõesClienteContrato> AllClientRequisitions = DBContractClientRequisition.GetByContract(contrato.ContractNo);

                            if (AllClientRequisitions != null && AllClientRequisitions.Count > 0)
                            {
                                foreach (RequisiçõesClienteContrato item in AllClientRequisitions)
                                {
                                    ContractClientRequisitionViewModel ClientRequisition = DBContractClientRequisition.ParseToViewModel(item);

                                    row.CreateCell(0).SetCellValue(contrato.ContractNo);
                                    row.CreateCell(1).SetCellValue(contrato.Description);
                                    row.CreateCell(2).SetCellValue(contrato.ClientNo);
                                    row.CreateCell(3).SetCellValue(contrato.ClientName);
                                    row.CreateCell(4).SetCellValue(contrato.CodeRegion);
                                    row.CreateCell(5).SetCellValue(ClientRequisition.StartDate);
                                    row.CreateCell(6).SetCellValue(ClientRequisition.EndDate);
                                    row.CreateCell(7).SetCellValue(ClientRequisition.ClientRequisitionNo);
                                    row.CreateCell(8).SetCellValue(ClientRequisition.RequisitionDate);
                                    row.CreateCell(9).SetCellValue(ClientRequisition.PromiseNo);
                                    row.CreateCell(10).SetCellValue(ClientRequisition.InvoiceGroup);
                                    row.CreateCell(11).SetCellValue(ClientRequisition.ProjectNo);
                                    if (!string.IsNullOrEmpty(ClientRequisition.ProjectNo))
                                    {
                                        Projetos Project = AllProjects.Where(x => x.NºProjeto == ClientRequisition.ProjectNo).FirstOrDefault();
                                        if (Project != null && !string.IsNullOrEmpty(Project.Descrição))
                                            row.CreateCell(12).SetCellValue(Project.Descrição);
                                    }
                                    row.CreateCell(13).SetCellValue(ClientRequisition.LastInvoiceDate);

                                    count++;
                                    row = excelSheet.CreateRow(count);
                                    newLine = false;
                                }
                            }

                            if (newLine == true)
                                count++;
                            else
                                newLine = true;
                        }
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return Json(sFileName);
        }
        //2
        public IActionResult ExportAllNotaEncomendaDownload(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contratos\\" + "tmp\\" + sFileName;
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public string GetTextoFatura(DateTime DataInicioFatura, DateTime DataFimFatura)
        {
            string TextoFatura = "";
            string mes = DataFimFatura.ToString("MMMM");

            int DiaInicio = DataInicioFatura.Day;
            string DiaInicioTexto = DiaInicio < 10 ? "0" + DiaInicio.ToString() : DiaInicio.ToString();
            int MesInicio = DataInicioFatura.Month;
            string MesInicioTexto = MesInicio < 10 ? "0" + MesInicio.ToString() : MesInicio.ToString();
            int AnoInicio = DataInicioFatura.Year;
            string AnoInicioTexto = AnoInicio.ToString();

            int DiaFim = DataFimFatura.Day;
            string DiaFimTexto = DiaFim < 10 ? "0" + DiaFim.ToString() : DiaFim.ToString();
            int MesFim = DataFimFatura.Month;
            string MesFimTexto = MesFim < 10 ? "0" + MesFim.ToString() : MesFim.ToString();
            int AnoFim = DataFimFatura.Year;
            string AnoFimTexto = AnoFim.ToString();

            var UltimoDia = DateTime.DaysInMonth(AnoFim, MesFim);

            if (DataInicioFatura == DataFimFatura)
            {
                TextoFatura = DiaInicioTexto + "/" + MesInicioTexto + "/" + AnoInicioTexto;
            }
            else
            {
                if (DiaInicio == 1)
                {
                    if (DiaFim == UltimoDia && MesInicio == MesFim && AnoInicio == AnoFim)
                    {
                        TextoFatura = DataInicioFatura.ToString("MMMM").ToUpper() + "/" + AnoFimTexto;
                    }
                    else
                    {
                        if (DiaFim == UltimoDia && MesInicio != MesFim && AnoInicio == AnoFim)
                        {
                            TextoFatura = DataInicioFatura.ToString("MMMM").ToUpper() + " a " + DataFimFatura.ToString("MMMM").ToUpper() + "/" + AnoFimTexto;
                        }
                        else
                        {
                            if (DiaFim == UltimoDia && MesInicio != MesFim && AnoInicio != AnoFim)
                            {
                                TextoFatura = DataInicioFatura.ToString("MMMM").ToUpper() + "/" + AnoInicioTexto + " a " + DataFimFatura.ToString("MMMM").ToUpper() + "/" + AnoFimTexto;
                            }
                            else
                            {
                                TextoFatura = DiaInicioTexto + "/" + MesInicioTexto + "/" + AnoInicioTexto + " a " + DiaFimTexto + "/" + MesFimTexto + "/" + AnoFimTexto;
                            }
                        }
                    }
                }
                else
                {
                    TextoFatura = DiaInicioTexto + "/" + MesInicioTexto + "/" + AnoInicioTexto + " a " + DiaFimTexto + "/" + MesFimTexto + "/" + AnoFimTexto;
                }
            }

            return TextoFatura;
        }

        [HttpPost]
        public JsonResult VerificarPDF([FromBody] string pdf)
        {
            string pdfPath = _generalConfig.FileUploadFolder + "FaturasClientes\\" + pdf.Replace("@", "\\");

            if (System.IO.File.Exists(pdfPath))
                return Json(true);
            else
                return Json(false);
        }

        [Route("Contratos/LoadPDF/{pdf}")]
        public ActionResult LoadPDF(string pdf)
        {
            string pdfPath = _generalConfig.FileUploadFolder + "FaturasClientes\\" + pdf.Replace("@", "\\");

            if (System.IO.File.Exists(pdfPath))
            {
                var stream = new FileStream(pdfPath, FileMode.Open, FileAccess.Read);
                var result = new FileStreamResult(stream, "application/pdf");

                return result;
            }
            else
                return null;
        }

        public static string MakeEmailBodyContent(string BodyText)
        {
            string Body = @"<html>" +
                                "<head>" +
                                    "<style>" +
                                        "table{border:0;} " +
                                        "td{width:600px; vertical-align: top;}" +
                                    "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<table>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Caro (a)," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                BodyText +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Com os melhores cumprimentos," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<i>SUCH - Serviço de Utilização Comum dos Hospitais</i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";

            return Body;
        }
    }
}