using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaLogica;
using Entidades;

namespace CapaVista
{
    public partial class frmUsuario : Form
    {
        private string usuario;

        public frmUsuario(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void frmUsuario_Load(object sender, EventArgs e)
        {
            lblBienvenido.Text = "Bienvenido, " + usuario;

            // el catálogo y stock es solo para el rol ventas, el resto de los roles ve el panel sin ese botón
            btnCatalogoStock.Visible = EsRolVentas();
        }

        private bool EsRolVentas()
        {
            CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

            int idRol = servicioUsuarios.ObtenerRolUsuario(usuario);
            rol rolDelUsuario = servicioUsuarios.ListarRoles().FirstOrDefault(r => r.IdRol == idRol);

            return rolDelUsuario != null && rolDelUsuario.NombreRol.Equals("ventas", StringComparison.OrdinalIgnoreCase);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmUserConfig frm = new frmUserConfig(usuario);
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        // El usuario común entra al mismo menú que el administrador, pero con permisos acotados
        // (consulta de productos, movimientos sin ajustes, scrap, reportes y su propia actividad).
        private void btnCatalogoStock_Click(object sender, EventArgs e)
        {
            new frmCatalogoStock(usuario).ShowDialog();
        }

        private void lblBienvenido_Click(object sender, EventArgs e)
        {

        }
    }
}
