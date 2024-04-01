using Asp.Versioning;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //Para que aparezca en todas las versiones de la API
    [ApiVersionNeutral]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepo;
        private APIResponse _response;

        public UsuarioController(IUsuarioRepositorio usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
            _response = new();
        }

        [HttpPost("login")] // api/Usuario/login
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO modelo)
        {
            var loginResponse = await _usuarioRepo.Login(modelo);
            if (loginResponse.Usuario == null || loginResponse.Token.IsNullOrEmpty())
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorMessages.Add("UserName o Password son incorrectos");
                return BadRequest(_response);
            }

            _response.statusCode = HttpStatusCode.OK;
            _response.Resultado = loginResponse;
            return Ok(_response);
        }

        [HttpPost("registrar")] // api/Usuario/registrar
        public async Task<IActionResult> Registrar([FromBody] RegistroRequestDTO modelo)
        {
            bool isUsuarioUnico = _usuarioRepo.IsUsuarioUnico(modelo.UserName);

            if (!isUsuarioUnico)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorMessages.Add("UserName o Password son incorrectos");
                return BadRequest(_response);
            }

            var usuario = await _usuarioRepo.Registrar(modelo);

            if (usuario == null)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorMessages.Add("UserName o Password son incorrectos");
                return BadRequest(_response);
            }

            _response.statusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
