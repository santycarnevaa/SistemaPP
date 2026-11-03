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

            // los dos primeros reportes devuelven productos, pero solo completan algunas columnas
            string[] columnasVisibles = null;

            switch (cmbReporte.SelectedIndex)
            {
                case 0:
                    dgvReporte.DataSource = servicioProductos.ObtenerAlertaReposicion();
                    columnasVisibles = new[] { "ID_Codigo", "Nombre", "Stock_Actual", "Punto_Reposicion" };
                    break;
                case 1:
                    dgvReporte.DataSource = servicioProductos.ObtenerProximosAVencer((int)numDias.Value);
                    columnasVisibles = new[] { "ID_Codigo", "Nombre", "Numero_Lote", "Fecha_Vencimiento", "Stock_Actual" };
                    break;
                case 2:
                    dgvReporte.DataSource = servicioProductos.ReporteStockPorCategoria();
                    break;
                case 3:
                    dgvReporte.DataSource = servicioProductos.ReporteStockPorMarca();
                    break;
            }

            if (columnasVisibles != null)
            {
                foreach (DataGridViewColumn columna in dgvReporte.Columns)
                    columna.Visible = Array.IndexOf(columnasVisibles, columna.Name) >= 0;
            }

            AjusteVentana.AjustarAGrilla(this, dgvReporte);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
