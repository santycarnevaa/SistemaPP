using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_auditoriaDatos : CD_conexion
    {
        public List<AuditoriaCambio> Obtener(string tablaAfectada, int? idUsuario, DateTime? desde, DateTime? hasta)
        {
            List<AuditoriaCambio> registros = new List<AuditoriaCambio>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerAuditoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Tabla_Afectada", string.IsNullOrWhiteSpace(tablaAfectada) ? DBNull.Value : tablaAfectada);
                    cmd.Parameters.AddWithValue("@FK_Usuario", idUsuario.HasValue ? idUsuario.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Desde", desde.HasValue ? desde.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Hasta", hasta.HasValue ? hasta.Value : DBNull.Value);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        registros.Add(new AuditoriaCambio
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Tabla_Afectada = reader["Tabla_Afectada"].ToString(),
                            ID_Registro_Afectado = Convert.ToInt32(reader["ID_Registro_Afectado"]),
                            Accion = reader["Accion"].ToString(),
                            FK_Usuario = Convert.ToInt32(reader["FK_Usuario"]),
                            Fecha_Hora = Convert.ToDateTime(reader["Fecha_Hora"]),
                            Detalle_Cambio = reader["Detalle_Cambio"] == DBNull.Value ? null : reader["Detalle_Cambio"].ToString()
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_auditoriaDatos.Obtener", ex);
                return new List<AuditoriaCambio>();
            }

            return registros;
        }

        public bool RegistrarCambio(string tablaAfectada, int idRegistroAfectado, string accion, int idUsuario, string detalle = null)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_RegistrarCambioAuditoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Tabla_Afectada", tablaAfectada);
                    cmd.Parameters.AddWithValue("@ID_Registro_Afectado", idRegistroAfectado);
                    cmd.Parameters.AddWithValue("@Accion", accion);
                    cmd.Parameters.AddWithValue("@FK_Usuario", idUsuario);
                    cmd.Parameters.AddWithValue("@Detalle_Cambio", string.IsNullOrWhiteSpace(detalle) ? DBNull.Value : detalle);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_auditoriaDatos.RegistrarCambio", ex);
                return false;
            }
        }

        // Punto de entrada pensado para que CUALQUIER clase CD_*Datos.cs del
        // sistema (de cualquiera de los 8 grupos) deje rastro de un cambio sin
        // tener que instanciar nada ni depender de más clases de Grupo 1 que
        // esta. Nunca lanza una excepción hacia quien la llama: si falla,
        // queda solo en el log de errores — una auditoría que no se pudo
        // escribir no tiene por qué frenar la operación real del usuario.
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
