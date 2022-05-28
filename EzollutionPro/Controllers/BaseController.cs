using EzollutionPro_BAL.Models;
using EzollutionPro_BAL.Services;
using EzollutionPro_BAL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers
{
    [NoCache]
    public class BaseController : Controller
    {        
        public UserModel GetUserInfo()
        {
            return (UserModel)Session["UserInfo"];
        }
        protected ResponseStatus ResponseStatus { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Thread.Sleep(1000);
            ResponseStatus = new ResponseStatus();
            #region
            var user = GetUserInfo();
            string url = this.Request.Url.PathAndQuery;
            if (user != null)
            {
                var role = user.iRoleId;
                ViewBag.CurrentRole = user.sRoleName;
                if (url != "")
                {

                    string[] controller = url.ToString().Split('/');
                    if (controller[1].ToLower() != "home")
                    {
                        List<string> objPath = new List<string>();
                        objPath = MenuService.Instance.GetRights(user.iRoleId);
                        if (!(objPath.Exists(x => x.ToLower().Contains(controller[1].ToLower()))))
                        {
                            TempData["AuthMessage"] = "You are not authorized to view that page.";
                            filterContext.Result = Redirect("/");
                        }
                    }
                }
                ViewBag.User_name = user.sFirstName+ " " +user.sLastName;
                ViewBag.Photo = user.sPhotoUrl;
                ViewBag.Menu = MenuService.Instance.GetMenuRights(user.iRoleId);
                ViewBag.Masters = MenuService.Instance.GetMasters(user.iRoleId);
                base.OnActionExecuting(filterContext);
            }
            else
            {
                filterContext.Result = RedirectToAction("Login", "Account", new { returnUrl = url });
            }
            #endregion
        }
    }

    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException("filterContext");

            var cache = GetCache(filterContext);

            cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            cache.SetValidUntilExpires(false);
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// Get the reponse cache
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual HttpCachePolicyBase GetCache(ResultExecutingContext filterContext)
        {
            return filterContext.HttpContext.Response.Cache;
        }
    }
}