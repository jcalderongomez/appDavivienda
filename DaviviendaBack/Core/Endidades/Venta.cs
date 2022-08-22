using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entidades
{
    public class Venta
    {        
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage= "La fecha de la venta es requerida")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 100")]
        public string FechaVenta { get; set; }

        [Required(ErrorMessage= "El cliente al que se le va a vender es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 100")]
        public int ClienteId { get; set; }
                
        [ForeignKey ("ClienteId")]
        public Cliente Cliente {get; set; }
        
        [Required(ErrorMessage= "El producto que se va a vender es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 100")]
        
        public int ProductoId { get; set; }
        
        [ForeignKey ("ProductoId")]
        public Producto Producto  {get; set; }
        
        [Required(ErrorMessage= "El descuento que se va a realizar sobre la venta es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 100")]
        
        public double Descuento { get; set; }
        
        [Required(ErrorMessage= "El total de la venta es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 100")]
        public double Total { get; set; }
    }
}