using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class CD_usuariosDatos : CD_conexion
    {
        public int insertarUsuario(
            string usuario,
            string nombre,
            string apellido,
            string correo,
            DateTime fechaNacimiento,
            int idRol)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        INSERT INTO dbo.Usuarios
                        (
                            Usuario,
                            Nombre,
                            Apellido,
                            Correo,
                            FechaNacimiento,
                            IdRol,
                            Activo,
                            PrimerLogin,
                            FechaAlta
                        )
                        VALUES
                        (
                            @Usuario,
                            @Nombre,
                            @Apellido,
                            @Correo,
                            @FechaNacimiento,
                            @IdRol,
                            1,
                            1,
                            GETDATE()
                        );

                        SELECT CAST(SCOPE_IDENTITY() AS INT);
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);

                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Apellido", apellido);
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);
                    cmd.Parameters.AddWithValue("@IdRol", idRol);

                    object result = cmd.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public bool actualizarDatosUsuario(
            int idUsuario,
            string nombre,
            string apellido,
            string correo,
            DateTime fechaNacimiento,
            int idRol)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        UPDATE dbo.Usuarios
                        SET
                            Nombre = @Nombre,
                            Apellido = @Apellido,
                            Correo = @Correo,
                            FechaNacimiento = @FechaNacimiento,
                            IdRol = @IdRol
                        WHERE IdUsuario = @IdUsuario;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Apellido", apellido);
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);
                    cmd.Parameters.AddWithValue("@IdRol", idRol);

                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool actualizarEstadoUsuario(int idUsuario, bool activo)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        UPDATE dbo.Usuarios
                        SET Activo = @Activo
                        WHERE IdUsuario = @IdUsuario;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);

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

        public bool actualizarPrimerLogin(int idUsuario, bool primerLogin)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        UPDATE dbo.Usuarios
                        SET PrimerLogin = @PrimerLogin
                        WHERE IdUsuario = @IdUsuario;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@PrimerLogin", primerLogin);

                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public int obtenerIdUsuario(string usuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT IdUsuario
                        FROM dbo.Usuarios
                        WHERE Usuario = @Usuario;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Usuario", usuario);

                    object result = cmd.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public string obtenerUsuarioPorId(int idUsuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT Usuario
                        FROM dbo.Usuarios
                        WHERE IdUsuario = @IdUsuario;
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

        public bool existeUsuario(string usuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT COUNT(*)
                        FROM dbo.Usuarios
                        WHERE Usuario = @Usuario;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Usuario", usuario);

                    int cantidad = Convert.ToInt32(cmd.ExecuteScalar());

                    return cantidad > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool existeCorreo(string correo)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT COUNT(*)
                        FROM dbo.Usuarios
                        WHERE Correo = @Correo;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Correo", correo);

                    int cantidad = Convert.ToInt32(cmd.ExecuteScalar());

                    return cantidad > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool usuarioActivo(string usuario)
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

        public bool esPrimerLogin(string usuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT PrimerLogin
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

        public int obtenerRolUsuario(string usuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT IdRol
                        FROM dbo.Usuarios
                        WHERE Usuario = @Usuario;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Usuario", usuario);

                    object result = cmd.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
            catch
            {
                return -1;
            }
        }
        public int RegistrarPersonaUsuarioConDireccion(
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

    string nombre,
    string apellido,
    string dni,
    string telefono,
    DateTime fechaNacimiento,

    string usuario,
    string correo,
    int idRol)
        {
            using (SqlConnection conexion = conectar())
            using (SqlTransaction tran = conexion.BeginTransaction())
            {
                try
                {
                    int idDireccion;

                    string insertDireccionSql = @"
                INSERT INTO dbo.Direcciones
                (
                    Calle,
                    Numero,
                    CodigoPostal,
                    Depto,
                    Piso,
                    Provincia,
                    Partido,
                    Localidad,
                    Latitud,
                    Longitud,
                    DireccionOriginal,
                    DireccionNormalizada,
                    FechaAlta
                )
                VALUES
                (
                    @Calle,
                    @Numero,
                    @CodigoPostal,
                    @Depto,
                    @Piso,
                    @Provincia,
                    @Partido,
                    @Localidad,
                    NULL,
                    NULL,
                    @DireccionOriginal,
                    @DireccionNormalizada,
                    GETDATE()
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

                    using (SqlCommand cmdDireccion = new SqlCommand(insertDireccionSql, conexion, tran))
                    {
                        cmdDireccion.Parameters.AddWithValue("@Calle", calle);
                        cmdDireccion.Parameters.AddWithValue("@Numero", numero);
                        cmdDireccion.Parameters.AddWithValue("@CodigoPostal", codigoPostal);
                        cmdDireccion.Parameters.AddWithValue("@Depto", string.IsNullOrWhiteSpace(depto) ? DBNull.Value : depto);
                        cmdDireccion.Parameters.AddWithValue("@Piso", string.IsNullOrWhiteSpace(piso) ? DBNull.Value : piso);
                        cmdDireccion.Parameters.AddWithValue("@Provincia", provincia);
                        cmdDireccion.Parameters.AddWithValue("@Partido", partido);
                        cmdDireccion.Parameters.AddWithValue("@Localidad", localidad);
                        cmdDireccion.Parameters.AddWithValue("@DireccionOriginal", direccionOriginal);
                        cmdDireccion.Parameters.AddWithValue("@DireccionNormalizada", direccionNormalizada);

                        idDireccion = Convert.ToInt32(cmdDireccion.ExecuteScalar());
                    }

                    int idPersona;

                    string insertPersonaSql = @"
                INSERT INTO dbo.Personas
                (
                    Nombre,
                    Apellido,
                    DNI,
                    Telefono,
                    FechaNacimiento,
                    IdDireccion,
                    FechaAlta
                )
                VALUES
                (
                    @Nombre,
                    @Apellido,
                    @DNI,
                    @Telefono,
                    @FechaNacimiento,
                    @IdDireccion,
                    GETDATE()
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

                    using (SqlCommand cmdPersona = new SqlCommand(insertPersonaSql, conexion, tran))
                    {
                        cmdPersona.Parameters.AddWithValue("@Nombre", nombre);
                        cmdPersona.Parameters.AddWithValue("@Apellido", apellido);
                        cmdPersona.Parameters.AddWithValue("@DNI", dni);
                        cmdPersona.Parameters.AddWithValue("@Telefono", telefono);
                        cmdPersona.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);
                        cmdPersona.Parameters.AddWithValue("@IdDireccion", idDireccion);

                        idPersona = Convert.ToInt32(cmdPersona.ExecuteScalar());
                    }

                    int idUsuario;

                    string insertUsuarioSql = @"
                INSERT INTO dbo.Usuarios
                (
                    IdPersona,
                    Usuario,
                    Correo,
                    IdRol,
                    Activo,
                    PrimerLogin,
                    FechaAlta
                )
                VALUES
                (
                    @IdPersona,
                    @Usuario,
                    @Correo,
                    @IdRol,
                    1,
                    1,
                    GETDATE()
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

                    using (SqlCommand cmdUsuario = new SqlCommand(insertUsuarioSql, conexion, tran))
                    {
                        cmdUsuario.Parameters.AddWithValue("@IdPersona", idPersona);
                        cmdUsuario.Parameters.AddWithValue("@Usuario", usuario);
                        cmdUsuario.Parameters.AddWithValue("@Correo", correo);
                        cmdUsuario.Parameters.AddWithValue("@IdRol", idRol);

                        idUsuario = Convert.ToInt32(cmdUsuario.ExecuteScalar());
                    }

                    tran.Commit();
                    return idUsuario;
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    throw new Exception("Error al registrar usuario en base de datos: " + ex.Message, ex);
                }
            }
        }
    }
}