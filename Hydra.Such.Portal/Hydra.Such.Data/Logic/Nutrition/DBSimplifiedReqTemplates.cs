using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Hydra.Such.Data.ViewModel.Nutrition;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBSimplifiedReqTemplates
    {
        #region CRUD
        public static RequisiçõesSimplificadas GetById(string requisitionId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesSimplificadas
                        .Include("LinhasRequisiçõesSimplificadas")
                        .Where(x => x.NºRequisição == requisitionId && x.RequisiçãoModelo.Value).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<RequisiçõesSimplificadas> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.RequisiçõesSimplificadas
                        .Include("LinhasRequisiçõesSimplificadas")
                        .Where(x => x.RequisiçãoModelo.Value).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static RequisiçõesSimplificadas Create(RequisiçõesSimplificadas item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraCriação = DateTime.Now;
                    item.RequisiçãoModelo = true;
                    ctx.RequisiçõesSimplificadas.Add(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static RequisiçõesSimplificadas Update(RequisiçõesSimplificadas item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraModificação = DateTime.Now;
                    item.RequisiçãoModelo = true;
                    ctx.RequisiçõesSimplificadas.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(RequisiçõesSimplificadas item)
        {
            try
            {
                if (item != null)
                {
                    using (var ctx = new SuchDBContext())
                    {
                        ctx.RequisiçõesSimplificadas.RemoveRange(ctx.RequisiçõesSimplificadas.Where(x => x.NºRequisição == item.NºRequisição && x.RequisiçãoModelo.Value));
                        ctx.LinhasRequisiçõesSimplificadas.RemoveRange(ctx.LinhasRequisiçõesSimplificadas.Where(x => x.NºRequisição == item.NºRequisição));
                        ctx.SaveChanges();
                    }

                    return true;
                }
            }
            catch {  }

            return false;
        }
        #endregion

        #region Parse Utilities
        public static SimplifiedReqTemplateViewModel ParseToViewModel(this RequisiçõesSimplificadas item)
        {
            if (item != null)
            {
                return new SimplifiedReqTemplateViewModel()
                {
                    RequisitionTemplateId = item.NºRequisição,
                    Description = item.Observações,

                    LocationCode = item.CódLocalização,
                    CodeRegion = item.CódigoRegião,
                    CodeFunctionalArea = item.CódigoÁreaFuncional,
                    CodeResponsabilityCenter = item.CódigoCentroResponsabilidade,

                    CreateDate = item.DataHoraCriação.HasValue ? item.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
                    UpdateDate = item.DataHoraModificação.HasValue ? item.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
                    CreateUser = item.UtilizadorCriação,
                    UpdateUser = item.UtilizadorModificação,
                    Lines = DBSimplifiedReqTemplateLines.ParseToViewModel(item.LinhasRequisiçõesSimplificadas.ToList()),
                };
            }
            return null;
        }

        public static List<SimplifiedReqTemplateViewModel> ParseToViewModel(this List<RequisiçõesSimplificadas> items)
        {
            List<SimplifiedReqTemplateViewModel> parsedItems = new List<SimplifiedReqTemplateViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static RequisiçõesSimplificadas ParseToDB(this SimplifiedReqTemplateViewModel item)
        {
            if (item != null)
            {
                return new RequisiçõesSimplificadas()
                {
                    NºRequisição = item.RequisitionTemplateId,
                    Observações = item.Description,
                    CódigoRegião = item.CodeRegion,
                    CódigoÁreaFuncional = item.CodeFunctionalArea,
                    CódLocalização = item.LocationCode,
                    CódigoCentroResponsabilidade = item.CodeResponsabilityCenter,
                    DataHoraCriação = string.IsNullOrEmpty(item.CreateDate) ? (DateTime?)null : DateTime.Parse(item.CreateDate),
                    DataHoraModificação = string.IsNullOrEmpty(item.UpdateDate) ? (DateTime?)null : DateTime.Parse(item.UpdateDate),
                    UtilizadorCriação = item.CreateUser,
                    UtilizadorModificação = item.UpdateUser,
                    LinhasRequisiçõesSimplificadas = item.Lines.ParseToDB()
                };
            }
            return null;
        }

        public static List<RequisiçõesSimplificadas> ParseToDB(this List<SimplifiedReqTemplateViewModel> items)
        {
            List<RequisiçõesSimplificadas> parsedItems = new List<RequisiçõesSimplificadas>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
