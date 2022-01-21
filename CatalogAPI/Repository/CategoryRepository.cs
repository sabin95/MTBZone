using CatalogAPI.Data;
using CatalogAPI.Models;
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

        public void AddCategory(CategoryModel categoryModel)
        {
            if (categoryModel is null)
            {
                throw new ArgumentNullException(nameof(categoryModel), "Category should not be null!");
            }
            var category = new Categories()
            {
                Id = categoryModel.Id,
                Name = categoryModel.Name
            };
            _context.Category.Add(category);
            _context.SaveChangesAsync();
        }

        public void EditCategoryById(long id,CategoryModel categoryModel)
        {
            if(id<0)
            {
                throw new ArgumentNullException(nameof(id), "Id should be grater than 0!");
            }
            if(categoryModel is null)
            {
                throw new ArgumentNullException(nameof(categoryModel), "Category should not be null!");
            }
            var categoryToBeUpdated = _context.Category.FirstOrDefault(x => x.Id == id);
            if (categoryToBeUpdated is null)
            {
                throw new ArgumentNullException(nameof(categoryToBeUpdated), "Category does not exist!");
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
            var categoryToBeDeleted = _context.Category.FirstOrDefault(x => x.Id == id);
            if(categoryToBeDeleted is null)
            {
                throw new ArgumentNullException(nameof(categoryToBeDeleted), "Category does not exist!");
            }
            _context.Category.Remove(categoryToBeDeleted);
            _context.SaveChangesAsync();
        }
    }
}
