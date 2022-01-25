namespace CartAPI.Commands
{
    public class ItemCommand
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
    }
}
