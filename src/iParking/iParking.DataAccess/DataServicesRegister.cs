using iParking.DataAccess.DataServices;
using iParking.DataAccess.DataServices.CardServices;
using iParking.DataAccess.DataServices.VehicleServices;
using Microsoft.Extensions.DependencyInjection;

namespace iParking.DataAccess
{
    public static class DataServicesRegister
    {
        public static void AddiParkingDataServices(this IServiceCollection services, string connectionStringName)
        {
            services.AddTransient<ISqlConnectionFactory>(provider => new SqlConnectionFactory(connectionStringName ));
            services.AddScoped<IParkingDataServices, ParkingDataServicesCommand>();
            services.AddScoped<IUserDataServices, UserDataServices>();
            services.AddScoped<ICardDataServices, CardDataServices>();
            services.AddScoped<IVehicleDataServices, VehicleDataServices>();
        }
    }
}

