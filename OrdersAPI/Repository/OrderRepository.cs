using CartAPI.Events;
using Microsoft.EntityFrameworkCore;
using OrdersAPI.Data;
using OrdersAPI.Results;

namespace OrdersAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _orderContext;

        public OrderRepository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public async Task<List<OrderResult>> GetAllOrders()
        {
            var results = await _orderContext.Orders.Select(x => new OrderResult()
            {
                Id = x.Id,
                State = x.State
            }).ToListAsync();
            return results;
        }

        public async Task CreateOrder(CartOrdered cartOrdered)
        {
            var order = new Order()
            {
                State = Utils.Utils.State.Pending.ToString(),
                
            };
            _orderContext.Orders.Add(order);
            await _orderContext.SaveChangesAsync();

            order.Items = cartOrdered.Items.Select(x => new Item()
            {
                Title = x.Title,
                Quantity = x.Quantity,
                Price = x.Price,
                OrderId = order.Id
            }).ToList();
            await _orderContext.SaveChangesAsync();
        }
    }
}
