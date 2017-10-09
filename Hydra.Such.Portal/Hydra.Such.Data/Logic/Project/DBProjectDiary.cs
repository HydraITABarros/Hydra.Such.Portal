using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.Logic.Project
{
    public class DBProjectDiary
    {
        #region CRUD

        public static List<DiárioDeProjeto> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DiárioDeProjeto Create(DiárioDeProjeto ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DiárioDeProjeto.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static DiárioDeProjeto Update(DiárioDeProjeto ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DiárioDeProjeto.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(DiárioDeProjeto ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DiárioDeProjeto.Remove(ObjectToDelete);
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
    }
}
