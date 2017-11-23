using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBAutorizacaoFHRH
    {
        public static List<AutorizacaoFHRH> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AutorizacaoFHRH.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static AutorizacaoFHRH Create(AutorizacaoFHRH ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.AutorizacaoFHRH.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static AutorizacaoFHRH Update(AutorizacaoFHRH ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraÚltimaAlteração = DateTime.Now;
                    ctx.AutorizacaoFHRH.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(AutorizacaoFHRH ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AutorizacaoFHRH.Remove(ObjectToDelete);
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