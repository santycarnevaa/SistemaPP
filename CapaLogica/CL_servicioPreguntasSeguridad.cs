using CapaDatos;
using Entidades;
using Utilidades;

namespace CapaLogica
{
    // Lógica de negocio de preguntas de seguridad: arma/valida las respuestas
    // que usan el primer login y la recuperación de contraseña.
    public class CL_ServicioPreguntasSeguridad
    {
        private CD_preguntasDatos preguntasDatos = new CD_preguntasDatos();
        private CD_usuariosDatos usuarioDatos = new CD_usuariosDatos();
        private encriptar encriptar = new encriptar();

        // Igual que el método de abajo, pero recibe el nombre de usuario en vez del ID:
        // primero lo resuelve contra la base y después reusa CD_preguntasDatos.
        public List<PreguntaSeguridad> obtenerPreguntasUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return new List<PreguntaSeguridad>();

            int idUsuario = usuarioDatos.BuscarUsuarioPorNombreUser(usuario);

            if (idUsuario == -1)
                return new List<PreguntaSeguridad>();

            return preguntasDatos.obtenerPreguntasUsuario(idUsuario);
        }

        // Verifica que las respuestas escritas en el formulario de recuperación coincidan
        // (por hash, comparando en el mismo orden en que se mostraron las preguntas) con las
        // que el usuario guardó en su momento. Alcanza con que UNA no coincida para rechazar todo.
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

        // Guarda todas las respuestas del primer login: hashea cada una (nunca se guarda en texto
        // plano) y las inserta de a una. Si alguna falla a mitad de camino, corta y devuelve false
        // (las que ya se insertaron antes del fallo quedan guardadas igual).
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

        // Trae el pool completo de 15 preguntas del sistema, para el formulario de primer login
        // (frmPregUsuario), donde el usuario todavía no tiene ninguna respondida.
        public List<PreguntaSeguridad> ObtenerTodasLasPreguntasDisponibles()
        {
            return preguntasDatos.ObtenerPreguntasDisponibles();
        }
    }
}
