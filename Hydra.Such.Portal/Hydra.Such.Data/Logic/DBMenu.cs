using Hydra.Such.Data.Database;
using Hydra.Such.Data.Extensions;
using Hydra.Such.Data.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Hydra.Such.Data.Enumerations;

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
        public static List<Menu> GetAllFull()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    var FeaturesMenus = ctx.FeaturesMenus.ToList();

                    return ctx.Menu.ToList().Select(m => new Menu()
                    {
                        Action = m.Action,
                        Active = m.Active,
                        Controller = m.Controller,
                        Features = FeaturesMenus.Where(f => f.IdMenu == m.Id).Select(f => (Features)f.IdFeature).ToHashSet(),
                        HtmlAttributes = m.HtmlAttributes,
                        Icon = m.Icon,
                        Id = m.Id,
                        Parent = m.Parent,
                        RouteParameters = m.RouteParameters,
                        Title = m.Title,
                        Weight = m.Weight
                    }).ToList();
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

        public static bool Delete(Menu ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Menu.Remove(ObjectToDelete);
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

        public static List<Menu> GetAllByUserId(string userId)
        {

            try
            {
                using (var ctx = new SuchDBContext())
                {
                    MenuComparer comparer = new MenuComparer();
                    HashSet<Menu> menuList = null;

                    var user = DBUserConfigurations.GetById(userId);
                    if (user == null)
                        throw new Exception();

                    if (user.Administrador && true)
                    {
                        menuList = ctx.Menu.Where(p => p.Active == true).ToHashSet(comparer);
                    }
                    else
                    {
                        HashSet<int> featuresIds = GetFeaturesByUserId(userId);
                        HashSet<int> menusIds = null;

                        // list menu id from features                    
                        if (featuresIds != null && featuresIds.Count() > 0)
                            menusIds = ctx.FeaturesMenus.Where(fm => featuresIds.Contains(fm.IdFeature)).Select(fm => fm.IdMenu).ToHashSet();
                        // get menu                        
                        if (menusIds != null && menusIds.Count() > 0)
                        {
                            var userMenuList = ctx.Menu.Where(m => menusIds.Contains(m.Id) && m.Active).ToHashSet(comparer);
                            var parentMenuList = GetAllParentsByMenuList(userMenuList);
                            menuList = userMenuList.Union(parentMenuList).ToHashSet(comparer);
                        }
                    }

                    return menuList.OrderBy(m => m.Weight).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MenuViewModel> ParseToViewModel(this List<Menu> menu)
        {
            if (menu == null) { return new List<MenuViewModel>(); }
            return menu.GroupBy(m => m.Parent).ToList().ParseToViewModel();
        }

        private static List<MenuViewModel> ParseToViewModel(this List<IGrouping<int?, Menu>> groupedMenu, int? parentId = null)
        {
            List<MenuViewModel> treeMenu = null;

            var parent = groupedMenu.FirstOrDefault(m => m.Key == parentId);

            if (parent != null)
            {
                groupedMenu.Remove(parent);

                treeMenu = parent.Select(m =>
                {
                    var RouteParameters = new Dictionary<string, string>();
                    var HtmlAttributes = new Dictionary<string, string>();
                    try { RouteParameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(m.RouteParameters); } catch { }
                    try { HtmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(m.RouteParameters); } catch { }
                    return new MenuViewModel
                    {
                        Icon = m.Icon,
                        Title = m.Title,
                        Weight = m.Weight,
                        Controller = m.Controller,
                        Action = m.Action,
                        RouteParameters = RouteParameters,
                        HtmlAttributes = HtmlAttributes,
                        Submenu = groupedMenu.ParseToViewModel(m.Id)
                    };
                }).ToList();
            }
            return treeMenu;
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
        private static HashSet<Menu> GetAllParentsByMenuList(HashSet<Menu> menuList)
        {
            MenuComparer comparer = new MenuComparer();
            HashSet<Menu> parents = null;
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    HashSet<int?> parentIds = menuList.Where(m => m.Parent != null).Select(m => m.Parent).ToHashSet();
                    if (parentIds.Count() < 1) { return parents; }
                    parents = ctx.Menu.Where(m => parentIds.Contains(m.Id)).ToHashSet(comparer);
                    if (parents.FirstOrDefault(m => m.Parent != null) != null)
                    {
                        parents.UnionWith(GetAllParentsByMenuList(parents));
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
