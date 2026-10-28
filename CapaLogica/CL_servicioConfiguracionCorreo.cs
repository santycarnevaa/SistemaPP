using CapaDatos;
using Entidades;
using Utilidades;

namespace CapaLogica
{
    public class CL_servicioConfiguracionCorreo
    {
        private CD_configuracionCorreoDatos configCorreoDatos = new CD_configuracionCorreoDatos();

        public ConfiguracionCorreo ObtenerConfiguracion()
        {
            return configCorreoDatos.ObtenerConfiguracionCorreo();
        }

        public bool ActualizarSoloDias(string correoRemitente, int diasRotacion)
        {
            return configCorreoDatos.GuardarConfiguracionCorreo(correoRemitente, diasRotacion);
        }

        public bool RotarCredenciales(string correoRemitente, string passwordNueva, int diasRotacion)
        {
            var (passwordCifrada, iv) = CifradoAes.Cifrar(passwordNueva);
            return configCorreoDatos.GuardarConfiguracionCorreo(correoRemitente, diasRotacion, passwordCifrada, iv);
        }

        public bool CorrespondeRotar()
        {
            ConfiguracionCorreo config = ObtenerConfiguracion();

            if (config == null)
                return true; // todavía no se cargó ninguna credencial

            DateTime proximaRotacion = config.FechaUltimaRotacion.AddDays(config.DiasRotacion);
            return DateTime.Today >= proximaRotacion;
        }

        public (string from, string password) ObtenerCredencialesParaEnvio()
        {
            ConfiguracionCorreo config = ObtenerConfiguracion();

            if (config == null)
                throw new Exception("No hay credenciales de correo configuradas. Cargalas en Configuración.");

            string passwordPlano = CifradoAes.Descifrar(config.PasswordCifrada, config.IV);
            return (config.CorreoRemitente, passwordPlano);
        }
    }
}
