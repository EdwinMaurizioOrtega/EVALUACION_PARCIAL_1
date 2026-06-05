using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.Models
{
    public class ClienteModel
    {
        [Key]
        public int cliente_id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [Display(Name = "Apellido")]
        public string apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "La cédula debe tener exactamente 10 dígitos")]
        [Display(Name = "Cédula")]
        public string cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [Display(Name = "Email")]
        public string email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe tener exactamente 10 dígitos")]
        [Display(Name = "Teléfono")]
        public string telefono { get; set; } = string.Empty;
    }
}
