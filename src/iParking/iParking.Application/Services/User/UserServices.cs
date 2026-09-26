using iParking.DataAccess.DataServices;
using iParking.Domain.Entities;
using iParking.Domain.Entities.MultiTenant;
using iParking.Infrastructure.Security;
using iParking.Application.DTOs.User;

namespace iParking.Application.Services.User
{
    public class UserServices : IUserServices
    {
        private readonly IUserDataServices _userDataServices;
        private readonly ISecurityHash _securityHash;
        private readonly ITokenService _tokenService;

        public UserServices(IUserDataServices userDataServices, ISecurityHash securityHash, ITokenService tokenService)
        {
            _userDataServices = userDataServices ?? throw new ArgumentNullException(nameof(userDataServices));
            _securityHash = securityHash ?? throw new ArgumentNullException(nameof(securityHash));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<ActionResponseSession> CreatedUser(CreateUserDto newUser)
        {
            var user = await _userDataServices.GetUserAsync(newUser.Rut, newUser.Dv);

            var response = new ActionResponseSession();

            if (user != null && user.Estado == 0)
            {
                response.Message = "El usuario ya se encuentra registrado pero esta deshabilitado";
                response.Code = 409;

                return response;
            }

            if (user != null && user.Dv.Equals(newUser.Dv) && user!.Rut.Equals(newUser.Rut) && user.ClaveAcceso.Equals(newUser.Password))
            {
                response.Status = true;
                response.Code = 201;
                response.Id = user.IdUsuario;

                return response;
            }

            // Note: This logic will be replaced by UserManager in the next phase
            var newUserEntity = new ApplicationUser
            {
                Rut = newUser.Rut,
                Dv = newUser.Dv,
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                PhoneNumber = newUser.PhoneNumber,
                ClaveAcceso = _securityHash.GenerateHash(newUser.Password) // Temporary compatibility
            };

            var userId = await _userDataServices.CreatedUser(newUser);

            if (userId > 0)
            {
                response.Status = true;
                response.Code = 201;
                response.Id = userId;

            }
            else
            {
                response.Message = "Error tratando de crear el usuario";
            }

            return response;
        }

        public async Task<ApplicationUser?> GetUser(string email)
        {
            return await _userDataServices.GetUserAsync(email);
        }
    }
}
