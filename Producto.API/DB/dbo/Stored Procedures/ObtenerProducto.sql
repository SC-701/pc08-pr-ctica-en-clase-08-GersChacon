CREATE PROCEDURE ObtenerProducto
	-- Add the parameters for the stored procedure here
	@Id uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT Producto.Id, Producto.IdSubCategoria, Producto.Nombre, Producto.Precio, Producto.Descripcion, Producto.Stock, Producto.CodigoBarras, SubCategorias.Nombre AS SubCategoria, Categorias.Nombre AS Categoria
FROM     Categorias INNER JOIN
                  SubCategorias ON Categorias.Id = SubCategorias.IdCategoria INNER JOIN
                  Producto ON SubCategorias.Id = Producto.IdSubCategoria
WHERE  (Producto.Id = @Id)
END