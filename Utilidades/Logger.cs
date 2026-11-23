using System.IO;

namespace Utilidades
{
    // guarda los errores en un txt al lado del exe
    // se usa en los catch de la capa datos que antes se comian la excepcion
    // sin avisar (devolvian false, null o lista vacia y no quedaba nada de lo que paso)
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
                // si ni siquiera se puede escribir el log no hay nada mas que hacer,
                // no queremos que guardar un error tire otro error
            }
        }
    }
}
