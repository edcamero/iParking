namespace iParking.Domain.Entities.Payment
{
    public class FormaPago
    {
        public FormaPago(int id, string tarjeta, bool @default, string tipo)
        {
            Id = id;
            Tarjeta = tarjeta;
            Default = @default;
            Tipo = tipo;
        }

        public int Id { get; set; }
        public string Tarjeta { get; set; }
        public bool Default { get; set; }
        public string Tipo { get; set; }
       
    }
}
