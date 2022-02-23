using AutoMapper;
using CatalogAPI.Common.Commands;
using CatalogAPI.Common.Data;
using CatalogAPI.Common.Results;
using Microsoft.EntityFrameworkCore;
using OrdersAPI.Events;

namespace CatalogAPI.Common.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly CatalogContext _catalogContext;
        private readonly IMapper _mapper;

        public ProductsRepository(CatalogContext catalogContext, IMapper mapper)
        {
            _catalogContext = catalogContext;
            _mapper = mapper;
        }
        public async Task<ProductResult> AddProduct(ProductCommand productCommand)
        {
            if (productCommand is null)
            {
                throw new ArgumentNullException(nameof(productCommand), "Product should not be null!");
            }
            var category = await _catalogContext.Categories.FirstOrDefaultAsync(x => x.Id == productCommand.CategoryId);
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "No category found for this id!");
            }
            var product = new Product()
            {
                CategoryId = productCommand.CategoryId,
                Description = productCommand.Description,
                Price = productCommand.Price,
                Title = productCommand.Title,
                Stock = 0
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

        public async Task DeleteProductById(Guid id)
        {
            var result = await _catalogContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                throw new ArgumentException(nameof(result), "Product does not exist!");
            }
            _catalogContext.Products.Remove(result);
            await _catalogContext.SaveChangesAsync();
        }

        public async Task<ProductResult> EditProductById(Guid id, ProductCommand productCommand)
        {
            if (productCommand == null)
            {
                throw new ArgumentNullException(nameof(productCommand), "Product cannot be null!");
            }
            var productToBeUpdated = await _catalogContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productToBeUpdated == null)
            {
                throw new ArgumentException(nameof(productToBeUpdated), "Product does not exist!");
            }
            var category = await _catalogContext.Categories.FirstOrDefaultAsync(x => x.Id == productCommand.CategoryId);
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "No category found for this id!");
            }
            productToBeUpdated.Title = productCommand.Title;
            productToBeUpdated.Price = productCommand.Price;
            productToBeUpdated.Description = productCommand.Description;
            productToBeUpdated.CategoryId = productCommand.CategoryId;
            await _catalogContext.SaveChangesAsync();
            var productResult = _mapper.Map<ProductResult>(productToBeUpdated);
            return productResult;
        }

        public async Task<List<ProductResult>> GetAllProducts()
        {
            var products = await _catalogContext.Products.ToListAsync();
            var results = _mapper.Map<List<ProductResult>>(products);
            return results;
        }

        public async Task<ProductResult> GetProductById(Guid id)
        {
            var result = await _catalogContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return null;
            }
            var product = _mapper.Map<ProductResult>(result);
            return product;
        }

        public async Task<ProductResult> IncreaseStockPerProduct(Guid productId, long quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException(nameof(quantity), "Quantity cannot be lower than 0!");
            }
            var product = await _catalogContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (product == null)
            {
                throw new ArgumentException(nameof(product), "No product found for this id!");
            }
            product.Stock += quantity;
            await _catalogContext.SaveChangesAsync();
            var productResult = new ProductResult()
            {
                Id = product.Id,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Price = product.Price,
                Title = product.Title
            };
            return productResult;
        }
        public async Task DecreaseStockPerProduct(List<OrderCreatedItem> orderCreatedItems)
        {
            var itemsExternalIds = orderCreatedItems.Select(x => x.ExternalId).ToList();
            var products = await _catalogContext.Products.Where(x => itemsExternalIds.Contains(x.Id)).ToListAsync();
            var dictOrder = orderCreatedItems.ToDictionary(i => i.ExternalId, i => i.Quantity);
            foreach (var product in products)
            {
                if (product.Stock < dictOrder[product.Id])
                {
                    throw new ArgumentException("Quantity cannot be greater than actual stock!");
                }
                product.Stock -= dictOrder[product.Id];
            }
            await _catalogContext.SaveChangesAsync();
        }


    }
}
