using System;
using System.ComponentModel.DataAnnotations;
using iParking.Domain.Enums;

namespace iParking.Domain.Entities.Payment
{
    /// <summary>
    /// Entidad de dominio que representa un medio de pago de tarjeta registrado.
    /// Cumple estrictamente con las normas PCI-DSS:
    /// - NO almacena código de seguridad (CVV/CVC).
    /// - NO almacena el número de tarjeta completo (PAN) en texto plano.
    /// - Utiliza tokenización provista por la pasarela de pagos (CardToken).
    /// - Almacena únicamente los últimos 4 dígitos para referencia del usuario.
    /// </summary>
    public class PaymentCard
    {
        /// <summary>
        /// Identificador único del registro de tarjeta.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Identificador del usuario propietario de la tarjeta.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Token seguro provisto por la pasarela de pagos (Transbank, Klap, Stripe, etc.).
        /// </summary>
        [Required]
        [StringLength(255)]
        public string CardToken { get; set; } = string.Empty;

        /// <summary>
        /// Últimos 4 dígitos del número de tarjeta para visualización del cliente (ej: "4242").
        /// </summary>
        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string LastFourDigits { get; set; } = string.Empty;

        /// <summary>
        /// Franquicia de la tarjeta (Visa, MasterCard, Amex, etc.).
        /// </summary>
        [StringLength(50)]
        public string? CardBrand { get; set; }

        /// <summary>
        /// Mes de vencimiento (1 - 12).
        /// </summary>
        [Range(1, 12)]
        public int ExpirationMonth { get; set; }

        /// <summary>
        /// Año de vencimiento en 4 dígitos (ej: 2028).
        /// </summary>
        [Range(2020, 2100)]
        public int ExpirationYear { get; set; }

        /// <summary>
        /// Indica si es el medio de pago predeterminado del usuario.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Estado operativo de la tarjeta (Activa, Inactiva, Bloqueada, Expirada).
        /// </summary>
        public EstadoTarjeta Status { get; set; } = EstadoTarjeta.Activa;

        /// <summary>
        /// Fecha y hora UTC en la que fue registrada la tarjeta.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha y hora UTC de la última actualización.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Verifica si la tarjeta se encuentra vencida respecto a una fecha UTC.
        /// </summary>
        public bool IsExpired(DateTime referenceUtc)
        {
            var expirationDate = new DateTime(ExpirationYear, ExpirationMonth, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddMonths(1)
                .AddDays(-1);

            return referenceUtc > expirationDate;
        }
    }
}
