using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersAPI.Data
{
    [Table("Items", Schema = "Orders")]
    public class Item
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }

        [ForeignKey("OrderId")]
        public Guid OrderId { get; set; }
        public Guid ExternalId { get; set; }
    }
}
