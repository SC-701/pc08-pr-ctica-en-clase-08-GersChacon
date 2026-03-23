using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Servicios.Registro;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Servicios
{
    public class TipoCambioServicio : ITipoCambioServicio
    {
        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;

        public TipoCambioServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<decimal?> ObtenerTipoCambio()
        {
            try
            {
                var urlBase = _configuracion.ObtenerMetodo("BancoCentralCR", "UrlBase");
                var bearerToken = _configuracion.ObtenerMetodo("BancoCentralCR", "BearerToken");
                var fechaActual = DateTime.Now.ToString("yyyy/MM/dd");
                var urlCompleta = $"{urlBase}?fechaInicio={fechaActual}&fechaFin={fechaActual}&idioma=ES";
                var cliente = _httpClientFactory.CreateClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var respuesta = await cliente.GetAsync(urlCompleta);
                if (!respuesta.IsSuccessStatusCode)
                {
                    var errorContent = await respuesta.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error {respuesta.StatusCode}: {errorContent}");
                }
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var respuestaDeserializada = JsonSerializer.Deserialize<Root>(resultado, opciones);
                if (respuestaDeserializada?.datos != null &&
                respuestaDeserializada.datos.Count > 0 &&
                respuestaDeserializada.datos[0].indicadores != null &&
                 respuestaDeserializada.datos[0].indicadores.Count > 0 &&
                 respuestaDeserializada.datos[0].indicadores[0].series != null &&
                 respuestaDeserializada.datos[0].indicadores[0].series.Count > 0)
                {
                    return (decimal?)respuestaDeserializada.datos[0].indicadores[0].series[0].valorDatoPorPeriodo;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener tipo de cambio: {ex.Message}", ex);
            }
        }
    }
}