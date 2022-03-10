namespace OrdersAPI.Common.Results
{
    public class ItemResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public Guid OrderId { get; set; }
        public Guid ExternalId { get; set; }
    }
}
