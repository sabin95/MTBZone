using CartsAPI.Events;
using OrdersAPI.Common.Results;

namespace OrdersAPI.Common.Repository
{
    public interface IOrderRepository
    {
        public Task<List<OrderResult>> GetAllOrders();
        public Task CreateOrder(CartOrdered cartOrdered);
    }
}
