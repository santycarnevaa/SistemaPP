using Microsoft.Data.SqlClient;
using Entidades;

namespace CapaDatos
{
    public class CD_georefDatos : CD_conexion
    {
        public bool guardarProvincia(string idProvincia, string nombre)
        {
            using (SqlConnection conexion = conectar())
            {
                string query = @"
                    IF NOT EXISTS (
                        SELECT 1 FROM dbo.Provincias WHERE IdProvincia = @IdProvincia
                    )
                    BEGIN
                        INSERT INTO dbo.Provincias (IdProvincia, Nombre)
                        VALUES (@IdProvincia, @Nombre);
                    END";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdProvincia", idProvincia);
                cmd.Parameters.AddWithValue("@Nombre", nombre);

                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public bool guardarPartido(string idPartido, string nombre, string idProvincia)
        {
            using (SqlConnection conexion = conectar())
            {
                string query = @"
                    IF NOT EXISTS (
                        SELECT 1 FROM dbo.Partidos WHERE IdPartido = @IdPartido
                    )
                    BEGIN
                        INSERT INTO dbo.Partidos (IdPartido, Nombre, IdProvincia)
                        VALUES (@IdPartido, @Nombre, @IdProvincia);
                    END";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdPartido", idPartido);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@IdProvincia", idProvincia);

                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public bool guardarLocalidad(string idLocalidad, string nombre, string idPartido)
        {
            using (SqlConnection conexion = conectar())
            {
                string query = @"
                    IF NOT EXISTS (
                        SELECT 1 FROM dbo.Localidades WHERE IdLocalidad = @IdLocalidad
                    )
                    BEGIN
                        INSERT INTO dbo.Localidades (IdLocalidad, Nombre, IdPartido)
                        VALUES (@IdLocalidad, @Nombre, @IdPartido);
                    END";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdLocalidad", idLocalidad);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@IdPartido", idPartido);

                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public List<Provincia> obtenerProvincias()
        {
            List<Provincia> lista = new List<Provincia>();

            using (SqlConnection conexion = conectar())
            {
                string query = @"
                    SELECT IdProvincia, Nombre
                    FROM dbo.Provincias
                    ORDER BY Nombre";

                SqlCommand cmd = new SqlCommand(query, conexion);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Provincia
                    {
                        IdProvincia = reader["IdProvincia"].ToString(),
                        Nombre = reader["Nombre"].ToString()
                    });
                }

                reader.Close();
            }

            return lista;
        }

        public List<Partido> obtenerPartidosPorProvincia(string idProvincia)
        {
            List<Partido> lista = new List<Partido>();

            using (SqlConnection conexion = conectar())
            {
                string query = @"
                    SELECT IdPartido, Nombre, IdProvincia
                    FROM dbo.Partidos
                    WHERE IdProvincia = @IdProvincia
                    ORDER BY Nombre";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdProvincia", idProvincia);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Partido
                    {
                        IdPartido = reader["IdPartido"].ToString(),
                        Nombre = reader["Nombre"].ToString(),
                        IdProvincia = reader["IdProvincia"].ToString()
                    });
                }

                reader.Close();
            }

            return lista;
        }

        public List<Localidad> obtenerLocalidadesPorPartido(string idPartido)
        {
            List<Localidad> lista = new List<Localidad>();

            using (SqlConnection conexion = conectar())
            {
                string query = @"
                    SELECT IdLocalidad, Nombre, IdPartido
                    FROM dbo.Localidades
                    WHERE IdPartido = @IdPartido
                    ORDER BY Nombre";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdPartido", idPartido);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Localidad
                    {
                        IdLocalidad = reader["IdLocalidad"].ToString(),
                        Nombre = reader["Nombre"].ToString(),
                        IdPartido = reader["IdPartido"].ToString()
                    });
                }

                reader.Close();
            }

            return lista;
        }

        public bool hayDatosCargados()
        {
            using (SqlConnection conexion = conectar())
            {
                string query = "SELECT COUNT(*) FROM dbo.Provincias";

                SqlCommand cmd = new SqlCommand(query, conexion);

                int cantidad = Convert.ToInt32(cmd.ExecuteScalar());

                return cantidad > 0;
            }
        }
    }
}