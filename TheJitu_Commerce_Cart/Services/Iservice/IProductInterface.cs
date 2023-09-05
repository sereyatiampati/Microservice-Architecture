using TheJitu_Commerce_Cart.Model.Dto;

namespace TheJitu_Commerce_Cart.Services.Iservice
{
    public interface IProductInterface
    {
        Task<IEnumerable<ProductDto>> GetProductaAsync();
    }
}
