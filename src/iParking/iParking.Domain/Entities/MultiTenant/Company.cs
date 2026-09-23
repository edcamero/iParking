using System;
using iParking.Domain.Common;

namespace iParking.Domain.Entities.MultiTenant
{
    /// <summary>
    /// Representa una empresa cliente en el modelo SaaS.
    /// Cada empresa tiene sus propias sedes, usuarios y configuraciones.
    /// </summary>
    public class Company : BaseEntity
    {
        public string BusinessName { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty; // NIT/RUT
        public string LogoUrl { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public int MaxParkingLots { get; set; } // Límite de parqueaderos por plan
        public int MaxUsers { get; set; }       // Límite de usuarios por plan

        // Navegación
        public virtual ICollection<ParkingLot> ParkingLots { get; set; } = new List<ParkingLot>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
