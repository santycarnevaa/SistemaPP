using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_movimientosStockDatos : CD_conexion
    {
        public List<MovimientoStock> ObtenerPorProducto(int idProducto, DateTime? desde, DateTime? hasta)
        {
            List<MovimientoStock> movimientos = new List<MovimientoStock>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerMovimientosPorProducto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FK_Producto", idProducto);
                    cmd.Parameters.AddWithValue("@Desde", desde.HasValue ? desde.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Hasta", hasta.HasValue ? hasta.Value : DBNull.Value);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        movimientos.Add(new MovimientoStock
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            FK_Producto = Convert.ToInt32(reader["FK_Producto"]),
                            FK_Usuario = Convert.ToInt32(reader["FK_Usuario"]),
                            Tipo_Movimiento = reader["Tipo_Movimiento"].ToString(),
                            Cantidad = Convert.ToInt32(reader["Cantidad"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            Tipo_Documento_Origen = reader["Tipo_Documento_Origen"] == DBNull.Value ? null : reader["Tipo_Documento_Origen"].ToString(),
                            ID_Documento_Origen = reader["ID_Documento_Origen"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ID_Documento_Origen"])
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_movimientosStockDatos.ObtenerPorProducto", ex);
                return new List<MovimientoStock>();
            }

            return movimientos;
        }

        // Devuelve null si salió todo bien, o el mensaje de error que mandó el SP
        // (por ejemplo "el movimiento dejaría el stock en negativo") para que la
        // capa de Lógica se lo pueda mostrar al usuario tal cual.
        public string InsertarMovimiento(MovimientoStock m, out int stockResultante)
        {
            stockResultante = -1;

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_InsertarMovimientoStock", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FK_Producto", m.FK_Producto);
                    cmd.Parameters.AddWithValue("@FK_Usuario", m.FK_Usuario);
                    cmd.Parameters.AddWithValue("@Tipo_Movimiento", m.Tipo_Movimiento);
                    cmd.Parameters.AddWithValue("@Cantidad", m.Cantidad);
                    cmd.Parameters.AddWithValue("@Tipo_Documento_Origen", string.IsNullOrWhiteSpace(m.Tipo_Documento_Origen) ? DBNull.Value : m.Tipo_Documento_Origen);
                    cmd.Parameters.AddWithValue("@ID_Documento_Origen", m.ID_Documento_Origen.HasValue ? m.ID_Documento_Origen.Value : DBNull.Value);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // El SP devuelve (ID, Stock_Resultante) si salió bien, o (Error) si falló.
                        if (HasColumn(reader, "Error"))
                        {
                            string error = reader["Error"].ToString();
                            reader.Close();
                            return error;
                        }

                        stockResultante = Convert.ToInt32(reader["Stock_Resultante"]);
                    }

                    reader.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_movimientosStockDatos.InsertarMovimiento", ex);
                return "Error de base de datos al registrar el movimiento.";
            }
        }

        private bool HasColumn(SqlDataReader reader, string nombreColumna)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == nombreColumna)
                    return true;
            }
            return false;
        }
    }
}
