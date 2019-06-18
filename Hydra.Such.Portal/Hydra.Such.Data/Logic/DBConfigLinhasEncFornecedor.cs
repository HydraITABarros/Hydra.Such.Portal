using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBConfigLinhasEncFornecedor
    {
        #region CRUD
        public static List<ConfigLinhasEncFornecedor> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfigLinhasEncFornecedor.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfigLinhasEncFornecedor GetByVendorNoAndLineNo(string VendorNo, int LineNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfigLinhasEncFornecedor.Where(x => x.VendorNo == VendorNo && x.LineNo == LineNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ConfigLinhasEncFornecedor> GetByVendorNo(string VendorNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfigLinhasEncFornecedor.Where(x => x.VendorNo == VendorNo).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static int GetMaxLineNoByVendorNo(string VendorNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ConfigLinhasEncFornecedor LastVendor = new ConfigLinhasEncFornecedor();
                    LastVendor = ctx.ConfigLinhasEncFornecedor.Where(x => x.VendorNo == VendorNo).ToList().LastOrDefault();
                    if (LastVendor != null)
                        return LastVendor.LineNo;
                    else
                        return 0;
                }
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        public static ConfigLinhasEncFornecedor Create(ConfigLinhasEncFornecedor ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.ConfigLinhasEncFornecedor.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ConfigLinhasEncFornecedor Update(ConfigLinhasEncFornecedor ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.ConfigLinhasEncFornecedor.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(ConfigLinhasEncFornecedor ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfigLinhasEncFornecedor.Remove(ObjectToDelete);
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

        public static ConfigLinhasEncFornecedor ParseToDB(ConfigLinhasEncFornecedorViewModel x)
        {
            if (x == null)
                return null;

            ConfigLinhasEncFornecedor result = new ConfigLinhasEncFornecedor()
            {
                VendorNo = x.VendorNo,
                LineNo = x.LineNo,
                Type = x.Type,
                No = x.No,
                Description = x.Description,
                Description2 = x.Description2,
                Quantity = x.Quantity,
                UnitOfMeasure = x.UnitOfMeasure,
                Valor = x.Valor,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraModificacao = x.DataHoraModificacao,
                UtilizadorModificacao = x.UtilizadorModificacao
            };

            return result;
        }

        public static ConfigLinhasEncFornecedorViewModel ParseToViewModel(ConfigLinhasEncFornecedor x)
        {
            if (x == null)
                return null;

            ConfigLinhasEncFornecedorViewModel result = new ConfigLinhasEncFornecedorViewModel()
            {
                VendorNo = x.VendorNo,
                LineNo = x.LineNo,
                Type = x.Type,
                No = x.No,
                Description = x.Description,
                Description2 = x.Description2,
                Quantity = x.Quantity,
                UnitOfMeasure = x.UnitOfMeasure,
                Valor = x.Valor,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraModificacao = x.DataHoraModificacao,
                UtilizadorModificacao = x.UtilizadorModificacao
            };

            return result;
        }
    }
}
