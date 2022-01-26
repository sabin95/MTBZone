using CatalogAPI.Commands;
using CatalogAPI.Results;

namespace CatalogAPI.Repository
{
    public interface ICategoryRepository
    {
        public Task<List<CategoryResult>> GetAllCategoriesAsync();
        public Task<CategoryResult> GetCategoryById(long id);
        public Task<CategoryResult> AddCategory(CategoryCommand categoryCommand);
        public Task<CategoryResult> EditCategoryById(long id, CategoryCommand categoryCommand);
        public Task DeleteCategoryById(long id);
    }
}
