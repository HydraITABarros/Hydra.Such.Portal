using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel.Compras;

namespace Hydra.Such.Data.Logic
{
    public static class DBComprador
    {
        #region CRUD
        public static Comprador GetById(string CodComprador)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Comprador.Where(x => x.CodComprador == CodComprador).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Comprador> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Comprador.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Comprador Create(Comprador item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraCriação = DateTime.Now;
                    ctx.Comprador.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Comprador> Create(List<Comprador> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.DataHoraCriação = DateTime.Now;
                        ctx.Comprador.Add(x);
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

        public static Comprador Update(Comprador item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraModificação = DateTime.Now;
                    ctx.Comprador.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string CodComprador)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    Comprador contact = ctx.Comprador.Where(x => x.CodComprador == CodComprador).FirstOrDefault();
                    if (contact != null)
                    {
                        ctx.Comprador.Remove(contact);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool Delete(Comprador item)
        {
            return Delete(new List<Comprador> { item });
        }

        public static bool Delete(List<Comprador> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Comprador.RemoveRange(items);
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

        public static CompradorViewModel ParseToViewModel(this Comprador item)
        {
            if (item != null)
            {
                return new CompradorViewModel()
                {
                    CodComprador = item.CodComprador,
                    NomeComprador = item.NomeComprador,
                    DataHoraCriacao = item.DataHoraCriação,
                    DataHoraModificacao = item.DataHoraModificação,
                    UtilizadorCriacao = item.UtilizadorCriação,
                    UtilizadorModificacao = item.UtilizadorModificação
                };
            }
            return null;
        }

        public static List<CompradorViewModel> ParseToViewModel(this List<Comprador> items)
        {
            List<CompradorViewModel> contacts = new List<CompradorViewModel>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToViewModel()));
            return contacts;
        }

        public static Comprador ParseToDB(this CompradorViewModel item)
        {
            if (item != null)
            {
                return new Comprador()
                {
                    CodComprador = item.CodComprador,
                    NomeComprador = item.NomeComprador,
                    DataHoraCriação = item.DataHoraCriacao,
                    DataHoraModificação = item.DataHoraModificacao,
                    UtilizadorCriação = item.UtilizadorCriacao,
                    UtilizadorModificação = item.UtilizadorModificacao
                };
            }
            return null;
        }

        public static List<Comprador> ParseToDB(this List<CompradorViewModel> items)
        {
            List<Comprador> contacts = new List<Comprador>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToDB()));
            return contacts;
        }
    }
}
