using iParking.DataAccess.DbContexts;
using iParking.DataAccess.Models;
using iParking.Domain.ParkingModels;
using Microsoft.EntityFrameworkCore;

namespace iParking.DataAccess.DataServices
{
    public class ParkingDataServices : IParkingDataServices
    {
        private readonly IDbContextFactory<ParkingContext> _dbContextFactory;

        public ParkingDataServices(IDbContextFactory<ParkingContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<Parking> CreateParking(Parking newParking)
        {

            using var dbContext = _dbContextFactory.CreateDbContext();

            dbContext.Parkings.Add(newParking);

            await dbContext.SaveChangesAsync();

            return newParking;
        }

        public async Task<Parking> GetParking(int id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var parking = await dbContext.Parkings.FirstAsync(p => p.Id == id);

            return parking;
        }

        public async Task<List<Parking>> GetParkings()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var parkings = await dbContext.Parkings.ToListAsync();

            return parkings;
        }

        public async Task<Parking?> UpdateParking(Parking updateParking)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var parking = await dbContext.Parkings.FirstAsync(p => p.Id == updateParking.Id);

            if (parking != null)
            {
                parking.Name = updateParking.Name;
                parking.Location = updateParking.Location;
                parking.UpdatedAt = updateParking.UpdatedAt;

                dbContext.SaveChanges();

                return parking;
            }

            return null;
        }

        public async Task<Parking?> DeleteParking(int id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var parking = await dbContext.Parkings.FirstAsync(p => p.Id == id);

            if (parking != null)
            {
                dbContext.Parkings.Remove(parking);
                dbContext.SaveChanges();

                return parking;
            }

            return null;
        }

        public Task<List<NearbyParkingLot>> GetNearbyParkings()
        {
            throw new NotImplementedException();
        }
    }
}
