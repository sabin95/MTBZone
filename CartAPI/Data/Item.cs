namespace CartAPI.Data
{
    public class Item
    {
        public long Id { get; set; } 
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public long CartId { get; set; }
    }
}
