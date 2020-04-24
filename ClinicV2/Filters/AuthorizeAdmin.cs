using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicV2.Filters
{
    public class AuthorizeAdmin : System.Web.Mvc.ActionFilterAttribute, System.Web.Mvc.IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["AdminIsLogin"] == null)
            {
                filterContext.Result = new System.Web.Mvc.RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary {
                        {"Controller", "Account" },
                        { "Action","Login"}
                    });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}