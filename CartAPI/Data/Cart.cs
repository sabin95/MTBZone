using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CartAPI.Utils.Utils;

namespace CartAPI.Data
{
    [Table("Carts", Schema = "Cart")]
    public class Cart
    {
        [Key]
        public Guid Id { get; set; }
        public string State { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
