namespace iParking.Domain.Entities
{
    public class Tariff
    {
        public int TariffId { get; set; }
        public int TenantId { get; set; }
        public int VehicleTypeId { get; set; }
        public string BillingType { get; set; } // Fraction, Hour, Day, Month
        public decimal Price { get; set; }
        public int ToleranceMinutes { get; set; }
    }
}
