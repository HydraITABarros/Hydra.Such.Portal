using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;

namespace Hydra.Such.Data.Logic
{
    public static class DBUserConfigurations
    {

        public static ConfigUtilizadores GetById(string id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfigUtilizadores.Where(x => x.IdUtilizador == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfigUtilizadores GetByEmployeeNo(string EmployeeNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfigUtilizadores.Where(x => x.EmployeeNo == EmployeeNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ConfigUtilizadores> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfigUtilizadores.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfigUtilizadores Create(ConfigUtilizadores ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.ConfigUtilizadores.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfigUtilizadores Update(ConfigUtilizadores ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.ConfigUtilizadores.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

    }
}
