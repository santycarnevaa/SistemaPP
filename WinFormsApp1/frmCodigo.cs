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
    public partial class frmCodigo : Form
    {
        string codigo = "12345"; //es el codigo que te manda al gmail
        public frmCodigo()
        {
            InitializeComponent();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {

            string contraseña = txt1.Text + txt2.Text + txt3.Text + txt4.Text + txt5.Text;

            if (contraseña != codigo)
            {
                MessageBox.Show("Contraseña equivocada", "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt1.Text = "";
                txt2.Text = "";
                txt3.Text = "";
                txt4.Text = "";
                txt5.Text = "";
                txt2.Enabled = false;
                txt3.Enabled = false;
                txt4.Enabled = false;
                txt5.Enabled = false;
                txt1.Focus();
                return;
            }
            frmrecuperar frm = new frmrecuperar();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void txt1_TextChanged(object sender, EventArgs e)
        {
            Boton();
            if (txt1.Text == "")
            {
                txt1.Focus();
            }
            else
            {
                txt2.Enabled = true;
                txt2.Focus();
            }
        }

        private void txt2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)8)
            {
                txt1.Focus();

            }
        }

        private void txt2_TextChanged(object sender, EventArgs e)
        {
            Boton();
            if (txt2.Text == "")
            {
                txt2.Focus();
            }
            else
            {
                txt3.Enabled = true;
                txt3.Focus();
            }
        }

        private void txt3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)8)
            {
                txt2.Focus();

            }
        }

        private void txt3_TextChanged(object sender, EventArgs e)
        {
            Boton();
            if (txt3.Text == "")
            {
                txt3.Focus();
            }
            else
            {
                txt4.Enabled = true;
                txt4.Focus();
            }
        }

        private void txt4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)8)
            {
                txt3.Focus();

            }
        }

        private void txt4_TextChanged(object sender, EventArgs e)
        {
            Boton();
            if (txt4.Text == "")
            {
                txt4.Focus();
            }
            else
            {
                txt5.Enabled = true;
                txt5.Focus();
            }
        }

        private void txt5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)8)
            {
                txt4.Focus();

            }
        }

        private void txt5_TextChanged(object sender, EventArgs e)
        {
            Boton();
            if (txt5.Text == "")
            {
                txt5.Focus();
            }
        }
        private void Boton()
        {
            {
                if (txt1.Text == "" || txt2.Text == "" || txt3.Text == "" || txt4.Text == "" || txt5.Text == "")
                {
                    btnSiguiente.Enabled = false;
                }
                else
                {
                    btnSiguiente.Enabled = true;
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmlogin frm = new frmlogin();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }
    }
}
