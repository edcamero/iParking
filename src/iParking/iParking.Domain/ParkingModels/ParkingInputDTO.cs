using System.ComponentModel.DataAnnotations;

namespace iParking.Domain.Parking
{
    public class ParkingInputDTO
    {
        [Required(ErrorMessage = "El campo 'Name' es requerido.")]
        public string Name { get; set; } = null!;

        [StringLength(200, ErrorMessage = "El campo 'Location' debe tener una longitud máxima de 200 caracteres.")]
        [Required(ErrorMessage = "El campo 'Location' es requerido.")]
        public string? Location { get; set; }
    }

    public class ParkingInputUpdateDTO : ParkingInputDTO
    {
        public int Id { get; set; }
    }
}
