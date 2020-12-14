using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBSISLOGProdutos
    {
        #region CRUD
        public static SISLOGProdutos GetById(string Codigo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.SISLOGProdutos.Where(x => x.Codigo == Codigo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<SISLOGProdutos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.SISLOGProdutos.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static SISLOGProdutos Create(SISLOGProdutos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataAtualizacao = DateTime.Now;
                    ctx.SISLOGProdutos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static SISLOGProdutos Update(SISLOGProdutos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataAtualizacao = DateTime.Now;
                    ctx.SISLOGProdutos.Update(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Delete(string Codigo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    SISLOGProdutos Produto = ctx.SISLOGProdutos.Where(x => x.Codigo == Codigo).FirstOrDefault();
                    if (Produto != null)
                    {
                        ctx.SISLOGProdutos.Remove(Produto);
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
