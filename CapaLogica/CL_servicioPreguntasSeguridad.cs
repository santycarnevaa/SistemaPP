using CapaDatos;
using Entidades;
using Utilidades;

namespace CapaLogica
{
    public class CL_ServicioPreguntasSeguridad
    {
        private CD_preguntasDatos preguntasDatos = new CD_preguntasDatos();
        private CD_usuariosDatos usuarioDatos = new CD_usuariosDatos();
        private encriptar encriptar = new encriptar();

        public List<PreguntaSeguridad> obtenerPreguntasUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return new List<PreguntaSeguridad>();

            int idUsuario = usuarioDatos.obtenerIdUsuario(usuario);

            if (idUsuario == -1)
                return new List<PreguntaSeguridad>();

            return preguntasDatos.obtenerPreguntasUsuario(idUsuario);
        }

        public bool validarRespuestas(string usuario, List<string> respuestas)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return false;

            if (respuestas == null || respuestas.Count == 0)
                return false;

            int idUsuario = usuarioDatos.obtenerIdUsuario(usuario);

            if (idUsuario == -1)
                return false;

            List<PreguntaSeguridad> preguntasUsuario =
                preguntasDatos.obtenerPreguntasUsuario(idUsuario);

            if (preguntasUsuario.Count != respuestas.Count)
                return false;

            for (int i = 0; i < preguntasUsuario.Count; i++)
            {
                string respuestaIngresada = respuestas[i];

                if (string.IsNullOrWhiteSpace(respuestaIngresada))
                    return false;

                string hashIngresado = encriptar.HashMetodo(respuestaIngresada);

                if (hashIngresado != preguntasUsuario[i].RespuestaHash)
                    return false;
            }

            return true;
        }

        public bool registrarRespuestasUsuario(int idUsuario, Dictionary<int, string> respuestas)
        {
            if (idUsuario <= 0)
                return false;

            if (respuestas == null || respuestas.Count == 0)
                return false;

            foreach (var item in respuestas)
            {
                int idPregunta = item.Key;
                string respuesta = item.Value;

                if (string.IsNullOrWhiteSpace(respuesta))
                    return false;

                string respuestaHash = encriptar.HashMetodo(respuesta);

                bool guardado = preguntasDatos.insertarRespuestaUsuario(
                    idUsuario,
                    idPregunta,
                    respuestaHash
                );

                if (!guardado)
                    return false;
            }

            return true;
        }

        public List<PreguntaSeguridad> ObtenerPreguntasDisponibles()
        {
            return preguntasDatos.obtenerPreguntasDisponibles();
        }
    }
}