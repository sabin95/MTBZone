using IdentityAPI.Results;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.Data
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}

