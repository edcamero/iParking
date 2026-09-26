using iParking.Domain.Entities.MultiTenant;
using iParking.Domain.Entities;

namespace iParking.Application.Services.User
{
    public interface IUserServices
    {
        Task<ActionResponseSession> CreatedUser(CreateUserDto newUser);
        Task<ApplicationUser?> GetUser(string email);
    }
}
