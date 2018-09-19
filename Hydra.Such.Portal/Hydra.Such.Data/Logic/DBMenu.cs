using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
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

        public static List<Menu> GetAllByUserId(string userId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<Menu> menuList = null;

                    var user = DBUserConfigurations.GetById(userId);
                    if (user == null)
                        throw new Exception();

                    if (user.Administrador && true)
                    {
                        menuList = ctx.Menu.Where(p => p.Active == true).ToList();
                    }
                    else
                    {
                        HashSet<int> featuresIds = GetFeaturesByUserId(userId);
                        List<int> menusIds = null;
                     
                        // list menu id from features                    
                        if (featuresIds != null && featuresIds.Count() > 0)
                            menusIds = ctx.FeaturesMenus.Where(fm => featuresIds.Contains(fm.IdFeature)).Select(fm => fm.IdMenu).ToList();
                        // get menu                        
                        if (menusIds != null && menusIds.Count() > 0)
                        {
                            var userMenuList = ctx.Menu.Where(m => menusIds.Contains(m.Id) && m.Active).ToList();
                            var parentMenuList = GetAllParentsByMenuList(userMenuList);
                            menuList = userMenuList.Union(parentMenuList).ToList();
                        }
                    }
                    menuList = menuList.OrderBy(m => m.Parent).ToList();

                    return menuList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static HashSet<int> GetFeaturesByUserId(string userId)
        {
            HashSet<int> result = new HashSet<int>();

            var listProfiles = DBUserProfiles.GetByUserId(userId);
            if (listProfiles != null && listProfiles.Count() > 0)
            {
                var listProfilesId = listProfiles.Select(s => s.IdPerfil).ToList();
                foreach (var profileId in listProfilesId)
                {
                    var listAccessProfile = DBAccessProfiles.GetByProfileModelId(profileId).ToList();
                    if (listAccessProfile != null && listAccessProfile.Count() > 0)
                    {
                        var listProfileFeatures = new HashSet<int>();
                        listProfileFeatures = listAccessProfile.Select(s => s.Funcionalidade).ToHashSet<int>();
                        result.UnionWith(listProfileFeatures);
                    }
                }
            }

            var listUserAccess = DBUserAccesses.GetByUserId(userId);
            if (listUserAccess != null && listUserAccess.Count() > 0)
            {
                var listFeatures = new HashSet<int>();
                listFeatures = listUserAccess.Select(s => s.Funcionalidade).ToHashSet<int>();
                result.UnionWith(listFeatures);
            }

            return result;
        }
        private static List<Menu> GetAllParentsByMenuList(List<Menu> menuList)
        {
            List<Menu> parents = null;
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<int?> parentIds = menuList.Where(m => m.Parent != null).Select(m => m.Parent).ToList();
                    if (parentIds.Count() < 1) { return parents; }
                    parents = ctx.Menu.Where(m => parentIds.Contains(m.Id)).ToList();
                    if (parents.FirstOrDefault(m => m.Parent != null) != null)
                    {
                        parents.AddRange(GetAllParentsByMenuList(parents));
                    };
                    return parents;
                }
            }
            catch (Exception ex)
            {
                return parents;
            }
        }

        #region Parses
        /*
        public static List<MenuViewModel> fromMenuListToMenuViewModelList()
        {

        }
        */
        #endregion



    }
}
