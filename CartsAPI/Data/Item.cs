using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartsAPI.Data
{
    [Table("Items", Schema = "Carts")]
    public class Item
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        [ForeignKey("CartId")]
        public Guid CartId { get; set; }
        public Guid ExternalId { get; set; }
    }
}
