using System.ComponentModel.DataAnnotations;

namespace TheJitu_commerce_Rewards.Models
{
    public class Rewards
    {
        [Key]
        public Guid RewardsId { get; set; }

        [Required]
        public string UserId { get; set; }=string.Empty;    

        [Required]
        public int RewardsAmount { get;set; }
    }
}
