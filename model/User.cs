using System.ComponentModel.DataAnnotations;
using BCrypt.Net;

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

        // Make the Password property private (Encapsulation)
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        private string Password { get; set; }

        // Create a public method to set the password
        public void SetPassword(string password)
        {
            Password = BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Create a public method to check the password
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, this.Password);
        }
    }
}