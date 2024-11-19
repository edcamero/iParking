using iParking.Domain.Entities.Usuario;
using iParking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iParking.Domain.Entities.Auth;

namespace iParking.Application.Services.Auth
{
    public interface IAuthServices
    {
        Task<bool> Login(LoginInput login);
    }
}
