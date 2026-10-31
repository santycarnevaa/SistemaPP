-- ============================================================================
-- GRUPO 1 — Correcciones posteriores a la auditoría (AUDITORIA_GRUPO1.md)
-- Correr DESPUÉS de Grupo1_CatalogoStockAuditoria.sql. Es re-ejecutable:
-- usa DROP ... IF EXISTS + CREATE (no CREATE OR ALTER).
--
-- Qué cambia:
--  * Cada alta/baja/modificación del grupo 1 escribe su fila de auditoría
--    DENTRO del mismo SP y la misma transacción (antes la escribía C# después
--    y silenciaba los errores). El ID auditado es el del registro real.
--  * Auditoria_Cambios es inmutable: UPDATE y DELETE se rechazan por trigger.
--  * Stock_Actual solo cambia desde SP_InsertarMovimientoStock y
--    SP_InsertarScrap (trigger lo impone aunque alguien haga UPDATE a mano).
--    Se elimina SP_ActualizarStockActual.
--  * Ajuste puede ser positivo o negativo (Cantidad con signo, solo en Ajuste).
--  * No se mueve stock de productos dados de baja.
--  * El scrap también deja un Egreso en el kardex (Tipo_Documento_Origen='Scrap').
--  * Reportes con nombres, kardex con saldo, vencimientos incluyen el día de hoy.
-- ============================================================================

-- ----------------------------------------------------------------------------
-- Constraints
-- ----------------------------------------------------------------------------
ALTER TABLE dbo.Movimientos_Stock DROP CONSTRAINT IF EXISTS CK_MovStock_Cantidad;
GO
ALTER TABLE dbo.Movimientos_Stock ADD CONSTRAINT CK_MovStock_Cantidad
    CHECK ((Tipo_Movimiento = 'Ajuste' AND Cantidad <> 0)
        OR (Tipo_Movimiento <> 'Ajuste' AND Cantidad > 0));
GO

ALTER TABLE dbo.Productos DROP CONSTRAINT IF EXISTS CK_Productos_Valores;
GO
ALTER TABLE dbo.Productos ADD CONSTRAINT CK_Productos_Valores
    CHECK (Precio_Compra > 0 AND Precio_Venta > 0
       AND Stock_Critico >= 0
       AND Punto_Reposicion >= 0 AND Punto_Reposicion <= Stock_Maximo);
GO

-- ----------------------------------------------------------------------------
-- Triggers
-- ----------------------------------------------------------------------------
DROP TRIGGER IF EXISTS dbo.TR_Auditoria_Inmutable;
GO
CREATE TRIGGER dbo.TR_Auditoria_Inmutable
ON dbo.Auditoria_Cambios
INSTEAD OF UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    THROW 50020, 'Auditoria_Cambios es de solo escritura: no se puede modificar ni borrar un registro.', 1;
END;
GO

DROP TRIGGER IF EXISTS dbo.TR_Productos_ProtegerStock;
GO
CREATE TRIGGER dbo.TR_Productos_ProtegerStock
ON dbo.Productos
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF ISNULL(TRY_CAST(SESSION_CONTEXT(N'g1_permitir_stock') AS INT), 0) = 1
        RETURN;

    IF NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        IF EXISTS (SELECT 1 FROM inserted WHERE Stock_Actual <> 0)
            THROW 50010, 'Stock_Actual solo puede cambiar mediante un movimiento de stock o un registro de scrap.', 1;
    END
    ELSE IF UPDATE(Stock_Actual)
         AND EXISTS (SELECT 1 FROM inserted i JOIN deleted d ON d.ID_Codigo = i.ID_Codigo
                     WHERE i.Stock_Actual <> d.Stock_Actual)
    BEGIN
        THROW 50010, 'Stock_Actual solo puede cambiar mediante un movimiento de stock o un registro de scrap.', 1;
    END
END;
GO

-- ----------------------------------------------------------------------------
-- Productos
-- ----------------------------------------------------------------------------
DROP PROCEDURE IF EXISTS dbo.SP_ActualizarStockActual;
GO

DROP PROCEDURE IF EXISTS dbo.SP_InsertarProducto;
GO
CREATE PROCEDURE dbo.SP_InsertarProducto
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
    @FK_Usuario        INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @propia BIT = 0;

    BEGIN TRY
        IF @@TRANCOUNT = 0 BEGIN BEGIN TRAN; SET @propia = 1; END
        ELSE SAVE TRAN sp_ins_producto;

        IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE IdUsuario = @FK_Usuario)
            THROW 50003, 'Usuario inválido.', 1;

        INSERT INTO dbo.Productos
            (FK_Categoria, FK_Marca, Nombre, Precio_Compra, Precio_Venta,
             Stock_Actual, Stock_Critico, Stock_Minimo, Stock_Ideal, Stock_Maximo,
             Punto_Reposicion, Numero_Lote, Fecha_Vencimiento, Estado, Habilitado)
        VALUES
            (@FK_Categoria, @FK_Marca, @Nombre, @Precio_Compra, @Precio_Venta,
             0, @Stock_Critico, @Stock_Minimo, @Stock_Ideal, @Stock_Maximo,
             @Punto_Reposicion, @Numero_Lote, @Fecha_Vencimiento, 'Activo', 1);

        DECLARE @Id INT = SCOPE_IDENTITY();

        INSERT INTO dbo.Auditoria_Cambios
            (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
        VALUES
            ('Productos', @Id, 'Alta', @FK_Usuario, GETDATE(),
             CONCAT('Alta de producto ''', @Nombre, ''' — compra ', @Precio_Compra, ', venta ', @Precio_Venta,
                    ', niveles ', @Stock_Critico, '/', @Stock_Minimo, '/', @Stock_Ideal, '/', @Stock_Maximo,
                    ', reposición ', @Punto_Reposicion));

        IF @propia = 1 COMMIT TRAN;

        SELECT @Id AS ID_Codigo;
    END TRY
    BEGIN CATCH
        IF @propia = 1 BEGIN IF @@TRANCOUNT > 0 ROLLBACK TRAN; END
        ELSE IF XACT_STATE() = 1 ROLLBACK TRAN sp_ins_producto;
        THROW;
    END CATCH
END;
GO

DROP PROCEDURE IF EXISTS dbo.SP_ActualizarProducto;
GO
CREATE PROCEDURE dbo.SP_ActualizarProducto
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
    @Estado            VARCHAR(30),
    @FK_Usuario        INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @propia BIT = 0;

    BEGIN TRY
        IF @@TRANCOUNT = 0 BEGIN BEGIN TRAN; SET @propia = 1; END
        ELSE SAVE TRAN sp_upd_producto;

        IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE IdUsuario = @FK_Usuario)
            THROW 50003, 'Usuario inválido.', 1;

        DECLARE @oCat INT, @oMarca INT, @oNombre VARCHAR(150), @oPC DECIMAL(18,2), @oPV DECIMAL(18,2),
                @oCri INT, @oMin INT, @oIde INT, @oMax INT, @oPR INT, @oLote VARCHAR(50),
                @oVto DATE, @oEstado VARCHAR(30), @oHab BIT;

        SELECT @oCat = FK_Categoria, @oMarca = FK_Marca, @oNombre = Nombre, @oPC = Precio_Compra,
               @oPV = Precio_Venta, @oCri = Stock_Critico, @oMin = Stock_Minimo, @oIde = Stock_Ideal,
               @oMax = Stock_Maximo, @oPR = Punto_Reposicion, @oLote = Numero_Lote,
               @oVto = Fecha_Vencimiento, @oEstado = Estado, @oHab = Habilitado
        FROM dbo.Productos WITH (UPDLOCK, HOLDLOCK)
        WHERE ID_Codigo = @ID_Codigo;

        IF @oHab IS NULL THROW 50001, 'El producto no existe.', 1;
        IF @oHab = 0 THROW 50004, 'El producto está dado de baja.', 1;

        DECLARE @det VARCHAR(MAX) = CONCAT_WS('; ',
            IIF(@oNombre <> @Nombre, CONCAT('Nombre: ', @oNombre, ' -> ', @Nombre), NULL),
            IIF(@oCat <> @FK_Categoria, CONCAT('Categoría: ', @oCat, ' -> ', @FK_Categoria), NULL),
            IIF(@oMarca <> @FK_Marca, CONCAT('Marca: ', @oMarca, ' -> ', @FK_Marca), NULL),
            IIF(@oPC <> @Precio_Compra, CONCAT('Precio compra: ', @oPC, ' -> ', @Precio_Compra), NULL),
            IIF(@oPV <> @Precio_Venta, CONCAT('Precio venta: ', @oPV, ' -> ', @Precio_Venta), NULL),
            IIF(@oCri <> @Stock_Critico, CONCAT('Stock crítico: ', @oCri, ' -> ', @Stock_Critico), NULL),
            IIF(@oMin <> @Stock_Minimo, CONCAT('Stock mínimo: ', @oMin, ' -> ', @Stock_Minimo), NULL),
            IIF(@oIde <> @Stock_Ideal, CONCAT('Stock ideal: ', @oIde, ' -> ', @Stock_Ideal), NULL),
            IIF(@oMax <> @Stock_Maximo, CONCAT('Stock máximo: ', @oMax, ' -> ', @Stock_Maximo), NULL),
            IIF(@oPR <> @Punto_Reposicion, CONCAT('Punto reposición: ', @oPR, ' -> ', @Punto_Reposicion), NULL),
            IIF(ISNULL(@oLote, '') <> ISNULL(@Numero_Lote, ''), CONCAT('Lote: ', ISNULL(@oLote, '(sin lote)'), ' -> ', ISNULL(@Numero_Lote, '(sin lote)')), NULL),
            IIF(ISNULL(CONVERT(VARCHAR(10), @oVto, 23), '') <> ISNULL(CONVERT(VARCHAR(10), @Fecha_Vencimiento, 23), ''),
                CONCAT('Vencimiento: ', ISNULL(CONVERT(VARCHAR(10), @oVto, 23), '(sin vencimiento)'), ' -> ', ISNULL(CONVERT(VARCHAR(10), @Fecha_Vencimiento, 23), '(sin vencimiento)')), NULL),
            IIF(@oEstado <> @Estado, CONCAT('Estado: ', @oEstado, ' -> ', @Estado), NULL));

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

        INSERT INTO dbo.Auditoria_Cambios
            (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
        VALUES
            ('Productos', @ID_Codigo, 'Modificación', @FK_Usuario, GETDATE(),
             ISNULL(@det, 'Guardado sin cambios en los datos'));

        IF @propia = 1 COMMIT TRAN;

        SELECT 1 AS Filas_Afectadas;
    END TRY
    BEGIN CATCH
        IF @propia = 1 BEGIN IF @@TRANCOUNT > 0 ROLLBACK TRAN; END
        ELSE IF XACT_STATE() = 1 ROLLBACK TRAN sp_upd_producto;
        THROW;
    END CATCH
END;
GO

DROP PROCEDURE IF EXISTS dbo.SP_DarBajaProducto;
GO
CREATE PROCEDURE dbo.SP_DarBajaProducto
    @ID_Codigo  INT,
    @FK_Usuario INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @propia BIT = 0;

    BEGIN TRY
        IF @@TRANCOUNT = 0 BEGIN BEGIN TRAN; SET @propia = 1; END
        ELSE SAVE TRAN sp_baja_producto;

        IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE IdUsuario = @FK_Usuario)
            THROW 50003, 'Usuario inválido.', 1;

        UPDATE dbo.Productos
        SET Habilitado = 0
        WHERE ID_Codigo = @ID_Codigo AND Habilitado = 1;

        DECLARE @Filas INT = @@ROWCOUNT;

        IF @Filas > 0
            INSERT INTO dbo.Auditoria_Cambios
                (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
            VALUES
                ('Productos', @ID_Codigo, 'Baja', @FK_Usuario, GETDATE(), 'Baja lógica de producto');

        IF @propia = 1 COMMIT TRAN;

        SELECT @Filas AS Filas_Afectadas;
    END TRY
    BEGIN CATCH
        IF @propia = 1 BEGIN IF @@TRANCOUNT > 0 ROLLBACK TRAN; END
        ELSE IF XACT_STATE() = 1 ROLLBACK TRAN sp_baja_producto;
        THROW;
    END CATCH
END;
GO

-- ----------------------------------------------------------------------------
-- Movimientos de stock
-- Ingreso suma, Egreso resta, Ajuste suma la Cantidad con su signo.
-- ----------------------------------------------------------------------------
DROP PROCEDURE IF EXISTS dbo.SP_InsertarMovimientoStock;
GO
CREATE PROCEDURE dbo.SP_InsertarMovimientoStock
    @FK_Producto            INT,
    @FK_Usuario             INT,
    @Tipo_Movimiento        VARCHAR(20),
    @Cantidad               INT,
    @Tipo_Documento_Origen  VARCHAR(50) = NULL,
    @ID_Documento_Origen    INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @propia BIT = 0;

    BEGIN TRY
        IF @@TRANCOUNT = 0 BEGIN BEGIN TRAN; SET @propia = 1; END
        ELSE SAVE TRAN sp_mov_stock;

        IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE IdUsuario = @FK_Usuario)
            THROW 50003, 'Usuario inválido.', 1;

        IF @Tipo_Movimiento NOT IN ('Ingreso', 'Egreso', 'Ajuste')
            THROW 50005, 'Tipo de movimiento inválido.', 1;

        IF (@Tipo_Movimiento = 'Ajuste' AND @Cantidad = 0)
           OR (@Tipo_Movimiento <> 'Ajuste' AND @Cantidad <= 0)
            THROW 50006, 'Cantidad inválida para el tipo de movimiento.', 1;

        DECLARE @StockActual INT, @Habilitado BIT;

        SELECT @StockActual = Stock_Actual, @Habilitado = Habilitado
        FROM dbo.Productos WITH (UPDLOCK, HOLDLOCK)
        WHERE ID_Codigo = @FK_Producto;

        IF @StockActual IS NULL THROW 50001, 'El producto no existe.', 1;
        IF @Habilitado = 0 THROW 50004, 'El producto está dado de baja: no admite movimientos.', 1;

        DECLARE @StockNuevo INT =
            CASE @Tipo_Movimiento WHEN 'Egreso' THEN @StockActual - @Cantidad ELSE @StockActual + @Cantidad END;

        IF @StockNuevo < 0
            THROW 50007, 'El movimiento dejaría el stock en negativo.', 1;

        INSERT INTO dbo.Movimientos_Stock
            (FK_Producto, FK_Usuario, Tipo_Movimiento, Cantidad, Fecha, Tipo_Documento_Origen, ID_Documento_Origen)
        VALUES
            (@FK_Producto, @FK_Usuario, @Tipo_Movimiento, @Cantidad, GETDATE(), @Tipo_Documento_Origen, @ID_Documento_Origen);

        DECLARE @IdMov INT = SCOPE_IDENTITY();

        EXEC sp_set_session_context N'g1_permitir_stock', 1;
        UPDATE dbo.Productos SET Stock_Actual = @StockNuevo WHERE ID_Codigo = @FK_Producto;
        EXEC sp_set_session_context N'g1_permitir_stock', NULL;

        INSERT INTO dbo.Auditoria_Cambios
            (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
        VALUES
            ('Movimientos_Stock', @IdMov, 'Alta', @FK_Usuario, GETDATE(),
             CONCAT(@Tipo_Movimiento, ' de ', @Cantidad, ' unidades del producto ', @FK_Producto,
                    ' — stock ', @StockActual, ' -> ', @StockNuevo));

        IF @propia = 1 COMMIT TRAN;

        SELECT @IdMov AS ID, @StockNuevo AS Stock_Resultante;
    END TRY
    BEGIN CATCH
        EXEC sp_set_session_context N'g1_permitir_stock', NULL;
        DECLARE @msg VARCHAR(4000) = ERROR_MESSAGE();
        IF @propia = 1 BEGIN IF @@TRANCOUNT > 0 ROLLBACK TRAN; END
        ELSE IF XACT_STATE() = 1 ROLLBACK TRAN sp_mov_stock;
        SELECT @msg AS Error;
    END CATCH
END;
GO

DROP PROCEDURE IF EXISTS dbo.SP_ObtenerMovimientosPorProducto;
GO
-- Kardex en orden cronológico, con el stock que quedó después de cada movimiento.
CREATE PROCEDURE dbo.SP_ObtenerMovimientosPorProducto
    @FK_Producto INT,
    @Desde       DATETIME = NULL,
    @Hasta       DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    WITH K AS (
        SELECT m.ID, m.FK_Producto, p.Nombre AS Producto, m.FK_Usuario, u.Usuario,
               m.Tipo_Movimiento, m.Cantidad, m.Fecha, m.Tipo_Documento_Origen, m.ID_Documento_Origen,
               SUM(CASE m.Tipo_Movimiento WHEN 'Egreso' THEN -m.Cantidad ELSE m.Cantidad END)
                   OVER (ORDER BY m.Fecha, m.ID ROWS UNBOUNDED PRECEDING) AS Stock_Posterior
        FROM dbo.Movimientos_Stock m
        JOIN dbo.Productos p ON p.ID_Codigo = m.FK_Producto
        JOIN dbo.Usuarios  u ON u.IdUsuario = m.FK_Usuario
        WHERE m.FK_Producto = @FK_Producto
    )
    SELECT ID, FK_Producto, Producto, FK_Usuario, Usuario, Tipo_Movimiento, Cantidad, Fecha,
           Tipo_Documento_Origen, ID_Documento_Origen, Stock_Posterior
    FROM K
    WHERE (@Desde IS NULL OR Fecha >= @Desde)
      AND (@Hasta IS NULL OR Fecha <= @Hasta)
    ORDER BY Fecha, ID;
END;
GO

-- ----------------------------------------------------------------------------
-- Scrap
-- ----------------------------------------------------------------------------
DROP PROCEDURE IF EXISTS dbo.SP_InsertarScrap;
GO
CREATE PROCEDURE dbo.SP_InsertarScrap
    @FK_Producto INT,
    @FK_Usuario  INT,
    @Cantidad    INT,
    @Motivo      VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @propia BIT = 0;

    BEGIN TRY
        IF @@TRANCOUNT = 0 BEGIN BEGIN TRAN; SET @propia = 1; END
        ELSE SAVE TRAN sp_scrap;

        IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE IdUsuario = @FK_Usuario)
            THROW 50003, 'Usuario inválido.', 1;

        IF @Cantidad <= 0 THROW 50006, 'La cantidad debe ser mayor a cero.', 1;
        IF LEN(LTRIM(RTRIM(ISNULL(@Motivo, '')))) = 0 THROW 50008, 'Debe indicar el motivo del scrap.', 1;

        DECLARE @StockActual INT, @PrecioCompra DECIMAL(18,2), @Habilitado BIT;

        SELECT @StockActual = Stock_Actual, @PrecioCompra = Precio_Compra, @Habilitado = Habilitado
        FROM dbo.Productos WITH (UPDLOCK, HOLDLOCK)
        WHERE ID_Codigo = @FK_Producto;

        IF @StockActual IS NULL THROW 50001, 'El producto no existe.', 1;
        IF @Habilitado = 0 THROW 50004, 'El producto está dado de baja: no admite scrap.', 1;

        DECLARE @StockNuevo INT = @StockActual - @Cantidad;
        IF @StockNuevo < 0
            THROW 50007, 'No hay stock suficiente para dar de baja esa cantidad.', 1;

        DECLARE @Costo DECIMAL(18,2) = CAST(@Cantidad AS DECIMAL(18,2)) * @PrecioCompra;

        INSERT INTO dbo.Registros_Scrap (FK_Producto, FK_Usuario, Cantidad, Motivo, Costo_Valorizado, Fecha)
        VALUES (@FK_Producto, @FK_Usuario, @Cantidad, @Motivo, @Costo, GETDATE());

        DECLARE @IdScrap INT = SCOPE_IDENTITY();

        -- El scrap también queda en el kardex como egreso, así el historial cierra con el stock.
        INSERT INTO dbo.Movimientos_Stock
            (FK_Producto, FK_Usuario, Tipo_Movimiento, Cantidad, Fecha, Tipo_Documento_Origen, ID_Documento_Origen)
        VALUES (@FK_Producto, @FK_Usuario, 'Egreso', @Cantidad, GETDATE(), 'Scrap', @IdScrap);

        DECLARE @IdMov INT = SCOPE_IDENTITY();

        EXEC sp_set_session_context N'g1_permitir_stock', 1;
        UPDATE dbo.Productos SET Stock_Actual = @StockNuevo WHERE ID_Codigo = @FK_Producto;
        EXEC sp_set_session_context N'g1_permitir_stock', NULL;

        INSERT INTO dbo.Auditoria_Cambios
            (Tabla_Afectada, ID_Registro_Afectado, Accion, FK_Usuario, Fecha_Hora, Detalle_Cambio)
        VALUES
            ('Registros_Scrap', @IdScrap, 'Alta', @FK_Usuario, GETDATE(),
             CONCAT('Scrap de ', @Cantidad, ' unidades del producto ', @FK_Producto, ' por ''', @Motivo,
                    ''' — costo valorizado ', @Costo, ', stock ', @StockActual, ' -> ', @StockNuevo)),
            ('Movimientos_Stock', @IdMov, 'Alta', @FK_Usuario, GETDATE(),
             CONCAT('Egreso por scrap #', @IdScrap, ' de ', @Cantidad, ' unidades del producto ', @FK_Producto));

        IF @propia = 1 COMMIT TRAN;

        SELECT @IdScrap AS ID, @StockNuevo AS Stock_Resultante, @Costo AS Costo_Valorizado;
    END TRY
    BEGIN CATCH
        EXEC sp_set_session_context N'g1_permitir_stock', NULL;
        DECLARE @msg VARCHAR(4000) = ERROR_MESSAGE();
        IF @propia = 1 BEGIN IF @@TRANCOUNT > 0 ROLLBACK TRAN; END
        ELSE IF XACT_STATE() = 1 ROLLBACK TRAN sp_scrap;
        SELECT @msg AS Error;
    END CATCH
END;
GO

DROP PROCEDURE IF EXISTS dbo.SP_ObtenerScrap;
GO
CREATE PROCEDURE dbo.SP_ObtenerScrap
    @Desde DATETIME = NULL,
    @Hasta DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT s.ID, s.FK_Producto, p.Nombre AS Producto, s.FK_Usuario, u.Usuario,
           s.Cantidad, s.Motivo, s.Costo_Valorizado, s.Fecha
    FROM dbo.Registros_Scrap s
    JOIN dbo.Productos p ON p.ID_Codigo = s.FK_Producto
    JOIN dbo.Usuarios  u ON u.IdUsuario = s.FK_Usuario
    WHERE (@Desde IS NULL OR s.Fecha >= @Desde)
      AND (@Hasta IS NULL OR s.Fecha <= @Hasta)
    ORDER BY s.Fecha DESC;
END;
GO

-- ----------------------------------------------------------------------------
-- Reportes
-- ----------------------------------------------------------------------------
DROP PROCEDURE IF EXISTS dbo.SP_ReporteStockPorCategoria;
GO
CREATE PROCEDURE dbo.SP_ReporteStockPorCategoria
AS
BEGIN
    SET NOCOUNT ON;

    SELECT c.Nombre AS Categoria,
           COUNT(p.ID_Codigo) AS Cantidad_Productos,
           ISNULL(SUM(p.Stock_Actual), 0) AS Stock_Total
    FROM dbo.Categorias c
    LEFT JOIN dbo.Productos p ON p.FK_Categoria = c.IdCategoria AND p.Habilitado = 1
    GROUP BY c.IdCategoria, c.Nombre
    ORDER BY c.Nombre;
END;
GO

DROP PROCEDURE IF EXISTS dbo.SP_ReporteStockPorMarca;
GO
CREATE PROCEDURE dbo.SP_ReporteStockPorMarca
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.Nombre AS Marca,
           COUNT(p.ID_Codigo) AS Cantidad_Productos,
           ISNULL(SUM(p.Stock_Actual), 0) AS Stock_Total
    FROM dbo.Marcas m
    LEFT JOIN dbo.Productos p ON p.FK_Marca = m.IdMarca AND p.Habilitado = 1
    GROUP BY m.IdMarca, m.Nombre
    ORDER BY m.Nombre;
END;
GO

DROP PROCEDURE IF EXISTS dbo.SP_ObtenerProductosProximosAVencer;
GO
-- Desde hoy (inclusive) hasta hoy + @DiasAnticipacion (inclusive).
CREATE PROCEDURE dbo.SP_ObtenerProductosProximosAVencer
    @DiasAnticipacion INT = 30
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);

    SELECT ID_Codigo, Nombre, Numero_Lote, Fecha_Vencimiento, Stock_Actual
    FROM dbo.Productos
    WHERE Habilitado = 1
      AND Fecha_Vencimiento IS NOT NULL
      AND Fecha_Vencimiento BETWEEN @Hoy AND DATEADD(DAY, @DiasAnticipacion, @Hoy)
    ORDER BY Fecha_Vencimiento;
END;
GO

-- ----------------------------------------------------------------------------
-- Auditoría
-- ----------------------------------------------------------------------------
DROP PROCEDURE IF EXISTS dbo.SP_ObtenerAuditoria;
GO
CREATE PROCEDURE dbo.SP_ObtenerAuditoria
    @Tabla_Afectada VARCHAR(100) = NULL,
    @FK_Usuario     INT = NULL,
    @Desde          DATETIME = NULL,
    @Hasta          DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT a.ID, a.Tabla_Afectada, a.ID_Registro_Afectado, a.Accion, a.FK_Usuario, u.Usuario,
           a.Fecha_Hora, a.Detalle_Cambio
    FROM dbo.Auditoria_Cambios a
    JOIN dbo.Usuarios u ON u.IdUsuario = a.FK_Usuario
    WHERE (@Tabla_Afectada IS NULL OR a.Tabla_Afectada = @Tabla_Afectada)
      AND (@FK_Usuario IS NULL OR a.FK_Usuario = @FK_Usuario)
      AND (@Desde IS NULL OR a.Fecha_Hora >= @Desde)
      AND (@Hasta IS NULL OR a.Fecha_Hora <= @Hasta)
    ORDER BY a.Fecha_Hora DESC, a.ID DESC;
END;
GO
