using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Contracts
{
    public static class DBContractInvoices
    {

        public static List<AutorizarFaturaçãoContratos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AutorizarFaturaçãoContratos.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<AutorizarFaturaçãoContratos> GetAllInvoice()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AutorizarFaturaçãoContratos.Where(x => x.Situação=="").ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<AutorizarFaturaçãoContratos> GetByContractNo(string contractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AutorizarFaturaçãoContratos.Where(x => x.NºContrato == contractNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
