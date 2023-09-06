using Microsoft.EntityFrameworkCore;
using TheJitu_commerce_EmailService.Data;
using TheJitu_commerce_EmailService.Models;

namespace TheJitu_commerce_EmailService.Services
{
    public class EmailService
    {
        private DbContextOptions<AppDbContext> options;

        public EmailService()
        {
        }

        public EmailService(DbContextOptions<AppDbContext> options)
        {
            this.options = options;
        }


        public async Task SaveData(EmailLoggers emailLoggers)
        {
            //create _context

            var _context = new AppDbContext(this.options);
            _context.EmailLoggers.Add(emailLoggers);
           await  _context.SaveChangesAsync();
        }

    }
}
