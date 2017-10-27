using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FolhasDeHoras;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBFolhasDeHoras
    {
        #region CRUD
        public static FolhasDeHoras GetById(string NFolhaDeHora)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FolhasDeHoras.Where(x => x.NºFolhaDeHoras == NFolhaDeHora).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<FolhasDeHoras> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FolhasDeHoras.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static FolhasDeHoras Create(FolhasDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.FolhasDeHoras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static FolhasDeHoras Update(FolhasDeHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.FolhasDeHoras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Delete(string FolhaDeHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.FolhasDeHoras.RemoveRange(ctx.FolhasDeHoras.Where(x => x.NºFolhaDeHoras == FolhaDeHoraNo));
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        public static List<FolhasDeHoras> GetAllByArea(int AreaId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FolhasDeHoras.Where(x => x.Área == AreaId - 1).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<FolhaDeHoraListItemViewModel> GetAllByAreaToList(int AreaId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //return ctx.FolhasDeHoras.Where(x => x.Área == AreaId - 1).Select(x => new FolhaDeHoraListItemViewModel()
                    return ctx.FolhasDeHoras.Select(x => new FolhaDeHoraListItemViewModel()
                    {
                        FolhaDeHorasNo = x.NºFolhaDeHoras,
                        Area = x.Área,
                        AreaText = x.Área.ToString(),
                        ProjectNo = x.NºProjeto,
                        EmployeeNo = x.NºEmpregado,
                        DateTimeDeparture = x.DataHoraPartida,
                        DateDepartureText = x.DataHoraPartida.Value.ToShortDateString(),
                        TimeDepartureText = x.DataHoraPartida.Value.ToShortTimeString(),
                        DateTimeArrival = x.DataHoraChegada,
                        DateArrivalText = x.DataHoraChegada.Value.ToShortDateString(),
                        TimeArrivalText = x.DataHoraChegada.Value.ToShortTimeString(),
                        TypeDeslocation = x.TipoDeslocação,
                        TypeDeslocationText = x.TipoDeslocação.ToString(),
                        CodeTypeKms = x.CódigoTipoKmS,
                        DisplacementOutsideCity = x.DeslocaçãoForaConcelho,
                        DisplacementOutsideCityText = x.DeslocaçãoForaConcelho.ToString(),
                        Validators = x.Validadores,
                        Status = x.Estado,
                        StatusText = x.Estado.ToString(),
                        CreatedBy = x.CriadoPor,
                        DateTimeCreation = x.DataHoraCriação,
                        DateCreationText = x.DataHoraCriação.Value.ToShortDateString(),
                        TimeCreationText = x.DataHoraCriação.Value.ToShortTimeString(),
                        DateTimeLastState = x.DataHoraÚltimoEstado,
                        DateLastStateText = x.DataHoraÚltimoEstado.Value.ToShortDateString(),
                        TimeLastStateText = x.DataHoraÚltimoEstado.Value.ToShortTimeString(),
                        UserCreation = x.CriadoPor,
                        DateTimeModification = x.DataHoraModificação,
                        DateModificationText = x.DataHoraModificação.Value.ToShortDateString(),
                        TimeModificationText = x.DataHoraModificação.Value.ToShortTimeString(),
                        UserModification = x.UtilizadorModificação,
                        EmployeeName = x.NomeEmpregado,
                        CarRegistration = x.Matrícula,
                        Finished = x.Terminada,
                        FinishedText = x.Terminada.ToString(),
                        FinishedBy = x.TerminadoPor,
                        DateTimeFinished = x.DataHoraTerminado,
                        DateFinishedText = x.DataHoraTerminado.Value.ToShortDateString(),
                        TimeFinishedText = x.DataHoraTerminado.Value.ToShortTimeString(),
                        Validated = x.Validado,
                        ValidatedText = x.Validado.ToString(),
                        PlannedScrolling = x.DeslocaçãoPlaneada,
                        PlannedScrollingText = x.DeslocaçãoPlaneada.ToString(),
                        Comments = x.Observações,
                        Responsible1 = x.NºResponsável1,
                        Responsible2 = x.NºResponsável2,
                        Responsible3 = x.NºResponsável3,
                        ValidatorsRHKM = x.ValidadoresRhKm,
                        RegionCode = x.CódigoRegião,
                        AreaCode = x.CódigoÁreaFuncional,
                        CRESPCode = x.CódigoCentroResponsabilidade,
                        Validator = x.Validador,
                        DateTimeValidation = x.DataHoraValidação,
                        DateValidationText = x.DataHoraValidação.Value.ToShortDateString(),
                        TimeValidationText = x.DataHoraValidação.Value.ToShortTimeString(),
                        IntegratorRH = x.IntegradorEmRh,
                        DateTimeIntegrationRH = x.DataIntegraçãoEmRh,
                        DateIntegrationRHText = x.DataIntegraçãoEmRh.Value.ToShortDateString(),
                        TimeIntegrationRHText = x.DataIntegraçãoEmRh.Value.ToShortTimeString(),
                        IntegratorRHKM = x.IntegradorEmRhKm,
                        DateTimeIntegrationRHKM = x.DataIntegraçãoEmRhKm,
                        DateIntegrationRHKMText = x.DataIntegraçãoEmRhKm.Value.ToShortDateString(),
                        TimeIntegrationRHKMText = x.DataIntegraçãoEmRhKm.Value.ToShortTimeString()
                    }).ToList(); ;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region PERCURSO
        #endregion
    }
}
