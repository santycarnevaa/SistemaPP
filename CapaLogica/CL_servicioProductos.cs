using System.Data;
using CapaDatos;
using Entidades;

namespace CapaLogica
{
    public class CL_servicioProductos
    {
        private CD_productosDatos productosDatos = new CD_productosDatos();

        public List<Producto> ObtenerProductos(bool soloHabilitados = true)
        {
            return productosDatos.ObtenerProductos(soloHabilitados);
        }

        public Producto ObtenerProductoPorId(int idCodigo)
        {
            return productosDatos.ObtenerProductoPorId(idCodigo);
        }

        public List<Producto> BuscarProductos(string texto, int? idCategoria, int? idMarca)
        {
            return productosDatos.BuscarProductos(texto, idCategoria, idMarca);
        }

        public List<Categoria> ObtenerCategorias()
        {
            return productosDatos.ObtenerCategorias();
        }

        public List<Marca> ObtenerMarcas()
        {
            return productosDatos.ObtenerMarcas();
        }

        public resultadoProducto RegistrarProducto(Producto p, int idUsuario)
        {
            resultadoProducto validacion = Validar(p);

            if (validacion != resultadoProducto.Ok)
                return validacion;

            int idNuevo = productosDatos.InsertarProducto(p);

            if (idNuevo <= 0)
                return resultadoProducto.ErrorBaseDatos;

            CD_auditoriaDatos.Registrar("Productos", idNuevo, "Alta", idUsuario, $"Alta de producto '{p.Nombre}'");

            return resultadoProducto.Ok;
        }

        public resultadoProducto ActualizarProducto(Producto p, int idUsuario)
        {
            resultadoProducto validacion = Validar(p);

            if (validacion != resultadoProducto.Ok)
                return validacion;

            bool ok = productosDatos.ActualizarProducto(p);

            if (!ok)
                return resultadoProducto.ErrorBaseDatos;

            CD_auditoriaDatos.Registrar("Productos", p.ID_Codigo, "Modificación", idUsuario, $"Modificación de producto '{p.Nombre}'");

            return resultadoProducto.Ok;
        }

        public bool DarBajaProducto(int idCodigo, int idUsuario)
        {
            bool ok = productosDatos.DarBajaProducto(idCodigo);

            if (ok)
                CD_auditoriaDatos.Registrar("Productos", idCodigo, "Baja", idUsuario, "Baja lógica de producto");

            return ok;
        }

        public List<Producto> ObtenerAlertaReposicion()
        {
            return productosDatos.ObtenerBajoPuntoReposicion();
        }

        public List<Producto> ObtenerProximosAVencer(int diasAnticipacion)
        {
            return productosDatos.ObtenerProximosAVencer(diasAnticipacion);
        }

        public DataTable ReporteStockPorCategoria()
        {
            return productosDatos.ReporteStockPorCategoria();
        }

        public DataTable ReporteStockPorMarca()
        {
            return productosDatos.ReporteStockPorMarca();
        }

        public string ObtenerMensajeResultado(resultadoProducto resultado)
        {
            switch (resultado)
            {
                case resultadoProducto.Ok:
                    return "Producto guardado correctamente.";
                case resultadoProducto.NombreVacio:
                    return "Debe ingresar el nombre del producto.";
                case resultadoProducto.CategoriaInvalida:
                    return "Debe seleccionar una categoría.";
                case resultadoProducto.MarcaInvalida:
                    return "Debe seleccionar una marca.";
                case resultadoProducto.PreciosInvalidos:
                    return "El precio de compra y el de venta deben ser mayores a cero.";
                case resultadoProducto.OrdenStockInvalido:
                    return "Los niveles de stock deben cumplir: Crítico < Mínimo < Ideal < Máximo.";
                case resultadoProducto.ErrorBaseDatos:
                    return "No se pudo guardar el producto en la base de datos.";
                default:
                    return "Ocurrió un error desconocido.";
            }
        }

        private resultadoProducto Validar(Producto p)
        {
            if (string.IsNullOrWhiteSpace(p.Nombre))
                return resultadoProducto.NombreVacio;

            if (p.FK_Categoria <= 0)
                return resultadoProducto.CategoriaInvalida;

            if (p.FK_Marca <= 0)
                return resultadoProducto.MarcaInvalida;

            if (p.Precio_Compra <= 0 || p.Precio_Venta <= 0)
                return resultadoProducto.PreciosInvalidos;

            if (!(p.Stock_Critico < p.Stock_Minimo && p.Stock_Minimo < p.Stock_Ideal && p.Stock_Ideal < p.Stock_Maximo))
                return resultadoProducto.OrdenStockInvalido;

            return resultadoProducto.Ok;
        }
    }
}
