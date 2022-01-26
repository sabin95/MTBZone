using CatalogAPI.Commands;
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
        public async Task<ProductResult> AddProduct(ProductCommand productCommand)
        {
            if (productCommand is null)
            {
                throw new ArgumentNullException(nameof(productCommand), "Product should not be null!");
            }
            var product = new Product()
            {
                CategoryId = productCommand.CategoryId,
                Description = productCommand.Description,
                Price = productCommand.Price,
                Title = productCommand.Title
            };
            _catalogContext.Products.Add(product);
            await _catalogContext.SaveChangesAsync();
            var productResult = new ProductResult()
            {
                Id = product.Id,
                Title = product.Title,                
                Price = productCommand.Price,
                Description = productCommand.Description,
                CategoryId = productCommand.CategoryId,
            };
            return productResult;
        }

        public async Task DeleteProductById(long id)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be lower than 0!");
            }
            var result = await _catalogContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                throw new ArgumentException(nameof(result), "Product does not exist!");
            }
            _catalogContext.Products.Remove(result);
            await _catalogContext.SaveChangesAsync();
        }

        public async Task<ProductResult> EditProductById(long id, ProductCommand productCommand)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be lower than 0!");
            }
            if (productCommand == null)
            {
                throw new ArgumentNullException(nameof(productCommand), "Product cannot be null!");
            }
            var productToBeUpdated = await _catalogContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productToBeUpdated == null)
            {
                throw new ArgumentException(nameof(productToBeUpdated), "Product does not exist!");
            }
            productToBeUpdated.Title = productCommand.Title;
            productToBeUpdated.Price = productCommand.Price;
            productToBeUpdated.Description = productCommand.Description;
            productToBeUpdated.CategoryId = productCommand.CategoryId;
            await _catalogContext.SaveChangesAsync();
            var productResult = new ProductResult()
            {
                Id = productToBeUpdated.Id,
                Title = productToBeUpdated.Title,
                Price = productToBeUpdated.Price,
                Description = productToBeUpdated.Description,
                CategoryId = productToBeUpdated.CategoryId
            };
            return productResult;
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
            return product;
        }
    }
}
