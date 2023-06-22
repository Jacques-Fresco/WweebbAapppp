using System.ComponentModel.DataAnnotations;

namespace VehicleQuotes.ResourceModels
{
    public class User
    {
        [Required]
        public string UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters, dude!")]
        public string Password { get; set; } //^&%yutQWE
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}