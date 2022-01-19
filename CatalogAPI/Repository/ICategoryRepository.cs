using CatalogAPI.Models;

namespace CatalogAPI.Repository
{
    public interface ICategoryRepository
    {
        public Task<List<CategoryModel>> GetAllCategoriesAsync();
        public Task<CategoryModel> GetCategoryById(long id);
    }
}
