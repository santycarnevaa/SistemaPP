using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_configuracionDatos : CD_ejecutarConsultasSP
    {
        public configuracionSistema obtenerConfiguracion()
        {
            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerConfiguracion", null);

                if (tabla.Rows.Count == 0)
                    return null;

                DataRow fila = tabla.Rows[0];
                return new configuracionSistema
                {
                    idConfiguracion = Convert.ToInt32(fila["IdPolitica"]),
                    minCaracteres = Convert.ToInt32(fila["MinCaracteres"]),
                    cantPreguntas = Convert.ToInt32(fila["CantPreguntas"]),
                    requiereMayusculas = Convert.ToBoolean(fila["RequiereMayusculas"]),
                    requiereMinusculas = Convert.ToBoolean(fila["RequiereMinusculas"]),
                    requiereNumeros = Convert.ToBoolean(fila["RequiereNumeros"]),
                    requiereEspeciales = Convert.ToBoolean(fila["RequiereEspeciales"]),
                    requiere2FA = Convert.ToBoolean(fila["Requiere2FA"]),
                    noRepetirPasswords = Convert.ToBoolean(fila["NoRepetirPasswords"]),
                    validarDatosPersonales = Convert.ToBoolean(fila["ValidarDatosPersonales"])
                };
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_configuracionDatos.obtenerConfiguracion", ex);
                return null;
            }
        }

        public bool actualizarConfiguracion(configuracionSistema config)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@MinCaracteres", SqlDbType.Int) { Value = config.minCaracteres },
                new SqlParameter("@CantPreguntas", SqlDbType.Int) { Value = config.cantPreguntas },
                new SqlParameter("@RequiereMayusculas", SqlDbType.Bit) { Value = config.requiereMayusculas },
                new SqlParameter("@RequiereMinusculas", SqlDbType.Bit) { Value = config.requiereMinusculas },
                new SqlParameter("@RequiereNumeros", SqlDbType.Bit) { Value = config.requiereNumeros },
                new SqlParameter("@RequiereEspeciales", SqlDbType.Bit) { Value = config.requiereEspeciales },
                new SqlParameter("@Requiere2FA", SqlDbType.Bit) { Value = config.requiere2FA },
                new SqlParameter("@NoRepetirPasswords", SqlDbType.Bit) { Value = config.noRepetirPasswords },
                new SqlParameter("@ValidarDatosPersonales", SqlDbType.Bit) { Value = config.validarDatosPersonales }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ActualizarPoliticasSeguridad", parametros);

                // si algo falla adentro del sp no tira excepcion, devuelve una fila con la columna error.
                // hay que revisarla para no dar por guardado algo que fallo
                if (tabla.Rows.Count > 0 && tabla.Columns[0].ColumnName == "Error")
                    throw new Exception(tabla.Rows[0][0].ToString());

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error SQL al actualizar configuración: " + ex.Message, ex);
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
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_configuracionDatos.obtenerMinimoCaracteresContra", ex);
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
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_configuracionDatos.obtenerCantidadPreguntasSeguridad", ex);
                return 3;
            }
        }
    }
}
