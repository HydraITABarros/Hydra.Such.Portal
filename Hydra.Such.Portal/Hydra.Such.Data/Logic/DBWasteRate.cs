using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBWasteRate
    {
        #region CRUD
        public static TaxaResiduos GetById(string NRecurso)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TaxaResiduos.Where(x => x.Recurso == NRecurso).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static List<TaxaResiduos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TaxaResiduos.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static TaxaResiduos Create(TaxaResiduos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.TaxaResiduos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static TaxaResiduos Update(TaxaResiduos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.TaxaResiduos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(TaxaResiduos ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.TaxaResiduos.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion
        public static WasteRateViewModel ParseToViewModel(TaxaResiduos x)
        {
            return new WasteRateViewModel()
            {
               Data = x.Data.HasValue ? x.Data.Value.ToString("yyyy-MM-dd") : "",
               FamiliaRecurso = x.FamiliaRecurso,
               Recurso = x.Recurso,
                DataHoraCriação = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
                DataHoraModificação = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
                UtilizadorCriação = x.UtilizadorCriação,
                UtilizadorModificação = x.UtilizadorModificação,
                Selected = false
            };
        }
        public static List<WasteRateViewModel> ParseToViewModel(List<TaxaResiduos> items)
        {
            List<WasteRateViewModel> parsedItems = new List<WasteRateViewModel>();
            if (items != null && items.Count > 0)
            {
                items.ForEach(x =>
                    parsedItems.Add(ParseToViewModel(x))
                );
            }
            return parsedItems;
        }
        public static TaxaResiduos ParseToDatabase(WasteRateViewModel x)
        {
            return new TaxaResiduos()
            {
              
                Data = x.Data == "" || x.Data == null ? (DateTime?)null : DateTime.Parse(x.Data),
                Recurso = x.Recurso,
                FamiliaRecurso = x.FamiliaRecurso,
                DataHoraCriação = x.DataHoraCriação == "" || x.DataHoraCriação == null ? (DateTime?)null : DateTime.Parse(x.DataHoraCriação),
                DataHoraModificação = x.DataHoraModificação == "" || x.DataHoraModificação == null ? (DateTime?)null : DateTime.Parse(x.DataHoraModificação),
                UtilizadorCriação = x.UtilizadorCriação,
                UtilizadorModificação = x.UtilizadorModificação
            };
        }
        public static List<TaxaResiduos> ParseToDatabase(this List<WasteRateViewModel> items)
        {
            List<TaxaResiduos> WasteRate = new List<TaxaResiduos>();
            if (items != null)
                items.ForEach(x =>
                    WasteRate.Add(ParseToDatabase(x)));
            return WasteRate;
        }
    }
}
