using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface ICategoryRepository
    {
        void SaveCategory(Category category);
        Category GetCategoryById(int id);
        void DeleteCategory(Category category);
        void UpdateCategory(Category category);
        List<Category> GetCategories();
    }
}
