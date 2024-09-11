using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using peliculasWebApi.DTOs;
using peliculasWebApi.Entidades;
using System.Security.Claims;

namespace peliculasWebApi.Controllers
{
    [Route("api/rating")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;

        public RatingsController(UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] RatingDTO ratingDTO)
        {
            // Verifica que el claim "email" existe
            var emailClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                // Log para depurar
                var claims = HttpContext.User.Claims.Select(c => new { c.Type, c.Value }).ToList();
                Console.WriteLine("Claims disponibles: " + JsonConvert.SerializeObject(claims));

                return BadRequest("El claim de email no está presente.");
            }

            var email = emailClaim.Value;

            // Verifica que el usuario existe
            var usuario = await userManager.FindByEmailAsync(email);
            if (usuario == null)
            {
                return BadRequest("No se encontró un usuario con ese email.");
            }

            var usuarioId = usuario.Id;

            var ratingActual = await context.Ratings
                .FirstOrDefaultAsync(x => x.PeliculaId == ratingDTO.PeliculaId
                && x.UsuarioId == usuarioId);

            if (ratingActual == null)
            {
                var rating = new Rating();
                rating.PeliculaId = ratingDTO.PeliculaId;
                rating.Puntuacion = ratingDTO.Puntuacion;
                rating.UsuarioId = usuarioId;
                context.Add(rating);
            }
            else
            {
                ratingActual.Puntuacion = ratingDTO.Puntuacion;
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
