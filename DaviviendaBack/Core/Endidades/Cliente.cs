using System.ComponentModel.DataAnnotations;

namespace Core.Entidades
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage= "La cédula del cliente es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 20")]
        public string Cedula { get; set; }

        [Required(ErrorMessage= "El nombre del cliente es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 50")]
        public string Nombre { get; set; }

        [Required(ErrorMessage= "El apellido del cliente es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 50")]
        public string Apellido { get; set; }

        [Required(ErrorMessage= "La dirección del cliente es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 100")]
        public string Direccion { get; set; }

        [Required(ErrorMessage= "El teléfono del cliente es requerido")]
        [MaxLength(100, ErrorMessage = "No sea mayor a 15")]
        public string Telefono { get; set; }
    }
}