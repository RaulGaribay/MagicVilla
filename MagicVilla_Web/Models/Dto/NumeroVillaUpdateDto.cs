using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class NumeroVillaUpdateDto
    {
        //Villa numero
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }

        [Required]
        public int VillaId { get; set; }

        public string DetalleEspecial { get; set; }
    }
}
