using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBAccessProfiles
    {
        #region CRUD
        public static AcessosPerfil GetById(int IdPerfil, int Área, int Funcionalidade)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosPerfil.Where(x => x.IdPerfil == IdPerfil && x.Área == Área && x.Funcionalidade == Funcionalidade).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AcessosPerfil> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosPerfil.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static AcessosPerfil Create(AcessosPerfil ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AcessosPerfil.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static AcessosPerfil Update(AcessosPerfil ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AcessosPerfil.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool DeleteAllFromProfile(int ProfileId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<AcessosPerfil> ProfileAccessesToDelete = ctx.AcessosPerfil.Where(x => x.IdPerfil == ProfileId).ToList();
                    ctx.AcessosPerfil.RemoveRange(ProfileAccessesToDelete);
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

        public static List<AcessosPerfil> GetByProfileModelId(int ProfileModelId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosPerfil.Where(x => x.IdPerfil == ProfileModelId).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
