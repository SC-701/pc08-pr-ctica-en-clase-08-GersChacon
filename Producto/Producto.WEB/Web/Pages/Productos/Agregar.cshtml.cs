using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class AgregarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        [BindProperty]
        public ProductoRequest producto { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> subCategorias { get; set; } = new();

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public ActionResult OnGet()
        {
            subCategorias = new List<SelectListItem>();
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarProducto");
            using var cliente = new HttpClient();

            var respuesta = await cliente.PostAsJsonAsync(endpoint, producto);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }

        public async Task<JsonResult> OnGetObtenerCategorias()
        {
            var categorias = await ObtenerCategoriasAsync();
            return new JsonResult(categorias);
        }

        public async Task<JsonResult> OnGetObtenerSubCategorias(Guid categoriaId)
        {
            var subs = await ObtenerSubCategoriasAsync(categoriaId);
            return new JsonResult(subs);
        }

        private async Task<List<TokenItem>> ObtenerCategoriasAsync()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias");
            using var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            if (!respuesta.IsSuccessStatusCode || respuesta.StatusCode == HttpStatusCode.NoContent)
                return new List<TokenItem>();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<TokenItem>>(resultado, opciones) ?? new();
        }

        private async Task<List<SubCategoriaItem>> ObtenerSubCategoriasAsync(Guid categoriaId)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerSubCategorias");
            using var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, categoriaId));

            var respuesta = await cliente.SendAsync(solicitud);
            if (!respuesta.IsSuccessStatusCode || respuesta.StatusCode == HttpStatusCode.NoContent)
                return new List<SubCategoriaItem>();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<SubCategoriaItem>>(resultado, opciones) ?? new();
        }
    }
}
