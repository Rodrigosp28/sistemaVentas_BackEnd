using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sistemaVentas_BackEnd.Entities;
using sistemaVentas_BackEnd.Entities.Responses;
using sistemaVentas_BackEnd.repository;

namespace sistemaVentas_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IClientes _cliente;
        public ClienteController(ILogger<ClienteController> logger,IClientes clientes)
        {
            _logger = logger;
            _cliente = clientes;
        }

        [HttpGet("obtenerCliente")]
        public async Task<IActionResult> obtenerClientes()
        {
            var response = new responseBase<List<Clientes>>();
            try
            {
                response = await _cliente.obtenerClientes();
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("obtenerClientebyId/{id}")]
        public async Task<IActionResult> obtenerClientesbyId(int id)
        {
            var response = new responseBase<Clientes>();
            try
            {
                response = await _cliente.obtenerClientebyId(id);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost("insertarCliente")]
        public async Task<IActionResult> insertarCliente([FromBody] Clientes cliente)
        {
            var response = new responseBase<Clientes>();
            try
            {
                response = await _cliente.insertarCliente(cliente);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost("sesionCliente")]
        public async Task<IActionResult> sesionCliente([FromBody] Clientes cliente)
        {
            var response = new responseBase<Clientes>();
            try
            {
                response = await _cliente.sesionCliente(cliente);
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
