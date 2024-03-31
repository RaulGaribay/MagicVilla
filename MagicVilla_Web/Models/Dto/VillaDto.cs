using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [MaxLength(30, ErrorMessage = "El nombre debe tener máximo 30 caracteres")]
        public string Nombre { get; set; }

        public string Detalle { get; set; }

        [Required(ErrorMessage = "El campo tarifa es requerido")]
        public double Tarifa { get; set; }

        public int Ocupantes { get; set; }

        public int MetrosCuadrados { get; set; }

        public string ImagenUrl { get; set; }

        public string Amenidad { get; set; }
    }
}
