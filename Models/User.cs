using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
