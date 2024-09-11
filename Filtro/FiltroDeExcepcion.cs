using Microsoft.AspNetCore.Mvc.Filters;

namespace peliculasWebApi.Filtro
{
<<<<<<< HEAD
    public class FiltroDeExcepcion : ExceptionFilterAttribute
    {
        private readonly ILogger<FiltroDeExcepcion> _logger;
        public FiltroDeExcepcion(ILogger<FiltroDeExcepcion> logger)
=======
    public class FiltroDeExcepcion: ExceptionFilterAttribute
    {
        private readonly ILogger<FiltroDeExcepcion> _logger;
        public FiltroDeExcepcion(ILogger<FiltroDeExcepcion> logger) 
>>>>>>> 2af95bc638e0cb8dfd888ba82555fb6ab1b34aea
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}
