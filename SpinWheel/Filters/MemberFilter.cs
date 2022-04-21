using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace SpinWheel.Filters
{
    public class MemberFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies[".MEMBERAUTH"];
            if (cookie == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {{"action", "Login"}, {"controller", "User"}});
            }
            else
            {
                var ticketInfo = FormsAuthentication.Decrypt(cookie.Value);
                if (ticketInfo != null)
                {
                    var data = ticketInfo.UserData;
                    filterContext.RouteData.Values["UserId"] = data.Split('|')[0];
                    filterContext.RouteData.Values["UserName"] = data.Split('|')[1];
                    filterContext.RouteData.Values["TypeUser"] = data.Split('|')[2];
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {{"action", "Login"}, {"controller", "User"}});
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }

    public class MemberLoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies[".MEMBERAUTH"];
            if (cookie == null)
            {
                filterContext.RouteData.Values["Email"] = "";
                filterContext.RouteData.Values["UserId"] = "";
            }
            else
            {
                var ticketInfo = FormsAuthentication.Decrypt(cookie.Value);
                var arrData = ticketInfo?.UserData.Split('|');
                filterContext.RouteData.Values["Email"] = ticketInfo.Name;
                filterContext.RouteData.Values["UserId"] = arrData[0];
            }

            base.OnActionExecuting(filterContext);
        }
    }
}