using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersAPI.Data
{
    [Table("Items", Schema = "Orders")]
    public class Item
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public long OrderId { get; set; }
        public long ExternalId { get; set; }
    }
}
