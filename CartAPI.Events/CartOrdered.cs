
namespace CartAPI.Events
{
    public class CartOrdered
    {
        public long Id { get; set; }
        public List<CartOrderedItem> Items { get; set; }
    }
    public class CartOrderedItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
    }
}
