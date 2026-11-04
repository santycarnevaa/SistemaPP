using System;
using System.Windows.Forms;
using CapaLogica;

namespace CapaVista
{
    // Menú del grupo 1. Lo abren tanto el administrador como el usuario común: lo que cambia
    // es lo que puede hacer cada uno dentro de cada pantalla (el administrador edita el
    // catálogo, hace ajustes y ve toda la auditoría; el usuario consulta, mueve stock y ve
    // solo su propia actividad).
    public partial class frmCatalogoStock : Form
    {
        private readonly string usuario;
        private readonly int idUsuario;
        private readonly bool esAdmin;

        public frmCatalogoStock(string usuario)
        {
            InitializeComponent();
            EstiloSistema.Aplicar(this);
            this.usuario = usuario;

            CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();
            idUsuario = servicioUsuarios.ObtenerIdUsuario(usuario);
            esAdmin = servicioUsuarios.EsAdministradorPorId(idUsuario);

            if (!esAdmin)
            {
                btnProductos.Text = "Consultar productos";
                btnAuditoria.Text = "Mi actividad";
            }
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            new frmProductos(usuario, soloLectura: !esAdmin).ShowDialog();
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
            if (esAdmin)
                new frmAuditoria().ShowDialog();
            else
                new frmAuditoria(idUsuario).ShowDialog();
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            new frmReportesProductos().ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
