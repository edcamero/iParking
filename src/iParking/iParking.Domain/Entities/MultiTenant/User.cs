using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace iParking.Domain.Entities.MultiTenant
{
    /// <summary>
    /// Entidad de usuario que hereda de IdentityUser<int> para usar con ASP.NET Core Identity.
    /// Incluye campos personalizados para el modelo de negocio.
    /// </summary>
    public class User : IdentityUser<int>
    {
        [StringLength(20)]
        public string Rut { get; set; } = string.Empty;

        [StringLength(1)]
        public string Dv { get; set; } = string.Empty;

        [StringLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [StringLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefono { get; set; }

        // Campos adicionales para el modelo SaaS
        public int? CompanyId { get; set; }
        public string? ImeiCelular { get; set; }
        public string? SerieCelular { get; set; }
        public string? VersionApp { get; set; }
        public string? Ciudad { get; set; }
        public DateTime? LastLoginDate { get; set; }

        // Navegación
        public virtual Company? Company { get; set; }
        public virtual ICollection<Parking.ParkingSession> OperatedSessions { get; set; } = new List<Parking.ParkingSession>();
        public virtual ICollection<Subscription.Subscription> Subscriptions { get; set; } = new List<Subscription.Subscription>();
    }
}
