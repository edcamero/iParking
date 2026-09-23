using System;
using iParking.Domain.Common;
using iParking.Domain.Enums;

namespace iParking.Domain.Entities.Subscription
{
    /// <summary>
    /// Representa una franja horaria específica asociada a un plan o suscripción.
    /// Ejemplos: Plan Nocturno (18:00 - 06:00), Plan Oficina (Lun-Vie 08:00 - 18:00).
    /// </summary>
    public class TimeFrame : BaseEntity
    {
        public int SubscriptionId { get; set; } // Relación con la suscripción/mensualidad
        public DayOfWeek DayOfWeek { get; set; } // Día de la semana (0 = Domingo, 6 = Sábado)
        public TimeSpan StartTime { get; set; } // Hora de inicio de la franja
        public TimeSpan EndTime { get; set; }   // Hora de fin de la franja
        
        // Configuración de tolerancia
        public int EntryGraceMinutes { get; set; } = 0;  // Minutos de gracia para entrada anticipada
        public int ExitGraceMinutes { get; set; } = 0;   // Minutos de gracia para salida tardía
        
        // Navegación
        public virtual Subscription Subscription { get; set; } = null!;
    }
}
