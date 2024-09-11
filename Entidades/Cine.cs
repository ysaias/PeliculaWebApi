using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace peliculasWebApi.Entidades
{
    public class Cine
    {
        public int Id { get; set; }

        [Required]
        [StringLength(75)]
        public string Nombre { get; set; }
        public Point Ubicacion { get; set; }
        public List<PeliculasCines> PeliculasCines { get; set; }
    }

}
