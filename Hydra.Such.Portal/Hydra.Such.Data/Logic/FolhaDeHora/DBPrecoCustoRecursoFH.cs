using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBPrecoCustoRecursoFH
    {
        public static List<PrecoCustoRecursoFh> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PrecoCustoRecursoFh.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static PrecoCustoRecursoFh Create(PrecoCustoRecursoFh ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.PrecoCustoRecursoFh.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static PrecoCustoRecursoFh Update(PrecoCustoRecursoFh ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.PrecoCustoRecursoFh.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(PrecoCustoRecursoFh ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PrecoCustoRecursoFh.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }
    }
}