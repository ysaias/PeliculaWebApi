using System.ComponentModel.DataAnnotations;

namespace peliculasWebApi.DTOs
{
    public class CineCreacionDTO
    {
        [Required]
        [StringLength(75)]
        public string Nombre { get; set; }
        [Range(-90, 90)]
        public double Longitud { get; set; }
        [Range(-180, 180)]
        public double Latitud { get; set; }
    }
}
