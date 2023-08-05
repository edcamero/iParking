using iParking.Domain.Entities.Usuario;

namespace iParking.DataAccess.DataServices
{
    public interface IUserDataServices
    {
        Task<bool> CheckIfUserExists(string rut, string dv);
        Task<Usuario?> GetUserAsync(string? rut, string? dv);
        Task<int> CreatedUser(UsuarioNuevo nuevoUsuario);
    }
}
