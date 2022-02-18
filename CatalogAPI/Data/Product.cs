using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogAPI.Data
{
    [Table("Products", Schema = "Catalog")]
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public double Price { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long Stock { get; set; }
        [ForeignKey("CategoryId")]
        public Guid CategoryId { get; set; }
    }
}
