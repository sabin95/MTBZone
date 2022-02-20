using CatalogAPI.Commands;
using CatalogAPI.Results;

namespace CatalogAPI.Repository
{
    public interface ICategoriesRepository
    {
        public Task<List<CategoryResult>> GetAllCategoriesAsync();
        public Task<CategoryResult> GetCategoryById(Guid id);
        public Task<CategoryResult> AddCategory(CategoryCommand categoryCommand);
        public Task<CategoryResult> EditCategoryById(Guid id, CategoryCommand categoryCommand);
        public Task DeleteCategoryById(Guid id);
    }
}
