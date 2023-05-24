using iParking.Domain.Parking;


namespace iParking.Application.Services.Parking
{
    public  interface IParkingServices
    {
        public Task<DataAccess.Models.Parking> CreateParking(ParkingInputDTO parkingInput);
        public Task<DataAccess.Models.Parking> GetParking(int id);
        public Task<List<DataAccess.Models.Parking>> GetParkings();
        public Task<DataAccess.Models.Parking?> UpdateParking(ParkingInputUpdateDTO parkingInput, int id);
        public Task<DataAccess.Models.Parking?> DeleteParking(int id);
    }
}
