namespace iParking.Domain.Entities
{
    public class RequestPayExternal
    {
        public required string reference_id { get; set; } 
        public required string email { get; set; }
        public required string rut { get; set; }
        public string phone { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address_line { get; set; }
        public string address_city { get; set; }
        public string address_state { get; set; }
        public string amount { get; set; }
        public string name_item { get; set; }
        public string code_item { get; set; }
    }
}
