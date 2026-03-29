 
CREATE PROCEDURE ObtenerCategoria
    @Id AS UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre
    FROM Categorias
    WHERE Id = @Id
END