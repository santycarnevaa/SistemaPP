using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    // datos de las preguntas y respuestas de seguridad
    public class CD_preguntasDatos : CD_ejecutarConsultasSP
    {
        // trae las 15 preguntas del sistema, sin filtrar por usuario
        // se usa para mostrarle todas al usuario la primera vez que las tiene que responder
        public List<PreguntaSeguridad> ObtenerPreguntasDisponibles()
        {
            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ListarPreguntasSeguridad", null);

                List<PreguntaSeguridad> preguntas = new List<PreguntaSeguridad>();
                foreach (DataRow fila in tabla.Rows)
                {
                    preguntas.Add(new PreguntaSeguridad
                    {
                        IdPregunta = Convert.ToInt32(fila["IdPregunta"]),
                        Pregunta = fila["Pregunta"].ToString(),
                        RespuestaHash = string.Empty
                    });
                }

                return preguntas;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_preguntasDatos.ObtenerPreguntasDisponibles", ex);
                return new List<PreguntaSeguridad>();
            }
        }

        // trae solo las preguntas que ya respondio ese usuario con el hash de cada respuesta.
        // el filtro por usuario lo hace el sp. se usa para recuperar la contraseña
        public List<PreguntaSeguridad> obtenerPreguntasUsuario(int idUsuario)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerPreguntasUsuario", parametros);

                List<PreguntaSeguridad> lista = new List<PreguntaSeguridad>();
                foreach (DataRow fila in tabla.Rows)
                {
                    // no se carga el idusuario porque el sp ya filtro por usuario
                    lista.Add(new PreguntaSeguridad
                    {
                        IdPregunta = Convert.ToInt32(fila["IdPregunta"]),
                        Pregunta = fila["Pregunta"].ToString(),
                        RespuestaHash = fila["RespuestaHash"].ToString()
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al listar preguntas de seguridad: " + ex.Message, ex);
            }
        }

        // guarda la respuesta (ya hasheada en la capa logica) de un usuario a una pregunta
        public bool InsertarRespuestaUsuario(int idUsuario, int idPregunta, string respuestaHash)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@IdPregunta", SqlDbType.Int) { Value = idPregunta },
                new SqlParameter("@RespuestaHash", SqlDbType.VarChar, 256) { Value = respuestaHash }
            };

            try
            {
                EjecutarConsulta("dbo.SP_GuardarRespuestasSeguridad", parametros, devuelveDatos: false);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al guardar respuesta de seguridad: " + ex.Message, ex);
            }
        }

        // compara en la base si el hash coincide con el que esta guardado
        // para esa pregunta y ese usuario
        public bool VerificarRespuestaUsuario(int idUsuario, int idPregunta, string respuestaHash)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@IdPregunta", SqlDbType.Int) { Value = idPregunta },
                new SqlParameter("@RespuestaHash", SqlDbType.VarChar, 256) { Value = respuestaHash }
            };

            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_VerificarRespuestaSeguridad", parametros));

                return resultado != null && Convert.ToBoolean(resultado);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_preguntasDatos.VerificarRespuestaUsuario", ex);
                return false;
            }
        }

        // dice si el usuario ya respondio alguna pregunta
        // (para saber si puede recuperar la contraseña con preguntas)
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
