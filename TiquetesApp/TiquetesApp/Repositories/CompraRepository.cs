using Microsoft.EntityFrameworkCore;
using TiquetesApp.Data;
using TiquetesApp.Models;

namespace TiquetesApp.Repositories
{
    public class CompraRepository : ICompraRepository
    {
        private readonly AppDbContext _context;

        public CompraRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Agregar(Compra compra)
        {
            _context.Compras.Add(compra);
            _context.SaveChanges();
        }

        public List<Compra> ObtenerPorUsuario(int usuarioId)
            => _context.Compras
                .Include(c => c.Evento)
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.FechaCompra)
                .ToList();

        public List<Compra> ObtenerTodas()
            => _context.Compras
                .Include(c => c.Evento)
                .Include(c => c.Usuario)
                .OrderByDescending(c => c.FechaCompra)
                .ToList();
    }
}