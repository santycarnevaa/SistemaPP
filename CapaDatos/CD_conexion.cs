using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class CD_conexion
    {
        private readonly string cadenaConexion =
            "Server=localhost;Database=SistemaUsuariosDB;Trusted_Connection=True;TrustServerCertificate=True;";

        protected SqlConnection conectar()
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            conexion.Open();
            return conexion;
        }
    }
}