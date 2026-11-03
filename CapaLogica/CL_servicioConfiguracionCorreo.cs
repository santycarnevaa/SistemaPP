using System.Text.Json;
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
            ConfiguracionCorreo config = configCorreoDatos.ObtenerConfiguracionCorreo();

            // si la base todavia no tiene credenciales, se cargan desde mailsettings.json
            if (config == null && CargarDesdeArchivo())
                config = configCorreoDatos.ObtenerConfiguracionCorreo();

            return config;
        }

        // Lee el correo remitente y la contraseña de aplicacion del archivo mailsettings.json
        // y los guarda en la base (la contraseña queda cifrada, igual que desde Configuracion)
        private bool CargarDesdeArchivo()
        {
            string ruta = BuscarArchivoMailSettings();

            if (ruta == null)
                return false;

            try
            {
                using JsonDocument documento = JsonDocument.Parse(File.ReadAllText(ruta));

                string from = documento.RootElement.GetProperty("From").GetString();
                string password = documento.RootElement.GetProperty("Password").GetString();

                if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(password))
                    return false;

                // Google muestra la contraseña de aplicacion con espacios, pero se usa sin ellos
                return RotarCredenciales(from.Trim(), password.Replace(" ", ""), 90);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CL_servicioConfiguracionCorreo.CargarDesdeArchivo", ex);
                return false;
            }
        }

        // Busca el archivo junto al ejecutable y, si no esta, en las carpetas de arriba
        // (cuando se ejecuta desde Visual Studio, queda en la carpeta del repositorio)
        private string BuscarArchivoMailSettings()
        {
            DirectoryInfo carpeta = new DirectoryInfo(AppContext.BaseDirectory);

            while (carpeta != null)
            {
                string ruta = Path.Combine(carpeta.FullName, "mailsettings.json");

                if (File.Exists(ruta))
                    return ruta;

                carpeta = carpeta.Parent;
            }

            return null;
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
