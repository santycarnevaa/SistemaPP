using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_scrapDatos : CD_ejecutarConsultasSP
    {
        public List<RegistroScrap> ObtenerScrap(DateTime? desde, DateTime? hasta)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@Desde", SqlDbType.DateTime) { Value = (object?)desde ?? DBNull.Value },
                new SqlParameter("@Hasta", SqlDbType.DateTime) { Value = (object?)hasta ?? DBNull.Value }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerScrap", parametros);

                List<RegistroScrap> registros = new List<RegistroScrap>();
                foreach (DataRow fila in tabla.Rows)
                {
                    registros.Add(new RegistroScrap
                    {
                        ID = Convert.ToInt32(fila["ID"]),
                        FK_Producto = Convert.ToInt32(fila["FK_Producto"]),
                        FK_Usuario = Convert.ToInt32(fila["FK_Usuario"]),
                        Cantidad = Convert.ToInt32(fila["Cantidad"]),
                        Motivo = fila["Motivo"].ToString(),
                        Costo_Valorizado = Convert.ToDecimal(fila["Costo_Valorizado"]),
                        Fecha = Convert.ToDateTime(fila["Fecha"]),
                        Producto = fila["Producto"].ToString(),
                        Usuario = fila["Usuario"].ToString()
                    });
                }

                return registros;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_scrapDatos.ObtenerScrap", ex);
                return new List<RegistroScrap>();
            }
        }

        // igual que insertarmovimiento: devuelve null si salio bien o el error
        // que mando el sp (por ej si no hay stock suficiente)
        public string InsertarScrap(RegistroScrap s, out int stockResultante, out decimal costoValorizado)
        {
            stockResultante = -1;
            costoValorizado = 0;

            SqlParameter[] parametros =
            {
                new SqlParameter("@FK_Producto", SqlDbType.Int) { Value = s.FK_Producto },
                new SqlParameter("@FK_Usuario", SqlDbType.Int) { Value = s.FK_Usuario },
                new SqlParameter("@Cantidad", SqlDbType.Int) { Value = s.Cantidad },
                new SqlParameter("@Motivo", SqlDbType.VarChar, 200) { Value = s.Motivo }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_InsertarScrap", parametros);

                if (tabla.Rows.Count > 0)
                {
                    if (tabla.Columns.Contains("Error"))
                        return tabla.Rows[0]["Error"].ToString();

                    stockResultante = Convert.ToInt32(tabla.Rows[0]["Stock_Resultante"]);
                    costoValorizado = Convert.ToDecimal(tabla.Rows[0]["Costo_Valorizado"]);
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_scrapDatos.InsertarScrap", ex);
                return "Error de base de datos al registrar el scrap.";
            }
        }
    }
}
