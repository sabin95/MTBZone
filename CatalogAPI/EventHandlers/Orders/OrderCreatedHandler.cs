using CatalogAPI.Repository;
using OrdersAPI.Events;
using RabbitMQ.Receiver;

namespace CatalogAPI.EventHandlers.Orders
{
    public class OrderCreatedHandler : IHandler<OrderCreated>
    {
        private readonly IProductRepository _productRepository;

        public OrderCreatedHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task Handle(OrderCreated message)
        {
            foreach(var item in message.Items)
            {
                await _productRepository.DecreaseStockPerProduct(item.ExternalId, item.Quantity);
            }
        }
    }
}
