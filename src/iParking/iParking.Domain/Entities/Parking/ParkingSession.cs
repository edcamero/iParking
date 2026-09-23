using System;
using iParking.Domain.Common;
using iParking.Domain.Enums;

namespace iParking.Domain.Entities.Parking
{
    /// <summary>
    /// Representa una sesión de estacionamiento (entrada/salida de un vehículo).
    /// </summary>
    public class ParkingSession : BaseEntity
    {
        public int ParkingLotId { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public VehicleType VehicleType { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public ParkingSessionStatus Status { get; set; } = ParkingSessionStatus.Active;
        public int? ParkingSpotId { get; set; }
        public int? OperatorUserId { get; set; }
        
        // Datos de cobro
        public int? RateId { get; set; }
        public decimal? TotalAmount { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool IsPaid { get; set; }
        public string TicketCode { get; set; } = string.Empty; // Código QR/Barras
        
        // Datos de suscripción/franja horaria
        public int? SubscriptionId { get; set; } // Si aplica, la suscripción asociada
        public bool HasExcess { get; set; } = false; // Indica si hay excedentes de franja
        public decimal? ExcessAmount { get; set; } // Monto total de excedentes
        
        // Navegación
        public virtual ParkingLot ParkingLot { get; set; } = null!;
        public virtual ParkingSpot? ParkingSpot { get; set; }
        public virtual User? OperatorUser { get; set; }
        public virtual ParkingRate? Rate { get; set; }
        public virtual Subscription.Subscription? Subscription { get; set; }
        public virtual ICollection<ParkingSessionExcess> ExcessRecords { get; set; } = new List<ParkingSessionExcess>();
    }
}
