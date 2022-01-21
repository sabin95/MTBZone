using CatalogAPI.Data;
using CatalogAPI.Results;
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
        public void AddProduct(ProductResult model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "Product should not be null!");
            }
            var product = new Product()
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                Description = model.Description,
                Price = model.Price,
                Title = model.Title
            };
            _catalogContext.Products.Add(product);
            _catalogContext.SaveChangesAsync();
        }

        public void DeleteProductById(long id)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be lower than 0!");
            }
            var result = _catalogContext.Products.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                throw new ArgumentException(nameof(result), "Product does not exist!");
            }
            _catalogContext.Products.Remove(result);
            _catalogContext.SaveChangesAsync();
        }

        public void EditProductById(long id, ProductResult model)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be lower than 0!");
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Product cannot be null!");
            }
            var productToBeUpdated = _catalogContext.Products.FirstOrDefault(x => x.Id == id);
            if (productToBeUpdated == null)
            {
                throw new ArgumentException(nameof(productToBeUpdated), "Product does not exist!");
            }
            productToBeUpdated.Id = model.Id;
            productToBeUpdated.Title = model.Title;
            productToBeUpdated.Price = model.Price;
            productToBeUpdated.Description = model.Description;
            productToBeUpdated.CategoryId = model.CategoryId;
            _catalogContext.SaveChangesAsync();
        }

        public async Task<List<ProductResult>> GetAllProducts()
        {
            var results = await _catalogContext.Products.Select(x => new ProductResult
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Description = x.Description,
                Price = x.Price,
                Title = x.Title
            }).ToListAsync();
            return results;
        }

        public async Task<ProductResult> GetProductById(long id)
        {
            var result = await _catalogContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return null;
            }
            var product =  new ProductResult()
            {
                Id = result.Id,
                CategoryId = result.CategoryId,
                Description = result.Description,
                Price = result.Price,
                Title = result.Title
            };
            if(product==null)
            {
                return null;
            }
            return product;
        }
    }
}
