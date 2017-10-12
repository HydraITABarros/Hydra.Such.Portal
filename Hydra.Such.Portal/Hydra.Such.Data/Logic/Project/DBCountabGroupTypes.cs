using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Project
{
    public class DBCountabGroupTypes
    {
        public static TiposGrupoContabProjeto GetById(int Codigo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TiposGrupoContabProjeto.Where(x => x.Código == Codigo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<TiposGrupoContabProjeto> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TiposGrupoContabProjeto.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static TiposGrupoContabProjeto Create(TiposGrupoContabProjeto ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.TiposGrupoContabProjeto.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static TiposGrupoContabProjeto Update(TiposGrupoContabProjeto ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.TiposGrupoContabProjeto.Update(ObjectToUpdate);
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
                    List<TiposGrupoContabProjeto> ProfileAccessesToDelete = ctx.TiposGrupoContabProjeto.Where(x => x.Código == ProfileId).ToList();
                    ctx.TiposGrupoContabProjeto.RemoveRange(ProfileAccessesToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
