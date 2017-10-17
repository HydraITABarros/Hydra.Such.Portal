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

        public static bool Delete(FolhasDeHoras ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.FolhasDeHoras.RemoveRange(ctx.FolhasDeHoras.Where(x => x.NºFolhaDeHoras == ObjectToDelete.NºFolhaDeHoras));
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
                        DateDepartureTime = x.DataHoraPartida,
                        DateDepartureTimeText = x.DataHoraPartida.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DateTimeArrival = x.DataHoraChegada,
                        DateTimeArrivalText = x.DataHoraChegada.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
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
                        DateTimeCreationText = x.DataHoraCriação.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DateTimeLastState = x.DataHoraÚltimoEstado,
                        DateTimeLastStateText = x.DataHoraÚltimoEstado.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        UserCreation = x.UtilizadorCriação,
                        DateTimeModification = x.DataHoraModificação,
                        DateTimeModificationText = x.DataHoraModificação.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        UserModification = x.UtilizadorModificação
                    }).ToList(); ;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
