using CatalogAPI.Data;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _catalogContext;

        public ProductRepository(CatalogContext catalogContext)
        {
           _catalogContext = catalogContext;
        }
        public void AddProduct(ProductModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "Product should not be null!");
            }
            var product = new Products()
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                Description = model.Description,
                Price = model.Price,
                Title = model.Title
            };
            _catalogContext.Product.Add(product);
            _catalogContext.SaveChangesAsync();
        }

        public void DeleteProductById(long id)
        {
            if(id<0)
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be lower than 0!");
            }
            var result = _catalogContext.Product.FirstOrDefault(x => x.Id == id);
            if(result == null)
            {
                throw new ArgumentNullException(nameof(result), "Product does not exist!");
            }
            _catalogContext.Product.Remove(result);
            _catalogContext.SaveChangesAsync();
        }

        public void EditProductById(long id, ProductModel model)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be lower than 0!");
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Product cannot be null!");
            }
            var productToBeUpdated = _catalogContext.Product.FirstOrDefault(x => x.Id == id);
            if (productToBeUpdated == null)
            {
                throw new ArgumentNullException(nameof(productToBeUpdated), "Product does not exist!");
            }
            productToBeUpdated.Id = model.Id;
            productToBeUpdated.Title = model.Title;
            productToBeUpdated.Price = model.Price;
            productToBeUpdated.Description = model.Description;
            productToBeUpdated.CategoryId = model.CategoryId;
            _catalogContext.SaveChangesAsync();
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            var results = await _catalogContext.Product.Select(x => new ProductModel
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Description = x.Description,
                Price = x.Price,
                Title = x.Title
            }).ToListAsync();
            return results;
        }

        public async Task<ProductModel> GetProductById(long id)
        {
            var result = await _catalogContext.Product.Where(x => x.Id == id)
                    .Select(x => new ProductModel
                    {
                        Id = x.Id,
                        CategoryId = x.CategoryId,
                        Description = x.Description,
                        Price = x.Price,
                        Title = x.Title
                    }).FirstOrDefaultAsync();
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result), "No product found for this id!");
            }
            return result;
        }       
    }
}
