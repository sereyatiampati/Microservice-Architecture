using AutoMapper;
using TheJitu_commerce_Order.Model.Dto;
using TheJitu_commerce_Order.Models;
using TheJitu_commerce_Order.Models.Dto;

namespace TheJitu_commerce_Order.Profiles
{
    public class OrderProfiles:Profile
    {
        public OrderProfiles()
        {
            CreateMap<CartHeaderDto, OrderHeaderDto>()
             .ForMember(dest => dest.OrderTotal, src => src.MapFrom(x => x.CartTotal)).ReverseMap();

            CreateMap<CartDetailsDto, OrderDetailsDto>()
               .ForMember(dest => dest.ProductName, src => src.MapFrom(x => x.Product.Name))
               .ForMember(dest => dest.Price, src => src.MapFrom(x => x.Product.Price));

            CreateMap<OrderDetailsDto, CartDetailsDto>();

            CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();


        }
    }
}
