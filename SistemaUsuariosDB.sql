-- Base de datos del Sistema de Usuarios
-- Ejecutar este script primero y despues Grupo1_CatalogoStockAuditoria.sql

IF DB_ID('SistemaUsuariosDB') IS NULL
    CREATE DATABASE SistemaUsuariosDB;
GO

USE SistemaUsuariosDB;
GO

-- ============================================================
-- TABLAS
-- ============================================================

-- Datos para la API de direcciones (georef)
CREATE TABLE Provincias (
    IdProvincia VARCHAR(10) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Partidos (
    IdPartido VARCHAR(20) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    IdProvincia VARCHAR(10) NOT NULL,
    CONSTRAINT FK_Partidos_Provincias FOREIGN KEY (IdProvincia) REFERENCES Provincias(IdProvincia)
);
GO

CREATE TABLE Localidades (
    IdLocalidad VARCHAR(30) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(150) NOT NULL,
    IdPartido VARCHAR(20) NOT NULL,
    CONSTRAINT FK_Localidades_Partidos FOREIGN KEY (IdPartido) REFERENCES Partidos(IdPartido)
);
GO

CREATE TABLE Direcciones (
    IdDireccion INT IDENTITY(1,1) PRIMARY KEY,
    Calle VARCHAR(150) NOT NULL,
    Numero VARCHAR(20) NULL,
    CodigoPostal VARCHAR(20) NULL,
    Depto VARCHAR(20) NULL,
    Piso VARCHAR(20) NULL,
    Provincia VARCHAR(100) NOT NULL,
    Partido VARCHAR(100) NULL,
    Localidad VARCHAR(150) NULL,
    Latitud DECIMAL(10,7) NULL,
    Longitud DECIMAL(10,7) NULL,
    DireccionOriginal VARCHAR(250) NULL,
    DireccionNormalizada VARCHAR(250) NULL
);
GO

-- Personas y usuarios
CREATE TABLE Roles (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL UNIQUE
);
GO

CREATE TABLE Personas (
    IdPersona INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    DNI VARCHAR(20) NOT NULL UNIQUE,
    Telefono VARCHAR(30) NULL,
    FechaNacimiento DATE NULL,
    IdDireccion INT NULL,
    FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Personas_Direcciones FOREIGN KEY (IdDireccion) REFERENCES Direcciones(IdDireccion)
);
GO

CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Usuario VARCHAR(50) NOT NULL UNIQUE,
    IdPersona INT NOT NULL,
    IdRol INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    PrimerLogin BIT NOT NULL DEFAULT 1,
    Usa2FA BIT NOT NULL DEFAULT 0,
    FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
    Correo VARCHAR(100) NOT NULL DEFAULT '',
    CONSTRAINT FK_Usuarios_Personas FOREIGN KEY (IdPersona) REFERENCES Personas(IdPersona),
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (IdRol) REFERENCES Roles(IdRol)
);
GO

-- Seguridad: passwords, historial, recuperacion y preguntas
CREATE TABLE Passwords (
    IdPassword INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    PasswordHash VARCHAR(256) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Passwords_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);
GO

CREATE TABLE HistorialPasswords (
    IdHistorial INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    PasswordHash VARCHAR(256) NOT NULL,
    FechaUso DATETIME NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Historial_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);
GO

CREATE TABLE RecuperacionPassword (
    IdRecuperacion INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    Codigo VARCHAR(100) NULL,
    FechaSolicitud DATETIME NULL DEFAULT GETDATE(),
    Usado BIT NULL DEFAULT 0,
    CONSTRAINT FK_Recuperacion_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);
GO

CREATE TABLE PreguntasSeguridad (
    IdPregunta INT IDENTITY(1,1) PRIMARY KEY,
    Pregunta VARCHAR(255) NOT NULL
);
GO

CREATE TABLE RespuestasUsuario (
    IdRespuesta INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    IdPregunta INT NOT NULL,
    RespuestaHash VARCHAR(256) NOT NULL,
    CONSTRAINT FK_Respuestas_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT FK_Respuestas_Preguntas FOREIGN KEY (IdPregunta) REFERENCES PreguntasSeguridad(IdPregunta)
);
GO

-- Configuracion de las politicas de contraseña (hay una sola fila)
CREATE TABLE PoliticasSeguridad (
    IdPolitica INT IDENTITY(1,1) PRIMARY KEY,
    MinCaracteres INT NULL DEFAULT 6,
    CantPreguntas INT NULL DEFAULT 2,
    RequiereMayusculas BIT NULL DEFAULT 0,
    RequiereMinusculas BIT NULL DEFAULT 0,
    RequiereNumeros BIT NULL DEFAULT 0,
    RequiereEspeciales BIT NULL DEFAULT 0,
    Requiere2FA BIT NULL DEFAULT 0,
    NoRepetirPasswords BIT NULL DEFAULT 1,
    ValidarDatosPersonales BIT NULL DEFAULT 1
);
GO

-- Cuenta de correo que usa el sistema para mandar mails.
-- La contraseña se guarda cifrada con AES (ver Utilidades)
CREATE TABLE ConfiguracionCorreo (
    IdConfiguracionCorreo INT IDENTITY(1,1) PRIMARY KEY,
    CorreoRemitente VARCHAR(100) NOT NULL,
    PasswordCifrada VARBINARY(MAX) NOT NULL,
    IV VARBINARY(16) NOT NULL,
    FechaUltimaRotacion DATETIME NOT NULL DEFAULT GETDATE(),
    DiasRotacion INT NOT NULL DEFAULT 90
);
GO

-- ============================================================
-- PROCEDIMIENTOS: direcciones / georef
-- ============================================================

CREATE OR ALTER PROCEDURE SP_ObtenerProvincias
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdProvincia, Nombre FROM Provincias ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE SP_ObtenerPartidosPorProvincia
    @IdProvincia VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdPartido, Nombre, IdProvincia
    FROM Partidos
    WHERE IdProvincia = @IdProvincia
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE SP_ObtenerLocalidadesPorPartido
    @IdPartido VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdLocalidad, Nombre, IdPartido
    FROM Localidades
    WHERE IdPartido = @IdPartido
    ORDER BY Nombre;
END;
GO

-- Devuelve 1 si ya se cargaron las provincias, 0 si no
CREATE OR ALTER PROCEDURE SP_HayDatosGeograficosCargados
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Provincias)
        SELECT CAST(1 AS BIT);
    ELSE
        SELECT CAST(0 AS BIT);
END;
GO

-- Los 3 siguientes solo insertan si el dato no existe todavia,
-- asi se puede sincronizar con la API varias veces
CREATE OR ALTER PROCEDURE SP_GuardarProvincia
    @IdProvincia VARCHAR(10),
    @Nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM Provincias WHERE IdProvincia = @IdProvincia)
        INSERT INTO Provincias (IdProvincia, Nombre) VALUES (@IdProvincia, @Nombre);
END;
GO

CREATE OR ALTER PROCEDURE SP_GuardarPartido
    @IdPartido VARCHAR(20),
    @Nombre VARCHAR(100),
    @IdProvincia VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM Partidos WHERE IdPartido = @IdPartido)
        INSERT INTO Partidos (IdPartido, Nombre, IdProvincia) VALUES (@IdPartido, @Nombre, @IdProvincia);
END;
GO

CREATE OR ALTER PROCEDURE SP_GuardarLocalidad
    @IdLocalidad VARCHAR(30),
    @Nombre VARCHAR(150),
    @IdPartido VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM Localidades WHERE IdLocalidad = @IdLocalidad)
        INSERT INTO Localidades (IdLocalidad, Nombre, IdPartido) VALUES (@IdLocalidad, @Nombre, @IdPartido);
END;
GO

CREATE OR ALTER PROCEDURE SP_GuardarDireccion
    @Calle VARCHAR(150),
    @Numero VARCHAR(20) = NULL,
    @CodigoPostal VARCHAR(20) = NULL,
    @Depto VARCHAR(20) = NULL,
    @Piso VARCHAR(20) = NULL,
    @Provincia VARCHAR(100),
    @Partido VARCHAR(100) = NULL,
    @Localidad VARCHAR(150) = NULL,
    @Latitud DECIMAL(10,7) = NULL,
    @Longitud DECIMAL(10,7) = NULL,
    @DireccionOriginal VARCHAR(250) = NULL,
    @DireccionNormalizada VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Direcciones (Calle, Numero, CodigoPostal, Depto, Piso, Provincia, Partido, Localidad,
                             Latitud, Longitud, DireccionOriginal, DireccionNormalizada)
    VALUES (@Calle, @Numero, @CodigoPostal, @Depto, @Piso, @Provincia, @Partido, @Localidad,
            @Latitud, @Longitud, @DireccionOriginal, @DireccionNormalizada);

    -- devuelve el id de la direccion recien creada
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdDireccion;
END;
GO

CREATE OR ALTER PROCEDURE SP_ActualizarDireccion
    @IdDireccion INT,
    @Calle VARCHAR(150),
    @Numero VARCHAR(20) = NULL,
    @CodigoPostal VARCHAR(20) = NULL,
    @Depto VARCHAR(20) = NULL,
    @Piso VARCHAR(20) = NULL,
    @Provincia VARCHAR(100),
    @Partido VARCHAR(100) = NULL,
    @Localidad VARCHAR(150) = NULL,
    @Latitud DECIMAL(10,7) = NULL,
    @Longitud DECIMAL(10,7) = NULL,
    @DireccionOriginal VARCHAR(250) = NULL,
    @DireccionNormalizada VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Direcciones
    SET Calle = @Calle,
        Numero = @Numero,
        CodigoPostal = @CodigoPostal,
        Depto = @Depto,
        Piso = @Piso,
        Provincia = @Provincia,
        Partido = @Partido,
        Localidad = @Localidad,
        Latitud = @Latitud,
        Longitud = @Longitud,
        DireccionOriginal = @DireccionOriginal,
        DireccionNormalizada = @DireccionNormalizada
    WHERE IdDireccion = @IdDireccion;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- ============================================================
-- PROCEDIMIENTOS: busquedas de personas
-- ============================================================

CREATE OR ALTER PROCEDURE SP_ExisteDNI
    @DNI VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Personas WHERE DNI = @DNI)
        SELECT 1 AS Existe;
    ELSE
        SELECT 0 AS Existe;
END;
GO

CREATE OR ALTER PROCEDURE SP_BuscarPersonaPorDNI
    @DNI VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Personas WHERE DNI = @DNI;
END;
GO

CREATE OR ALTER PROCEDURE SP_BuscarPersonaPorApellido
    @Apellido VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Personas WHERE Apellido LIKE '%' + @Apellido + '%';
END;
GO

CREATE OR ALTER PROCEDURE SP_BuscarPersonaPorNombre
    @Nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.IdUsuario, p.IdPersona, d.IdDireccion, r.IdRol, u.Activo,
        p.Nombre, p.Apellido, p.DNI, p.Telefono, p.FechaNacimiento,
        u.Usuario, u.Correo,
        r.NombreRol AS Rol,
        CASE WHEN u.Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado,
        d.Calle, d.Numero, d.CodigoPostal, d.Depto, d.Piso, d.Provincia, d.Partido, d.Localidad
    FROM Usuarios u
    INNER JOIN Personas p ON u.IdPersona = p.IdPersona
    INNER JOIN Direcciones d ON p.IdDireccion = d.IdDireccion
    INNER JOIN Roles r ON u.IdRol = r.IdRol
    WHERE p.Nombre LIKE '%' + @Nombre + '%';
END;
GO

CREATE OR ALTER PROCEDURE SP_BuscarPersonaPorCP
    @CodigoPostal VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT P.*, D.*
    FROM Personas P
    INNER JOIN Direcciones D ON P.IdDireccion = D.IdDireccion
    WHERE D.CodigoPostal = @CodigoPostal;
END;
GO

CREATE OR ALTER PROCEDURE SP_BuscarPersonaPorLocalidad
    @Localidad VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT P.*, D.*
    FROM Personas P
    INNER JOIN Direcciones D ON P.IdDireccion = D.IdDireccion
    WHERE D.Localidad LIKE '%' + @Localidad + '%';
END;
GO

CREATE OR ALTER PROCEDURE SP_BuscarPersonaPorPartido
    @Partido VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT P.*, D.*
    FROM Personas P
    INNER JOIN Direcciones D ON P.IdDireccion = D.IdDireccion
    WHERE D.Partido LIKE '%' + @Partido + '%';
END;
GO

CREATE OR ALTER PROCEDURE SP_BuscarPersonaPorProvincia
    @Provincia VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT P.*, D.*
    FROM Personas P
    INNER JOIN Direcciones D ON P.IdDireccion = D.IdDireccion
    WHERE D.Provincia LIKE '%' + @Provincia + '%';
END;
GO

-- ============================================================
-- PROCEDIMIENTOS: usuarios y roles
-- ============================================================

CREATE OR ALTER PROCEDURE SP_ListarRoles
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdRol, NombreRol FROM Roles ORDER BY NombreRol;
END;
GO

CREATE OR ALTER PROCEDURE SP_ExisteUsuario
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Usuarios WHERE Usuario = @Usuario)
        SELECT 1 AS Existe;
    ELSE
        SELECT 0 AS Existe;
END;
GO

CREATE OR ALTER PROCEDURE SP_ExisteCorreo
    @Correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Usuarios WHERE Correo = @Correo)
        SELECT 1 AS Existe;
    ELSE
        SELECT 0 AS Existe;
END;
GO

CREATE OR ALTER PROCEDURE SP_ObtenerCorreoPorUsuario
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Correo FROM Usuarios WHERE Usuario = @Usuario;
END;
GO

CREATE OR ALTER PROCEDURE SP_ObtenerRolPorUsuario
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdRol FROM Usuarios WHERE Usuario = @Usuario;
END;
GO

-- Es igual al de arriba pero con el nombre que usa el C# (CD_usuariosDatos)
CREATE OR ALTER PROCEDURE SP_ObtenerRolUsuario
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdRol FROM Usuarios WHERE Usuario = @Usuario;
END;
GO

CREATE OR ALTER PROCEDURE SP_BuscarUsuarioPorId
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario, U.Usuario, U.Activo, U.PrimerLogin, U.Usa2FA,
        U.FechaAlta AS FechaAltaUsuario, U.Correo,
        R.IdRol, R.NombreRol,
        P.IdPersona, P.Nombre, P.Apellido, P.DNI, P.Telefono, P.FechaNacimiento,
        D.IdDireccion, D.Calle, D.Numero, D.CodigoPostal, D.Depto, D.Piso,
        D.Provincia, D.Partido, D.Localidad, D.Latitud, D.Longitud,
        D.DireccionOriginal, D.DireccionNormalizada
    FROM Usuarios U
    INNER JOIN Personas P ON U.IdPersona = P.IdPersona
    INNER JOIN Roles R ON U.IdRol = R.IdRol
    LEFT JOIN Direcciones D ON P.IdDireccion = D.IdDireccion
    WHERE U.IdUsuario = @IdUsuario;
END;
GO

CREATE OR ALTER PROCEDURE SP_BuscarUsuarioPorNombreUser
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario, U.Usuario, U.Activo, U.PrimerLogin, U.Usa2FA,
        U.FechaAlta AS FechaAltaUsuario, U.Correo,
        R.IdRol, R.NombreRol,
        P.IdPersona, P.Nombre, P.Apellido, P.DNI, P.Telefono, P.FechaNacimiento,
        D.IdDireccion, D.Calle, D.Numero, D.CodigoPostal, D.Depto, D.Piso,
        D.Provincia, D.Partido, D.Localidad, D.Latitud, D.Longitud,
        D.DireccionOriginal, D.DireccionNormalizada
    FROM Usuarios U
    INNER JOIN Personas P ON U.IdPersona = P.IdPersona
    INNER JOIN Roles R ON U.IdRol = R.IdRol
    LEFT JOIN Direcciones D ON P.IdDireccion = D.IdDireccion
    WHERE U.Usuario = @Usuario;
END;
GO

CREATE OR ALTER PROCEDURE SP_BuscarUsuarioPorCorreo
    @Correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Usuarios WHERE Correo LIKE '%' + @Correo + '%';
END;
GO

-- Lista de usuarios para la grilla del admin
CREATE OR ALTER PROCEDURE SP_ObtenerUsuariosGrilla
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario, U.IdPersona, P.IdDireccion,
        P.Nombre, P.Apellido, P.DNI, P.Telefono, P.FechaNacimiento,
        U.Usuario, U.Correo, U.IdRol,
        R.NombreRol AS Rol,
        U.Activo,
        CASE WHEN U.Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado,
        D.Calle, D.Numero, D.CodigoPostal, D.Depto, D.Piso, D.Provincia, D.Partido, D.Localidad
    FROM Usuarios U
    INNER JOIN Personas P ON U.IdPersona = P.IdPersona
    LEFT JOIN Direcciones D ON P.IdDireccion = D.IdDireccion
    LEFT JOIN Roles R ON U.IdRol = R.IdRol
    ORDER BY U.IdUsuario;
END;
GO

-- Lo mismo pero con filtros (busqueda de texto, estado y rol).
-- Si un filtro viene en NULL (o '' en la busqueda) no se aplica
CREATE OR ALTER PROCEDURE SP_BuscarUsuariosGrilla
    @Busqueda NVARCHAR(100) = '',
    @Activo BIT = NULL,
    @IdRol INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario, U.IdPersona, P.IdDireccion,
        P.Nombre, P.Apellido, P.DNI, P.Telefono, P.FechaNacimiento,
        U.Usuario, U.Correo, U.IdRol,
        R.NombreRol AS Rol,
        U.Activo,
        CASE WHEN U.Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado,
        D.Calle, D.Numero, D.CodigoPostal, D.Depto, D.Piso, D.Provincia, D.Partido, D.Localidad
    FROM Usuarios U
    INNER JOIN Personas P ON U.IdPersona = P.IdPersona
    LEFT JOIN Direcciones D ON P.IdDireccion = D.IdDireccion
    LEFT JOIN Roles R ON U.IdRol = R.IdRol
    WHERE
        (@Busqueda = ''
         OR P.Nombre LIKE '%' + @Busqueda + '%'
         OR P.Apellido LIKE '%' + @Busqueda + '%'
         OR P.DNI LIKE '%' + @Busqueda + '%'
         OR P.Telefono LIKE '%' + @Busqueda + '%'
         OR U.Usuario LIKE '%' + @Busqueda + '%'
         OR U.Correo LIKE '%' + @Busqueda + '%'
         OR R.NombreRol LIKE '%' + @Busqueda + '%'
         OR D.Calle LIKE '%' + @Busqueda + '%'
         OR D.Numero LIKE '%' + @Busqueda + '%'
         OR D.CodigoPostal LIKE '%' + @Busqueda + '%'
         OR D.Depto LIKE '%' + @Busqueda + '%'
         OR D.Piso LIKE '%' + @Busqueda + '%'
         OR D.Provincia LIKE '%' + @Busqueda + '%'
         OR D.Partido LIKE '%' + @Busqueda + '%'
         OR D.Localidad LIKE '%' + @Busqueda + '%')
        AND (@Activo IS NULL OR U.Activo = @Activo)
        AND (@IdRol IS NULL OR U.IdRol = @IdRol)
    ORDER BY U.IdUsuario;
END;
GO

-- Da de alta o baja a un usuario. Devuelve 1 si modifico algo, 0 si no
CREATE OR ALTER PROCEDURE SP_ActualizarEstadoUsuario
    @IdUsuario INT,
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios SET Activo = @Activo WHERE IdUsuario = @IdUsuario;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END AS Resultado;
END;
GO

CREATE OR ALTER PROCEDURE SP_AsignarRolUsuario
    @IdUsuario INT,
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Roles WHERE IdRol = @IdRol)
    BEGIN
        RAISERROR('El rol indicado no existe.', 16, 1);
        RETURN;
    END;

    UPDATE Usuarios SET IdRol = @IdRol WHERE IdUsuario = @IdUsuario;

    SELECT 'Rol asignado correctamente.' AS Mensaje;
END;
GO

-- Modifica persona, direccion y usuario en una sola transaccion
CREATE OR ALTER PROCEDURE SP_ActualizarDatosUsuario
    @IdUsuario INT,

    @Nombre VARCHAR(100),
    @Apellido VARCHAR(100),
    @DNI VARCHAR(20),
    @Telefono VARCHAR(30),
    @FechaNacimiento DATE,

    @Calle VARCHAR(100),
    @Numero VARCHAR(20),
    @CodigoPostal VARCHAR(20),
    @Depto VARCHAR(20) = NULL,
    @Piso VARCHAR(20) = NULL,
    @Provincia VARCHAR(100),
    @Partido VARCHAR(100),
    @Localidad VARCHAR(100),
    @Latitud DECIMAL(18, 10) = NULL,
    @Longitud DECIMAL(18, 10) = NULL,
    @DireccionOriginal VARCHAR(255),
    @DireccionNormalizada VARCHAR(255),

    @Correo VARCHAR(100),
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdPersona INT;
    DECLARE @IdDireccion INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @IdPersona = IdPersona FROM Usuarios WHERE IdUsuario = @IdUsuario;

        IF @IdPersona IS NULL
        BEGIN
            RAISERROR('El usuario no existe.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        SELECT @IdDireccion = IdDireccion FROM Personas WHERE IdPersona = @IdPersona;

        UPDATE Personas
        SET Nombre = @Nombre,
            Apellido = @Apellido,
            DNI = @DNI,
            Telefono = @Telefono,
            FechaNacimiento = @FechaNacimiento
        WHERE IdPersona = @IdPersona;

        UPDATE Direcciones
        SET Calle = @Calle,
            Numero = @Numero,
            CodigoPostal = @CodigoPostal,
            Depto = @Depto,
            Piso = @Piso,
            Provincia = @Provincia,
            Partido = @Partido,
            Localidad = @Localidad,
            Latitud = @Latitud,
            Longitud = @Longitud,
            DireccionOriginal = @DireccionOriginal,
            DireccionNormalizada = @DireccionNormalizada
        WHERE IdDireccion = @IdDireccion;

        UPDATE Usuarios
        SET Correo = @Correo,
            IdRol = @IdRol
        WHERE IdUsuario = @IdUsuario;

        COMMIT TRANSACTION;

        SELECT 1 AS Resultado;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

-- Registra direccion + persona + usuario + password en una sola transaccion.
-- Si algo falla se deshace todo
CREATE OR ALTER PROCEDURE SP_RegistrarUsuarioCompleto
    @Nombre VARCHAR(100),
    @Apellido VARCHAR(100),
    @DNI VARCHAR(20),
    @Telefono VARCHAR(30) = NULL,
    @FechaNacimiento DATE,

    @Calle VARCHAR(100),
    @Numero VARCHAR(20),
    @CodigoPostal VARCHAR(20) = NULL,
    @Depto VARCHAR(20) = NULL,
    @Piso VARCHAR(20) = NULL,
    @Provincia VARCHAR(100),
    @Partido VARCHAR(100),
    @Localidad VARCHAR(100),
    @Latitud DECIMAL(18, 10) = NULL,
    @Longitud DECIMAL(18, 10) = NULL,
    @DireccionOriginal VARCHAR(255) = NULL,
    @DireccionNormalizada VARCHAR(255) = NULL,

    @Usuario VARCHAR(50),
    @Correo VARCHAR(100),
    @PasswordHash VARCHAR(255),
    @IdRol INT,
    @Usa2FA BIT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdDireccion INT;
    DECLARE @IdPersona INT;
    DECLARE @IdUsuario INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1) direccion
        INSERT INTO Direcciones (Calle, Numero, CodigoPostal, Depto, Piso, Provincia, Partido, Localidad,
                                 Latitud, Longitud, DireccionOriginal, DireccionNormalizada)
        VALUES (@Calle, @Numero, @CodigoPostal, @Depto, @Piso, @Provincia, @Partido, @Localidad,
                @Latitud, @Longitud, @DireccionOriginal, @DireccionNormalizada);

        SET @IdDireccion = SCOPE_IDENTITY();

        -- 2) persona
        INSERT INTO Personas (Nombre, Apellido, DNI, Telefono, FechaNacimiento, IdDireccion)
        VALUES (@Nombre, @Apellido, @DNI, @Telefono, @FechaNacimiento, @IdDireccion);

        SET @IdPersona = SCOPE_IDENTITY();

        -- 3) usuario (activo y con primer login pendiente)
        INSERT INTO Usuarios (IdPersona, Usuario, Correo, IdRol, Activo, PrimerLogin, Usa2FA)
        VALUES (@IdPersona, @Usuario, @Correo, @IdRol, 1, 1, @Usa2FA);

        SET @IdUsuario = SCOPE_IDENTITY();

        -- 4) password
        INSERT INTO Passwords (IdUsuario, PasswordHash, FechaCreacion)
        VALUES (@IdUsuario, @PasswordHash, GETDATE());

        COMMIT TRANSACTION;

        SELECT @IdUsuario AS IdUsuario;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_UsuarioActivo
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Activo FROM Usuarios WHERE Usuario = @Usuario;
END;
GO

-- Se usa para validar que la contraseña nueva no tenga datos personales
CREATE OR ALTER PROCEDURE SP_ObtenerDatosPersonalesPorUsuario
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT P.Nombre, P.Apellido, P.DNI, P.FechaNacimiento
    FROM Usuarios U
    INNER JOIN Personas P ON U.IdPersona = P.IdPersona
    WHERE U.Usuario = @Usuario;
END;
GO

-- ============================================================
-- PROCEDIMIENTOS: login y contraseñas
-- ============================================================

-- Devuelve cuantos usuarios coinciden (1 = login correcto, 0 = incorrecto).
-- Solo vale la ultima contraseña del usuario y tiene que estar activo
CREATE OR ALTER PROCEDURE SP_VerificarLogin
    @Usuario VARCHAR(50),
    @PasswordHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*)
    FROM Usuarios U
    INNER JOIN Passwords P ON U.IdUsuario = P.IdUsuario
    WHERE U.Usuario = @Usuario
      AND U.Activo = 1
      AND P.PasswordHash = @PasswordHash
      AND P.FechaCreacion = (SELECT MAX(P2.FechaCreacion)
                             FROM Passwords P2
                             WHERE P2.IdUsuario = U.IdUsuario);
END;
GO

CREATE OR ALTER PROCEDURE SP_EsPrimerLogin
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PrimerLogin FROM Usuarios WHERE Usuario = @Usuario;
END;
GO

CREATE OR ALTER PROCEDURE SP_CambiarPassword
    @IdUsuario INT,
    @NuevaPasswordHash VARCHAR(256),
    @ValidarHistorial BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE IdUsuario = @IdUsuario)
        BEGIN
            RAISERROR('El usuario no existe.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        -- no deja repetir una contraseña que ya uso
        IF @ValidarHistorial = 1 AND EXISTS (SELECT 1 FROM HistorialPasswords
                                             WHERE IdUsuario = @IdUsuario
                                               AND PasswordHash = @NuevaPasswordHash)
        BEGIN
            RAISERROR('La contraseña ya fue utilizada anteriormente.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        INSERT INTO Passwords (IdUsuario, PasswordHash, FechaCreacion)
        VALUES (@IdUsuario, @NuevaPasswordHash, GETDATE());

        INSERT INTO HistorialPasswords (IdUsuario, PasswordHash, FechaUso)
        VALUES (@IdUsuario, @NuevaPasswordHash, GETDATE());

        UPDATE Usuarios SET PrimerLogin = 0 WHERE IdUsuario = @IdUsuario;

        COMMIT TRANSACTION;

        SELECT 'Contraseña actualizada correctamente.' AS Mensaje;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT ERROR_MESSAGE() AS Error;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_VerificarPasswordEnHistorial
    @IdUsuario INT,
    @PasswordHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM HistorialPasswords
               WHERE IdUsuario = @IdUsuario AND PasswordHash = @PasswordHash)
        SELECT 1 AS Existe;
    ELSE
        SELECT 0 AS Existe;
END;
GO

CREATE OR ALTER PROCEDURE SP_InsertarPassword
    @IdUsuario INT,
    @PasswordHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Passwords (IdUsuario, PasswordHash, FechaCreacion)
    VALUES (@IdUsuario, @PasswordHash, GETDATE());
END;
GO

CREATE OR ALTER PROCEDURE SP_ObtenerContrasAnteriores
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT PasswordHash
    FROM Passwords
    WHERE IdUsuario = @IdUsuario
    ORDER BY FechaCreacion DESC;
END;
GO

CREATE OR ALTER PROCEDURE SP_ObtenerUltimoPasswordHash
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 PasswordHash
    FROM Passwords
    WHERE IdUsuario = @IdUsuario
    ORDER BY FechaCreacion DESC;
END;
GO

-- Guarda la password nueva y marca que ya hizo el primer login
CREATE OR ALTER PROCEDURE SP_ActualizarPasswordYPrimerLogin
    @IdUsuario INT,
    @PasswordHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Passwords (IdUsuario, PasswordHash, FechaCreacion)
        VALUES (@IdUsuario, @PasswordHash, GETDATE());

        UPDATE Usuarios SET PrimerLogin = 0 WHERE IdUsuario = @IdUsuario;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

-- ============================================================
-- PROCEDIMIENTOS: preguntas de seguridad
-- ============================================================

CREATE OR ALTER PROCEDURE SP_ListarPreguntasSeguridad
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdPregunta, Pregunta FROM PreguntasSeguridad ORDER BY Pregunta;
END;
GO

-- Si el usuario ya respondio esa pregunta actualiza la respuesta, si no la inserta
CREATE OR ALTER PROCEDURE SP_GuardarRespuestasSeguridad
    @IdUsuario INT,
    @IdPregunta INT,
    @RespuestaHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM PreguntasSeguridad WHERE IdPregunta = @IdPregunta)
        BEGIN
            RAISERROR('La pregunta de seguridad no existe.', 16, 1);
            RETURN;
        END;

        IF EXISTS (SELECT 1 FROM RespuestasUsuario
                   WHERE IdUsuario = @IdUsuario AND IdPregunta = @IdPregunta)
        BEGIN
            UPDATE RespuestasUsuario
            SET RespuestaHash = @RespuestaHash
            WHERE IdUsuario = @IdUsuario AND IdPregunta = @IdPregunta;

            SELECT 'Respuesta de seguridad actualizada correctamente.' AS Mensaje;
        END
        ELSE
        BEGIN
            INSERT INTO RespuestasUsuario (IdUsuario, IdPregunta, RespuestaHash)
            VALUES (@IdUsuario, @IdPregunta, @RespuestaHash);

            SELECT 'Respuesta de seguridad guardada correctamente.' AS Mensaje;
        END
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS Error;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_ObtenerPreguntasUsuario
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.IdPregunta, p.Pregunta, r.RespuestaHash
    FROM RespuestasUsuario r
    INNER JOIN PreguntasSeguridad p ON r.IdPregunta = p.IdPregunta
    WHERE r.IdUsuario = @IdUsuario;
END;
GO

CREATE OR ALTER PROCEDURE SP_VerificarRespuestaSeguridad
    @IdUsuario INT,
    @IdPregunta INT,
    @RespuestaHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM RespuestasUsuario
               WHERE IdUsuario = @IdUsuario
                 AND IdPregunta = @IdPregunta
                 AND RespuestaHash = @RespuestaHash)
        SELECT 1 AS Correcta, 'La respuesta es correcta.' AS Mensaje;
    ELSE
        SELECT 0 AS Correcta, 'La respuesta es incorrecta.' AS Mensaje;
END;
GO

-- ============================================================
-- PROCEDIMIENTOS: politicas de seguridad
-- ============================================================

CREATE OR ALTER PROCEDURE SP_ObtenerPoliticasSeguridad
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 * FROM PoliticasSeguridad;
END;
GO

CREATE OR ALTER PROCEDURE SP_ObtenerConfiguracion
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        IdPolitica, MinCaracteres, CantPreguntas,
        RequiereMayusculas, RequiereMinusculas, RequiereNumeros, RequiereEspeciales,
        Requiere2FA, NoRepetirPasswords, ValidarDatosPersonales
    FROM PoliticasSeguridad
    ORDER BY IdPolitica;
END;
GO

-- Si todavia no hay ninguna fila la crea, si ya hay la actualiza.
-- @RequiereMinusculas tiene valor por defecto para no romper llamadas viejas
CREATE OR ALTER PROCEDURE SP_ActualizarPoliticasSeguridad
    @MinCaracteres INT,
    @CantPreguntas INT,
    @RequiereMayusculas BIT,
    @RequiereNumeros BIT,
    @RequiereEspeciales BIT,
    @Requiere2FA BIT,
    @NoRepetirPasswords BIT,
    @ValidarDatosPersonales BIT,
    @RequiereMinusculas BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM PoliticasSeguridad)
        BEGIN
            INSERT INTO PoliticasSeguridad (MinCaracteres, CantPreguntas, RequiereMayusculas, RequiereMinusculas,
                                            RequiereNumeros, RequiereEspeciales, Requiere2FA,
                                            NoRepetirPasswords, ValidarDatosPersonales)
            VALUES (@MinCaracteres, @CantPreguntas, @RequiereMayusculas, @RequiereMinusculas,
                    @RequiereNumeros, @RequiereEspeciales, @Requiere2FA,
                    @NoRepetirPasswords, @ValidarDatosPersonales);

            SELECT 'Políticas de seguridad creadas correctamente.' AS Mensaje;
        END
        ELSE
        BEGIN
            UPDATE PoliticasSeguridad
            SET MinCaracteres = @MinCaracteres,
                CantPreguntas = @CantPreguntas,
                RequiereMayusculas = @RequiereMayusculas,
                RequiereMinusculas = @RequiereMinusculas,
                RequiereNumeros = @RequiereNumeros,
                RequiereEspeciales = @RequiereEspeciales,
                Requiere2FA = @Requiere2FA,
                NoRepetirPasswords = @NoRepetirPasswords,
                ValidarDatosPersonales = @ValidarDatosPersonales;

            SELECT 'Políticas de seguridad actualizadas correctamente.' AS Mensaje;
        END
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS Error;
    END CATCH
END;
GO

-- ============================================================
-- PROCEDIMIENTOS: configuracion del correo
-- ============================================================

CREATE OR ALTER PROCEDURE SP_ObtenerConfiguracionCorreo
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        IdConfiguracionCorreo, CorreoRemitente, PasswordCifrada, IV,
        FechaUltimaRotacion, DiasRotacion
    FROM ConfiguracionCorreo
    ORDER BY IdConfiguracionCorreo;
END;
GO

-- Si @PasswordCifrada viene en NULL no se toca la contraseña,
-- solo se actualizan el correo y los dias de rotacion
CREATE OR ALTER PROCEDURE SP_GuardarConfiguracionCorreo
    @CorreoRemitente VARCHAR(100),
    @DiasRotacion INT,
    @PasswordCifrada VARBINARY(MAX) = NULL,
    @IV VARBINARY(16) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM ConfiguracionCorreo)
    BEGIN
        IF @PasswordCifrada IS NULL
        BEGIN
            RAISERROR('Debe cargar una contraseña la primera vez.', 16, 1);
            RETURN;
        END

        INSERT INTO ConfiguracionCorreo (CorreoRemitente, PasswordCifrada, IV, FechaUltimaRotacion, DiasRotacion)
        VALUES (@CorreoRemitente, @PasswordCifrada, @IV, GETDATE(), @DiasRotacion);
    END
    ELSE IF @PasswordCifrada IS NOT NULL
    BEGIN
        UPDATE ConfiguracionCorreo
        SET CorreoRemitente = @CorreoRemitente,
            PasswordCifrada = @PasswordCifrada,
            IV = @IV,
            DiasRotacion = @DiasRotacion,
            FechaUltimaRotacion = GETDATE();
    END
    ELSE
    BEGIN
        UPDATE ConfiguracionCorreo
        SET CorreoRemitente = @CorreoRemitente,
            DiasRotacion = @DiasRotacion;
    END

    SELECT 'Configuración de correo guardada correctamente.' AS Mensaje;
END;
GO

-- ============================================================
-- DATOS INICIALES
-- Sin esto no se puede registrar ningun usuario
-- ============================================================

-- OJO con el orden: frmRegistro.cs usa Usuario = 1 y Administrador = 2
INSERT INTO Roles (NombreRol) VALUES ('Usuario');
INSERT INTO Roles (NombreRol) VALUES ('Administrador');
GO

INSERT INTO PreguntasSeguridad (Pregunta) VALUES
('¿Cuál es el nombre de tu primera mascota?'),
('¿Cuál es el nombre de soltera de tu madre?'),
('¿En qué ciudad naciste?'),
('¿Cuál fue el nombre de tu primer colegio?'),
('¿Cuál es tu comida favorita?'),
('¿Cuál es el nombre de tu mejor amigo de la infancia?'),
('¿Cuál fue la marca de tu primer auto?'),
('¿Cuál es tu película favorita?'),
('¿En qué calle vivías cuando eras chico?'),
('¿Cuál es el segundo nombre de tu padre?'),
('¿Cuál fue tu primer trabajo?'),
('¿Cuál es el nombre de tu profesor favorito?'),
('¿Cuál es tu equipo deportivo favorito?'),
('¿Cuál es el nombre de tu abuela materna?'),
('¿Cuál fue el destino de tu primer viaje?');
GO

-- fila de politicas con los valores por defecto
INSERT INTO PoliticasSeguridad DEFAULT VALUES;
GO
