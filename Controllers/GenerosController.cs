using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using peliculasWebApi.Entidades;

namespace peliculasWebApi.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController: ControllerBase
    {
        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDbContext context;

        public GenerosController(ILogger<GenerosController> logger,
            ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        //Cualquiera de estas tres formas de hacer el endpoint funciona. 

        //[HttpGet("listao")] // api/generos/listado
        //[HttpGet("/listadogeneros")] // /listadogeneros 
        [HttpGet] // api/generos
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
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public AcceptedResult Put([FromBody] Genero genero) {

            throw new NotImplementedException();
        }

        [HttpDelete]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }



    }
}
