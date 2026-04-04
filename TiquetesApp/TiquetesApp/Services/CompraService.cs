using TiquetesApp.Exceptions;
using TiquetesApp.Models;
using TiquetesApp.Models.ViewModels;
using TiquetesApp.Repositories;

namespace TiquetesApp.Services
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _compraRepo;
        private readonly IEventoRepository _eventoRepo;

        public CompraService(ICompraRepository compraRepo, IEventoRepository eventoRepo)
        {
            _compraRepo = compraRepo;
            _eventoRepo = eventoRepo;
        }

        public Compra Comprar(CompraViewModel model, int usuarioId, string nombreCliente)
        {
            var evento = _eventoRepo.ObtenerPorId(model.EventoId);

            if (evento == null)
                throw new NotFoundException("El evento no existe.");

            if (evento.CantidadDisponible <= 0)
                throw new BusinessException("No hay tiquetes disponibles para este evento.");

            if (model.Cantidad > evento.CantidadDisponible)
                throw new BusinessException($"Solo quedan {evento.CantidadDisponible} tiquetes disponibles.");

            var compra = new Compra
            {
                EventoId = evento.Id,
                UsuarioId = usuarioId,
                NombreCliente = nombreCliente,
                Cantidad = model.Cantidad,
                Total = evento.Precio * model.Cantidad,
                FechaCompra = DateTime.Now
            };

            // Reducir disponibilidad
            evento.CantidadDisponible -= model.Cantidad;
            _eventoRepo.Actualizar(evento);

            _compraRepo.Agregar(compra);

            return compra;
        }

        public List<Compra> ObtenerHistorialUsuario(int usuarioId)
            => _compraRepo.ObtenerPorUsuario(usuarioId);

        public List<Compra> ObtenerTodas()
            => _compraRepo.ObtenerTodas();
    }
}