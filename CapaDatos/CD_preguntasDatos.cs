using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    // Acceso a datos de preguntas y respuestas de seguridad.
    public class CD_preguntasDatos : CD_conexion
    {
        // Trae las 15 preguntas del sistema, sin filtrar por usuario.
        // Se usa para mostrarle a un usuario el pool completo la primera vez que tiene que responderlas.
        public List<PreguntaSeguridad> ObtenerPreguntasDisponibles()
        {
            List<PreguntaSeguridad> preguntas = new List<PreguntaSeguridad>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_ListarPreguntasSeguridad", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        preguntas.Add(new PreguntaSeguridad
                        {
                            IdPregunta = Convert.ToInt32(reader["IdPregunta"]),
                            Pregunta = reader["Pregunta"].ToString(),
                            RespuestaHash = string.Empty
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_preguntasDatos.ObtenerPreguntasDisponibles", ex);
                return new List<PreguntaSeguridad>();
            }

            return preguntas;
        }

        // Trae únicamente las preguntas que ESE usuario ya respondió, junto con el hash guardado
        // de cada respuesta. El filtro por usuario lo hace el SP (WHERE IdUsuario = @IdUsuario);
        // se usa en la recuperación de contraseña, para saber qué preguntarle a cada quien.
        public List<PreguntaSeguridad> obtenerPreguntasUsuario(int idUsuario)
        {
            List<PreguntaSeguridad> lista = new List<PreguntaSeguridad>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerPreguntasUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // El objeto no incluye IdUsuario: el filtro por usuario ya se aplicó
                        // en el SP, así que no hace falta cargarlo acá.
                        lista.Add(new PreguntaSeguridad
                        {
                            IdPregunta = Convert.ToInt32(reader["IdPregunta"]),
                            Pregunta = reader["Pregunta"].ToString(),
                            RespuestaHash = reader["RespuestaHash"].ToString()
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al listar preguntas de seguridad: " + ex.Message, ex);
            }
            return lista;
        }

        // Guarda la respuesta (ya hasheada por la capa de lógica) de un usuario a una pregunta puntual.
        public bool InsertarRespuestaUsuario(int idUsuario, int idPregunta, string respuestaHash)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_GuardarRespuestasSeguridad", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@IdPregunta", idPregunta);
                    cmd.Parameters.AddWithValue("@RespuestaHash", respuestaHash);

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al guardar respuesta de seguridad: " + ex.Message, ex);
            }
        }

        // Compara, directamente en la base, si el hash pasado por parámetro coincide con el
        // que ya está guardado para esa pregunta de ese usuario.
        public bool VerificarRespuestaUsuario(int idUsuario, int idPregunta, string respuestaHash)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_VerificarRespuestaSeguridad", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@IdPregunta", idPregunta);
                    cmd.Parameters.AddWithValue("@RespuestaHash", respuestaHash);

                    object resultado = cmd.ExecuteScalar();

                    return resultado != null && Convert.ToBoolean(resultado);
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_preguntasDatos.VerificarRespuestaUsuario", ex);
                return false;
            }
        }

        // Indica si el usuario ya respondió al menos una pregunta de seguridad
        // (para saber si puede usar la recuperación de contraseña por preguntas).
        public bool UsuarioTienePreguntas(int idUsuario)
        {
            try
            {
                List<PreguntaSeguridad> preguntas = obtenerPreguntasUsuario(idUsuario);
                return preguntas.Count > 0;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_preguntasDatos.UsuarioTienePreguntas", ex);
                return false;
            }
        }
    }
}
