using CatalogAPI.Repository;
using MessagingService.Receiver;
using OrdersAPI.Events;

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
