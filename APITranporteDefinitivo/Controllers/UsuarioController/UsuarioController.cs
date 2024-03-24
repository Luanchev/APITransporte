using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Transporte.Core.Entities;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using Transporte.Core.Interfaces.IUser;
using Transporte.Core.Interfaces.ICamion;

namespace APITranporteDefinitivo.Controllers.UsuarioController
{
    [Route("api")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly string key;
        public readonly IUser serviceUser;
        public ResponseService rs = new ResponseService();

        public UsuarioController(IConfiguration config, IUser _serviceUser)
        {
            serviceUser = _serviceUser;
            key = config.GetSection("Jwt").GetSection("Key").ToString();
        }

        [HttpPost]
        [Route("validar")]
        public async Task<IActionResult> ValidarUsuario([FromBody]  UserDTO request)
        {
            // aqui lo que hacemos es validar la información de la base de datos
            var rService = await serviceUser.ValidarUsuario(request.correo, request.password);
            if (rService.Status == StatusCodes.Status404NotFound)
            {
                return NotFound(rService);
            }
            else if (rService.Status == StatusCodes.Status200OK)
            {
                return Ok(rService);
            }
            else
            {
                return BadRequest(rService);
            }


        }

    }
}
