using CatalogAPI.Results;

namespace CatalogAPI.Repository
{
    public interface IProductRepository
    {
        public Task AddProduct(ProductResult model);
        public Task<List<ProductResult>> GetAllProducts();
        public Task<ProductResult> GetProductById(long id);
        public Task EditProductById(long id, ProductResult model);
        public Task DeleteProductById(long id);

    }
}
