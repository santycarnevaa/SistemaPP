using System;
using System.Linq;
using System.Windows.Forms;
using CapaLogica;

namespace CapaVista
{
    public partial class frmScrap : Form
    {
        private readonly string usuario;
        private int idUsuario;

        private readonly CL_servicioStock servicioStock = new CL_servicioStock();
        private readonly CL_servicioProductos servicioProductos = new CL_servicioProductos();
        private readonly CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

        public frmScrap(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void frmScrap_Load(object sender, EventArgs e)
        {
            idUsuario = servicioUsuarios.ObtenerIdUsuario(usuario);

            cmbProducto.DataSource = servicioProductos.ObtenerProductos();
            cmbProducto.DisplayMember = "Nombre";
            cmbProducto.ValueMember = "ID_Codigo";
            cmbProducto.SelectedIndex = -1;

            dtpHasta.Value = DateTime.Today;

            CargarScrapDelPeriodo();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CargarScrapDelPeriodo();
        }

        private void CargarScrapDelPeriodo()
        {
            var registros = servicioStock.ObtenerScrapDelPeriodo(dtpDesde.Value.Date, dtpHasta.Value.Date.AddDays(1).AddSeconds(-1));

            dgvScrap.DataSource = null;
            dgvScrap.DataSource = registros;

            foreach (string columna in new[] { "FK_Producto", "FK_Usuario" })
            {
                if (dgvScrap.Columns[columna] != null)
                    dgvScrap.Columns[columna].Visible = false;
            }

            int unidades = registros.Sum(r => r.Cantidad);
            decimal total = registros.Sum(r => r.Costo_Valorizado);
            lblTotalPeriodo.Text = $"Total del período: {unidades} unidades — costo valorizado: {total:C}";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue == null)
            {
                new MensajeError("Seleccioná un producto.").Mostrar();
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                new MensajeError("La cantidad debe ser un número entero mayor a cero.").Mostrar();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                new MensajeError("Ingresá el motivo del scrap.").Mostrar();
                return;
            }

            int idProducto = (int)cmbProducto.SelectedValue;

            var (exito, mensaje) = servicioStock.RegistrarScrap(idProducto, cantidad, txtMotivo.Text.Trim(), idUsuario);

            if (exito)
            {
                new MensajeExito("Scrap registrado correctamente.").Mostrar();
                txtCantidad.Text = "";
                txtMotivo.Text = "";
                CargarScrapDelPeriodo();
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
