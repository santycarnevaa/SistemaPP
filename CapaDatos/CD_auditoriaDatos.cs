using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_auditoriaDatos : CD_ejecutarConsultasSP
    {
        public List<AuditoriaCambio> Obtener(string tablaAfectada, int? idUsuario, DateTime? desde, DateTime? hasta)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@Tabla_Afectada", SqlDbType.VarChar, 100) { Value = ValorONulo(tablaAfectada) },
                new SqlParameter("@FK_Usuario", SqlDbType.Int) { Value = (object?)idUsuario ?? DBNull.Value },
                new SqlParameter("@Desde", SqlDbType.DateTime) { Value = (object?)desde ?? DBNull.Value },
                new SqlParameter("@Hasta", SqlDbType.DateTime) { Value = (object?)hasta ?? DBNull.Value }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerAuditoria", parametros);

                List<AuditoriaCambio> registros = new List<AuditoriaCambio>();
                foreach (DataRow fila in tabla.Rows)
                {
                    registros.Add(new AuditoriaCambio
                    {
                        ID = Convert.ToInt32(fila["ID"]),
                        Tabla_Afectada = fila["Tabla_Afectada"].ToString(),
                        ID_Registro_Afectado = Convert.ToInt32(fila["ID_Registro_Afectado"]),
                        Accion = fila["Accion"].ToString(),
                        FK_Usuario = Convert.ToInt32(fila["FK_Usuario"]),
                        Fecha_Hora = Convert.ToDateTime(fila["Fecha_Hora"]),
                        Detalle_Cambio = fila["Detalle_Cambio"] == DBNull.Value ? null : fila["Detalle_Cambio"].ToString(),
                        Usuario = fila["Usuario"].ToString()
                    });
                }

                return registros;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_auditoriaDatos.Obtener", ex);
                return new List<AuditoriaCambio>();
            }
        }

        // tablas que ya tienen algun registro de auditoria, asi el filtro se arma solo
        // y no hay que mantener una lista fija
        public List<string> ObtenerTablasAuditadas()
        {
            try
            {
                DataTable tabla = EjecutarConsulta(
                    "SELECT DISTINCT Tabla_Afectada FROM dbo.Auditoria_Cambios ORDER BY Tabla_Afectada",
                    null, tipo: CommandType.Text);

                List<string> tablas = new List<string>();
                foreach (DataRow fila in tabla.Rows)
                    tablas.Add(fila["Tabla_Afectada"].ToString());

                return tablas;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_auditoriaDatos.ObtenerTablasAuditadas", ex);
                return new List<string>();
            }
        }

        public List<KeyValuePair<int, string>> ObtenerUsuarios()
        {
            try
            {
                DataTable tabla = EjecutarConsulta(
                    "SELECT IdUsuario, Usuario FROM dbo.Usuarios ORDER BY Usuario",
                    null, tipo: CommandType.Text);

                List<KeyValuePair<int, string>> usuarios = new List<KeyValuePair<int, string>>();
                foreach (DataRow fila in tabla.Rows)
                    usuarios.Add(new KeyValuePair<int, string>(Convert.ToInt32(fila["IdUsuario"]), fila["Usuario"].ToString()));

                return usuarios;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_auditoriaDatos.ObtenerUsuarios", ex);
                return new List<KeyValuePair<int, string>>();
            }
        }

        public bool RegistrarCambio(string tablaAfectada, int idRegistroAfectado, string accion, int idUsuario, string detalle = null)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@Tabla_Afectada", SqlDbType.VarChar, 100) { Value = tablaAfectada },
                new SqlParameter("@ID_Registro_Afectado", SqlDbType.Int) { Value = idRegistroAfectado },
                new SqlParameter("@Accion", SqlDbType.VarChar, 20) { Value = accion },
                new SqlParameter("@FK_Usuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@Detalle_Cambio", SqlDbType.VarChar, -1) { Value = ValorONulo(detalle) }
            };

            try
            {
                EjecutarConsulta("dbo.SP_RegistrarCambioAuditoria", parametros, devuelveDatos: false);
                return true;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_auditoriaDatos.RegistrarCambio", ex);
                return false;
            }
        }

        // cualquier clase de datos puede llamar a esto para dejar registrado un cambio
        // sin tener que crear nada. nunca tira excepcion, si falla solo queda en el log,
        // porque si no se pudo guardar la auditoria no tiene sentido frenar lo que hacia el usuario
        public static void Registrar(string tablaAfectada, int idRegistroAfectado, string accion, int idUsuario, string detalle = null)
        {
            try
            {
                new CD_auditoriaDatos().RegistrarCambio(tablaAfectada, idRegistroAfectado, accion, idUsuario, detalle);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_auditoriaDatos.Registrar", ex);
            }
        }
    }
}
