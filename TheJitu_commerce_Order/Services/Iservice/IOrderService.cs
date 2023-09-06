using TheJitu_commerce_Order.Model.Dto;
using TheJitu_commerce_Order.Models.Dto;

namespace TheJitu_commerce_Order.Services.Iservice
{
    public interface IOrderService
    {
        Task<OrderHeaderDto> CreateOrderHeader(CartDto cartDto);

    }
}
