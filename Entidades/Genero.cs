using peliculasWebApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace peliculasWebApi.Entidades
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

<<<<<<< HEAD
        public List<PeliculasGeneros> PeliculasGeneros { get; set; }
    }
}

=======
        
    }
}
>>>>>>> 2af95bc638e0cb8dfd888ba82555fb6ab1b34aea
