using CatalogAPI.Models;

namespace CatalogAPI.Repository
{
    public interface IProductRepository
    {
        public void AddProduct(ProductModel model);
        public Task<List<ProductModel>> GetAllProducts();
        public Task<ProductModel> GetProductById(long id);
        public void EditProductById(long id, ProductModel model);
        public void DeleteProductById(long id);

    }
}
