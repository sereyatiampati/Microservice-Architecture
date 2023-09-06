using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TheJitu_commerce_Order.Model.Dto;

namespace TheJitu_commerce_Order.Models
{
    public class OrderDetails
    {
        [Key]
        public Guid OrderDetailsId { get; set; }
        public Guid OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader OrderHeader { get; set; }
        public Guid ProductId { get; set; }
        [NotMapped]
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public double Price { get; set; }
    
}
}
