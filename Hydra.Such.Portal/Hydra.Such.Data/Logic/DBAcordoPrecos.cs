using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBAcordoPrecos
    {
        #region CRUD
        public static AcordoPrecos GetById(string NoProcedimento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AcordoPrecosModelView> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcordoPrecos.Select(AcordoPreco => new AcordoPrecosModelView()
                    {
                        NoProcedimento = AcordoPreco.NoProcedimento,
                        DtInicioTexto = AcordoPreco.DtInicio == null ? "" : Convert.ToDateTime(AcordoPreco.DtInicio).ToShortDateString(),
                        DtFimTexto = AcordoPreco.DtFim == null ? "" : Convert.ToDateTime(AcordoPreco.DtFim).ToShortDateString(),
                        ValorTotal = AcordoPreco.ValorTotal
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static AcordoPrecos Create(AcordoPrecos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AcordoPrecos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static AcordoPrecos Update(AcordoPrecos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AcordoPrecos.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string NoProcedimento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    AcordoPrecos userAcordoPrecos = ctx.AcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento).FirstOrDefault();
                    if (userAcordoPrecos != null)
                    {
                        ctx.AcordoPrecos.Remove(userAcordoPrecos);
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
