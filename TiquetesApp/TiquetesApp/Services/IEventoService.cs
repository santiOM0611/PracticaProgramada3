using TiquetesApp.Models;

namespace TiquetesApp.Services
{
    public interface IEventoService
    {
        List<Evento> ObtenerTodos();
        Evento ObtenerDetalle(int id);
        void Crear(Evento evento);
    }
}