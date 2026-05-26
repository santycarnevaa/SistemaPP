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
        string Preg1 = "asd";
        string Preg2 = "asd";
        string Preg3 = "asd";
        string Preg4 = "asd";
        string Preg5 = "asd";

        public frmrecuperar()
        {
            InitializeComponent();
        }


        private void btnIngresar_Click(object sender, EventArgs e)
        {
            bool todoOk = true;

            if (frmConfigAdmin.cantidadPreguntas >= 1)
            {
                if (txtPreg1.Text != Preg1) todoOk = false;
            }

            if (frmConfigAdmin.cantidadPreguntas == 5)
            {
                if (txtPreg2.Text != Preg2 || txtPreg4.Text != Preg4) todoOk = false;
            }

            if (frmConfigAdmin.cantidadPreguntas >= 3)
            {
                if (txtPreg3.Text != Preg3 || txtPreg5.Text != Preg5) todoOk = false;
            }

            if (todoOk)
            {
                frmContraseña frm = new frmContraseña(usuario);
                this.Hide();
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                txtPreg1.Text = txtPreg2.Text = txtPreg3.Text = txtPreg4.Text = txtPreg5.Text = "";
                MessageBox.Show("Una o más respuestas están incorrectas", "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void ingresar()
        {
            bool habilitar = true;

            if (frmConfigAdmin.cantidadPreguntas >= 1 && string.IsNullOrWhiteSpace(txtPreg1.Text))
                habilitar = false;

            if (frmConfigAdmin.cantidadPreguntas >= 3 && (string.IsNullOrWhiteSpace(txtPreg3.Text) || string.IsNullOrWhiteSpace(txtPreg5.Text)))
                habilitar = false;

            if (frmConfigAdmin.cantidadPreguntas == 5 && (string.IsNullOrWhiteSpace(txtPreg2.Text) || string.IsNullOrWhiteSpace(txtPreg4.Text)))
                habilitar = false;
            btnIngresar.Enabled = true;
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
        private void txtPreg4_TextChanged(object sender, EventArgs e)
        {
            ingresar();
        }
        private void txtPreg5_TextChanged(object sender, EventArgs e)
        {
            ingresar();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmlogin frm = new frmlogin();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void frmrecuperar_Load(object sender, EventArgs e)
        {
            //si es una sola
            lblPreg1.Visible = txtPreg1.Visible = (frmConfigAdmin.cantidadPreguntas >= 1);
            //si son 5 si o si
            lblPreg2.Visible = txtPreg2.Visible = (frmConfigAdmin.cantidadPreguntas == 5);
            lblPreg4.Visible = txtPreg4.Visible = (frmConfigAdmin.cantidadPreguntas == 5);
            //si son 3-5
            lblPreg3.Visible = txtPreg3.Visible = (frmConfigAdmin.cantidadPreguntas >= 3);
            lblPreg5.Visible = txtPreg5.Visible = (frmConfigAdmin.cantidadPreguntas >= 3);
            ingresar();
        }
    }
}