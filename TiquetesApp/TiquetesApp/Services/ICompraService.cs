using TiquetesApp.Models;
using TiquetesApp.Models.ViewModels;

namespace TiquetesApp.Services
{
    public interface ICompraService
    {
        Compra Comprar(CompraViewModel model, int usuarioId, string nombreCliente);
        List<Compra> ObtenerHistorialUsuario(int usuarioId);
        List<Compra> ObtenerTodas();
    }
}