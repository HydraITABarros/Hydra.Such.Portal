using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBOrigemDestinoFh
    {
        #region CRUD

        public static OrigemDestinoFh GetById(string id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.OrigemDestinoFh.FirstOrDefault(x => x.Código == id);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<OrigemDestinoFh> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.OrigemDestinoFh.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static OrigemDestinoFh Create(OrigemDestinoFh ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.OrigemDestinoFh.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static OrigemDestinoFh Update(OrigemDestinoFh ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraÚltimaAlteração = DateTime.Now;
                    ctx.OrigemDestinoFh.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(OrigemDestinoFh ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.OrigemDestinoFh.Remove(ObjectToDelete);
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
