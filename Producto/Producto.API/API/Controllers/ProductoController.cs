using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductoController : ControllerBase, IProductoController
    {
        private readonly IProductoFlujo _productoFlujo;
        private readonly ILogger<ProductoController> _logger;

        #region Operaciones
        public ProductoController(IProductoFlujo productoFlujo, ILogger<ProductoController> logger)
        {
            _productoFlujo = productoFlujo;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Agregar([FromBody] ProductoRequest producto)
        {
            var result = await _productoFlujo.Agregar(producto);
            return CreatedAtAction(nameof(Obtener), new { Id = result }, null);
        }

        [HttpPut("{Id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Editar([FromRoute] Guid Id, [FromBody] ProductoRequest producto)
        {
            if (!await VerificarProductoExiste(Id))
                return NotFound("El producto no existe");

            var result = await _productoFlujo.Editar(Id, producto);
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Eliminar([FromRoute] Guid Id)
        {
            if (!await VerificarProductoExiste(Id))
                return NotFound("El producto no existe");

            await _productoFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Obtener()
        {
            var result = await _productoFlujo.Obtener();
            if (!result.Any())
                return NoContent();

            return Ok(result);
        }

        [HttpGet("{Id}")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Obtener([FromRoute] Guid Id)
        {
            var result = await _productoFlujo.Obtener(Id);
            return Ok(result);
        }
        #endregion

        #region Helpers
        private async Task<bool> VerificarProductoExiste(Guid Id)
        {
            var producto = await _productoFlujo.Obtener(Id);
            return producto != null;
        }
        #endregion
    }
}
