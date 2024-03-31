using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    //Nombre del servidor - HEMDIRLAP306\SQLEXPRESS

    [Route("api/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase //Herencia
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numeroRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo,
                                     INumeroVillaRepositorio numeroRepo, IMapper mapper) //Inyeccion de dependencias
        { 
            _logger = logger;
            _villaRepo = villaRepo;
            _numeroRepo = numeroRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                _logger.LogInformation("Obtener todas las villas");
                //return Ok(VillaStore.villaList); //Trabajando con lista en memoria
                //incluirPropiedades es para traer los datos del padre, en este caso Villa
                IEnumerable<NumeroVilla> numeroVillaList = await _numeroRepo.ObtenerTodos(incluirPropiedades:"Villa");

                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillaList); //Convierte de numeroVilla a numeroVillaDto
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            
            return _response;
        }

        //ProducesResponse para documentar los tipos de respuesta (status codes) que retorna el endpoint/método
        //El modelo que sale de ejemplo en swagger viene de lo que esta entre ActionResult, en este caso VillaDto
        //Mientras que los parametros vienen de lo que esta entre parentesis
        //Task siempre se cierra antes del nombre del método
        [HttpGet("{id:int}", Name = "GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogInformation("Error al obtener el numero villa " + id);
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest("El id debe ser mayor a 0. " + _response);
                }

                var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id, incluirPropiedades: "Villa");

                if (numeroVilla == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound("Numero Villa no encontrado. " + _response);
                }

                _response.Resultado = _mapper.Map<NumeroVillaDto>(numeroVilla); //Convierte de NumeroVilla a NumeroVillaDto
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response); 
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        //[FromBody] indica que vamos a recibir datos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearNumeroVilla([FromBody]NumeroVillaCreateDto createDto)
        {
            try
            {
                //Si usamos model state, no cambiar por _response
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //Validación personalizada de ModelState
                if (await _numeroRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "El número de Villa ya existe!");
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Id == createDto.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "El id de la Villa no existe!");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto); //Convierte de NumeroVillaCreateDto a NumeroVilla
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                await _numeroRepo.Crear(modelo);

                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;
                //Para retornar la Url del recurso creado
                return CreatedAtRoute("GetNumeroVilla", new { id = modelo.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);

                if (numeroVilla == null)
                {
                    _response.IsExitoso = false; 
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _numeroRepo.Remover(numeroVilla);
                //Dice que se debe retornar no content cuando se elimina sin embargo es el primer curso donde veo esto
                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return BadRequest(_response);
        }
        
        //UpdateVilla y UpdatePartialVilla pueden hacerse sin usar un try catch como en los anteriores métodos
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.VillaNo)
                {
                    _response.IsExitoso = false;    
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (await _villaRepo.Obtener(v => v.Id == updateDto.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "El id de la Villa no existe!");
                    return BadRequest(ModelState);
                }

                NumeroVilla numeroVilla = _mapper.Map<NumeroVilla>(updateDto); //Convierte de NumeroVillaUpdateDto a NumeroVilla

                await _numeroRepo.Actualizar(numeroVilla);
                _response.statusCode = HttpStatusCode.NoContent;
                
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return BadRequest(_response);
        }
    }
}
