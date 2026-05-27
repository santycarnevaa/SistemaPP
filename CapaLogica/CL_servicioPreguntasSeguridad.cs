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
        private CL_ServicioConfiguracion servicioConfiguracion = new CL_ServicioConfiguracion();

        public List<PreguntaSeguridad> obtenerPreguntasUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return new List<PreguntaSeguridad>();

            int idUsuario = usuarioDatos.BuscarUsuarioPorNombreUser(usuario);

            if (idUsuario == -1)
                return new List<PreguntaSeguridad>();

            return preguntasDatos.obtenerPreguntasUsuario(idUsuario);
        }

        public bool validarRespuestas(string usuario, List<string> respuestasIngresadas)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return false;

            if (respuestasIngresadas == null || respuestasIngresadas.Count == 0)
                return false;

            List<PreguntaSeguridad> preguntasUsuario = obtenerPreguntasUsuario(usuario);

            if (preguntasUsuario.Count < respuestasIngresadas.Count)
                return false;

            for (int i = 0; i < respuestasIngresadas.Count; i++)
            {
                string respuestaIngresada = respuestasIngresadas[i];

                if (string.IsNullOrWhiteSpace(respuestaIngresada))
                    return false;

                string respuestaNormalizada = respuestaIngresada.Trim().ToLower();

                string hashIngresado = encriptar.HashMetodo(respuestaNormalizada);

                string hashGuardado = preguntasUsuario[i].RespuestaHash;

                if (hashIngresado != hashGuardado)
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

                bool guardado = preguntasDatos.InsertarRespuestaUsuario(
                    idUsuario,
                    idPregunta,
                    respuestaHash
                );

                if (!guardado)
                    return false;
            }

            return true;
        }

        public List<PreguntaSeguridad> ObtenerPreguntasAleatoriasSegunConfiguracion()
        {
            List<PreguntaSeguridad> preguntas = preguntasDatos.ObtenerPreguntasDisponibles();

            int cantidadPreguntas = servicioConfiguracion.obtenerCantidadPreguntasSeguridad();

            if (cantidadPreguntas <= 0)
                cantidadPreguntas = 3;

            if (preguntas.Count < cantidadPreguntas)
                return new List<PreguntaSeguridad>();

            Random random = new Random();

            return preguntas
                .OrderBy(x => random.Next())
                .Take(cantidadPreguntas)
                .ToList();
        }
    }
}