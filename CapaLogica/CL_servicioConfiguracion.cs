using CapaDatos;
using Entidades;

namespace CapaLogica
{
    public class CL_ServicioConfiguracion
    {
        private CD_configuracionDatos configDatos = new CD_configuracionDatos();

        public configuracionSistema ObtenerConfiguracion()
        {
            return configDatos.obtenerConfiguracion();
        }

        public bool actualizarConfiguracion(configuracionSistema config)
        {
            if (config == null)
                return false;

            if (config.minCaracteres < 4)
                return false;

            if (config.cantPreguntas < 1)
                return false;

            return configDatos.actualizarConfiguracion(config);
        }

        public int ObtenerMinimoCaracteresPassword()
        {
            configuracionSistema config = configDatos.obtenerConfiguracion();

            if (config == null)
                return 8;

            return config.minCaracteres;
        }

        public int obtenerCantidadPreguntasSeguridad()
        {
            configuracionSistema config = configDatos.obtenerConfiguracion();

            if (config == null)
                return 3;

            return config.cantPreguntas;
        }
    }
}
