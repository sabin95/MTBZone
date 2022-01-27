namespace CartAPI.Commands
{
    public class AddItemToCartCommand
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public long CartId { get; set; }
    }
}
