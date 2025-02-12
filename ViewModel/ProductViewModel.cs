using System.ComponentModel.DataAnnotations;
using Marketplace.Models;

namespace Marketplace.ViewModel
{
    public class ProductRequestViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock can't be negative")]
        public int Stock { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        public Product ToProduct(string imageUrl)
        {
            return new Product()
            {
                Name = Name,
                Stock = Stock,
                Price = Price,
                Description = Description,
                ImageUrl = imageUrl
            };
        }

        public static ProductRequestViewModel FromModel(Product product)
        {
            return new ProductRequestViewModel()
            {
                Name = product.Name,
                Stock = product.Stock,
                Price = product.Price,
                Description = product.Description
            };
        }
    }
}
