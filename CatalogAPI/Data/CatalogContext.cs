using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> dbContextOptions)
            :base(dbContextOptions)
        {

        }

        public DbSet<Categories> Category { get; set; }                
    }
}
