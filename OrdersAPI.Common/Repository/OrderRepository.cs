using AutoMapper;
using CartsAPI.Events;
using Microsoft.EntityFrameworkCore;
using MTBZone.Messaging.Sender;
using OrdersAPI.Common.Data;
using OrdersAPI.Common.Results;
using OrdersAPI.Events;

namespace OrdersAPI.Common.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _orderContext;
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public OrderRepository(OrderContext orderContext, ISender sender, IMapper mapper)
        {
            _orderContext = orderContext;
            _sender = sender;
            _mapper = mapper;
        }

        public async Task<List<OrderResult>> GetAllOrders()
        {
            var orders = await _orderContext.Orders.Include(y => y.Items).ToListAsync();
            var results = _mapper.Map<List<OrderResult>>(orders);
            return results;
        }


        public async Task CreateOrder(CartOrderedEvent cartOrdered)
        {
            var order = new Order()
            {
                State = Utils.Utils.State.Pending.ToString(),
                
            };
            _orderContext.Orders.Add(order);
            //await _orderContext.SaveChangesAsync();
            var message = new OrderCreatedEvent()
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
            await _sender.Send(message);
            order.Items = cartOrdered.Items.Select(x => new Item()
            {
                Title = x.Title,
                Quantity = x.Quantity,
                Price = x.Price,
                OrderId = order.Id,
                ExternalId=x.ExternalId
            }).ToList();
            foreach (var item in order.Items)
            {
                _orderContext.OrderItems.Add(item);
            }
            await _orderContext.SaveChangesAsync();
        }
    }
}
