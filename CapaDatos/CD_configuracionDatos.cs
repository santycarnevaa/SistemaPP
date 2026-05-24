using Microsoft.Data.SqlClient;
using System.Data;
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
                    SqlCommand cmd = new SqlCommand("SP_ObtenerPoliticasSeguridad", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        configuracionSistema config = new configuracionSistema
                        {
                            idConfiguracion = Convert.ToInt32(reader["IdPolitica"]),
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
                    SqlCommand cmd = new SqlCommand("SP_ActualizarPoliticasSeguridad", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MinCaracteres", config.minCaracteres);
                    cmd.Parameters.AddWithValue("@CantPreguntas", config.cantPreguntas);
                    cmd.Parameters.AddWithValue("@RequiereMayusculas", config.requiereMayusculas);
                    cmd.Parameters.AddWithValue("@RequiereMinusculas", config.requiereMinusculas);
                    cmd.Parameters.AddWithValue("@RequiereNumeros", config.requiereNumeros);
                    cmd.Parameters.AddWithValue("@RequiereEspeciales", config.requiereEspeciales);
                    cmd.Parameters.AddWithValue("@Requiere2FA", config.requiere2FA);
                    cmd.Parameters.AddWithValue("@NoRepetirPasswords", config.noRepetirPasswords);
                    cmd.Parameters.AddWithValue("@ValidarDatosPersonales", config.validarDatosPersonales);

                    cmd.ExecuteNonQuery();
                    return true;
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
                configuracionSistema config = obtenerConfiguracion();

                if (config != null)
                    return config.minCaracteres;

                return 8;
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
                configuracionSistema config = obtenerConfiguracion();

                if (config != null)
                    return config.cantPreguntas;

                return 3;
            }
            catch
            {
                return 3;
            }
        }
    }
}