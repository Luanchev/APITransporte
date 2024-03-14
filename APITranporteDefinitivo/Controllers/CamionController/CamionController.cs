using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using Transporte.Core.Entities;
using Transporte.Core.Interfaces.ICamion;

namespace APITranporteDefinitivo.Controllers.CamionController
{
    //esta es la capa que expone los servicios

    [Route("api")] //las rutas se definen con minuscula
    [ApiController]
    public class CamionController : ControllerBase
    {

        public readonly ICamion serviceCamion;
        public ResponseService rs = new ResponseService();  

        //constructor
        public CamionController(ICamion _serviceCamion) 
        {
            serviceCamion = _serviceCamion; 
        }


        [HttpGet] // estos son los metodos HTTP
        [Route("camion")]
        public async Task<IActionResult> GetAllCamiones()
        {
            var rService = await serviceCamion.GetAllCamiones(); 

            return Ok(rService);
        }


        [HttpGet]
        [Route("camion/placa/{placa}")]
        public async Task<IActionResult> GetCamionByPlaca([FromRoute] string placa) // 
        {
            var rService = await serviceCamion.GetCamionByPlaca(placa);

            if (rService.Status == 400)
            {
                return NotFound(rService); //retorna el estatus 
            }
            else if (rService.Status == 200)
            {

                return Ok(rService);
            }
            else
            {
                return BadRequest(rService);
            }               
        }


        [HttpPost]
        [Route("camion")] 
        public async Task<IActionResult> CreateCamion([FromBody] CamionDTO camion) //frombody esta indicando los parametros que recibe
        {
            var rService = await serviceCamion.CreateCamion(camion);
            if (rService.Status == 400)
            {
                return NotFound(rService); //retorna el estatus 
            }
            else if (rService.Status == 200)
            {

                return Ok(rService);
            }
            else
            {
                return BadRequest(rService);
            }

        }

        [HttpPut]
        [Route("camion")]

        public async Task<IActionResult> EditCamion([FromBody] CamionDTO camion) //frombody esta indicando los parametros que recibe
        {
            var rService = await serviceCamion.EditCamion(camion);
            if (rService.Status == 400)
            {
                return NotFound(rService); //retorna el estatus 
            }
            else if (rService.Status == 200)
            {

                return Ok(rService);
            }
            else
            {
                return BadRequest(rService);
            }

        }


        [HttpDelete]
        [Route("camion/{IdCamion}")]

        public async Task<IActionResult> DeleteCamion( int IdCamion) 
        {
            var rService = await serviceCamion.DeleteCamion(IdCamion);
            if (rService.Status == 400)
            {
                return NotFound(rService); //retorna el estatus 
            }
            else if (rService.Status == 200)
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
