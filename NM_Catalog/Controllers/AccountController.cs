using System.Web.Mvc;
using NM_Catalog.ViewModels;
using NM_Catalog.Utilities;
using System.Linq;
using NM_Catalog.Models;
using NM_Catalog.Helpers;

namespace NM_Catalog.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(CustomerLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (AuthorizeUser(model.Email, model.Password))
                {
                    if(IsUserAdmin(model.Email, model.Password))
                        return RedirectToAction("addProduct", "products");

                    return RedirectToAction("index", "main");
                }

                ViewBag.errorMsg = "ელ. ფოსტა ან პაროლი არასწორია!";
            }
            else
            {
                ViewBag.errorMsg = "ელ. ფოსტა ან პაროლი აუცილებელია!";
            }

            return View();
        }

        [HttpPost]
        public ActionResult Registration(CustomerRegisterViewModel model)
        {            
            if (ModelState.IsValid)
            {
                if (model.Password != model.RepeatedPassword)
                    ViewBag.errorMsg = "განმეორებითი პაროლი არ ემთხვევა ძირითად პაროლს!";
                else
                {
                    using (DataContext ctx = new DataContext())
                    {
                        string hashedPassword = HashHelper.Calc(model.Password);
                        ctx.Customers.Add(new Customer
                        {
                            FullName = model.FullName,
                            Email = model.Email,
                            Password = hashedPassword
                        });

                        if (ctx.SaveChanges() >= 0)
                            return View("login", new CustomerLoginViewModel { Email = model.Email });
                    }
                }
            }
            else
            {
                ViewBag.errorMsg = "ყველა ველის შევსება აუცილებელია!";
            }

            return View();
        }

        bool AuthorizeUser(string email, string password)
        {
            using (DataContext ctx = new DataContext())
            {
                bool result = false;
                string hashedPassword = HashHelper.Calc(password);
                Customer customer = ctx.Customers.FirstOrDefault(c => c.Email == email && c.Password == hashedPassword);

                if (customer != null)
                {
                    Session.Add("CurrentUser", new CustomerLoginViewModel { Id = customer.Id, FullName = customer.FullName, Email = customer.Email });
                    result = true;
                }
                else
                {
                    if(IsUserAdmin(email, password))
                    {
                        Session.Add("CurrentAdmin", new CustomerLoginViewModel { FullName = "Admin" });
                        result = true;
                    }
                }

                return result;
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("CurrentUser");
            return RedirectToAction("index", "main");
        }

        private bool IsUserAdmin(string email, string password)
        {
            bool result = false;
            if (email == "i.baramidze89@gmail.com" && password == "Bami9999")
            {
                result = true;
            }
            return result;
        }
    }
}