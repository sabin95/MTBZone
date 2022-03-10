using CatalogAPI.Common.Repository;
using MTBZone.Messaging.Receiver;
using OrdersAPI.Events;

namespace CatalogAPI.EventHandlers.Orders
{
    public class OrderCreatedHandler : IHandler<OrderCreatedEvent>
    {
        private readonly IProductsRepository _productRepository;

        public OrderCreatedHandler(IProductsRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task Handle(OrderCreatedEvent message)
        {
            await _productRepository.DecreaseStockPerProduct(message.Items);
        }
    }
}