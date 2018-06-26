using NM_Catalog.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NM_Catalog.Controllers
{
    public class MainController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}