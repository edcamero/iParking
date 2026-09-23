namespace iParking.Domain.Common
{
    /// <summary>
    /// Entidad base para todas las entidades del dominio.
    /// Implementa el principio DRY centralizando propiedades comunes.
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Alias de compatibilidad legacy para CreatedAt.
        /// </summary>
        public DateTime CreatedDate
        {
            get => CreatedAt;
            set => CreatedAt = value;
        }

        /// <summary>
        /// Alias de compatibilidad legacy para UpdatedAt.
        /// </summary>
        public DateTime? ModifiedDate
        {
            get => UpdatedAt;
            set => UpdatedAt = value;
        }
    }
}
