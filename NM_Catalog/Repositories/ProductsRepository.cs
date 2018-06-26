using NM_Catalog.Helpers;
using NM_Catalog.Models;
using NM_Catalog.Utilities;
using NM_Catalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NM_Catalog.Repositories
{
    public static class ProductsRepository
    {
        public static CatalogResponseViewModel GetProducts(int? categoryId, int? subCategoryId, string param, int page)
        {
            using (DataContext ctx = new DataContext())
            {
                string _param = param.ToLower();
                List<Product> products = new List<Product>();

                if (subCategoryId == null)
                {
                    products = ctx.Products.Where(p => p.CategoryId == categoryId).ToList();
                }
                else
                {
                    products = ctx.Products.Where(p => p.SubCategoryId == subCategoryId).ToList();
                }

                //search
                if (!string.IsNullOrEmpty(_param))
                {
                    products = products.Where(p => p.Name.ToLower().Contains(_param) || p.Description.ToLower().Contains(_param)).ToList();
                }
                //---

                var result = new CatalogResponseViewModel();

                if (HttpContext.Current.Session["CurrentAdmin"] != null)
                    result = new CatalogResponseViewModel
                    {
                        Products = products.Select(p => new ProductViewModel
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Price = p.Price,
                            QuantityToAdd = 1,
                            ProductImage = p.ProductImage
                        }).ToList(),
                        TotalItemsCount = products.Count
                    };
                else
                    result = new CatalogResponseViewModel
                    {
                        Products = products.Skip((page - 1) * 10).Take(10).Select(p => new ProductViewModel
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Price = p.Price,
                            QuantityToAdd = 1,
                            ProductImage = p.ProductImage
                        }).ToList(),
                        TotalItemsCount = products.Count
                    };

                return result;
            }
        }

        public static List<ProductViewModel> GetNewlyAddedProducts()
        {
            using (DataContext ctx = new DataContext())
            {
                List<ProductViewModel> newlyAddedProducs = ctx.Products.OrderByDescending(p => p.Id).Take(10).Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductImage = p.ProductImage
                }).ToList();

                if (newlyAddedProducs != null && newlyAddedProducs.Count > 0)
                    newlyAddedProducs[0].IsActive = "active";

                return newlyAddedProducs;
            }
        }

        public static bool AddToCart(int id, int quantity, int userId)
        {
            using (DataContext ctx = new DataContext())
            {
                var shoppingCart = ctx.ShoppingCarts.FirstOrDefault(sc => sc.ProductId == id && sc.CustomerId == userId);

                if (shoppingCart == null)
                    ctx.ShoppingCarts.Add(new ShoppingCart
                    {
                        ProductId = id,
                        Quantity = quantity,
                        CustomerId = userId
                    });
                else
                {
                    shoppingCart.Quantity += quantity;
                    ctx.Entry(shoppingCart).State = EntityState.Modified;
                }

                return ctx.SaveChanges() >= 0;
            }
        }

        public static int GetCartItemsCount(int userId)
        {
            using (DataContext ctx = new DataContext())
            {
                return ctx.ShoppingCarts.Where(sc => sc.CustomerId == userId).Count();
            }
        }

        public static List<ProductViewModel> GetCartItems(int userId)
        {
            using (DataContext ctx = new DataContext())
            {
                List<ShoppingCart> cartProducts = ctx.ShoppingCarts.Where(sc => sc.CustomerId == userId).ToList();

                List<ProductViewModel> products = ctx.Products.ToList().Where(p => cartProducts.Select(ci => ci.ProductId).Contains(p.Id)).Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ProductImage = p.ProductImage
                }).ToList();

                foreach (ProductViewModel p in products)
                {
                    ShoppingCart sc = cartProducts.First(cp => cp.ProductId == p.Id);
                    p.Sum = Math.Round(p.Price * sc.Quantity, 2);
                    p.QuantityToAdd = sc.Quantity;
                }

                return products;
            }
        }

        public static bool RemoveFromChart(int id, int userId)
        {
            using (DataContext ctx = new DataContext())
            {
                var shoppingCart = ctx.ShoppingCarts.FirstOrDefault(sc => sc.ProductId == id && sc.CustomerId == userId);

                if (shoppingCart != null)
                {
                    ctx.Entry(shoppingCart).State = EntityState.Deleted;

                    return ctx.SaveChanges() >= 0;
                }

                return false;
            }
        }

        public static bool SendOrderToEmail(List<OrderViewModel> orders, int userId, string userName, string userEmail)
        {
            try
            {
                using (DataContext ctx = new DataContext())
                {
                    List<ProductViewModel> products = ctx.ShoppingCarts.Include("Product").Where(sc => sc.CustomerId == userId).ToList().Select(sc => new ProductViewModel
                    {
                        Id = sc.ProductId,
                        Name = sc.Product.Name,
                        Price = sc.Product.Price,
                        QuantityToAdd = orders.First(o => o.ProductId == sc.ProductId).Quantity
                    }).ToList();

                    StringBuilder sb = new StringBuilder("მოგესალმებით!\n\nთქვენი შეკვეთა გაგზავნილია და მუშავდება.\n\n");

                    foreach (ProductViewModel product in products)
                    {
                        product.Sum = Math.Round(product.Price * product.QuantityToAdd, 2);
                        sb.AppendLine($"{product.Name} {product.QuantityToAdd}ცალი {product.Price}ლარი | ჯამი: {product.Sum}ლარი.");
                    }

                    sb.AppendLine($"\nსულ ჯამი: {Math.Round(products.Sum(p => p.Sum), 2)}ლარი.");

                    //bool sendResult = EmailHelper.SendEmail(
                    //    $"Bamigroup.ge - შეკვეთა \"{userName}\" ({DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")})", //subject
                    //    "smtp.bamigroup.ge", //smtp
                    //    587, //port
                    //    false, //enable ssl
                    //    "info@bamigroup.ge", //sender name
                    //    "info@bamigroup.ge", //login
                    //    "Bami123!@#", //password
                    //    new List<string> { //bcc(s)
                    //        userEmail,
                    //        "vasil.beruashvili@gmail.com"
                    //    },
                    //    sb.ToString() //message text
                    //);

                    var mail = new MailMessage("info@bamigroup.ge",
                        "vasil.beruashvili@gmail.com")
                    {
                        Subject = $"Bamigroup.ge - შეკვეთა \"{userName}\" ({DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")})",
                        Body = sb.ToString()
                    };

                    var client = new SmtpClient
                    {
                        Port = 587,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = true,
                        Host = "bamigroup.ge"
                    };

                    client.Send(mail);

                    //if (sendResult)
                    //    ctx.ShoppingCarts.RemoveRange(ctx.ShoppingCarts.Where(sc => sc.CustomerId == userId));

                    //return sendResult && ctx.SaveChanges() > 0;

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }



        public static bool SaveProduct(ProductViewModel product)
        {
            using (DataContext ctx = new DataContext())
            {
                Product p = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    SubCategoryId = product.SubCategoryId,
                    ProductImage = product.ProductImage
                };

                if(product.Id == 0) //add new
                {
                    ctx.Products.Add(p);
                }
                else //edit old
                {
                    ctx.Entry(p).State = EntityState.Modified;
                }

                return ctx.SaveChanges() >= 0;
            }
        }
        public static bool DeleteProduct(int productId)
        {
            using (DataContext ctx = new DataContext())
            {
                if (!ctx.ShoppingCarts.Any(sc => sc.ProductId == productId))
                {
                    Product pr = ctx.Products.First(p => p.Id == productId);
                    ctx.Entry(pr).State = EntityState.Deleted;
                }
                else
                    return false;

                return ctx.SaveChanges() >= 0;
            }
        }
    }
}