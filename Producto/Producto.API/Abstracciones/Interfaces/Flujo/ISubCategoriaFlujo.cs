using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface ISubCategoriaFlujo
    {
        Task<IEnumerable<SubCategoria>> ObtenerPorCategoria(Guid idCategoria);
    }
}
