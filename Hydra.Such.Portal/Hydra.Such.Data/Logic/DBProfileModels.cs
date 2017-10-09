using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBProfileModels
    {
        #region CRUD
        public static PerfisModelo GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PerfisModelo.Where(x => x.Id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<PerfisModelo> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PerfisModelo.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static PerfisModelo Create(PerfisModelo ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PerfisModelo.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static PerfisModelo Update(PerfisModelo ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PerfisModelo.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(PerfisModelo ProfileModelToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    DBUserProfiles.DeleteAllFromProfile(ProfileModelToDelete.Id);
                    ctx.Remove(ProfileModelToDelete);
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

        public static List<PerfisModelo> GetByUserId(string UserId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PerfisUtilizador.Where(x => x.IdUtilizador == UserId).Select(x => x.IdPerfilNavigation).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

    }
}
