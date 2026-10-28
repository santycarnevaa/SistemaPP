using System;
using System.Windows.Forms;
using CapaLogica;

namespace CapaVista
{
    public partial class frmReportesProductos : Form
    {
        private readonly CL_servicioProductos servicioProductos = new CL_servicioProductos();

        public frmReportesProductos()
        {
            InitializeComponent();
        }

        private void frmReportesProductos_Load(object sender, EventArgs e)
        {
            cmbReporte.SelectedIndex = 0;
        }

        private void cmbReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            numDias.Enabled = cmbReporte.SelectedIndex == 1; // "Productos próximos a vencer"
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            dgvReporte.DataSource = null;

            switch (cmbReporte.SelectedIndex)
            {
                case 0:
                    dgvReporte.DataSource = servicioProductos.ObtenerAlertaReposicion();
                    break;
                case 1:
                    dgvReporte.DataSource = servicioProductos.ObtenerProximosAVencer((int)numDias.Value);
                    break;
                case 2:
                    dgvReporte.DataSource = servicioProductos.ReporteStockPorCategoria();
                    break;
                case 3:
                    dgvReporte.DataSource = servicioProductos.ReporteStockPorMarca();
                    break;
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
