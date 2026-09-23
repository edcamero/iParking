using iParking.DataAccess.DataServices;
using iParking.DataAccess.DataServices.VehicleDataServices;
using iParking.DataAccess.Repositories;
using iParking.DataAccess.Repositories.Companies;
using iParking.DataAccess.Repositories.Parking;
using Microsoft.Extensions.DependencyInjection;

namespace iParking.DataAccess
{
    public static class DataServicesRegister
    {
        public static void AddiParkingDataServices(this IServiceCollection services, string connectionStringName)
        {
            services.AddTransient<ISqlConnectionFactory>(provider => new SqlConnectionFactory(connectionStringName));
            services.AddScoped<IParkingDataServices, ParkingDataServicesCommand>();
            services.AddScoped<IUserDataServices, UserDataServices>();          
            services.AddScoped<IVehicleDataServices, VehicleData>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IParkingLotRepository, ParkingLotRepository>();
            services.AddScoped<IParkingSessionRepository, ParkingSessionRepository>();
        }
    }
}

