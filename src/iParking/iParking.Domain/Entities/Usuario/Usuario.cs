using System;
using System.ComponentModel.DataAnnotations;
using iParking.Domain.Enums;

namespace iParking.Domain.Entities.Usuario
{
    /// <summary>
    /// Entidad de usuario legacy compatible con la base de datos existente TBL_USUARIOS.
    /// </summary>
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Rut { get; set; } = string.Empty;
        public string Dv { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int Estado { get; set; } = 1;
        public string ClaveAcceso { get; set; } = string.Empty;
        
        // Campos adicionales para el modelo SaaS
        public int? CompanyId { get; set; }
        public UserRole Role { get; set; } = UserRole.Operator;
        public string? ImeiCelular { get; set; }
        public string? SerieCelular { get; set; }
        public string? VersionApp { get; set; }
        public string? Ciudad { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}
