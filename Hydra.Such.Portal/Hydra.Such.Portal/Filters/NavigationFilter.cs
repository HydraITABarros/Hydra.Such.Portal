using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;

namespace Hydra.Such.Portal.Filters
{

    public class NavigationFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var httpMethod = context.HttpContext.Request.Method;
            var user = context.HttpContext.User;
            dynamic viewBag = null;
            try { viewBag = ((Controller)(context.Controller)).ViewBag; } catch { }
            if (httpMethod == "GET" && user.Identity.IsAuthenticated && viewBag != null )
            {
                var teste = context.Controller.GetType();
                var session = context.HttpContext.Session;
                var menu = new List<MenuViewModel>();

                if (session.GetString("menu") == null || true)
                {
                    menu = DBMenu.GetAllByUserId(user.Identity.Name).ParseToViewModel();
                    session.SetString("menu", JsonConvert.SerializeObject(menu));
                }
                else
                {
                    menu = JsonConvert.DeserializeObject<List<MenuViewModel>>(session.GetString("menu"));
                }
                viewBag._menu = menu;
            }
            base.OnActionExecuting(context);
        }

    }
}