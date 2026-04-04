using TiquetesApp.Data;
using TiquetesApp.Models;

namespace TiquetesApp.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly AppDbContext _context;

        public EventoRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Evento> ObtenerTodos()
            => _context.Eventos.OrderBy(e => e.Fecha).ToList();

        public Evento? ObtenerPorId(int id)
            => _context.Eventos.Find(id);

        public bool ExisteId(int id)
            => _context.Eventos.Any(e => e.Id == id);

        public void Agregar(Evento evento)
        {
            _context.Eventos.Add(evento);
            _context.SaveChanges();
        }

        public void Actualizar(Evento evento)
        {
            _context.Eventos.Update(evento);
            _context.SaveChanges();
        }
    }
}