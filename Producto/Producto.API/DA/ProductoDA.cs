using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ProductoDA : IProductoDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;

        #region Constructor
        public ProductoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }
        #endregion

        #region Operaciones
        public async Task<Guid> Agregar(ProductoRequest producto)
        {
            string query = @"AgregarProducto";

            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(
                query,
                new
                {
                    Id = Guid.NewGuid(),
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    IdSubCategoria = producto.IdSubCategoria,
                    CodigoBarras = producto.CodigoBarras
                });

            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid Id, ProductoRequest producto)
        {
            await VerificarProductoExiste(Id);

            string query = @"EditarProducto";

            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(
                query,
                new
                {
                    Id = Id,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    CodigoBarras = producto.CodigoBarras,
                    IdSubCategoria = producto.IdSubCategoria
                });

            return resultadoConsulta;
        }

        public async Task<Guid> Eliminar(Guid Id)
        {
            await VerificarProductoExiste(Id);
            string query = @"EliminarProducto";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(
                query,
                new
                {
                    Id = Id
                });

            return resultadoConsulta;
        }

        public async Task<IEnumerable<ProductoResponse>> Obtener()
        {
            string query = @"ObtenerProductos";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProductoResponse>(query);
            return resultadoConsulta;
        }

        public async Task<ProductoResponse> Obtener(Guid Id)
        {
            string query = @"ObtenerProducto";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProductoResponse>(
                query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }
        #endregion

        #region Helpers
        private async Task VerificarProductoExiste(Guid Id)
        {
            ProductoResponse? producto = await Obtener(Id);
            if (producto == null)
                throw new Exception("No se encontró el producto");
        }
        #endregion
    }
}