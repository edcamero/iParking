using iParking.Domain.Entities.Usuario;
using iParking.Domain.Entities;

namespace iParking.Application.Services.User
{
    public interface IUserServices
    {
        Task<ActionResponseSession> CreatedUser(UsuarioNuevo nuevoUsuario);
    }
}
