using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaDatos
{
    public class CD_georefDatos : CD_conexion
    {
        public bool guardarProvincia(string idProvincia, string nombre)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_GuardarProvincia", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdProvincia", idProvincia);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool guardarPartido(string idPartido, string nombre, string idProvincia)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_GuardarPartido", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdPartido", idPartido);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@IdProvincia", idProvincia);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool guardarLocalidad(string idLocalidad, string nombre, string idPartido)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_GuardarLocalidad", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdLocalidad", idLocalidad);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@IdPartido", idPartido);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<Provincia> obtenerProvincias()
        {
            List<Provincia> lista = new List<Provincia>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_ObtenerProvincias", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

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
            }
            catch { }

            return lista;
        }

        public List<Partido> obtenerPartidosPorProvincia(string idProvincia)
        {
            List<Partido> lista = new List<Partido>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_ObtenerPartidosPorProvincia", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
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
            }
            catch { }

            return lista;
        }

        public List<Localidad> obtenerLocalidadesPorPartido(string idPartido)
        {
            List<Localidad> lista = new List<Localidad>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_ObtenerLocalidadesPorPartido", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
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
            }
            catch { }

            return lista;
        }

        public bool hayDatosCargados()
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_HayDatosGeograficosCargados", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    object resultado = cmd.ExecuteScalar();

                    return resultado != null && Convert.ToBoolean(resultado);
                }
            }
            catch
            {
                return false;
            }
        }

        public int guardarDireccion(
            string calle,
            string numero,
            string codigoPostal,
            string depto,
            string piso,
            string provincia,
            string partido,
            string localidad,
            decimal? latitud,
            decimal? longitud,
            string direccionOriginal,
            string direccionNormalizada)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_GuardarDireccion", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Calle", calle);
                    cmd.Parameters.AddWithValue("@Numero", string.IsNullOrWhiteSpace(numero) ? DBNull.Value : numero);
                    cmd.Parameters.AddWithValue("@CodigoPostal", string.IsNullOrWhiteSpace(codigoPostal) ? DBNull.Value : codigoPostal);
                    cmd.Parameters.AddWithValue("@Depto", string.IsNullOrWhiteSpace(depto) ? DBNull.Value : depto);
                    cmd.Parameters.AddWithValue("@Piso", string.IsNullOrWhiteSpace(piso) ? DBNull.Value : piso);
                    cmd.Parameters.AddWithValue("@Provincia", provincia);
                    cmd.Parameters.AddWithValue("@Partido", string.IsNullOrWhiteSpace(partido) ? DBNull.Value : partido);
                    cmd.Parameters.AddWithValue("@Localidad", string.IsNullOrWhiteSpace(localidad) ? DBNull.Value : localidad);
                    cmd.Parameters.AddWithValue("@Latitud", latitud.HasValue ? latitud.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Longitud", longitud.HasValue ? longitud.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DireccionOriginal", string.IsNullOrWhiteSpace(direccionOriginal) ? DBNull.Value : direccionOriginal);
                    cmd.Parameters.AddWithValue("@DireccionNormalizada", string.IsNullOrWhiteSpace(direccionNormalizada) ? DBNull.Value : direccionNormalizada);

                    object resultado = cmd.ExecuteScalar();

                    return resultado != null ? Convert.ToInt32(resultado) : -1;
                }
            }
            catch
            {
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
            string provincia,
            string partido,
            string localidad,
            decimal? latitud,
            decimal? longitud,
            string direccionOriginal,
            string direccionNormalizada)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SP_ActualizarDireccion", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdDireccion", idDireccion);
                    cmd.Parameters.AddWithValue("@Calle", calle);
                    cmd.Parameters.AddWithValue("@Numero", string.IsNullOrWhiteSpace(numero) ? DBNull.Value : numero);
                    cmd.Parameters.AddWithValue("@CodigoPostal", string.IsNullOrWhiteSpace(codigoPostal) ? DBNull.Value : codigoPostal);
                    cmd.Parameters.AddWithValue("@Depto", string.IsNullOrWhiteSpace(depto) ? DBNull.Value : depto);
                    cmd.Parameters.AddWithValue("@Piso", string.IsNullOrWhiteSpace(piso) ? DBNull.Value : piso);
                    cmd.Parameters.AddWithValue("@Provincia", provincia);
                    cmd.Parameters.AddWithValue("@Partido", string.IsNullOrWhiteSpace(partido) ? DBNull.Value : partido);
                    cmd.Parameters.AddWithValue("@Localidad", string.IsNullOrWhiteSpace(localidad) ? DBNull.Value : localidad);
                    cmd.Parameters.AddWithValue("@Latitud", latitud.HasValue ? latitud.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Longitud", longitud.HasValue ? longitud.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DireccionOriginal", string.IsNullOrWhiteSpace(direccionOriginal) ? DBNull.Value : direccionOriginal);
                    cmd.Parameters.AddWithValue("@DireccionNormalizada", string.IsNullOrWhiteSpace(direccionNormalizada) ? DBNull.Value : direccionNormalizada);

                    object resultado = cmd.ExecuteScalar();

                    if (resultado != null)
                        return Convert.ToInt32(resultado) > 0;

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}