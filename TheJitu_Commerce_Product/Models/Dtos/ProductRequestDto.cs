namespace TheJitu_Commerce_Product.Models.Dtos
{
    public class ProductRequestDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;


        public double Price { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}
