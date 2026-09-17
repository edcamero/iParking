using MediatR;
using iParking.DataAccess.DataServices;
using iParking.Domain.Entities.Usuario;
using iParking.Infrastructure.Security;

namespace iParking.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ActionResponseSession>
    {
        private readonly IUserDataServices _userDataServices;
        private readonly ISecurityHash _securityHash;
        private readonly ITokenService _tokenService;

        public CreateUserCommandHandler(IUserDataServices userDataServices, ISecurityHash securityHash, ITokenService tokenService)
        {
            _userDataServices = userDataServices ?? throw new ArgumentNullException(nameof(userDataServices));
            _securityHash = securityHash ?? throw new ArgumentNullException(nameof(securityHash));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<ActionResponseSession> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDataServices.GetUserAsync(request.Rut, request.Dv);

            var response = new ActionResponseSession();

            if (user != null && user.Estado == 0)
            {
                response.Message = "El usuario ya se encuentra registrado pero esta deshabilitado";
                response.Code = 409;

                return response;
            }

            if (user != null && user.Dv.Equals(request.Dv) && user!.Rut.Equals(request.Rut) && user.ClaveAcceso.Equals(request.Password))
            {
                response.Status = true;
                response.Code = 201;
                response.KeySession = _tokenService.GenerateToken(user.IdUsuario.ToString());
                response.Id = user.IdUsuario;

                return response;
            }

            var nuevoUsuario = new UsuarioNuevo
            {
                Rut = request.Rut,
                DigVer = request.Dv,
                Mail = request.Mail,
                Password = request.Password,
                ClaveAcceso = _securityHash.GenerateHash(request.Password),
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                Telefono = request.Telefono
            };

            var userId = await _userDataServices.CreatedUser(nuevoUsuario);

            if (userId > 0)
            {
                response.Status = true;
                response.Code = 201;
                response.KeySession = _tokenService.GenerateToken(userId.ToString());
                response.Id = userId;
            }
            else
            {
                response.Message = "Error tratando de crear el usuario";
            }

            return response;
        }
    }
}
