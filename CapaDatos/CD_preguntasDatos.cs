using Microsoft.Data.SqlClient;
using System.Data;
using Entidades;

namespace CapaDatos
{
    public class CD_preguntasDatos : CD_conexion
    {
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
            catch
            {
                return new List<PreguntaSeguridad>();
            }

            return preguntas;
        }

        public List<PreguntaSeguridad> ObtenerPreguntasUsuario(int idUsuario)
        {
            List<PreguntaSeguridad> preguntas = new List<PreguntaSeguridad>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_ObtenerPreguntasUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

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
            catch
            {
                return false;
            }
        }

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
            catch
            {
                return false;
            }
        }

        public bool UsuarioTienePreguntas(int idUsuario)
        {
            try
            {
                List<PreguntaSeguridad> preguntas = ObtenerPreguntasUsuario(idUsuario);
                return preguntas.Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}