using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBActionsConfection
    {
        #region CRUD
        public static List<AçõesDeConfeção> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AçõesDeConfeção.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static AçõesDeConfeção Create(AçõesDeConfeção ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.AçõesDeConfeção.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static AçõesDeConfeção Update(AçõesDeConfeção ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.AçõesDeConfeção.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static bool Delete(AçõesDeConfeção ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AçõesDeConfeção.Remove(ObjectToDelete);
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

        public static List<AçõesDeConfeção> GetAllFiles()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AçõesDeConfeção.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static AçõesDeConfeção ParseToDb(this ActionsConfectionViewModel x)
        {
            if (x != null)
            {
                return new AçõesDeConfeção()
                {
                    Código = x.Code,                
                    Descrição = x.Description,
                    DataHoraCriação = x.CreateDate,
                    DataHoraModificação = x.UpdateDate,
                    UtilizadorCriação = x.UpdateUser,
                    UtilizadorModificação = x.UpdateUser
                };
            }
            return null;
        }
        public static List<AçõesDeConfeção> ParseToDatabase(this List<ActionsConfectionViewModel> items)
        {
            List<AçõesDeConfeção> itemsParse = new List<AçõesDeConfeção>();
            if (items != null)
                items.ForEach(x =>
                    itemsParse.Add(ParseToDb(x)));
            return itemsParse;
        }

        public static ActionsConfectionViewModel ParseToViewModel(this AçõesDeConfeção item)
        {
            if (item != null)
            {
                return new ActionsConfectionViewModel()
                {

                    Code = item.Código,                   
                    Description = item.Descrição,
                    CreateDate = item.DataHoraCriação,
                    UpdateDate = item.DataHoraModificação,
                    CreateUser = item.UtilizadorCriação,
                    UpdateUser = item.UtilizadorModificação

                };
            }
            return null;
        }

        public static List<ActionsConfectionViewModel> ParseToViewModel(this List<AçõesDeConfeção> items)
        {
            List<ActionsConfectionViewModel> parsedItems = new List<ActionsConfectionViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(ParseToViewModel(x)));
            return parsedItems;
        }

    
    }
}
