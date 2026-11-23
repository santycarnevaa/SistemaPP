using System;
using System.Collections.Generic;
using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_georefDatos : CD_ejecutarConsultasSP
    {
        public bool guardarProvincia(string idProvincia, string nombre)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdProvincia", SqlDbType.VarChar, 10) { Value = idProvincia },
                new SqlParameter("@Nombre", SqlDbType.VarChar, 100) { Value = nombre }
            };

            try
            {
                EjecutarConsulta("dbo.SP_GuardarProvincia", parametros, devuelveDatos: false);
                return true;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_georefDatos.guardarProvincia", ex);
                return false;
            }
        }

        public bool guardarPartido(string idPartido, string nombre, string idProvincia)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdPartido", SqlDbType.VarChar, 20) { Value = idPartido },
                new SqlParameter("@Nombre", SqlDbType.VarChar, 100) { Value = nombre },
                new SqlParameter("@IdProvincia", SqlDbType.VarChar, 10) { Value = idProvincia }
            };

            try
            {
                EjecutarConsulta("dbo.SP_GuardarPartido", parametros, devuelveDatos: false);
                return true;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_georefDatos.guardarPartido", ex);
                return false;
            }
        }

        public bool guardarLocalidad(string idLocalidad, string nombre, string idPartido)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdLocalidad", SqlDbType.VarChar, 30) { Value = idLocalidad },
                new SqlParameter("@Nombre", SqlDbType.VarChar, 150) { Value = nombre },
                new SqlParameter("@IdPartido", SqlDbType.VarChar, 20) { Value = idPartido }
            };

            try
            {
                EjecutarConsulta("dbo.SP_GuardarLocalidad", parametros, devuelveDatos: false);
                return true;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_georefDatos.guardarLocalidad", ex);
                return false;
            }
        }

        public List<Provincia> obtenerProvincias()
        {
            List<Provincia> lista = new List<Provincia>();

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerProvincias", null);

                foreach (DataRow fila in tabla.Rows)
                {
                    lista.Add(new Provincia
                    {
                        IdProvincia = fila["IdProvincia"].ToString(),
                        Nombre = fila["Nombre"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_georefDatos.obtenerProvincias", ex);
            }

            return lista;
        }

        public List<Partido> obtenerPartidosPorProvincia(string idProvincia)
        {
            List<Partido> lista = new List<Partido>();

            SqlParameter[] parametros =
            {
                new SqlParameter("@IdProvincia", SqlDbType.VarChar, 10) { Value = idProvincia }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerPartidosPorProvincia", parametros);

                foreach (DataRow fila in tabla.Rows)
                {
                    lista.Add(new Partido
                    {
                        IdPartido = fila["IdPartido"].ToString(),
                        Nombre = fila["Nombre"].ToString(),
                        IdProvincia = fila["IdProvincia"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_georefDatos.obtenerPartidosPorProvincia", ex);
            }

            return lista;
        }

        public List<Localidad> obtenerLocalidadesPorPartido(string idPartido)
        {
            List<Localidad> lista = new List<Localidad>();

            SqlParameter[] parametros =
            {
                new SqlParameter("@IdPartido", SqlDbType.VarChar, 20) { Value = idPartido }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerLocalidadesPorPartido", parametros);

                foreach (DataRow fila in tabla.Rows)
                {
                    lista.Add(new Localidad
                    {
                        IdLocalidad = fila["IdLocalidad"].ToString(),
                        Nombre = fila["Nombre"].ToString(),
                        IdPartido = fila["IdPartido"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_georefDatos.obtenerLocalidadesPorPartido", ex);
            }

            return lista;
        }

        public bool hayDatosCargados()
        {
            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_HayDatosGeograficosCargados", null));

                return resultado != null && Convert.ToBoolean(resultado);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_georefDatos.hayDatosCargados", ex);
                return false;
            }
        }

        public int guardarDireccion(
            string calle,
            string numero,
            string codigoPostal,
            string depto,
            string piso,
            string idLocalidad,
            decimal? latitud,
            decimal? longitud,
            string direccionOriginal,
            string direccionNormalizada)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@Calle", SqlDbType.VarChar, 150) { Value = calle },
                new SqlParameter("@Numero", SqlDbType.VarChar, 20) { Value = ValorONulo(numero) },
                new SqlParameter("@CodigoPostal", SqlDbType.VarChar, 20) { Value = ValorONulo(codigoPostal) },
                new SqlParameter("@Depto", SqlDbType.VarChar, 20) { Value = ValorONulo(depto) },
                new SqlParameter("@Piso", SqlDbType.VarChar, 20) { Value = ValorONulo(piso) },
                new SqlParameter("@IdLocalidad", SqlDbType.VarChar, 30) { Value = idLocalidad },
                new SqlParameter("@Latitud", SqlDbType.Decimal) { Precision = 10, Scale = 7, Value = (object?)latitud ?? DBNull.Value },
                new SqlParameter("@Longitud", SqlDbType.Decimal) { Precision = 10, Scale = 7, Value = (object?)longitud ?? DBNull.Value },
                new SqlParameter("@DireccionOriginal", SqlDbType.VarChar, 250) { Value = ValorONulo(direccionOriginal) },
                new SqlParameter("@DireccionNormalizada", SqlDbType.VarChar, 250) { Value = ValorONulo(direccionNormalizada) }
            };

            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_GuardarDireccion", parametros));

                return resultado != null ? Convert.ToInt32(resultado) : -1;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_georefDatos.guardarDireccion", ex);
                return -1;
            }
        }

        public bool actualizarDireccion(
            int idDireccion,
            string calle,
            string numero,
            string codigoPostal,
            string depto,
            string piso,
            string idLocalidad,
            decimal? latitud,
            decimal? longitud,
            string direccionOriginal,
            string direccionNormalizada)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdDireccion", SqlDbType.Int) { Value = idDireccion },
                new SqlParameter("@Calle", SqlDbType.VarChar, 150) { Value = calle },
                new SqlParameter("@Numero", SqlDbType.VarChar, 20) { Value = ValorONulo(numero) },
                new SqlParameter("@CodigoPostal", SqlDbType.VarChar, 20) { Value = ValorONulo(codigoPostal) },
                new SqlParameter("@Depto", SqlDbType.VarChar, 20) { Value = ValorONulo(depto) },
                new SqlParameter("@Piso", SqlDbType.VarChar, 20) { Value = ValorONulo(piso) },
                new SqlParameter("@IdLocalidad", SqlDbType.VarChar, 30) { Value = idLocalidad },
                new SqlParameter("@Latitud", SqlDbType.Decimal) { Precision = 10, Scale = 7, Value = (object?)latitud ?? DBNull.Value },
                new SqlParameter("@Longitud", SqlDbType.Decimal) { Precision = 10, Scale = 7, Value = (object?)longitud ?? DBNull.Value },
                new SqlParameter("@DireccionOriginal", SqlDbType.VarChar, 250) { Value = ValorONulo(direccionOriginal) },
                new SqlParameter("@DireccionNormalizada", SqlDbType.VarChar, 250) { Value = ValorONulo(direccionNormalizada) }
            };

            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_ActualizarDireccion", parametros));

                if (resultado != null)
                    return Convert.ToInt32(resultado) > 0;

                return true;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_georefDatos.actualizarDireccion", ex);
                return false;
            }
        }
    }
}
