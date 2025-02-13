using System.ComponentModel.DataAnnotations;
using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.ViewModel
{
    public class ProductRequestViewModel
    {
        public int? Id { get; set; }
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
        public string? ImageUrl { get; set;  }

        public IFormFile? Image { get; set; }

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

        public Product Patch(Product product, string imageUrl = "")
        {

            if (!string.IsNullOrWhiteSpace(this.Name))
            {
                product.Name = this.Name;
            }

            if (this.Stock >= 0) 
            {
                product.Stock = this.Stock;
            }

            if (this.Price > 0)
            {
                product.Price = this.Price;
            }

            if (!string.IsNullOrWhiteSpace(this.Description))
            {
                product.Description = this.Description;
            }

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                product.ImageUrl = imageUrl;
            }

            return product;
        }


        public static ProductRequestViewModel FromModel(Product product)
        {
            return new ProductRequestViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Stock = product.Stock,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl
            };
        }
    }

    public class ProductFilter
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "size")]
        public int Size { get; set; } = 12;

        [FromQuery(Name = "name")]
        public string Name { get; set; } = "";
    }
}
