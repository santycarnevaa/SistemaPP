using System.Data;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_contraDatos : CD_ejecutarConsultasSP
    {
        public bool CambiarContra(int idUsuario, string nuevoPasswordHash, bool validarHistorial = true)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@NuevaPasswordHash", SqlDbType.VarChar, 256) { Value = nuevoPasswordHash },
                new SqlParameter("@ValidarHistorial", SqlDbType.Bit) { Value = validarHistorial }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_CambiarPassword", parametros);

                // el sp devuelve una fila con mensaje si salio bien o con error si fallo
                return tabla.Rows.Count > 0 && tabla.Rows[0][0] is string mensaje && mensaje.Contains("correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al cambiar la contraseña: " + ex.Message, ex);
            }
        }

        public bool insertarPassword(int idUsuario, string passwordHash)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@PasswordHash", SqlDbType.VarChar, 256) { Value = passwordHash }
            };

            try
            {
                // el sp tiene set nocount on y no devuelve las filas afectadas,
                // asi que si no tiro excepcion es porque se guardo
                EjecutarConsulta("dbo.SP_InsertarPassword", parametros, devuelveDatos: false);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al registrar contraseña temporal: " + ex.Message, ex);
            }
        }

        public List<string> obtenerContrasAnteriores(int idUsuario)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerContrasAnteriores", parametros);

                List<string> passwords = new List<string>();
                foreach (DataRow fila in tabla.Rows)
                    passwords.Add(fila.Field<string>("PasswordHash") ?? string.Empty);

                return passwords;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_contraDatos.obtenerContrasAnteriores", ex);
                return new List<string>();
            }
        }

        public string obtenerUltimoPasswordHash(int idUsuario)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerUltimoPasswordHash", parametros);

                return tabla.Rows.Count > 0 ? tabla.Rows[0].Field<string>("PasswordHash") ?? string.Empty : string.Empty;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_contraDatos.obtenerUltimoPasswordHash", ex);
                return string.Empty;
            }
        }

        public bool verificarLogin(string usuario, string passwordHash)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@Usuario", SqlDbType.VarChar, 50) { Value = usuario },
                new SqlParameter("@PasswordHash", SqlDbType.VarChar, 256) { Value = passwordHash }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_VerificarLogin", parametros);

                // el sp devuelve una sola celda con el count(*)
                return tabla.Rows.Count > 0 && tabla.Rows[0].Field<int>(0) > 0;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_contraDatos.verificarLogin", ex);
                return false;
            }
        }

        public bool actualizarPasswordYPrimerLogin(int idUsuario, string passwordHash)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@PasswordHash", SqlDbType.VarChar, 256) { Value = passwordHash }
            };

            try
            {
                EjecutarConsulta("dbo.SP_ActualizarPasswordYPrimerLogin", parametros, devuelveDatos: false);
                return true;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_contraDatos.actualizarPasswordYPrimerLogin", ex);
                return false;
            }
        }
    }
}
