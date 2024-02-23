using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sistemaVentas_BackEnd.Entities;
using sistemaVentas_BackEnd.Entities.Responses;
using sistemaVentas_BackEnd.repository;

namespace sistemaVentas_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiendaController : ControllerBase
    {
        public readonly ILogger<TiendaController> logger;
        public readonly ITienda _tienda;
        public TiendaController(ILogger<TiendaController> logger,ITienda tienda)
        {
            this.logger = logger;
            _tienda = tienda;
        }

        [HttpGet("obtenerTiendas")]
        public async Task<IActionResult> obtenerTiendas()
        {
            var response = new responseBase<List<Tienda>>();
            try
            {
                response = await _tienda.obtenerTiendas();
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("obtenerTiendasbyId/{id}")]
        public async Task<IActionResult> obtenerTiendasbyId(int id)
        {
            var response = new responseBase<Tienda>();
            try
            {
                response = await _tienda.obtenerTiendabyId(id);

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost("insertarTienda")]
        public async Task<IActionResult> insertarTienda([FromBody] Tienda tienda)
        {
            var response = new responseBase<Tienda>();
            try
            {
                response = await _tienda.insertarTienda(tienda);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }
    }
}
