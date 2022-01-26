using CatalogAPI.Commands;
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
            return category;
        }

        public async Task<CategoryResult> AddCategory(CategoryCommand categoryCommand)
        {
            if (categoryCommand is null)
            {
                throw new ArgumentNullException(nameof(categoryCommand), "Category should not be null!");
            }
            var category = new Category()
            {
                Name = categoryCommand.Name
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var categoryResult = new CategoryResult()
            {
                Id = category.Id,
                Name = category.Name
            };
            return categoryResult;
        }

        public async Task<CategoryResult> EditCategoryById(long id, CategoryCommand categoryCommand)
        {
            if(id<0)
            {
                throw new ArgumentNullException(nameof(id), "Id should be grater than 0!");
            }
            if(categoryCommand is null)
            {
                throw new ArgumentNullException(nameof(categoryCommand), "Category should not be null!");
            }
            var categoryToBeUpdated =await  _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (categoryToBeUpdated is null)
            {
                throw new ArgumentException(nameof(categoryToBeUpdated), "Category does not exist!");
            }
            categoryToBeUpdated.Name = categoryCommand.Name;
            await _context.SaveChangesAsync();
            var categoryResult = new CategoryResult()
            {
                Id = id,
                Name = categoryToBeUpdated.Name
            };
            return categoryResult;
        }
        public async Task DeleteCategoryById(long id)
        {
            if(id<0)
            {
                throw new ArgumentNullException(nameof(id), "Id should be greater than 0!");
            }
            var categoryToBeDeleted = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(categoryToBeDeleted is null)
            {
                throw new ArgumentException(nameof(categoryToBeDeleted), "Category does not exist!");
            }
            _context.Categories.Remove(categoryToBeDeleted);
            await _context.SaveChangesAsync();
        }
    }
}
