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

        private readonly CL_servicioProductos servicioProductos = new CL_servicioProductos();
        private readonly CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

        public frmProductos(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
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

        private void ActualizarGrilla()
        {
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = servicioProductos.ObtenerProductos();

            if (dgvProductos.Columns["FK_Categoria"] != null)
                dgvProductos.Columns["FK_Categoria"].Visible = false;

            if (dgvProductos.Columns["FK_Marca"] != null)
                dgvProductos.Columns["FK_Marca"].Visible = false;

            if (dgvProductos.Columns["Habilitado"] != null)
                dgvProductos.Columns["Habilitado"].Visible = false;
        }

        private void dgvProductos_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvProductos.Rows[e.RowIndex].DataBoundItem == null)
                return;

            Producto p = (Producto)dgvProductos.Rows[e.RowIndex].DataBoundItem;

            idSeleccionado = p.ID_Codigo;
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
            if (dgvProductos.Rows.Count > 0)
                dgvProductos.ClearSelection();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposNumericos(out Producto producto))
                return;

            producto.ID_Codigo = idSeleccionado;
            producto.Estado = "Activo";

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
            dtpFechaVencimiento.Enabled = chkTieneVencimiento.Checked;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LimpiarCampos()
        {
            idSeleccionado = 0;
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

            if (!decimal.TryParse(txtPrecioCompra.Text, out decimal precioCompra) ||
                !decimal.TryParse(txtPrecioVenta.Text, out decimal precioVenta))
            {
                new MensajeError("Los precios deben ser valores numéricos.").Mostrar();
                return false;
            }

            if (!int.TryParse(txtStockCritico.Text, out int stockCritico) ||
                !int.TryParse(txtStockMinimo.Text, out int stockMinimo) ||
                !int.TryParse(txtStockIdeal.Text, out int stockIdeal) ||
                !int.TryParse(txtStockMaximo.Text, out int stockMaximo) ||
                !int.TryParse(txtPuntoReposicion.Text, out int puntoReposicion))
            {
                new MensajeError("Los niveles de stock deben ser números enteros.").Mostrar();
                return false;
            }

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
