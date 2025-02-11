using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        private static PasswordHasher<User> _hasher = new();

        [Key]
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

        public User HashPassword()
        {
            Password = _hasher.HashPassword(this, Password);
            return this;
        }

        public bool VerifyPassword(string password)
        {
            return _hasher.VerifyHashedPassword(this, Password, password) == PasswordVerificationResult.Success;
        }
    }
}
