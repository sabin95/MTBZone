namespace OrdersAPI.Events
{
    public class OrderCreatedEvent
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public List<OrderCreatedItem> Items { get; set; }
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