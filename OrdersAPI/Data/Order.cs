namespace OrdersAPI.Data
{
    public class Order
    {
        public long Id { get; set; }
        public string State { get; set; }
        public List<Item> Items { get; set; }
    }
}
