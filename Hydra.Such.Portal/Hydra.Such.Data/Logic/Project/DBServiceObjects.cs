using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.Logic.Project
{
   public class DBServiceObjects
    {
        #region CRUD

        public static List<ObjetosDeServiço> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ObjetosDeServiço.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static ObjetosDeServiço Create(ObjetosDeServiço ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ObjetosDeServiço.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static ObjetosDeServiço Update(ObjetosDeServiço ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ObjetosDeServiço.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(ObjetosDeServiço ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ObjetosDeServiço.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static ObjetosDeServiço GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ObjetosDeServiço.FirstOrDefault(x => x.Código == id);
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
