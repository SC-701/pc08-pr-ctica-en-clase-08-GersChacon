using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class SubCategoriaDA : ISubCategoriaDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;

        public SubCategoriaDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<IEnumerable<SubCategoria>> ObtenerPorCategoria(Guid idCategoria)
        {
            string query = "ObtenerSubCategoriasPorCategoria";
            var resultado = await _sqlConnection.QueryAsync<SubCategoria>(
                query, new { IdCategoria = idCategoria });
            return resultado;
        }
    }
}
