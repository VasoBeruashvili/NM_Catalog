using NM_Catalog.ViewModels;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NM_Catalog.Helpers
{
    public static class SessionHelper
    {
        public static CustomerLoginViewModel GetCurrentUser()
        {
            return HttpContext.Current.Session["CurrentUser"] as CustomerLoginViewModel;
        }
    }
}