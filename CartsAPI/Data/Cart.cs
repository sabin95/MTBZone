using System.ComponentModel.DataAnnotations.Schema;
using static CartsAPI.Utils.Utils;

namespace CartsAPI.Data
{
    [Table("Carts", Schema = "Cart")]
    public class Cart
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public List<Item> Items { get; set; }
    }
}
