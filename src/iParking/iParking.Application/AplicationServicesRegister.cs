using iParking.Application.Services.Parking;
using iParking.Application.Services.User;
using iParking.Application.Services.Vehicle;
using Microsoft.Extensions.DependencyInjection;

namespace iParking.Application
{
    public static class AplicationServicesRegister
    {
        public static void AddiParkingAplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPayExtenalService, PayExtenalService>();
            services.AddScoped<IParkingServices, ParkingServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IVehicleServices, VehicleServices>();
        }
    }
}
