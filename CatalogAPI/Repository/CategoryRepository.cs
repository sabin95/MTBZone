using CatalogAPI.Data;
using CatalogAPI.Results;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogContext _context;

        public CategoryRepository(CatalogContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryResult>> GetAllCategoriesAsync()
        {
            var results = await _context.Categories.Select(x=> new CategoryResult
            {
                Id = x.Id,
                Name = x.Name                    
            }).ToListAsync();
            return results;
        }

        public async Task<CategoryResult> GetCategoryById(long id)
        {
            var result = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return null;
            }
            var category = new CategoryResult()
            {
                Id = result.Id,
                Name = result.Name
            };
            if (category == null)
            {
                return null;
            }
            return category;
        }

        public void AddCategory(CategoryResult categoryModel)
        {
            if (categoryModel is null)
            {
                throw new ArgumentNullException(nameof(categoryModel), "Category should not be null!");
            }
            var category = new Category()
            {
                Id = categoryModel.Id,
                Name = categoryModel.Name
            };
            _context.Categories.Add(category);
            _context.SaveChangesAsync();
        }

        public void EditCategoryById(long id,CategoryResult categoryModel)
        {
            if(id<0)
            {
                throw new ArgumentNullException(nameof(id), "Id should be grater than 0!");
            }
            if(categoryModel is null)
            {
                throw new ArgumentNullException(nameof(categoryModel), "Category should not be null!");
            }
            var categoryToBeUpdated = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (categoryToBeUpdated is null)
            {
                throw new ArgumentException(nameof(categoryToBeUpdated), "Category does not exist!");
            }
            categoryToBeUpdated.Id = categoryModel.Id;
            categoryToBeUpdated.Name = categoryModel.Name;
            _context.SaveChangesAsync();
        }
        public void DeleteCategoryById(long id)
        {
            if(id<0)
            {
                throw new ArgumentNullException(nameof(id), "Id should be greater than 0!");
            }
            var categoryToBeDeleted = _context.Categories.FirstOrDefault(x => x.Id == id);
            if(categoryToBeDeleted is null)
            {
                throw new ArgumentException(nameof(categoryToBeDeleted), "Category does not exist!");
            }
            _context.Categories.Remove(categoryToBeDeleted);
            _context.SaveChangesAsync();
        }
    }
}
