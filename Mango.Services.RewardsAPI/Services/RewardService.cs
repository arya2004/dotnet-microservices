
using Mango.Services.RewardsAPI.Models;
using Mango.Services.RewardsAPI.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Licenses;
using NuGet.Packaging.Signing;
using System.Text;
using Mango.Services.RewardsAPI.Message;

namespace Mango.Services.RewardsAPI.Services
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public RewardService(DbContextOptions<AppDbContext> options)
        {
            this._dbOptions = options;
        }


      

        public async Task UpdateRewards(RewardMessage rewardMessage)
        {
            try
            {
                Rewards reward = new()
                { OrderId = rewardMessage.OrderId,
                   RewardsActivity = rewardMessage.RewardsActivity,
                   UserId = rewardMessage.UserId,
                   RewardsDate = DateTime.Now,
                };

                await using var _db = new AppDbContext(_dbOptions);
                await _db.Rewards.AddAsync(reward);
                await _db.SaveChangesAsync();
                

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
               
            }
        }
    }

    
}
