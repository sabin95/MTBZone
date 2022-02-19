using Microsoft.EntityFrameworkCore;

namespace CartsAPI.Data
{
    public class CartsContext : DbContext
    {
        public CartsContext(DbContextOptions<CartsContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
