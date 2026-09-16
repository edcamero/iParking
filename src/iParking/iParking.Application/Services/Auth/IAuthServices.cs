using iParking.Domain.Entities.Usuario;
using iParking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iParking.Domain.Entities.Auth;
using iParking.Domain.Entities.Usuario;
using iParking.Domain.Shared;

namespace iParking.Application.Services.Auth
{
    /// <summary>
    /// Servicio de autenticación que aplica principios SOLID:
    /// - Single Responsibility: Solo maneja autenticación
    /// - Dependency Inversion: Depende de abstracciones (IUserRepository, ISecurityHash, ITokenService)
    /// </summary>
    public interface IAuthServices
    {
        Task<Result<bool>> LoginAsync(LoginInput login);
    }
}
