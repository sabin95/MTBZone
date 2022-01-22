using CatalogAPI.Results;

namespace CatalogAPI.Repository
{
    public interface IProductRepository
    {
        public void AddProduct(ProductResult model);
        public Task<List<ProductResult>> GetAllProducts();
        public Task<ProductResult> GetProductById(long id);
        public void EditProductById(long id, ProductResult model);
        public void DeleteProductById(long id);

    }
}
