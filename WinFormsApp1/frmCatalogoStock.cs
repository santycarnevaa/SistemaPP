using System;
using System.Windows.Forms;

namespace CapaVista
{
    public partial class frmCatalogoStock : Form
    {
        private readonly string usuario;

        public frmCatalogoStock(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            new frmProductos(usuario).ShowDialog();
        }

        private void btnMovimientos_Click(object sender, EventArgs e)
        {
            new frmMovimientosStock(usuario).ShowDialog();
        }

        private void btnScrap_Click(object sender, EventArgs e)
        {
            new frmScrap(usuario).ShowDialog();
        }

        private void btnAuditoria_Click(object sender, EventArgs e)
        {
            new frmAuditoria().ShowDialog();
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            new frmReportesProductos().ShowDialog();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
