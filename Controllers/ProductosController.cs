using Microsoft.AspNetCore.Mvc;
using ProductosADO.Api.Models;
using ProductosADO.Api.Repositories;

namespace ProductosADO.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepository _repo;
        public ProductosController(IProductoRepository repo) => _repo = repo;

        [HttpGet]
        public IActionResult ObtenerTodos() => Ok(_repo.ObtenerTodos());

        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            var prod = _repo.ObtenerPorId(id);
            return prod == null ? NotFound($"Producto {id} no existe") : Ok(prod);
        }

        [HttpGet("buscar/{termino}")]
        public IActionResult Buscar(string termino) => Ok(_repo.Buscar(termino));

        [HttpPost]
        public IActionResult Insertar([FromBody] Producto producto)
        {
            var nuevoId = _repo.Insertar(producto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoId }, new { id = nuevoId, mensaje = "Insertado correctamente" });
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Producto producto)
        {
            producto.Id = id;
            var ok = _repo.Actualizar(producto);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var ok = _repo.Eliminar(id);
            return ok ? NoContent() : NotFound();
        }
    }
}