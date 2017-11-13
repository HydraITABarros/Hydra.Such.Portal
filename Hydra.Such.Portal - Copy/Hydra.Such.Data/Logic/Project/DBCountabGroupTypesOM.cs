using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Project
{
    public class DBCountabGroupTypesOM
    {
        public static TiposGrupoContabOmProjeto GetById(int Codigo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TiposGrupoContabOmProjeto.Where(x => x.Código == Codigo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<TiposGrupoContabOmProjeto> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TiposGrupoContabOmProjeto.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static TiposGrupoContabOmProjeto Create(TiposGrupoContabOmProjeto ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.TiposGrupoContabOmProjeto.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static TiposGrupoContabOmProjeto Update(TiposGrupoContabOmProjeto ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.TiposGrupoContabOmProjeto.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool DeleteAllFromProfile(TiposGrupoContabOmProjeto ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.TiposGrupoContabOmProjeto.Remove(ObjectToDelete);
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
