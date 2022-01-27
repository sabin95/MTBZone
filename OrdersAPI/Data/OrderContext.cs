using Microsoft.EntityFrameworkCore;

namespace OrdersAPI.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions dbContextOptions) 
            : base(dbContextOptions)
        {

        }

        public DbSet<Order> Orders { get; set; }
    }
}
