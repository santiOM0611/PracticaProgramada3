using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiquetesApp.Models
{
    public class Compra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EventoId { get; set; }
        public Evento? Evento { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Required]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debe comprar al menos 1 tiquete")]
        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public DateTime FechaCompra { get; set; } = DateTime.Now;
    }
}