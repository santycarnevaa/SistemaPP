using CapaDatos;
using Entidades;
using Utilidades;

namespace CapaLogica
{
    // logica de las preguntas de seguridad: arma y valida las respuestas
    // que se usan en el primer login y al recuperar la contraseña
    public class CL_ServicioPreguntasSeguridad
    {
        private CD_preguntasDatos preguntasDatos = new CD_preguntasDatos();
        private CD_usuariosDatos usuarioDatos = new CD_usuariosDatos();
        private encriptar encriptar = new encriptar();

        // igual que el de abajo pero recibe el usuario en vez del id:
        // primero busca el id en la base y despues usa cd_preguntasdatos
        public List<PreguntaSeguridad> obtenerPreguntasUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return new List<PreguntaSeguridad>();

            int idUsuario = usuarioDatos.BuscarUsuarioPorNombreUser(usuario);

            if (idUsuario == -1)
                return new List<PreguntaSeguridad>();

            return preguntasDatos.obtenerPreguntasUsuario(idUsuario);
        }

        // verifica que las respuestas que escribio en recuperar coincidan
        // (por hash y en el mismo orden en que se mostraron las preguntas) con las
        // que guardo el usuario. si una no coincide se rechaza todo
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

        // guarda todas las respuestas del primer login: hashea cada una (nunca se guarda
        // en texto plano) y las inserta de a una. si alguna falla corta y devuelve false
        // (las que ya se guardaron antes de que falle quedan guardadas igual)
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

        // trae las 15 preguntas del sistema para el form del primer login
        // (frmpregusuario), ahi el usuario todavia no respondio ninguna
        public List<PreguntaSeguridad> ObtenerTodasLasPreguntasDisponibles()
        {
            return preguntasDatos.ObtenerPreguntasDisponibles();
        }
    }
}
