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
    public partial class frmConfigAdmin : Form
    {
        public static int caracteres = 8;
        public static int requiereNumeros = 0;
        public static int requiereEspeciales = 0;
        public static int cantidadPreguntas = 3;
        public static int requiere2FA = 0;
        public frmConfigAdmin()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmAdmin frm = new frmAdmin();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCaracteres.Text))
            {
                caracteres = Convert.ToInt32(txtCaracteres.Text);
            }
            else
            {
                caracteres = 8;
                txtCaracteres.Text = "8";
            }

            requiere2FA = ckb2FA.Checked ? 1 : 0;
            int numerosValor = ckbNumeros.Checked ? 1 : 0;
            int especialesValor = ckbCaracteresEsp.Checked ? 1 : 0;
            requiereNumeros = numerosValor;
            requiereEspeciales = especialesValor;

            if (rb1.Checked) cantidadPreguntas = 1;
            else if (rb5.Checked) cantidadPreguntas = 5;
            else cantidadPreguntas = 3;      
        }

        private void frmConfigAdmin_Load(object sender, EventArgs e)
        {
            txtCaracteres.Text = caracteres.ToString();       
            ckbNumeros.Checked = (requiereNumeros == 1);
            ckbCaracteresEsp.Checked = (requiereEspeciales == 1);
            ckb2FA.Checked = (requiere2FA == 1);

            if (cantidadPreguntas == 1) rb1.Checked = true;
            else if (cantidadPreguntas == 5) rb5.Checked = true;
            else rb3.Checked = true;
        }
    }
}
