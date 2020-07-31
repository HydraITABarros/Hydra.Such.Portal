using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBConfiguracaoParametros
    {
        public static ConfiguracaoParametros GetById(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguracaoParametros.Where(x => x.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ConfiguracaoParametros GetByParametro(string Parametro)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguracaoParametros.Where(x => x.Parametro == Parametro).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ConfiguracaoParametros> GetListByParametro(string Parametro)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguracaoParametros.Where(x => x.Parametro == Parametro).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ConfiguracaoParametros> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguracaoParametros.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ConfiguracaoParametros Create(ConfiguracaoParametros ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.ConfiguracaoParametros.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ConfiguracaoParametros Update(ConfiguracaoParametros ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.ConfiguracaoParametros.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Delete(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ConfiguracaoParametros config = ctx.ConfiguracaoParametros.Where(x => x.ID == ID).FirstOrDefault();
                    if (config != null)
                    {
                        ctx.ConfiguracaoParametros.Remove(config);
                        ctx.SaveChanges();

                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

    }
}
