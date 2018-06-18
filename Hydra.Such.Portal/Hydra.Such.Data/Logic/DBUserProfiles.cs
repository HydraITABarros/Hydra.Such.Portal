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
        public static PerfisUtilizador GetById(string userId, int profileId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PerfisUtilizador.Where(x => x.IdUtilizador == userId && x.IdPerfil == profileId).FirstOrDefault();
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
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.PerfisUtilizador.Add(ObjectToCreate);

                    //Add ProfileAccesses to UserAccesses
                    List<AcessosPerfil> ProfileAccessesToAdd = DBAccessProfiles.GetByProfileModelId(ObjectToCreate.IdPerfil);

                    ProfileAccessesToAdd.ForEach(pa =>
                    {
                        AcessosUtilizador AU = DBUserAccesses.GetById(ObjectToCreate.IdUtilizador, pa.Funcionalidade);
                        if (AU != null)
                        {
                            ctx.AcessosUtilizador.Remove(AU);
                            ctx.SaveChanges();
                        }

                        AU = new AcessosUtilizador()
                        {
                            IdUtilizador = ObjectToCreate.IdUtilizador,
                            Funcionalidade = pa.Funcionalidade,
                            Leitura = pa.Leitura,
                            Inserção = pa.Inserção,
                            Modificação = pa.Modificação,
                            Eliminação = pa.Eliminação,
                            DataHoraCriação = DateTime.Now,
                            UtilizadorCriação = pa.UtilizadorCriação
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
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
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

        public static bool Delete(PerfisUtilizador objectToDelete)
        {
            return Delete(new List<PerfisUtilizador>() { objectToDelete });
        }

        public static bool Delete(List<PerfisUtilizador> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PerfisUtilizador.RemoveRange(items);
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

        public static List<PerfisUtilizador> GetByUserId(string UserId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PerfisUtilizador.Where(x => x.IdUtilizador == UserId).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
