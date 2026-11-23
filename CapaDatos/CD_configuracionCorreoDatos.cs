using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class CD_configuracionCorreoDatos : CD_ejecutarConsultasSP
    {
        public ConfiguracionCorreo ObtenerConfiguracionCorreo()
        {
            DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerConfiguracionCorreo", null);

            if (tabla.Rows.Count == 0)
                return null;

            DataRow fila = tabla.Rows[0];
            return new ConfiguracionCorreo
            {
                IdConfiguracionCorreo = Convert.ToInt32(fila["IdConfiguracionCorreo"]),
                CorreoRemitente = fila["CorreoRemitente"].ToString(),
                PasswordCifrada = (byte[])fila["PasswordCifrada"],
                IV = (byte[])fila["IV"],
                FechaUltimaRotacion = Convert.ToDateTime(fila["FechaUltimaRotacion"]),
                DiasRotacion = Convert.ToInt32(fila["DiasRotacion"])
            };
        }

        public bool GuardarConfiguracionCorreo(string correoRemitente, int diasRotacion, byte[] passwordCifrada = null, byte[] iv = null)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@CorreoRemitente", SqlDbType.VarChar, 100) { Value = correoRemitente },
                new SqlParameter("@DiasRotacion", SqlDbType.Int) { Value = diasRotacion },
                new SqlParameter("@PasswordCifrada", SqlDbType.VarBinary, -1) { Value = (object?)passwordCifrada ?? DBNull.Value },
                new SqlParameter("@IV", SqlDbType.VarBinary, 16) { Value = (object?)iv ?? DBNull.Value }
            };

            EjecutarConsulta("dbo.SP_GuardarConfiguracionCorreo", parametros, devuelveDatos: false);
            return true;
        }
    }
}
