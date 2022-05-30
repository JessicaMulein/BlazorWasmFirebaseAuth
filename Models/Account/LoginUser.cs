using System.ComponentModel.DataAnnotations;

namespace Microsoft.Identity.Firebase.Models.Account
{
    public class LoginUser
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "The Password field must be a minimum of 6 characters")]
        public string Password { get; set; }
    }
}
