CREATE PROCEDURE [dbo].[ObtenerPerfilesxUsuario]
@NombreUsuario VARCHAR(MAX),
@CorreoElectronico VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

SELECT        Perfiles.Id, Perfiles.Nombre
FROM            Perfiles INNER JOIN
                         PerfilesxUsuario ON Perfiles.Id = PerfilesxUsuario.IdPerfil INNER JOIN
                         Usuarios ON PerfilesxUsuario.IdUsuario = Usuarios.Id
WHERE        (Usuarios.NombreUsuario = @Nombreusuario) OR (Usuarios.CorreoElectronico = @CorreoElectronico)

END
