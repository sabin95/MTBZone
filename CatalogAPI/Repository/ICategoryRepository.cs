using CatalogAPI.Results;

namespace CatalogAPI.Repository
{
    public interface ICategoryRepository
    {
        public Task<List<CategoryResult>> GetAllCategoriesAsync();
        public Task<CategoryResult> GetCategoryById(long id);
        public Task AddCategory(CategoryResult categoryModel);
        public Task EditCategoryById(long id,CategoryResult category);
        public Task DeleteCategoryById(long id);
    }
}
