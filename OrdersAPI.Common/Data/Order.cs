using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersAPI.Common.Data
{
    [Table("Orders", Schema = "Orders")]
    public class Order
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public List<Item> Items { get; set; }
    }
}
