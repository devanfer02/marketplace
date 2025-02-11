using System.ComponentModel.DataAnnotations;
using Marketplace.Models;

namespace Marketplace.ViewModel
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            Email = "";
            Password = "";
        }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password at least 6 characters")]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            Fullname = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }
        [Required]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password at least 6 characters")]
        public string Password { get; set; }

        public User ToUser()
        {
            return new User()
            {
                Fullname = Fullname,
                Email = Email,
                Password = Password,
            };
        }
    }
}
