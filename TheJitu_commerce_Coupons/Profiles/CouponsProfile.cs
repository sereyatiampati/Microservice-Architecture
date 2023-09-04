using AutoMapper;
using TheJitu_commerce_Coupons.Model;
using TheJitu_commerce_Coupons.Model.Dtos;

namespace TheJitu_commerce_Coupons.Profiles
{
    public class CouponsProfile:Profile
    {

        public CouponsProfile()
        {
            CreateMap<CouponRequestDto,Coupon>().ReverseMap();
        }
    }
}
