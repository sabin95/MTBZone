namespace CartAPI.Data
{
    public class Cart
    {
        public long Id { get; set; }
        public List<Item> Items { get; set; }
    }
}
