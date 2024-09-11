
namespace peliculasWebApi.Utilidades
{
    public interface IAlmacenadorArchivos
    {
        Task BorrarArchivo(string ruta, string contenedor);
        Task<string> EditarArchico(string contenedor, IFormFile archivo, string ruta);
        Task<string> GuardarArchivo(string contenedor, IFormFile archivo);
    }
}