using MediatR;
using iParking.Domain.Entities.Usuario;

namespace iParking.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<ActionResponseSession>
    {
        public string Rut { get; set; } = string.Empty;
        public string Dv { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }
}
