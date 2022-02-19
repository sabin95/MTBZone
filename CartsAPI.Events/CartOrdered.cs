
namespace CartsAPI.Events
{
    public class CartOrdered
    {
        public Guid Id { get; set; }
        public List<CartOrderedItem> Items { get; set; }
    }
    public class CartOrderedItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public Guid ExternalId { get; set; }
    }
}
