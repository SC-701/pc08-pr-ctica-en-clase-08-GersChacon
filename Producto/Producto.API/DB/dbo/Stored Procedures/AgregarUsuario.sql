CREATE PROCEDURE [dbo].[AgregarUsuario]
@NombreUsuario VARCHAR(MAX),
@PasswordHash VARCHAR(MAX),
@CorreoElectronico VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Id AS UniqueIdentifier =NEWID();
	BEGIN TRAN
		INSERT INTO [dbo].[Usuarios]
			   ([Id]
			   ,[NombreUsuario]
			   ,[PasswordHash]
			   ,[CorreoElectronico])
		 VALUES
			   (@Id
			   ,@NombreUsuario
			   ,@PasswordHash
			   ,@CorreoElectronico)

		INSERT INTO [dbo].[PerfilesxUsuario]
           ([IdUsuario]
           ,[IdPerfil])
		VALUES
           (@Id
           ,1)
	SELECT @Id
	COMMIT TRAN
END
