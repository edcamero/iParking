using System;
using iParking.Domain.Common;
using iParking.Domain.Enums;

namespace iParking.Domain.Entities.Parking
{
    /// <summary>
    /// Representa un registro de excedente de franja horaria (entrada anticipada o salida tardía).
    /// Almacena el tiempo excedente y el estado del cobro correspondiente.
    /// </summary>
    public class ParkingSessionExcess : BaseEntity
    {
        public int ParkingSessionId { get; set; } // Relación con la sesión de estacionamiento
        
        // Tipo de excedente
        public ExcessType ExcessType { get; set; } // EarlyEntry o LateExit
        
        // Tiempo calculado
        public DateTime ReferenceTime { get; set; } // Hora límite de la franja (inicio para entrada, fin para salida)
        public DateTime ActualTime { get; set; }    // Hora real del evento (entrada o salida)
        public int ExcessMinutes { get; set; }      // Minutos excedentes calculados
        
        // Cobro
        public decimal ExcessAmount { get; set; }   // Monto a cobrar por el excedente
        public ExcessPaymentStatus PaymentStatus { get; set; } = ExcessPaymentStatus.Pending;
        public PaymentMethod? PaymentMethod { get; set; } // Método de pago usado para el excedente
        public DateTime? PaymentDate { get; set; }  // Fecha de pago del excedente
        public string? TransactionId { get; set; }  // ID de transacción de pasarela de pago
        public string? FailureReason { get; set; }  // Razón del fallo si el cobro falló
        
        // Configuración aplicada
        public int AppliedGraceMinutes { get; set; } = 0; // Minutos de gracia aplicados
        public decimal RatePerMinute { get; set; }        // Tarifa por minuto aplicada
        
        // Navegación
        public virtual ParkingSession ParkingSession { get; set; } = null!;
    }
}
