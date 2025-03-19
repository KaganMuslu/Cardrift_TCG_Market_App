using System.ComponentModel.DataAnnotations;

namespace Cardrift___TCG_Market_App.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required!")]
        [StringLength(50, ErrorMessage = "Username must be at most 50 characters!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid email format!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters!")]
        public string Password { get; set; }
    }
}
