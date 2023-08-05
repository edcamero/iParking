namespace iParking.Domain.Entities
{
    public class ActionResponse
    {
        public  int Id { get; set; }
        public bool Status { get; set; } = false;
        public string? Message { get; set; } 
        public int Code { get; set; }   = 0;
    }


    public class ActionResponseSession: ActionResponse
    {
        public int KeySession { get; set; } = 0;
    }
}
