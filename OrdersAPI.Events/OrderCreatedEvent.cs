using MTBZone.Messaging;

namespace OrdersAPI.Events
{
    public class OrderCreatedEvent : Event
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public List<OrderCreatedItem> Items { get; set; }
        public override string Type => typeof(OrderCreatedEvent).Name;
    }

    public class OrderCreatedItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public Guid ExternalId { get; set; }
    }
}