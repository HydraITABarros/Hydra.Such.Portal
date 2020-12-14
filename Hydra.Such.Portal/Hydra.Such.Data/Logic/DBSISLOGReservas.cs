using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBSISLOGReservas
    {
        #region CRUD
        public static SISLOGReservas GetById(string NoRequisicao, int NoLinha)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.SISLOGReservas.Where(x => x.NoRequisicao == NoRequisicao && x.NoLinha == NoLinha).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<SISLOGReservas> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.SISLOGReservas.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static SISLOGReservas Create(SISLOGReservas item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraCriacao = DateTime.Now;
                    ctx.SISLOGReservas.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static SISLOGReservas Update(SISLOGReservas item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraModificacao = DateTime.Now;
                    ctx.SISLOGReservas.Update(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Delete(string NoRequisicao, int NoLinha)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    SISLOGReservas Reserva = ctx.SISLOGReservas.Where(x => x.NoRequisicao == NoRequisicao && x.NoLinha == NoLinha).FirstOrDefault();
                    if (Reserva != null)
                    {
                        ctx.SISLOGReservas.Remove(Reserva);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        #endregion

    }
}
