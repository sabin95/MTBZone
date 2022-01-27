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
    }
}
