using OrdersAPI.Results;

namespace OrdersAPI.Repository
{
    public interface IOrderRepository
    {
        public Task<List<OrderResult>> GetAllOrders();
    }
}
