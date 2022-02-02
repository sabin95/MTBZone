using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogAPI.Data
{
    [Table("Categories", Schema = "Catalog")]
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
