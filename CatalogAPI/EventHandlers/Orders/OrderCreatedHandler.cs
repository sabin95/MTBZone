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
            await _productRepository.DecreaseStockPerProduct(message.Items);
        }
    }
}
