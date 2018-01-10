using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public class DBClassificationFilesTechniques
    {
        #region CRUD
        public static List<ClassificaçãoFichasTécnicas> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ClassificaçãoFichasTécnicas.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ClassificaçãoFichasTécnicas Create(ClassificaçãoFichasTécnicas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.ClassificaçãoFichasTécnicas.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ClassificaçãoFichasTécnicas Update(ClassificaçãoFichasTécnicas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.ClassificaçãoFichasTécnicas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(ClassificaçãoFichasTécnicas ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ClassificaçãoFichasTécnicas.Remove(ObjectToDelete);
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

        public static List<ClassificaçãoFichasTécnicas> GetAllFiles()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ClassificaçãoFichasTécnicas.Where(x => x.Tipo == 1).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
