using iParking.Application.Services.Auth;
using iParking.Application.Services.Parking;
using iParking.Application.Services.User;
using iParking.Application.Services.Vehicle;
using iParking.Application.ServicesExternal;
using iParking.Domain.ExternalServices;
using iParking.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace iParking.Application
{
    /// <summary>
    /// Configuración de inyección de dependencias para la capa de aplicación.
    /// Sigue el principio de Inversión de Dependencias (SOLID-D).
    /// </summary>
    public static class AplicationServicesRegister
    {
        /// <summary>
        /// Registra todos los servicios de aplicación en el contenedor DI.
        /// </summary>
        public static void AddiParkingAplicationServices(this IServiceCollection services)
        {
            // Servicios de infraestructura
            services.AddSingleton<IServicesConfigurationService, ServicesConfigurationService>();
            services.AddSingleton<ISecurityHash, SecurityHash>();
            
            // Servicios de negocio (Scoped = por request HTTP)
            services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
            services.AddScoped<IParkingServices, ParkingServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IVehicleServices, VehicleServices>();
        }
    }
}
