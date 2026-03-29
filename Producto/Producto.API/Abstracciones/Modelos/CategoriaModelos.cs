namespace Abstracciones.Modelos
{
    public class Categoria
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
    }

    public class SubCategoria
    {
        public Guid Id { get; set; }
        public Guid IdCategoria { get; set; }
        public string Nombre { get; set; }
    }
}
