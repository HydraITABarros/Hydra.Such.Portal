using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBAttachments
    {
        #region CRUD
        public static Anexos GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Anexos.Where(x => x.NºLinha == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Anexos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Anexos.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Anexos Create(Anexos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraCriação = DateTime.Now;
                    ctx.Anexos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Anexos> Create(List<Anexos> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.DataHoraCriação = DateTime.Now;
                        ctx.Anexos.Add(x);
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

        public static Anexos Update(Anexos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraModificação = DateTime.Now;
                    ctx.Anexos.Update(item);
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
                    Anexos attachment = ctx.Anexos.Where(x => x.NºLinha == id).FirstOrDefault();
                    if (attachment != null)
                    {
                        ctx.Anexos.Remove(attachment);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool Delete(Anexos item)
        {
            return Delete(new List<Anexos> { item });
        }

        public static bool Delete(List<Anexos> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Anexos.RemoveRange(items);
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
