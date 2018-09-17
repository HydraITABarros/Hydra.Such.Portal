using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Approvals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBMenu
    {
        #region CRUD
        public static Menu GetById(int Id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Menu.Where(x => x.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<Menu> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Menu.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Menu Create(Menu ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Menu.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Menu Update(Menu ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Menu.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion

        public static List<Menu> GetByUserId(string userId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<Menu> menus = null;
                    List<int> featuresIds = null;
                    List<int> menusIds = null;

                    // toDo -> get features ids from user id
                    featuresIds = new List<int> { 1, 2, 3, 4 };
                    // list menu id from features                    
                    if (featuresIds != null && featuresIds.Count() > 0)
                        menusIds = ctx.FeaturesMenus.Where(fm=> featuresIds.Contains(fm.IdFeature)).Select(fm => fm.IdMenu).ToList();
                    // get menu
                    if(menusIds != null && menusIds.Count() > 0)
                        menus = ctx.Menu.Where(m => menusIds.Contains(m.Id)).ToList();

                    return menus;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Parses


        #endregion



    }
}
