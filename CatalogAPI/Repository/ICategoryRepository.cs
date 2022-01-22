using CatalogAPI.Results;

namespace CatalogAPI.Repository
{
    public interface ICategoryRepository
    {
        public Task<List<CategoryResult>> GetAllCategoriesAsync();
        public Task<CategoryResult> GetCategoryById(long id);
        public void AddCategory(CategoryResult categoryModel);
        public void EditCategoryById(long id,CategoryResult category);
        public void DeleteCategoryById(long id);
    }
}
