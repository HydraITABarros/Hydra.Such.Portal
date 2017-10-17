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
                        ProjectNo = cFolhaDeHora.NºProjeto,
                        EmployeeNo = cFolhaDeHora.NºEmpregado,
                        DateDepartureTime = cFolhaDeHora.DataHoraPartida,
                        DateTimeArrival = cFolhaDeHora.DataHoraChegada,
                        TypeDeslocation = cFolhaDeHora.TipoDeslocação,
                        CodeTypeKms = cFolhaDeHora.CódigoTipoKmS,
                        DisplacementOutsideCity = cFolhaDeHora.DeslocaçãoForaConcelho,
                        Validators = cFolhaDeHora.Validadores,
                        Status = cFolhaDeHora.Estado,
                        CreatedBy = cFolhaDeHora.CriadoPor,
                        DateTimeCreation = cFolhaDeHora.DataHoraCriação,
                        DateTimeLastState = cFolhaDeHora.DataHoraÚltimoEstado,
                        UserCreation = cFolhaDeHora.UtilizadorCriação,
                        DateTimeModification = cFolhaDeHora.DataHoraModificação,
                        UserModification = cFolhaDeHora.UtilizadorModificação
                    };

                    return Json(result);
                }

                return Json(new FolhaDeHoraDetailsViewModel());
            }
            return Json(false);
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
                    data.FolhaDeHorasNo = DBNumerationConfigurations.GetNextNumeration(FolhaDeHoraNumerationConfigurationId);

                    FolhasDeHoras cFolhaDeHora = new FolhasDeHoras()
                    {
                        NºFolhaDeHoras = data.FolhaDeHorasNo,
                        Área = data.Area,
                        NºProjeto = data.ProjectNo,
                        NºEmpregado = data.EmployeeNo,
                        DataHoraPartida = data.DateDepartureTime,
                        DataHoraChegada = data.DateTimeArrival,
                        TipoDeslocação = data.TypeDeslocation,
                        CódigoTipoKmS = data.CodeTypeKms,
                        DeslocaçãoForaConcelho = data.DisplacementOutsideCity,
                        Validadores = data.Validators,
                        Estado = data.Status,
                        CriadoPor = data.CreatedBy,
                        DataHoraCriação = data.DateTimeCreation,
                        DataHoraÚltimoEstado = data.DateTimeLastState,
                        UtilizadorCriação = data.UserCreation,
                        DataHoraModificação = data.DateTimeModification,
                        UtilizadorModificação = data.UserModification
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
                    DataHoraPartida = data.DateDepartureTime,
                    DataHoraChegada = data.DateTimeArrival,
                    TipoDeslocação = data.TypeDeslocation,
                    CódigoTipoKmS = data.CodeTypeKms,
                    DeslocaçãoForaConcelho = data.DisplacementOutsideCity,
                    Validadores = data.Validators,
                    Estado = data.Status,
                    CriadoPor = data.CreatedBy,
                    DataHoraCriação = data.DateTimeCreation,
                    DataHoraÚltimoEstado = data.DateTimeLastState,
                    UtilizadorCriação = data.UserCreation,
                    DataHoraModificação = data.DateTimeModification,
                    UtilizadorModificação = data.UserModification
                };

                DBFolhasDeHoras.Update(cFolhaDeHora);
                return Json(data);
            }
            return Json(false);
        }

        #endregion
    }
}
