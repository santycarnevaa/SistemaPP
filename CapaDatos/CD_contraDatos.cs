using Microsoft.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_contraDatos : CD_conexion
    {
        public bool CambiarContra(int idUsuario, string nuevoPasswordHash)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_CambiarPassword", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@NuevaPasswordHash", nuevoPasswordHash);

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
                    string query = @"
                        INSERT INTO dbo.Passwords
                        (
                            IdUsuario,
                            PasswordHash,
                            FechaCreacion
                        )
                        VALUES
                        (
                            @IdUsuario,
                            @PasswordHash,
                            GETDATE()
                        );
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);

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
                    string query = @"
                        SELECT PasswordHash
                        FROM dbo.Passwords
                        WHERE IdUsuario = @IdUsuario
                        ORDER BY FechaCreacion DESC;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        passwords.Add(reader["PasswordHash"].ToString());
                    }

                    reader.Close();
                }
            }
            catch
            {
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
                    string query = @"
                        SELECT TOP 1 PasswordHash
                        FROM dbo.Passwords
                        WHERE IdUsuario = @IdUsuario
                        ORDER BY FechaCreacion DESC;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    object result = cmd.ExecuteScalar();

                    return result != null ? result.ToString() : string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool verificarLogin(string usuario, string passwordHash)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT COUNT(*)
                        FROM dbo.Usuarios U
                        INNER JOIN dbo.Passwords P
                            ON U.IdUsuario = P.IdUsuario
                        WHERE U.Usuario = @Usuario
                          AND U.Activo = 1
                          AND P.PasswordHash = @PasswordHash
                          AND P.FechaCreacion = (
                                SELECT MAX(P2.FechaCreacion)
                                FROM dbo.Passwords P2
                                WHERE P2.IdUsuario = U.IdUsuario
                          );
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);

                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    int cantidad = Convert.ToInt32(cmd.ExecuteScalar());

                    return cantidad > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool actualizarPasswordYPrimerLogin(int idUsuario, string passwordHash)
        {
            using (SqlConnection conexion = conectar())
            using (SqlTransaction tran = conexion.BeginTransaction())
            {
                try
                {
                    string insertPassword = @"
                        INSERT INTO dbo.Passwords
                        (
                            IdUsuario,
                            PasswordHash,
                            FechaCreacion,
                            EsTemporal
                        )
                        VALUES
                        (
                            @IdUsuario,
                            @PasswordHash,
                            GETDATE(),
                            0
                        );
                    ";

                    SqlCommand cmdPassword = new SqlCommand(insertPassword, conexion, tran);
                    cmdPassword.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmdPassword.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    cmdPassword.ExecuteNonQuery();

                    string updateUsuario = @"
                        UPDATE dbo.Usuarios
                        SET PrimerLogin = 0
                        WHERE IdUsuario = @IdUsuario;
                    ";

                    SqlCommand cmdUsuario = new SqlCommand(updateUsuario, conexion, tran);
                    cmdUsuario.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmdUsuario.ExecuteNonQuery();

                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                }
            }
        }
    }
}