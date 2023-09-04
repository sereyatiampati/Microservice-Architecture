using AutoMapper;
using TheJitu_Commerce_Product.Models;
using TheJitu_Commerce_Product.Models.Dtos;

namespace TheJitu_Commerce_Product.Profiles
{
    public class ProductProfile:Profile
    {

        public ProductProfile()
        {
            CreateMap<ProductRequestDto,Product>().ReverseMap();
        }
    }
}
