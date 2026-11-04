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
            EstiloSistema.Aplicar(this);
            ComboBuscable.Activar(cmbProducto);
            this.usuario = usuario;
        }

        private void frmScrap_Load(object sender, EventArgs e)
        {
            idUsuario = servicioUsuarios.ObtenerIdUsuario(usuario);

            cmbProducto.DisplayMember = "Nombre";
            cmbProducto.ValueMember = "ID_Codigo";
            cmbProducto.DataSource = servicioProductos.ObtenerProductos();
            cmbProducto.SelectedIndex = -1;

            // no se puede elegir una fecha posterior a hoy
            dtpDesde.MaxDate = DateTime.Today;
            dtpHasta.MaxDate = DateTime.Today;
            dtpHasta.Value = DateTime.Today;

            // las fechas arrancan desactivadas, cada una se activa con su casilla
            dtpDesde.Checked = false;
            dtpHasta.Checked = false;

            CargarScrapDelPeriodo();

            // el filtrado es automatico: al cambiar las fechas se vuelve a cargar
            dtpDesde.ValueChanged += CambioFecha;
            dtpHasta.ValueChanged += CambioFecha;
        }

        private void CambioFecha(object sender, EventArgs e)
        {
            CargarScrapDelPeriodo();
        }

        private void CargarScrapDelPeriodo()
        {
            // la fecha que esta desactivada no filtra
            DateTime? desde = dtpDesde.Checked ? dtpDesde.Value.Date : null;
            DateTime? hasta = dtpHasta.Checked ? dtpHasta.Value.Date.AddDays(1).AddSeconds(-1) : null;

            var registros = servicioStock.ObtenerScrapDelPeriodo(desde, hasta);

            dgvScrap.DataSource = null;
            dgvScrap.DataSource = registros;

            foreach (string columna in new[] { "FK_Producto", "FK_Usuario" })
            {
                if (dgvScrap.Columns[columna] != null)
                    dgvScrap.Columns[columna].Visible = false;
            }

            AjusteVentana.AjustarAGrilla(this, dgvScrap);

            int unidades = registros.Sum(r => r.Cantidad);
            decimal total = registros.Sum(r => r.Costo_Valorizado);
            lblTotalPeriodo.Text = $"Total del período: {unidades} unidades — costo valorizado: {total:C}";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ErrorCampo.Limpiar(this);

            if (cmbProducto.SelectedValue == null)
            {
                ErrorCampo.Mostrar(cmbProducto, "Seleccioná un producto.");
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                ErrorCampo.Mostrar(txtCantidad, "Debe ser un entero mayor a 0.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                ErrorCampo.Mostrar(txtMotivo, "Ingresá el motivo del scrap.");
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
