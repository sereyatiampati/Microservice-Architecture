using Microsoft.EntityFrameworkCore;
using TheJitu_commerce_Rewards.Data;
using TheJitu_commerce_Rewards.Models;
using TheJitu_commerce_Rewards.Models.Dto;

namespace TheJitu_commerce_Rewards.Services
{
    public class RewardsService
    {
        private DbContextOptions<AppDBContext> options;

        public RewardsService(DbContextOptions<AppDBContext> options)
        {
            this.options = options;
        }


        public async Task  AddRewards (RewardsDto dto)
        {
            var reward = new Rewards()
            {
                UserId = dto.UserId,
                RewardsAmount = (dto.TotalAmount / 100)
            };

            var _db = new AppDBContext(options);
            await _db.AddAsync(reward);
           await _db.SaveChangesAsync();
        }
    }
}
