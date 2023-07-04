using BusinessObjects;
using DataAccess;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public void SaveCategory(Category category) => CategoryDAO.SaveCategory(category);
        public void UpdateCategory(Category category) => CategoryDAO.UpdateCategory(category);
        public List<Category> GetCategories() => CategoryDAO.GetCategories();
        public Category GetCategoryById(int id) => CategoryDAO.FindCategoryById(id);

        public void DeleteCategory(Category category) => CategoryDAO.DeleteCategory(category);
    }
}
