using iParking.Domain.Entities.Auth;
using iParking.Domain.Entities.Usuario;

namespace iParking.DataAccess.DataServices
{
    /// <summary>
    /// Interfaz legacy para servicios de usuario.
    /// Nota: Esta interfaz será reemplazada por IUserRepository en futuras iteraciones.
    /// </summary>
    [Obsolete("Use IUserRepository en su lugar")]
    public interface IUserDataServices
    {
        Task<bool> CheckIfUserExists(string rut, string dv);
        Task<Usuario?> GetUserAsync(string? rut, string? dv);
        Task<int> CreatedUser(UsuarioNuevo nuevoUsuario);
        Task<bool> Login(LoginInput login);

        Task<Usuario?> GetUserAsync(string mail);
    }
}
