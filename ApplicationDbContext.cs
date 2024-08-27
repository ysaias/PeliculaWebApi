using Microsoft.EntityFrameworkCore;
using peliculasWebApi.Entidades;

namespace peliculasWebApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Genero> Generos { get; set; }
    }
}
