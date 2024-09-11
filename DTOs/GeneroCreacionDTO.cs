using peliculasWebApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace peliculasWebApi.DTOs
{
    public class GeneroCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
