namespace iParking.Domain.Entities.Vehicle
{
    public class VehicleUserInput
    {
        public int KeySession { get; set; }
        public string Mail { get; set; } = null!;
        public string ClaveAcceso { get; set; } = null!;
        public string ImeiPos { get; set; } = null!;
        public string Placa { get; set; } = null!;
        public string SerieCelular { get; set; } = null!;
        public string VersionApp { get; set; } = null!;
    }

}
