using AutoMapper;
using CatalogAPI.Commands;
using CatalogAPI.Data;
using CatalogAPI.Results;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repository
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly CatalogContext _catalogContext;
        private readonly IMapper _mapper;

        public CategoriesRepository(CatalogContext catalogContext, IMapper mapper)
        {
            _catalogContext = catalogContext;
            _mapper = mapper;
        }
        public async Task<List<CategoryResult>> GetAllCategoriesAsync()
        {
            var categories = await _catalogContext.Categories.ToListAsync();
            var results = _mapper.Map<List<CategoryResult>>(categories);
            return results;
        }

        public async Task<CategoryResult> GetCategoryById(Guid id)
        {
            var result = await _catalogContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return null;
            }
            var category = _mapper.Map<CategoryResult>(result);
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
            _catalogContext.Categories.Add(category);
            await _catalogContext.SaveChangesAsync();

            var categoryResult = new CategoryResult()
            {
                Id = category.Id,
                Name = category.Name
            };
            return categoryResult;
        }

        public async Task<CategoryResult> EditCategoryById(Guid id, CategoryCommand categoryCommand)
        {
            if (categoryCommand is null)
            {
                throw new ArgumentNullException(nameof(categoryCommand), "Category should not be null!");
            }
            var categoryToBeUpdated = await _catalogContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (categoryToBeUpdated is null)
            {
                throw new ArgumentException(nameof(categoryToBeUpdated), "Category does not exist!");
            }
            categoryToBeUpdated.Name = categoryCommand.Name;
            await _catalogContext.SaveChangesAsync();
            var categoryResult = new CategoryResult()
            {
                Id = id,
                Name = categoryToBeUpdated.Name
            };
            return categoryResult;
        }
        public async Task DeleteCategoryById(Guid id)
        {
            var categoryToBeDeleted = await _catalogContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (categoryToBeDeleted is null)
            {
                throw new ArgumentException(nameof(categoryToBeDeleted), "Category does not exist!");
            }
            _catalogContext.Categories.Remove(categoryToBeDeleted);
            await _catalogContext.SaveChangesAsync();
        }
    }
}
