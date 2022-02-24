namespace CatalogAPI.Common.Commands
{
    public class ProductCommand
    {
        public double Price { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
    }
}
