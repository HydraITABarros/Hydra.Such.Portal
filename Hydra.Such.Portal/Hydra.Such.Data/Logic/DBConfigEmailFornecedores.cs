using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBConfigEmailFornecedores
    {
        #region CRUD
        public static ConfiguraçãoEmailFornecedores GetById(string CodFornecedor, string CResp)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguraçãoEmailFornecedores.Where(x => x.CodFornecedor == CodFornecedor && x.Cresp == CResp).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ConfiguraçãoEmailFornecedores> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ConfiguraçãoEmailFornecedores.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ConfiguraçãoEmailFornecedores Create(ConfiguraçãoEmailFornecedores item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraCriacao = DateTime.Now;
                    ctx.ConfiguraçãoEmailFornecedores.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ConfiguraçãoEmailFornecedores> Create(List<ConfiguraçãoEmailFornecedores> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.DataHoraCriacao = DateTime.Now;
                        ctx.ConfiguraçãoEmailFornecedores.Add(x);
                    });
                    ctx.SaveChanges();
                }

                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ConfiguraçãoEmailFornecedores Update(ConfiguraçãoEmailFornecedores item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraModificacao = DateTime.Now;
                    ctx.ConfiguraçãoEmailFornecedores.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string CodFornecedor, string CResp)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ConfiguraçãoEmailFornecedores EmailFornecedor = ctx.ConfiguraçãoEmailFornecedores.Where(x => x.CodFornecedor == CodFornecedor && x.Cresp == CResp).FirstOrDefault();
                    if (EmailFornecedor != null)
                    {
                        ctx.ConfiguraçãoEmailFornecedores.Remove(EmailFornecedor);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool Delete(ConfiguraçãoEmailFornecedores item)
        {
            return Delete(new List<ConfiguraçãoEmailFornecedores> { item });
        }

        public static bool Delete(List<ConfiguraçãoEmailFornecedores> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ConfiguraçãoEmailFornecedores.RemoveRange(items);
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
    }
}
