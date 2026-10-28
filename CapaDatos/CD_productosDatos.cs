using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_productosDatos : CD_conexion
    {
        public List<Producto> ObtenerProductos(bool soloHabilitados = true)
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerProductos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SoloHabilitados", soloHabilitados);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        productos.Add(MapearProducto(reader));
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ObtenerProductos", ex);
                return new List<Producto>();
            }

            return productos;
        }

        public Producto ObtenerProductoPorId(int idCodigo)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerProductoPorId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Codigo", idCodigo);

                    SqlDataReader reader = cmd.ExecuteReader();

                    Producto producto = null;

                    if (reader.Read())
                        producto = MapearProducto(reader);

                    reader.Close();
                    return producto;
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ObtenerProductoPorId", ex);
                return null;
            }
        }

        public List<Producto> BuscarProductos(string texto, int? idCategoria, int? idMarca)
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_BuscarProductos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Texto", string.IsNullOrWhiteSpace(texto) ? DBNull.Value : texto);
                    cmd.Parameters.AddWithValue("@FK_Categoria", idCategoria.HasValue ? idCategoria.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@FK_Marca", idMarca.HasValue ? idMarca.Value : DBNull.Value);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        productos.Add(MapearProducto(reader));
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.BuscarProductos", ex);
                return new List<Producto>();
            }

            return productos;
        }

        public int InsertarProducto(Producto p)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_InsertarProducto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FK_Categoria", p.FK_Categoria);
                    cmd.Parameters.AddWithValue("@FK_Marca", p.FK_Marca);
                    cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                    cmd.Parameters.AddWithValue("@Precio_Compra", p.Precio_Compra);
                    cmd.Parameters.AddWithValue("@Precio_Venta", p.Precio_Venta);
                    cmd.Parameters.AddWithValue("@Stock_Critico", p.Stock_Critico);
                    cmd.Parameters.AddWithValue("@Stock_Minimo", p.Stock_Minimo);
                    cmd.Parameters.AddWithValue("@Stock_Ideal", p.Stock_Ideal);
                    cmd.Parameters.AddWithValue("@Stock_Maximo", p.Stock_Maximo);
                    cmd.Parameters.AddWithValue("@Punto_Reposicion", p.Punto_Reposicion);
                    cmd.Parameters.AddWithValue("@Numero_Lote", string.IsNullOrWhiteSpace(p.Numero_Lote) ? DBNull.Value : p.Numero_Lote);
                    cmd.Parameters.AddWithValue("@Fecha_Vencimiento", p.Fecha_Vencimiento.HasValue ? p.Fecha_Vencimiento.Value : DBNull.Value);

                    object resultado = cmd.ExecuteScalar();

                    return resultado != null ? Convert.ToInt32(resultado) : -1;
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.InsertarProducto", ex);
                return -1;
            }
        }

        public bool ActualizarProducto(Producto p)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ActualizarProducto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_Codigo", p.ID_Codigo);
                    cmd.Parameters.AddWithValue("@FK_Categoria", p.FK_Categoria);
                    cmd.Parameters.AddWithValue("@FK_Marca", p.FK_Marca);
                    cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                    cmd.Parameters.AddWithValue("@Precio_Compra", p.Precio_Compra);
                    cmd.Parameters.AddWithValue("@Precio_Venta", p.Precio_Venta);
                    cmd.Parameters.AddWithValue("@Stock_Critico", p.Stock_Critico);
                    cmd.Parameters.AddWithValue("@Stock_Minimo", p.Stock_Minimo);
                    cmd.Parameters.AddWithValue("@Stock_Ideal", p.Stock_Ideal);
                    cmd.Parameters.AddWithValue("@Stock_Maximo", p.Stock_Maximo);
                    cmd.Parameters.AddWithValue("@Punto_Reposicion", p.Punto_Reposicion);
                    cmd.Parameters.AddWithValue("@Numero_Lote", string.IsNullOrWhiteSpace(p.Numero_Lote) ? DBNull.Value : p.Numero_Lote);
                    cmd.Parameters.AddWithValue("@Fecha_Vencimiento", p.Fecha_Vencimiento.HasValue ? p.Fecha_Vencimiento.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", p.Estado);

                    object resultado = cmd.ExecuteScalar();

                    return resultado != null && Convert.ToInt32(resultado) > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ActualizarProducto", ex);
                return false;
            }
        }

        public bool DarBajaProducto(int idCodigo)
        {
            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_DarBajaProducto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Codigo", idCodigo);

                    object resultado = cmd.ExecuteScalar();

                    return resultado != null && Convert.ToInt32(resultado) > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.DarBajaProducto", ex);
                return false;
            }
        }

        public List<Producto> ObtenerBajoPuntoReposicion()
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerProductosBajoPuntoReposicion", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        productos.Add(new Producto
                        {
                            ID_Codigo = Convert.ToInt32(reader["ID_Codigo"]),
                            Nombre = reader["Nombre"].ToString(),
                            Stock_Actual = Convert.ToInt32(reader["Stock_Actual"]),
                            Punto_Reposicion = Convert.ToInt32(reader["Punto_Reposicion"])
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ObtenerBajoPuntoReposicion", ex);
                return new List<Producto>();
            }

            return productos;
        }

        public List<Producto> ObtenerProximosAVencer(int diasAnticipacion)
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ObtenerProductosProximosAVencer", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DiasAnticipacion", diasAnticipacion);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        productos.Add(new Producto
                        {
                            ID_Codigo = Convert.ToInt32(reader["ID_Codigo"]),
                            Nombre = reader["Nombre"].ToString(),
                            Numero_Lote = reader["Numero_Lote"] == DBNull.Value ? null : reader["Numero_Lote"].ToString(),
                            Fecha_Vencimiento = reader["Fecha_Vencimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["Fecha_Vencimiento"])
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ObtenerProximosAVencer", ex);
                return new List<Producto>();
            }

            return productos;
        }

        public DataTable ReporteStockPorCategoria()
        {
            DataTable tabla = new DataTable();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ReporteStockPorCategoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(tabla);
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ReporteStockPorCategoria", ex);
            }

            return tabla;
        }

        public DataTable ReporteStockPorMarca()
        {
            DataTable tabla = new DataTable();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("dbo.SP_ReporteStockPorMarca", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(tabla);
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ReporteStockPorMarca", ex);
            }

            return tabla;
        }

        public List<Categoria> ObtenerCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SELECT IdCategoria, Nombre FROM dbo.Categorias ORDER BY Nombre", conexion);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        categorias.Add(new Categoria
                        {
                            IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                            Nombre = reader["Nombre"].ToString()
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ObtenerCategorias", ex);
                return new List<Categoria>();
            }

            return categorias;
        }

        public List<Marca> ObtenerMarcas()
        {
            List<Marca> marcas = new List<Marca>();

            try
            {
                using (SqlConnection conexion = conectar())
                {
                    SqlCommand cmd = new SqlCommand("SELECT IdMarca, Nombre FROM dbo.Marcas ORDER BY Nombre", conexion);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        marcas.Add(new Marca
                        {
                            IdMarca = Convert.ToInt32(reader["IdMarca"]),
                            Nombre = reader["Nombre"].ToString()
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ObtenerMarcas", ex);
                return new List<Marca>();
            }

            return marcas;
        }

        private Producto MapearProducto(SqlDataReader reader)
        {
            return new Producto
            {
                ID_Codigo = Convert.ToInt32(reader["ID_Codigo"]),
                FK_Categoria = Convert.ToInt32(reader["FK_Categoria"]),
                FK_Marca = Convert.ToInt32(reader["FK_Marca"]),
                Nombre = reader["Nombre"].ToString(),
                Precio_Compra = Convert.ToDecimal(reader["Precio_Compra"]),
                Precio_Venta = Convert.ToDecimal(reader["Precio_Venta"]),
                Stock_Actual = Convert.ToInt32(reader["Stock_Actual"]),
                Stock_Critico = Convert.ToInt32(reader["Stock_Critico"]),
                Stock_Minimo = Convert.ToInt32(reader["Stock_Minimo"]),
                Stock_Ideal = Convert.ToInt32(reader["Stock_Ideal"]),
                Stock_Maximo = Convert.ToInt32(reader["Stock_Maximo"]),
                Punto_Reposicion = Convert.ToInt32(reader["Punto_Reposicion"]),
                Numero_Lote = reader["Numero_Lote"] == DBNull.Value ? null : reader["Numero_Lote"].ToString(),
                Fecha_Vencimiento = reader["Fecha_Vencimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["Fecha_Vencimiento"]),
                Estado = reader["Estado"].ToString(),
                Habilitado = Convert.ToBoolean(reader["Habilitado"])
            };
        }
    }
}
