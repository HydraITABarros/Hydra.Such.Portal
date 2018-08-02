using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Contracts
{
    public class DBAuthorizeInvoiceContracts
    {
        public static AutorizarFaturaçãoContratos Create(AutorizarFaturaçãoContratos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AutorizarFaturaçãoContratos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static AutorizarFaturaçãoContratos Update(AutorizarFaturaçãoContratos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.AutorizarFaturaçãoContratos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
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
                    ctx.AutorizarFaturaçãoContratos.RemoveRange(ctx.AutorizarFaturaçãoContratos.ToList());
                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DeleteAllAllowedInvoiceAndLines()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<AutorizarFaturaçãoContratos> all = GetAll();
                    foreach (var item in all)
                    {
                        if (item.NãoFaturar != true)
                        {
                            ctx.Remove(item);
                        }

                        List<LinhasFaturaçãoContrato> lines = DBInvoiceContractLines.GetById(item.NºContrato);
                        foreach (var line in lines)
                        {
                            DBInvoiceContractLines.DeleteById(line);
                        }
                    }
                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


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
        public static List<AutorizarFaturaçãoContratos> GetAllByContGroup(string ContractNo/*, int? Group*/)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AutorizarFaturaçãoContratos.Where(x => x.NºContrato == ContractNo /*&& x.GrupoFatura == Group*/).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<AutorizarFaturaçãoContratos> GetPedding()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AutorizarFaturaçãoContratos.Where(x => x.Situação!="").ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
