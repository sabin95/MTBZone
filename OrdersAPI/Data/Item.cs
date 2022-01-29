namespace OrdersAPI.Data
{
    public class Item
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public long OrderId { get; set; }
    }
}
