using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBTabelaLog
    {
        #region CRUD
        public static TabelaLog GetById(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TabelaLog.Where(x => x.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<TabelaLog> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TabelaLog.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static TabelaLog Create(TabelaLog item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHora = DateTime.Now;
                    ctx.TabelaLog.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static TabelaLog Update(TabelaLog item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHora = DateTime.Now;
                    ctx.TabelaLog.Update(item);
                    ctx.SaveChanges();
                }

                return item;
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
                    TabelaLog userTabelaLog = ctx.TabelaLog.Where(x => x.ID == ID).FirstOrDefault();
                    if (userTabelaLog != null)
                    {
                        ctx.TabelaLog.Remove(userTabelaLog);
                        ctx.SaveChanges();

                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        #endregion
    }
}
