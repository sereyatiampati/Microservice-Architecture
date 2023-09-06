using Microsoft.EntityFrameworkCore;
using TheJitu_commerce_EmailService.Models;

namespace TheJitu_commerce_EmailService.Data
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<EmailLoggers> EmailLoggers { get; set; }                   
    }
}
