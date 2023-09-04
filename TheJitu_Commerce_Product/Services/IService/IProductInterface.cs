using TheJitu_Commerce_Product.Models;

namespace TheJitu_Commerce_Product.Services.IService
{
    public interface IProductInterface
    {

        Task<IEnumerable<Product>> GetProductsAsync();

        Task<Product> GetProductByIdAsync(Guid id);

        Task<string> AddProductAsync(Product product);

        Task<string> DeleteProductAsync(Product product);

        Task<string> UpdateProductAsync(Product product);
    }
}
