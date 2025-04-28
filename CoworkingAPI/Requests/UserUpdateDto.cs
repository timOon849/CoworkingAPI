using System.ComponentModel.DataAnnotations;

namespace CoworkingAPI.Requests
{
    public class UserUpdateDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "RoleId must be between 1 and 3")]
        public int RoleId { get; set; }
    }
}
