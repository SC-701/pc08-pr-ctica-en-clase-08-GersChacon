using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ProductoBase
    {
        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(100, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres", MinimumLength = 3)]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción del producto es requerida")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio del producto es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [DisplayName("Precio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock del producto es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        [DisplayName("Stock")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El código de barras es requerido")]
        [StringLength(13, MinimumLength = 8, ErrorMessage = "El código de barras debe tener entre 8 y 13 caracteres")]
        [RegularExpression(@"^[0-9]{8,13}$", ErrorMessage = "El código de barras debe contener solo números")]
        [DisplayName("Código de Barras")]
        public string CodigoBarras { get; set; }
    }

    public class ProductoRequest : ProductoBase
    {
        [Required(ErrorMessage = "La subcategoría es requerida")]
        public Guid IdSubCategoria { get; set; }
    }

    public class ProductoResponse : ProductoBase
    {
        public Guid Id { get; set; }
        public string? SubCategoria { get; set; }
        public string? Categoria { get; set; }
    }

    public class TokenItem
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
    }

    public class SubCategoriaItem
    {
        public Guid Id { get; set; }
        public Guid IdCategoria { get; set; }
        public string Nombre { get; set; }
    }
}
