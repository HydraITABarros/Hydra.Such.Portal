using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.Logic
{
    public class DBMealTypes
    {
        #region CRUD
        public static List<TiposRefeição> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TiposRefeição.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static TiposRefeição Create(TiposRefeição ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.TiposRefeição.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static TiposRefeição Update(TiposRefeição ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.TiposRefeição.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(TiposRefeição ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.TiposRefeição.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public static TiposRefeição GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TiposRefeição.FirstOrDefault(x => x.Código == id);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion
    }
}
