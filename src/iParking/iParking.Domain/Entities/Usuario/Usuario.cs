using System;
using iParking.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace iParking.Domain.Entities.Usuario
{
    /// <summary>
    /// Entidad principal de usuario que soporta el modelo multi-empresa y roles.
    /// Hereda de IdentityUser<int> para integrar con ASP.NET Core Identity.
    /// </summary>
    public class User : IdentityUser<int>
    {
        public string Rut { get; set; } = string.Empty;
        public string Dv { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int Estado { get; set; } = 1;
        
        // Nuevos campos para modelo SaaS
        public int? CompanyId { get; set; } // null para SuperAdmin
        public UserRole Role { get; set; } = UserRole.Operator;
        public string? ImeiCelular { get; set; }
        public string? SerieCelular { get; set; }
        public string? VersionApp { get; set; }
        public string? Ciudad { get; set; }
        public DateTime? LastLoginDate { get; set; }

        // Navegación
        public virtual Company? Company { get; set; }
        public virtual ICollection<ParkingSession> OperatedSessions { get; set; } = new List<ParkingSession>();
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
