using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_movimientosStockDatos : CD_ejecutarConsultasSP
    {
        public List<MovimientoStock> ObtenerPorProducto(int idProducto, DateTime? desde, DateTime? hasta)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@FK_Producto", SqlDbType.Int) { Value = idProducto },
                new SqlParameter("@Desde", SqlDbType.DateTime) { Value = (object?)desde ?? DBNull.Value },
                new SqlParameter("@Hasta", SqlDbType.DateTime) { Value = (object?)hasta ?? DBNull.Value }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerMovimientosPorProducto", parametros);

                List<MovimientoStock> movimientos = new List<MovimientoStock>();
                foreach (DataRow fila in tabla.Rows)
                {
                    movimientos.Add(new MovimientoStock
                    {
                        ID = Convert.ToInt32(fila["ID"]),
                        FK_Producto = Convert.ToInt32(fila["FK_Producto"]),
                        FK_Usuario = Convert.ToInt32(fila["FK_Usuario"]),
                        Tipo_Movimiento = fila["Tipo_Movimiento"].ToString(),
                        Cantidad = Convert.ToInt32(fila["Cantidad"]),
                        Fecha = Convert.ToDateTime(fila["Fecha"]),
                        Tipo_Documento_Origen = fila["Tipo_Documento_Origen"] == DBNull.Value ? null : fila["Tipo_Documento_Origen"].ToString(),
                        ID_Documento_Origen = fila["ID_Documento_Origen"] == DBNull.Value ? (int?)null : Convert.ToInt32(fila["ID_Documento_Origen"]),
                        Producto = fila["Producto"].ToString(),
                        Usuario = fila["Usuario"].ToString(),
                        Stock_Posterior = Convert.ToInt32(fila["Stock_Posterior"])
                    });
                }

                return movimientos;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_movimientosStockDatos.ObtenerPorProducto", ex);
                return new List<MovimientoStock>();
            }
        }

        // devuelve null si salio bien o el mensaje de error del sp
        // (por ej si el stock queda negativo) para que la capa logica lo muestre tal cual
        public string InsertarMovimiento(MovimientoStock m, out int stockResultante)
        {
            stockResultante = -1;

            SqlParameter[] parametros =
            {
                new SqlParameter("@FK_Producto", SqlDbType.Int) { Value = m.FK_Producto },
                new SqlParameter("@FK_Usuario", SqlDbType.Int) { Value = m.FK_Usuario },
                new SqlParameter("@Tipo_Movimiento", SqlDbType.VarChar, 20) { Value = m.Tipo_Movimiento },
                new SqlParameter("@Cantidad", SqlDbType.Int) { Value = m.Cantidad },
                new SqlParameter("@Tipo_Documento_Origen", SqlDbType.VarChar, 50) { Value = ValorONulo(m.Tipo_Documento_Origen) },
                new SqlParameter("@ID_Documento_Origen", SqlDbType.Int) { Value = (object?)m.ID_Documento_Origen ?? DBNull.Value }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_InsertarMovimientoStock", parametros);

                if (tabla.Rows.Count > 0)
                {
                    // si salio bien el sp devuelve id y stock_resultante, si fallo devuelve error
                    if (tabla.Columns.Contains("Error"))
                        return tabla.Rows[0]["Error"].ToString();

                    stockResultante = Convert.ToInt32(tabla.Rows[0]["Stock_Resultante"]);
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_movimientosStockDatos.InsertarMovimiento", ex);
                return "Error de base de datos al registrar el movimiento.";
            }
        }
    }
}
