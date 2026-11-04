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
            EstiloSistema.Aplicar(this);
            ComboBuscable.Activar(cmbProducto);
            this.usuario = usuario;
        }

        private void frmMovimientosStock_Load(object sender, EventArgs e)
        {
            idUsuario = servicioUsuarios.ObtenerIdUsuario(usuario);

            // Los ajustes manuales de stock son solo del administrador (también se valida en Lógica).
            if (!servicioUsuarios.EsAdministradorPorId(idUsuario))
                cmbTipoMovimiento.Items.Remove("Ajuste");

            // DisplayMember/ValueMember van ANTES del DataSource: si no, al asignarlos el combo
            // dispara SelectedIndexChanged con un Producto como SelectedValue y la pantalla se cierra.
            cmbProducto.DisplayMember = "Nombre";
            cmbProducto.ValueMember = "ID_Codigo";
            cmbProducto.DataSource = servicioProductos.ObtenerProductos();
            cmbProducto.SelectedIndex = -1;

            // no se puede elegir una fecha posterior a hoy
            dtpDesde.MaxDate = DateTime.Today;
            dtpHasta.MaxDate = DateTime.Today;
            dtpHasta.Value = DateTime.Today;

            // el filtrado es automatico: al cambiar las fechas se vuelve a cargar
            dtpDesde.ValueChanged += CambioFecha;
            dtpHasta.ValueChanged += CambioFecha;
        }

        private void CambioFecha(object sender, EventArgs e)
        {
            CargarKardex();
        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarKardex();
        }

        private void CargarKardex()
        {
            if (cmbProducto.SelectedValue is not int idProducto)
            {
                dgvKardex.DataSource = null;
                return;
            }

            dgvKardex.DataSource = null;
            dgvKardex.DataSource = servicioStock.ObtenerKardex(idProducto, dtpDesde.Value.Date, dtpHasta.Value.Date.AddDays(1).AddSeconds(-1));

            foreach (string columna in new[] { "FK_Producto", "FK_Usuario" })
            {
                if (dgvKardex.Columns[columna] != null)
                    dgvKardex.Columns[columna].Visible = false;
            }

            AjusteVentana.AjustarAGrilla(this, dgvKardex);
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ErrorCampo.Limpiar(this);

            if (cmbProducto.SelectedValue == null)
            {
                ErrorCampo.Mostrar(cmbProducto, "Seleccioná un producto.");
                return;
            }

            if (cmbTipoMovimiento.SelectedItem == null)
            {
                ErrorCampo.Mostrar(cmbTipoMovimiento, "Seleccioná el tipo.");
                return;
            }

            string tipoMovimiento = cmbTipoMovimiento.SelectedItem.ToString();
            bool esAjuste = tipoMovimiento == "Ajuste";

            // Ingreso y Egreso: entero > 0. Ajuste: entero distinto de 0, con signo (+ suma, - resta).
            if (!int.TryParse(txtCantidad.Text, out int cantidad) || (esAjuste ? cantidad == 0 : cantidad <= 0))
            {
                ErrorCampo.Mostrar(txtCantidad, esAjuste
                    ? "Entero distinto de 0 (negativo para restar)."
                    : "Debe ser un entero mayor a 0.");
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
