-- ============================================================================
-- GRUPO 1 — Catálogo de Productos, Stock y Auditoría
-- Trabajo Práctico Integrador — Sistema de Gestión Empresarial 2026
-- ============================================================================
-- Este script crea las 4 tablas de este grupo y sus stored procedures.
--
-- ATENCIÓN: después de correr este script hay que correr Grupo1_Correcciones.sql,
-- que reemplaza varios de los procedures de abajo (auditoría dentro de la
-- transacción, kardex con saldo, reportes con nombres, etc.), agrega los
-- triggers de protección y elimina SP_ActualizarStockActual.
--
-- IMPORTANTE: para que las FOREIGN KEY de más abajo funcionen, ya tienen que
-- existir en la base:
--   - dbo.Categorias (con PK IdCategoria)
--   - dbo.Marcas     (con PK IdMarca)
--   - dbo.Usuarios   (con PK IdUsuario) -- esta ya existe en SistemaPP
-- Categorias y Marcas son de otro grupo. Si todavía no las armaron y querés
-- probar esto en tu máquina, al final del script hay dos tablas "de prueba"
-- comentadas que podés descomentar solo para testear en local.
-- ============================================================================

-- ----------------------------------------------------------------------------
-- Productos
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Productos (
    ID_Codigo           INT IDENTITY(1,1) NOT NULL,
    FK_Categoria        INT NOT NULL,
    FK_Marca            INT NOT NULL,
    Nombre              VARCHAR(150) NOT NULL,
    Precio_Compra       DECIMAL(18,2) NOT NULL,
    Precio_Venta        DECIMAL(18,2) NOT NULL,
    Stock_Actual        INT NOT NULL DEFAULT 0,
    Stock_Critico       INT NOT NULL,
    Stock_Minimo        INT NOT NULL,
    Stock_Ideal         INT NOT NULL,
    Stock_Maximo        INT NOT NULL,
    Punto_Reposicion    INT NOT NULL,
    Numero_Lote         VARCHAR(50)  NULL,
    Fecha_Vencimiento   DATE         NULL,
    Estado              VARCHAR(30)  NOT NULL DEFAULT 'Activo',
    Habilitado          BIT          NOT NULL DEFAULT 1,

    CONSTRAINT PK_Productos PRIMARY KEY (ID_Codigo),

    CONSTRAINT FK_Productos_Categoria
        FOREIGN KEY (FK_Categoria) REFERENCES dbo.Categorias(IdCategoria),
    CONSTRAINT FK_Productos_Marca
        FOREIGN KEY (FK_Marca) REFERENCES dbo.Marcas(IdMarca),

    CONSTRAINT CK_Productos_StockActual
        CHECK (Stock_Actual >= 0),
    CONSTRAINT CK_Productos_OrdenStock
        CHECK (Stock_Critico < Stock_Minimo AND Stock_Minimo < Stock_Ideal AND Stock_Ideal < Stock_Maximo)
);
GO

-- ----------------------------------------------------------------------------
-- Movimientos_Stock
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Movimientos_Stock (
    ID                     INT IDENTITY(1,1) NOT NULL,
    FK_Producto            INT NOT NULL,
    FK_Usuario             INT NOT NULL,
    Tipo_Movimiento        VARCHAR(20) NOT NULL,
    Cantidad               INT NOT NULL,
    Fecha                  DATETIME NOT NULL DEFAULT GETDATE(),
    Tipo_Documento_Origen  VARCHAR(50) NULL,
    ID_Documento_Origen    INT NULL,

    CONSTRAINT PK_MovimientosStock PRIMARY KEY (ID),

    CONSTRAINT FK_MovStock_Producto
        FOREIGN KEY (FK_Producto) REFERENCES dbo.Productos(ID_Codigo),
    CONSTRAINT FK_MovStock_Usuario
        FOREIGN KEY (FK_Usuario) REFERENCES dbo.Usuarios(IdUsuario),

    CONSTRAINT CK_MovStock_Cantidad
        CHECK (Cantidad > 0),
    CONSTRAINT CK_MovStock_Tipo
        CHECK (Tipo_Movimiento IN ('Ingreso', 'Egreso', 'Ajuste'))
);
GO

-- ----------------------------------------------------------------------------
-- Registros_Scrap
-- Nota: se agregó "Fecha" porque el reporte "Scrap del período" la necesita
-- y el enunciado original no la incluía en la lista de campos.
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Registros_Scrap (
    ID                 INT IDENTITY(1,1) NOT NULL,
    FK_Producto        INT NOT NULL,
    FK_Usuario         INT NOT NULL,
    Cantidad           INT NOT NULL,
    Motivo             VARCHAR(200) NOT NULL,
    Costo_Valorizado   DECIMAL(18,2) NOT NULL,
    Fecha              DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_RegistrosScrap PRIMARY KEY (ID),

    CONSTRAINT FK_Scrap_Producto
        FOREIGN KEY (FK_Producto) REFERENCES dbo.Productos(ID_Codigo),
    CONSTRAINT FK_Scrap_Usuario
        FOREIGN KEY (FK_Usuario) REFERENCES dbo.Usuarios(IdUsuario),

    CONSTRAINT CK_Scrap_Cantidad
        CHECK (Cantidad > 0)
);
GO

-- ----------------------------------------------------------------------------
-- Auditoria_Cambios
-- Tabla_Afectada / ID_Registro_Afectado NO son FK real a propósito: tienen
-- que poder señalar una fila de cualquier otra tabla del sistema.
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Auditoria_Cambios (
    ID                      INT IDENTITY(1,1) NOT NULL,
    Tabla_Afectada          VARCHAR(100) NOT NULL,
    ID_Registro_Afectado    INT NOT NULL,
    Accion                  VARCHAR(20) NOT NULL,
    FK_Usuario              INT NOT NULL,
    Fecha_Hora              DATETIME NOT NULL DEFAULT GETDATE(),
    Detalle_Cambio          VARCHAR(MAX) NULL,

    CONSTRAINT PK_AuditoriaCambios PRIMARY KEY (ID),

    CONSTRAINT FK_Auditoria_Usuario
        FOREIGN KEY (FK_Usuario) REFERENCES dbo.Usuarios(IdUsuario),

    CONSTRAINT CK_Auditoria_Accion
        CHECK (Accion IN ('Alta', 'Modificación', 'Baja'))
);
GO

-- ============================================================================
-- STORED PROCEDURES — Productos
-- ============================================================================

-- Lista de productos para la grilla principal. @SoloHabilitados = 1 trae solo
-- los que no fueron dados de baja (que es el caso normal de uso).
CREATE OR ALTER PROCEDURE dbo.SP_ObtenerProductos
    @SoloHabilitados BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID_Codigo, FK_Categoria, FK_Marca, Nombre, Precio_Compra, Precio_Venta,
           Stock_Actual, Stock_Critico, Stock_Minimo, Stock_Ideal, Stock_Maximo,
           Punto_Reposicion, Numero_Lote, Fecha_Vencimiento, Estado, Habilitado
    FROM dbo.Productos
    WHERE (@SoloHabilitados = 0 OR Habilitado = 1)
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_ObtenerProductoPorId
    @ID_Codigo INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID_Codigo, FK_Categoria, FK_Marca, Nombre, Precio_Compra, Precio_Venta,
           Stock_Actual, Stock_Critico, Stock_Minimo, Stock_Ideal, Stock_Maximo,
           Punto_Reposicion, Numero_Lote, Fecha_Vencimiento, Estado, Habilitado
    FROM dbo.Productos
    WHERE ID_Codigo = @ID_Codigo;
END;
GO

-- Buscador con filtros opcionales: pasá NULL en los que no quieras usar.
CREATE OR ALTER PROCEDURE dbo.SP_BuscarProductos
    @Texto        VARCHAR(150) = NULL,
    @FK_Categoria INT = NULL,
    @FK_Marca     INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID_Codigo, FK_Categoria, FK_Marca, Nombre, Precio_Compra, Precio_Venta,
           Stock_Actual, Stock_Critico, Stock_Minimo, Stock_Ideal, Stock_Maximo,
           Punto_Reposicion, Numero_Lote, Fecha_Vencimiento, Estado, Habilitado
    FROM dbo.Productos
    WHERE Habilitado = 1
      AND (@Texto IS NULL OR Nombre LIKE '%' + @Texto + '%')
      AND (@FK_Categoria IS NULL OR FK_Categoria = @FK_Categoria)
      AND (@FK_Marca IS NULL OR FK_Marca = @FK_Marca)
    ORDER BY Nombre;
END;
GO

-- Alta de producto. La validación de Critico<Minimo<Ideal<Maximo también
-- está como CHECK en la tabla, pero primero tiene que rechazarla la capa de
-- Lógica en C# — el CHECK es solo la última red de seguridad.
CREATE OR ALTER PROCEDURE dbo.SP_InsertarProducto
    @FK_Categoria      INT,
    @FK_Marca          INT,
    @Nombre            VARCHAR(150),
    @Precio_Compra     DECIMAL(18,2),
    @Precio_Venta      DECIMAL(18,2),
    @Stock_Critico     INT,
    @Stock_Minimo      INT,
    @Stock_Ideal       INT,
    @Stock_Maximo      INT,
    @Punto_Reposicion  INT,
    @Numero_Lote       VARCHAR(50) = NULL,
    @Fecha_Vencimiento DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Productos
        (FK_Categoria, FK_Marca, Nombre, Precio_Compra, Precio_Venta,
         Stock_Actual, Stock_Critico, Stock_Minimo, Stock_Ideal, Stock_Maximo,
         Punto_Reposicion, Numero_Lote, Fecha_Vencimiento, Estado, Habilitado)
    VALUES
        (@FK_Categoria, @FK_Marca, @Nombre, @Precio_Compra, @Precio_Venta,
         0, @Stock_Critico, @Stock_Minimo, @Stock_Ideal, @Stock_Maximo,
         @Punto_Reposicion, @Numero_Lote, @Fecha_Vencimiento, 'Activo', 1);

    SELECT SCOPE_IDENTITY() AS ID_Codigo;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_ActualizarProducto
    @ID_Codigo         INT,
    @FK_Categoria      INT,
    @FK_Marca          INT,
    @Nombre            VARCHAR(150),
    @Precio_Compra     DECIMAL(18,2),
    @Precio_Venta      DECIMAL(18,2),
    @Stock_Critico     INT,
    @Stock_Minimo      INT,
    @Stock_Ideal       INT,
    @Stock_Maximo      INT,
    @Punto_Reposicion  INT,
    @Numero_Lote       VARCHAR(50) = NULL,
    @Fecha_Vencimiento DATE = NULL,
    @Estado            VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Productos
    SET FK_Categoria      = @FK_Categoria,
        FK_Marca          = @FK_Marca,
        Nombre            = @Nombre,
        Precio_Compra     = @Precio_Compra,
        Precio_Venta      = @Precio_Venta,
        Stock_Critico     = @Stock_Critico,
        Stock_Minimo      = @Stock_Minimo,
        Stock_Ideal       = @Stock_Ideal,
        Stock_Maximo      = @Stock_Maximo,
        Punto_Reposicion  = @Punto_Reposicion,
        Numero_Lote       = @Numero_Lote,
        Fecha_Vencimiento = @Fecha_Vencimiento,
        Estado            = @Estado
    WHERE ID_Codigo = @ID_Codigo;

    SELECT @@ROWCOUNT AS Filas_Afectadas;
END;
GO

-- Baja lógica: nunca se borra la fila (otras tablas todavía pueden estar
-- apuntándole por FK, y perderíamos el historial de movimientos/scrap).
CREATE OR ALTER PROCEDURE dbo.SP_DarBajaProducto
    @ID_Codigo INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Productos
    SET Habilitado = 0
    WHERE ID_Codigo = @ID_Codigo;

    SELECT @@ROWCOUNT AS Filas_Afectadas;
END;
GO

-- Solo para uso interno: la llaman SP_InsertarMovimientoStock y
-- SP_InsertarScrap, nunca la Vista directamente.
CREATE OR ALTER PROCEDURE dbo.SP_ActualizarStockActual
    @ID_Codigo    INT,
    @Nuevo_Stock  INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Productos
    SET Stock_Actual = @Nuevo_Stock
    WHERE ID_Codigo = @ID_Codigo;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_ObtenerProductosBajoPuntoReposicion
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID_Codigo, Nombre, Stock_Actual, Punto_Reposicion
    FROM dbo.Productos
    WHERE Habilitado = 1
      AND Stock_Actual < Punto_Reposicion
    ORDER BY (Punto_Reposicion - Stock_Actual) DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_ObtenerProductosProximosAVencer
    @DiasAnticipacion INT = 30
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID_Codigo, Nombre, Numero_Lote, Fecha_Vencimiento
    FROM dbo.Productos
    WHERE Habilitado = 1
      AND Fecha_Vencimiento IS NOT NULL
      AND Fecha_Vencimiento BETWEEN GETDATE() AND DATEADD(DAY, @DiasAnticipacion, GETDATE())
    ORDER BY Fecha_Vencimiento;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_ReporteStockPorCategoria
AS
BEGIN
    SET NOCOUNT ON;

    SELECT FK_Categoria, COUNT(*) AS Cantidad_Productos, SUM(Stock_Actual) AS Stock_Total
    FROM dbo.Productos
    WHERE Habilitado = 1
    GROUP BY FK_Categoria;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_ReporteStockPorMarca
AS
BEGIN
    SET NOCOUNT ON;

    SELECT FK_Marca, COUNT(*) AS Cantidad_Productos, SUM(Stock_Actual) AS Stock_Total
    FROM dbo.Productos
    WHERE Habilitado = 1
    GROUP BY FK_Marca;
END;
GO

-- ============================================================================
-- STORED PROCEDURES — Movimientos_Stock
-- ============================================================================

-- Kardex de un producto. @Desde/@Hasta en NULL trae todo el historial.
CREATE OR ALTER PROCEDURE dbo.SP_ObtenerMovimientosPorProducto
    @FK_Producto INT,
    @Desde       DATETIME = NULL,
    @Hasta       DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID, FK_Producto, FK_Usuario, Tipo_Movimiento, Cantidad, Fecha,
           Tipo_Documento_Origen, ID_Documento_Origen
    FROM dbo.Movimientos_Stock
    WHERE FK_Producto = @FK_Producto
      AND (@Desde IS NULL OR Fecha >= @Desde)
      AND (@Hasta IS NULL OR Fecha <= @Hasta)
    ORDER BY Fecha DESC;
END;
GO

-- Registra el movimiento Y actualiza el stock del producto, las dos cosas
-- como una sola operación (transacción): si algo falla, no queda ni el
-- movimiento guardado ni el stock a medio actualizar.
--
-- Decisión de diseño (consultar con el profe, no está en la consigna):
-- 'Ingreso' suma al stock; 'Egreso' y 'Ajuste' restan. Si en tu caso un
-- Ajuste puede ser tanto positivo como negativo, este SP hay que
-- extenderlo con un parámetro extra que indique el signo.
CREATE OR ALTER PROCEDURE dbo.SP_InsertarMovimientoStock
    @FK_Producto            INT,
    @FK_Usuario              INT,
    @Tipo_Movimiento         VARCHAR(20),
    @Cantidad                INT,
    @Tipo_Documento_Origen   VARCHAR(50) = NULL,
    @ID_Documento_Origen     INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @StockActual INT;
        DECLARE @StockNuevo INT;

        SELECT @StockActual = Stock_Actual
        FROM dbo.Productos WITH (UPDLOCK, HOLDLOCK)
        WHERE ID_Codigo = @FK_Producto;

        IF @StockActual IS NULL
        BEGIN
            RAISERROR('El producto no existe.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        IF @Tipo_Movimiento = 'Ingreso'
            SET @StockNuevo = @StockActual + @Cantidad;
        ELSE
            SET @StockNuevo = @StockActual - @Cantidad;

        IF @StockNuevo < 0
        BEGIN
            RAISERROR('El movimiento dejaría el stock en negativo.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        INSERT INTO dbo.Movimientos_Stock
            (FK_Producto, FK_Usuario, Tipo_Movimiento, Cantidad, Fecha,
             Tipo_Documento_Origen, ID_Documento_Origen)
        VALUES
            (@FK_Producto, @FK_Usuario, @Tipo_Movimiento, @Cantidad, GETDATE(),
             @Tipo_Documento_Origen, @ID_Documento_Origen);

        UPDATE dbo.Productos
        SET Stock_Actual = @StockNuevo
        WHERE ID_Codigo = @FK_Producto;

        COMMIT TRANSACTION;

        SELECT SCOPE_IDENTITY() AS ID, @StockNuevo AS Stock_Resultante;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT ERROR_MESSAGE() AS Error;
    END CATCH
END;
GO

-- ============================================================================
-- STORED PROCEDURES — Registros_Scrap
-- ============================================================================

CREATE OR ALTER PROCEDURE dbo.SP_ObtenerScrap
    @Desde DATETIME = NULL,
    @Hasta DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID, FK_Producto, FK_Usuario, Cantidad, Motivo, Costo_Valorizado, Fecha
    FROM dbo.Registros_Scrap
    WHERE (@Desde IS NULL OR Fecha >= @Desde)
      AND (@Hasta IS NULL OR Fecha <= @Hasta)
    ORDER BY Fecha DESC;
END;
GO

-- Igual que el movimiento: inserta el scrap y descuenta el stock en una
-- sola transacción. Costo_Valorizado se calcula ACÁ, con el Precio_Compra
-- que tiene el producto en este momento — no se recibe por parámetro,
-- para que no dependa de qué precio haya leído la capa de Lógica antes.
CREATE OR ALTER PROCEDURE dbo.SP_InsertarScrap
    @FK_Producto INT,
    @FK_Usuario  INT,
    @Cantidad    INT,
    @Motivo      VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @StockActual INT;
        DECLARE @PrecioCompra DECIMAL(18,2);
        DECLARE @StockNuevo INT;
        DECLARE @CostoValorizado DECIMAL(18,2);

        SELECT @StockActual = Stock_Actual, @PrecioCompra = Precio_Compra
        FROM dbo.Productos WITH (UPDLOCK, HOLDLOCK)
        WHERE ID_Codigo = @FK_Producto;

        IF @StockActual IS NULL
        BEGIN
            RAISERROR('El producto no existe.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        SET @StockNuevo = @StockActual - @Cantidad;

        IF @StockNuevo < 0
        BEGIN
            RAISERROR('No hay stock suficiente para dar de baja esa cantidad.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        SET @CostoValorizado = @Cantidad * @PrecioCompra;

        INSERT INTO dbo.Registros_Scrap
            (FK_Producto, FK_Usuario, Cantidad, Motivo, Costo_Valorizado, Fecha)
        VALUES
            (@FK_Producto, @FK_Usuario, @Cantidad, @Motivo, @CostoValorizado, GETDATE());

        UPDATE dbo.Productos
        SET Stock_Actual = @StockNuevo
        WHERE ID_Codigo = @FK_Producto;

        COMMIT TRANSACTION;

        SELECT SCOPE_IDENTITY() AS ID, @StockNuevo AS Stock_Resultante, @CostoValorizado AS Costo_Valorizado;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT ERROR_MESSAGE() AS Error;
    END CATCH
END;
GO

-- ============================================================================
-- STORED PROCEDURES — Auditoria_Cambios
-- Estos dos SPs son los que va a llamar Utilidades/Auditor.cs, no solo
-- Grupo 1 — cualquier grupo que quiera dejar rastro de un cambio pasa por acá.
-- ============================================================================

CREATE OR ALTER PROCEDURE dbo.SP_RegistrarCambioAuditoria
    @Tabla_Afectada       VARCHAR(100),
    @ID_Registro_Afectado INT,
    @Accion               VARCHAR(20),
    @FK_Usuario           INT,
    @Detalle_Cambio       VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Auditoria_Cambios
        (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
    VALUES
        (@Tabla_Afectada, @ID_Registro_Afectado, @Accion, @FK_Usuario, GETDATE(), @Detalle_Cambio);
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_ObtenerAuditoria
    @Tabla_Afectada VARCHAR(100) = NULL,
    @FK_Usuario     INT = NULL,
    @Desde          DATETIME = NULL,
    @Hasta          DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID, Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio
    FROM dbo.Auditoria_Cambios
    WHERE (@Tabla_Afectada IS NULL OR Tabla_Afectada = @Tabla_Afectada)
      AND (@FK_Usuario IS NULL OR FK_Usuario = @FK_Usuario)
      AND (@Desde IS NULL OR Fecha_Hora >= @Desde)
      AND (@Hasta IS NULL OR Fecha_Hora <= @Hasta)
    ORDER BY Fecha_Hora DESC;
END;
GO

-- ============================================================================
-- SOLO PARA PROBAR EN TU MÁQUINA — no es parte de lo que entrega Grupo 1.
-- Si Categorias/Marcas todavía no las armó el grupo que las tiene asignadas,
-- descomentá esto para poder correr el script completo y probar en local.
-- Cuando el otro grupo entregue las suyas, borrá estas dos y usá las reales.
-- ============================================================================

-- CREATE TABLE dbo.Categorias (
--     IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
--     Nombre      VARCHAR(100) NOT NULL
-- );
-- GO
--
-- CREATE TABLE dbo.Marcas (
--     IdMarca INT IDENTITY(1,1) PRIMARY KEY,
--     Nombre  VARCHAR(100) NOT NULL
-- );
-- GO
