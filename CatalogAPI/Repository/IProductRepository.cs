using CatalogAPI.Commands;
using CatalogAPI.Results;

namespace CatalogAPI.Repository
{
    public interface IProductRepository
    {
        public Task<ProductResult> AddProduct(ProductCommand productCommand);
        public Task<List<ProductResult>> GetAllProducts();
        public Task<ProductResult> GetProductById(long id);
        public Task<ProductResult> EditProductById(long id, ProductCommand productCommand);
        public Task DeleteProductById(long id);
        public Task<ProductResult> IncreaseStockPerProduct(long productId, long quantity);
        public Task<ProductResult> DecreaseStockPerProduct(long productId, long quantity);

    }
}
