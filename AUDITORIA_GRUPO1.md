# Auditoría Grupo 1 — Catálogo, Stock y Auditoría

Fecha: 2026-10-31 · Base: `SistemaUsuariosDB` (SQL Server local, unificada)

**Alcance y método.** Lectura del código (Entidades, CapaDatos, CapaLogica, formularios) y del script
`Grupo1_CatalogoStockAuditoria.sql`, más pruebas de los stored procedures directamente en la base.
**No se ejecutó la interfaz WinForms**: lo que dice sobre pantallas sale de leer el código.
Las pruebas en la base se limpiaron (tablas del grupo 1 de nuevo en 0 filas, identidades reseteadas).

## Estado actual: corregido (2026-10-31)

Los puntos de este informe se corrigieron con `Grupo1_Correcciones.sql` (ya aplicado a `SistemaUsuariosDB`) y cambios en
CapaDatos / CapaLogica / formularios. Se probó cada regla en la base y de punta a punta desde la capa lógica de C#
(alta, modificación, baja, ingreso, egreso, ajuste ±, scrap, kardex, reportes y auditoría). La interfaz gráfica no se ejecutó.

Decisiones tomadas: `Ajuste` lleva signo (+ suma, − resta); el scrap además deja un Egreso en el kardex
(`Tipo_Documento_Origen = 'Scrap'`); "próximos a vencer" va de hoy (inclusive) a hoy + N días.
Quedan fuera del alcance del grupo 1: auditar los demás módulos (ellos llaman a `SP_RegistrarCambioAuditoria` o a
`CD_auditoriaDatos.Registrar`), ABM de Marcas/Categorías (hoy hay 4 y 4 de semilla) y el acceso del rol Usuario.

Lo que sigue describe el estado ANTES de las correcciones.

## Resumen

| Requisito | Estado |
|---|---|
| CRUD de Productos con combos Categoría/Marca | Cumple con faltantes menores |
| Alta de Movimiento de Stock + actualiza stock | Cumple (con dudas sobre `Ajuste`) |
| Alta de Scrap, descuenta stock, costo = Cantidad × Precio_Compra | Cumple (probado: 4 × 100 = 400) |
| Consulta de Auditoría | Parcial: falta filtro por usuario |
| Auditoría de TODAS las tablas del sistema | **No cumple** |
| Auditoría inmutable (cada cambio una fila nueva) | **No cumple** (se puede UPDATE/DELETE) |
| Stock solo cambia por movimiento o scrap | Cumple en la UI, **no en la base** |
| Stock nunca negativo | Cumple (probado) |
| Critico < Minimo < Ideal < Maximo | Cumple (Lógica + CHECK, probado) |
| 6 reportes | 4 cumplen con detalles, 2 incompletos |

## Críticos

1. **Solo el grupo 1 deja rastro de auditoría.** Las únicas llamadas a `CD_auditoriaDatos.Registrar` están en
   `CL_servicioProductos` y `CL_servicioStock`. Usuarios, personas, contraseñas, configuración, correo y
   georreferencia no auditan nada. El enunciado pide que cualquier alta/baja/modificación de cualquier tabla quede registrada.
2. **La auditoría se escribe después de la operación, fuera de la transacción, y los errores se tragan**
   (`CD_auditoriaDatos.Registrar` no propaga excepciones). Si falla, la operación queda hecha sin rastro y nadie se entera.
   Caso real: `ObtenerIdUsuario` devuelve `-1` si no encuentra el usuario y la FK rechaza el insert (probado).
   Corrección: registrar la auditoría dentro de cada SP, en la misma transacción.
3. **`Auditoria_Cambios` se puede modificar y borrar.** Probado: `UPDATE` y `DELETE` afectaron filas. Falta un trigger
   `INSTEAD OF UPDATE, DELETE` que lance error, o `DENY UPDATE, DELETE`.
4. **`SP_ActualizarStockActual` permite fijar el stock a cualquier valor sin dejar movimiento** (probado: 3 → 50 sin kardex).
   La UI no lo llama, pero la regla de negocio queda abierta en la base. Los SP de movimiento y scrap ya hacen su propio
   `UPDATE`, así que se puede eliminar.
5. **La auditoría de movimientos y scrap guarda el ID del producto como `ID_Registro_Afectado`**, no el ID del movimiento
   o del scrap. Los SP devuelven ese ID, pero `CD_*` no lo lee. El `Detalle_Cambio` de una modificación no tiene valores
   antes/después.
6. **Falta el filtro por usuario en la pantalla de auditoría.** `frmAuditoria.Filtrar` pasa `null` como usuario.
   El combo de tablas está fijo en 3 valores (Productos, Movimientos_Stock, Registros_Scrap). Muestra `FK_Usuario` como número.

## Importantes

7. **`Ajuste` siempre resta** (`SP_InsertarMovimientoStock`). Un ajuste real puede ser positivo o negativo. Está anotado en
   el propio script como "consultar con el profe". Hay que decidirlo; hoy no hay forma de ajustar hacia arriba.
8. **Un egreso rechazado hace `ROLLBACK` de la transacción del llamador** (error 266/3903 reproducido). Si otro módulo llama
   a estos SP dentro de su transacción, se la destruye. Usar `@@TRANCOUNT` / `SAVE TRANSACTION`.
9. **Se aceptan movimientos sobre productos dados de baja** (probado: ingreso sobre producto con `Habilitado = 0`).
10. **Próximos a vencer** usa `BETWEEN GETDATE() AND ...` con una columna `DATE`: un producto que vence hoy no aparece
    (probado). Usar `CAST(GETDATE() AS DATE)`. Los ya vencidos tampoco aparecen: decidir si deben verse.
11. **Reportes de stock por categoría y por marca devuelven el ID** (`FK_Categoria = 1`), no el nombre. Falta el `JOIN`.
12. **Scrap del período muestra solo el costo total**; el enunciado pide cantidad y costo valorizado total.
13. **Kardex** muestra IDs de producto y usuario y no tiene saldo acumulado.
14. **No hay ABM de Marcas ni Categorías** (ni pantalla ni SP). Por eso no encontraste dónde cargarlas. El script aclara que son
    "de otro grupo": confirmar quién las entrega. Hoy la base tiene 4 marcas y 4 categorías de semilla.
15. **El catálogo solo se abre desde `frmAdmin`**; `frmUsuario` (rol 1) no tiene acceso y ningún formulario del catálogo
    valida rol. Definir qué puede ver/hacer un usuario común.

## Menores

16. `frmProductos.btnGuardar_Click` pisa `Estado = "Activo"` en cada edición.
17. Validaciones faltantes en Lógica: `Punto_Reposicion` (puede ser negativo o mayor al máximo), stocks críticos negativos,
    `Precio_Venta < Precio_Compra`, nombre duplicado, vencimiento en el pasado, lote vacío.
18. `decimal.TryParse` usa la cultura actual: en configuración regional es-AR, `10.50` puede leerse como 1050.
19. `frmProductos` no usa `SP_BuscarProductos` (no hay buscador) y no hay forma de ver ni reactivar productos dados de baja.
20. `SP_DarBajaProducto` devuelve 1 aunque el producto ya estuviera dado de baja y vuelve a auditar.

## Repositorio y despliegue

- Sin commitear: `frmCatalogoStock.Designer.cs` (modificado), `frmCatalogoStock.resx`, `mailsettings.example.json`,
  `mailsettings.json`. **No abrí `mailsettings.json`**; si tiene credenciales de correo, no debe subirse (agregar a `.gitignore`).
- Al armar la base unificada fallaron 6 `CREATE OR ALTER` de contraseñas (ya recreados con `CREATE`). Conviene dejar un único
  script de despliegue probado desde una base vacía.

## Orden sugerido de corrección

1. Auditoría dentro de los SP (misma transacción) + inmutabilidad + ID correcto del registro (puntos 2, 3, 5).
2. Extender la auditoría a los demás módulos (punto 1).
3. Eliminar `SP_ActualizarStockActual` y bloquear movimientos sobre productos dados de baja (4, 9).
4. Filtro por usuario y nombres en reportes (6, 11, 12, 13).
5. Decisión sobre `Ajuste` y sobre Marcas/Categorías (7, 14), vencimientos (10), accesos por rol (15).
