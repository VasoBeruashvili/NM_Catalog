using NM_Catalog.Models;
using NM_Catalog.Utilities;
using NM_Catalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NM_Catalog.Repositories
{
    public static class CategoriesRepository
    {
        public static List<CategoryViewModel> GetCategories()
        {
            using(DataContext ctx = new DataContext())
            {
                return ctx.Categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    FontawesomeIcon = c.FontawesomeIcon
                }).ToList();
            }
        }

        public static List<SubCategoryViewModel> GetSubCategories(int categoryId)
        {
            using (DataContext ctx = new DataContext())
            {
                return ctx.SubCategories.Where(sc => sc.CategoryId == categoryId).OrderBy(sc => sc.Name).Select(sc => new SubCategoryViewModel
                {
                    Id = sc.Id,
                    Name = sc.Name
                }).ToList();
            }
        }




        public static bool SaveSubCat(string name, int catId)
        {
            using (DataContext ctx = new DataContext())
            {
                ctx.SubCategories.Add(new SubCategory
                {
                    Name = name,
                    CategoryId = catId
                });

                return ctx.SaveChanges() >= 0;
            }
        }
        public static bool DeleteSubCat(int subCatId)
        {
            using (DataContext ctx = new DataContext())
            {
                if (!ctx.Products.Any(p => p.SubCategoryId == subCatId))
                {
                    ctx.SubCategories.Remove(ctx.SubCategories.First(sc => sc.Id == subCatId));
                }
                else
                    return false;

                return ctx.SaveChanges() >= 0;
            }
        }
    }
}