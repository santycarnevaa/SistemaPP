using Microsoft.Data.SqlClient;
using Entidades;

namespace CapaDatos
{
    public class CD_preguntasDatos : CD_conexion
    {
        public List<PreguntaSeguridad> obtenerPreguntasDisponibles()
        {
            List<PreguntaSeguridad> preguntas = new List<PreguntaSeguridad>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT IdPregunta, Pregunta
                        FROM dbo.PreguntasSeguridad
                        ORDER BY IdPregunta;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);

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
            catch
            {
                return new List<PreguntaSeguridad>();
            }

            return preguntas;
        }

        public List<PreguntaSeguridad> obtenerPreguntasUsuario(int idUsuario)
        {
            List<PreguntaSeguridad> preguntas = new List<PreguntaSeguridad>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT 
                            P.IdPregunta,
                            P.Pregunta,
                            R.RespuestaHash
                        FROM dbo.RespuestasSeguridad R
                        INNER JOIN dbo.PreguntasSeguridad P
                            ON R.IdPregunta = P.IdPregunta
                        WHERE R.IdUsuario = @IdUsuario
                        ORDER BY P.IdPregunta;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        preguntas.Add(new PreguntaSeguridad
                        {
                            IdPregunta = Convert.ToInt32(reader["IdPregunta"]),
                            Pregunta = reader["Pregunta"].ToString(),
                            RespuestaHash = reader["RespuestaHash"].ToString()
                        });
                    }

                    reader.Close();
                }
            }
            catch
            {
                return new List<PreguntaSeguridad>();
            }

            return preguntas;
        }

        public bool insertarRespuestaUsuario(int idUsuario, int idPregunta, string respuestaHash)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        IF EXISTS (
                            SELECT 1
                            FROM dbo.RespuestasSeguridad
                            WHERE IdUsuario = @IdUsuario
                              AND IdPregunta = @IdPregunta
                        )
                        BEGIN
                            UPDATE dbo.RespuestasSeguridad
                            SET RespuestaHash = @RespuestaHash
                            WHERE IdUsuario = @IdUsuario
                              AND IdPregunta = @IdPregunta;
                        END
                        ELSE
                        BEGIN
                            INSERT INTO dbo.RespuestasSeguridad
                            (
                                IdUsuario,
                                IdPregunta,
                                RespuestaHash
                            )
                            VALUES
                            (
                                @IdUsuario,
                                @IdPregunta,
                                @RespuestaHash
                            );
                        END;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@IdPregunta", idPregunta);
                    cmd.Parameters.AddWithValue("@RespuestaHash", respuestaHash);

                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool usuarioTienePreguntas(int idUsuario)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT COUNT(*)
                        FROM dbo.RespuestasSeguridad
                        WHERE IdUsuario = @IdUsuario;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    int cantidad = Convert.ToInt32(cmd.ExecuteScalar());

                    return cantidad > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}