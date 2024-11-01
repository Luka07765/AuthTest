using System.ComponentModel.DataAnnotations;

namespace AuthLearning.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Minimum password length is 6 characters")]
        public string Password { get; set; }
    }
}