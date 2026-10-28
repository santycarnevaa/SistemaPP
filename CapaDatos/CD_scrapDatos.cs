using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_scrapDatos : CD_conexion
    {
        public List<RegistroScrap> ObtenerScrap(DateTime? desde, DateTime? hasta)
        {
            List<RegistroScrap> registros = new List<RegistroScrap>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerScrap", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Desde", desde.HasValue ? desde.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Hasta", hasta.HasValue ? hasta.Value : DBNull.Value);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        registros.Add(new RegistroScrap
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            FK_Producto = Convert.ToInt32(reader["FK_Producto"]),
                            FK_Usuario = Convert.ToInt32(reader["FK_Usuario"]),
                            Cantidad = Convert.ToInt32(reader["Cantidad"]),
                            Motivo = reader["Motivo"].ToString(),
                            Costo_Valorizado = Convert.ToDecimal(reader["Costo_Valorizado"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"])
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_scrapDatos.ObtenerScrap", ex);
                return new List<RegistroScrap>();
            }

            return registros;
        }

        // Igual que InsertarMovimiento: devuelve null si salió bien, o el mensaje
        // de error del SP (por ejemplo "no hay stock suficiente").
        public string InsertarScrap(RegistroScrap s, out int stockResultante, out decimal costoValorizado)
        {
            stockResultante = -1;
            costoValorizado = 0;

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_InsertarScrap", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FK_Producto", s.FK_Producto);
                    cmd.Parameters.AddWithValue("@FK_Usuario", s.FK_Usuario);
                    cmd.Parameters.AddWithValue("@Cantidad", s.Cantidad);
                    cmd.Parameters.AddWithValue("@Motivo", s.Motivo);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        if (HasColumn(reader, "Error"))
                        {
                            string error = reader["Error"].ToString();
                            reader.Close();
                            return error;
                        }

                        stockResultante = Convert.ToInt32(reader["Stock_Resultante"]);
                        costoValorizado = Convert.ToDecimal(reader["Costo_Valorizado"]);
                    }

                    reader.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_scrapDatos.InsertarScrap", ex);
                return "Error de base de datos al registrar el scrap.";
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
