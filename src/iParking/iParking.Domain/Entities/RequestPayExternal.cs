using System.ComponentModel.DataAnnotations;

namespace iParking.Domain.Entities
{
    /// <summary>
    /// Datos para iniciar un pago mediante pasarela externa.
    /// NOTA: Esta clase NUNCA debe contener campos para datos de tarjetas de crédito.
    /// Los datos sensibles se ingresan directamente en la plataforma del proveedor de pagos.
    /// </summary>
    public class RequestPayExternal
    {
        /// <summary>
        /// ID único de referencia para el pago (ej: ID de sesión de parqueo)
        /// </summary>
        [Required(ErrorMessage = "El ID de referencia es requerido")]
        public required string reference_id { get; set; }

        /// <summary>
        /// Email del cliente para notificaciones
        /// </summary>
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public required string email { get; set; }

        /// <summary>
        /// Documento de identidad (RUT, NIT, DNI, etc.)
        /// </summary>
        [Required(ErrorMessage = "El documento es requerido")]
        public required string rut { get; set; }

        /// <summary>
        /// Número de teléfono
        /// </summary>
        [Phone(ErrorMessage = "Número de teléfono inválido")]
        public string? phone { get; set; }

        /// <summary>
        /// Nombre del cliente
        /// </summary>
        public string? first_name { get; set; }

        /// <summary>
        /// Apellido del cliente
        /// </summary>
        public string? last_name { get; set; }

        /// <summary>
        /// Dirección del cliente
        /// </summary>
        public string? address_line { get; set; }

        /// <summary>
        /// Ciudad del cliente
        /// </summary>
        public string? address_city { get; set; }

        /// <summary>
        /// Estado/Departamento del cliente
        /// </summary>
        public string? address_state { get; set; }

        /// <summary>
        /// Monto a pagar (debe ser mayor a 0)
        /// </summary>
        [Required(ErrorMessage = "El monto es requerido")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Monto inválido. Use formato decimal positivo")]
        public required string amount { get; set; }

        /// <summary>
        /// Nombre del ítem o servicio por el que se paga
        /// </summary>
        public string? name_item { get; set; }

        /// <summary>
        /// Código interno del ítem
        /// </summary>
        public string? code_item { get; set; }
    }
}
