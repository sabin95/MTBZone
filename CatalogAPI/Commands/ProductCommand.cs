namespace CatalogAPI.Commands
{
    public class ProductCommand
    {
        public double Price { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
    }
}
