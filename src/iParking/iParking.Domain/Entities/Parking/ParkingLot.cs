using System;
using iParking.Domain.Common;
using iParking.Domain.Enums;

namespace iParking.Domain.Entities.Parking
{
    /// <summary>
    /// Representa un parqueadero o sede de una empresa.
    /// Contiene la configuración de capacidad, horarios y ubicación.
    /// </summary>
    public class ParkingLot : BaseEntity
    {
        public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public bool Is24Hours { get; set; }
        public int TotalCapacity { get; set; }
        public bool IsActive { get; set; } = true;

        // Navegación
        public virtual Company Company { get; set; } = null!;
        public virtual ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();
        public virtual ICollection<ParkingRate> Rates { get; set; } = new List<ParkingRate>();
        public virtual ICollection<ParkingSession> Sessions { get; set; } = new List<ParkingSession>();
    }
}
