using TheJitu_Commerce_Cart.Model.Dto;

namespace TheJitu_Commerce_Cart.Services.Iservice
{
    public interface ICartservice
    {

        Task<bool> CartUpsert(CartDto cartDto);

        Task<CartDto> GetUserCart(Guid userId);

        Task<bool> ApplyCoupons(CartDto cartDto);

        Task<bool> RemoveFromCart(Guid CartDetailId);
    }
}
