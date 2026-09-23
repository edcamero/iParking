using System;
using iParking.Domain.Common;
using iParking.Domain.Enums;

namespace iParking.Domain.Entities.Parking
{
    /// <summary>
    /// Representa una tarifa o regla de cobro configurada para un parqueadero.
    /// Soporta tarifas fraccionadas, diarias, de pernocta y suscripciones.
    /// </summary>
    public class ParkingRate : BaseEntity
    {
        public int ParkingLotId { get; set; }
        public string Name { get; set; } = string.Empty; // Ej: "Tarifa Hora Auto", "Mensualidad Moto"
        public RateType RateType { get; set; }
        public VehicleType VehicleType { get; set; }
        public decimal Amount { get; set; }
        public int FreeMinutes { get; set; } // Tiempo de gracia
        public int? BillableMinutes { get; set; } // null si es tarifa fija (día/mensualidad)
        public bool IsActive { get; set; } = true;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Navegación
        public virtual ParkingLot ParkingLot { get; set; } = null!;
    }
}
