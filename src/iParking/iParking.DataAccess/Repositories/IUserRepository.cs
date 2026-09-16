using iParking.Domain.Entities.Usuario;

namespace iParking.DataAccess.Repositories
{
    /// <summary>
    /// Repositorio para operaciones de usuarios.
    /// Implementa principios SOLID: Single Responsibility y Dependency Inversion.
    /// </summary>
    public interface IUserRepository
    {
        Task<Usuario?> GetUserByRutAsync(string rut, string dv);
        Task<Usuario?> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string rut, string dv);
        Task<int> CreateUserAsync(UsuarioNuevo usuario);
        Task<bool> LoginAsync(string email, string hashedPassword);
    }
}
