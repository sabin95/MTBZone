using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Data
{
    public class CategoryContext : DbContext
    {
        public CategoryContext(DbContextOptions<CategoryContext> dbContextOptions)
            :base(dbContextOptions)
        {

        }

        public DbSet<Categories> Category { get; set; }                
    }
}
