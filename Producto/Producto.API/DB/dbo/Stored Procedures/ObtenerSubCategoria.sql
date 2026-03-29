 
CREATE PROCEDURE ObtenerSubCategoria
    @Id AS UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT sc.Id, sc.IdCategoria, sc.Nombre, c.Nombre AS NombreCategoria
    FROM SubCategorias sc
    INNER JOIN Categorias c ON c.Id = sc.IdCategoria
    WHERE sc.Id = @Id
END