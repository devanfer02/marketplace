using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int Stock { get; set; }
        public required decimal Price { get; set; }
        public DateTimeOffset CreatedAt {  get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
