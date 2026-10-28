using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class CD_configuracionCorreoDatos : CD_conexion
    {
        public ConfiguracionCorreo ObtenerConfiguracionCorreo()
        {
            using (SqlConnection conexion = conectar())
            {
                SqlCommand cmd = new SqlCommand("SP_ObtenerConfiguracionCorreo", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new ConfiguracionCorreo
                    {
                        IdConfiguracionCorreo = Convert.ToInt32(reader["IdConfiguracionCorreo"]),
                        CorreoRemitente = reader["CorreoRemitente"].ToString(),
                        PasswordCifrada = (byte[])reader["PasswordCifrada"],
                        IV = (byte[])reader["IV"],
                        FechaUltimaRotacion = Convert.ToDateTime(reader["FechaUltimaRotacion"]),
                        DiasRotacion = Convert.ToInt32(reader["DiasRotacion"])
                    };
                }

                return null;
            }
        }

        public bool GuardarConfiguracionCorreo(string correoRemitente, int diasRotacion, byte[] passwordCifrada = null, byte[] iv = null)
        {
            using (SqlConnection conexion = conectar())
            {
                SqlCommand cmd = new SqlCommand("SP_GuardarConfiguracionCorreo", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CorreoRemitente", correoRemitente);
                cmd.Parameters.AddWithValue("@DiasRotacion", diasRotacion);
                cmd.Parameters.AddWithValue("@PasswordCifrada", (object)passwordCifrada ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IV", (object)iv ?? DBNull.Value);

                cmd.ExecuteNonQuery();
                return true;
            }
        }
    }
}
