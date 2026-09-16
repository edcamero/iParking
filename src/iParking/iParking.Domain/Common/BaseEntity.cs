namespace iParking.Domain.Common
{
    /// <summary>
    /// Entidad base para todas las entidades del dominio.
    /// Implementa el principio DRY centralizando propiedades comunes.
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
