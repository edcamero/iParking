using iParking.Domain.Enums;

namespace iParking.Domain.Entities.Parking
{
    /// <summary>
    /// Representa un puesto o celda individual dentro de un parqueadero.
    /// </summary>
    public class ParkingSpot : BaseEntity
    {
        public int ParkingLotId { get; set; }
        public string Identifier { get; set; } = string.Empty; // Ej: "A-12"
        public string Sector { get; set; } = string.Empty;     // Ej: "Sector A"
        public VehicleType VehicleType { get; set; }
        public ParkingSpotStatus Status { get; set; } = ParkingSpotStatus.Available;
        public bool IsDisabled { get; set; } // Para personas con discapacidad

        // Navegación
        public virtual ParkingLot ParkingLot { get; set; } = null!;
    }
}
