using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FolhasDeHoras;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBPresencasFolhaDeHoras
    {
        #region CRUD
        public static List<PresençasFolhaDeHoras> GetByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PresençasFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PresençasFolhaDeHoras> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PresençasFolhaDeHoras.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PresençasFolhaDeHoras Create(PresençasFolhaDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.PresençasFolhaDeHoras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PresençasFolhaDeHoras Update(PresençasFolhaDeHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.PresençasFolhaDeHoras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PresençasFolhaDeHoras.RemoveRange(ctx.PresençasFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo));
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

        public static List<PresencasFolhaDeHorasListItemViewModel> GetAllByPresencaToList(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PresençasFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo).Select(x => new PresencasFolhaDeHorasListItemViewModel()
                    {
                        FolhaDeHorasNo = x.NºFolhaDeHoras,
                        Date = x.Data,
                        DateText = x.Data.ToShortDateString(),
                        FirstHourEntry = Convert.ToString(x.Hora1ªEntrada),
                        FirstHourDeparture = Convert.ToString(x.Hora1ªSaída),
                        SecondHourEntry = Convert.ToString(x.Hora2ªEntrada),
                        SecondHourDeparture = Convert.ToString(x.Hora2ªSaída),
                        DateTimeCreation = x.DataHoraCriação,
                        DateTimeCreationText = x.DataHoraCriação.Value.ToShortDateString(),
                        UserCreation = x.UtilizadorCriação,
                        DateTimeModification = x.DataHoraModificação,
                        DateTimeModificationText = x.DataHoraModificação.Value.ToShortDateString(),
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
