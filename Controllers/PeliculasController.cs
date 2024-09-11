using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using peliculasWebApi.DTOs;
using peliculasWebApi.Entidades;
using peliculasWebApi.Utilidades;
using System.Security.Claims;

namespace peliculasWebApi.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly UserManager<IdentityUser> userManager;
        private readonly string contenedor = "peliculas";


        public PeliculasController(ApplicationDbContext context,
            IMapper mapper, IAlmacenadorArchivos almacenadorArchivos,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
            this.userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<LandingPageDTO>> Get()
        {
            var top = 6;
            var hoy = DateTime.Today;
            var proximosEstrenos = await context.Peliculas
                .Where(x => x.FechaLanzamiento > hoy)
                .OrderBy(x => x.FechaLanzamiento)
                .Take(top)
                .ToListAsync();

            var enCines = await context.Peliculas
                .Where(x => x.EnCines)
                .OrderBy(x => x.FechaLanzamiento)
                .Take(top)
                .ToListAsync();

            var resultado = new LandingPageDTO();
            resultado.ProximosEstrenos = mapper.Map<List<PeliculaDTO>>(proximosEstrenos);
            resultado.EnCines = mapper.Map<List<PeliculaDTO>>(enCines);

            return resultado;
        }

        [HttpGet("PutGet/{id:int}")]

        public async Task<ActionResult<PeliculaPutGetDTO>> PutGet(int id)
        {
            var peliculaActionResult = await Get(id);
            if (peliculaActionResult.Result is NotFoundResult) { return NotFound(); }

            var pelicula = peliculaActionResult.Value;
            var generosSelecionadosIds = pelicula.Generos.Select(x => x.Id).ToList();
            var generosNoSeleccionados = await context.Generos
                .Where(x => !generosSelecionadosIds.Contains(x.Id))
                .ToListAsync();

            var cinesSelecionadosIds = pelicula.Cines.Select(x => x.Id).ToList();
            var cinesNoSeleccionados = await context.Cines
                .Where(x => !cinesSelecionadosIds.Contains(x.Id))
                .ToListAsync();
            var generosNoSelecionadosDTO = mapper.Map<List<GeneroDTO>>(generosNoSeleccionados);
            var cinesNoSelecionadosDTO = mapper.Map<List<CineDTO>>(cinesNoSeleccionados);

            var respuesta = new PeliculaPutGetDTO();
            respuesta.Pelicula = pelicula;
            respuesta.GenerosSeleccionados = pelicula.Generos;
            respuesta.GenerosNoSeleccionados = generosNoSelecionadosDTO;
            respuesta.CinesSeleccionados = pelicula.Cines;
            respuesta.CinesNoSeleccionados = cinesNoSelecionadosDTO;
            respuesta.Actores = pelicula.Actores;

            return respuesta;

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Esto proporcionará detalles sobre qué campos son incorrectos
            }

            var pelicula = await context.Peliculas
                .Include(x => x.PeliculasActores)
                .Include(x => x.PeliculasGeneros)
                .Include(x => x.PeliculasCines)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            pelicula = mapper.Map(peliculaCreacionDTO, pelicula);

            if (peliculaCreacionDTO.Poster != null)
            {
                pelicula.Poster = await almacenadorArchivos.EditarArchico(contenedor, peliculaCreacionDTO.Poster, pelicula.Poster);

            }

            EscribirOrdenActores(pelicula);

            await context.SaveChangesAsync();
            return NoContent();

        }


        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas
                .Include(x => x.PeliculasGeneros).ThenInclude(x => x.Genero)
                .Include(x => x.PeliculasActores).ThenInclude(x => x.Actor)
                .Include(x => x.PeliculasCines).ThenInclude(x => x.Cine)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula == null) { return NotFound(); }



            var promedioVoto = 0.0;
            var usuarioVoto = 0;

            if (await context.Ratings.AnyAsync(x => x.PeliculaId == id))
            {
                promedioVoto = await context.Ratings.Where(x => x.PeliculaId == id)
                    .AverageAsync(x => x.Puntuacion);

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var emailClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                    var email = emailClaim.Value;
                    var usuario = await userManager.FindByEmailAsync(email);
                    var usuarioId = usuario.Id;
                    var ratingDB = await context.Ratings
                        .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.PeliculaId == id);

                    if (ratingDB != null)
                    {
                        usuarioVoto = ratingDB.Puntuacion;
                    }
                }

            }

            var dto = mapper.Map<PeliculaDTO>(pelicula);
            dto.VotoUsuario = usuarioVoto;
            dto.PromedioVoto = promedioVoto;
            dto.Actores = dto.Actores.OrderBy(x => x.Orden).ToList();

            return dto;
        }

        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] PeliculasFiltrarDTO peliculasFiltrarDTO)
        {
            var peliculasQueryable = context.Peliculas.AsQueryable();

            if (!string.IsNullOrEmpty(peliculasFiltrarDTO.Titulo))
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.Titulo.Contains(peliculasFiltrarDTO.Titulo));
            }

            if (peliculasFiltrarDTO.EnCines)
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.EnCines);
            }

            if (peliculasFiltrarDTO.ProximosEstrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(x => x.FechaLanzamiento > hoy);
            }

            if (peliculasFiltrarDTO.GeneroId != 0)
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.PeliculasGeneros.Select(y => y.GeneroId)
                    .Contains(peliculasFiltrarDTO.GeneroId));
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(peliculasQueryable);

            var peliculas = await peliculasQueryable.Paginar(peliculasFiltrarDTO.PaginacionDTO).ToListAsync();
            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);

            if (peliculaCreacionDTO.Poster != null)
            {
                pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenedor, peliculaCreacionDTO.Poster);
            }

            EscribirOrdenActores(pelicula);
            await context.Peliculas.AddAsync(pelicula);
            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpGet("postget")]
        public async Task<ActionResult<PeliculasPostGetDTO>> PostGet()
        {
            var cines = await context.Cines.ToListAsync();
            var generos = await context.Generos.ToListAsync();

            var cinesDTO = mapper.Map<List<CineDTO>>(cines);
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);

            return new PeliculasPostGetDTO { Cines = cinesDTO, Generos = generosDTO };
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            context.Remove(pelicula);
            await context.SaveChangesAsync();
            await almacenadorArchivos.BorrarArchivo(pelicula.Poster, contenedor);
            return NoContent();
        }

        private void EscribirOrdenActores(Pelicula pelicula)
        {
            if (pelicula.PeliculasActores != null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }
        }


    }
}
