using Entidades;
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
            catch (Exception ex)
            {
                throw new Exception("Error SQL al registrar usuario: " + ex.Message, ex);
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
                    SqlCommand cmd = new SqlCommand("SP_EsPrimerLogin", conexion);
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
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerRolUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Usuario", usuario);

                    object resultado = cmd.ExecuteScalar();

                    if (resultado == null || resultado == DBNull.Value)
                        return 0;

                    return Convert.ToInt32(resultado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al obtener el rol del usuario: " + ex.Message, ex);
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
            public bool ActualizarEstadoUsuario(int idUsuario, bool activo)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ActualizarEstadoUsuario", conexion);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@Activo", activo);

                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool ActualizarDatosUsuario(
    int idUsuario,

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

    string correo,
    int idRol)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ActualizarDatosUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Apellido", apellido);
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    cmd.Parameters.AddWithValue("@Telefono", telefono);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);

                    cmd.Parameters.AddWithValue("@Calle", calle);
                    cmd.Parameters.AddWithValue("@Numero", numero);
                    cmd.Parameters.AddWithValue("@CodigoPostal", codigoPostal);
                    cmd.Parameters.AddWithValue("@Depto", string.IsNullOrWhiteSpace(depto) ? DBNull.Value : depto);
                    cmd.Parameters.AddWithValue("@Piso", string.IsNullOrWhiteSpace(piso) ? DBNull.Value : piso);
                    cmd.Parameters.AddWithValue("@Provincia", provincia);
                    cmd.Parameters.AddWithValue("@Partido", partido);
                    cmd.Parameters.AddWithValue("@Localidad", localidad);
                    cmd.Parameters.AddWithValue("@Latitud", latitud.HasValue ? latitud.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Longitud", longitud.HasValue ? longitud.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DireccionOriginal", direccionOriginal);
                    cmd.Parameters.AddWithValue("@DireccionNormalizada", direccionNormalizada);

                    cmd.Parameters.AddWithValue("@Correo", correo);
                    cmd.Parameters.AddWithValue("@IdRol", idRol);

                    object resultado = cmd.ExecuteScalar();

                    return resultado != null && Convert.ToInt32(resultado) == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al actualizar usuario: " + ex.Message, ex);
            }
        }
        public string ObtenerCorreoPorUsuario(string usuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerCorreoPorUsuario", conexion);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Usuario", usuario);

                    object resultado = cmd.ExecuteScalar();

                    return resultado != null ? resultado.ToString() : "";
                }
            }
            catch
            {
                return "";
            }
        }
        public List<UsuarioGrilla> ObtenerUsuariosGrilla()
        {
            List<UsuarioGrilla> lista = new List<UsuarioGrilla>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerUsuariosGrilla", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        lista.Add(new UsuarioGrilla
                        {
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            IdPersona = Convert.ToInt32(reader["IdPersona"]),
                            IdDireccion = reader["IdDireccion"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdDireccion"]),

                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            DNI = reader["DNI"].ToString(),
                            Telefono = reader["Telefono"].ToString(),

                            Usuario = reader["Usuario"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            IdRol = Convert.ToInt32(reader["IdRol"]),
                            Rol = reader["Rol"].ToString(),

                            Activo = Convert.ToBoolean(reader["Activo"]),
                            Estado = reader["Estado"].ToString(),

                            Calle = reader["Calle"].ToString(),
                            Numero = reader["Numero"].ToString(),
                            CodigoPostal = reader["CodigoPostal"].ToString(),
                            Depto = reader["Depto"].ToString(),
                            Piso = reader["Piso"].ToString(),
                            Provincia = reader["Provincia"].ToString(),
                            Partido = reader["Partido"].ToString(),
                            Localidad = reader["Localidad"].ToString(),

                            FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"])
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al obtener usuarios para la grilla: " + ex.Message, ex);
            }

            return lista;
        }
        public List<UsuarioGrilla> obtenerUsuariosGrilla()
        {
            List<UsuarioGrilla> lista = new List<UsuarioGrilla>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerUsuariosGrilla", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        lista.Add(new UsuarioGrilla
                        {
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            IdPersona = Convert.ToInt32(reader["IdPersona"]),
                            IdDireccion = reader["IdDireccion"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdDireccion"]),

                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            DNI = reader["DNI"].ToString(),
                            Telefono = reader["Telefono"].ToString(),

                            Usuario = reader["Usuario"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            IdRol = Convert.ToInt32(reader["IdRol"]),
                            Rol = reader["Rol"].ToString(),

                            Activo = Convert.ToBoolean(reader["Activo"]),
                            Estado = reader["Estado"].ToString(),

                            Calle = reader["Calle"].ToString(),
                            Numero = reader["Numero"].ToString(),
                            CodigoPostal = reader["CodigoPostal"].ToString(),
                            Depto = reader["Depto"].ToString(),
                            Piso = reader["Piso"].ToString(),
                            Provincia = reader["Provincia"].ToString(),
                            Partido = reader["Partido"].ToString(),
                            Localidad = reader["Localidad"].ToString(),

                            FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"])
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al obtener usuarios para la grilla: " + ex.Message, ex);
            }

            return lista;
        }
    }
} 