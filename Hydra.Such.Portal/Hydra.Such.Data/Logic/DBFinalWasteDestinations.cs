using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.Logic
{
    public class DBFinalWasteDestinations
    {
        #region CRUD
        public static List<DestinosFinaisResíduos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DestinosFinaisResíduos.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static DestinosFinaisResíduos Create(DestinosFinaisResíduos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DestinosFinaisResíduos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static DestinosFinaisResíduos Update(DestinosFinaisResíduos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DestinosFinaisResíduos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(DestinosFinaisResíduos ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DestinosFinaisResíduos.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public static DestinosFinaisResíduos GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DestinosFinaisResíduos.FirstOrDefault(x => x.Código == id);
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
