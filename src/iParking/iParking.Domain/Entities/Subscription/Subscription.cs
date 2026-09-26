using System;
using iParking.Domain.Common;
using iParking.Domain.Enums;
using iParking.Domain.Entities.MultiTenant;
using iParking.Domain.Entities.Parking;

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
        public new bool IsActive { get; set; } = true;
        public bool AutoRenew { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public DateTime? NextPaymentDate { get; set; }
        public int GracePeriodDays { get; set; } // Días de gracia después del vencimiento
        
        // Configuración de franja horaria (para planes con restricción de horario)
        public bool HasTimeFrameRestriction { get; set; } = false; // Si es true, aplica franjas horarias
        public int MaxExcessMinutesPerMonth { get; set; } = 180;   // Máximo minutos excedentes permitidos por mes (default 3 horas)
        public bool AllowNegativeBalance { get; set; } = false;    // Permite saldo en contra para cobro diferido
        public decimal ExcessRateMultiplier { get; set; } = 1.0m;  // Multiplicador de tarifa para excedentes (1.0 = tarifa normal)

        // Navegación
        public virtual Company Company { get; set; } = null!;
        public virtual ParkingLot? ParkingLot { get; set; }
        public virtual MultiTenant.ApplicationUser User { get; set; } = null!;
        public virtual ICollection<TimeFrame> TimeFrames { get; set; } = new List<TimeFrame>();
        public virtual ICollection<ParkingSessionExcess> ExcessRecords { get; set; } = new List<ParkingSessionExcess>();
    }
}
