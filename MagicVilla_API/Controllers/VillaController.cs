using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    //Nombre del servidor - HEMDIRLAP306\SQLEXPRESS

    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase //Herencia
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db) //Inyeccion de dependencias
        { 
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Obtener todas las villas");
            //return Ok(VillaStore.villaList); //Trabajando con lista en memoria

            return Ok(_db.Villas.ToList()); //Trabajando con bd
        }

        //ProducesResponse para documentar los tipos de respuesta (status codes) que retorna el endpoint/método
        //El modelo que sale de ejemplo en swagger viene de lo que esta entre ActionResult, en este caso VillaDto
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation("Error al obtener la villa " + id);
                return BadRequest("El id debe ser mayor a 0");
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound("Villa no encontrada");
            }

            return Ok(villa);
        }

        //[FromBody] indica que vamos a recibir datos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CrearVilla([FromBody]VillaDto villaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Validación personalizada de ModelState
            if(_db.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower())  != null)
            {
                ModelState.AddModelError("NombreExiste", "Ya existe una Villa con ese nombre");
                return BadRequest(ModelState);
            }

            if (villaDto == null)
            {
                return BadRequest();
            }
            else if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            /*villaDto.Id = VillaStore.villaList.Max(v => v.Id) + 1;

            VillaStore.villaList.Add(villaDto);

            return Ok(villaDto);*/ //Para listas, el return con ok es para bd o lista en memoria

            Villa villa = new ()
            {
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Ocupantes = villaDto.Ocupantes,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Amenidad = villaDto.Amenidad
            };

            _db.Villas.Add(villa);
            _db.SaveChanges();
            //Para retornar la Url del recurso creado
            return CreatedAtRoute("GetVilla", new { id = villaDto.Id}, villaDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa);
            _db.SaveChanges();

            //Dice que se debe retornar no content cuando se elimina sin embargo es el primer curso donde veo esto
            return NoContent();
        }
        
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if(villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }

            //var villa = VillaStore.villaList.Find(v => v.Id == id);

            //if (villa == null)
            //{
            //    return NotFound();
            //}

            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            Villa villa = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Ocupantes = villaDto.Ocupantes,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Amenidad = villaDto.Amenidad
            };

            _db.Villas.Update(villa);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            //As no tracking para consultar el registro y que no se quede abierto (no se traquee?)
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            VillaDto villaDto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                Tarifa = villa.Tarifa,
                Ocupantes = villa.Ocupantes,
                MetrosCuadrados = villa.MetrosCuadrados,
                ImagenUrl = villa.ImagenUrl,
                Amenidad = villa.Amenidad
            };

            patchDto.ApplyTo(villaDto, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Ocupantes = villaDto.Ocupantes,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Amenidad = villaDto.Amenidad
            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();
            return NoContent();
        }

    }
}
