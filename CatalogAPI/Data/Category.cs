using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogAPI.Data
{
    [Table("Categories", Schema = "Catalog")]
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
