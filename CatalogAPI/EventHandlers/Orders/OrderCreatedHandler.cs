using CatalogAPI.Repository;
using MTBZone.Messaging.Receiver;
using OrdersAPI.Events;

namespace CatalogAPI.EventHandlers.Orders
{
    public class OrderCreatedHandler : IHandler<OrderCreated>
    {
        private readonly IProductsRepository _productRepository;

        public OrderCreatedHandler(IProductsRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task Handle(OrderCreated message)
        {
            await _productRepository.DecreaseStockPerProduct(message.Items);
        }
    }
}