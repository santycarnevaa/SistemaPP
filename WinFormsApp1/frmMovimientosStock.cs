using System;
using System.Windows.Forms;
using CapaLogica;

namespace CapaVista
{
    public partial class frmMovimientosStock : Form
    {
        private readonly string usuario;
        private int idUsuario;

        private readonly CL_servicioStock servicioStock = new CL_servicioStock();
        private readonly CL_servicioProductos servicioProductos = new CL_servicioProductos();
        private readonly CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

        public frmMovimientosStock(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void frmMovimientosStock_Load(object sender, EventArgs e)
        {
            idUsuario = servicioUsuarios.ObtenerIdUsuario(usuario);

            cmbProducto.DataSource = servicioProductos.ObtenerProductos();
            cmbProducto.DisplayMember = "Nombre";
            cmbProducto.ValueMember = "ID_Codigo";
            cmbProducto.SelectedIndex = -1;

            dtpHasta.Value = DateTime.Today;
        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarKardex();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CargarKardex();
        }

        private void CargarKardex()
        {
            if (cmbProducto.SelectedValue == null)
            {
                dgvKardex.DataSource = null;
                return;
            }

            int idProducto = (int)cmbProducto.SelectedValue;

            dgvKardex.DataSource = null;
            dgvKardex.DataSource = servicioStock.ObtenerKardex(idProducto, dtpDesde.Value.Date, dtpHasta.Value.Date.AddDays(1).AddSeconds(-1));

            foreach (string columna in new[] { "FK_Producto", "FK_Usuario" })
            {
                if (dgvKardex.Columns[columna] != null)
                    dgvKardex.Columns[columna].Visible = false;
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue == null)
            {
                new MensajeError("Seleccioná un producto.").Mostrar();
                return;
            }

            if (cmbTipoMovimiento.SelectedItem == null)
            {
                new MensajeError("Seleccioná el tipo de movimiento.").Mostrar();
                return;
            }

            string tipoMovimiento = cmbTipoMovimiento.SelectedItem.ToString();
            bool esAjuste = tipoMovimiento == "Ajuste";

            // Ingreso y Egreso: entero > 0. Ajuste: entero distinto de 0, con signo (+ suma, - resta).
            if (!int.TryParse(txtCantidad.Text, out int cantidad) || (esAjuste ? cantidad == 0 : cantidad <= 0))
            {
                new MensajeError(esAjuste
                    ? "La cantidad del ajuste debe ser un número entero distinto de cero (negativo para restar)."
                    : "La cantidad debe ser un número entero mayor a cero.").Mostrar();
                return;
            }

            int idProducto = (int)cmbProducto.SelectedValue;

            var (exito, mensaje) = servicioStock.RegistrarMovimiento(idProducto, tipoMovimiento, cantidad, idUsuario);

            if (exito)
            {
                new MensajeExito("Movimiento registrado correctamente.").Mostrar();
                txtCantidad.Text = "";
                CargarKardex();
            }
            else
            {
                new MensajeError(mensaje).Mostrar();
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
