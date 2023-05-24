using iParking.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace iParking.DataAccess.DataServices
{
    public class VehicleDataServices
    {
        private readonly IDbContextFactory<ParkingContext> _dbContextFactory;
    }
}
