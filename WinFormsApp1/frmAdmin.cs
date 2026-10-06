using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista
{
    public partial class frmAdmin : Form
    {
        private string usuario;
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
            this.Show();
        }

        private void btnPreg_Click(object sender, EventArgs e)
        {
            frmConfigAdmin frm = new frmConfigAdmin(usuario);
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        // cerrar sesion: vuelve al login
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            lblBienvenido.Text = "Bienvenido, " + usuario;
        }
    }
}
