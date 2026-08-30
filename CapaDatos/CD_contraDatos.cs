using System.Data;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_contraDatos : CD_conexion
    {
        public bool CambiarContra(int idUsuario, string nuevoPasswordHash, bool validarHistorial = true)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_CambiarPassword", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@NuevaPasswordHash", nuevoPasswordHash);
                    cmd.Parameters.AddWithValue("@ValidarHistorial", validarHistorial);

                    object resultado = cmd.ExecuteScalar();

                    if (resultado == null)
                        return false;

                    string mensaje = resultado.ToString();

                    return mensaje.Contains("correctamente");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al cambiar la contraseña: " + ex.Message, ex);
            }
        }

        public bool insertarPassword(int idUsuario, string passwordHash)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_InsertarPassword", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al registrar contraseña temporal: " + ex.Message, ex);
            }
        }

        public List<string> obtenerContrasAnteriores(int idUsuario)
        {
            List<string> passwords = new List<string>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerContrasAnteriores", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        passwords.Add(reader["PasswordHash"].ToString());
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_contraDatos.obtenerContrasAnteriores", ex);
                return new List<string>();
            }

            return passwords;
        }

        public string obtenerUltimoPasswordHash(int idUsuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerUltimoPasswordHash", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    object result = cmd.ExecuteScalar();

                    return result != null ? result.ToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_contraDatos.obtenerUltimoPasswordHash", ex);
                return string.Empty;
            }
        }

        public bool verificarLogin(string usuario, string passwordHash)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_VerificarLogin", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    int cantidad = Convert.ToInt32(cmd.ExecuteScalar());

                    return cantidad > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_contraDatos.verificarLogin", ex);
                return false;
            }
        }

        public bool actualizarPasswordYPrimerLogin(int idUsuario, string passwordHash)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ActualizarPasswordYPrimerLogin", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_contraDatos.actualizarPasswordYPrimerLogin", ex);
                return false;
            }
        }
    }
}
