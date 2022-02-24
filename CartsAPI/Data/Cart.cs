using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartsAPI.Data
{
    [Table("Carts", Schema = "Carts")]
    public class Cart
    {
        [Key]
        public Guid Id { get; set; }
        public string State { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
