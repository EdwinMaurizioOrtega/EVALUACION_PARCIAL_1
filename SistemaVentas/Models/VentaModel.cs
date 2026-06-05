using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaVentas.Models
{
    public class VentaModel
    {
        [Key]
        public int venta_id { get; set; }

        [Display(Name = "Fecha de Venta")]
        public DateTime fecha_venta { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        [Display(Name = "Cantidad")]
        public int cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio Unitario")]
        public decimal precio_unitario { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total")]
        public decimal total { get; set; }

        // FK → Clientes
        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        [Display(Name = "Cliente")]
        public int cliente_id { get; set; }

        [ForeignKey("cliente_id")]
        public ClienteModel? Cliente { get; set; }

        // FK → Productos
        [Required(ErrorMessage = "Debe seleccionar un producto")]
        [Display(Name = "Producto")]
        public int producto_id { get; set; }

        [ForeignKey("producto_id")]
        public ProductoModel? Producto { get; set; }
    }
}
