using System.Web.Mvc;

namespace NM_Catalog.Utilities
{
    public class ValidateAdminFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["CurrentAdmin"] == null)
            {
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new RedirectResult("/account/login");
                }
            }
        }
    }
}