using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;

namespace Reglas
{
    public class ProductoReglas : IProductoReglas
    {
        private readonly ITipoCambioServicio _tipoCambioServicio;
        private readonly IConfiguracion _configuracion;

        public ProductoReglas(ITipoCambioServicio tipoCambioServicio, IConfiguracion configuracion)
        {
            _tipoCambioServicio = tipoCambioServicio;
            _configuracion = configuracion;
        }

        public async Task<decimal> CalcularPrecioUSD(decimal precio)
        {
            try
            {
                var tipoCambio = await _tipoCambioServicio.ObtenerTipoCambio();

                if (!tipoCambio.HasValue || tipoCambio.Value == 0)
                {
                    throw new Exception("No se pudo obtener el tipo de cambio actual");
                }

                return Math.Round(precio / tipoCambio.Value, 2);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al calcular precio en USD: {ex.Message}", ex);
            }
        }
    }
}