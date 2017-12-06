using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;


namespace Hydra.Such.Data.Logic
{
    public static class DBPlaces
    {
        #region CRUD

        public static Locais GetById(int Cod)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Locais.Where(x => x.Código == Cod).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Locais> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Locais.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Locais Create(Locais item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraCriação = DateTime.Now;
                    ctx.Locais.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Locais> Create(List<Locais> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.DataHoraCriação = DateTime.Now;
                        ctx.Locais.Add(x);
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

        public static Locais Update(Locais item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraModificação = DateTime.Now;
                    ctx.Locais.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(int cod)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    Locais places = ctx.Locais.Where(x => x.Código == cod).FirstOrDefault();
                    if (places != null)
                    {
                        ctx.Locais.Remove(places);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool Delete(Locais item)
        {
            return Delete(new List<Locais> { item });
        }

        public static bool Delete(List<Locais> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Locais.RemoveRange(items);
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

        public static PlacesViewModel ParseToViewModel(this Locais item)
        {
            if (item != null)
            {
                return new PlacesViewModel()
                {
                    Code = item.Código,
                    Description=item.Descrição,
                    Address=item.Endereço,
                    Locality=item.Localidade,
                    Responsiblerecept = item.ResponsávelReceção,
                    Postalcode = item.CódigoPostal,
                    CreateDate = item.DataHoraCriação.HasValue ? item.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
                    UpdateDate = item.DataHoraModificação.HasValue ? item.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
                    CreateUser = item.UtilizadorCriação,
                    UpdateUser = item.UtilizadorModificação
                };
            }
            return null;
        }

        public static List<PlacesViewModel> ParseToViewModel(this List<Locais> items)
        {
            List<PlacesViewModel> places = new List<PlacesViewModel>();
            if (items != null)
                items.ForEach(x =>
                    places.Add(x.ParseToViewModel()));
            return places;
        }

        public static Locais ParseToDB(this PlacesViewModel item)
        {
            if (item != null)
            {
                return new Locais()
                {
                    Código = item.Code,
                    Descrição = item.Description,
                    Endereço=item.Address,
                    CódigoPostal=item.Postalcode,
                    Localidade=item.Locality,
                    ResponsávelReceção=item.Responsiblerecept,
                    DataHoraCriação = string.IsNullOrEmpty(item.CreateDate) ? (DateTime?)null : DateTime.Parse(item.CreateDate),
                    DataHoraModificação = string.IsNullOrEmpty(item.UpdateDate) ? (DateTime?)null : DateTime.Parse(item.UpdateDate),
                    UtilizadorCriação = item.CreateUser,
                    UtilizadorModificação = item.UpdateUser
                };
            }
            return null;
        }

        public static List<Locais> ParseToDB(this List<PlacesViewModel> items)
        {
            List<Locais> places = new List<Locais>();
            if (items != null)
                items.ForEach(x =>
                    places.Add(x.ParseToDB()));
            return places;
        }
    }
}
