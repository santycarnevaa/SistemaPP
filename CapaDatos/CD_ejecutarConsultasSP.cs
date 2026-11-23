using System.Data;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    // esta clase ejecuta cualquier sp y se encarga de los errores
    // las clases de datos heredan de aca y solo arman los parametros y leen lo que vuelve
    public abstract class CD_ejecutarConsultasSP : CD_conexion
    {
        // si devuelve datos el sp trae filas y se cargan en la tabla
        // si no devuelve datos el sp solo modifica y la tabla vuelve vacia
        // tipo text es solo para los select que todavia no tienen su sp
        protected DataTable EjecutarConsulta(string nombreSP, SqlParameter[] parametros, bool devuelveDatos = true,
                                             CommandType tipo = CommandType.StoredProcedure)
        {
            DataTable tabla = new DataTable();

            try
            {
                using (SqlConnection conexion = conectar())   // conectar() ya la devuelve abierta
                using (SqlCommand cmd = new SqlCommand(nombreSP, conexion))
                {
                    cmd.CommandType = tipo;

                    if (parametros != null)
                        cmd.Parameters.AddRange(parametros);

                    if (devuelveDatos)
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                            tabla.Load(reader);
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                // 50000 es el raiserror del sp, el mensaje ya se entiende asi que se pasa igual
                throw new Exception(ex.Message, ex);
            }
            catch (SqlException ex)
            {
                // servidor apagado, base que no existe, sp mal escrito, etc
                throw new Exception("No se pudo acceder a la base de datos.\n" + ex.Message, ex);
            }

            return tabla;
        }

        // primer celda del resultado (lo que antes daba executescalar), null si no vino nada
        protected static object? PrimerValor(DataTable tabla)
        {
            return tabla.Rows.Count > 0 ? tabla.Rows[0][0] : null;
        }

        // si el texto esta vacio manda null a la base
        protected static object ValorONulo(string? texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? DBNull.Value : texto;
        }

        // true si el error lo tiro el raiserror del sp (codigo 50000)
        protected static bool EsErrorDelSP(Exception ex)
        {
            return ex.InnerException is SqlException sql && sql.Number == 50000;
        }
    }
}
