using Microsoft.AspNetCore.Identity;

namespace TiquetesApp.Models
{
    public class Usuario : IdentityUser<int>
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public List<Compra> Compras { get; set; } = new();
    }
}