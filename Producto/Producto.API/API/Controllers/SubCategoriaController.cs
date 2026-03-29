using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriaController : ControllerBase, ISubCategoriaController
    {
        private readonly ISubCategoriaFlujo _subCategoriaFlujo;

        public SubCategoriaController(ISubCategoriaFlujo subCategoriaFlujo)
        {
            _subCategoriaFlujo = subCategoriaFlujo;
        }

        [HttpGet("{idCategoria}")]
        public async Task<IActionResult> ObtenerPorCategoria([FromRoute] Guid idCategoria)
        {
            var result = await _subCategoriaFlujo.ObtenerPorCategoria(idCategoria);
            if (!result.Any())
                return NoContent();

            return Ok(result);
        }
    }
}
