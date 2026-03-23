using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;

namespace Flujo
{
    public class ProductoFlujo : IProductoFlujo
    {
        private readonly IProductoDA _productoDA;
        private readonly IProductoReglas _productoReglas;

        public ProductoFlujo(IProductoDA productoDA, IProductoReglas productoReglas)
        {
            _productoDA = productoDA;
            _productoReglas = productoReglas;
        }

        public async Task<Guid> Agregar(ProductoRequest producto)
        {
            return await _productoDA.Agregar(producto);
        }

        public async Task<Guid> Editar(Guid Id, ProductoRequest producto)
        {
            return await _productoDA.Editar(Id, producto);
        }

        public async Task<Guid> Eliminar(Guid Id)
        {
            return await _productoDA.Eliminar(Id);
        }

        public async Task<IEnumerable<ProductoResponse>> Obtener()
        {
            return await _productoDA.Obtener();
        }

        public async Task<ProductoDetalle> Obtener(Guid Id)
        {
            var productoBase = await _productoDA.Obtener(Id);

            if (productoBase == null)
                return null;

            var productoDetalle = new ProductoDetalle
            {
                Id = productoBase.Id,
                Nombre = productoBase.Nombre,
                Descripcion = productoBase.Descripcion,
                Precio = productoBase.Precio,
                Stock = productoBase.Stock,
                CodigoBarras = productoBase.CodigoBarras,
                SubCategoria = productoBase.SubCategoria,
                Categoria = productoBase.Categoria
            };

            productoDetalle.PrecioUSD = await _productoReglas.CalcularPrecioUSD(productoBase.Precio);

            return productoDetalle;
        }
    }
}