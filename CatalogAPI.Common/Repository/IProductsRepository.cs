﻿using CatalogAPI.Common.Commands;
using CatalogAPI.Common.Results;
using OrdersAPI.Events;

namespace CatalogAPI.Common.Repository
{
    public interface IProductsRepository
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
