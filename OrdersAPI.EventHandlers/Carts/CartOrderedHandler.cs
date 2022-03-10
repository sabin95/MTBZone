using CartsAPI.Events;
using MTBZone.Messaging.Receiver;
using OrdersAPI.Common.Repository;

namespace OrdersAPI.EventHandlers.Carts
{
    public class CartOrderedHandler : IHandler<CartOrderedEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public CartOrderedHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task Handle(CartOrderedEvent message)
        {
            await _orderRepository.CreateOrder(message);
        }
    }
}
