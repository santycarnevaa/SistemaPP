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
            EstiloSistema.Aplicar(this);
        }

        private void frmReportesProductos_Load(object sender, EventArgs e)
        {
            cmbReporte.SelectedIndex = 0;
        }

        private void cmbReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            numDias.Enabled = cmbReporte.SelectedIndex == 1; // "Productos próximos a vencer"

            // el reporte se genera solo al elegirlo
            btnGenerar_Click(sender, e);
        }

        private void numDias_ValueChanged(object sender, EventArgs e)
        {
            if (cmbReporte.SelectedIndex == 1)
                btnGenerar_Click(sender, e);
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
