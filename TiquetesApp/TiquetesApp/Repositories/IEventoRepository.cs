using TiquetesApp.Models;

namespace TiquetesApp.Repositories
{
    public interface IEventoRepository
    {
        List<Evento> ObtenerTodos();
        Evento? ObtenerPorId(int id);
        bool ExisteId(int id);
        void Agregar(Evento evento);
        void Actualizar(Evento evento);
    }
}