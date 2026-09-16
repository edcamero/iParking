using System;
using iParking.Domain.Enums;

namespace iParking.Domain.Entities.Subscription
{
    /// <summary>
    /// Representa una suscripción o mensualidad activa para un vehículo.
    /// </summary>
    public class Subscription : BaseEntity
    {
        public int CompanyId { get; set; }
        public int? ParkingLotId { get; set; } // null si aplica a todos los parqueaderos de la empresa
        public string LicensePlate { get; set; } = string.Empty;
        public VehicleType VehicleType { get; set; }
        public int UserId { get; set; } // Cliente propietario del vehículo
        public RateType PlanType { get; set; } // Mensual, Quincenal, etc.
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool AutoRenew { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public DateTime? NextPaymentDate { get; set; }
        public int GracePeriodDays { get; set; } // Días de gracia después del vencimiento

        // Navegación
        public virtual Company Company { get; set; } = null!;
        public virtual ParkingLot? ParkingLot { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
