using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public class DBShoppingNecessity
    {
        #region CRUD
      

        public static List<DiárioRequisiçãoUnidProdutiva> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioRequisiçãoUnidProdutiva.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DiárioRequisiçãoUnidProdutiva Create(DiárioRequisiçãoUnidProdutiva ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.DiárioRequisiçãoUnidProdutiva.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static DiárioRequisiçãoUnidProdutiva Update(DiárioRequisiçãoUnidProdutiva ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.DiárioRequisiçãoUnidProdutiva.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(DiárioRequisiçãoUnidProdutiva ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DiárioRequisiçãoUnidProdutiva.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static List<DiárioRequisiçãoUnidProdutiva> GetByLineNo(int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioRequisiçãoUnidProdutiva.Where(x => x.NºLinha == LineNo).ToList();
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
