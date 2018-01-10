using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public class DBLinesRecordTechnicalOfPlates
    {
        public static List<LinhasFichasTécnicasPratos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasFichasTécnicasPratos.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasFichasTécnicasPratos Create(LinhasFichasTécnicasPratos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.LinhasFichasTécnicasPratos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static LinhasFichasTécnicasPratos Update(LinhasFichasTécnicasPratos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.LinhasFichasTécnicasPratos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(LinhasFichasTécnicasPratos ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasFichasTécnicasPratos.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

    }
}
