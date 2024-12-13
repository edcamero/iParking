namespace iParking.Domain.Entities.Vehicle
{
    public class VehicleUserInput
    {
        public string KeySession { get; set; } = null!;
        public string ImeiPos { get; set; } = null!;
        public string Placa { get; set; } = null!;
        public string SerieCelular { get; set; } = null!;
        public string VersionApp { get; set; } = null!;
    }

    public class VehicleUserInsert
    {
        public int UserId { get; set; }
        public string Placa { get; set; } = null!;
    }

}
