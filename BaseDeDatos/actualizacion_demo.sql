-- Script de actualizacion para SistemaUsuariosDB
-- Crea los SPs que faltaban para cargar datos de Georef y ajusta
-- el cambio de contraseña para respetar la politica "permite repetir contraseñas".
-- Se puede ejecutar varias veces sin problema.

USE SistemaUsuariosDB;
GO

CREATE OR ALTER PROCEDURE dbo.SP_GuardarProvincia
    @IdProvincia VARCHAR(10),
    @Nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Provincias WHERE IdProvincia = @IdProvincia)
        UPDATE dbo.Provincias SET Nombre = @Nombre WHERE IdProvincia = @IdProvincia;
    ELSE
        INSERT INTO dbo.Provincias (IdProvincia, Nombre) VALUES (@IdProvincia, @Nombre);
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_GuardarPartido
    @IdPartido VARCHAR(20),
    @Nombre VARCHAR(100),
    @IdProvincia VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Partidos WHERE IdPartido = @IdPartido)
        UPDATE dbo.Partidos SET Nombre = @Nombre, IdProvincia = @IdProvincia WHERE IdPartido = @IdPartido;
    ELSE
        INSERT INTO dbo.Partidos (IdPartido, Nombre, IdProvincia) VALUES (@IdPartido, @Nombre, @IdProvincia);
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_GuardarLocalidad
    @IdLocalidad VARCHAR(30),
    @Nombre VARCHAR(150),
    @IdPartido VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Localidades WHERE IdLocalidad = @IdLocalidad)
        UPDATE dbo.Localidades SET Nombre = @Nombre, IdPartido = @IdPartido WHERE IdLocalidad = @IdLocalidad;
    ELSE
        INSERT INTO dbo.Localidades (IdLocalidad, Nombre, IdPartido) VALUES (@IdLocalidad, @Nombre, @IdPartido);
END;
GO

-- Orden fijo para que las preguntas siempre salgan en el mismo orden
CREATE OR ALTER PROCEDURE dbo.SP_ObtenerPreguntasUsuario
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.IdPregunta,
        p.Pregunta,
        r.RespuestaHash
    FROM dbo.RespuestasUsuario r
    INNER JOIN dbo.PreguntasSeguridad p
        ON r.IdPregunta = p.IdPregunta
    WHERE r.IdUsuario = @IdUsuario
    ORDER BY p.IdPregunta;
END;
GO

-- El control de "no repetir contraseñas" ahora lo hace la capa logica
-- segun la configuracion del administrador.
CREATE OR ALTER PROCEDURE dbo.SP_CambiarPassword
    @IdUsuario INT,
    @NuevaPasswordHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE IdUsuario = @IdUsuario)
        BEGIN
            RAISERROR('El usuario no existe.', 16, 1);
        END;

        INSERT INTO dbo.Passwords (IdUsuario, PasswordHash, FechaCreacion)
        VALUES (@IdUsuario, @NuevaPasswordHash, GETDATE());

        INSERT INTO dbo.HistorialPasswords (IdUsuario, PasswordHash, FechaUso)
        VALUES (@IdUsuario, @NuevaPasswordHash, GETDATE());

        UPDATE dbo.Usuarios
        SET PrimerLogin = 0
        WHERE IdUsuario = @IdUsuario;

        COMMIT TRANSACTION;

        SELECT 'Password actualizada correctamente.' AS Mensaje;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT ERROR_MESSAGE() AS Error;
    END CATCH
END;
GO
