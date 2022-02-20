using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersAPI.Common.Data
{
    [Table("Orders", Schema = "Orders")]
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public string State { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
