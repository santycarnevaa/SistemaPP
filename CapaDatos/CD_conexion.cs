using System.IO;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    // La cadena de conexión ya no está hardcodeada acá: se lee una sola vez desde
    // appsettings.json (ubicado junto al .exe) la primera vez que se necesita.
    public class CD_conexion
    {
        private static readonly string cadenaConexion = CargarCadenaConexion();

        private static string CargarCadenaConexion()
        {
            string rutaConfig = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

            if (!File.Exists(rutaConfig))
            {
                throw new FileNotFoundException(
                    "No se encontró appsettings.json. Tiene que estar en la misma carpeta que el ejecutable, " +
                    "con la cadena de conexión bajo ConnectionStrings:SistemaUsuariosDB.",
                    rutaConfig
                );
            }

            string json = File.ReadAllText(rutaConfig);
            using JsonDocument documento = JsonDocument.Parse(json);

            string? cadena = documento.RootElement
                .GetProperty("ConnectionStrings")
                .GetProperty("SistemaUsuariosDB")
                .GetString();

            if (string.IsNullOrWhiteSpace(cadena))
                throw new InvalidOperationException("La cadena de conexión ConnectionStrings:SistemaUsuariosDB está vacía en appsettings.json.");

            return cadena;
        }

        protected SqlConnection conectar()
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            conexion.Open();
            return conexion;
        }
    }
}
