using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersAPI.Data
{
    [Table("Orders", Schema = "Orders")]
    public class Order
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public List<Item> Items { get; set; }
    }
}
