using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public class DBCoffeeShopMovements
    {
        public static MovimentosCafetariaRefeitório Create(MovimentosCafetariaRefeitório ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.MovimentosCafetariaRefeitório.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
