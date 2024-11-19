using iParking.DataAccess.DataServices;
using iParking.Domain.Entities.Auth;
using iParking.Infrastructure.Security;

namespace iParking.Application.Services.Auth
{
    public class AuthServices : IAuthServices
    {
        private readonly IUserDataServices _userDataServices;
        private readonly ISecurityHash _securityHash;

        public AuthServices(IUserDataServices userDataServices, ISecurityHash securityHash)
        {
            _userDataServices = userDataServices ?? throw new ArgumentNullException(nameof(userDataServices));
            _securityHash = securityHash ?? throw new ArgumentNullException(nameof(securityHash));
        }

        public async Task<bool> Login(LoginInput login)
        {
            login.ClaveAcceso = _securityHash.GenerateHash(login.ClaveAcceso);
            return await _userDataServices.Login(login);
        }
    }
}
