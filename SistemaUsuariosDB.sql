USE [SistemaUsuariosDB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================================
-- SistemaUsuariosDB - script corregido
-- Base: SistemaUsuariosDB.sql original + hallazgos de Auditoria_SistemaPP.pdf
--       + tareas T01, T04, T08-BD, T11-DAT de Roadmap_SistemaPP.pdf
--
-- Generado para revisión de Arce. NO fue ejecutado contra ningún servidor.
--
-- Decisiones tomadas al consolidar (marcadas también en línea con "-- NOTA"):
--   1) Se agrega SP_ObtenerRolUsuario (nombre exacto que invoca el C#) sin
--      tocar SP_ObtenerRolPorUsuario, que ya existía y queda intacto.
--   2) NO se agrega la columna EsTemporal a Passwords. La corrección real
--      está en CD_contraDatos.cs (T02): sacar esa columna del INSERT. Acá
--      no se le da soporte de esquema a algo que ningún flujo activo usa.
--   3) NO se crean SP_ActualizarUsuarioCompleto ni SP_ActualizarPrimerLogin.
--      Los métodos C# que los invocan no tienen ningún llamador (confirmado
--      por grep sobre el repo) - el roadmap (T20) indica eliminarlos, no
--      darles soporte en la base.
--   4) RequiereMinusculas se agrega a PoliticasSeguridad con default 0, y el
--      parámetro nuevo del SP de update tiene valor por defecto (= 0) para
--      no romper la llamada actual desde CD_configuracionDatos.cs, que
--      todavía no lo envía. Falta el lado C# (mapeo + control en
--      frmConfigAdmin) para que sea usable de punta a punta.
--   5) SP_ActualizarEstadoUsuario se reordena: ahora hace el UPDATE primero
--      y devuelve @@ROWCOUNT real, en vez de un SELECT 1 fijo antes del
--      UPDATE (hallazgo 6 / T04).
--   6) Se agregan SP_GuardarProvincia, SP_GuardarPartido y SP_GuardarLocalidad
--      (hallazgo 3 / T01), con patrón idempotente (IF NOT EXISTS) porque
--      pueden llamarse repetidas veces durante la sincronización geográfica.
--   7) Se agregan datos semilla de Roles y PreguntasSeguridad (hallazgo 7 /
--      T08-BD). El orden de inserción de Roles importa: coincide con el
--      mapeo hardcodeado en frmRegistro.cs (Usuario=1, Administrador=2).
--      Corregir ese hardcodeo (T08-VIS, para que frmRegistro consulte
--      SP_ListarRoles) es una tarea aparte, del lado C#, no de este script.
-- ============================================================================

-- datos para la api de direccion
CREATE TABLE [dbo].[Provincias](
	[IdProvincia] [varchar](10) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
PRIMARY KEY
(
	[IdProvincia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Partidos](
	[IdPartido] [varchar](20) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[IdProvincia] [varchar](10) NOT NULL,
PRIMARY KEY
(
	[IdPartido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Localidades](
	[IdLocalidad] [varchar](30) NOT NULL,
	[Nombre] [varchar](150) NOT NULL,
	[IdPartido] [varchar](20) NOT NULL,
PRIMARY KEY
(
	[IdLocalidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Direcciones](
	[IdDireccion] [int] IDENTITY(1,1) NOT NULL,
	[Calle] [varchar](150) NOT NULL,
	[Numero] [varchar](20) NULL,
	[CodigoPostal] [varchar](20) NULL,
	[Depto] [varchar](20) NULL,
	[Piso] [varchar](20) NULL,
	[Provincia] [varchar](100) NOT NULL,
	[Partido] [varchar](100) NULL,
	[Localidad] [varchar](150) NULL,
	[Latitud] [decimal](10, 7) NULL,
	[Longitud] [decimal](10, 7) NULL,
	[DireccionOriginal] [varchar](250) NULL,
	[DireccionNormalizada] [varchar](250) NULL,
PRIMARY KEY
(
	[IdDireccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Datos personales y cuenta de acceso
CREATE TABLE [dbo].[Roles](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[NombreRol] [varchar](50) NOT NULL,
CONSTRAINT [PK_Roles] PRIMARY KEY
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED
(
	[NombreRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Personas](
	[IdPersona] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Apellido] [varchar](100) NOT NULL,
	[DNI] [varchar](20) NOT NULL,
	[Telefono] [varchar](30) NULL,
	[FechaNacimiento] [date] NULL,
	[IdDireccion] [int] NULL,
	[FechaAlta] [datetime] NOT NULL,
PRIMARY KEY
(
	[IdPersona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED
(
	[DNI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [varchar](50) NOT NULL,
	[IdPersona] [int] NOT NULL,
	[IdRol] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[PrimerLogin] [bit] NOT NULL,
	[Usa2FA] [bit] NOT NULL,
	[FechaAlta] [datetime] NOT NULL,
	[Correo] [varchar](100) NOT NULL,
PRIMARY KEY
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED
(
	[Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Seguridad de acceso: passwords, historial, recuperacion y preguntas de seguridad
-- NOTA (hallazgo 4 / T02): esta tabla NO incluye EsTemporal a propósito.
-- El INSERT roto que la referencia está en CD_contraDatos.cs y se corrige ahí.
CREATE TABLE [dbo].[Passwords](
	[IdPassword] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[PasswordHash] [varchar](256) NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
PRIMARY KEY
(
	[IdPassword] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[HistorialPasswords](
	[IdHistorial] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[PasswordHash] [varchar](256) NOT NULL,
	[FechaUso] [datetime] NULL,
PRIMARY KEY
(
	[IdHistorial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[RecuperacionPassword](
	[IdRecuperacion] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Codigo] [varchar](100) NULL,
	[FechaSolicitud] [datetime] NULL,
	[Usado] [bit] NULL,
PRIMARY KEY
(
	[IdRecuperacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PreguntasSeguridad](
	[IdPregunta] [int] IDENTITY(1,1) NOT NULL,
	[Pregunta] [varchar](255) NOT NULL,
PRIMARY KEY
(
	[IdPregunta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[RespuestasUsuario](
	[IdRespuesta] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdPregunta] [int] NOT NULL,
	[RespuestaHash] [varchar](256) NOT NULL,
PRIMARY KEY
(
	[IdRespuesta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- NOTA (hallazgo 9 / T11-DAT): se agrega RequiereMinusculas, ausente en el
-- script original. La entidad C# configuracionSistema.cs ya tenía la
-- propiedad; sin esta columna nunca se podía persistir.
CREATE TABLE [dbo].[PoliticasSeguridad](
	[IdPolitica] [int] IDENTITY(1,1) NOT NULL,
	[MinCaracteres] [int] NULL,
	[CantPreguntas] [int] NULL,
	[RequiereMayusculas] [bit] NULL,
	[RequiereMinusculas] [bit] NULL,
	[RequiereNumeros] [bit] NULL,
	[RequiereEspeciales] [bit] NULL,
	[Requiere2FA] [bit] NULL,
	[NoRepetirPasswords] [bit] NULL,
	[ValidarDatosPersonales] [bit] NULL,
PRIMARY KEY
(
	[IdPolitica] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- es para poner los valores x defecto
ALTER TABLE [dbo].[Usuarios] ADD DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Usuarios] ADD DEFAULT ((1)) FOR [PrimerLogin]
GO
ALTER TABLE [dbo].[Usuarios] ADD DEFAULT ((0)) FOR [Usa2FA]
GO
ALTER TABLE [dbo].[Usuarios] ADD DEFAULT (getdate()) FOR [FechaAlta]
GO
ALTER TABLE [dbo].[Usuarios] ADD CONSTRAINT [DF_Usuarios_Correo] DEFAULT ('') FOR [Correo]
GO

ALTER TABLE [dbo].[Personas] ADD DEFAULT (getdate()) FOR [FechaAlta]
GO

ALTER TABLE [dbo].[Passwords] ADD DEFAULT (getdate()) FOR [FechaCreacion]
GO

ALTER TABLE [dbo].[HistorialPasswords] ADD DEFAULT (getdate()) FOR [FechaUso]
GO

ALTER TABLE [dbo].[RecuperacionPassword] ADD DEFAULT (getdate()) FOR [FechaSolicitud]
GO
ALTER TABLE [dbo].[RecuperacionPassword] ADD DEFAULT ((0)) FOR [Usado]
GO

ALTER TABLE [dbo].[PoliticasSeguridad] ADD DEFAULT ((6)) FOR [MinCaracteres]
GO
ALTER TABLE [dbo].[PoliticasSeguridad] ADD DEFAULT ((2)) FOR [CantPreguntas]
GO
ALTER TABLE [dbo].[PoliticasSeguridad] ADD DEFAULT ((0)) FOR [RequiereMayusculas]
GO
ALTER TABLE [dbo].[PoliticasSeguridad] ADD DEFAULT ((0)) FOR [RequiereMinusculas]
GO
ALTER TABLE [dbo].[PoliticasSeguridad] ADD DEFAULT ((0)) FOR [RequiereNumeros]
GO
ALTER TABLE [dbo].[PoliticasSeguridad] ADD DEFAULT ((0)) FOR [RequiereEspeciales]
GO
ALTER TABLE [dbo].[PoliticasSeguridad] ADD DEFAULT ((0)) FOR [Requiere2FA]
GO
ALTER TABLE [dbo].[PoliticasSeguridad] ADD DEFAULT ((1)) FOR [NoRepetirPasswords]
GO
ALTER TABLE [dbo].[PoliticasSeguridad] ADD DEFAULT ((1)) FOR [ValidarDatosPersonales]
GO

-- foraneas

ALTER TABLE [dbo].[Partidos] WITH CHECK ADD CONSTRAINT [FK_Partidos_Provincias] FOREIGN KEY([IdProvincia])
REFERENCES [dbo].[Provincias] ([IdProvincia])
GO
ALTER TABLE [dbo].[Partidos] CHECK CONSTRAINT [FK_Partidos_Provincias]
GO

ALTER TABLE [dbo].[Localidades] WITH CHECK ADD CONSTRAINT [FK_Localidades_Partidos] FOREIGN KEY([IdPartido])
REFERENCES [dbo].[Partidos] ([IdPartido])
GO
ALTER TABLE [dbo].[Localidades] CHECK CONSTRAINT [FK_Localidades_Partidos]
GO

ALTER TABLE [dbo].[Personas] WITH CHECK ADD CONSTRAINT [FK_Personas_Direcciones] FOREIGN KEY([IdDireccion])
REFERENCES [dbo].[Direcciones] ([IdDireccion])
GO
ALTER TABLE [dbo].[Personas] CHECK CONSTRAINT [FK_Personas_Direcciones]
GO

ALTER TABLE [dbo].[Usuarios] WITH CHECK ADD CONSTRAINT [FK_Usuarios_Personas] FOREIGN KEY([IdPersona])
REFERENCES [dbo].[Personas] ([IdPersona])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Personas]
GO
ALTER TABLE [dbo].[Usuarios] WITH CHECK ADD CONSTRAINT [FK_Usuarios_Roles] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Roles] ([IdRol])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Roles]
GO

ALTER TABLE [dbo].[Passwords] WITH CHECK ADD CONSTRAINT [FK_Passwords_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Passwords] CHECK CONSTRAINT [FK_Passwords_Usuarios]
GO

ALTER TABLE [dbo].[RespuestasUsuario] WITH CHECK ADD FOREIGN KEY([IdPregunta])
REFERENCES [dbo].[PreguntasSeguridad] ([IdPregunta])
GO

-- storeprocedure


CREATE PROCEDURE [dbo].[SP_ObtenerProvincias]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdProvincia,
        Nombre
    FROM dbo.Provincias
    ORDER BY Nombre;
END;
GO

CREATE PROCEDURE [dbo].[SP_ObtenerPartidosPorProvincia]
    @IdProvincia VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdPartido,
        Nombre,
        IdProvincia
    FROM dbo.Partidos
    WHERE IdProvincia = @IdProvincia
    ORDER BY Nombre;
END;
GO

CREATE PROCEDURE [dbo].[SP_ObtenerLocalidadesPorPartido]
    @IdPartido VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdLocalidad,
        Nombre,
        IdPartido
    FROM dbo.Localidades
    WHERE IdPartido = @IdPartido
    ORDER BY Nombre;
END;
GO

CREATE PROCEDURE [dbo].[SP_HayDatosGeograficosCargados]
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Provincias)
        SELECT CAST(1 AS BIT);
    ELSE
        SELECT CAST(0 AS BIT);
END;
GO

-- ----------------------------------------------------------------------------
-- NUEVOS (hallazgo 3 / T01): CD_georefDatos.cs invoca estos tres SP para
-- sincronizar Provincias/Partidos/Localidades desde la API externa y no
-- existían en el script original. Se implementan idempotentes (IF NOT
-- EXISTS) porque la sincronización puede reintentarse.
-- ----------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[SP_GuardarProvincia]
    @IdProvincia VARCHAR(10),
    @Nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Provincias WHERE IdProvincia = @IdProvincia)
    BEGIN
        INSERT INTO dbo.Provincias (IdProvincia, Nombre)
        VALUES (@IdProvincia, @Nombre);
    END
END;
GO

CREATE PROCEDURE [dbo].[SP_GuardarPartido]
    @IdPartido VARCHAR(20),
    @Nombre VARCHAR(100),
    @IdProvincia VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Partidos WHERE IdPartido = @IdPartido)
    BEGIN
        INSERT INTO dbo.Partidos (IdPartido, Nombre, IdProvincia)
        VALUES (@IdPartido, @Nombre, @IdProvincia);
    END
END;
GO

CREATE PROCEDURE [dbo].[SP_GuardarLocalidad]
    @IdLocalidad VARCHAR(30),
    @Nombre VARCHAR(150),
    @IdPartido VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Localidades WHERE IdLocalidad = @IdLocalidad)
    BEGIN
        INSERT INTO dbo.Localidades (IdLocalidad, Nombre, IdPartido)
        VALUES (@IdLocalidad, @Nombre, @IdPartido);
    END
END;
GO
-- ----------------------------------------------------------------------------


CREATE PROCEDURE [dbo].[SP_GuardarDireccion]
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

    INSERT INTO dbo.Direcciones
    (
        Calle,
        Numero,
        CodigoPostal,
        Depto,
        Piso,
        Provincia,
        Partido,
        Localidad,
        Latitud,
        Longitud,
        DireccionOriginal,
        DireccionNormalizada
    )
    VALUES
    (
        @Calle,
        @Numero,
        @CodigoPostal,
        @Depto,
        @Piso,
        @Provincia,
        @Partido,
        @Localidad,
        @Latitud,
        @Longitud,
        @DireccionOriginal,
        @DireccionNormalizada
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdDireccion;
END;
GO

CREATE PROCEDURE [dbo].[SP_ActualizarDireccion]
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

    UPDATE dbo.Direcciones
    SET
        Calle = @Calle,
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


CREATE PROCEDURE [dbo].[SP_ExisteDNI]
    @DNI VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM dbo.Personas
        WHERE DNI = @DNI
    )
    BEGIN
        SELECT 1 AS Existe;
    END
    ELSE
    BEGIN
        SELECT 0 AS Existe;
    END;
END;
GO

CREATE PROCEDURE [dbo].[SP_BuscarPersonaPorDNI]
    @DNI VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Personas
    WHERE DNI = @DNI;
END;
GO

CREATE PROCEDURE [dbo].[SP_BuscarPersonaPorApellido]
    @Apellido VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Personas
    WHERE Apellido LIKE '%' + @Apellido + '%';
END;
GO

CREATE PROCEDURE [dbo].[SP_BuscarPersonaPorNombre]
    @Nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.IdUsuario,
        p.IdPersona,
        d.IdDireccion,
        r.IdRol,
        u.Activo,

        p.Nombre,
        p.Apellido,
        p.DNI,
        p.Telefono,
        p.FechaNacimiento,

        u.Usuario,
        u.Correo,

        r.NombreRol AS Rol,

        CASE
            WHEN u.Activo = 1 THEN 'Activo'
            ELSE 'Inactivo'
        END AS Estado,

        d.Calle,
        d.Numero,
        d.CodigoPostal,
        d.Depto,
        d.Piso,
        d.Provincia,
        d.Partido,
        d.Localidad

    FROM Usuarios u
    INNER JOIN Personas p ON u.IdPersona = p.IdPersona
    INNER JOIN Direcciones d ON p.IdDireccion = d.IdDireccion
    INNER JOIN Roles r ON u.IdRol = r.IdRol
    WHERE p.Nombre LIKE '%' + @Nombre + '%';
END;
GO

CREATE PROCEDURE [dbo].[SP_BuscarPersonaPorCP]
    @CodigoPostal VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.*,
        D.*
    FROM dbo.Personas P
    INNER JOIN dbo.Direcciones D
        ON P.IdDireccion = D.IdDireccion
    WHERE D.CodigoPostal = @CodigoPostal;
END;
GO

CREATE PROCEDURE [dbo].[SP_BuscarPersonaPorLocalidad]
    @Localidad VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.*,
        D.*
    FROM dbo.Personas P
    INNER JOIN dbo.Direcciones D
        ON P.IdDireccion = D.IdDireccion
    WHERE D.Localidad LIKE '%' + @Localidad + '%';
END;
GO

CREATE PROCEDURE [dbo].[SP_BuscarPersonaPorPartido]
    @Partido VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.*,
        D.*
    FROM dbo.Personas P
    INNER JOIN dbo.Direcciones D
        ON P.IdDireccion = D.IdDireccion
    WHERE D.Partido LIKE '%' + @Partido + '%';
END;
GO

CREATE PROCEDURE [dbo].[SP_BuscarPersonaPorProvincia]
    @Provincia VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.*,
        D.*
    FROM dbo.Personas P
    INNER JOIN dbo.Direcciones D
        ON P.IdDireccion = D.IdDireccion
    WHERE D.Provincia LIKE '%' + @Provincia + '%';
END;
GO

CREATE PROCEDURE [dbo].[SP_ListarRoles]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdRol,
        NombreRol
    FROM dbo.Roles
    ORDER BY NombreRol;
END;
GO

CREATE PROCEDURE [dbo].[SP_ExisteUsuario]
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM dbo.Usuarios
        WHERE Usuario = @Usuario
    )
    BEGIN
        SELECT 1 AS Existe;
    END
    ELSE
    BEGIN
        SELECT 0 AS Existe;
    END;
END;
GO

CREATE PROCEDURE [dbo].[SP_ExisteCorreo]
    @Correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM dbo.Usuarios
        WHERE Correo = @Correo
    )
    BEGIN
        SELECT 1 AS Existe;
    END
    ELSE
    BEGIN
        SELECT 0 AS Existe;
    END;
END;
GO

CREATE PROCEDURE [dbo].[SP_ObtenerCorreoPorUsuario]
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Correo
    FROM dbo.Usuarios
    WHERE Usuario = @Usuario;
END;
GO

CREATE PROCEDURE [dbo].[SP_ObtenerRolPorUsuario]
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdRol
    FROM dbo.Usuarios
    WHERE Usuario = @Usuario;
END;
GO

-- ----------------------------------------------------------------------------
-- NUEVO (hallazgo 3 / T01): CD_usuariosDatos.ObtenerRolUsuario invoca
-- "dbo.SP_ObtenerRolUsuario" (sin "Por"), que no existía. Se agrega con el
-- nombre exacto que espera el C#, en vez de renombrar SP_ObtenerRolPorUsuario,
-- para no romper nada que dependa del nombre anterior.
-- ----------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[SP_ObtenerRolUsuario]
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdRol
    FROM dbo.Usuarios
    WHERE Usuario = @Usuario;
END;
GO
-- ----------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[SP_BuscarUsuarioPorId]
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.Usuario,
        U.Activo,
        U.PrimerLogin,
        U.Usa2FA,
        U.FechaAlta AS FechaAltaUsuario,
        U.Correo,

        R.IdRol,
        R.NombreRol,

        P.IdPersona,
        P.Nombre,
        P.Apellido,
        P.DNI,
        P.Telefono,
        P.FechaNacimiento,

        D.IdDireccion,
        D.Calle,
        D.Numero,
        D.CodigoPostal,
        D.Depto,
        D.Piso,
        D.Provincia,
        D.Partido,
        D.Localidad,
        D.Latitud,
        D.Longitud,
        D.DireccionOriginal,
        D.DireccionNormalizada
    FROM dbo.Usuarios U
    INNER JOIN dbo.Personas P
        ON U.IdPersona = P.IdPersona
    INNER JOIN dbo.Roles R
        ON U.IdRol = R.IdRol
    LEFT JOIN dbo.Direcciones D
        ON P.IdDireccion = D.IdDireccion
    WHERE U.IdUsuario = @IdUsuario;
END;
GO

CREATE PROCEDURE [dbo].[SP_BuscarUsuarioPorNombreUser]
       @Usuario varchar(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.Usuario,
        U.Activo,
        U.PrimerLogin,
        U.Usa2FA,
        U.FechaAlta AS FechaAltaUsuario,
        U.Correo,

        R.IdRol,
        R.NombreRol,

        P.IdPersona,
        P.Nombre,
        P.Apellido,
        P.DNI,
        P.Telefono,
        P.FechaNacimiento,

        D.IdDireccion,
        D.Calle,
        D.Numero,
        D.CodigoPostal,
        D.Depto,
        D.Piso,
        D.Provincia,
        D.Partido,
        D.Localidad,
        D.Latitud,
        D.Longitud,
        D.DireccionOriginal,
        D.DireccionNormalizada
    FROM dbo.Usuarios U
    INNER JOIN dbo.Personas P
        ON U.IdPersona = P.IdPersona
    INNER JOIN dbo.Roles R
        ON U.IdRol = R.IdRol
    LEFT JOIN dbo.Direcciones D
        ON P.IdDireccion = D.IdDireccion
    WHERE U.Usuario = @Usuario;
END;
GO

CREATE PROCEDURE [dbo].[SP_BuscarUsuarioPorCorreo]
    @Correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Usuarios
    WHERE Correo LIKE '%' + @Correo + '%';
END;
GO

CREATE PROCEDURE [dbo].[SP_BuscarUsuariosGrilla]
(
    @Busqueda NVARCHAR(100) = '',
    @Activo BIT = NULL,
    @IdRol INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.IdPersona,
        P.IdDireccion,
        P.Nombre,
        P.Apellido,
        P.DNI,
        P.Telefono,
        P.FechaNacimiento,
        U.Usuario,
        U.Correo,
        U.IdRol,
        R.NombreRol AS Rol,
        U.Activo,
        CASE
            WHEN U.Activo = 1 THEN 'Activo'
            ELSE 'Inactivo'
        END AS Estado,
        D.Calle,
        D.Numero,
        D.CodigoPostal,
        D.Depto,
        D.Piso,
        D.Provincia,
        D.Partido,
        D.Localidad
    FROM dbo.Usuarios U
    INNER JOIN dbo.Personas P
        ON U.IdPersona = P.IdPersona
    LEFT JOIN dbo.Direcciones D
        ON P.IdDireccion = D.IdDireccion
    LEFT JOIN dbo.Roles R
        ON U.IdRol = R.IdRol
    WHERE
    (
        @Busqueda = ''
        OR P.Nombre LIKE '%' + @Busqueda + '%'
        OR P.Apellido LIKE '%' + @Busqueda + '%'
        OR CAST(P.DNI AS VARCHAR(20)) LIKE '%' + @Busqueda + '%'
        OR P.Telefono LIKE '%' + @Busqueda + '%'
        OR U.Usuario LIKE '%' + @Busqueda + '%'
        OR U.Correo LIKE '%' + @Busqueda + '%'
        OR R.NombreRol LIKE '%' + @Busqueda + '%'
        OR D.Calle LIKE '%' + @Busqueda + '%'
        OR CAST(D.Numero AS VARCHAR(20)) LIKE '%' + @Busqueda + '%'
        OR D.CodigoPostal LIKE '%' + @Busqueda + '%'
        OR D.Depto LIKE '%' + @Busqueda + '%'
        OR D.Piso LIKE '%' + @Busqueda + '%'
        OR D.Provincia LIKE '%' + @Busqueda + '%'
        OR D.Partido LIKE '%' + @Busqueda + '%'
        OR D.Localidad LIKE '%' + @Busqueda + '%'
    )
    AND
    (
        @Activo IS NULL
        OR U.Activo = @Activo
    )
    AND
    (
        @IdRol IS NULL
        OR U.IdRol = @IdRol
    )
    ORDER BY U.IdUsuario;
END;
GO

CREATE PROCEDURE [dbo].[SP_ObtenerUsuariosGrilla]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.IdPersona,
        P.IdDireccion,

        P.Nombre,
        P.Apellido,
        P.DNI,
        P.Telefono,
        P.FechaNacimiento,

        U.Usuario,
        U.Correo,
        U.IdRol,
        R.NombreRol AS Rol,
        U.Activo,
        CASE
            WHEN U.Activo = 1 THEN 'Activo'
            ELSE 'Inactivo'
        END AS Estado,

        D.Calle,
        D.Numero,
        D.CodigoPostal,
        D.Depto,
        D.Piso,
        D.Provincia,
        D.Partido,
        D.Localidad
    FROM dbo.Usuarios U
    INNER JOIN dbo.Personas P
        ON U.IdPersona = P.IdPersona
    LEFT JOIN dbo.Direcciones D
        ON P.IdDireccion = D.IdDireccion
    LEFT JOIN dbo.Roles R
        ON U.IdRol = R.IdRol
    ORDER BY U.IdUsuario;
END;
GO

-- NOTA (hallazgo 6 / T04): antes hacía SELECT 1 AS Resultado ANTES del
-- UPDATE, por lo que siempre devolvía éxito aunque el IdUsuario no
-- existiera o no se modificara ninguna fila. Ahora hace el UPDATE primero
-- y devuelve @@ROWCOUNT real. El C# (CD_usuariosDatos.CambiarEstadoUsuario)
-- ya esperaba un escalar 0/1 llamado "Resultado", así que no requiere
-- ningún cambio para aprovechar la corrección.
CREATE PROCEDURE [dbo].[SP_ActualizarEstadoUsuario]
    @IdUsuario INT,
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Usuarios
    SET Activo = @Activo
    WHERE IdUsuario = @IdUsuario;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END AS Resultado;
END;
GO

CREATE PROCEDURE [dbo].[SP_AsignarRolUsuario]
    @IdUsuario INT,
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Roles
        WHERE IdRol = @IdRol
    )
    BEGIN
        RAISERROR('El rol indicado no existe.',16,1);
        RETURN;
    END;

    UPDATE dbo.Usuarios
    SET IdRol = @IdRol
    WHERE IdUsuario = @IdUsuario;

    SELECT 'Rol asignado correctamente.' AS Mensaje;
END;
GO

CREATE PROCEDURE [dbo].[SP_ActualizarDatosUsuario]
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

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdPersona INT;
        DECLARE @IdDireccion INT;

        SELECT
            @IdPersona = U.IdPersona
        FROM dbo.Usuarios U
        WHERE U.IdUsuario = @IdUsuario;

        IF @IdPersona IS NULL
        BEGIN
            RAISERROR('El usuario no existe.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        SELECT
            @IdDireccion = P.IdDireccion
        FROM dbo.Personas P
        WHERE P.IdPersona = @IdPersona;

        UPDATE dbo.Personas
        SET
            Nombre = @Nombre,
            Apellido = @Apellido,
            DNI = @DNI,
            Telefono = @Telefono,
            FechaNacimiento = @FechaNacimiento
        WHERE IdPersona = @IdPersona;

        UPDATE dbo.Direcciones
        SET
            Calle = @Calle,
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

        UPDATE dbo.Usuarios
        SET
            Correo = @Correo,
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

CREATE PROCEDURE [dbo].[SP_RegistrarUsuarioCompleto]
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

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdDireccion INT;
        DECLARE @IdPersona INT;
        DECLARE @IdUsuario INT;

        -- 1) Insertar dirección
        INSERT INTO dbo.Direcciones
        (
            Calle,
            Numero,
            CodigoPostal,
            Depto,
            Piso,
            Provincia,
            Partido,
            Localidad,
            Latitud,
            Longitud,
            DireccionOriginal,
            DireccionNormalizada
        )
        VALUES
        (
            @Calle,
            @Numero,
            @CodigoPostal,
            @Depto,
            @Piso,
            @Provincia,
            @Partido,
            @Localidad,
            @Latitud,
            @Longitud,
            @DireccionOriginal,
            @DireccionNormalizada
        );

        SET @IdDireccion = SCOPE_IDENTITY();

        -- 2) Insertar persona
        INSERT INTO dbo.Personas
        (
            Nombre,
            Apellido,
            DNI,
            Telefono,
            FechaNacimiento,
            IdDireccion
        )
        VALUES
        (
            @Nombre,
            @Apellido,
            @DNI,
            @Telefono,
            @FechaNacimiento,
            @IdDireccion
        );

        SET @IdPersona = SCOPE_IDENTITY();

        -- 3) Insertar usuario
        INSERT INTO dbo.Usuarios
        (
            IdPersona,
            Usuario,
            Correo,
            IdRol,
            Activo,
            PrimerLogin,
            Usa2FA
        )
        VALUES
        (
            @IdPersona,
            @Usuario,
            @Correo,
            @IdRol,
            1,
            1,
            @Usa2FA
        );

        SET @IdUsuario = SCOPE_IDENTITY();

        -- 4) Insertar password
        INSERT INTO dbo.Passwords
        (
            IdUsuario,
            PasswordHash,
            FechaCreacion
        )
        VALUES
        (
            @IdUsuario,
            @PasswordHash,
            GETDATE()
        );

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

-- --- Autenticacion y passwords ---
CREATE PROCEDURE [dbo].[SP_VerificarLogin]
    @Usuario VARCHAR(50),
    @PasswordHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.Usuario,
        U.Correo,
        U.Activo,
        U.PrimerLogin,
        U.Usa2FA,

        R.IdRol,
        R.NombreRol,

        P.IdPersona,
        P.Nombre,
        P.Apellido,
        P.DNI
    FROM dbo.Usuarios U
    INNER JOIN dbo.Roles R
        ON U.IdRol = R.IdRol
    INNER JOIN dbo.Personas P
        ON U.IdPersona = P.IdPersona
    INNER JOIN dbo.Passwords Pass
        ON Pass.IdUsuario = U.IdUsuario
    WHERE
        U.Usuario = @Usuario
        AND Pass.PasswordHash = @PasswordHash
        AND Pass.IdPassword = (
            SELECT TOP 1 P2.IdPassword
            FROM dbo.Passwords P2
            WHERE P2.IdUsuario = U.IdUsuario
            ORDER BY P2.FechaCreacion DESC, P2.IdPassword DESC
        );
END;
GO

CREATE PROCEDURE [dbo].[SP_EsPrimerLogin]
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT PrimerLogin
    FROM dbo.Usuarios
    WHERE Usuario = @Usuario;
END;
GO

CREATE PROCEDURE [dbo].[SP_CambiarPassword]
    @IdUsuario INT,
    @NuevaPasswordHash VARCHAR(256),
    @ValidarHistorial BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.Usuarios
            WHERE IdUsuario = @IdUsuario
        )
        BEGIN
            RAISERROR('El usuario no existe.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        IF @ValidarHistorial = 1 AND EXISTS (
            SELECT 1
            FROM dbo.HistorialPasswords
            WHERE IdUsuario = @IdUsuario
              AND PasswordHash = @NuevaPasswordHash
        )
        BEGIN
            RAISERROR('La contraseña ya fue utilizada anteriormente.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        INSERT INTO dbo.Passwords (
            IdUsuario,
            PasswordHash,
            FechaCreacion
        )
        VALUES (
            @IdUsuario,
            @NuevaPasswordHash,
            GETDATE()
        );

        INSERT INTO dbo.HistorialPasswords (
            IdUsuario,
            PasswordHash,
            FechaUso
        )
        VALUES (
            @IdUsuario,
            @NuevaPasswordHash,
            GETDATE()
        );

        UPDATE dbo.Usuarios
        SET PrimerLogin = 0
        WHERE IdUsuario = @IdUsuario;

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

CREATE PROCEDURE [dbo].[SP_VerificarPasswordEnHistorial]
    @IdUsuario INT,
    @PasswordHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM dbo.HistorialPasswords
        WHERE IdUsuario = @IdUsuario
          AND PasswordHash = @PasswordHash
    )
    BEGIN
        SELECT 1 AS Existe;
    END
    ELSE
    BEGIN
        SELECT 0 AS Existe;
    END
END;
GO

-- --- Preguntas de seguridad ---
CREATE PROCEDURE [dbo].[SP_ListarPreguntasSeguridad]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdPregunta,
        Pregunta
    FROM dbo.PreguntasSeguridad
    ORDER BY Pregunta;
END;
GO

CREATE PROCEDURE [dbo].[SP_GuardarRespuestasSeguridad]
    @IdUsuario INT,
    @IdPregunta INT,
    @RespuestaHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- verifica que exista la pregunta
        IF NOT EXISTS (
            SELECT 1
            FROM dbo.PreguntasSeguridad
            WHERE IdPregunta = @IdPregunta
        )
        BEGIN
            RAISERROR('La pregunta de seguridad no existe.',16,1);
            RETURN;
        END;

        -- si ya existe respuesta, la actualiza
        IF EXISTS (
            SELECT 1
            FROM dbo.RespuestasUsuario
            WHERE
                IdUsuario = @IdUsuario
                AND IdPregunta = @IdPregunta
        )
        BEGIN

            UPDATE dbo.RespuestasUsuario
            SET
                RespuestaHash = @RespuestaHash
            WHERE
                IdUsuario = @IdUsuario
                AND IdPregunta = @IdPregunta;

            SELECT 'Respuesta de seguridad actualizada correctamente.' AS Mensaje;

        END
        ELSE
        BEGIN

            INSERT INTO dbo.RespuestasUsuario (
                IdUsuario,
                IdPregunta,
                RespuestaHash
            )
            VALUES (
                @IdUsuario,
                @IdPregunta,
                @RespuestaHash
            );

            SELECT 'Respuesta de seguridad guardada correctamente.' AS Mensaje;

        END

    END TRY
    BEGIN CATCH

        SELECT
            ERROR_MESSAGE() AS Error;

    END CATCH
END;
GO

CREATE PROCEDURE [dbo].[SP_ObtenerPreguntasUsuario]
    @IdUsuario INT
AS
BEGIN
    SELECT
        p.IdPregunta,
        p.Pregunta,
        r.RespuestaHash
    FROM RespuestasUsuario r
    INNER JOIN PreguntasSeguridad p
        ON r.IdPregunta = p.IdPregunta
    WHERE r.IdUsuario = @IdUsuario;
END
GO

CREATE PROCEDURE [dbo].[SP_VerificarRespuestaSeguridad]
    @IdUsuario INT,
    @IdPregunta INT,
    @RespuestaHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM dbo.RespuestasUsuario
        WHERE
            IdUsuario = @IdUsuario
            AND IdPregunta = @IdPregunta
            AND RespuestaHash = @RespuestaHash
    )
    BEGIN

        SELECT
            1 AS Correcta,
            'La respuesta es correcta.' AS Mensaje;

    END
    ELSE
    BEGIN

        SELECT
            0 AS Correcta,
            'La respuesta es incorrecta.' AS Mensaje;

    END
END;
GO

-- --- Politicas de seguridad ---
CREATE PROCEDURE [dbo].[SP_ObtenerPoliticasSeguridad]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 *
    FROM dbo.PoliticasSeguridad;
END;
GO

-- NOTA (hallazgo 9 / T11-DAT): se agrega RequiereMinusculas al SELECT.
-- No requiere cambios de firma (sin parámetros), por lo que es
-- retrocompatible con cualquier llamador existente.
CREATE PROCEDURE [dbo].[SP_ObtenerConfiguracion]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        IdPolitica,
        MinCaracteres,
        CantPreguntas,
        RequiereMayusculas,
        RequiereMinusculas,
        RequiereNumeros,
        RequiereEspeciales,
        Requiere2FA,
        NoRepetirPasswords,
        ValidarDatosPersonales
    FROM dbo.PoliticasSeguridad
    ORDER BY IdPolitica;
END;
GO

-- NOTA (hallazgo 9 / T11-DAT): se agrega @RequiereMinusculas con valor por
-- defecto (= 0) para no romper la llamada actual desde
-- CD_configuracionDatos.cs, que hoy no envía este parámetro. Cuando se
-- actualice el C# (mapeo + checkbox en frmConfigAdmin) para enviarlo, la
-- columna ya va a estar lista para recibir el valor real.
CREATE PROCEDURE [dbo].[SP_ActualizarPoliticasSeguridad]
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

        -- Si no existe registro, lo crea
        IF NOT EXISTS (
            SELECT 1
            FROM dbo.PoliticasSeguridad
        )
        BEGIN

            INSERT INTO dbo.PoliticasSeguridad (
                MinCaracteres,
                CantPreguntas,
                RequiereMayusculas,
                RequiereMinusculas,
                RequiereNumeros,
                RequiereEspeciales,
                Requiere2FA,
                NoRepetirPasswords,
                ValidarDatosPersonales
            )
            VALUES (
                @MinCaracteres,
                @CantPreguntas,
                @RequiereMayusculas,
                @RequiereMinusculas,
                @RequiereNumeros,
                @RequiereEspeciales,
                @Requiere2FA,
                @NoRepetirPasswords,
                @ValidarDatosPersonales
            );

            SELECT 'Políticas de seguridad creadas correctamente.' AS Mensaje;

        END
        ELSE
        BEGIN

            UPDATE dbo.PoliticasSeguridad
            SET
                MinCaracteres = @MinCaracteres,
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

        SELECT
            ERROR_MESSAGE() AS Error;

    END CATCH
END;
GO

-- ============================================================================
-- FASE 2 DEL ROADMAP: procedimientos nuevos
-- Reemplazan el SQL manual que quedaba en CD_contraDatos.cs y en
-- CD_usuariosDatos.UsuarioActivo (T14), y agregan el soporte necesario para
-- la contraseña temporal en recuperación (T10) y la validación de datos
-- personales reales en la contraseña (T12).
-- ============================================================================

CREATE OR ALTER PROCEDURE [dbo].[SP_InsertarPassword]
    @IdUsuario INT,
    @PasswordHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Passwords
    (
        IdUsuario,
        PasswordHash,
        FechaCreacion
    )
    VALUES
    (
        @IdUsuario,
        @PasswordHash,
        GETDATE()
    );
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[SP_ObtenerContrasAnteriores]
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT PasswordHash
    FROM dbo.Passwords
    WHERE IdUsuario = @IdUsuario
    ORDER BY FechaCreacion DESC;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[SP_ObtenerUltimoPasswordHash]
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 PasswordHash
    FROM dbo.Passwords
    WHERE IdUsuario = @IdUsuario
    ORDER BY FechaCreacion DESC;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[SP_VerificarLogin]
    @Usuario VARCHAR(50),
    @PasswordHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*)
    FROM dbo.Usuarios U
    INNER JOIN dbo.Passwords P
        ON U.IdUsuario = P.IdUsuario
    WHERE U.Usuario = @Usuario
      AND U.Activo = 1
      AND P.PasswordHash = @PasswordHash
      AND P.FechaCreacion = (
            SELECT MAX(P2.FechaCreacion)
            FROM dbo.Passwords P2
            WHERE P2.IdUsuario = U.IdUsuario
      );
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[SP_ActualizarPasswordYPrimerLogin]
    @IdUsuario INT,
    @PasswordHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO dbo.Passwords
        (
            IdUsuario,
            PasswordHash,
            FechaCreacion
        )
        VALUES
        (
            @IdUsuario,
            @PasswordHash,
            GETDATE()
        );

        UPDATE dbo.Usuarios
        SET PrimerLogin = 0
        WHERE IdUsuario = @IdUsuario;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[SP_UsuarioActivo]
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Activo
    FROM dbo.Usuarios
    WHERE Usuario = @Usuario;
END;
GO

-- Datos personales reales del usuario, para T12: validar que la contraseña
-- nueva no contenga nombre, apellido, DNI ni año de nacimiento.
CREATE OR ALTER PROCEDURE [dbo].[SP_ObtenerDatosPersonalesPorUsuario]
    @Usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.Nombre,
        P.Apellido,
        P.DNI,
        P.FechaNacimiento
    FROM dbo.Usuarios U
    INNER JOIN dbo.Personas P
        ON U.IdPersona = P.IdPersona
    WHERE U.Usuario = @Usuario;
END;
GO

-- ============================================================================
-- DATOS SEMILLA (hallazgo 7 / T08-BD)
-- Sin esto, una instalación limpia no puede completar el alta de ningún
-- usuario: no hay Roles ni PreguntasSeguridad cargadas.
-- ============================================================================

-- El orden de inserción importa: coincide con el mapeo hardcodeado en
-- frmRegistro.cs (obtenerIdRol(): "Usuario" -> 1, "Administrador" -> 2).
-- T08-VIS (pendiente, lado C#) debería reemplazar ese hardcodeo por una
-- consulta real a SP_ListarRoles.
IF NOT EXISTS (SELECT 1 FROM dbo.Roles)
BEGIN
    INSERT INTO dbo.Roles (NombreRol) VALUES ('Usuario');
    INSERT INTO dbo.Roles (NombreRol) VALUES ('Administrador');
END
GO

-- Se cargan 15 preguntas: en el primer login el usuario responde TODAS
-- (CargarTodasLasPreguntas en frmPregUsuario.cs), y en la recuperación de
-- cuenta se le piden solo CantPreguntas (config de frmConfigAdmin) de las
-- que ya respondió.
IF NOT EXISTS (SELECT 1 FROM dbo.PreguntasSeguridad)
BEGIN
    INSERT INTO dbo.PreguntasSeguridad (Pregunta) VALUES
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
END
GO

-- Fila de PoliticasSeguridad por defecto. Es opcional: SP_ActualizarPoliticasSeguridad
-- ya se auto-crea la fila la primera vez que el administrador guarda la
-- configuración. Se siembra igual para que el sistema tenga una política
-- real desde el primer arranque, sin depender de que el admin entre primero
-- a guardar configuración.
IF NOT EXISTS (SELECT 1 FROM dbo.PoliticasSeguridad)
BEGIN
    INSERT INTO dbo.PoliticasSeguridad DEFAULT VALUES;
END
GO

-- ============================================================================
-- Rotación de credenciales de correo (feature agregada durante el
-- diagnóstico del error 5.7.0 de Gmail SMTP). La contraseña de aplicación
-- ya no vive hardcodeada en Utilidades/enviarMail.cs: se guarda cifrada acá
-- (AES-256, clave fuera del repo en %LocalAppData%\SistemaPP\mail.key,
-- ver Utilidades/CifradoAes.cs), y el administrador la administra desde
-- frmConfigAdmin.
-- ============================================================================

CREATE TABLE [dbo].[ConfiguracionCorreo](
    [IdConfiguracionCorreo] [int] IDENTITY(1,1) NOT NULL,
    [CorreoRemitente] [varchar](100) NOT NULL,
    [PasswordCifrada] [varbinary](max) NOT NULL,
    [IV] [varbinary](16) NOT NULL,
    [FechaUltimaRotacion] [datetime] NOT NULL,
    [DiasRotacion] [int] NOT NULL,
PRIMARY KEY
(
    [IdConfiguracionCorreo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ConfiguracionCorreo] ADD DEFAULT ((90)) FOR [DiasRotacion]
GO
ALTER TABLE [dbo].[ConfiguracionCorreo] ADD DEFAULT (getdate()) FOR [FechaUltimaRotacion]
GO

CREATE PROCEDURE [dbo].[SP_ObtenerConfiguracionCorreo]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        IdConfiguracionCorreo,
        CorreoRemitente,
        PasswordCifrada,
        IV,
        FechaUltimaRotacion,
        DiasRotacion
    FROM dbo.ConfiguracionCorreo
    ORDER BY IdConfiguracionCorreo;
END;
GO

-- @PasswordCifrada / @IV en NULL significa "no tocar la contraseña, solo
-- actualizar correo remitente y días de rotación" (no usado hoy desde la
-- UI, que siempre pide los tres campos juntos, pero el SP queda preparado
-- por si se separa en dos acciones más adelante).
CREATE PROCEDURE [dbo].[SP_GuardarConfiguracionCorreo]
    @CorreoRemitente VARCHAR(100),
    @DiasRotacion INT,
    @PasswordCifrada VARBINARY(MAX) = NULL,
    @IV VARBINARY(16) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.ConfiguracionCorreo)
    BEGIN
        IF @PasswordCifrada IS NULL
        BEGIN
            RAISERROR('Debe cargar una contraseña la primera vez.', 16, 1);
            RETURN;
        END

        INSERT INTO dbo.ConfiguracionCorreo (CorreoRemitente, PasswordCifrada, IV, FechaUltimaRotacion, DiasRotacion)
        VALUES (@CorreoRemitente, @PasswordCifrada, @IV, GETDATE(), @DiasRotacion);
    END
    ELSE
    BEGIN
        IF @PasswordCifrada IS NOT NULL
        BEGIN
            UPDATE dbo.ConfiguracionCorreo
            SET CorreoRemitente = @CorreoRemitente,
                PasswordCifrada = @PasswordCifrada,
                IV = @IV,
                DiasRotacion = @DiasRotacion,
                FechaUltimaRotacion = GETDATE();
        END
        ELSE
        BEGIN
            UPDATE dbo.ConfiguracionCorreo
            SET CorreoRemitente = @CorreoRemitente,
                DiasRotacion = @DiasRotacion;
        END
    END

    SELECT 'Configuración de correo guardada correctamente.' AS Mensaje;
END;
GO
