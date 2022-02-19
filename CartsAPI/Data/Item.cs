using System.ComponentModel.DataAnnotations.Schema;

namespace CartsAPI.Data
{
    [Table("Items", Schema = "Cart")]
    public class Item
    {
        public Guid Id { get; set; } 
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public Guid CartId { get; set; }
        public Guid ExternalId { get; set; }
    }
}
