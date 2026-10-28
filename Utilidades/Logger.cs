using System.IO;

namespace Utilidades
{
    // Registro simple de errores en un archivo de texto junto al ejecutable.
    // Se usa en los catch de la capa de datos que antes tragaban la excepción
    // en silencio (devolvían false/null/lista vacía sin dejar rastro de qué pasó).
    public static class Logger
    {
        private static readonly string rutaLog = Path.Combine(AppContext.BaseDirectory, "errores.log");

        public static void RegistrarError(string origen, Exception ex)
        {
            try
            {
                string linea =
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {origen}: {ex.GetType().Name}: {ex.Message}" +
                    Environment.NewLine + ex.StackTrace + Environment.NewLine + Environment.NewLine;

                File.AppendAllText(rutaLog, linea);
            }
            catch
            {
                // Si ni siquiera se puede escribir el log, no hay nada más que hacer:
                // no queremos que registrar un error termine tirando uno nuevo.
            }
        }
    }
}
