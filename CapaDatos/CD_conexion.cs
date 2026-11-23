using System.IO;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    // la cadena de conexion no esta en el codigo, se lee una sola vez del appsettings.json
    // que esta al lado del .exe, la primera vez que hace falta
    // es abstract para que nadie haga new, solo se hereda
    public abstract class CD_conexion
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
