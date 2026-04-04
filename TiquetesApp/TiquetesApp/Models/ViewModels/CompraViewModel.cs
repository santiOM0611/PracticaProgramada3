using System.ComponentModel.DataAnnotations;

namespace TiquetesApp.Models.ViewModels
{
    public class CompraViewModel
    {
        [Required]
        public int EventoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Mínimo 1 tiquete")]
        public int Cantidad { get; set; }
    }
}