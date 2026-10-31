using CapaDatos;
using Entidades;

namespace CapaLogica
{
    public class CL_servicioStock
    {
        private CD_movimientosStockDatos movimientosDatos = new CD_movimientosStockDatos();
        private CD_scrapDatos scrapDatos = new CD_scrapDatos();

        private static readonly string[] TiposMovimientoValidos = { "Ingreso", "Egreso", "Ajuste" };

        // Devuelve (true, null) si salió bien, o (false, mensaje) si se rechazó
        // — ya sea por una validación de acá o por una regla que aplicó el SP
        // (por ejemplo, que el stock quedaría negativo).
        public (bool Exito, string Mensaje) RegistrarMovimiento(int idProducto, string tipoMovimiento, int cantidad, int idUsuario, string tipoDocOrigen = null, int? idDocOrigen = null)
        {
            if (idUsuario <= 0)
                return (false, "No se pudo identificar al usuario que realiza la operación.");

            if (idProducto <= 0)
                return (false, "Debe seleccionar un producto.");

            if (!TiposMovimientoValidos.Contains(tipoMovimiento))
                return (false, "Tipo de movimiento inválido.");

            // Ingreso y Egreso llevan cantidad positiva; el Ajuste lleva signo (+ suma, - resta).
            if (tipoMovimiento == "Ajuste" ? cantidad == 0 : cantidad <= 0)
                return (false, tipoMovimiento == "Ajuste"
                    ? "La cantidad del ajuste no puede ser cero (usá un signo negativo para restar)."
                    : "La cantidad debe ser mayor a cero.");

            MovimientoStock movimiento = new MovimientoStock
            {
                FK_Producto = idProducto,
                FK_Usuario = idUsuario,
                Tipo_Movimiento = tipoMovimiento,
                Cantidad = cantidad,
                Tipo_Documento_Origen = tipoDocOrigen,
                ID_Documento_Origen = idDocOrigen
            };

            // El SP actualiza el stock y deja la auditoría en la misma transacción.
            string error = movimientosDatos.InsertarMovimiento(movimiento, out _);

            if (error != null)
                return (false, error);

            return (true, null);
        }

        public (bool Exito, string Mensaje) RegistrarScrap(int idProducto, int cantidad, string motivo, int idUsuario)
        {
            if (idUsuario <= 0)
                return (false, "No se pudo identificar al usuario que realiza la operación.");

            if (idProducto <= 0)
                return (false, "Debe seleccionar un producto.");

            if (cantidad <= 0)
                return (false, "La cantidad debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(motivo))
                return (false, "Debe ingresar el motivo del scrap.");

            RegistroScrap scrap = new RegistroScrap
            {
                FK_Producto = idProducto,
                FK_Usuario = idUsuario,
                Cantidad = cantidad,
                Motivo = motivo
            };

            string error = scrapDatos.InsertarScrap(scrap, out _, out _);

            if (error != null)
                return (false, error);

            return (true, null);
        }

        public List<MovimientoStock> ObtenerKardex(int idProducto, DateTime? desde, DateTime? hasta)
        {
            return movimientosDatos.ObtenerPorProducto(idProducto, desde, hasta);
        }

        public List<RegistroScrap> ObtenerScrapDelPeriodo(DateTime? desde, DateTime? hasta)
        {
            return scrapDatos.ObtenerScrap(desde, hasta);
        }
    }
}
