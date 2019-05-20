using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBPedidosDEV
    {
        #region CRUD
        public static PedidosDEV GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PedidosDEV.Where(x => x.ID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PedidosDEV> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PedidosDEV.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PedidosDEV Create(PedidosDEV item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataCriacao = DateTime.Now;
                    ctx.PedidosDEV.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public static List<PedidosDEV> Create(List<PedidosDEV> items)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            items.ForEach(x =>
        //            {
        //                x.DataHoraCriação = DateTime.Now;
        //                ctx.PedidosDEV.Add(x);
        //            });
        //            ctx.SaveChanges();
        //        }
        //        return items;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public static PedidosDEV Update(PedidosDEV item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataAlteracao = DateTime.Now;
                    ctx.PedidosDEV.Update(item);
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
                    PedidosDEV contact = ctx.PedidosDEV.Where(x => x.ID == id).FirstOrDefault();
                    if (contact != null)
                    {
                        ctx.PedidosDEV.Remove(contact);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        //public static bool Delete(PedidosDEV item)
        //{
        //    return Delete(new List<PedidosDEV> { item });
        //}

        //public static bool Delete(List<PedidosDEV> items)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            ctx.PedidosDEV.RemoveRange(items);
        //            ctx.SaveChanges();
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        #endregion

        public static PedidosDEVViewModel ParseToViewModel(this PedidosDEV item)
        {
            if (item != null)
            {
                return new PedidosDEVViewModel()
                {
                    ID = item.ID,
                    Processo = item.Processo,
                    Descricao = item.Descricao,
                    Acao = item.Acao,
                    URL = item.URL,
                    Estado = item.Estado,
                    DataEstado = item.DataEstado,
                    DataEstadoText = item.DataEstado.HasValue ? Convert.ToDateTime(item.DataEstado).ToString("yyyy-MM-dd") : "",
                    DataPedido = item.DataPedido,
                    DataPedidoText = item.DataPedido.HasValue ? Convert.ToDateTime(item.DataPedido).ToString("yyyy-MM-dd") : "",
                    PedidoPor = item.PedidoPor,
                    DataConclusao = item.DataConclusao,
                    DataConclusaoText = item.DataConclusao.HasValue ? Convert.ToDateTime(item.DataConclusao).ToString("yyyy-MM-dd") : "",
                    Intervenientes = item.Intervenientes,
                    NoHorasPrevistas = item.NoHorasPrevistas,
                    NoHorasRealizadas = item.NoHorasRealizadas,
                    CriadoPor = item.CriadoPor,
                    DataCriacao = item.DataCriacao,
                    DataCriacaoText = item.DataCriacao.HasValue ? Convert.ToDateTime(item.DataCriacao).ToString("yyyy-MM-dd") : "",
                    AlteradoPor = item.AlteradoPor,
                    DataAlteracao = item.DataAlteracao,
                    DataAlteracaoText = item.DataAlteracao.HasValue ? Convert.ToDateTime(item.DataAlteracao).ToString("yyyy-MM-dd") : "",
                };
            }
            return null;
        }

        public static List<PedidosDEVViewModel> ParseToViewModel(this List<PedidosDEV> items)
        {
            List<PedidosDEVViewModel> contacts = new List<PedidosDEVViewModel>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToViewModel()));
            return contacts;
        }

        public static PedidosDEV ParseToDB(this PedidosDEVViewModel item)
        {
            if (item != null)
            {
                return new PedidosDEV()
                {
                    ID = item.ID,
                    Processo = item.Processo,
                    Descricao = item.Descricao,
                    Acao = item.Acao,
                    URL = item.URL,
                    Estado = item.Estado,
                    DataEstado = !string.IsNullOrEmpty(item.DataEstadoText) ? DateTime.Parse(item.DataEstadoText) : (DateTime?)null,
                    DataPedido = !string.IsNullOrEmpty(item.DataPedidoText) ? DateTime.Parse(item.DataPedidoText) : (DateTime?)null,
                    PedidoPor = item.PedidoPor,
                    DataConclusao = !string.IsNullOrEmpty(item.DataConclusaoText) ? DateTime.Parse(item.DataConclusaoText) : (DateTime?)null,
                    Intervenientes = item.Intervenientes,
                    NoHorasPrevistas = item.NoHorasPrevistas,
                    NoHorasRealizadas = item.NoHorasRealizadas,
                    CriadoPor = item.CriadoPor,
                    DataCriacao = item.DataCriacao,
                    AlteradoPor = item.AlteradoPor,
                    DataAlteracao = item.DataAlteracao,
                };
            }
            return null;
        }

        public static List<PedidosDEV> ParseToDB(this List<PedidosDEVViewModel> items)
        {
            List<PedidosDEV> contacts = new List<PedidosDEV>();
            if (items != null)
                items.ForEach(x =>
                    contacts.Add(x.ParseToDB()));
            return contacts;
        }
    }
}
