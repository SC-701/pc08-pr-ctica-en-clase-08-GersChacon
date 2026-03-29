using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase, ICategoriaController
    {
        private readonly ICategoriaFlujo _categoriaFlujo;

        public CategoriaController(ICategoriaFlujo categoriaFlujo)
        {
            _categoriaFlujo = categoriaFlujo;
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var result = await _categoriaFlujo.Obtener();
            if (!result.Any())
                return NoContent();

            return Ok(result);
        }
    }
}
