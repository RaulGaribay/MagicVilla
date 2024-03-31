using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModel
{
    public class NumeroVillaViewModel
    {
        //Directorio a nivel de los models llamado View Models, y aquí vamos a crear nosotros nuestro
        //primer ViewModel, el cual sirve para encapsular distintos modelos/clases.
        //En este caso nosotros vamos a encapsular dos clases, una lista y la clase de número de villa.

        public NumeroVillaViewModel()
        {
            NumeroVilla = new NumeroVillaCreateDto();
        }

        public NumeroVillaCreateDto NumeroVilla { get; set; }

        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
