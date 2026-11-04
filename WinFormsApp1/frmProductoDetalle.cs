using System;
using System.Windows.Forms;
using CapaLogica;
using Entidades;

namespace CapaVista
{
    // Ventana para dar de alta, modificar o solo consultar un producto.
    // Si no recibe producto es un alta. Devuelve OK cuando se guardo algo.
    public partial class frmProductoDetalle : Form
    {
        private readonly int idUsuario;
        private readonly Producto producto;
        private readonly bool consulta;

        private readonly CL_servicioProductos servicioProductos = new CL_servicioProductos();

        public frmProductoDetalle(int idUsuario, Producto producto, bool consulta)
        {
            InitializeComponent();
            EstiloSistema.Aplicar(this);
            ComboBuscable.Activar(cmbCategoria);
            ComboBuscable.Activar(cmbMarca);
            this.idUsuario = idUsuario;
            this.producto = producto;
            this.consulta = consulta;
        }

        private void frmProductoDetalle_Load(object sender, EventArgs e)
        {
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "IdCategoria";
            cmbCategoria.DataSource = servicioProductos.ObtenerCategorias();
            cmbCategoria.SelectedIndex = -1;

            cmbMarca.DisplayMember = "Nombre";
            cmbMarca.ValueMember = "IdMarca";
            cmbMarca.DataSource = servicioProductos.ObtenerMarcas();
            cmbMarca.SelectedIndex = -1;

            if (producto == null)
                Text = "Nuevo producto";
            else
            {
                Text = consulta ? "Detalle del producto" : "Modificar producto";
                MostrarProducto();
            }

            if (consulta)
                ActivarModoConsulta();
            else
                LimitarVencimiento();
        }

        private void MostrarProducto()
        {
            cmbCategoria.SelectedValue = producto.FK_Categoria;
            cmbMarca.SelectedValue = producto.FK_Marca;
            txtNombre.Text = producto.Nombre;
            txtPrecioCompra.Text = producto.Precio_Compra.ToString();
            txtPrecioVenta.Text = producto.Precio_Venta.ToString();
            txtStockCritico.Text = producto.Stock_Critico.ToString();
            txtStockMinimo.Text = producto.Stock_Minimo.ToString();
            txtStockIdeal.Text = producto.Stock_Ideal.ToString();
            txtStockMaximo.Text = producto.Stock_Maximo.ToString();
            txtPuntoReposicion.Text = producto.Punto_Reposicion.ToString();
            txtNumeroLote.Text = producto.Numero_Lote;

            if (producto.Fecha_Vencimiento.HasValue)
            {
                chkTieneVencimiento.Checked = true;
                dtpFechaVencimiento.Value = producto.Fecha_Vencimiento.Value;
            }
        }

        // El vencimiento se elige de hoy en adelante. Si el producto ya esta vencido se deja ver
        // su fecha, pero para guardarlo hay que corregirla (se controla al validar).
        private void LimitarVencimiento()
        {
            DateTime minimo = DateTime.Today;

            if (producto != null && producto.Fecha_Vencimiento.HasValue && producto.Fecha_Vencimiento.Value.Date < minimo)
                minimo = producto.Fecha_Vencimiento.Value.Date;

            dtpFechaVencimiento.MinDate = minimo;
        }

        private void ActivarModoConsulta()
        {
            btnGuardar.Visible = false;
            btnCancelar.Text = "Cerrar";

            cmbCategoria.Enabled = false;
            cmbMarca.Enabled = false;
            chkTieneVencimiento.AutoCheck = false;
            dtpFechaVencimiento.Enabled = false;

            foreach (TextBox caja in new[] { txtNombre, txtPrecioCompra, txtPrecioVenta, txtStockCritico,
                                             txtStockMinimo, txtStockIdeal, txtStockMaximo, txtPuntoReposicion, txtNumeroLote })
                caja.ReadOnly = true;
        }

        private void chkTieneVencimiento_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaVencimiento.Enabled = chkTieneVencimiento.Checked && !consulta;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposNumericos(out Producto datos))
                return;

            resultadoProducto resultado;

            if (producto == null)
            {
                datos.Estado = "Activo";
                resultado = servicioProductos.RegistrarProducto(datos, idUsuario);
            }
            else
            {
                datos.ID_Codigo = producto.ID_Codigo;
                datos.Estado = producto.Estado;
                resultado = servicioProductos.ActualizarProducto(datos, idUsuario);
            }

            if (resultado == resultadoProducto.Ok)
            {
                new MensajeExito(servicioProductos.ObtenerMensajeResultado(resultado)).Mostrar();
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                new MensajeError(servicioProductos.ObtenerMensajeResultado(resultado)).Mostrar();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Dice qué campo falta o está mal con un texto abajo del campo.
        private bool LeerEntero(TextBox caja, out int valor)
        {
            valor = 0;

            if (string.IsNullOrWhiteSpace(caja.Text))
            {
                ErrorCampo.Mostrar(caja, "Completá este campo.");
                return false;
            }

            if (!int.TryParse(caja.Text, System.Globalization.NumberStyles.Integer, null, out valor))
            {
                ErrorCampo.Mostrar(caja, "Debe ser un número entero.");
                return false;
            }

            return true;
        }

        // Sin separador de miles: con cultura es-AR "100.50" se leería como 10050, así que se rechaza.
        private bool LeerPrecio(TextBox caja, out decimal valor)
        {
            valor = 0;

            if (string.IsNullOrWhiteSpace(caja.Text))
            {
                ErrorCampo.Mostrar(caja, "Completá este campo.");
                return false;
            }

            if (!decimal.TryParse(caja.Text, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite,
                    System.Globalization.CultureInfo.CurrentCulture, out valor))
            {
                string separador = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                ErrorCampo.Mostrar(caja, $"Número inválido (decimales con \"{separador}\").");
                return false;
            }

            return true;
        }

        private bool ValidarCamposNumericos(out Producto datos)
        {
            datos = null;
            ErrorCampo.Limpiar(this);

            bool ok = true;

            if (cmbCategoria.SelectedValue == null)
            {
                ErrorCampo.Mostrar(cmbCategoria, "Seleccioná una categoría.");
                ok = false;
            }

            if (cmbMarca.SelectedValue == null)
            {
                ErrorCampo.Mostrar(cmbMarca, "Seleccioná una marca.");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                ErrorCampo.Mostrar(txtNombre, "Completá este campo.");
                ok = false;
            }

            ok &= LeerPrecio(txtPrecioCompra, out decimal precioCompra);
            ok &= LeerPrecio(txtPrecioVenta, out decimal precioVenta);
            ok &= LeerEntero(txtStockCritico, out int stockCritico);
            ok &= LeerEntero(txtStockMinimo, out int stockMinimo);
            ok &= LeerEntero(txtStockIdeal, out int stockIdeal);
            ok &= LeerEntero(txtStockMaximo, out int stockMaximo);
            ok &= LeerEntero(txtPuntoReposicion, out int puntoReposicion);

            // un producto no se puede guardar ya vencido
            if (chkTieneVencimiento.Checked && dtpFechaVencimiento.Value.Date < DateTime.Today)
            {
                ErrorCampo.Mostrar(dtpFechaVencimiento, "La fecha no puede ser anterior a hoy.");
                ok = false;
            }

            if (!ok)
                return false;

            datos = new Producto
            {
                FK_Categoria = (int)cmbCategoria.SelectedValue,
                FK_Marca = (int)cmbMarca.SelectedValue,
                Nombre = txtNombre.Text.Trim(),
                Precio_Compra = precioCompra,
                Precio_Venta = precioVenta,
                Stock_Critico = stockCritico,
                Stock_Minimo = stockMinimo,
                Stock_Ideal = stockIdeal,
                Stock_Maximo = stockMaximo,
                Punto_Reposicion = puntoReposicion,
                Numero_Lote = txtNumeroLote.Text.Trim(),
                Fecha_Vencimiento = chkTieneVencimiento.Checked ? dtpFechaVencimiento.Value.Date : (DateTime?)null
            };

            return true;
        }
    }
}
