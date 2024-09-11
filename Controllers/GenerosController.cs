<<<<<<< HEAD
﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using peliculasWebApi.DTOs;
using peliculasWebApi.Entidades;
using peliculasWebApi.Utilidades;
=======
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using peliculasWebApi.Entidades;
>>>>>>> 2af95bc638e0cb8dfd888ba82555fb6ab1b34aea

namespace peliculasWebApi.Controllers
{
    [Route("api/generos")]
    [ApiController]
<<<<<<< HEAD
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class GenerosController : ControllerBase
    {
        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenerosController(ILogger<GenerosController> logger,
            ApplicationDbContext context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
=======
    public class GenerosController: ControllerBase
    {
        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDbContext context;

        public GenerosController(ILogger<GenerosController> logger,
            ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
>>>>>>> 2af95bc638e0cb8dfd888ba82555fb6ab1b34aea
        }

        //Cualquiera de estas tres formas de hacer el endpoint funciona. 

        //[HttpGet("listao")] // api/generos/listado
        //[HttpGet("/listadogeneros")] // /listadogeneros 
        [HttpGet] // api/generos
<<<<<<< HEAD
        public async Task<ActionResult<List<GeneroDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {

            var queryable = context.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var generos = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<GeneroDTO>>(generos);

        }

        [HttpGet("todos")]
        [AllowAnonymous]
        public async Task<ActionResult<List<GeneroDTO>>> Todos()
        {
            var generos = await context.Generos.OrderBy(x => x.Nombre).ToListAsync();
            return mapper.Map<List<GeneroDTO>>(generos);
        }

        [HttpGet("{Id:int}")] // Con esta restriccion :int, no dejo que entren con un valor cero
        public async Task<ActionResult<GeneroDTO>> Get(int Id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == Id);
            if (genero == null)
            {
                return NotFound();
            }
            return mapper.Map<GeneroDTO>(genero);

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(genero);
=======
        public async Task<ActionResult<List<Genero>>> Get() 
        { 
            return await context.Generos.ToListAsync();
        }

        [HttpGet("{id:int}")] // Con esta restriccion :int, no dejo que entren con un valor cero
        public async Task<ActionResult<Genero>> Get(int id)
        {
            throw new NotImplementedException();
           
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Genero genero) {
            context.Add(genero);    
>>>>>>> 2af95bc638e0cb8dfd888ba82555fb6ab1b34aea
            await context.SaveChangesAsync();
            return NoContent();
        }

<<<<<<< HEAD
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {

            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);

            if (genero == null)
            {
                return NotFound();
            }

            genero = mapper.Map(generoCreacionDTO, genero);
            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Genero() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
=======
        [HttpPut]
        public AcceptedResult Put([FromBody] Genero genero) {

            throw new NotImplementedException();
        }

        [HttpDelete]
        public void Delete(int id)
        {
            throw new NotImplementedException();
>>>>>>> 2af95bc638e0cb8dfd888ba82555fb6ab1b34aea
        }



    }
}
