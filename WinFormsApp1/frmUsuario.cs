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

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmlogin frm = new frmlogin();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmConfig frm = new frmConfig(usuario);
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }
    }
}
