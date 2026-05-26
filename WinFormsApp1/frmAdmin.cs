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
        public frmAdmin()
        {
            InitializeComponent();
        }

        private void btnRegistros_Click(object sender, EventArgs e)
        {
            frmRegistro frm = new frmRegistro();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void btnPreg_Click(object sender, EventArgs e)
        {
            frmConfigAdmin frm = new frmConfigAdmin();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmlogin frm = new frmlogin();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            lblBienvenido.Text = "Bienvenido, " + frmlogin.user;
        }
    }
}
