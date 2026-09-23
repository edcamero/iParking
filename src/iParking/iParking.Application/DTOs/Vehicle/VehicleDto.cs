using System.ComponentModel.DataAnnotations;
using iParking.Domain.Enums;

namespace iParking.Application.DTOs.Vehicle
{
    /// <summary>
    /// DTO para registrar un vehículo en el sistema.
    /// </summary>
    public class CreateVehicleDto
    {
        [Required(ErrorMessage = "La placa es obligatoria")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "La placa debe tener entre 4 y 10 caracteres")]
        public string Plate { get; set; } = string.Empty;

        public string? Alias { get; set; }

        public VehicleType VehicleType { get; set; } = VehicleType.Car;

        public bool IsDefault { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para visualización de vehículos.
    /// </summary>
    public class VehicleResponseDto
    {
        public int Id { get; set; }
        public string Plate { get; set; } = string.Empty;
        public string? Alias { get; set; }
        public VehicleType Type { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
