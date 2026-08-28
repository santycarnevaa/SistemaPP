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

namespace CapaVista
{
    public partial class frmAdmin : Form
    {
        private string usuario;
        private CL_servicioConfiguracionCorreo servicioConfigCorreo = new CL_servicioConfiguracionCorreo();

        public frmAdmin(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void btnRegistros_Click(object sender, EventArgs e)
        {
            frmRegistro frm = new frmRegistro(usuario);
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void btnPreg_Click(object sender, EventArgs e)
        {
            frmConfigAdmin frm = new frmConfigAdmin(usuario);
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            lblBienvenido.Text = "Bienvenido, " + usuario;

            if (servicioConfigCorreo.CorrespondeRotar())
            {
                MessageBox.Show(
                    "Corresponde rotar la contraseña de aplicación de correo. Podés hacerlo desde Configuración.",
                    "Rotación de credenciales",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}
