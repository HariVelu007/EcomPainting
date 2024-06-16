using Microsoft.EntityFrameworkCore;

namespace EcomPainting.Models
{
    public class EcomContext : DbContext
    {
        public EcomContext(DbContextOptions<EcomContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
