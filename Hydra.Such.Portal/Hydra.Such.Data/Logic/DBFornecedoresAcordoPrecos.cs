using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBFornecedoresAcordoPrecos
    {
        #region CRUD
        public static FornecedoresAcordoPrecos GetById(string NoProcedimento, string NoFornecedor)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FornecedoresAcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento && x.NoFornecedor == NoFornecedor).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<FornecedoresAcordoPrecos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FornecedoresAcordoPrecos.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static FornecedoresAcordoPrecos Create(FornecedoresAcordoPrecos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.FornecedoresAcordoPrecos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static FornecedoresAcordoPrecos Update(FornecedoresAcordoPrecos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.FornecedoresAcordoPrecos.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string NoProcedimento, string NoFornecedor)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    FornecedoresAcordoPrecos userFornecedorAcordoPrecos = ctx.FornecedoresAcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento && x.NoFornecedor == NoFornecedor).FirstOrDefault();
                    if (userFornecedorAcordoPrecos != null)
                    {
                        ctx.FornecedoresAcordoPrecos.Remove(userFornecedorAcordoPrecos);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool DeleteByProcedimento(string NoProcedimento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    FornecedoresAcordoPrecos userFornecedorAcordoPrecos = ctx.FornecedoresAcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento).FirstOrDefault();
                    if (userFornecedorAcordoPrecos != null)
                    {
                        ctx.FornecedoresAcordoPrecos.Remove(userFornecedorAcordoPrecos);
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
