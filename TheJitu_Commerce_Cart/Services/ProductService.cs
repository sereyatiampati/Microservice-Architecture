using Newtonsoft.Json;
using TheJitu_Commerce_Cart.Model.Dto;
using TheJitu_Commerce_Cart.Services.Iservice;

namespace TheJitu_Commerce_Cart.Services
{
    public class ProductService : IProductInterface
    {
        private readonly IHttpClientFactory _clientFactory;
        public ProductService(IHttpClientFactory clientFactory)
        {

            _clientFactory = clientFactory;

        }
        public async  Task<IEnumerable<ProductDto>> GetProductaAsync()
        {
          //Create a client
          var client = _clientFactory.CreateClient("Product");
            var response = await client.GetAsync("/api/Product");
            var content = await response.Content.ReadAsStringAsync();
            var responseDto= JsonConvert.DeserializeObject<ResponseDto>(content);

            if (responseDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseDto.Result));
            }
            return new List<ProductDto>();

        }
    }
}
