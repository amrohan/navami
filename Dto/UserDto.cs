using System.ComponentModel.DataAnnotations;

namespace navami.Dto
{

    public class UserDto
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string? Role { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Invalid mobile number")]
        public string? Mobile { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        public bool? IsDeactivated { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}