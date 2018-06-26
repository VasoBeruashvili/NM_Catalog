using NM_Catalog.Helpers;
using NM_Catalog.Models;
using NM_Catalog.Utilities;
using NM_Catalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NM_Catalog.Controllers
{
    public class ProductsController : BaseController
    {
        public CustomerLoginViewModel _currentUser = SessionHelper.GetCurrentUser();

        public ActionResult Category(int id)
        {
            ViewBag.categoryId = id;
            return View();
        }
        
        [ValidateUserFilter]
        public ActionResult ShoppingCart()
        {
            return View();
        }

        [ValidateAdminFilter]
        public ActionResult AddProduct()
        {
            return View();
        }

        public JsonResult GetProducts(int? categoryId, int? subCategoryId, string param, int page)
        {
            CatalogResponseViewModel result = Repositories.ProductsRepository.GetProducts(categoryId, subCategoryId, param, page);

            return Json(new { products = result.Products, totalItemsCount = result.TotalItemsCount });
        }

        [ValidateUserFilter]
        public JsonResult AddToCart(int id, int quantity)
        {
            return Json(Repositories.ProductsRepository.AddToCart(id, quantity, _currentUser.Id));
        }

        public JsonResult GetCartItemsCount()
        {
            return Json(Repositories.ProductsRepository.GetCartItemsCount(_currentUser == null ? 0 : _currentUser.Id));
        }

        [ValidateUserFilter]
        public JsonResult GetCartItems()
        {
            var products = Repositories.ProductsRepository.GetCartItems(_currentUser.Id);
            return Json(new { products = products, totalSum = Math.Round(products.Sum(p => p.Sum), 2) });
        }

        [ValidateUserFilter]
        public JsonResult RemoveFromChart(int id)
        {
            return Json(Repositories.ProductsRepository.RemoveFromChart(id, _currentUser.Id));
        }

        [ValidateUserFilter]
        public JsonResult SendOrderToEmail(List<OrderViewModel> orders)
        {
            return Json(Repositories.ProductsRepository.SendOrderToEmail(orders, _currentUser.Id, _currentUser.FullName, _currentUser.Email));
        }



        public JsonResult GetCategories()
        {
            return Json(Repositories.CategoriesRepository.GetCategories());
        }

        public JsonResult GetSubCategories(int categoryId)
        {
            return Json(Repositories.CategoriesRepository.GetSubCategories(categoryId));
        }



        public JsonResult SaveSubCat(string name, int catId)
        {
            return Json(Repositories.CategoriesRepository.SaveSubCat(name, catId));
        }
        public JsonResult DeleteSubCat(int subCatId)
        {
            return Json(Repositories.CategoriesRepository.DeleteSubCat(subCatId));
        }
        public JsonResult SaveProduct(ProductViewModel product)
        {
            return Json(Repositories.ProductsRepository.SaveProduct(product));
        }
        public JsonResult DeleteProduct(int id)
        {
            return Json(Repositories.ProductsRepository.DeleteProduct(id));
        }
    }
}