namespace OrdersAPI.Events
{
    public class OrderCreated
    {
        public long Id { get; set; }
        public string State { get; set; }
        public List<OrderCreatedItem> Items { get; set; }
    }

    public class OrderCreatedItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public long ExternalId { get; set; }
    }
}