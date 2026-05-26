using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using CapaLogica;


namespace CapaVista
{
    public partial class frmContraseña : Form
    {
        private CL_servicioContras servicioContras = new CL_servicioContras();
        private string usuario;
        public frmContraseña(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            string nuevaContra = txtNuevaContra.Text;
            string confirmarContra = txtConfirmarContra.Text;

            if (string.IsNullOrWhiteSpace(nuevaContra))
            {
                MessageBox.Show("Ingrese una nueva contraseña.");
                return;
            }

            if (string.IsNullOrWhiteSpace(confirmarContra))
            {
                MessageBox.Show("Confirme la nueva contraseña.");
                return;
            }

            if (nuevaContra.Length < (frmConfigAdmin.caracteres))
            {
                MessageBox.Show("La contraseña es muy corta. El mínimo requerido es de " + frmConfigAdmin.caracteres + " caracteres.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (frmConfigAdmin.requiereNumeros == 1)
            {
                if (!Regex.IsMatch(nuevaContra, @"[0-9]"))
                {
                    MessageBox.Show("La contraseña debe contener al menos un número.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (frmConfigAdmin.requiereEspeciales == 1)
            {
                if (!Regex.IsMatch(nuevaContra, @"[^a-zA-Z0-9]"))
                {
                    MessageBox.Show("La contraseña debe contener al menos un carácter especial (ej: !, @, #, $, %).", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; 
                }
            }

            try
            {
                bool cambioOk = servicioContras.cambiarContra(usuario, nuevaContra);

                if (cambioOk)
                {
                    MessageBox.Show("Contraseña cambiada correctamente.");

                    frmlogin frm = new frmlogin();
                    this.Hide();
                    frm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo cambiar la contraseña. Verifique que cumpla las políticas y que no haya sido usada antes.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error real al cambiar contraseña: " + ex.Message);
            }
        }

        private void boton() //habilita el boton cuando los dos txt son iguales
        {
            {
                if (txtNuevaContra.Text == txtConfirmarContra.Text)
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
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void frmContraseña_Load(object sender, EventArgs e)
        {
            lblCaracteres.Text = "La contraseña debe tener al menos " + frmConfigAdmin.caracteres + " caracteres.";
        
        }
    }
}
