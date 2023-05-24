using iParking.DataAccess.DataServices;
using iParking.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace iParking.DataAccess
{
    public static class DataServicesRegisterer
    {
        public static void AddiParkingDbContexts(this IServiceCollection services, string connectionStringName)
        {
            services.AddDbContextFactory<ParkingContext>(
                options =>
                {
                    options.UseSqlServer(connectionStringName);
                },
                lifetime: ServiceLifetime.Transient);

            services.AddScoped<IParkingDataServices, ParkingDataServices>();
        }

    }
}

