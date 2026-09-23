using System.ComponentModel.DataAnnotations;

namespace iParking.Application.DTOs.User
{
    /// <summary>
    /// DTO para la creación de un nuevo usuario en la capa de aplicación.
    /// Excluye explícitamente atributos de estado interno o persistencia.
    /// </summary>
    public class CreateUserDto
    {
        [Required(ErrorMessage = "El RUT es obligatorio")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El RUT debe tener entre 7 y 20 caracteres")]
        public string Rut { get; set; } = string.Empty;

        [Required(ErrorMessage = "El dígito verificador es obligatorio")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El dígito verificador debe ser 1 carácter")]
        public string Dv { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        public string Apellidos { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;
        public string? ImeiCelular { get; set; }
        public string? SerieCelular { get; set; }
        public string? VersionApp { get; set; }
        public string? Ciudad { get; set; }
    }
}
