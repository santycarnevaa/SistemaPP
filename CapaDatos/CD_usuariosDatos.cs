using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_usuariosDatos : CD_ejecutarConsultasSP
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
            string idLocalidad,
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
            SqlParameter[] parametros =
            {
                // persona
                new SqlParameter("@Nombre", SqlDbType.VarChar, 100) { Value = nombre },
                new SqlParameter("@Apellido", SqlDbType.VarChar, 100) { Value = apellido },
                new SqlParameter("@DNI", SqlDbType.VarChar, 20) { Value = dni },
                new SqlParameter("@Telefono", SqlDbType.VarChar, 30) { Value = ValorONulo(telefono) },
                new SqlParameter("@FechaNacimiento", SqlDbType.Date) { Value = fechaNacimiento },

                // direccion
                new SqlParameter("@Calle", SqlDbType.VarChar, 100) { Value = calle },
                new SqlParameter("@Numero", SqlDbType.VarChar, 20) { Value = ValorONulo(numero) },
                new SqlParameter("@CodigoPostal", SqlDbType.VarChar, 20) { Value = ValorONulo(codigoPostal) },
                new SqlParameter("@Depto", SqlDbType.VarChar, 20) { Value = ValorONulo(depto) },
                new SqlParameter("@Piso", SqlDbType.VarChar, 20) { Value = ValorONulo(piso) },
                new SqlParameter("@IdLocalidad", SqlDbType.VarChar, 30) { Value = idLocalidad },
                new SqlParameter("@Latitud", SqlDbType.Decimal) { Precision = 18, Scale = 10, Value = (object?)latitud ?? DBNull.Value },
                new SqlParameter("@Longitud", SqlDbType.Decimal) { Precision = 18, Scale = 10, Value = (object?)longitud ?? DBNull.Value },
                new SqlParameter("@DireccionOriginal", SqlDbType.VarChar, 255) { Value = ValorONulo(direccionOriginal) },
                new SqlParameter("@DireccionNormalizada", SqlDbType.VarChar, 255) { Value = ValorONulo(direccionNormalizada) },

                // usuario
                new SqlParameter("@Usuario", SqlDbType.VarChar, 50) { Value = usuario },
                new SqlParameter("@Correo", SqlDbType.VarChar, 100) { Value = correo },
                new SqlParameter("@PasswordHash", SqlDbType.VarChar, 255) { Value = passwordHash },
                new SqlParameter("@IdRol", SqlDbType.Int) { Value = idRol },
                new SqlParameter("@Usa2FA", SqlDbType.Bit) { Value = usa2FA }
            };

            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_RegistrarUsuarioCompleto", parametros));

                return resultado != null ? Convert.ToInt32(resultado) : -1;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.RegistrarUsuarioCompleto", ex);
                throw new Exception("Error SQL al registrar usuario: " + ex.Message, ex);
            }
        }

        public bool AsignarRolUsuario(int idUsuario, int idRol)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@IdRol", SqlDbType.Int) { Value = idRol }
            };

            try
            {
                EjecutarConsulta("dbo.SP_AsignarRolUsuario", parametros, devuelveDatos: false);
                return true;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.AsignarRolUsuario", ex);
                return false;
            }
        }

        public List<rol> ListarRoles()
        {
            DataTable tabla = EjecutarConsulta("dbo.SP_ListarRoles", null);

            List<rol> roles = new List<rol>();
            foreach (DataRow fila in tabla.Rows)
            {
                roles.Add(new rol
                {
                    IdRol = Convert.ToInt32(fila["IdRol"]),
                    NombreRol = fila["NombreRol"].ToString()
                });
            }

            return roles;
        }

        public bool AgregarRol(string nombreRol)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@NombreRol", SqlDbType.VarChar, 50) { Value = nombreRol }
            };

            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_AgregarRol", parametros));

                return resultado != null && Convert.ToInt32(resultado) > 0;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.AgregarRol", ex);
                return false;
            }
        }

        // devuelve vacio si salio bien o el mensaje de la base (por ej si ya existe el rol)
        public string ModificarRol(int idRol, string nombreRol)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdRol", SqlDbType.Int) { Value = idRol },
                new SqlParameter("@NombreRol", SqlDbType.VarChar, 50) { Value = nombreRol }
            };

            try
            {
                EjecutarConsulta("dbo.SP_ModificarRol", parametros, devuelveDatos: false);
                return "";
            }
            catch (Exception ex) when (EsErrorDelSP(ex))
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.ModificarRol", ex);
                return "No se pudo modificar el rol. Intente nuevamente.";
            }
        }

        public string EliminarRol(int idRol)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdRol", SqlDbType.Int) { Value = idRol }
            };

            try
            {
                EjecutarConsulta("dbo.SP_EliminarRol", parametros, devuelveDatos: false);
                return "";
            }
            catch (Exception ex) when (EsErrorDelSP(ex))
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.EliminarRol", ex);
                return "No se pudo dar de baja el rol. Intente nuevamente.";
            }
        }

        public int BuscarUsuarioPorNombreUser(string usuario)
        {
            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_BuscarUsuarioPorNombreUser", ParametroUsuario(usuario)));

                return resultado != null ? Convert.ToInt32(resultado) : 0;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.BuscarUsuarioPorNombreUser", ex);
                return 0;
            }
        }

        public DataTable BuscarUsuarioPorCorreo(string correo)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@Correo", SqlDbType.VarChar, 100) { Value = correo }
            };

            try
            {
                return EjecutarConsulta("dbo.SP_BuscarUsuarioPorCorreo", parametros);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.BuscarUsuarioPorCorreo", ex);
                return new DataTable();
            }
        }

        public string BuscarUsuarioPorId(int idUsuario)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario }
            };

            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_BuscarUsuarioPorId", parametros));

                return resultado != null ? resultado.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.BuscarUsuarioPorId", ex);
                return string.Empty;
            }
        }

        public DataTable BuscarPersonaPorPartido(string partido)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@Partido", SqlDbType.VarChar, 100) { Value = partido }
            };

            try
            {
                return EjecutarConsulta("dbo.SP_BuscarPersonaPorPartido", parametros);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.BuscarPersonaPorPartido", ex);
                return new DataTable();
            }
        }

        public DataTable BuscarPersonaPorLocalidad(string localidad)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@Localidad", SqlDbType.VarChar, 100) { Value = localidad }
            };

            try
            {
                return EjecutarConsulta("dbo.SP_BuscarPersonaPorLocalidad", parametros);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.BuscarPersonaPorLocalidad", ex);
                return new DataTable();
            }
        }

        public DataTable BuscarPersonaPorCP(string codigoPostal)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@CodigoPostal", SqlDbType.VarChar, 20) { Value = codigoPostal }
            };

            try
            {
                return EjecutarConsulta("dbo.SP_BuscarPersonaPorCP", parametros);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.BuscarPersonaPorCP", ex);
                return new DataTable();
            }
        }

        public bool ExisteUsuario(string usuario)
        {
            try
            {
                return Convert.ToBoolean(PrimerValor(EjecutarConsulta("dbo.SP_ExisteUsuario", ParametroUsuario(usuario))));
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.ExisteUsuario", ex);
                return false;
            }
        }

        public bool ExisteCorreo(string correo)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@Correo", SqlDbType.VarChar, 100) { Value = correo }
            };

            try
            {
                return Convert.ToBoolean(PrimerValor(EjecutarConsulta("dbo.SP_ExisteCorreo", parametros)));
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.ExisteCorreo", ex);
                return false;
            }
        }

        public bool ExisteDNI(string dni)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@DNI", SqlDbType.VarChar, 20) { Value = dni }
            };

            try
            {
                return Convert.ToBoolean(PrimerValor(EjecutarConsulta("dbo.SP_ExisteDNI", parametros)));
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.ExisteDNI", ex);
                return false;
            }
        }

        public bool UsuarioActivo(string usuario)
        {
            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_UsuarioActivo", ParametroUsuario(usuario)));

                return resultado != null && Convert.ToBoolean(resultado);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.UsuarioActivo", ex);
                return false;
            }
        }

        // nombre, apellido, dni y fecha de nacimiento del usuario, se usan
        // en cl_serviciocontras para que la contraseña no los tenga
        public (string nombre, string apellido, string dni, DateTime? fechaNacimiento) ObtenerDatosPersonalesPorUsuario(string usuario)
        {
            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerDatosPersonalesPorUsuario", ParametroUsuario(usuario));

                if (tabla.Rows.Count == 0)
                    return (string.Empty, string.Empty, string.Empty, null);

                DataRow fila = tabla.Rows[0];
                DateTime? fechaNacimiento = fila["FechaNacimiento"] == DBNull.Value
                    ? (DateTime?)null
                    : Convert.ToDateTime(fila["FechaNacimiento"]);

                return (fila["Nombre"].ToString(), fila["Apellido"].ToString(), fila["DNI"].ToString(), fechaNacimiento);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.ObtenerDatosPersonalesPorUsuario", ex);
                return (string.Empty, string.Empty, string.Empty, null);
            }
        }

        public bool EsPrimerLogin(string usuario)
        {
            try
            {
                return Convert.ToBoolean(PrimerValor(EjecutarConsulta("dbo.SP_EsPrimerLogin", ParametroUsuario(usuario))));
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.EsPrimerLogin", ex);
                return false;
            }
        }

        // rol de un usuario activo por su id (0 si no existe o esta inactivo)
        public int ObtenerRolPorId(int idUsuario)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario }
            };

            try
            {
                object? resultado = PrimerValor(EjecutarConsulta(
                    "SELECT IdRol FROM dbo.Usuarios WHERE IdUsuario = @IdUsuario AND Activo = 1",
                    parametros, tipo: CommandType.Text));

                return resultado == null || resultado == DBNull.Value ? 0 : Convert.ToInt32(resultado);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.ObtenerRolPorId", ex);
                return 0;
            }
        }

        public int ObtenerRolUsuario(string usuario)
        {
            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_ObtenerRolPorUsuario", ParametroUsuario(usuario)));

                if (resultado == null || resultado == DBNull.Value)
                    return 0;

                return Convert.ToInt32(resultado);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.ObtenerRolUsuario", ex);
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
            string idLocalidad,
            string direccionOriginal,
            string direccionNormalizada,
            DateTime fechaNacimiento,
            string usuario,
            string correo,
            string passwordHash,
            int idRol)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@Nombre", SqlDbType.VarChar, 100) { Value = nombre },
                new SqlParameter("@Apellido", SqlDbType.VarChar, 100) { Value = apellido },
                new SqlParameter("@DNI", SqlDbType.VarChar, 20) { Value = dni },
                new SqlParameter("@Telefono", SqlDbType.VarChar, 30) { Value = (object?)telefono ?? DBNull.Value },
                new SqlParameter("@FechaNacimiento", SqlDbType.Date) { Value = fechaNacimiento },

                new SqlParameter("@Calle", SqlDbType.VarChar, 100) { Value = calle },
                new SqlParameter("@Numero", SqlDbType.VarChar, 20) { Value = (object?)numero ?? DBNull.Value },
                new SqlParameter("@CodigoPostal", SqlDbType.VarChar, 20) { Value = (object?)codigoPostal ?? DBNull.Value },
                new SqlParameter("@Depto", SqlDbType.VarChar, 20) { Value = (object?)depto ?? DBNull.Value },
                new SqlParameter("@Piso", SqlDbType.VarChar, 20) { Value = (object?)piso ?? DBNull.Value },
                new SqlParameter("@IdLocalidad", SqlDbType.VarChar, 30) { Value = idLocalidad },
                new SqlParameter("@Latitud", SqlDbType.Decimal) { Precision = 18, Scale = 10, Value = DBNull.Value },
                new SqlParameter("@Longitud", SqlDbType.Decimal) { Precision = 18, Scale = 10, Value = DBNull.Value },
                new SqlParameter("@DireccionOriginal", SqlDbType.VarChar, 255) { Value = (object?)direccionOriginal ?? DBNull.Value },
                new SqlParameter("@DireccionNormalizada", SqlDbType.VarChar, 255) { Value = (object?)direccionNormalizada ?? DBNull.Value },

                new SqlParameter("@Usuario", SqlDbType.VarChar, 50) { Value = usuario },
                new SqlParameter("@Correo", SqlDbType.VarChar, 100) { Value = correo },
                new SqlParameter("@PasswordHash", SqlDbType.VarChar, 255) { Value = passwordHash },
                new SqlParameter("@IdRol", SqlDbType.Int) { Value = idRol },
                new SqlParameter("@Usa2FA", SqlDbType.Bit) { Value = false }
            };

            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_RegistrarUsuarioCompleto", parametros));

                return resultado != null ? Convert.ToInt32(resultado) : 0;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.RegistrarPersonaUsuarioConDireccion", ex);
                return 0;
            }
        }

        public bool CambiarEstadoUsuario(int idUsuario, bool activo)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@Activo", SqlDbType.Bit) { Value = activo }
            };

            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_ActualizarEstadoUsuario", parametros));

                return resultado != null && Convert.ToInt32(resultado) == 1;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.CambiarEstadoUsuario", ex);
                throw new Exception("Error al cambiar estado del usuario: " + ex.Message, ex);
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
            string idLocalidad,
            decimal? latitud,
            decimal? longitud,
            string direccionOriginal,
            string direccionNormalizada,

            string correo,
            int idRol)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },

                new SqlParameter("@Nombre", SqlDbType.VarChar, 100) { Value = nombre },
                new SqlParameter("@Apellido", SqlDbType.VarChar, 100) { Value = apellido },
                new SqlParameter("@DNI", SqlDbType.VarChar, 20) { Value = dni },
                new SqlParameter("@Telefono", SqlDbType.VarChar, 30) { Value = (object?)telefono ?? DBNull.Value },
                new SqlParameter("@FechaNacimiento", SqlDbType.Date) { Value = fechaNacimiento },

                new SqlParameter("@Calle", SqlDbType.VarChar, 100) { Value = calle },
                new SqlParameter("@Numero", SqlDbType.VarChar, 20) { Value = (object?)numero ?? DBNull.Value },
                new SqlParameter("@CodigoPostal", SqlDbType.VarChar, 20) { Value = (object?)codigoPostal ?? DBNull.Value },
                new SqlParameter("@Depto", SqlDbType.VarChar, 20) { Value = ValorONulo(depto) },
                new SqlParameter("@Piso", SqlDbType.VarChar, 20) { Value = ValorONulo(piso) },
                new SqlParameter("@IdLocalidad", SqlDbType.VarChar, 30) { Value = idLocalidad },
                new SqlParameter("@Latitud", SqlDbType.Decimal) { Precision = 18, Scale = 10, Value = (object?)latitud ?? DBNull.Value },
                new SqlParameter("@Longitud", SqlDbType.Decimal) { Precision = 18, Scale = 10, Value = (object?)longitud ?? DBNull.Value },
                new SqlParameter("@DireccionOriginal", SqlDbType.VarChar, 255) { Value = (object?)direccionOriginal ?? DBNull.Value },
                new SqlParameter("@DireccionNormalizada", SqlDbType.VarChar, 255) { Value = (object?)direccionNormalizada ?? DBNull.Value },

                new SqlParameter("@Correo", SqlDbType.VarChar, 100) { Value = correo },
                new SqlParameter("@IdRol", SqlDbType.Int) { Value = idRol }
            };

            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_ActualizarDatosUsuario", parametros));

                return resultado != null && Convert.ToInt32(resultado) == 1;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.ActualizarDatosUsuario", ex);
                throw new Exception("Error SQL al actualizar usuario: " + ex.Message, ex);
            }
        }

        public string ObtenerCorreoPorUsuario(string usuario)
        {
            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_ObtenerCorreoPorUsuario", ParametroUsuario(usuario)));

                return resultado != null ? resultado.ToString() : "";
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.ObtenerCorreoPorUsuario", ex);
                return "";
            }
        }

        public List<UsuarioGrilla> ObtenerUsuariosGrilla()
        {
            try
            {
                return ConvertirAUsuariosGrilla(EjecutarConsulta("dbo.SP_BuscarUsuariosGrilla", null));
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_usuariosDatos.ObtenerUsuariosGrilla", ex);
                throw new Exception("Error SQL al obtener usuarios para la grilla: " + ex.Message, ex);
            }
        }

        public List<UsuarioGrilla> BuscarUsuariosParaGrilla(string texto, string filtro)
        {
            bool? activo = null;
            int? idRol = null;

            if (filtro == "Usuarios Activos")
                activo = true;
            else if (filtro == "Usuarios Inactivos")
                activo = false;
            else if (filtro == "Rol Usuario")
                idRol = 1;
            else if (filtro == "Rol Administrador")
                idRol = 2;

            SqlParameter[] parametros =
            {
                new SqlParameter("@Busqueda", SqlDbType.NVarChar, 100) { Value = texto ?? "" },
                new SqlParameter("@Activo", SqlDbType.Bit) { Value = (object?)activo ?? DBNull.Value },
                new SqlParameter("@IdRol", SqlDbType.Int) { Value = (object?)idRol ?? DBNull.Value }
            };

            return ConvertirAUsuariosGrilla(EjecutarConsulta("dbo.SP_BuscarUsuariosGrilla", parametros));
        }

        // varios sp reciben solo el usuario (varchar(50))
        private static SqlParameter[] ParametroUsuario(string usuario)
        {
            return new[] { new SqlParameter("@Usuario", SqlDbType.VarChar, 50) { Value = usuario } };
        }

        // pasa las filas de la grilla de usuarios a objetos usuariogrilla
        private static List<UsuarioGrilla> ConvertirAUsuariosGrilla(DataTable tabla)
        {
            List<UsuarioGrilla> lista = new List<UsuarioGrilla>();

            foreach (DataRow fila in tabla.Rows)
            {
                lista.Add(new UsuarioGrilla
                {
                    IdUsuario = Convert.ToInt32(fila["IdUsuario"]),
                    IdPersona = Convert.ToInt32(fila["IdPersona"]),
                    IdDireccion = fila["IdDireccion"] == DBNull.Value ? 0 : Convert.ToInt32(fila["IdDireccion"]),

                    Nombre = fila["Nombre"].ToString(),
                    Apellido = fila["Apellido"].ToString(),
                    DNI = fila["DNI"].ToString(),
                    Telefono = fila["Telefono"].ToString(),

                    Usuario = fila["Usuario"].ToString(),
                    Correo = fila["Correo"].ToString(),
                    IdRol = Convert.ToInt32(fila["IdRol"]),
                    Rol = fila["Rol"].ToString(),

                    Activo = Convert.ToBoolean(fila["Activo"]),
                    Estado = fila["Estado"].ToString(),

                    Calle = fila["Calle"].ToString(),
                    Numero = fila["Numero"].ToString(),
                    CodigoPostal = fila["CodigoPostal"].ToString(),
                    Depto = fila["Depto"].ToString(),
                    Piso = fila["Piso"].ToString(),
                    Provincia = fila["Provincia"].ToString(),
                    Partido = fila["Partido"].ToString(),
                    Localidad = fila["Localidad"].ToString(),

                    FechaNacimiento = Convert.ToDateTime(fila["FechaNacimiento"])
                });
            }

            return lista;
        }
    }
}
