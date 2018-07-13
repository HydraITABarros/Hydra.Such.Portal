using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Compras;
using Hydra.Such.Data.ViewModel.Compras;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Compras
{
    public class DBConfigMercadoLocal
    {
        public static List<ConfigMercadoLocal> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfigMercadoLocal.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ConfigMercadoLocal GetByID(string RegiaoMercadoLocal)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfigMercadoLocal.Where(x => x.RegiaoMercadoLocal == RegiaoMercadoLocal).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ConfigMercadoLocal Create(ConfigMercadoLocal ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfigMercadoLocal.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ConfigMercadoLocal Update(ConfigMercadoLocal ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfigMercadoLocal.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool Delete(ConfigMercadoLocal ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfigMercadoLocal.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}