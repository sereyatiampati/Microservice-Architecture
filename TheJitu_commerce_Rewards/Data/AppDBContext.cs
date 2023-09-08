using Microsoft.EntityFrameworkCore;
using TheJitu_commerce_Rewards.Models;

namespace TheJitu_commerce_Rewards.Data
{
    public class AppDBContext:DbContext
    {

        public AppDBContext( DbContextOptions<AppDBContext> options):base(options)
        {
            
        }

        public DbSet<Rewards> Rewards { get; set; } 
    }
}
