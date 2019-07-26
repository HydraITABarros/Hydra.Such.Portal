using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBAttachments
    {
        #region CRUD
        public static List<Anexos> GetById(string id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Anexos.Where(x => x.NºOrigem.ToLower() == id.ToLower()).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //zpgm-Este método deve receber o tipo visto a chave da tabela ser constituída pelos campos “Tipo Origem”, “Nº Origem”, “Nº Linha”
        public static List<Anexos> GetById(TipoOrigemAnexos type, string id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Anexos.Where(
                        x => x.TipoOrigem == type).Where(
                        x => x.NºOrigem.ToLower() == id.ToLower()).ToList();
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

        public static AttachmentsViewModel ParseToViewModel(Anexos x)
        {
            AttachmentsViewModel result = new AttachmentsViewModel()
            {
               DocLineNo = x.NºLinha,
               DocNumber = x.NºOrigem,
               DocType = x.TipoOrigem,
               Url = x.UrlAnexo,
               CreateDateTime = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
               UpdateDateTime = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
               CreateUser = x.UtilizadorCriação,
               UpdateUser = x.UtilizadorModificação
            };

            return result;

        }

        public static List<AttachmentsViewModel> ParseToViewModel(List<Anexos> items)
        {
            List<AttachmentsViewModel> anexos = new List<AttachmentsViewModel>();
            if (items != null)
                items.ForEach(x =>
                    anexos.Add(DBAttachments.ParseToViewModel(x)));
            return anexos;
        }

        public static Anexos ParseToDB(AttachmentsViewModel x)
        {
            Anexos result = new Anexos()
            {
                NºLinha = x.DocLineNo,
                NºOrigem = x.DocNumber,
                TipoOrigem = x.DocType,
                UrlAnexo = x.Url,
                DataHoraCriação = x.CreateDateTime != null ? DateTime.Parse(x.CreateDateTime) : (DateTime?)null,
                DataHoraModificação = x.CreateDateTime != null ? DateTime.Parse(x.CreateDateTime) : (DateTime?)null,
                UtilizadorCriação = x.CreateUser,
                UtilizadorModificação = x.UpdateUser
            };

            return result;

        }

    }
}
