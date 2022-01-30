using CartAPI.Events;
using Microsoft.EntityFrameworkCore;
using MTBZone.RabbitMQ.Sender;
using OrdersAPI.Data;
using OrdersAPI.Events;
using OrdersAPI.Results;

namespace OrdersAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _orderContext;
        private readonly IRabbitMQSender _rabbitMQSender;

        public OrderRepository(OrderContext orderContext, IRabbitMQSender rabbitMQSender)
        {
            _orderContext = orderContext;
            _rabbitMQSender = rabbitMQSender;
        }

        public async Task<List<OrderResult>> GetAllOrders()
        {
            var results = await _orderContext.Orders.Include(y => y.Items).Select(x => new OrderResult()
            {
                Id = x.Id,
                State = x.State,
                Items = x.Items.Select(xi => new ItemResult()
                {
                    Id = xi.Id,
                    Title = xi.Title,
                    Price = xi.Price,
                    Quantity = xi.Quantity,
                    OrderId = xi.OrderId,
                    ExternalId = xi.ExternalId
                }).ToList()
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
            var message = new OrderCreated()
            {
                Id = cartOrdered.Id,
                Items = cartOrdered.Items.Select(x => new OrderCreatedItem()
                {
                    Id = x.Id,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Title = x.Title,
                    ExternalId = x.ExternalId
                }).ToList()
            };
            _rabbitMQSender.Send(message);
            order.Items = cartOrdered.Items.Select(x => new Item()
            {
                Title = x.Title,
                Quantity = x.Quantity,
                Price = x.Price,
                OrderId = order.Id,
                ExternalId=x.ExternalId
            }).ToList();
            await _orderContext.SaveChangesAsync();
        }
    }
}
