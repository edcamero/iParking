using System;
using System.ComponentModel.DataAnnotations;
using iParking.Domain.Enums;

namespace iParking.Domain.Entities.Vehicle
{
    /// <summary>
    /// Entidad de dominio que representa un vehículo registrado en el sistema.
    /// Reemplaza la estructura legacy EVehicle corrigiendo tipos de datos (int en lugar de decimal para IDs, DateTime para marcas temporales).
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Identificador único del vehículo.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Placa patente del vehículo (ej: "AB-CD-12" o "ABCD12").
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Plate { get; set; } = string.Empty;

        /// <summary>
        /// Alias o nombre asignado por el usuario (ej: "Mi Auto").
        /// </summary>
        public string? Alias { get; set; }

        /// <summary>
        /// Categoría o tipo de vehículo (Automóvil, Motocicleta, Camión, Bicicleta).
        /// </summary>
        public VehicleType Type { get; set; } = VehicleType.Car;

        /// <summary>
        /// Identificador del usuario propietario del vehículo.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Indica si esta placa es la predeterminada del usuario.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Estado de activación del vehículo en el sistema.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Fecha y hora UTC en la que fue registrado el vehículo.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha y hora UTC de la última actualización.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
