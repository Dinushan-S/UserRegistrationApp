using System.ComponentModel.DataAnnotations;

namespace UserRegistrationApp.Models
{
    public class User
    {
        public User()
        {
            Username = string.Empty; // Initialize to an empty string
            Email = string.Empty; // Initialize to an empty string
            Password = string.Empty; // Initialize to an empty string
        }
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }
    }
}