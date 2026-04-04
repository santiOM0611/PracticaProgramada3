using TiquetesApp.Models;

namespace TiquetesApp.Repositories
{
    public interface ICompraRepository
    {
        void Agregar(Compra compra);
        List<Compra> ObtenerPorUsuario(int usuarioId);
        List<Compra> ObtenerTodas();
    }
}