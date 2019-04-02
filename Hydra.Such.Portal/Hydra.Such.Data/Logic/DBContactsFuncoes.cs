using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBContactsFuncoes
    {
        #region CRUD
        public static ContactosFuncoes GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContactosFuncoes.Where(x => x.ID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ContactosFuncoes> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContactosFuncoes.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static ContactosFuncoes Create(ContactosFuncoes item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataCriacao = DateTime.Now;
                    ctx.ContactosFuncoes.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ContactosFuncoes> Create(List<ContactosFuncoes> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.DataCriacao = DateTime.Now;
                        ctx.ContactosFuncoes.Add(x);
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

        public static ContactosFuncoes Update(ContactosFuncoes item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataAlteracao = DateTime.Now;
                    ctx.ContactosFuncoes.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ContactosFuncoes funcao = ctx.ContactosFuncoes.Where(x => x.ID == id).FirstOrDefault();
                    if (funcao != null)
                    {
                        ctx.ContactosFuncoes.Remove(funcao);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool Delete(ContactosFuncoes item)
        {
            return Delete(new List<ContactosFuncoes> { item });
        }

        public static bool Delete(List<ContactosFuncoes> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ContactosFuncoes.RemoveRange(items);
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

        public static ContactFuncoesViewModel ParseToViewModel(this ContactosFuncoes item)
        {
            if (item != null)
            {
                return new ContactFuncoesViewModel()
                {
                    ID = item.ID,
                    Funcao = item.Funcao,
                    CriadoPor = item.CriadoPor,
                    DataCriacao = item.DataCriacao,
                    DataCriacaoText = item.DataCriacao.HasValue ? item.DataCriacao.Value.ToString("yyyy-MM-dd") : "",
                    AlteradoPor = item.AlteradoPor,
                    DataAlteracao = item.DataAlteracao,
                    DataAlteracaoText = item.DataAlteracao.HasValue ? item.DataAlteracao.Value.ToString("yyyy-MM-dd") : ""
                };
            }
            return null;
        }

        public static List<ContactFuncoesViewModel> ParseToViewModel(this List<ContactosFuncoes> items)
        {
            List<ContactFuncoesViewModel> contacts = new List<ContactFuncoesViewModel>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToViewModel()));
            return contacts;
        }

        public static ContactosFuncoes ParseToDB(this ContactFuncoesViewModel item)
        {
            if (item != null)
            {
                return new ContactosFuncoes()
                {
                    ID = item.ID,
                    Funcao = item.Funcao,
                    CriadoPor = item.CriadoPor,
                    DataCriacao = string.IsNullOrEmpty(item.DataCriacaoText) ? (DateTime?)null : DateTime.Parse(item.DataCriacaoText),
                    AlteradoPor = item.AlteradoPor,
                    DataAlteracao = string.IsNullOrEmpty(item.DataAlteracaoText) ? (DateTime?)null : DateTime.Parse(item.DataAlteracaoText)
                };
            }
            return null;
        }

        public static List<ContactosFuncoes> ParseToDB(this List<ContactFuncoesViewModel> items)
        {
            List<ContactosFuncoes> contacts = new List<ContactosFuncoes>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToDB()));
            return contacts;
        }
    }
}
