using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Contracts
{
    public static class DBInvoiceContractLines
    {
        #region CRUD

        public static LinhasFaturaçãoContrato Create(LinhasFaturaçãoContrato ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasFaturaçãoContrato.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }
                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool DeleteAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasFaturaçãoContrato.RemoveRange(ctx.LinhasFaturaçãoContrato.ToList());
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

        public static List<LinhasFaturaçãoContrato> GetById(string contractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasFaturaçãoContrato.Where(x => x.NºContrato == contractNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasFaturaçãoContrato> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasFaturaçãoContrato.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
