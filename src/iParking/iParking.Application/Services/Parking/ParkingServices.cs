using iParking.DataAccess.DataServices;
using iParking.Domain.Parking;
using ParkingData = iParking.DataAccess.Models.Parking;


namespace iParking.Application.Services.Parking
{
    public class ParkingServices: IParkingServices
    {
        private readonly IParkingDataServices _parkingDataServices;

        public ParkingServices(IParkingDataServices parkingDataServices)
        {
            _parkingDataServices = parkingDataServices ?? throw new ArgumentNullException(nameof(parkingDataServices));
        }


        public Task<ParkingData> CreateParking(ParkingInputDTO parkingInput)
        {
            DateTime currentDate = DateTime.Now;

            var parkingRepository = new DataAccess.Models.Parking
            {
                Location = parkingInput.Location,
                Name = parkingInput.Name,
                CreatedAt = currentDate,
                UpdatedAt = currentDate,
            };

            return _parkingDataServices.CreateParking(parkingRepository);
        }

        public async Task<ParkingData> GetParking(int id)
        {
            return await _parkingDataServices.GetParking(id);
        }

        public async Task<List<ParkingData>> GetParkings()
        {
            return await _parkingDataServices.GetParkings();
        }

        public Task<ParkingData?> UpdateParking(ParkingInputUpdateDTO parkingInput, int id)
        {
            DateTime currentDate = DateTime.Now;

            var parkingRepository = new DataAccess.Models.Parking
            {
                Id = id,
                Location = parkingInput.Location,
                Name = parkingInput.Name,
                UpdatedAt = currentDate,
            };

            return _parkingDataServices.UpdateParking(parkingRepository);
        }

        public async Task<ParkingData?> DeleteParking(int id)
        {
            return await _parkingDataServices.DeleteParking(id);
        }
    }
}
