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
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmPregUsuario frm = new frmPregUsuario();
            frm.ShowDialog();
            this.Close();
        }

        private void btnContraseña_Click(object sender, EventArgs e)
        {
            frmrecuperar frm = new frmrecuperar();
            frm.ShowDialog();
            this.Close();
        }
    }
}
