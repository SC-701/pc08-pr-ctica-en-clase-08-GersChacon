using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        [BindProperty]
        public ProductoResponse producto { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> categorias { get; set; } = new();

        [BindProperty]
        public List<SelectListItem> subCategorias { get; set; } = new();

        [BindProperty]
        public Guid categoriaSeleccionada { get; set; }

        [BindProperty]
        public Guid subCategoriaSeleccionada { get; set; }

        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task<ActionResult> OnGet(Guid? id)
        {
            if (id == null)
                return NotFound();

            string endpointProducto = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");
            using var cliente = new HttpClient();
            var solicitudProducto = new HttpRequestMessage(HttpMethod.Get, string.Format(endpointProducto, id));
            var respuestaProducto = await cliente.SendAsync(solicitudProducto);
            respuestaProducto.EnsureSuccessStatusCode();

            var resultadoJson = await respuestaProducto.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            producto = JsonSerializer.Deserialize<ProductoResponse>(resultadoJson, opciones)!;

            var listaCategorias = await ObtenerCategoriasAsync();
            categorias = listaCategorias.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text  = c.Nombre
            }).ToList();

            if (producto != null && !string.IsNullOrEmpty(producto.Categoria))
            {
                var catMatch = categorias.FirstOrDefault(c => c.Text == producto.Categoria);
                if (catMatch != null)
                {
                    categoriaSeleccionada = Guid.Parse(catMatch.Value);

                    var listaSubs = await ObtenerSubCategoriasAsync(categoriaSeleccionada);
                    subCategorias = listaSubs.Select(s => new SelectListItem
                    {
                        Value    = s.Id.ToString(),
                        Text     = s.Nombre,
                        Selected = s.Nombre == producto.SubCategoria
                    }).ToList();

                    var subMatch = subCategorias.FirstOrDefault(s => s.Text == producto.SubCategoria);
                    if (subMatch != null)
                        subCategoriaSeleccionada = Guid.Parse(subMatch.Value);
                }
            }

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (producto.Id == Guid.Empty)
                return NotFound();

            if (!ModelState.IsValid)
                return Page();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarProducto");
            using var cliente = new HttpClient();

            var request = new ProductoRequest
            {
                IdSubCategoria    = subCategoriaSeleccionada,
                Nombre            = producto.Nombre,
                Descripcion       = producto.Descripcion,
                Precio            = producto.Precio,
                Stock             = producto.Stock,
                CodigoBarras      = producto.CodigoBarras
            };

            var respuesta = await cliente.PutAsJsonAsync(string.Format(endpoint, producto.Id), request);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
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
