using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBLinhasAcordoPrecos
    {
        #region CRUD
        public static LinhasAcordoPrecos GetById(string NoProcedimento, string NoFornecedor, string CodProduto, DateTime DtValidadeInicio, string Cresp, string Localizacao)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasAcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento && x.NoFornecedor == NoFornecedor &&
                                                        x.CodProduto == CodProduto && x.DtValidadeInicio == DtValidadeInicio &&
                                                        x.Cresp == Cresp && x.Localizacao == Localizacao).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasAcordoPrecos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasAcordoPrecos.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static LinhasAcordoPrecos Create(LinhasAcordoPrecos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataCriacao = DateTime.Now;
                    ctx.LinhasAcordoPrecos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasAcordoPrecos Update(LinhasAcordoPrecos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasAcordoPrecos.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string NoProcedimento, string NoFornecedor, string CodProduto, DateTime DtValidadeInicio, string Cresp, string Localizacao)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    LinhasAcordoPrecos userLinhasAcordoPrecos = ctx.LinhasAcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento && x.NoFornecedor == NoFornecedor &&
                                                        x.CodProduto == CodProduto && x.DtValidadeInicio == DtValidadeInicio &&
                                                        x.Cresp == Cresp && x.Localizacao == Localizacao).FirstOrDefault();
                    if (userLinhasAcordoPrecos != null)
                    {
                        ctx.LinhasAcordoPrecos.Remove(userLinhasAcordoPrecos);
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
