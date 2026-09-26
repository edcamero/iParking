using System.ComponentModel.DataAnnotations;

namespace iParking.Application.DTOs.User
{
    /// <summary>
    /// DTO for creating a new user in the application layer.
    /// Excludes explicitly internal state or persistence attributes.
    /// </summary>
    public class CreateUserDto
    {
        [Required(ErrorMessage = "The RUT is required")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "The RUT must be between 7 and 20 characters")]
        public string Rut { get; set; } = string.Empty;

        [Required(ErrorMessage = "The check digit is required")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "The check digit must be 1 character")]
        public string Dv { get; set; } = string.Empty;

        [Required(ErrorMessage = "The email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "The password is required")]
        [MinLength(8, ErrorMessage = "The password must have at least 8 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
        public string? ImeiCelular { get; set; }
        public string? SerieCelular { get; set; }
        public string? VersionApp { get; set; }
        public string? City { get; set; }
    }
}
