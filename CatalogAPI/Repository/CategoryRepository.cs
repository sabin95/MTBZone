using CatalogAPI.Data;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryContext _context;

        public CategoryRepository(CategoryContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            var results = await _context.Category.Select(x=> new CategoryModel
            {
                Id = x.Id,
                Name = x.Name                    
            }).ToListAsync();
            return results;
        }

        public async Task<CategoryModel> GetCategoryById(long id)
        {
            if(id<0)
            {
                throw new ArgumentNullException(nameof(id), "Id should be equal or greater than 0!");
            }
            var result = await _context.Category.Where(x => x.Id == id).Select(x => new CategoryModel
            {
                Id = x.Id,
                Name = x.Name
            }).FirstOrDefaultAsync();
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result),"No category found for this id!");
            }
            return result;
        }
    }
}
