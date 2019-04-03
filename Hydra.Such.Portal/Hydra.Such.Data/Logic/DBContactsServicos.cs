using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBContactsServicos
    {
        #region CRUD
        public static ContactosServicos GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContactosServicos.Where(x => x.ID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ContactosServicos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContactosServicos.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static ContactosServicos Create(ContactosServicos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataCriacao = DateTime.Now;
                    ctx.ContactosServicos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ContactosServicos> Create(List<ContactosServicos> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.DataCriacao = DateTime.Now;
                        ctx.ContactosServicos.Add(x);
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

        public static ContactosServicos Update(ContactosServicos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataAlteracao = DateTime.Now;
                    ctx.ContactosServicos.Update(item);
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
                    ContactosServicos servico = ctx.ContactosServicos.Where(x => x.ID == id).FirstOrDefault();
                    if (servico != null)
                    {
                        ctx.ContactosServicos.Remove(servico);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool Delete(ContactosServicos item)
        {
            return Delete(new List<ContactosServicos> { item });
        }

        public static bool Delete(List<ContactosServicos> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ContactosServicos.RemoveRange(items);
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

        public static ContactServicosViewModel ParseToViewModel(this ContactosServicos item)
        {
            if (item != null)
            {
                return new ContactServicosViewModel()
                {
                    ID = item.ID,
                    Servico = item.Servico,
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

        public static List<ContactServicosViewModel> ParseToViewModel(this List<ContactosServicos> items)
        {
            List<ContactServicosViewModel> contacts = new List<ContactServicosViewModel>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToViewModel()));
            return contacts;
        }

        public static ContactosServicos ParseToDB(this ContactServicosViewModel item)
        {
            if (item != null)
            {
                return new ContactosServicos()
                {
                    ID = item.ID,
                    Servico = item.Servico,
                    CriadoPor = item.CriadoPor,
                    DataCriacao = string.IsNullOrEmpty(item.DataCriacaoText) ? (DateTime?)null : DateTime.Parse(item.DataCriacaoText),
                    AlteradoPor = item.AlteradoPor,
                    DataAlteracao = string.IsNullOrEmpty(item.DataAlteracaoText) ? (DateTime?)null : DateTime.Parse(item.DataAlteracaoText)
                };
            }
            return null;
        }

        public static List<ContactosServicos> ParseToDB(this List<ContactServicosViewModel> items)
        {
            List<ContactosServicos> contacts = new List<ContactosServicos>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToDB()));
            return contacts;
        }
    }
}
