using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBContacts
    {
        #region CRUD
        public static Contactos GetById(string id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contactos.Where(x => x.Nº == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Contactos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contactos.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Contactos Create(Contactos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraCriação = DateTime.Now;
                    ctx.Contactos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Contactos> Create(List<Contactos> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                        {
                            x.DataHoraCriação = DateTime.Now;
                            ctx.Contactos.Add(x);
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

        public static Contactos Update(Contactos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraModificação = DateTime.Now;
                    ctx.Contactos.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    Contactos contact = ctx.Contactos.Where(x => x.Nº == id).FirstOrDefault();
                    if (contact != null)
                    {
                        ctx.Contactos.Remove(contact);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool Delete(Contactos item)
        {
            return Delete(new List<Contactos> { item });
        }

        public static bool Delete(List<Contactos> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Contactos.RemoveRange(items);
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

        public static ContactViewModel ParseToViewModel(this Contactos item)
        {
            if (item != null)
            {
                return new ContactViewModel()
                {
                    Id = item.Nº,
                    Name = item.Nome,
                    CreateDate = item.DataHoraCriação.HasValue ? item.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
                    UpdateDate = item.DataHoraModificação.HasValue ? item.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
                    CreateUser = item.UtilizadorCriação,
                    UpdateUser = item.UtilizadorModificação,
                    City = item.Cidade,
                    ZipCode = item.Cp,
                    Email = item.Email,
                    EmailContact = item.EmailContato,
                    Address = item.Endereço,
                    Regiao = item.Regiao,
                    ContactFunction = item.FunçãoContato,
                    VATNumber = item.Nif,
                    Notes = item.Notas,
                    PersonContact = item.PessoaContato,
                    Phone = item.Telefone,
                    PhoneContact = item.TelefoneContato,
                    MobilePhoneContact = item.TelemovelContato
                };
            }
            return null;
        }

        public static List<ContactViewModel> ParseToViewModel(this List<Contactos> items)
        {
            List<ContactViewModel> contacts = new List<ContactViewModel>();
            if(items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToViewModel()));
            return contacts;
        }

        public static Contactos ParseToDB(this ContactViewModel item)
        {
            if (item != null)
            {
                return new Contactos()
                {
                    Nº = item.Id,
                    Nome = item.Name,
                    DataHoraCriação = string.IsNullOrEmpty(item.CreateDate) ? (DateTime?)null : DateTime.Parse(item.CreateDate),
                    DataHoraModificação = string.IsNullOrEmpty(item.UpdateDate) ? (DateTime?)null : DateTime.Parse(item.UpdateDate),
                    UtilizadorCriação = item.CreateUser,
                    UtilizadorModificação = item.UpdateUser,
                    Cidade = item.City,
                    Cp = item.ZipCode,
                    Email = item.Email,
                    EmailContato = item.EmailContact,
                    Endereço = item.Address,
                    FunçãoContato = item.ContactFunction,
                    Nif = item.VATNumber,
                    Notas = item.Notes,
                    PessoaContato = item.PersonContact,
                    Telefone = item.Phone,
                    TelefoneContato = item.PhoneContact,
                    TelemovelContato = item.MobilePhoneContact
                };
            }
            return null;
        }

        public static List<Contactos> ParseToDB(this List<ContactViewModel> items)
        {
            List<Contactos> contacts = new List<Contactos>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToDB()));
            return contacts;
        }
    }
}
