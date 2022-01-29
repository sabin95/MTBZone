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
            foreach(var result in results)
            {
                result.Items = await GetItemsByOrderId(result.Id);
            }
            return results;
        }

        private async Task<List<ItemResult>> GetItemsByOrderId(long orderId)
        {
            if (orderId < 0)
            {
                throw new ArgumentException(nameof(orderId), "Order id must be greater than 0!");
            }
            var orderResult = await _orderContext.OrderItems.FirstOrDefaultAsync(x => x.Id == orderId);
            if (orderResult is null)
            {
                throw new ArgumentException(nameof(orderResult), "No Order found for this id!");
            }
            var items = await _orderContext.OrderItems.Where(x => x.OrderId == orderId).ToListAsync();
            var itemsResult = new List<ItemResult>();
            foreach (var item in items)
            {
                itemsResult.Add(new ItemResult()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    OrderId = item.OrderId
                });
            }
            return itemsResult;
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
