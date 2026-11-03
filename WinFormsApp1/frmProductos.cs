using System;
using System.Windows.Forms;
using CapaLogica;
using Entidades;

namespace CapaVista
{
    public partial class frmProductos : Form
    {
        private readonly string usuario;
        private int idUsuario;
        private int idSeleccionado = 0;
        private string estadoSeleccionado = "Activo";

        private readonly CL_servicioProductos servicioProductos = new CL_servicioProductos();
        private readonly CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

        private readonly bool soloLectura;

        public frmProductos(string usuario, bool soloLectura = false)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.soloLectura = soloLectura;

            if (soloLectura)
                ActivarModoConsulta();
        }

        // Para el usuario común: se pueden ver los productos y su stock, pero no tocarlos.
        private void ActivarModoConsulta()
        {
            Text = "Productos (solo consulta)";

            btnNuevo.Visible = false;
            btnGuardar.Visible = false;
            btnBaja.Visible = false;
            btnCancelar.Visible = false;

            cmbCategoria.Enabled = false;
            cmbMarca.Enabled = false;
            chkTieneVencimiento.Enabled = false;
            dtpFechaVencimiento.Enabled = false;

            foreach (TextBox caja in new[] { txtNombre, txtPrecioCompra, txtPrecioVenta, txtStockCritico,
                                             txtStockMinimo, txtStockIdeal, txtStockMaximo, txtPuntoReposicion, txtNumeroLote })
                caja.ReadOnly = true;
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            idUsuario = servicioUsuarios.ObtenerIdUsuario(usuario);

            CargarCategorias();
            CargarMarcas();
            ActualizarGrilla();
            LimpiarCampos();
        }

        private void CargarCategorias()
        {
            cmbCategoria.DataSource = servicioProductos.ObtenerCategorias();
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "IdCategoria";
            cmbCategoria.SelectedIndex = -1;
        }

        private void CargarMarcas()
        {
            cmbMarca.DataSource = servicioProductos.ObtenerMarcas();
            cmbMarca.DisplayMember = "Nombre";
            cmbMarca.ValueMember = "IdMarca";
            cmbMarca.SelectedIndex = -1;
        }

        // Mientras se rearma la grilla no hay que cargar el formulario con la fila que el
        // control selecciona solo: si no, un "alta" terminaría pisando el primer producto.
        private bool cargandoGrilla;

        private void ActualizarGrilla()
        {
            cargandoGrilla = true;

            dgvProductos.DataSource = null;
            dgvProductos.DataSource = servicioProductos.ObtenerProductos();

            if (dgvProductos.Columns["FK_Categoria"] != null)
                dgvProductos.Columns["FK_Categoria"].Visible = false;

            if (dgvProductos.Columns["FK_Marca"] != null)
                dgvProductos.Columns["FK_Marca"].Visible = false;

            if (dgvProductos.Columns["Habilitado"] != null)
                dgvProductos.Columns["Habilitado"].Visible = false;

            AjusteVentana.AjustarAGrilla(this, dgvProductos);

            DeseleccionarGrilla();
            cargandoGrilla = false;
        }

        private void DeseleccionarGrilla()
        {
            dgvProductos.ClearSelection();
            dgvProductos.CurrentCell = null;
        }

        // RowEnter cubre la navegación con teclado; CellClick cubre volver a hacer clic en la
        // fila que ya era la actual (en ese caso RowEnter no se dispara).
        private void dgvProductos_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            // El control también selecciona filas solo (al mostrarse el formulario): solo vale
            // lo que hace el usuario, y eso implica que la grilla tiene el foco.
            if (cargandoGrilla || !dgvProductos.Focused)
                return;

            MostrarProducto(e.RowIndex);
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MostrarProducto(e.RowIndex);
        }

        private void MostrarProducto(int rowIndex)
        {
            if (rowIndex < 0 || dgvProductos.Rows[rowIndex].DataBoundItem == null)
                return;

            Producto p = (Producto)dgvProductos.Rows[rowIndex].DataBoundItem;

            idSeleccionado = p.ID_Codigo;
            estadoSeleccionado = p.Estado;
            cmbCategoria.SelectedValue = p.FK_Categoria;
            cmbMarca.SelectedValue = p.FK_Marca;
            txtNombre.Text = p.Nombre;
            txtPrecioCompra.Text = p.Precio_Compra.ToString();
            txtPrecioVenta.Text = p.Precio_Venta.ToString();
            txtStockCritico.Text = p.Stock_Critico.ToString();
            txtStockMinimo.Text = p.Stock_Minimo.ToString();
            txtStockIdeal.Text = p.Stock_Ideal.ToString();
            txtStockMaximo.Text = p.Stock_Maximo.ToString();
            txtPuntoReposicion.Text = p.Punto_Reposicion.ToString();
            txtNumeroLote.Text = p.Numero_Lote;

            if (p.Fecha_Vencimiento.HasValue)
            {
                chkTieneVencimiento.Checked = true;
                dtpFechaVencimiento.Value = p.Fecha_Vencimiento.Value;
            }
            else
            {
                chkTieneVencimiento.Checked = false;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            DeseleccionarGrilla();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposNumericos(out Producto producto))
                return;

            producto.ID_Codigo = idSeleccionado;
            producto.Estado = idSeleccionado == 0 ? "Activo" : estadoSeleccionado;

            resultadoProducto resultado;

            if (idSeleccionado == 0)
            {
                resultado = servicioProductos.RegistrarProducto(producto, idUsuario);
            }
            else
            {
                resultado = servicioProductos.ActualizarProducto(producto, idUsuario);
            }

            if (resultado == resultadoProducto.Ok)
            {
                new MensajeExito(servicioProductos.ObtenerMensajeResultado(resultado)).Mostrar();
                ActualizarGrilla();
                LimpiarCampos();
            }
            else
            {
                new MensajeError(servicioProductos.ObtenerMensajeResultado(resultado)).Mostrar();
            }
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                new MensajeError("Seleccioná un producto de la grilla.").Mostrar();
                return;
            }

            if (!new MensajeConfirmacion("¿Confirmás dar de baja este producto?").Confirmado())
                return;

            bool ok = servicioProductos.DarBajaProducto(idSeleccionado, idUsuario);

            if (ok)
            {
                new MensajeExito("Producto dado de baja.").Mostrar();
                ActualizarGrilla();
                LimpiarCampos();
            }
            else
            {
                new MensajeError("No se pudo dar de baja el producto.").Mostrar();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void chkTieneVencimiento_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaVencimiento.Enabled = chkTieneVencimiento.Checked && !soloLectura;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LimpiarCampos()
        {
            idSeleccionado = 0;
            estadoSeleccionado = "Activo";
            cmbCategoria.SelectedIndex = -1;
            cmbMarca.SelectedIndex = -1;
            txtNombre.Text = "";
            txtPrecioCompra.Text = "";
            txtPrecioVenta.Text = "";
            txtStockCritico.Text = "";
            txtStockMinimo.Text = "";
            txtStockIdeal.Text = "";
            txtStockMaximo.Text = "";
            txtPuntoReposicion.Text = "";
            txtNumeroLote.Text = "";
            chkTieneVencimiento.Checked = false;
        }

        // Dice exactamente qué campo falta o está mal y lo deja con el foco.
        private bool LeerEntero(TextBox caja, string nombreCampo, out int valor)
        {
            valor = 0;

            if (string.IsNullOrWhiteSpace(caja.Text))
            {
                new MensajeError($"Completá el campo \"{nombreCampo}\".").Mostrar();
                caja.Focus();
                return false;
            }

            if (!int.TryParse(caja.Text, System.Globalization.NumberStyles.Integer, null, out valor))
            {
                new MensajeError($"\"{nombreCampo}\" debe ser un número entero, sin decimales ni separadores.").Mostrar();
                caja.Focus();
                return false;
            }

            return true;
        }

        // Sin separador de miles: con cultura es-AR "100.50" se leería como 10050, así que se rechaza.
        private bool LeerPrecio(TextBox caja, string nombreCampo, out decimal valor)
        {
            valor = 0;

            if (string.IsNullOrWhiteSpace(caja.Text))
            {
                new MensajeError($"Completá el campo \"{nombreCampo}\".").Mostrar();
                caja.Focus();
                return false;
            }

            if (!decimal.TryParse(caja.Text, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite,
                    System.Globalization.CultureInfo.CurrentCulture, out valor))
            {
                string separador = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                new MensajeError($"\"{nombreCampo}\" debe ser un número (decimales con \"{separador}\", sin separador de miles).").Mostrar();
                caja.Focus();
                return false;
            }

            return true;
        }

        private bool ValidarCamposNumericos(out Producto producto)
        {
            producto = null;

            if (cmbCategoria.SelectedValue == null)
            {
                new MensajeError("Seleccioná una categoría.").Mostrar();
                return false;
            }

            if (cmbMarca.SelectedValue == null)
            {
                new MensajeError("Seleccioná una marca.").Mostrar();
                return false;
            }

            if (!LeerPrecio(txtPrecioCompra, "Precio de compra", out decimal precioCompra) ||
                !LeerPrecio(txtPrecioVenta, "Precio de venta", out decimal precioVenta))
                return false;

            if (!LeerEntero(txtStockCritico, "Stock crítico", out int stockCritico) ||
                !LeerEntero(txtStockMinimo, "Stock mínimo", out int stockMinimo) ||
                !LeerEntero(txtStockIdeal, "Stock ideal", out int stockIdeal) ||
                !LeerEntero(txtStockMaximo, "Stock máximo", out int stockMaximo) ||
                !LeerEntero(txtPuntoReposicion, "Punto de reposición", out int puntoReposicion))
                return false;

            producto = new Producto
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
