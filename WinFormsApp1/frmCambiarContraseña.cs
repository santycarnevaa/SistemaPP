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
    public partial class frmContraseña : Form
    {
        public frmContraseña()
        {
            InitializeComponent();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Su contraseña ha sido cambiada", "Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void boton() //habilita el boton cuando los dos txt son iguales
        {
            {
                if (txtnueva.Text == txtnueva2.Text)
                {
                    btnSiguiente.Enabled = true;
                }
                else
                {
                    btnSiguiente.Enabled = false;
                }
            }
        }

        private void txtnueva2_TextChanged(object sender, EventArgs e)
        {
            boton();
        }

        private void txtnueva_TextChanged(object sender, EventArgs e)
        {
            boton();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmlogin frm = new frmlogin();
            this.Close();
        }
    }
}
