using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBUserProfiles
    {
        #region CRUD
        public static PerfisUtilizador GetById(string UserId, int ProfileId, int Funcionalidade)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PerfisUtilizador.Where(x => x.IdUtilizador == UserId && x.IdPerfil == ProfileId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<PerfisUtilizador> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PerfisUtilizador.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static PerfisUtilizador Create(PerfisUtilizador ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //Add Profile User
                    ctx.PerfisUtilizador.Add(ObjectToCreate);

                    //Add ProfileAccesses to UserAccesses
                    List<AcessosPerfil> ProfileAccessesToAdd = DBAccessProfiles.GetByProfileModelId(ObjectToCreate.IdPerfil);

                    ProfileAccessesToAdd.ForEach(pa =>
                    {
                        AcessosUtilizador AU = DBUserAccesses.GetById(ObjectToCreate.IdUtilizador, pa.Área, pa.Funcionalidade);
                        if (AU != null)
                        {
                            ctx.AcessosUtilizador.Remove(AU);
                            ctx.SaveChanges();
                        }

                        AU = new AcessosUtilizador()
                        {
                            IdUtilizador = ObjectToCreate.IdUtilizador,
                            Área = pa.Área,
                            Funcionalidade = pa.Funcionalidade,
                            Leitura = pa.Leitura,
                            Inserção = pa.Inserção,
                            Modificação = pa.Modificação,
                            Eliminação = pa.Eliminação,
                        };

                        ctx.AcessosUtilizador.Add(AU);
                        ctx.SaveChanges();
                    });
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static PerfisUtilizador Update(PerfisUtilizador ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PerfisUtilizador.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(PerfisUtilizador ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PerfisUtilizador.Remove(ObjectToDelete);
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


        public static bool DeleteAllFromProfile(int ProfileId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PerfisUtilizador.RemoveRange(ctx.PerfisUtilizador.Where(x => x.IdPerfil == ProfileId));
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool DeleteAllFromUser(string userId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PerfisUtilizador.RemoveRange(ctx.PerfisUtilizador.Where(x => x.IdUtilizador == userId));
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
