using TiquetesApp.Exceptions;
using TiquetesApp.Models;
using TiquetesApp.Repositories;

namespace TiquetesApp.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _repo;

        public EventoService(IEventoRepository repo)
        {
            _repo = repo;
        }

        public List<Evento> ObtenerTodos()
            => _repo.ObtenerTodos();

        public Evento ObtenerDetalle(int id)
        {
            var evento = _repo.ObtenerPorId(id);
            if (evento == null)
                throw new NotFoundException($"Evento con id {id} no encontrado.");
            return evento;
        }

        public void Crear(Evento evento)
            => _repo.Agregar(evento);
    }
}