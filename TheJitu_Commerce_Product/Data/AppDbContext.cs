using Microsoft.EntityFrameworkCore;
using TheJitu_Commerce_Product.Models;

namespace TheJitu_Commerce_Product.Data
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
    }
}
