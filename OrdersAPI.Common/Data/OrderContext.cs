using Microsoft.EntityFrameworkCore;

namespace OrdersAPI.Common.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions dbContextOptions) 
            : base(dbContextOptions)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> OrderItems { get; set; }
    }
}
