using iParking.Domain.Entities.Auth;

namespace iParking.Domain.Entities.Payment
{
    public class CreditCardInput : SecureRequest
    {
        public string Mail { get; set; }
        public string ClaveAcceso { get; set; }
        public string ImeiPos { get; set; }
        public string SerieCelular { get; set; }
        public string NumeroTarjeta { get; set; }
        public string MesVcto { get; set; }
        public string AnoVcto { get; set; }
        public string CvTarjeta { get; set; }
        public string Tipo { get; set; }
        public int IdUsuario { get; set; }
        public string VersionApp { get; set; }
    }

}
