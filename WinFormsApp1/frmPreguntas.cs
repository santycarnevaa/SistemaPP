using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista
{
    public partial class frmrecuperar : Form
    {
        string usuario;
        public frmrecuperar()
        {
            InitializeComponent();
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

        private void frmrecuperar_Load(object sender, EventArgs e) => ClientSize = new Size(563, 190);
        string Codigo = "12345"; //es el codigo que te manda al gmail
        string Preg1 = "123";
        string Preg2 = "456";
        string Preg3 = "789";
        private void btnSiguiente_Click(object sender, EventArgs e)
        {

            string contraseña = txt1.Text + txt2.Text + txt3.Text + txt4.Text + txt5.Text;

            if (contraseña != Codigo)
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
            ClientSize = new Size(563, 366);
            lblPreg1.Visible = true;
            txtPreg1.Visible = true;
            lblPreg2.Visible = true;
            txtPreg2.Visible = true;
            lblPreg3.Visible = true;
            txtPreg3.Visible = true;
            txt1.Visible = false;
            txt2.Visible = false;
            txt3.Visible = false;
            txt4.Visible = false;
            txt5.Visible = false;
            btnIngresar.Visible = true;
            btnSiguiente.Visible = false;
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
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (txtPreg1.Text == Preg1 && txtPreg2.Text == Preg2 && txtPreg3.Text == Preg3)
            //cambia las variables preg1,2... por las respuestas correctas
            {
                frmContraseña frm = new frmContraseña(usuario);
                this.Hide();
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                //tenes que hacer que las preguntes se randomisen de nuevo(esta que se borre el txt para testear)
                txtPreg1.Text = "";
                txtPreg2.Text = "";
                txtPreg3.Text = "";
                MessageBox.Show("Una o mas respuestas estan incorrectas", "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ingresar()
        {
            if (txtPreg1.Text == "" || txtPreg2.Text == "" || txtPreg3.Text == "")
            {
                btnIngresar.Enabled = false;
            }
            else
            {
                btnIngresar.Enabled = true;
            }


        }

        private void txtPreg1_TextChanged(object sender, EventArgs e)
        {
            ingresar();
        }

        private void txtPreg2_TextChanged(object sender, EventArgs e)
        {
            ingresar();
        }

        private void txtPreg3_TextChanged(object sender, EventArgs e)
        {
            ingresar();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmlogin frm = new frmlogin();
            this.Close();
        }
    }
}