using System;
using System.Windows.Forms;
using CapaLogica;

namespace CapaVista
{
    public partial class frmAuditoria : Form
    {
        private readonly CL_servicioAuditoria servicioAuditoria = new CL_servicioAuditoria();

        public frmAuditoria()
        {
            InitializeComponent();
        }

        private void frmAuditoria_Load(object sender, EventArgs e)
        {
            cmbTabla.SelectedIndex = 0;
            dtpHasta.Value = DateTime.Today;
            Filtrar();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            Filtrar();
        }

        private void Filtrar()
        {
            string tabla = cmbTabla.SelectedItem?.ToString();

            if (tabla == "(Todas)")
                tabla = null;

            var registros = servicioAuditoria.ConsultarAuditoria(
                tabla, null, dtpDesde.Value.Date, dtpHasta.Value.Date.AddDays(1).AddSeconds(-1));

            dgvAuditoria.DataSource = null;
            dgvAuditoria.DataSource = registros;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
