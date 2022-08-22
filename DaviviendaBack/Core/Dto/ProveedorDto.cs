using System.ComponentModel.DataAnnotations;

namespace Core.Dto
{
    public class ProveedorDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage= "El RUT del proveedor es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 100")]
        public string Rut { get; set; }

        [Required(ErrorMessage= "El nombre del proveedor es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 100")]
        public string Nombre { get; set; }

        [Required(ErrorMessage= "El teléfono del proveedor es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 100")]
        public string Telefono { get; set; }

        [Required(ErrorMessage= "La pagina del proveedor es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 100")]
        public string Pagina { get; set; }
    }
}