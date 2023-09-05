using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheJitu_Commerce_Cart.Model
{
    public class CartHeader
    {
        [Key]
        public Guid CartHeaderId { get; set; }

        [Required]
        public Guid UserId { get; set; }


        public string? CouponCode { get; set; } = string.Empty;

        //do not create a column for this --- this is not going to the database 
        [NotMapped]
        public int Discount { get; set; }

        [NotMapped]
        public int CartTotal { get; set; }
    }
}
