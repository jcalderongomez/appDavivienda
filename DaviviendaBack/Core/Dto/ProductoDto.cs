using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entidades;

namespace Core.Dto
{
    public class ProductoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage= "El nombre del producto es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 100")]
        public string Nombre { get; set; }

        [Required(ErrorMessage= "El precio del producto es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 10")]
        public float Precio { get; set; }

        [Required(ErrorMessage= "El stock del producto es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 10")]
        public int Stock { get; set; }

        [Required(ErrorMessage= "El proveedor del producto es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 5")]
        public int ProveedorId { get; set; }
        
        [ForeignKey ("ProveedorId")]
        public Proveedor Proveedor { get; set; }        
    }
}