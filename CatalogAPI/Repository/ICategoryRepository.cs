using CatalogAPI.Models;

namespace CatalogAPI.Repository
{
    public interface ICategoryRepository
    {
        public Task<List<CategoryModel>> GetAllCategoriesAsync();
        public Task<CategoryModel> GetCategoryById(long id);
        public void AddCategoryById(CategoryModel categoryModel);
        public void EditCategoryById(long id,CategoryModel category);
        public void DeleteCategoryById(long id);
    }
}
