using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class ProceduresConfection
    {
        #region CRUD
        public static List<ProcedimentosDeConfeção> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ProcedimentosDeConfeção.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ProcedimentosDeConfeção Create(ProcedimentosDeConfeção ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.ProcedimentosDeConfeção.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ProcedimentosDeConfeção Update(ProcedimentosDeConfeção ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.ProcedimentosDeConfeção.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(ProcedimentosDeConfeção ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ProcedimentosDeConfeção.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        public static List<ProcedimentosDeConfeção> GetAllbyPlateNo(string PlateNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ProcedimentosDeConfeção.Where(x=> x.NºPrato == PlateNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ProcedimentosDeConfeção> GetAllbyActionNo(int actionNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ProcedimentosDeConfeção.Where(x => x.CódigoAção == actionNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        public static ProceduresConfectionViewModel ParseToViewModel(this ProcedimentosDeConfeção item)
        {
            if (item != null)
            {
                return new ProceduresConfectionViewModel()
                {
                    TechnicalSheetNo = item.NºPrato,
                    actionNo = item.CódigoAção,
                    description = item.Descrição,
                    orderNo = item.NºOrdem,
                    CreateDateTime = item.DataHoraCriação,
                    UpdateDateTime = item.DataHoraModificação,
                    CreateUser = item.UtilizadorCriação,
                    UpdateUser = item.UtilizadorModificação
                };
            }
            return null;
        }
        public static List<ProceduresConfectionViewModel> ParseToViewModel(this List<ProcedimentosDeConfeção> items)
        {
            List<ProceduresConfectionViewModel> parsedItems = new List<ProceduresConfectionViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }
        public static ProcedimentosDeConfeção ParseToDatabase(this ProceduresConfectionViewModel item)
        {
            if (item != null)
            {
                return new ProcedimentosDeConfeção()
                {
                    NºPrato=item.TechnicalSheetNo,
                    CódigoAção=item.actionNo,
                    Descrição=item.description,
                    NºOrdem=item.orderNo,
                    DataHoraCriação = item.CreateDateTime,
                    DataHoraModificação = item.UpdateDateTime,
                    UtilizadorCriação = item.UpdateUser,
                    UtilizadorModificação= item.UpdateUser
                };
            }
            return null;
        }
        public static List<ProcedimentosDeConfeção> ParseToDatabase(this List<ProceduresConfectionViewModel> items)
        {
            List<ProcedimentosDeConfeção> parsedItems = new List<ProcedimentosDeConfeção>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDatabase()));
            return parsedItems;
        }
    }
}
