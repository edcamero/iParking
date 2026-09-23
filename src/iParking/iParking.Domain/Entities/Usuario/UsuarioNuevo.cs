using System.ComponentModel.DataAnnotations;

namespace iParking.Domain.Entities.Usuario
{
    /// <summary>
    /// DTO para creación de nuevos usuarios con validaciones.
    /// Aplica principios de Clean Code con validaciones declarativas.
    /// Nota: El Estado NO es ingresado por el usuario, se asigna por defecto en el servicio.
    /// </summary>
    public class UsuarioNuevo
    {
        [Required(ErrorMessage = "El RUT es obligatorio")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El RUT debe tener entre 7 y 20 caracteres")]
        public string Rut { get; set; } = string.Empty;

        [Required(ErrorMessage = "El dígito verificador es obligatorio")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El dígito verificador debe ser un carácter")]
        public string Dv { get; set; } = string.Empty;

        /// <summary>
        /// Alias de compatibilidad legacy para Dv.
        /// </summary>
        [Obsolete("Use Dv instead")]
        public string DigVer
        {
            get => Dv;
            set => Dv = value;
        }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres")]
        public string Mail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100, ErrorMessage = "Los nombres no pueden exceder los 100 caracteres")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden exceder los 100 caracteres")]
        public string Apellidos { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El teléfono no tiene un formato válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [MaxLength(256, ErrorMessage = "La contraseña no puede exceder los 256 caracteres")]
        public string ClaveAcceso { get; set; } = string.Empty;

        /// <summary>
        /// Alias de compatibilidad legacy para ClaveAcceso.
        /// </summary>
        [Obsolete("Use ClaveAcceso instead")]
        public string Password
        {
            get => ClaveAcceso;
            set => ClaveAcceso = value;
        }

        [StringLength(100, ErrorMessage = "El IMEI no puede exceder los 100 caracteres")]
        public string? ImeiCelular { get; set; }

        [StringLength(100, ErrorMessage = "El serial no puede exceder los 100 caracteres")]
        public string? SerieCelular { get; set; }

        [StringLength(20, ErrorMessage = "La versión no puede exceder los 20 caracteres")]
        public string? VersionApp { get; set; }

        [StringLength(50, ErrorMessage = "La ciudad no puede exceder los 50 caracteres")]
        public string? Ciudad { get; set; }

        // NOTA: Estado eliminado - es un campo interno del sistema, no lo define el usuario al crear la cuenta
    }
}
