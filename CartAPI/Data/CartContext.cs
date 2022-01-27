using Microsoft.EntityFrameworkCore;

namespace CartAPI.Data
{
    public class CartContext : DbContext
    {
        public CartContext(DbContextOptions<CartContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
