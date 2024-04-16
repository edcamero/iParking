namespace iParking.Domain.Entities.Payment
{
    public  class PayDetails
    {
        public string KeySession { get; set; } = null!;
        public string QRCode { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ImeiPos { get; set; } = null!;
        public string Plate { get; set; } = null!;
        public string CellSerie { get; set; } = null!;
        public string AppVersion { get; set; } = null!;
    }
}
