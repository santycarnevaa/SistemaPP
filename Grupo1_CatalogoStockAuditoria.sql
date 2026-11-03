-- Grupo 1: Catalogo de productos, stock, scrap y auditoria
-- Correr DESPUES de SistemaUsuariosDB.sql (usa la tabla Usuarios)
-- Este script ya incluye las correcciones, no hace falta correr otro mas

USE SistemaUsuariosDB;
GO

-- ============================================================
-- TABLAS
-- ============================================================

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
    -- tiene que cumplirse: critico < minimo < ideal < maximo
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
    -- el Ajuste puede ser positivo o negativo, los otros dos siempre positivos
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

-- Tabla_Afectada e ID_Registro_Afectado no son FK porque pueden
-- apuntar a cualquier tabla del sistema
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

-- ============================================================
-- TRIGGERS
-- ============================================================

-- La auditoria no se puede modificar ni borrar
CREATE OR ALTER TRIGGER TR_Auditoria_Inmutable
ON Auditoria_Cambios
INSTEAD OF UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    THROW 50020, 'Auditoria_Cambios es de solo escritura: no se puede modificar ni borrar un registro.', 1;
END;
GO

-- El Stock_Actual solo se puede cambiar desde SP_InsertarMovimientoStock y SP_InsertarScrap.
-- Esos SP avisan con una variable de sesion (g1_permitir_stock) para que el trigger los deje pasar
CREATE OR ALTER TRIGGER TR_Productos_ProtegerStock
ON Productos
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF ISNULL(TRY_CAST(SESSION_CONTEXT(N'g1_permitir_stock') AS INT), 0) = 1
        RETURN;

    -- si es un insert (no hay filas en deleted) el stock tiene que arrancar en 0
    IF NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        IF EXISTS (SELECT 1 FROM inserted WHERE Stock_Actual <> 0)
            THROW 50010, 'Stock_Actual solo puede cambiar mediante un movimiento de stock o un registro de scrap.', 1;
    END
    ELSE IF UPDATE(Stock_Actual)
         AND EXISTS (SELECT 1 FROM inserted i
                     INNER JOIN deleted d ON d.ID_Codigo = i.ID_Codigo
                     WHERE i.Stock_Actual <> d.Stock_Actual)
    BEGIN
        THROW 50010, 'Stock_Actual solo puede cambiar mediante un movimiento de stock o un registro de scrap.', 1;
    END
END;
GO

-- ============================================================
-- PROCEDIMIENTOS: productos
-- ============================================================

-- Lista para la grilla. Con @SoloHabilitados = 1 no trae los dados de baja
CREATE OR ALTER PROCEDURE SP_ObtenerProductos
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

CREATE OR ALTER PROCEDURE SP_ObtenerProductoPorId
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

-- Los filtros son opcionales, si vienen en NULL no se usan
CREATE OR ALTER PROCEDURE SP_BuscarProductos
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

-- Alta de producto. Guarda el producto y la auditoria en la misma transaccion
CREATE OR ALTER PROCEDURE SP_InsertarProducto
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

        -- el stock siempre arranca en 0, despues se carga con un movimiento
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

-- Modifica el producto y en la auditoria deja que campos cambiaron
CREATE OR ALTER PROCEDURE SP_ActualizarProducto
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

        -- guardo los valores viejos para ver que cambio
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

-- Baja logica: no se borra la fila para no perder el historial de movimientos
CREATE OR ALTER PROCEDURE SP_DarBajaProducto
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

CREATE OR ALTER PROCEDURE SP_ObtenerProductosBajoPuntoReposicion
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

-- Productos que vencen entre hoy y hoy + @DiasAnticipacion
CREATE OR ALTER PROCEDURE SP_ObtenerProductosProximosAVencer
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

CREATE OR ALTER PROCEDURE SP_ReporteStockPorCategoria
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

CREATE OR ALTER PROCEDURE SP_ReporteStockPorMarca
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

-- ============================================================
-- PROCEDIMIENTOS: movimientos de stock
-- ============================================================

-- Kardex del producto en orden de fecha, con el stock que quedo despues de cada movimiento
CREATE OR ALTER PROCEDURE SP_ObtenerMovimientosPorProducto
    @FK_Producto INT,
    @Desde DATETIME = NULL,
    @Hasta DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    WITH Kardex AS (
        SELECT m.ID, m.FK_Producto, p.Nombre AS Producto, m.FK_Usuario, u.Usuario,
               m.Tipo_Movimiento, m.Cantidad, m.Fecha, m.Tipo_Documento_Origen, m.ID_Documento_Origen,
               -- el egreso resta, el ingreso suma y el ajuste ya viene con signo
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

-- Registra el movimiento, actualiza el stock y deja la auditoria, todo en una transaccion.
-- Ingreso suma, Egreso resta y Ajuste suma la cantidad con su signo.
-- Si falla devuelve una columna "Error" (el C# la lee)
CREATE OR ALTER PROCEDURE SP_InsertarMovimientoStock
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

        -- le aviso al trigger que este cambio de stock esta permitido
        EXEC sp_set_session_context N'g1_permitir_stock', 1;
        UPDATE Productos SET Stock_Actual = @StockNuevo WHERE ID_Codigo = @FK_Producto;
        EXEC sp_set_session_context N'g1_permitir_stock', NULL;

        INSERT INTO Auditoria_Cambios (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
        VALUES ('Movimientos_Stock', @IdMov, 'Alta', @FK_Usuario, GETDATE(),
                CONCAT(@Tipo_Movimiento, ' de ', @Cantidad, ' unidades del producto ', @FK_Producto,
                       ' - stock ', @StockActual, ' -> ', @StockNuevo));

        COMMIT TRANSACTION;

        SELECT @IdMov AS ID, @StockNuevo AS Stock_Resultante;
    END TRY
    BEGIN CATCH
        EXEC sp_set_session_context N'g1_permitir_stock', NULL;
        SET @Mensaje = ERROR_MESSAGE();

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT @Mensaje AS Error;
    END CATCH
END;
GO

-- ============================================================
-- PROCEDIMIENTOS: scrap
-- ============================================================

CREATE OR ALTER PROCEDURE SP_ObtenerScrap
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

-- Registra el scrap, descuenta el stock y lo deja como Egreso en el kardex.
-- El costo se calcula aca con el Precio_Compra actual del producto.
-- Igual que el movimiento, si falla devuelve la columna "Error"
CREATE OR ALTER PROCEDURE SP_InsertarScrap
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

        -- el scrap tambien queda en el kardex como egreso
        INSERT INTO Movimientos_Stock (FK_Producto, FK_Usuario, Tipo_Movimiento, Cantidad, Fecha,
                                       Tipo_Documento_Origen, ID_Documento_Origen)
        VALUES (@FK_Producto, @FK_Usuario, 'Egreso', @Cantidad, GETDATE(), 'Scrap', @IdScrap);

        SET @IdMov = SCOPE_IDENTITY();

        EXEC sp_set_session_context N'g1_permitir_stock', 1;
        UPDATE Productos SET Stock_Actual = @StockNuevo WHERE ID_Codigo = @FK_Producto;
        EXEC sp_set_session_context N'g1_permitir_stock', NULL;

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
        EXEC sp_set_session_context N'g1_permitir_stock', NULL;
        SET @Mensaje = ERROR_MESSAGE();

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT @Mensaje AS Error;
    END CATCH
END;
GO

-- ============================================================
-- PROCEDIMIENTOS: auditoria
-- ============================================================

-- Sirve para que cualquier parte del sistema deje registrado un cambio
CREATE OR ALTER PROCEDURE SP_RegistrarCambioAuditoria
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

CREATE OR ALTER PROCEDURE SP_ObtenerAuditoria
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

-- ============================================================
-- DATOS INICIALES (para poder cargar productos)
-- ============================================================

INSERT INTO Categorias (Nombre) VALUES ('Almacén'), ('Bebidas'), ('Limpieza'), ('Lácteos');
INSERT INTO Marcas (Nombre) VALUES ('Genérica'), ('La Serenísima'), ('Arcor');
GO
