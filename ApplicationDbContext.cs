<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
=======
﻿using Microsoft.EntityFrameworkCore;
>>>>>>> 2af95bc638e0cb8dfd888ba82555fb6ab1b34aea
using peliculasWebApi.Entidades;

namespace peliculasWebApi
{
<<<<<<< HEAD
    public class ApplicationDbContext : IdentityDbContext
=======
    public class ApplicationDbContext : DbContext
>>>>>>> 2af95bc638e0cb8dfd888ba82555fb6ab1b34aea
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

<<<<<<< HEAD
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeliculasActores>()
                .HasKey(x => new { x.ActorId, x.PeliculaId });

            modelBuilder.Entity<PeliculasGeneros>()
              .HasKey(x => new { x.PeliculaId, x.GeneroId });

            modelBuilder.Entity<PeliculasCines>()
              .HasKey(x => new { x.PeliculaId, x.CineId });


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculasCines> PeliculasCines { get; set; }
        public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
        public DbSet<Rating> Ratings { get; set; }



=======
        public DbSet<Genero> Generos { get; set; }
>>>>>>> 2af95bc638e0cb8dfd888ba82555fb6ab1b34aea
    }
}
