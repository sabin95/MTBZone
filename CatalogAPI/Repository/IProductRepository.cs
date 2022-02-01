using CatalogAPI.Commands;
using CatalogAPI.Results;
using OrdersAPI.Events;

namespace CatalogAPI.Repository
{
    public interface IProductRepository
    {
        public Task<ProductResult> AddProduct(ProductCommand productCommand);
        public Task<List<ProductResult>> GetAllProducts();
        public Task<ProductResult> GetProductById(Guid id);
        public Task<ProductResult> EditProductById(Guid id, ProductCommand productCommand);
        public Task DeleteProductById(Guid id);
        public Task<ProductResult> IncreaseStockPerProduct(Guid productId, long quantity);
        public Task DecreaseStockPerProduct(List<OrderCreatedItem> orderCreatedItems);

    }
}
