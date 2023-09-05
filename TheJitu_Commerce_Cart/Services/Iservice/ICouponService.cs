using TheJitu_Commerce_Cart.Model.Dto;

namespace TheJitu_Commerce_Cart.Services.Iservice
{
    public interface ICouponService
    {

        Task<CouponDto> GetCouponData(string CouponCode);
    }
}
