using System;

namespace iParking.Domain.Entities.Vehicle
{
    /// <summary>
    /// Entidad legacy de vehículo con tipos de datos desactualizados (decimales para IDs).
    /// Mantener temporalmente solo para compatibilidad hacia atrás con TBL_PLACA legacy.
    /// </summary>
    [Obsolete("Use Vehicle entity instead. EVehicle uses incorrect primitive types.")]
    public class EVehicle
    {
        public decimal IdPlaca { get; set; }
        public string Placa { get; set; }
        public decimal? PlacaDefault { get; set; }
        public decimal? IdUsuario { get; set; }
        public string FechaHoraCreado { get; set; }
        public decimal? Estado { get; set; }
    }
}
