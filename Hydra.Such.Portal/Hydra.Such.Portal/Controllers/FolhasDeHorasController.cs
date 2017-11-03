using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.FolhasDeHoras;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.FolhaDeHora;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.NAV;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.Logic.Project;

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

        #region Home
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetListFolhasDeHorasByArea([FromBody] int id)
        {
            List<FolhaDeHoraListItemViewModel> result = DBFolhasDeHoras.GetAllByAreaToList(id);

            result.ForEach(x =>
            {
                x.AreaText = EnumerablesFixed.Areas.Where(y => y.Id == x.Area).FirstOrDefault().Value;
                x.TypeDeslocationText = EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == x.TypeDeslocation).FirstOrDefault().Value;
                if (x.DisplacementOutsideCity.Value) x.DisplacementOutsideCityText = "Sim"; else x.DisplacementOutsideCityText = "Não";
                x.StatusText = EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
                x.Validators = DBUserConfigurations.GetById(x.Validators).Nome;
            });
            return Json(result);
        }
        #endregion

        #region Details
        public IActionResult Detalhes(String id)
        {
            ViewBag.FolhaDeHorasNo = id == null ? "" : id;
            return View();
        }

        [HttpPost]
        public JsonResult GetFolhaDeHoraDetails([FromBody] FolhaDeHoraDetailsViewModel data)
        {
            if (data != null)
            {
                FolhasDeHoras cFolhaDeHora = DBFolhasDeHoras.GetById(data.FolhaDeHorasNo);

                if (cFolhaDeHora != null)
                {
                    FolhaDeHoraDetailsViewModel result = new FolhaDeHoraDetailsViewModel()
                    {
                        FolhaDeHorasNo = cFolhaDeHora.NºFolhaDeHoras,
                        Area = cFolhaDeHora.Área,
                        AreaText = cFolhaDeHora.Área.ToString(),
                        ProjectNo = cFolhaDeHora.NºProjeto,
                        EmployeeNo = cFolhaDeHora.NºEmpregado,
                        DateTimeDeparture = cFolhaDeHora.DataHoraPartida,
                        DateDepartureText = cFolhaDeHora.DataHoraPartida == null ? String.Empty : cFolhaDeHora.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                        TimeDepartureText = cFolhaDeHora.DataHoraPartida.Value.ToShortTimeString(),
                        DateTimeArrival = cFolhaDeHora.DataHoraChegada,
                        DateArrivalText = cFolhaDeHora.DataHoraChegada == null ? String.Empty : cFolhaDeHora.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                        TimeArrivalText = cFolhaDeHora.DataHoraChegada.Value.ToShortTimeString(),
                        TypeDeslocation = cFolhaDeHora.TipoDeslocação,
                        TypeDeslocationText = cFolhaDeHora.TipoDeslocação.ToString(),
                        CodeTypeKms = cFolhaDeHora.CódigoTipoKmS,
                        DisplacementOutsideCity = cFolhaDeHora.DeslocaçãoForaConcelho,
                        DisplacementOutsideCityText = cFolhaDeHora.DeslocaçãoForaConcelho.ToString(),
                        Validators = cFolhaDeHora.Validadores,
                        Status = cFolhaDeHora.Estado,
                        StatusText = cFolhaDeHora.Estado.ToString(),
                        CreatedBy = cFolhaDeHora.CriadoPor,
                        DateTimeCreation = cFolhaDeHora.DataHoraCriação,
                        DateCreationText = cFolhaDeHora.DataHoraCriação == null ? String.Empty : cFolhaDeHora.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        TimeCreationText = cFolhaDeHora.DataHoraCriação.Value.ToShortTimeString(),
                        DateTimeLastState = cFolhaDeHora.DataHoraÚltimoEstado,
                        DateLastStateText = cFolhaDeHora.DataHoraÚltimoEstado == null ? String.Empty : cFolhaDeHora.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                        TimeLastStateText = cFolhaDeHora.DataHoraÚltimoEstado.Value.ToShortTimeString(),
                        UserCreation = cFolhaDeHora.CriadoPor,
                        DateTimeModification = cFolhaDeHora.DataHoraModificação,
                        DateModificationText = cFolhaDeHora.DataHoraModificação == null ? String.Empty : cFolhaDeHora.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                        TimeModificationText = cFolhaDeHora.DataHoraModificação.Value.ToShortTimeString(),
                        UserModification = cFolhaDeHora.UtilizadorModificação,
                        EmployeeName = cFolhaDeHora.NomeEmpregado,
                        CarRegistration = cFolhaDeHora.Matrícula,
                        Finished = cFolhaDeHora.Terminada,
                        FinishedText = cFolhaDeHora.Terminada.ToString(),
                        FinishedBy = cFolhaDeHora.TerminadoPor,
                        DateTimeFinished = cFolhaDeHora.DataHoraTerminado,
                        DateFinishedText = cFolhaDeHora.DataHoraTerminado == null ? String.Empty : cFolhaDeHora.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                        TimeFinishedText = cFolhaDeHora.DataHoraTerminado.Value.ToShortTimeString(),
                        Validated = cFolhaDeHora.Validado,
                        ValidatedText = cFolhaDeHora.Validado.ToString(),
                        PlannedScrolling = cFolhaDeHora.DeslocaçãoPlaneada,
                        PlannedScrollingText = cFolhaDeHora.DeslocaçãoPlaneada.ToString(),
                        Comments = cFolhaDeHora.Observações,
                        Responsible1 = cFolhaDeHora.NºResponsável1,
                        Responsible2 = cFolhaDeHora.NºResponsável2,
                        Responsible3 = cFolhaDeHora.NºResponsável3,
                        ValidatorsRHKM = cFolhaDeHora.ValidadoresRhKm,
                        RegionCode = cFolhaDeHora.CódigoRegião,
                        AreaCode = cFolhaDeHora.CódigoÁreaFuncional,
                        CRESPCode = cFolhaDeHora.CódigoCentroResponsabilidade,
                        Validator = cFolhaDeHora.Validador,
                        DateTimeValidation = cFolhaDeHora.DataHoraValidação,
                        DateValidationText = cFolhaDeHora.DataHoraValidação == null ? String.Empty : cFolhaDeHora.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                        TimeValidationText = cFolhaDeHora.DataHoraValidação.Value.ToShortTimeString(),
                        IntegratorRH = cFolhaDeHora.IntegradorEmRh,
                        DateTimeIntegrationRH = cFolhaDeHora.DataIntegraçãoEmRh,
                        DateIntegrationRHText = cFolhaDeHora.DataIntegraçãoEmRh == null ? String.Empty : cFolhaDeHora.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                        TimeIntegrationRHText = cFolhaDeHora.DataIntegraçãoEmRh.Value.ToShortTimeString(),
                        IntegratorRHKM = cFolhaDeHora.IntegradorEmRhKm,
                        DateTimeIntegrationRHKM = cFolhaDeHora.DataIntegraçãoEmRhKm,
                        DateIntegrationRHKMText = cFolhaDeHora.DataIntegraçãoEmRhKm == null ? String.Empty : cFolhaDeHora.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                        TimeIntegrationRHKMText = cFolhaDeHora.DataIntegraçãoEmRhKm.Value.ToShortTimeString()
                    };

                    Projetos cProject = DBProjects.GetById(cFolhaDeHora.NºProjeto);
                    result.ProjectDescription = cProject.Descrição;

                    List<NAVEmployeeViewModel> employee = DBNAV2009Employees.GetAll(cFolhaDeHora.NºEmpregado, _config.NAVDatabaseName, _config.NAVCompanyName);
                    result.EmployeeName = employee[0].Name;

                    return Json(result);
                }

                return Json(new FolhaDeHoraDetailsViewModel());
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult ValidateNumeration([FromBody] FolhaDeHoraDetailsViewModel data)
        {
            //Get FolhaDeHora Numeration
            Configuração Cfg = DBConfigurations.GetById(1);
            int FolhaDeHoraNumerationConfigurationId = Cfg.NumeraçãoFolhasDeHoras.Value;

            ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(FolhaDeHoraNumerationConfigurationId);

            //Validate if FolhaDeHorasNo is valid
            if (data.FolhaDeHorasNo != "" && !CfgNumeration.Manual.Value)
            {
                return Json("A numeração configurada para folha de horas não permite inserção manual.");
            }
            else if (data.FolhaDeHorasNo == "" && !CfgNumeration.Automático.Value)
            {
                return Json("É obrigatório inserir o Nº de Folha de Horas.");
            }

            return Json("");
        }

        //eReason = 1 -> Sucess
        //eReason = 2 -> Error creating Project on Databse 
        //eReason = 3 -> Error creating Project on NAV 
        //eReason = 4 -> Unknow Error 
        [HttpPost]
        public JsonResult CreateFolhaDeHora([FromBody] FolhaDeHoraDetailsViewModel data)
        {
            try
            {
                if (data != null)
                {
                    //Get FolhaDeHora Numeration
                    Configuração Configs = DBConfigurations.GetById(1);
                    int FolhaDeHoraNumerationConfigurationId = Configs.NumeraçãoFolhasDeHoras.Value;
                    data.FolhaDeHorasNo = DBNumerationConfigurations.GetNextNumeration(FolhaDeHoraNumerationConfigurationId, true);

                    FolhasDeHoras cFolhaDeHora = new FolhasDeHoras()
                    {
                        NºFolhaDeHoras = data.FolhaDeHorasNo,
                        Área = 1,//Convert.ToInt16(data.AreaText),
                        NºProjeto = data.ProjectNo,
                        NºEmpregado = data.EmployeeNo,
                        DataHoraPartida = DateTime.Parse(string.Concat(data.DateDepartureText, " ", data.TimeDepartureText)),
                        DataHoraChegada = DateTime.Parse(string.Concat(data.DateArrivalText, " ", data.TimeArrivalText)),
                        TipoDeslocação = data.TypeDeslocation,
                        CódigoTipoKmS = data.CodeTypeKms,
                        DeslocaçãoForaConcelho = Convert.ToBoolean(data.DisplacementOutsideCityText),
                        Validadores = User.Identity.Name,//data.Validators,
                        Estado = 1,//Convert.ToUInt16(data.StatusText),
                        CriadoPor = User.Identity.Name,
                        DataHoraCriação = DateTime.Now,
                        DataHoraÚltimoEstado = DateTime.Now,
                        UtilizadorCriação = User.Identity.Name,
                        DataHoraModificação = DateTime.Now,
                        UtilizadorModificação = User.Identity.Name,
                        NomeEmpregado = data.EmployeeNo,
                        Matrícula = data.CarRegistration,
                        Terminada = data.Finished,
                        TerminadoPor = User.Identity.Name,
                        DataHoraTerminado = DateTime.Now,
                        Validado = Convert.ToBoolean(data.ValidatedText),
                        DeslocaçãoPlaneada = Convert.ToBoolean(data.PlannedScrollingText),
                        Observações = data.Comments,
                        NºResponsável1 = User.Identity.Name,//data.Responsible1,
                        NºResponsável2 = User.Identity.Name,//data.Responsible2,
                        NºResponsável3 = User.Identity.Name,//data.Responsible3,
                        ValidadoresRhKm = User.Identity.Name,//data.ValidatorsRHKM,
                        CódigoRegião = data.RegionCode,
                        CódigoÁreaFuncional = data.AreaCode,
                        CódigoCentroResponsabilidade = data.CRESPCode,
                        Validador = User.Identity.Name,//data.Validator,
                        DataHoraValidação = DateTime.Now,
                        IntegradorEmRh = User.Identity.Name,//data.IntegratorRH,
                        DataIntegraçãoEmRh = DateTime.Now,
                        IntegradorEmRhKm = User.Identity.Name,//data.IntegratorRHKM,
                        DataIntegraçãoEmRhKm = DateTime.Now
                    };

                    //Create FolhaDeHora On Database
                    cFolhaDeHora = DBFolhasDeHoras.Create(cFolhaDeHora);

                    if (cFolhaDeHora == null)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro ao criar a Folha de Hora no Portal.";
                    }
                    else
                    {
                        //Update Last Numeration Used
                        ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(FolhaDeHoraNumerationConfigurationId);
                        ConfigNumerations.ÚltimoNºUsado = data.FolhaDeHorasNo;
                        DBNumerationConfigurations.Update(ConfigNumerations);

                        data.eReasonCode = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao criar a Folha de Hora.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateFolhaDeHora([FromBody] FolhaDeHoraDetailsViewModel data)
        {
            if (data != null)
            {
                FolhasDeHoras cFolhaDeHora = new FolhasDeHoras()
                {
                    NºFolhaDeHoras = data.FolhaDeHorasNo,
                    Área = Convert.ToInt16(data.AreaText),
                    NºProjeto = data.ProjectNo,
                    NºEmpregado = data.EmployeeNo,
                    DataHoraPartida = DateTime.Parse(string.Concat(data.DateDepartureText, " ", data.TimeDepartureText)),
                    DataHoraChegada = DateTime.Parse(string.Concat(data.DateArrivalText, " ", data.TimeArrivalText)),
                    TipoDeslocação = Convert.ToInt16(data.TypeDeslocationText),
                    CódigoTipoKmS = data.CodeTypeKms,
                    DeslocaçãoForaConcelho = Convert.ToBoolean(data.DisplacementOutsideCityText),
                    Validadores = data.Validators,
                    Estado = Convert.ToUInt16(data.StatusText),
                    CriadoPor = User.Identity.Name,
                    DataHoraCriação = DateTime.Now,
                    DataHoraÚltimoEstado = DateTime.Now,
                    //UserCreation = User.Identity.Name,
                    DataHoraModificação = DateTime.Now,
                    UtilizadorModificação = User.Identity.Name,
                    NomeEmpregado = data.EmployeeNo,
                    Matrícula = data.CarRegistration,
                    Terminada = data.Finished,
                    TerminadoPor = User.Identity.Name,
                    DataHoraTerminado = DateTime.Now,
                    Validado = Convert.ToBoolean(data.ValidatedText),
                    DeslocaçãoPlaneada = Convert.ToBoolean(data.PlannedScrollingText),
                    Observações = data.Comments,
                    NºResponsável1 = data.Responsible1,
                    NºResponsável2 = data.Responsible2,
                    NºResponsável3 = data.Responsible3,
                    ValidadoresRhKm = data.ValidatorsRHKM,
                    CódigoRegião = data.RegionCode,
                    CódigoÁreaFuncional = data.AreaCode,
                    CódigoCentroResponsabilidade = data.CRESPCode,
                    Validador = data.Validator,
                    DataHoraValidação = DateTime.Now,
                    IntegradorEmRh = data.IntegratorRH,
                    DataIntegraçãoEmRh = DateTime.Now,
                    IntegradorEmRhKm = data.IntegratorRHKM,
                    DataIntegraçãoEmRhKm = DateTime.Now
                };

                DBFolhasDeHoras.Update(cFolhaDeHora);
                return Json(data);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteFolhaDeHoras([FromBody] FolhaDeHoraDetailsViewModel data)
        {

            if (data != null)
            {
                ErrorHandler result = new ErrorHandler();
                DBFolhasDeHoras.Delete(data.FolhaDeHorasNo);
                result = new ErrorHandler()
                {
                    eReasonCode = 0,
                    eMessage = "Folha de Horas removida com sucesso."
                };
                return Json(result);
            }
            return Json(false);
        }
        #endregion

        #region Job Ledger Entry

        public IActionResult MovimentosDeFolhaDeHora(String FolhaDeHoraNo)
        {
            ViewBag.FolhaDeHoraNo = FolhaDeHoraNo;
            return View();
        }

        #endregion

        #region PERCURSO

        [HttpPost]
        public JsonResult PercursoGetAllByFolhaHoraNoToList([FromBody] string FolhaHoraNo)
        {
            try
            {
                List<PercursosEAjudasCustoDespesasFolhaDeHorasListItemViewModel> result = DBPercursosEAjudasCustoDespesasFolhaDeHoras.GetAllByPercursoToList(FolhaHoraNo);

                result.ForEach(x =>
                {
                    //x.AreaText = EnumerablesFixed.Areas.Where(y => y.Id == x.Area).FirstOrDefault().Value;
                    //x.TypeDeslocationText = EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == x.TypeDeslocation).FirstOrDefault().Value;
                    //if (x.DisplacementOutsideCity.Value) x.DisplacementOutsideCityText = "Sim"; else x.DisplacementOutsideCityText = "Não";
                    //x.StatusText = EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
                    //x.Validators = DBUserConfigurations.GetById(x.Validators).Nome;
                });

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region AJUDA

        [HttpPost]
        public JsonResult AjudaGetAllByFolhaHoraNoToList([FromBody] string FolhaHoraNo)
        {
            try
            {
                List<PercursosEAjudasCustoDespesasFolhaDeHorasListItemViewModel> result = DBPercursosEAjudasCustoDespesasFolhaDeHoras.GetAllByAjudaToList(FolhaHoraNo);

                result.ForEach(x =>
                {
                    //x.AreaText = EnumerablesFixed.Areas.Where(y => y.Id == x.Area).FirstOrDefault().Value;
                    //x.TypeDeslocationText = EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == x.TypeDeslocation).FirstOrDefault().Value;
                    //if (x.DisplacementOutsideCity.Value) x.DisplacementOutsideCityText = "Sim"; else x.DisplacementOutsideCityText = "Não";
                    //x.StatusText = EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
                    //x.Validators = DBUserConfigurations.GetById(x.Validators).Nome;
                });

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        #endregion

        #region MÃO-DE-OBRA

        [HttpPost]
        public JsonResult MaoDeObraGetAllByFolhaHoraNoToList([FromBody] string FolhaHoraNo)
        {
            try
            {
                List<MaoDeObraFolhaDeHorasListItemViewModel> result = DBMaoDeObraFolhaDeHoras.GetAllByMaoDeObraToList(FolhaHoraNo);

                result.ForEach(x =>
                {
                    //x.AreaText = EnumerablesFixed.Areas.Where(y => y.Id == x.Area).FirstOrDefault().Value;
                    //x.TypeDeslocationText = EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == x.TypeDeslocation).FirstOrDefault().Value;
                    //if (x.DisplacementOutsideCity.Value) x.DisplacementOutsideCityText = "Sim"; else x.DisplacementOutsideCityText = "Não";
                    //x.StatusText = EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
                    //x.Validators = DBUserConfigurations.GetById(x.Validators).Nome;
                });

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Presença

        [HttpPost]
        public JsonResult PresencasGetAllByFolhaHoraNoToList([FromBody] string FolhaHoraNo)
        {
            try
            {
                List<PresencasFolhaDeHorasListItemViewModel> result = DBPresencasFolhaDeHoras.GetAllByPresencaToList(FolhaHoraNo);

                result.ForEach(x =>
                {
                    //x.AreaText = EnumerablesFixed.Areas.Where(y => y.Id == x.Area).FirstOrDefault().Value;
                    //x.TypeDeslocationText = EnumerablesFixed.FolhaDeHoraTypeDeslocation.Where(y => y.Id == x.TypeDeslocation).FirstOrDefault().Value;
                    //if (x.DisplacementOutsideCity.Value) x.DisplacementOutsideCityText = "Sim"; else x.DisplacementOutsideCityText = "Não";
                    //x.StatusText = EnumerablesFixed.FolhaDeHoraStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
                    //x.Validators = DBUserConfigurations.GetById(x.Validators).Nome;
                });

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
