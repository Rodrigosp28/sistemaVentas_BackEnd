using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sistemaVentas_BackEnd.Entities;
using sistemaVentas_BackEnd.Entities.Responses;
using sistemaVentas_BackEnd.repository;
using System;

namespace sistemaVentas_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        public readonly ILogger<ArticuloController> _logger;
        public readonly IArticulo _articulo;
        private readonly IHostEnvironment _environment;
        public ArticuloController(ILogger<ArticuloController> logger,IArticulo articulo, IHostEnvironment environment)
        {
            _logger = logger;
            _articulo = articulo;
            _environment = environment;
        }

        [HttpGet("obtenerArticulos")]
        public async Task<IActionResult>obtenerArticulos()
        {
            var response = new responseBase<List<Articulo>>();
            try
            {
                response = await _articulo.obtenerArticulos();
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("obtenerArticulosbyId/{id}")]
        public async Task<IActionResult> obtenerArticulosbyId(int id)
        {
            var response = new responseBase<Articulo>();
            try
            {
                response = await _articulo.obtenerArticulosbyId(id);

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("obtenerArticulosbyIdTienda/{id}")]
        public async Task<IActionResult> obtenerArticulosbyIdTienda(int id)
        {
            var response = new responseBase<List<Articulo>>();
            try
            {
                response = await _articulo.obtenerArticulosbyIdTienda(id);

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost("insertarArticulo")]
        public async Task<IActionResult> insertarArticulo(IFormCollection formdata)
        {
            var response = new responseBase<Articulo>();
            List<string> fileExtension = new List<string>() { ".png", ".jpg", ".jpeg" };


            try
            {
                var articulo = new Articulo();
                articulo.codigo = formdata["codigo"].ToString();
                articulo.descripcion = formdata["descripcion"].ToString();
                articulo.precio = Convert.ToDouble(formdata["precio"]);
                articulo.stock = Convert.ToInt32(formdata["stock"]);
                articulo.id_tienda = Convert.ToInt32(formdata["id_tienda"]);

                var files = HttpContext.Request.Form.Files;

                if (files.Count == 0)
                {
                    throw new Exception("Se debe agregar un archivo");
                }
                foreach (var file in files)
                {
                    var fileName = file.FileName;
                    var ext = Path.GetExtension(fileName);
                    var buffer = file.Length;
                    double mb = (buffer / 1024f) / 1024f;

                    if (mb > 2)
                    {
                        throw new Exception("Tamaño del archivo demasiado grande");
                    }

                    if (!fileExtension.Contains(ext))
                    {
                        throw new Exception("extension del archivo no permitido");
                    }

                    if (file.Length == 0)
                    {
                        throw new Exception("Erro al leer archivo");
                    }

                    var pathOrigen = Path.Combine(_environment.ContentRootPath);
                    var carpetaArticulo = "\\" + "Articulos" + "\\" + articulo.codigo;
                    var carpetaArchivos = pathOrigen + "\\" + "Content"  + carpetaArticulo;
                    var nombreArchivo = "articulo_" + articulo.codigo + ext;
                    var rutacompleta = carpetaArchivos + "\\" + nombreArchivo;

                    articulo.url_img = "/Content" + carpetaArticulo + "\\" + nombreArchivo;

                    articulo.url_img = articulo.url_img.Replace("\\", "/");
                    
                    response = await _articulo.insertarArticulo(articulo);

                    if(response.success)
                    {
                        if (!Directory.Exists(carpetaArchivos))
                        {
                            Directory.CreateDirectory(carpetaArchivos);
                        }

                        using (var stream = System.IO.File.Create(rutacompleta))
                        {
                            await file.CopyToAsync(stream);
                        }

                    }



                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost("relArticuloCliente")]
        public async Task<IActionResult> relArticuloCliente([FromBody]requestRelClienteArticulo req)
        {
            var response = new responseMain();
            try
            {
                response = await _articulo.insertarArticuloCliente(req.RelClienteArticulo);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }
    }
}
