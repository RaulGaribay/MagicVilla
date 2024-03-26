using AutoMapper;
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
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db, IMapper mapper) //Inyeccion de dependencias
        { 
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener todas las villas");
            //return Ok(VillaStore.villaList); //Trabajando con lista en memoria
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();


            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList)); //Trabajando con bd y automapper
        }

        //ProducesResponse para documentar los tipos de respuesta (status codes) que retorna el endpoint/método
        //El modelo que sale de ejemplo en swagger viene de lo que esta entre ActionResult, en este caso VillaDto
        //Task siempre se cierra antes del nombre del método
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation("Error al obtener la villa " + id);
                return BadRequest("El id debe ser mayor a 0");
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound("Villa no encontrada");
            }

            return Ok(_mapper.Map<VillaDto>(villa)); //Convierte de Villa a VillaDto
        }

        //[FromBody] indica que vamos a recibir datos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody]VillaCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Validación personalizada de ModelState
            if(await _db.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == createDto.Nombre.ToLower())  != null)
            {
                ModelState.AddModelError("NombreExiste", "Ya existe una Villa con ese nombre");
                return BadRequest(ModelState);
            }

            if (createDto == null)
            {
                return BadRequest();
            }

            /*villaDto.Id = VillaStore.villaList.Max(v => v.Id) + 1;

            VillaStore.villaList.Add(villaDto);

            return Ok(villaDto);*/ //Para listas, el return con ok es para bd o lista en memoria

            Villa villa = _mapper.Map<Villa>(createDto); //Convierte de VillaCreateDto a Villa

            await _db.Villas.AddAsync(villa);
            await _db.SaveChangesAsync();
            //Para retornar la Url del recurso creado
            return CreatedAtRoute("GetVilla", new { id = villa.Id}, villa);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();

            //Dice que se debe retornar no content cuando se elimina sin embargo es el primer curso donde veo esto
            return NoContent();
        }
        
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if(updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }
            
            Villa villa = _mapper.Map<Villa>(updateDto); //Convierte de VillaUpdateDto a Villa

            _db.Villas.Update(villa);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            //As no tracking para consultar el registro y que no se quede abierto (no se traquee?)
            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa); //Convierte de Villa a VillaUpdateDto

            patchDto.ApplyTo(villaDto, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(villaDto); //Convierte de VillaUpdateDto a Villa

            _db.Villas.Update(modelo);
            await _db.SaveChangesAsync();
            return NoContent();
        }

    }
}
