using CartAPI.Events;
using OrdersAPI.Repository;
using RabbitMQ.Receiver;

namespace OrdersAPI.EventHandlers.Carts
{
    public class CartOrderedHandler : IHandler<CartOrdered>
    {
        private readonly IOrderRepository _orderRepository;

        public CartOrderedHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task Handle(CartOrdered message)
        {
            Console.WriteLine(message.ToString());
            await _orderRepository.CreateOrder(message);
        }
    }
}
