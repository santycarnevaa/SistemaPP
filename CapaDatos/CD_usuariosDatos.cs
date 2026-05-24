using Microsoft.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_usuariosDatos : CD_conexion
    {
        public int RegistrarUsuarioCompleto(
    string nombre,
    string apellido,
    string dni,
    string telefono,
    DateTime fechaNacimiento,

    string calle,
    string numero,
    string codigoPostal,
    string depto,
    string piso,
    string provincia,
    string partido,
    string localidad,
    decimal? latitud,
    decimal? longitud,
    string direccionOriginal,
    string direccionNormalizada,

    string usuario,
    string correo,
    string passwordHash,
    int idRol,
    bool usa2FA)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarUsuarioCompleto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Apellido", apellido);
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    cmd.Parameters.AddWithValue("@Telefono", string.IsNullOrWhiteSpace(telefono) ? DBNull.Value : telefono);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);

                    cmd.Parameters.AddWithValue("@Calle", calle);
                    cmd.Parameters.AddWithValue("@Numero", string.IsNullOrWhiteSpace(numero) ? DBNull.Value : numero);
                    cmd.Parameters.AddWithValue("@CodigoPostal", string.IsNullOrWhiteSpace(codigoPostal) ? DBNull.Value : codigoPostal);
                    cmd.Parameters.AddWithValue("@Depto", string.IsNullOrWhiteSpace(depto) ? DBNull.Value : depto);
                    cmd.Parameters.AddWithValue("@Piso", string.IsNullOrWhiteSpace(piso) ? DBNull.Value : piso);
                    cmd.Parameters.AddWithValue("@Provincia", provincia);
                    cmd.Parameters.AddWithValue("@Partido", string.IsNullOrWhiteSpace(partido) ? DBNull.Value : partido);
                    cmd.Parameters.AddWithValue("@Localidad", string.IsNullOrWhiteSpace(localidad) ? DBNull.Value : localidad);
                    cmd.Parameters.AddWithValue("@Latitud", latitud.HasValue ? latitud.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Longitud", longitud.HasValue ? longitud.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DireccionOriginal", string.IsNullOrWhiteSpace(direccionOriginal) ? DBNull.Value : direccionOriginal);
                    cmd.Parameters.AddWithValue("@DireccionNormalizada", string.IsNullOrWhiteSpace(direccionNormalizada) ? DBNull.Value : direccionNormalizada);

                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@IdRol", idRol);
                    cmd.Parameters.AddWithValue("@Usa2FA", usa2FA);

                    object resultado = cmd.ExecuteScalar();

                    return resultado != null ? Convert.ToInt32(resultado) : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public bool ActualizarUsuarioCompleto(
            int idUsuario,

            string usuario,
            string correo,
            int idRol,
            bool usa2FA,

            string nombre,
            string apellido,
            string dni,
            string telefono,
            DateTime fechaNacimiento,

            string calle,
            string numero,
            string codigoPostal,
            string depto,
            string piso,
            string provincia,
            string partido,
            string localidad,
            decimal? latitud,
            decimal? longitud,
            string direccionOriginal,
            string direccionNormalizada)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_ActualizarUsuarioCompleto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    cmd.Parameters.AddWithValue("@IdRol", idRol);
                    cmd.Parameters.AddWithValue("@Usa2FA", usa2FA);

                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Apellido", apellido);
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    cmd.Parameters.AddWithValue("@Telefono", string.IsNullOrWhiteSpace(telefono) ? DBNull.Value : telefono);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);

                    cmd.Parameters.AddWithValue("@Calle", calle);
                    cmd.Parameters.AddWithValue("@Numero", string.IsNullOrWhiteSpace(numero) ? DBNull.Value : numero);
                    cmd.Parameters.AddWithValue("@CodigoPostal", string.IsNullOrWhiteSpace(codigoPostal) ? DBNull.Value : codigoPostal);
                    cmd.Parameters.AddWithValue("@Depto", string.IsNullOrWhiteSpace(depto) ? DBNull.Value : depto);
                    cmd.Parameters.AddWithValue("@Piso", string.IsNullOrWhiteSpace(piso) ? DBNull.Value : piso);
                    cmd.Parameters.AddWithValue("@Provincia", provincia);
                    cmd.Parameters.AddWithValue("@Partido", string.IsNullOrWhiteSpace(partido) ? DBNull.Value : partido);
                    cmd.Parameters.AddWithValue("@Localidad", string.IsNullOrWhiteSpace(localidad) ? DBNull.Value : localidad);
                    cmd.Parameters.AddWithValue("@Latitud", latitud.HasValue ? latitud.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Longitud", longitud.HasValue ? longitud.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DireccionOriginal", string.IsNullOrWhiteSpace(direccionOriginal) ? DBNull.Value : direccionOriginal);
                    cmd.Parameters.AddWithValue("@DireccionNormalizada", string.IsNullOrWhiteSpace(direccionNormalizada) ? DBNull.Value : direccionNormalizada);

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool ActualizarPrimerLogin(int idUsuario, bool primerLogin)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_ActualizarPrimerLogin", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@PrimerLogin", primerLogin);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool AsignarRolUsuario(int idUsuario, int idRol)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_AsignarRolUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@IdRol", idRol);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch { return false; }
        }

        public int BuscarUsuarioPorNombreUser(string usuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_BuscarUsuarioPorNombreUser", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Usuario", usuario);

                    object resultado = cmd.ExecuteScalar();
                    return resultado != null ? Convert.ToInt32(resultado) : 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public DataTable BuscarUsuarioPorCorreo(string correo)
        {
            DataTable tabla = new DataTable();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_BuscarUsuarioPorCorreo", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Correo", correo);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(tabla);
                }
            }
            catch
            {
                tabla = new DataTable();
            }

            return tabla;
        }

        public string BuscarUsuarioPorId(int idUsuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_BuscarUsuarioPorId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);   

                    object resultado = cmd.ExecuteScalar();
                    return resultado != null ? resultado.ToString() : string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public DataTable BuscarPersonaPorPartido(string partido)
        {
            DataTable tabla = new DataTable();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_BuscarPersonaPorPartido", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Partido", partido);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(tabla);
                }
            }
            catch { }

            return tabla;
        }

        public DataTable BuscarPersonaPorLocalidad(string localidad)
        {
            DataTable tabla = new DataTable();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_BuscarPersonaPorLocalidad", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Localidad", localidad);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(tabla);
                }
            }
            catch { }

            return tabla;
        }

        public DataTable BuscarPersonaPorCP(string codigoPostal)
        {
            DataTable tabla = new DataTable();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_BuscarPersonaPorCP", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoPostal", codigoPostal);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(tabla);
                }
            }
            catch { }

            return tabla;
        }

        public bool ExisteUsuario(string usuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand(
                        "SP_ExisteUsuario",
                        conexion);

                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@Usuario",
                        usuario);

                    object resultado =
                        cmd.ExecuteScalar();

                    return Convert.ToBoolean(resultado);
                }
            }
            catch
            {
                return false;
            }
        }

        public bool ExisteCorreo(string correo)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_ExisteCorreo", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Correo", correo);

                    object resultado = cmd.ExecuteScalar();
                    return Convert.ToBoolean(resultado);
                }
            }
            catch
            {
                return false;
            }
        }
        public bool ExisteDNI(string dni)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_ExisteDNI", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DNI", dni);

                    object resultado = cmd.ExecuteScalar();
                    return Convert.ToBoolean(resultado);
                }
            }
            catch
            {
                return false;
            }
        }

        public bool UsuarioActivo(string usuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT Activo
                        FROM dbo.Usuarios
                        WHERE Usuario = @Usuario;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Usuario", usuario);

                    object result = cmd.ExecuteScalar();

                    return result != null && Convert.ToBoolean(result);
                }
            }
            catch
            {
                return false;
            }
        }

        public bool EsPrimerLogin(string usuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_VerificarPrimerLogin", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Usuario", usuario);

                    object resultado = cmd.ExecuteScalar();
                    return Convert.ToBoolean(resultado);
                }
            }
            catch
            {
                return false;
            }
        }

        public int ObtenerRolUsuario(string usuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_ObtenerRolUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Usuario", usuario);

                    object resultado = cmd.ExecuteScalar();
                    return resultado != null ? Convert.ToInt32(resultado) : 0;
                }
            }
            catch
            {
                return 0;
            }
        }
       
        public int RegistrarPersonaUsuarioConDireccion(
            string nombre,
            string apellido,
            string dni,
            string telefono,
            string calle,
            string numero,
            string codigoPostal,
            string depto,
            string piso,
            string provincia,
            string partido,
            string localidad,
            string direccionOriginal,
            string direccionNormalizada,
            DateTime fechaNacimiento,
            string usuario,
            string correo,
            string passwordHash,
            int idRol)
            {
                try
                {
                    using (SqlConnection conexion = conectar())
                    {
                        SqlCommand cmd = new SqlCommand("SP_RegistrarUsuarioCompleto", conexion);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Nombre", nombre);
                        cmd.Parameters.AddWithValue("@Apellido", apellido);
                        cmd.Parameters.AddWithValue("@DNI", dni);
                        cmd.Parameters.AddWithValue("@Telefono", telefono);
                        cmd.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);

                        cmd.Parameters.AddWithValue("@Calle", calle);
                        cmd.Parameters.AddWithValue("@Numero", numero);
                        cmd.Parameters.AddWithValue("@CodigoPostal", codigoPostal);
                        cmd.Parameters.AddWithValue("@Depto", depto);
                        cmd.Parameters.AddWithValue("@Piso", piso);
                        cmd.Parameters.AddWithValue("@Provincia", provincia);
                        cmd.Parameters.AddWithValue("@Partido", partido);
                        cmd.Parameters.AddWithValue("@Localidad", localidad);
                        cmd.Parameters.AddWithValue("@Latitud", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Longitud", DBNull.Value);
                        cmd.Parameters.AddWithValue("@DireccionOriginal", direccionOriginal);
                        cmd.Parameters.AddWithValue("@DireccionNormalizada", direccionNormalizada);

                        cmd.Parameters.AddWithValue("@Usuario", usuario);
                        cmd.Parameters.AddWithValue("@Correo", correo);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        cmd.Parameters.AddWithValue("@IdRol", idRol);
                        cmd.Parameters.AddWithValue("@Usa2FA", false);

                        object resultado = cmd.ExecuteScalar();

                        return resultado != null ? Convert.ToInt32(resultado) : 0;
                    }
                    }
                catch
                {
                    return 0;
                }
            }
    } 
}