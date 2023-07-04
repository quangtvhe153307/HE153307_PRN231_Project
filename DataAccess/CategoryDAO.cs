using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDAO
    {
        public static List<Category> GetCategories()
        {
            var listCategories = new List<Category>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listCategories = context.Categories
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listCategories;
        }
        public static Category FindCategoryById(int prodId)
        {
            Category category = null;
            try
            {
                using (var context = new MyDbContext())
                {
                    category = context.Categories
                        .SingleOrDefault(x => x.CategoryId == prodId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return category;
        }
        public static void SaveCategory(Category category)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Categories.Add(category);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateCategory(Category category)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry<Category>(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteCategory(Category category)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.Categories.SingleOrDefault(x => x.CategoryId == category.CategoryId);
                    context.Categories.Remove(p1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
