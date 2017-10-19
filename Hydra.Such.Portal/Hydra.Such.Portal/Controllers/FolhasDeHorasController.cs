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
                        AreaText = cFolhaDeHora.Área.Value.ToString(),
                        ProjectNo = cFolhaDeHora.NºProjeto,
                        EmployeeNo = cFolhaDeHora.NºEmpregado,
                        DateDepartureTime = cFolhaDeHora.DataHoraPartida,
                        DateDepartureTimeText = cFolhaDeHora.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                        DateTimeArrival = cFolhaDeHora.DataHoraChegada,
                        DateTimeArrivalText = cFolhaDeHora.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                        TypeDeslocation = cFolhaDeHora.TipoDeslocação,
                        CodeTypeKms = cFolhaDeHora.CódigoTipoKmS,
                        CodeTypeKmsInt = Convert.ToInt16(cFolhaDeHora.CódigoTipoKmS),
                        DisplacementOutsideCityInt = Convert.ToInt16(cFolhaDeHora.DeslocaçãoForaConcelho),
                        Validators = cFolhaDeHora.Validadores,
                        Status = cFolhaDeHora.Estado,
                        CreatedBy = cFolhaDeHora.CriadoPor,
                        DateTimeCreation = cFolhaDeHora.DataHoraCriação,
                        DateTimeCreationText = cFolhaDeHora.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        DateTimeLastState = cFolhaDeHora.DataHoraÚltimoEstado,
                        DateTimeLastStateText = cFolhaDeHora.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                        UserCreation = cFolhaDeHora.UtilizadorCriação,
                        DateTimeModification = cFolhaDeHora.DataHoraModificação,
                        DateTimeModificationText = cFolhaDeHora.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                        UserModification = cFolhaDeHora.UtilizadorModificação
                    };

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
            //3 = Numeração Folhas de Horas
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
                    //3 = Numeração Folhas de Horas
                    Configuração Configs = DBConfigurations.GetById(1);
                    int FolhaDeHoraNumerationConfigurationId = Configs.NumeraçãoFolhasDeHoras.Value;
                    data.FolhaDeHorasNo = DBNumerationConfigurations.GetNextNumeration(FolhaDeHoraNumerationConfigurationId, true);

                    FolhasDeHoras cFolhaDeHora = new FolhasDeHoras()
                    {
                        NºFolhaDeHoras = data.FolhaDeHorasNo,
                        Área = Convert.ToInt16(data.AreaText),
                        NºProjeto = data.ProjectNo,
                        NºEmpregado = data.EmployeeNo,
                        DataHoraPartida = DateTime.Parse(data.DateDepartureTimeText),
                        DataHoraChegada = DateTime.Parse(data.DateTimeArrivalText),
                        TipoDeslocação = data.TypeDeslocation,
                        CódigoTipoKmS = Convert.ToString(data.CodeTypeKmsInt),
                        DeslocaçãoForaConcelho = Convert.ToBoolean(data.DisplacementOutsideCityInt),
                        Validadores = data.Validators,
                        Estado = data.Status,
                        CriadoPor = User.Identity.Name,
                        DataHoraCriação = DateTime.Now,
                        DataHoraÚltimoEstado = DateTime.Now,
                        UtilizadorCriação = User.Identity.Name,
                        DataHoraModificação = DateTime.Now,
                        UtilizadorModificação = User.Identity.Name
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
                    Área = data.Area,
                    NºProjeto = data.ProjectNo,
                    NºEmpregado = data.EmployeeNo,
                    DataHoraPartida = DateTime.Parse(data.DateDepartureTimeText),
                    DataHoraChegada = DateTime.Parse(data.DateTimeArrivalText),
                    TipoDeslocação = data.TypeDeslocation,
                    CódigoTipoKmS = Convert.ToString(data.CodeTypeKmsInt),
                    DeslocaçãoForaConcelho = Convert.ToBoolean(data.DisplacementOutsideCityInt),
                    Validadores = data.Validators,
                    Estado = data.Status,
                    CriadoPor = data.CreatedBy,
                    DataHoraCriação = Convert.ToDateTime(data.DateTimeCreationText),
                    DataHoraÚltimoEstado = System.DateTime.Now,
                    UtilizadorCriação = User.Identity.Name,
                    DataHoraModificação = System.DateTime.Now,
                    UtilizadorModificação = User.Identity.Name
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
    }
}
