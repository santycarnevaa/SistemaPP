-- base de datos completa con lo de gonzales y ferrando

IF DB_ID('SistemaUsuariosDB') IS NULL -- lo agregue para q nose me haga tanto quilombo armar la base de datos
    CREATE DATABASE SistemaUsuariosDB;
GO

USE SistemaUsuariosDB;
GO



-- los campos para la api
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
    IdLocalidad VARCHAR(30) NOT NULL,
    Latitud DECIMAL(10,7) NULL,
    Longitud DECIMAL(10,7) NULL,
    DireccionOriginal VARCHAR(250) NULL,
    DireccionNormalizada VARCHAR(250) NULL,
    CONSTRAINT FK_Direcciones_Localidades FOREIGN KEY (IdLocalidad) REFERENCES Localidades(IdLocalidad)
);
GO

-- usuarios y personas
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

-- usuario y cosas con hash
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
    CONSTRAINT FK_Respuestas_Preguntas FOREIGN KEY (IdPregunta) REFERENCES PreguntasSeguridad(IdPregunta),
    -- un usuario no puede responder dos veces la misma pregunta
    CONSTRAINT UQ_Respuestas_UsuarioPregunta UNIQUE (IdUsuario, IdPregunta)
);
GO

-- configuracio para las contraseñas
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

-- el correo para los gmails de contraseñas
CREATE TABLE ConfiguracionCorreo (
    IdConfiguracionCorreo INT IDENTITY(1,1) PRIMARY KEY,
    CorreoRemitente VARCHAR(100) NOT NULL,
    PasswordCifrada VARBINARY(MAX) NOT NULL,
    IV VARBINARY(16) NOT NULL,
    FechaUltimaRotacion DATETIME NOT NULL DEFAULT GETDATE(),
    DiasRotacion INT NOT NULL DEFAULT 90
);
GO

-- tablas para la gestion de gonzales

CREATE TABLE Categorias (
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Marcas (
    IdMarca INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Productos (
    ID_Codigo INT IDENTITY(1,1) PRIMARY KEY,
    FK_Categoria INT NOT NULL,
    FK_Marca INT NOT NULL,
    Nombre VARCHAR(150) NOT NULL,
    Precio_Compra DECIMAL(18,2) NOT NULL,
    Precio_Venta DECIMAL(18,2) NOT NULL,
    Stock_Actual INT NOT NULL DEFAULT 0,
    Stock_Critico INT NOT NULL,
    Stock_Minimo INT NOT NULL,
    Stock_Ideal INT NOT NULL,
    Stock_Maximo INT NOT NULL,
    Punto_Reposicion INT NOT NULL,
    Numero_Lote VARCHAR(50) NULL,
    Fecha_Vencimiento DATE NULL,
    Estado VARCHAR(30) NOT NULL DEFAULT 'Activo',
    Habilitado BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Productos_Categoria FOREIGN KEY (FK_Categoria) REFERENCES Categorias(IdCategoria),
    CONSTRAINT FK_Productos_Marca FOREIGN KEY (FK_Marca) REFERENCES Marcas(IdMarca),
    CONSTRAINT CK_Productos_StockActual CHECK (Stock_Actual >= 0),
    CONSTRAINT CK_Productos_OrdenStock CHECK (Stock_Critico < Stock_Minimo AND Stock_Minimo < Stock_Ideal AND Stock_Ideal < Stock_Maximo),
    CONSTRAINT CK_Productos_Valores CHECK (Precio_Compra > 0 AND Precio_Venta > 0
                                           AND Stock_Critico >= 0
                                           AND Punto_Reposicion >= 0 AND Punto_Reposicion <= Stock_Maximo)
);
GO

CREATE TABLE Movimientos_Stock (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FK_Producto INT NOT NULL,
    FK_Usuario INT NOT NULL,
    Tipo_Movimiento VARCHAR(20) NOT NULL,
    Cantidad INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Tipo_Documento_Origen VARCHAR(50) NULL,
    ID_Documento_Origen INT NULL,
    CONSTRAINT FK_MovStock_Producto FOREIGN KEY (FK_Producto) REFERENCES Productos(ID_Codigo),
    CONSTRAINT FK_MovStock_Usuario FOREIGN KEY (FK_Usuario) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT CK_MovStock_Tipo CHECK (Tipo_Movimiento IN ('Ingreso', 'Egreso', 'Ajuste')),
    CONSTRAINT CK_MovStock_Cantidad CHECK ((Tipo_Movimiento = 'Ajuste' AND Cantidad <> 0)
                                           OR (Tipo_Movimiento <> 'Ajuste' AND Cantidad > 0))
);
GO

CREATE TABLE Registros_Scrap (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FK_Producto INT NOT NULL,
    FK_Usuario INT NOT NULL,
    Cantidad INT NOT NULL,
    Motivo VARCHAR(200) NOT NULL,
    Costo_Valorizado DECIMAL(18,2) NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Scrap_Producto FOREIGN KEY (FK_Producto) REFERENCES Productos(ID_Codigo),
    CONSTRAINT FK_Scrap_Usuario FOREIGN KEY (FK_Usuario) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT CK_Scrap_Cantidad CHECK (Cantidad > 0)
);
GO

CREATE TABLE Auditoria_Cambios (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Tabla_Afectada VARCHAR(100) NOT NULL,
    ID_Registro_Afectado INT NOT NULL,
    Accion VARCHAR(20) NOT NULL,
    FK_Usuario INT NOT NULL,
    Fecha_Hora DATETIME NOT NULL DEFAULT GETDATE(),
    Detalle_Cambio VARCHAR(MAX) NULL,
    CONSTRAINT FK_Auditoria_Usuario FOREIGN KEY (FK_Usuario) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT CK_Auditoria_Accion CHECK (Accion IN ('Alta', 'Modificación', 'Baja'))
);
GO

-- sp pra la api y direcciones

CREATE PROCEDURE SP_ObtenerProvincias
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdProvincia, Nombre FROM Provincias ORDER BY Nombre;
END;
GO

CREATE PROCEDURE SP_ObtenerPartidosPorProvincia
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

CREATE PROCEDURE SP_ObtenerLocalidadesPorPartido
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

-- Devuelve 1 si ya se cargaron las provincias 
CREATE PROCEDURE SP_HayDatosGeograficosCargados
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Provincias)
        SELECT CAST(1 AS BIT);
    ELSE
        SELECT CAST(0 AS BIT);
END;
GO

-- los 3 siguientes solo insertan si el dato no existe todavia,
-- es para q la api no se rompa y siempre cargue ls cosas
CREATE PROCEDURE SP_GuardarProvincia
    @IdProvincia VARCHAR(10),
    @Nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM Provincias WHERE IdProvincia = @IdProvincia)
        INSERT INTO Provincias (IdProvincia, Nombre) VALUES (@IdProvincia, @Nombre);
END;
GO

CREATE PROCEDURE SP_GuardarPartido
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

CREATE PROCEDURE SP_GuardarLocalidad
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

-- esto es para q no puedan poner una localidad q no exista en el partido
CREATE VIEW VW_Direcciones
AS
SELECT
    d.IdDireccion, d.Calle, d.Numero, d.CodigoPostal, d.Depto, d.Piso,
    d.IdLocalidad, l.Nombre AS Localidad,
    pa.IdPartido, pa.Nombre AS Partido,
    pr.IdProvincia, pr.Nombre AS Provincia,
    d.Latitud, d.Longitud, d.DireccionOriginal, d.DireccionNormalizada
FROM Direcciones d
INNER JOIN Localidades l ON d.IdLocalidad = l.IdLocalidad
INNER JOIN Partidos pa ON l.IdPartido = pa.IdPartido
INNER JOIN Provincias pr ON pa.IdProvincia = pr.IdProvincia;
GO

CREATE PROCEDURE SP_GuardarDireccion
    @Calle VARCHAR(150),
    @Numero VARCHAR(20) = NULL,
    @CodigoPostal VARCHAR(20) = NULL,
    @Depto VARCHAR(20) = NULL,
    @Piso VARCHAR(20) = NULL,
    @IdLocalidad VARCHAR(30),
    @Latitud DECIMAL(10,7) = NULL,
    @Longitud DECIMAL(10,7) = NULL,
    @DireccionOriginal VARCHAR(250) = NULL,
    @DireccionNormalizada VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Direcciones (Calle, Numero, CodigoPostal, Depto, Piso, IdLocalidad,
                             Latitud, Longitud, DireccionOriginal, DireccionNormalizada)
    VALUES (@Calle, @Numero, @CodigoPostal, @Depto, @Piso, @IdLocalidad,
            @Latitud, @Longitud, @DireccionOriginal, @DireccionNormalizada);

    -- guarda el id de la direccion para q pepi lo pueda usar en el visual
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdDireccion;
END;
GO

CREATE PROCEDURE SP_ActualizarDireccion
    @IdDireccion INT,
    @Calle VARCHAR(150),
    @Numero VARCHAR(20) = NULL,
    @CodigoPostal VARCHAR(20) = NULL,
    @Depto VARCHAR(20) = NULL,
    @Piso VARCHAR(20) = NULL,
    @IdLocalidad VARCHAR(30),
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
        IdLocalidad = @IdLocalidad,
        Latitud = @Latitud,
        Longitud = @Longitud,
        DireccionOriginal = @DireccionOriginal,
        DireccionNormalizada = @DireccionNormalizada
    WHERE IdDireccion = @IdDireccion;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

-- busquedas

CREATE PROCEDURE SP_ExisteDNI
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

CREATE PROCEDURE SP_BuscarPersonaPorDNI
    @DNI VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Personas WHERE DNI = @DNI;
END;
GO

CREATE PROCEDURE SP_BuscarPersonaPorApellido
    @Apellido VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Personas WHERE Apellido LIKE '%' + @Apellido + '%';
END;
GO

CREATE PROCEDURE SP_BuscarPersonaPorNombre
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
    INNER JOIN VW_Direcciones d ON p.IdDireccion = d.IdDireccion
    INNER JOIN Roles r ON u.IdRol = r.IdRol
    WHERE p.Nombre LIKE '%' + @Nombre + '%';
END;
GO

CREATE PROCEDURE SP_BuscarPersonaPorCP
    @CodigoPostal VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT P.*, D.*
    FROM Personas P
    INNER JOIN VW_Direcciones D ON P.IdDireccion = D.IdDireccion
    WHERE D.CodigoPostal = @CodigoPostal;
END;
GO

CREATE PROCEDURE SP_BuscarPersonaPorLocalidad
    @Localidad VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT P.*, D.*
    FROM Personas P
    INNER JOIN VW_Direcciones D ON P.IdDireccion = D.IdDireccion
    WHERE D.Localidad LIKE '%' + @Localidad + '%';
END;
GO

CREATE PROCEDURE SP_BuscarPersonaPorPartido
    @Partido VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT P.*, D.*
    FROM Personas P
    INNER JOIN VW_Direcciones D ON P.IdDireccion = D.IdDireccion
    WHERE D.Partido LIKE '%' + @Partido + '%';
END;
GO

CREATE PROCEDURE SP_BuscarPersonaPorProvincia
    @Provincia VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT P.*, D.*
    FROM Personas P
    INNER JOIN VW_Direcciones D ON P.IdDireccion = D.IdDireccion
    WHERE D.Provincia LIKE '%' + @Provincia + '%';
END;
GO

-- procedures para usuarios y los roles
CREATE PROCEDURE SP_ListarRoles
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdRol, NombreRol FROM Roles ORDER BY NombreRol;
END;
GO

-- agrega un rol nuevo, el nombre no se puede repetir
CREATE PROCEDURE SP_AgregarRol
    @NombreRol VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Roles WHERE NombreRol = @NombreRol)
    BEGIN
        RAISERROR('Ya existe un rol con ese nombre.', 16, 1);
        RETURN;
    END;

    INSERT INTO Roles (NombreRol) VALUES (@NombreRol);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdRol;
END;
GO

-- cambia el nombre de un rol, el nombre nuevo no puede ser el de otro rol
CREATE PROCEDURE SP_ModificarRol
    @IdRol INT,
    @NombreRol VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Roles WHERE IdRol = @IdRol)
    BEGIN
        RAISERROR('El rol no existe.', 16, 1);
        RETURN;
    END;

    IF EXISTS (SELECT 1 FROM Roles WHERE NombreRol = @NombreRol AND IdRol <> @IdRol)
    BEGIN
        RAISERROR('Ya existe un rol con ese nombre.', 16, 1);
        RETURN;
    END;

    UPDATE Roles SET NombreRol = @NombreRol WHERE IdRol = @IdRol;
END;
GO

-- da de baja un rol (lo borra), pero no los dos roles base (1 y 2) ni si hay usuarios con ese rol
CREATE PROCEDURE SP_EliminarRol
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @IdRol IN (1, 2)
    BEGIN
        RAISERROR('No se puede dar de baja un rol base del sistema.', 16, 1);
        RETURN;
    END;

    IF EXISTS (SELECT 1 FROM Usuarios WHERE IdRol = @IdRol)
    BEGIN
        RAISERROR('No se puede dar de baja el rol porque hay usuarios que lo tienen asignado.', 16, 1);
        RETURN;
    END;

    DELETE FROM Roles WHERE IdRol = @IdRol;
END;
GO

CREATE PROCEDURE SP_ExisteUsuario
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

CREATE PROCEDURE SP_ExisteCorreo
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

CREATE PROCEDURE SP_ObtenerCorreoPorUsuario
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Correo FROM Usuarios WHERE Usuario = @Usuario;
END;
GO

CREATE PROCEDURE SP_ObtenerRolPorUsuario
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdRol FROM Usuarios WHERE Usuario = @Usuario;
END;
GO

CREATE PROCEDURE SP_BuscarUsuarioPorId
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
    LEFT JOIN VW_Direcciones D ON P.IdDireccion = D.IdDireccion
    WHERE U.IdUsuario = @IdUsuario;
END;
GO

CREATE PROCEDURE SP_BuscarUsuarioPorNombreUser
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
    LEFT JOIN VW_Direcciones D ON P.IdDireccion = D.IdDireccion
    WHERE U.Usuario = @Usuario;
END;
GO

CREATE PROCEDURE SP_BuscarUsuarioPorCorreo
    @Correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Usuarios WHERE Correo LIKE '%' + @Correo + '%';
END;
GO

-- los mismos filtros y suma la bsuqueda

CREATE PROCEDURE SP_BuscarUsuariosGrilla
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
    LEFT JOIN VW_Direcciones D ON P.IdDireccion = D.IdDireccion
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

-- alta y baja
CREATE PROCEDURE SP_ActualizarEstadoUsuario
    @IdUsuario INT,
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios SET Activo = @Activo WHERE IdUsuario = @IdUsuario;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END AS Resultado;
END;
GO

CREATE PROCEDURE SP_AsignarRolUsuario
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

-- modificadores
CREATE PROCEDURE SP_ActualizarDatosUsuario
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
    @IdLocalidad VARCHAR(30),
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
            IdLocalidad = @IdLocalidad,
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

-- hace el registro complet
CREATE PROCEDURE SP_RegistrarUsuarioCompleto
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
    @IdLocalidad VARCHAR(30),
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

        INSERT INTO Direcciones (Calle, Numero, CodigoPostal, Depto, Piso, IdLocalidad,
                                 Latitud, Longitud, DireccionOriginal, DireccionNormalizada)
        VALUES (@Calle, @Numero, @CodigoPostal, @Depto, @Piso, @IdLocalidad,
                @Latitud, @Longitud, @DireccionOriginal, @DireccionNormalizada);

        SET @IdDireccion = SCOPE_IDENTITY();

        INSERT INTO Personas (Nombre, Apellido, DNI, Telefono, FechaNacimiento, IdDireccion)
        VALUES (@Nombre, @Apellido, @DNI, @Telefono, @FechaNacimiento, @IdDireccion);

        SET @IdPersona = SCOPE_IDENTITY();

        INSERT INTO Usuarios (IdPersona, Usuario, Correo, IdRol, Activo, PrimerLogin, Usa2FA)
        VALUES (@IdPersona, @Usuario, @Correo, @IdRol, 1, 1, @Usa2FA);

        SET @IdUsuario = SCOPE_IDENTITY();

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

CREATE PROCEDURE SP_UsuarioActivo
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Activo FROM Usuarios WHERE Usuario = @Usuario;
END;
GO

-- valida q la contraseña no tenga datos del usuer
CREATE PROCEDURE SP_ObtenerDatosPersonalesPorUsuario
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

-- tema login
CREATE PROCEDURE SP_VerificarLogin
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

CREATE PROCEDURE SP_EsPrimerLogin
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PrimerLogin FROM Usuarios WHERE Usuario = @Usuario;
END;
GO

CREATE PROCEDURE SP_CambiarPassword
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

        -- hace q no se repita la contraseña
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

CREATE PROCEDURE SP_VerificarPasswordEnHistorial
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

CREATE PROCEDURE SP_InsertarPassword
    @IdUsuario INT,
    @PasswordHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Passwords (IdUsuario, PasswordHash, FechaCreacion)
    VALUES (@IdUsuario, @PasswordHash, GETDATE());
END;
GO

CREATE PROCEDURE SP_ObtenerContrasAnteriores
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

CREATE PROCEDURE SP_ObtenerUltimoPasswordHash
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

-- marca el primer login y tamb hace q pida cambiar la contraseña
CREATE PROCEDURE SP_ActualizarPasswordYPrimerLogin
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

CREATE PROCEDURE SP_ListarPreguntasSeguridad
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdPregunta, Pregunta FROM PreguntasSeguridad ORDER BY Pregunta;
END;
GO

-- actualiza la respuesta
CREATE PROCEDURE SP_GuardarRespuestasSeguridad
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

CREATE PROCEDURE SP_ObtenerPreguntasUsuario
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

CREATE PROCEDURE SP_VerificarRespuestaSeguridad
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

-- configuraciones
CREATE PROCEDURE SP_ObtenerConfiguracion
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

--actualiza la configuracion
CREATE PROCEDURE SP_ActualizarPoliticasSeguridad
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

-- tema correo

CREATE PROCEDURE SP_ObtenerConfiguracionCorreo
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

CREATE PROCEDURE SP_GuardarConfiguracionCorreo
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

-- es para listar lo productos en las grillas
CREATE PROCEDURE SP_ObtenerProductos
    @SoloHabilitados BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID_Codigo, FK_Categoria, FK_Marca, Nombre, Precio_Compra, Precio_Venta,
           Stock_Actual, Stock_Critico, Stock_Minimo, Stock_Ideal, Stock_Maximo,
           Punto_Reposicion, Numero_Lote, Fecha_Vencimiento, Estado, Habilitado
    FROM Productos
    WHERE @SoloHabilitados = 0 OR Habilitado = 1
    ORDER BY Nombre;
END;
GO

CREATE PROCEDURE SP_ObtenerProductoPorId
    @ID_Codigo INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID_Codigo, FK_Categoria, FK_Marca, Nombre, Precio_Compra, Precio_Venta,
           Stock_Actual, Stock_Critico, Stock_Minimo, Stock_Ideal, Stock_Maximo,
           Punto_Reposicion, Numero_Lote, Fecha_Vencimiento, Estado, Habilitado
    FROM Productos
    WHERE ID_Codigo = @ID_Codigo;
END;
GO

-- pone los filtros de busqueda
CREATE PROCEDURE SP_BuscarProductos
    @Texto VARCHAR(150) = NULL,
    @FK_Categoria INT = NULL,
    @FK_Marca INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID_Codigo, FK_Categoria, FK_Marca, Nombre, Precio_Compra, Precio_Venta,
           Stock_Actual, Stock_Critico, Stock_Minimo, Stock_Ideal, Stock_Maximo,
           Punto_Reposicion, Numero_Lote, Fecha_Vencimiento, Estado, Habilitado
    FROM Productos
    WHERE Habilitado = 1
      AND (@Texto IS NULL OR Nombre LIKE '%' + @Texto + '%')
      AND (@FK_Categoria IS NULL OR FK_Categoria = @FK_Categoria)
      AND (@FK_Marca IS NULL OR FK_Marca = @FK_Marca)
    ORDER BY Nombre;
END;
GO
 -- da el alta de los productos
CREATE PROCEDURE SP_InsertarProducto
    @FK_Categoria INT,
    @FK_Marca INT,
    @Nombre VARCHAR(150),
    @Precio_Compra DECIMAL(18,2),
    @Precio_Venta DECIMAL(18,2),
    @Stock_Critico INT,
    @Stock_Minimo INT,
    @Stock_Ideal INT,
    @Stock_Maximo INT,
    @Punto_Reposicion INT,
    @Numero_Lote VARCHAR(50) = NULL,
    @Fecha_Vencimiento DATE = NULL,
    @FK_Usuario INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Id INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE IdUsuario = @FK_Usuario)
            THROW 50003, 'Usuario inválido.', 1;

        -- el stock siempre arranca en 0
        INSERT INTO Productos (FK_Categoria, FK_Marca, Nombre, Precio_Compra, Precio_Venta,
                               Stock_Actual, Stock_Critico, Stock_Minimo, Stock_Ideal, Stock_Maximo,
                               Punto_Reposicion, Numero_Lote, Fecha_Vencimiento, Estado, Habilitado)
        VALUES (@FK_Categoria, @FK_Marca, @Nombre, @Precio_Compra, @Precio_Venta,
                0, @Stock_Critico, @Stock_Minimo, @Stock_Ideal, @Stock_Maximo,
                @Punto_Reposicion, @Numero_Lote, @Fecha_Vencimiento, 'Activo', 1);

        SET @Id = SCOPE_IDENTITY();

        INSERT INTO Auditoria_Cambios (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
        VALUES ('Productos', @Id, 'Alta', @FK_Usuario, GETDATE(),
                CONCAT('Alta de producto ''', @Nombre, ''' - compra ', @Precio_Compra, ', venta ', @Precio_Venta));

        COMMIT TRANSACTION;

        SELECT @Id AS ID_Codigo;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

-- modifica el producto y en la auditoria deja que campos cambiaron
CREATE PROCEDURE SP_ActualizarProducto
    @ID_Codigo INT,
    @FK_Categoria INT,
    @FK_Marca INT,
    @Nombre VARCHAR(150),
    @Precio_Compra DECIMAL(18,2),
    @Precio_Venta DECIMAL(18,2),
    @Stock_Critico INT,
    @Stock_Minimo INT,
    @Stock_Ideal INT,
    @Stock_Maximo INT,
    @Punto_Reposicion INT,
    @Numero_Lote VARCHAR(50) = NULL,
    @Fecha_Vencimiento DATE = NULL,
    @Estado VARCHAR(30),
    @FK_Usuario INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ViejoNombre VARCHAR(150);
    DECLARE @ViejoCompra DECIMAL(18,2);
    DECLARE @ViejoVenta DECIMAL(18,2);
    DECLARE @ViejoEstado VARCHAR(30);
    DECLARE @ViejoHabilitado BIT;
    DECLARE @Detalle VARCHAR(MAX);

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE IdUsuario = @FK_Usuario)
            THROW 50003, 'Usuario inválido.', 1;

        -- guardo los valores viejos para registrar cada cambio
        SELECT @ViejoNombre = Nombre,
               @ViejoCompra = Precio_Compra,
               @ViejoVenta = Precio_Venta,
               @ViejoEstado = Estado,
               @ViejoHabilitado = Habilitado
        FROM Productos WITH (UPDLOCK, HOLDLOCK)
        WHERE ID_Codigo = @ID_Codigo;

        IF @ViejoHabilitado IS NULL
            THROW 50001, 'El producto no existe.', 1;

        IF @ViejoHabilitado = 0
            THROW 50004, 'El producto está dado de baja.', 1;

        SET @Detalle = CONCAT_WS('; ',
            IIF(@ViejoNombre <> @Nombre, CONCAT('Nombre: ', @ViejoNombre, ' -> ', @Nombre), NULL),
            IIF(@ViejoCompra <> @Precio_Compra, CONCAT('Precio compra: ', @ViejoCompra, ' -> ', @Precio_Compra), NULL),
            IIF(@ViejoVenta <> @Precio_Venta, CONCAT('Precio venta: ', @ViejoVenta, ' -> ', @Precio_Venta), NULL),
            IIF(@ViejoEstado <> @Estado, CONCAT('Estado: ', @ViejoEstado, ' -> ', @Estado), NULL));

        UPDATE Productos
        SET FK_Categoria = @FK_Categoria,
            FK_Marca = @FK_Marca,
            Nombre = @Nombre,
            Precio_Compra = @Precio_Compra,
            Precio_Venta = @Precio_Venta,
            Stock_Critico = @Stock_Critico,
            Stock_Minimo = @Stock_Minimo,
            Stock_Ideal = @Stock_Ideal,
            Stock_Maximo = @Stock_Maximo,
            Punto_Reposicion = @Punto_Reposicion,
            Numero_Lote = @Numero_Lote,
            Fecha_Vencimiento = @Fecha_Vencimiento,
            Estado = @Estado
        WHERE ID_Codigo = @ID_Codigo;

        INSERT INTO Auditoria_Cambios (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
        VALUES ('Productos', @ID_Codigo, 'Modificación', @FK_Usuario, GETDATE(),
                ISNULL(@Detalle, 'Se guardó sin cambios en nombre, precios ni estado'));

        COMMIT TRANSACTION;

        SELECT 1 AS Filas_Afectadas;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

-- da la baja logica
CREATE PROCEDURE SP_DarBajaProducto
    @ID_Codigo INT,
    @FK_Usuario INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Filas INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE IdUsuario = @FK_Usuario)
            THROW 50003, 'Usuario inválido.', 1;

        UPDATE Productos
        SET Habilitado = 0
        WHERE ID_Codigo = @ID_Codigo AND Habilitado = 1;

        SET @Filas = @@ROWCOUNT;

        -- solo audita si realmente dio de baja algo
        IF @Filas > 0
            INSERT INTO Auditoria_Cambios (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
            VALUES ('Productos', @ID_Codigo, 'Baja', @FK_Usuario, GETDATE(), 'Baja lógica de producto');

        COMMIT TRANSACTION;

        SELECT @Filas AS Filas_Afectadas;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE SP_ObtenerProductosBajoPuntoReposicion
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID_Codigo, Nombre, Stock_Actual, Punto_Reposicion
    FROM Productos
    WHERE Habilitado = 1
      AND Stock_Actual < Punto_Reposicion
    ORDER BY (Punto_Reposicion - Stock_Actual) DESC;
END;
GO

-- maneja los vencimientos
CREATE PROCEDURE SP_ObtenerProductosProximosAVencer
    @DiasAnticipacion INT = 30
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);

    SELECT ID_Codigo, Nombre, Numero_Lote, Fecha_Vencimiento, Stock_Actual
    FROM Productos
    WHERE Habilitado = 1
      AND Fecha_Vencimiento IS NOT NULL
      AND Fecha_Vencimiento BETWEEN @Hoy AND DATEADD(DAY, @DiasAnticipacion, @Hoy)
    ORDER BY Fecha_Vencimiento;
END;
GO

CREATE PROCEDURE SP_ReporteStockPorCategoria
AS
BEGIN
    SET NOCOUNT ON;

    SELECT c.Nombre AS Categoria,
           COUNT(p.ID_Codigo) AS Cantidad_Productos,
           ISNULL(SUM(p.Stock_Actual), 0) AS Stock_Total
    FROM Categorias c
    LEFT JOIN Productos p ON p.FK_Categoria = c.IdCategoria AND p.Habilitado = 1
    GROUP BY c.IdCategoria, c.Nombre
    ORDER BY c.Nombre;
END;
GO

CREATE PROCEDURE SP_ReporteStockPorMarca
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.Nombre AS Marca,
           COUNT(p.ID_Codigo) AS Cantidad_Productos,
           ISNULL(SUM(p.Stock_Actual), 0) AS Stock_Total
    FROM Marcas m
    LEFT JOIN Productos p ON p.FK_Marca = m.IdMarca AND p.Habilitado = 1
    GROUP BY m.IdMarca, m.Nombre
    ORDER BY m.Nombre;
END;
GO

-- muestra los movimeintos del stock
CREATE PROCEDURE SP_ObtenerMovimientosPorProducto
    @FK_Producto INT,
    @Desde DATETIME = NULL,
    @Hasta DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    WITH Kardex AS (
        SELECT m.ID, m.FK_Producto, p.Nombre AS Producto, m.FK_Usuario, u.Usuario,
               m.Tipo_Movimiento, m.Cantidad, m.Fecha, m.Tipo_Documento_Origen, m.ID_Documento_Origen,
               SUM(CASE m.Tipo_Movimiento WHEN 'Egreso' THEN -m.Cantidad ELSE m.Cantidad END)
                   OVER (ORDER BY m.Fecha, m.ID) AS Stock_Posterior
        FROM Movimientos_Stock m
        INNER JOIN Productos p ON p.ID_Codigo = m.FK_Producto
        INNER JOIN Usuarios u ON u.IdUsuario = m.FK_Usuario
        WHERE m.FK_Producto = @FK_Producto
    )
    SELECT ID, FK_Producto, Producto, FK_Usuario, Usuario, Tipo_Movimiento, Cantidad, Fecha,
           Tipo_Documento_Origen, ID_Documento_Origen, Stock_Posterior
    FROM Kardex
    WHERE (@Desde IS NULL OR Fecha >= @Desde)
      AND (@Hasta IS NULL OR Fecha <= @Hasta)
    ORDER BY Fecha, ID;
END;
GO

-- registraa el movimeinto para mostrarlo en auditoria
CREATE PROCEDURE SP_InsertarMovimientoStock
    @FK_Producto INT,
    @FK_Usuario INT,
    @Tipo_Movimiento VARCHAR(20),
    @Cantidad INT,
    @Tipo_Documento_Origen VARCHAR(50) = NULL,
    @ID_Documento_Origen INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StockActual INT;
    DECLARE @Habilitado BIT;
    DECLARE @StockNuevo INT;
    DECLARE @IdMov INT;
    DECLARE @Mensaje VARCHAR(4000);

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE IdUsuario = @FK_Usuario)
            THROW 50003, 'Usuario inválido.', 1;

        IF @Tipo_Movimiento NOT IN ('Ingreso', 'Egreso', 'Ajuste')
            THROW 50005, 'Tipo de movimiento inválido.', 1;

        IF (@Tipo_Movimiento = 'Ajuste' AND @Cantidad = 0)
           OR (@Tipo_Movimiento <> 'Ajuste' AND @Cantidad <= 0)
            THROW 50006, 'Cantidad inválida para el tipo de movimiento.', 1;

        SELECT @StockActual = Stock_Actual, @Habilitado = Habilitado
        FROM Productos WITH (UPDLOCK, HOLDLOCK)
        WHERE ID_Codigo = @FK_Producto;

        IF @StockActual IS NULL
            THROW 50001, 'El producto no existe.', 1;

        IF @Habilitado = 0
            THROW 50004, 'El producto está dado de baja: no admite movimientos.', 1;

        IF @Tipo_Movimiento = 'Egreso'
            SET @StockNuevo = @StockActual - @Cantidad;
        ELSE
            SET @StockNuevo = @StockActual + @Cantidad;

        IF @StockNuevo < 0
            THROW 50007, 'El movimiento dejaría el stock en negativo.', 1;

        INSERT INTO Movimientos_Stock (FK_Producto, FK_Usuario, Tipo_Movimiento, Cantidad, Fecha,
                                       Tipo_Documento_Origen, ID_Documento_Origen)
        VALUES (@FK_Producto, @FK_Usuario, @Tipo_Movimiento, @Cantidad, GETDATE(),
                @Tipo_Documento_Origen, @ID_Documento_Origen);

        SET @IdMov = SCOPE_IDENTITY();

        UPDATE Productos SET Stock_Actual = @StockNuevo WHERE ID_Codigo = @FK_Producto;

        INSERT INTO Auditoria_Cambios (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
        VALUES ('Movimientos_Stock', @IdMov, 'Alta', @FK_Usuario, GETDATE(),
                CONCAT(@Tipo_Movimiento, ' de ', @Cantidad, ' unidades del producto ', @FK_Producto,
                       ' - stock ', @StockActual, ' -> ', @StockNuevo));

        COMMIT TRANSACTION;

        SELECT @IdMov AS ID, @StockNuevo AS Stock_Resultante;
    END TRY
    BEGIN CATCH
        SET @Mensaje = ERROR_MESSAGE();

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT @Mensaje AS Error;
    END CATCH
END;
GO


CREATE PROCEDURE SP_ObtenerScrap
    @Desde DATETIME = NULL,
    @Hasta DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT s.ID, s.FK_Producto, p.Nombre AS Producto, s.FK_Usuario, u.Usuario,
           s.Cantidad, s.Motivo, s.Costo_Valorizado, s.Fecha
    FROM Registros_Scrap s
    INNER JOIN Productos p ON p.ID_Codigo = s.FK_Producto
    INNER JOIN Usuarios u ON u.IdUsuario = s.FK_Usuario
    WHERE (@Desde IS NULL OR s.Fecha >= @Desde)
      AND (@Hasta IS NULL OR s.Fecha <= @Hasta)
    ORDER BY s.Fecha DESC;
END;
GO

-- lo registra y lo descuenta del stock y lo agrega a auditoria
CREATE PROCEDURE SP_InsertarScrap
    @FK_Producto INT,
    @FK_Usuario INT,
    @Cantidad INT,
    @Motivo VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StockActual INT;
    DECLARE @PrecioCompra DECIMAL(18,2);
    DECLARE @Habilitado BIT;
    DECLARE @StockNuevo INT;
    DECLARE @Costo DECIMAL(18,2);
    DECLARE @IdScrap INT;
    DECLARE @IdMov INT;
    DECLARE @Mensaje VARCHAR(4000);

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE IdUsuario = @FK_Usuario)
            THROW 50003, 'Usuario inválido.', 1;

        IF @Cantidad <= 0
            THROW 50006, 'La cantidad debe ser mayor a cero.', 1;

        IF LEN(LTRIM(RTRIM(ISNULL(@Motivo, '')))) = 0
            THROW 50008, 'Debe indicar el motivo del scrap.', 1;

        SELECT @StockActual = Stock_Actual, @PrecioCompra = Precio_Compra, @Habilitado = Habilitado
        FROM Productos WITH (UPDLOCK, HOLDLOCK)
        WHERE ID_Codigo = @FK_Producto;

        IF @StockActual IS NULL
            THROW 50001, 'El producto no existe.', 1;

        IF @Habilitado = 0
            THROW 50004, 'El producto está dado de baja: no admite scrap.', 1;

        SET @StockNuevo = @StockActual - @Cantidad;

        IF @StockNuevo < 0
            THROW 50007, 'No hay stock suficiente para dar de baja esa cantidad.', 1;

        SET @Costo = @Cantidad * @PrecioCompra;

        INSERT INTO Registros_Scrap (FK_Producto, FK_Usuario, Cantidad, Motivo, Costo_Valorizado, Fecha)
        VALUES (@FK_Producto, @FK_Usuario, @Cantidad, @Motivo, @Costo, GETDATE());

        SET @IdScrap = SCOPE_IDENTITY();

        INSERT INTO Movimientos_Stock (FK_Producto, FK_Usuario, Tipo_Movimiento, Cantidad, Fecha,
                                       Tipo_Documento_Origen, ID_Documento_Origen)
        VALUES (@FK_Producto, @FK_Usuario, 'Egreso', @Cantidad, GETDATE(), 'Scrap', @IdScrap);

        SET @IdMov = SCOPE_IDENTITY();

        UPDATE Productos SET Stock_Actual = @StockNuevo WHERE ID_Codigo = @FK_Producto;

        INSERT INTO Auditoria_Cambios (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
        VALUES ('Registros_Scrap', @IdScrap, 'Alta', @FK_Usuario, GETDATE(),
                CONCAT('Scrap de ', @Cantidad, ' unidades del producto ', @FK_Producto,
                       ' por ''', @Motivo, ''' - costo ', @Costo, ', stock ', @StockActual, ' -> ', @StockNuevo)),
               ('Movimientos_Stock', @IdMov, 'Alta', @FK_Usuario, GETDATE(),
                CONCAT('Egreso por scrap #', @IdScrap, ' de ', @Cantidad, ' unidades del producto ', @FK_Producto));

        COMMIT TRANSACTION;

        SELECT @IdScrap AS ID, @StockNuevo AS Stock_Resultante, @Costo AS Costo_Valorizado;
    END TRY
    BEGIN CATCH
        SET @Mensaje = ERROR_MESSAGE();

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT @Mensaje AS Error;
    END CATCH
END;
GO

-- registra todo tipo de cambio
CREATE PROCEDURE SP_RegistrarCambioAuditoria
    @Tabla_Afectada VARCHAR(100),
    @ID_Registro_Afectado INT,
    @Accion VARCHAR(20),
    @FK_Usuario INT,
    @Detalle_Cambio VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Auditoria_Cambios (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
    VALUES (@Tabla_Afectada, @ID_Registro_Afectado, @Accion, @FK_Usuario, GETDATE(), @Detalle_Cambio);
END;
GO

CREATE PROCEDURE SP_ObtenerAuditoria
    @Tabla_Afectada VARCHAR(100) = NULL,
    @FK_Usuario INT = NULL,
    @Desde DATETIME = NULL,
    @Hasta DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT a.ID, a.Tabla_Afectada, a.ID_Registro_Afectado, a.Accion, a.FK_Usuario, u.Usuario,
           a.Fecha_Hora, a.Detalle_Cambio
    FROM Auditoria_Cambios a
    INNER JOIN Usuarios u ON u.IdUsuario = a.FK_Usuario
    WHERE (@Tabla_Afectada IS NULL OR a.Tabla_Afectada = @Tabla_Afectada)
      AND (@FK_Usuario IS NULL OR a.FK_Usuario = @FK_Usuario)
      AND (@Desde IS NULL OR a.Fecha_Hora >= @Desde)
      AND (@Hasta IS NULL OR a.Fecha_Hora <= @Hasta)
    ORDER BY a.Fecha_Hora DESC, a.ID DESC;
END;
GO


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

-- ¿guarda el valor x defecto
INSERT INTO PoliticasSeguridad DEFAULT VALUES;
GO


INSERT INTO Categorias (Nombre) VALUES ('Almacén'), ('Bebidas'), ('Limpieza'), ('Lácteos');
INSERT INTO Marcas (Nombre) VALUES ('Genérica'), ('La Serenísima'), ('Arcor');
GO

-- usuario administrador para poder entrar la primera vez
-- la contraseña se guarda hasheada igual que en la app (SHA256 de usuario + contraseña)
-- no tiene direccion cargada, se puede completar despues desde el sistema
DECLARE @IdPersona INT;
DECLARE @IdUsuario INT;

INSERT INTO Personas (Nombre, Apellido, DNI, Telefono, FechaNacimiento)
VALUES ('Admin', 'Sistema', '11111111', '1111111111', '1990-01-01');

SET @IdPersona = SCOPE_IDENTITY();

-- PrimerLogin en 0 para que no pida cambiar la contraseña al entrar
INSERT INTO Usuarios (Usuario, IdPersona, IdRol, Activo, PrimerLogin, Usa2FA, Correo)
VALUES ('admin', @IdPersona, (SELECT IdRol FROM Roles WHERE NombreRol = 'Administrador'), 1, 0, 0, 'admin@sistema.com');

SET @IdUsuario = SCOPE_IDENTITY();

INSERT INTO Passwords (IdUsuario, PasswordHash)
VALUES (@IdUsuario, 'c91kyEllFrjblrVFPF7uD+QWKyT1RJ/LQ3z9Rx1d2l8=');
GO
