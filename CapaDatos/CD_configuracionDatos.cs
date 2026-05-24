using Microsoft.Data.SqlClient;
using Entidades;

namespace CapaDatos
{
    public class CD_configuracionDatos : CD_conexion
    {
        public configuracionSistema obtenerConfiguracion()
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT TOP 1
                            IdConfiguracion,
                            MinCaracteres,
                            CantPreguntas,
                            RequiereMayusculas,
                            RequiereMinusculas,
                            RequiereNumeros,
                            RequiereEspeciales,
                            Requiere2FA,
                            NoRepetirPasswords,
                            ValidarDatosPersonales
                        FROM dbo.ConfiguracionSistema
                        ORDER BY IdConfiguracion;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        configuracionSistema config = new configuracionSistema
                        {
                            idConfiguracion = Convert.ToInt32(reader["IdConfiguracion"]),
                            minCaracteres = Convert.ToInt32(reader["MinCaracteres"]),
                            cantPreguntas = Convert.ToInt32(reader["CantPreguntas"]),
                            requiereMayusculas = Convert.ToBoolean(reader["RequiereMayusculas"]),
                            requiereMinusculas = Convert.ToBoolean(reader["RequiereMinusculas"]),
                            requiereNumeros = Convert.ToBoolean(reader["RequiereNumeros"]),
                            requiereEspeciales = Convert.ToBoolean(reader["RequiereEspeciales"]),
                            requiere2FA = Convert.ToBoolean(reader["Requiere2FA"]),
                            noRepetirPasswords = Convert.ToBoolean(reader["NoRepetirPasswords"]),
                            validarDatosPersonales = Convert.ToBoolean(reader["ValidarDatosPersonales"])
                        };

                        reader.Close();
                        return config;
                    }

                    reader.Close();
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool actualizarConfiguracion(configuracionSistema config)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        UPDATE dbo.ConfiguracionSistema
                        SET
                            MinCaracteres = @MinCaracteres,
                            CantPreguntas = @CantPreguntas,
                            RequiereMayusculas = @RequiereMayusculas,
                            RequiereMinusculas = @RequiereMinusculas,
                            RequiereNumeros = @RequiereNumeros,
                            RequiereEspeciales = @RequiereEspeciales,
                            Requiere2FA = @Requiere2FA,
                            NoRepetirPasswords = @NoRepetirPasswords,
                            ValidarDatosPersonales = @ValidarDatosPersonales
                        WHERE IdConfiguracion = @IdConfiguracion;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);

                    cmd.Parameters.AddWithValue("@IdConfiguracion", config.idConfiguracion);
                    cmd.Parameters.AddWithValue("@MinCaracteres", config.minCaracteres);
                    cmd.Parameters.AddWithValue("@CantPreguntas", config.cantPreguntas);
                    cmd.Parameters.AddWithValue("@RequiereMayusculas", config.requiereMayusculas);
                    cmd.Parameters.AddWithValue("@RequiereMinusculas", config.requiereMinusculas);
                    cmd.Parameters.AddWithValue("@RequiereNumeros", config.requiereNumeros);
                    cmd.Parameters.AddWithValue("@RequiereEspeciales", config.requiereEspeciales);
                    cmd.Parameters.AddWithValue("@Requiere2FA", config.requiere2FA);
                    cmd.Parameters.AddWithValue("@NoRepetirPasswords", config.noRepetirPasswords);
                    cmd.Parameters.AddWithValue("@ValidarDatosPersonales", config.validarDatosPersonales);

                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public int obtenerMinimoCaracteresContra()
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT TOP 1 MinCaracteres
                        FROM dbo.ConfiguracionSistema
                        ORDER BY IdConfiguracion;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);

                    object result = cmd.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : 8;
                }
            }
            catch
            {
                return 8;
            }
        }

        public int obtenerCantidadPreguntasSeguridad()
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    string query = @"
                        SELECT TOP 1 CantPreguntas
                        FROM dbo.ConfiguracionSistema
                        ORDER BY IdConfiguracion;
                    ";

                    SqlCommand cmd = new SqlCommand(query, conexion);

                    object result = cmd.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : 3;
                }
            }
            catch
            {
                return 3;
            }
        }
    }
}