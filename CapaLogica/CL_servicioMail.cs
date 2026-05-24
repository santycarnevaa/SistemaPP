using Utilidades;

namespace CapaLogica
{
    public class CL_servicioMail
    {
        public bool enviarContraTemporal(string correo, string passwordTemporal)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            if (string.IsNullOrWhiteSpace(passwordTemporal))
                return false;

            string asunto = "Contraseña temporal del sistema";

            string mensaje =
                "Se generó una contraseña temporal para tu usuario.\n\n" +
                "Contraseña temporal: " + passwordTemporal + "\n\n" +
                "Al iniciar sesión por primera vez, el sistema te pedirá cambiarla.";

            try
            {
                enviarMail.sendMail(correo, asunto, mensaje);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EnviarCodigoRecuperacion(string correo, string codigo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            if (string.IsNullOrWhiteSpace(codigo))
                return false;

            string asunto = "Código de recuperación";

            string mensaje =
                "Tu código de recuperación es: " + codigo + "\n\n" +
                "Si no solicitaste este código, ignorá este mensaje.";

            try
            {
                enviarMail.sendMail(correo, asunto, mensaje);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}