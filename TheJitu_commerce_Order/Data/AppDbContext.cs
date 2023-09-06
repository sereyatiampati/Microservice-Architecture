using Microsoft.EntityFrameworkCore;
using TheJitu_commerce_Order.Models;

namespace TheJitu_commerce_Order.Data
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
    }
}
