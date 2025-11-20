namespace iParking.Domain.Entities.Location
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Iso3 { get; set; }
        public string Iso2 { get; set; }
        public string PhoneCode { get; set; }
        public string Currency { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
    }
}
