using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBAnexosErros
    {
        #region CRUD
        public static AnexosErros GetById(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AnexosErros.Where(x => x.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AnexosErros> GetByOrigemAndCodigo(int Origem, string Codigo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AnexosErros.Where(x => x.Origem == Origem && x.Codigo == Codigo).OrderByDescending(y => y.DataHora_Criacao).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AnexosErros> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AnexosErros.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static AnexosErros Create(AnexosErros item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHora_Criacao = DateTime.Now;
                    ctx.AnexosErros.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static AnexosErros Update(AnexosErros item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHora_Alteracao = DateTime.Now;
                    ctx.AnexosErros.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    AnexosErros userAnexosErros = ctx.AnexosErros.Where(x => x.ID == ID).FirstOrDefault();
                    if (userAnexosErros != null)
                    {
                        ctx.AnexosErros.Remove(userAnexosErros);
                        ctx.SaveChanges();

                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        #endregion
    }
}
