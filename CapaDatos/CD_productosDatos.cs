using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;
using Utilidades;

namespace CapaDatos
{
    public class CD_productosDatos : CD_ejecutarConsultasSP
    {
        public List<Producto> ObtenerProductos(bool soloHabilitados = true)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@SoloHabilitados", SqlDbType.Bit) { Value = soloHabilitados }
            };

            try
            {
                return ConvertirAProductos(EjecutarConsulta("dbo.SP_ObtenerProductos", parametros));
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ObtenerProductos", ex);
                return new List<Producto>();
            }
        }

        public Producto ObtenerProductoPorId(int idCodigo)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@ID_Codigo", SqlDbType.Int) { Value = idCodigo }
            };

            try
            {
                List<Producto> productos = ConvertirAProductos(EjecutarConsulta("dbo.SP_ObtenerProductoPorId", parametros));
                return productos.Count > 0 ? productos[0] : null;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ObtenerProductoPorId", ex);
                return null;
            }
        }

        public List<Producto> BuscarProductos(string texto, int? idCategoria, int? idMarca)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@Texto", SqlDbType.VarChar, 150) { Value = ValorONulo(texto) },
                new SqlParameter("@FK_Categoria", SqlDbType.Int) { Value = (object?)idCategoria ?? DBNull.Value },
                new SqlParameter("@FK_Marca", SqlDbType.Int) { Value = (object?)idMarca ?? DBNull.Value }
            };

            try
            {
                return ConvertirAProductos(EjecutarConsulta("dbo.SP_BuscarProductos", parametros));
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.BuscarProductos", ex);
                return new List<Producto>();
            }
        }

        // el sp guarda la auditoria en la misma transaccion que el alta
        public int InsertarProducto(Producto p, int idUsuario)
        {
            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_InsertarProducto", ParametrosProducto(p, idUsuario).ToArray()));

                return resultado != null ? Convert.ToInt32(resultado) : -1;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.InsertarProducto", ex);
                return -1;
            }
        }

        public bool ActualizarProducto(Producto p, int idUsuario)
        {
            List<SqlParameter> parametros = ParametrosProducto(p, idUsuario);
            parametros.Add(new SqlParameter("@ID_Codigo", SqlDbType.Int) { Value = p.ID_Codigo });
            parametros.Add(new SqlParameter("@Estado", SqlDbType.VarChar, 30) { Value = p.Estado });

            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_ActualizarProducto", parametros.ToArray()));

                return resultado != null && Convert.ToInt32(resultado) > 0;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ActualizarProducto", ex);
                return false;
            }
        }

        public bool DarBajaProducto(int idCodigo, int idUsuario)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@ID_Codigo", SqlDbType.Int) { Value = idCodigo },
                new SqlParameter("@FK_Usuario", SqlDbType.Int) { Value = idUsuario }
            };

            try
            {
                object? resultado = PrimerValor(EjecutarConsulta("dbo.SP_DarBajaProducto", parametros));

                return resultado != null && Convert.ToInt32(resultado) > 0;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.DarBajaProducto", ex);
                return false;
            }
        }

        public List<Producto> ObtenerBajoPuntoReposicion()
        {
            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerProductosBajoPuntoReposicion", null);

                List<Producto> productos = new List<Producto>();
                foreach (DataRow fila in tabla.Rows)
                {
                    productos.Add(new Producto
                    {
                        ID_Codigo = Convert.ToInt32(fila["ID_Codigo"]),
                        Nombre = fila["Nombre"].ToString(),
                        Stock_Actual = Convert.ToInt32(fila["Stock_Actual"]),
                        Punto_Reposicion = Convert.ToInt32(fila["Punto_Reposicion"])
                    });
                }

                return productos;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ObtenerBajoPuntoReposicion", ex);
                return new List<Producto>();
            }
        }

        public List<Producto> ObtenerProximosAVencer(int diasAnticipacion)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("@DiasAnticipacion", SqlDbType.Int) { Value = diasAnticipacion }
            };

            try
            {
                DataTable tabla = EjecutarConsulta("dbo.SP_ObtenerProductosProximosAVencer", parametros);

                List<Producto> productos = new List<Producto>();
                foreach (DataRow fila in tabla.Rows)
                {
                    productos.Add(new Producto
                    {
                        ID_Codigo = Convert.ToInt32(fila["ID_Codigo"]),
                        Nombre = fila["Nombre"].ToString(),
                        Numero_Lote = fila["Numero_Lote"] == DBNull.Value ? null : fila["Numero_Lote"].ToString(),
                        Fecha_Vencimiento = fila["Fecha_Vencimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(fila["Fecha_Vencimiento"]),
                        Stock_Actual = Convert.ToInt32(fila["Stock_Actual"])
                    });
                }

                return productos;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ObtenerProximosAVencer", ex);
                return new List<Producto>();
            }
        }

        public List<ReporteStock> ReporteStockPorCategoria()
        {
            return ObtenerReporteStock("dbo.SP_ReporteStockPorCategoria", "Categoria", "CD_productosDatos.ReporteStockPorCategoria");
        }

        public List<ReporteStock> ReporteStockPorMarca()
        {
            return ObtenerReporteStock("dbo.SP_ReporteStockPorMarca", "Marca", "CD_productosDatos.ReporteStockPorMarca");
        }

        // los dos sp de reportes devuelven las mismas columnas, solo cambia
        // el nombre de la primera (categoria o marca)
        private List<ReporteStock> ObtenerReporteStock(string nombreSP, string columnaNombre, string origen)
        {
            try
            {
                DataTable tabla = EjecutarConsulta(nombreSP, null);

                List<ReporteStock> filas = new List<ReporteStock>();
                foreach (DataRow fila in tabla.Rows)
                {
                    filas.Add(new ReporteStock
                    {
                        Nombre = fila[columnaNombre].ToString(),
                        Cantidad_Productos = Convert.ToInt32(fila["Cantidad_Productos"]),
                        Stock_Total = Convert.ToInt32(fila["Stock_Total"])
                    });
                }

                return filas;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError(origen, ex);
                return new List<ReporteStock>();
            }
        }

        public List<Categoria> ObtenerCategorias()
        {
            try
            {
                DataTable tabla = EjecutarConsulta(
                    "SELECT IdCategoria, Nombre FROM dbo.Categorias ORDER BY Nombre",
                    null, tipo: CommandType.Text);

                List<Categoria> categorias = new List<Categoria>();
                foreach (DataRow fila in tabla.Rows)
                {
                    categorias.Add(new Categoria
                    {
                        IdCategoria = Convert.ToInt32(fila["IdCategoria"]),
                        Nombre = fila["Nombre"].ToString()
                    });
                }

                return categorias;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ObtenerCategorias", ex);
                return new List<Categoria>();
            }
        }

        public List<Marca> ObtenerMarcas()
        {
            try
            {
                DataTable tabla = EjecutarConsulta(
                    "SELECT IdMarca, Nombre FROM dbo.Marcas ORDER BY Nombre",
                    null, tipo: CommandType.Text);

                List<Marca> marcas = new List<Marca>();
                foreach (DataRow fila in tabla.Rows)
                {
                    marcas.Add(new Marca
                    {
                        IdMarca = Convert.ToInt32(fila["IdMarca"]),
                        Nombre = fila["Nombre"].ToString()
                    });
                }

                return marcas;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("CD_productosDatos.ObtenerMarcas", ex);
                return new List<Marca>();
            }
        }

        // parametros que comparten el sp de insertar y el de actualizar producto
        private static List<SqlParameter> ParametrosProducto(Producto p, int idUsuario)
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@FK_Categoria", SqlDbType.Int) { Value = p.FK_Categoria },
                new SqlParameter("@FK_Marca", SqlDbType.Int) { Value = p.FK_Marca },
                new SqlParameter("@Nombre", SqlDbType.VarChar, 150) { Value = p.Nombre },
                new SqlParameter("@Precio_Compra", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = p.Precio_Compra },
                new SqlParameter("@Precio_Venta", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = p.Precio_Venta },
                new SqlParameter("@Stock_Critico", SqlDbType.Int) { Value = p.Stock_Critico },
                new SqlParameter("@Stock_Minimo", SqlDbType.Int) { Value = p.Stock_Minimo },
                new SqlParameter("@Stock_Ideal", SqlDbType.Int) { Value = p.Stock_Ideal },
                new SqlParameter("@Stock_Maximo", SqlDbType.Int) { Value = p.Stock_Maximo },
                new SqlParameter("@Punto_Reposicion", SqlDbType.Int) { Value = p.Punto_Reposicion },
                new SqlParameter("@Numero_Lote", SqlDbType.VarChar, 50) { Value = ValorONulo(p.Numero_Lote) },
                new SqlParameter("@Fecha_Vencimiento", SqlDbType.Date) { Value = (object?)p.Fecha_Vencimiento ?? DBNull.Value },
                new SqlParameter("@FK_Usuario", SqlDbType.Int) { Value = idUsuario }
            };
        }

        // pasa las filas de productos a objetos producto
        private static List<Producto> ConvertirAProductos(DataTable tabla)
        {
            List<Producto> productos = new List<Producto>();

            foreach (DataRow fila in tabla.Rows)
            {
                productos.Add(new Producto
                {
                    ID_Codigo = Convert.ToInt32(fila["ID_Codigo"]),
                    FK_Categoria = Convert.ToInt32(fila["FK_Categoria"]),
                    FK_Marca = Convert.ToInt32(fila["FK_Marca"]),
                    Nombre = fila["Nombre"].ToString(),
                    Precio_Compra = Convert.ToDecimal(fila["Precio_Compra"]),
                    Precio_Venta = Convert.ToDecimal(fila["Precio_Venta"]),
                    Stock_Actual = Convert.ToInt32(fila["Stock_Actual"]),
                    Stock_Critico = Convert.ToInt32(fila["Stock_Critico"]),
                    Stock_Minimo = Convert.ToInt32(fila["Stock_Minimo"]),
                    Stock_Ideal = Convert.ToInt32(fila["Stock_Ideal"]),
                    Stock_Maximo = Convert.ToInt32(fila["Stock_Maximo"]),
                    Punto_Reposicion = Convert.ToInt32(fila["Punto_Reposicion"]),
                    Numero_Lote = fila["Numero_Lote"] == DBNull.Value ? null : fila["Numero_Lote"].ToString(),
                    Fecha_Vencimiento = fila["Fecha_Vencimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(fila["Fecha_Vencimiento"]),
                    Estado = fila["Estado"].ToString(),
                    Habilitado = Convert.ToBoolean(fila["Habilitado"])
                });
            }

            return productos;
        }
    }
}
