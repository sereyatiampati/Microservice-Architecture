using System.ComponentModel.DataAnnotations;

namespace TheJitu_Commerce_Cart.Model.Dto
{
    public class CouponDto
    {
        public Guid CouponId { get; set; }
       
        public string CouponCode { get; set; } = string.Empty;
 
        public int CouponAmount { get; set; }
    
        public int CouponMinAmont { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
