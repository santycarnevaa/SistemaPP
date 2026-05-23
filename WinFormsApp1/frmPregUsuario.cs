using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class frmPregUsuario : Form
    {
        public frmPregUsuario()
        {
            InitializeComponent();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (Resp1.Text == "" || Resp2.Text == "" || Resp3.Text == "" || Resp4.Text == "" || Resp5.Text == "")
            {
                MessageBox.Show("Hay un campo vacio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            frmUsuario frm = new frmUsuario();
            this.Hide();
            frm.ShowDialog();
            this.Close();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmUsuario frm = new frmUsuario();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }
    }
}
