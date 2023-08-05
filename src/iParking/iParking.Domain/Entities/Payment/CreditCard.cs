namespace iParking.Domain.Entities.Payment
{
    public class CreditCard
    {
        public decimal IdTarjeta { get; set; }
        public decimal? NumeroTarjeta { get; set; }
        public decimal? MesVcto { get; set; }
        public decimal? AnoVcto { get; set; }
        public decimal? Cvv { get; set; }
        public decimal? Tipo { get; set; }
        public decimal? DefaultTarjeta { get; set; }
        public decimal? IdUsuario { get; set; }
        public string FechaHoraCreado { get; set; }
        public decimal? Estado { get; set; }
    }
}
